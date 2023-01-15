namespace FsRope

open Types
open RopeNode
open RopeData

(* Internal module containing functions used for balancing and operating on an AA tree.
 * Implementation of AA trees, used as the rope's backing structure,
 * guided by following paper. https://arxiv.org/pdf/1412.4882.pdf *)
 module internal AaTree =
    let inline sngl node =
        match node with
        | E -> false
        | T(_, _, _, E) -> true
        | T(lvx, _, _, T(lvy, _, _, _)) -> lvx > lvy

    let inline lvl node =
        match node with
        | E -> 0
        | T(lvt, _, _, _) -> lvt

    let inline skew node =
        match node with
        | T(lvx, T(lvy, a, ky, b), kx, c) as t when lvx = lvy ->
            let innerVal = kx.SetData (idxLnSize b) (idxLnSize c)
            let innerNode =  T(lvx, b, innerVal, c)
            let outerVal = ky.SetData (idxLnSize a) (idxLnSize innerNode)
            T(lvx, a, outerVal, innerNode)
        | t -> t

    let inline split node =
        match node with
        | T(lvx, a, kx, T(lvy, b, ky, T(lvz, c, kz, d))) as t when lvx = lvy && lvy = lvz -> 
            let right = T(lvx, c, kz, d)
            let leftVal = kx.SetData (idxLnSize a) (idxLnSize b)
            let left = T(lvx, a, leftVal, b)
            let outerVal = ky.SetData (idxLnSize left) (idxLnSize right)
            T(lvx + 1, left, outerVal, right)
        | t -> t

    let inline nlvl node =
        match node with
        | T(lvt, _, _, _) as t -> if sngl t then lvt else lvt + 1
        | _ -> failwith "unexpected nlvl case"

    let inline adjust node =
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
        | _ -> node

    let rec splitMax =
        function
        | T(_, l, v, E) -> 
            let v = 
                { v with 
                    LeftIdx = size l; 
                    LeftLns = lines l; 
                    RightIdx = 0; 
                    RightLns = 0; }
            l, v, 0
        | T(h, l, v, r) -> 
            match splitMax r with
            | r', b, lns -> 
                let v' = { v with RightLns = lines r'; RightIdx = size r' }
                let tree = T(h, l, v', r') |> adjust
                let b' = { b with RightLns = lines tree; RightIdx = size tree }
                tree, b', lns
        | _ -> failwith "unexpected splitMax case"

    let rec foldOpt (f: OptimizedClosures.FSharpFunc<_, _, _>) x t =
        match t with
        | E -> x
        | T(_, l, v, r) ->
            let x = foldOpt f x l
            let x = f.Invoke(x, v)
            foldOpt f x r

    let fold f x t =
        foldOpt (OptimizedClosures.FSharpFunc<_, _, _>.Adapt (f)) x t
