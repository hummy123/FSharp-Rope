namespace FsRope

open Types
open RopeNode
open System.Globalization

(* Implementation of AA trees, used as the rope's backing structure,
 * guided by following paper: https://arxiv.org/pdf/1412.4882.pdf *)

module Rope =
    let empty = E

    let inline private size node =
        match node with
        | E -> 0
        | T(_, _, v, _) -> v.LeftIdx + v.RightIdx + 1

    let inline private sizeLeft node = 
        match node with 
        | E -> 0
        | T(_, _, v, _) -> v.LeftIdx

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

    let inline private skew node =
        match node with
        | T(lvx, T(lvy, a, ky, b), kx, c) when lvx = lvy -> 
            let innerVal = kx.SetIndex ky.RightIdx kx.RightIdx
            let innerNode =  T(lvx, b, innerVal, c)
            let outerVal = ky.SetRight (size innerNode)
            T(lvx, a, outerVal, innerNode)
        | t -> t

    let inline private split node =
        match node with
        | T(lvx, a, kx, T(lvy, b, ky, T(lvz, c, kz, d))) when lvx = lvy && lvy = lvz -> 
            let right = T(lvx, c, kz, d)
            let leftVal = kx.SetRight ky.LeftIdx
            let left = T(lvx, a, leftVal, b)
            let outerVal = ky.SetIndex (size left) (size right)
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
            let leftVal = kl.SetRight kb.LeftIdx
            let leftNode = T(lv1, a, leftVal, lb)
            let rightVal = kt.SetIndex kb.RightIdx kt.RightIdx
            let rightNode = T(lvt - 1, rb, rightVal, rt)
            let outerVal = kb.SetIndex (size leftNode) (size rightNode)
            T(lvb + 1, leftNode, outerVal, rightNode)
        | T(lvt, lt, kt, rt) when lvl rt < lvt -> 
            T(lvt - 1, lt, kt, rt) |> split
        | T(lvt, lt, kt, T(lvr, (T(lva, c, ka, d) as a), kr, b)) ->
            let leftVal = kt.SetRight ka.LeftIdx
            let leftNode = T(lvt - 1, lt, leftVal, c)
            let rightVal = kr.SetLeft ka.RightIdx
            let rightNode = T(nlvl a, d, rightVal, b) |> split
            let outerVal = ka.SetIndex (size leftNode) (size rightNode)
            T(lva + 1, leftNode, outerVal, rightNode)
        | _ -> failwith "unexpected adjust case"

    let rec private splitMax =
        function
        | T(_, l, v, E) -> (l, v)
        | T(h, l, v, r) as node -> let (r', b) = splitMax r in adjust <| node, b
        | _ -> failwith "unexpected dellrg case"

    let rec private foldOpt (f: OptimizedClosures.FSharpFunc<_, _, _>) x t =
        match t with
        | E -> x
        | T(_, l, v, r) ->
            let x = foldOpt f x l
            let x = f.Invoke(x, v)
            foldOpt f x r

    let fold f x t =
        foldOpt (OptimizedClosures.FSharpFunc<_, _, _>.Adapt (f)) x t

    let rec private insMin chr node =
        match node with
        | E -> T(1, E, RopeNode.create chr, E)
        | T(h, l, v, r) -> T(h, insMin chr l, v.IncrLeft(), r) |> skew |> split

    let rec private insMax chr =
        function
        | E -> T(1, E, RopeNode.create chr, E)
        | T(h, l, v, r) -> T(h, l, v.IncrRight(), insMax chr r) |> skew |> split

    (* Insert a char array. Private because a consumer would likely want to insert a string. *)
    let private insertChr insIndex chr rope =
        let rec ins curIndex node =
            match node with
            | E -> T(1, E, RopeNode.create chr, E)
            | T(h, l, v, r) ->
                if insIndex > curIndex then
                    T(h, l, v.IncrRight(), ins (curIndex + 1) r) |> skew |> split
                elif insIndex < curIndex then
                    T(h, ins (curIndex - 2) l, v.IncrLeft(), r) |> skew |> split
                else
                    (* We want to insert at the same index as this node. *)
                    let l = insMax chr l
                    T(h, l, v.IncrLeft(), r) |> skew |> split

        ins (sizeLeft rope) rope

    /// Inserts a string into a rope.
    let insert insIndex (str: string) rope =
        let enumerator = StringInfo.GetTextElementEnumerator(str)
        let rec ins idxAcc ropeAcc = 
            if enumerator.MoveNext() then
                let cur = enumerator.GetTextElement().ToCharArray()
                let rope = insertChr idxAcc cur ropeAcc
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
                then sub (curIndex - 2) l

                if start <= curIndex && finish >= curIndex then 
                    for i in v.Char do
                        acc.Add i

                if finish > curIndex
                then sub (curIndex + 1) r

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
                    then del (curIndex - 2) l
                    else l

                let right =
                    if finish > curIndex
                    then del (curIndex + 1) r
                    else r

                if start <= curIndex && finish >= curIndex then 
                    if left = E
                    then right
                    else 
                        let (newLeft, newVal) = splitMax left
                        T(h, newLeft, newVal, right) |> adjust
                else
                    T(h, left, v.SetIndex (size left) (size right), right) |> adjust

        del (sizeLeft rope) rope

    let text rope = 
        let arr = ResizeArray<char>()
        fold (fun _ node -> 
            for i in node.Char do
                arr.Add i
        ) () rope
        new string(arr.ToArray())

    type Rope with
        member this.Insert(index, str) = insert index str this
        member this.Substring(startIndex, length) = substring startIndex length this
        member this.Delete(startIndex, length) = delete startIndex length this
        member this.Text() = text this