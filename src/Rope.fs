namespace FsRope

open Types
open RopeNode
open System.Globalization

(* Implementation of AA trees, used as the rope's backing structure,
 * guided by following paper: https://arxiv.org/pdf/1412.4882.pdf *)

module Rope =
    let empty = E

    let rec private foldOpt (f: OptimizedClosures.FSharpFunc<_, _, _>) x t =
        match t with
        | E -> x
        | T(_, l, v, r) ->
            let x = foldOpt f x l
            let x = f.Invoke(x, v)
            foldOpt f x r

    let fold f x t =
        foldOpt (OptimizedClosures.FSharpFunc<_, _, _>.Adapt (f)) x t

    let text rope = 
        let arr = ResizeArray<char>()
        fold (fun _ node -> 
            for i in node.Char do
                arr.Add i
        ) () rope
        new string(arr.ToArray())

    let inline private size node =
        match node with
        | E -> 0
        | T(_, _, v, _) -> v.LeftIdx + v.RightIdx + 1

    let inline private idxLnSize node =
        match node with
        | E -> 0, 0
        | T(_, _, v, _) -> v.LeftIdx + v.RightIdx + 1, v.LeftLns + v.RightLns + v.IsLine

    let inline private sizeLeft node = 
        match node with 
        | E -> 0
        | T(_, _, v, _) -> v.LeftIdx

    let inline private sizeRight node = 
        match node with 
        | E -> 0
        | T(_, _, v, _) -> v.RightIdx

    /// O(1): Returns a boolean if tree is empty.
    let inline isEmpty node =
        match node with
        | E -> true
        | _ -> false

    let inline private sngl node =
        match node with
        | E -> false
        | T(_, _, _, E) -> true
        | T(lvx, _, _, T(lvy, _, _, _)) -> lvx > lvy

    let inline private lvl node =
        match node with
        | E -> 0
        | T(lvt, _, _, _) -> lvt

    let private skew node =
        match node with
        | T(lvx, T(lvy, a, ky, b), kx, c) as t when lvx = lvy ->
            let innerVal = kx.SetData (idxLnSize b) (idxLnSize c)
            let innerNode =  T(lvx, b, innerVal, c)
            let outerVal = ky.SetData (idxLnSize a) (idxLnSize innerNode)
            T(lvx, a, outerVal, innerNode)
        | t -> t

    let inline private split node =
        match node with
        | T(lvx, a, kx, T(lvy, b, ky, T(lvz, c, kz, d))) as t when lvx = lvy && lvy = lvz -> 
            let right = T(lvx, c, kz, d)
            let leftVal = kx.SetData (idxLnSize a) (idxLnSize b)
            let left = T(lvx, a, leftVal, b)
            let outerVal = ky.SetData (idxLnSize left) (idxLnSize right)
            T(lvx + 1, left, outerVal, right)
        | t -> t

    let private nlvl =
        function
        | T(lvt, _, _, _) as t -> if sngl t then lvt else lvt + 1
        | _ -> failwith "unexpected nlvl case"

    let inline private adjust node =
        match node with
        | T(lvt, lt, kt, rt) as t when lvl lt >= lvt - 1 && lvl rt >= (lvt - 1) -> 
            t
        | T(lvt, lt, kt, rt) when lvl rt < lvt - 1 && sngl lt -> 
            T(lvt - 1, lt, kt, rt) |> skew
        | T(lvt, T(lv1, a, kl, T(lvb, lb, kb, rb)), kt, rt) when lvl rt < lvt - 1 -> 
            let leftVal = kl.SetData (idxLnSize a) (idxLnSize lb)
            let leftNode = T(lv1, a, leftVal, lb)
            let rightVal = kt.SetData (idxLnSize rb) (idxLnSize rt)
            let rightNode = T(lvt - 1, rb, rightVal, rt)
            let outerVal = kb.SetData (idxLnSize leftNode) (idxLnSize rightNode)
            T(lvb + 1, leftNode, outerVal, rightNode)
        | T(lvt, lt, kt, rt) when lvl rt < lvt -> 
            T(lvt - 1, lt, kt, rt) |> split
        | T(lvt, lt, kt, T(lvr, (T(lva, c, ka, d) as a), kr, b)) ->
            let leftVal = kt.SetData (idxLnSize lt) (idxLnSize c)
            let leftNode = T(lvt - 1, lt, leftVal, c)
            let rightVal = kr.SetData (idxLnSize d) (idxLnSize b)
            let rightNode = T(nlvl a, d, rightVal, b) |> split
            let outerVal = ka.SetData (idxLnSize leftNode) (idxLnSize rightNode)
            T(lva + 1, leftNode, outerVal, rightNode)
        | _ -> failwith "unexpected adjust case"

    let rec private splitMax =
        function
        | T(_, l, v, E) -> (l, v.SetIndex (size l) 0)
        | T(h, l, v, r) -> 
            let (r', b) = splitMax r
            in adjust <| T(h, l, v, r'), b
        | _ -> failwith "unexpected dellrg case"

    let rec private insMin chr line node =
        match node with
        | E -> T(1, E, RopeNode.create chr line, E)
        | T(h, l, v, r) -> T(h, insMin chr line l, v.PlusLeft line, r) |> skew |> split

    let rec private insMax chr line =
        function
        | E -> T(1, E, RopeNode.create chr line, E)
        | T(h, l, v, r) -> T(h, l, v.PlusRight line, insMax chr line r)

    (* Insert a char array. Private because a consumer would likely want to insert a string. *)
    let private insertChr insIndex chr line rope =
        let rec ins curIndex node =
            match node with
            | E -> T(1, E, RopeNode.create chr line, E)
            | T(h, l, v, r) ->
                if insIndex > curIndex then
                    let nextIndex = curIndex + 1 + sizeLeft r
                    T(h, l, v.PlusRight line, ins nextIndex r) |> skew |> split
                elif insIndex < curIndex then
                    let nextIndex = curIndex - 1 - sizeRight l
                    T(h, ins nextIndex l, v.PlusLeft line, r) |> skew |> split
                else
                    (* We want to insert at the same index as this node. *)
                    let newLeft = insMax chr line l
                    T(h, newLeft, v.PlusLeft line, r) |> skew |> split

        ins (sizeLeft rope) rope

    /// Inserts a string into a rope.
    let insert insIndex (str: string) rope =
        let enumerator = StringInfo.GetTextElementEnumerator(str)
        let rec ins idxAcc ropeAcc = 
            if enumerator.MoveNext() then
                let cur = enumerator.GetTextElement()
                let line =
                    if cur.Contains("\n") || cur.Contains("\r")
                    then HasLine
                    else HasNoLine
                let cur = cur.ToCharArray()
                let rope = insertChr idxAcc cur line ropeAcc
                ins (idxAcc + 1) rope
            else
                ropeAcc
        ins insIndex rope

    let create str = insert 0 str E

    /// Returns a substring from the rope at the given start index and length.
    let substring (start: int) (length: int) rope =
        let finish = start + length
        let acc = ResizeArray<char>() (* Using mutable array for performance. *)
        
        let rec sub curIndex node =
            match node with
            | E -> ()
            | T(h, l, v, r) ->
                if start < curIndex
                then sub (curIndex - 1 - sizeRight l) l

                if start <= curIndex && finish > curIndex then 
                    for i in v.Char do
                        acc.Add i

                if finish > curIndex
                then sub (curIndex + 1 + sizeLeft r) r

        sub (sizeLeft rope) rope
        new string(acc.ToArray())

    /// Deletes a range of characters from the rope at the given start index and length.
    let delete (start: int) (length: int) rope =
        let finish = start + length

        let rec del curIndex node =
            match node with
            | E as empty -> empty
            | T(h, l, v, r) ->
                let left = 
                    if start < curIndex
                    then del (curIndex - 1 - sizeRight l) l
                    else l

                let right =
                    if finish > curIndex
                    then del (curIndex + 1 + sizeLeft r) r
                    else r

                if start <= curIndex && finish > curIndex then 
                    if left = E
                    then right
                    else 
                        let (newLeft, newVal) = splitMax left
                        let newVal = newVal.SetData (idxLnSize newLeft) (idxLnSize right)
                        T(h, newLeft, newVal, right) |> adjust
                else
                    T(h, left, v.SetData (idxLnSize left) (idxLnSize right), right) |> adjust

        del (sizeLeft rope) rope

    type Rope with
        member this.Insert(index, str) = insert index str this
        member this.Substring(startIndex, length) = substring startIndex length this
        member this.Delete(startIndex, length) = delete startIndex length this
        member this.Text() = text this