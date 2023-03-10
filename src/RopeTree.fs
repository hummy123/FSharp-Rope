namespace FsRope

open Types
open RopeNode
open RopeData
open AaTree
open System.Globalization
 
 (* This module defines the core algorithms for operating on the rope. *)
module internal RopeTree =
    let rec internal insMax chr line node cont =
        match node with
        | E -> T(1, E, RopeNode.create chr line, E) |> cont
        | T(h, l, v, r) -> 
            insMax chr line r (fun r' ->
                T(h, l, v.PlusRight line, r') |> skew |> split |> cont
            )

    /// Used for CPS.
    let inline topLevelCont t = t
    
    /// Inserts a char array into the rope at the specified index.
    let inline insertChr insIndex chr line rope =
        let rec ins curIndex node cont =
            match node with
            | E -> T(1, E, RopeNode.create chr line, E) |> cont
            | T(h, l, v, r) when insIndex < curIndex ->
                ins (curIndex - 1 - sizeRight l) l (fun l' ->
                    T(h, l', v.PlusLeft line, r) 
                    |> skew |> split |> cont
                )
            | T(h, l, v, r) when insIndex > curIndex ->
                ins (curIndex + 1 + sizeLeft r) r (fun r' -> 
                    T(h, l, v.PlusRight line, r') 
                    |> skew |> split |> cont
                )
            | T(h, l, v, r) ->
                (* We want to insert at the same index as this node. *)
                T(h, insMax chr line l topLevelCont, v.PlusLeft line, r) 
                |> skew |> split |> cont

        ins (sizeLeft rope) rope topLevelCont

    /// Deletes a range of characters from the rope at the given start index and length.
    let delete (start: int) (length: int) rope =
        let finish = start + length

        let rec del curIndex delLines node =
            match node with
            | E as empty -> empty, delLines
            | T(h, l, v, r) ->
                let (left, leftDel) = 
                    if start < curIndex
                    then del (curIndex - 1 - sizeRight l) delLines l
                    else l, 0

                let (right, rightDel) =
                    if finish > curIndex
                    then del (curIndex + 1 + sizeLeft r) delLines r
                    else r, 0

                if start <= curIndex && finish > curIndex then
                    let delLines = delLines + v.Lines
                    if left = E
                    then right, delLines
                    else 
                        let (newLeft, newVal, _) = splitMax left
                        let newVal = 
                            { newVal with 
                                LeftLns = lines newLeft;
                                LeftIdx = size newLeft;
                                RightIdx = size right;
                                RightLns = lines right; }
                        T(h, newLeft, newVal, right) |> adjust, delLines + leftDel + rightDel
                else
                    T(h, left, v.SetData (idxLnSize left) (idxLnSize right), right) |> adjust, leftDel + rightDel + delLines

        let (tree, _) = del (sizeLeft rope) 0 rope
        tree

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

    /// Returns a string containing the text at the specified line.
    let getLine (line: int) rope =
        let acc = ResizeArray<char>()
        let rec lin curLine node =
            match node with
            | E -> ()
            | T(h, l, v, r) ->
                if line <= curLine
                then lin (curLine - lineLength l - linesRight l) l

                if line = curLine then 
                    for i in v.Char do
                        acc.Add i

                if line >= curLine
                then lin (curLine + v.Lines + linesLeft r) r

        lin (linesLeft rope) rope
        new string(acc.ToArray())

    /// Returns a .NET List containing the indices of the given string.
    /// If the given string wasn't found, returns an empty list.
    let indicesOf string rope =
        let startChar = StringInfo.GetNextTextElement(string)
        let strLength = StringInfo(string).LengthInTextElements
        let acc = ResizeArray<int>()

        fold (fun pos node -> 
            if node.Char = startChar && substring pos strLength rope.Tree = string then
                acc.Add pos
            pos + 1
        ) 0 rope.Tree |> ignore
        
        acc

    /// Returns a string containing all of the rope's text in O(n) time.
    /// Used exclusively for testing. 
    /// More performant queries can be made with the getLine and substring functions.
    let text rope = 
        let arr = ResizeArray<char>()
        fold (fun _ node -> 
            for i in node.Char do
                arr.Add i
        ) () rope.Tree
        new string(arr.ToArray())
