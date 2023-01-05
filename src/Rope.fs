namespace FsRope

open Types
open RopeNode
open RopeData
open AaTree
open System.Globalization
 
 (* This module defines the core algorithms for operating on the rope. *)

 /// A rope is a data structure that allows manipulating text in O(log n) time.
module Rope =
    let empty = E

    let text rope = 
        let arr = ResizeArray<char>()
        fold (fun _ node -> 
            for i in node.Char do
                arr.Add i
        ) () rope
        new string(arr.ToArray())

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

    let getLine (line: int) rope =
        let acc = ResizeArray<char>()
        let rec lin curLine node =
            match node with
            | E -> ()
            | T(h, l, v, r) ->
                if line < curLine
                then lin (curLine - lineLength l - linesRight l) l

                if line = curLine then 
                    for i in v.Char do
                        acc.Add i

                if curLine > line
                then lin (curLine + lineLength r + linesLeft r) r

        lin (linesLeft rope) rope
        new string(acc.ToArray())

    type Rope with
        member this.Insert(index, str) = insert index str this
        member this.Substring(startIndex, length) = substring startIndex length this
        member this.Delete(startIndex, length) = delete startIndex length this
        member this.Text() = text this
        member this.GetLine line = getLine line this