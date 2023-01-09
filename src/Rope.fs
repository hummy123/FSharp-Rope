namespace FsRope

open Types
open RopeNode
open RopeData
open AaTree
open RopeTree
open System.Globalization
 
 (* This module defines an interface type that exposes an API making the Rope easy to use. *)

 /// A rope is a data structure that allows manipulating text in O(log n) time.
module Rope =
    /// Returns a Rope with the string inserted.
    let insert insIndex (str: string) rope =
        if str <> ""
        then
            let enumerator = StringInfo.GetTextElementEnumerator(str)
            let rec ins idxAcc ropeAcc = 
                if enumerator.MoveNext() then
                    let cur = enumerator.GetTextElement()
                    let line =
                            if cur.Contains("\n") || cur.Contains("\r")
                            then HasLine
                            else HasNoLine
                    let rope = insertChr idxAcc cur line ropeAcc
                    ins (idxAcc + 1) rope
                else
                    ropeAcc
            let tree = ins insIndex rope.Tree
            { Tree = tree; TextLength = size tree; LineCount = lines tree }
        else
            rope

    /// Returns a rope with text in the specified range removed.
    let delete start length rope = 
        let tree = delete start length rope.Tree
        { Tree = tree; TextLength = rope.TextLength - length; LineCount = lines tree }

    /// Returns a substring from the rope.
    let substring start length rope = substring start length rope.Tree

    /// Returns a string containing the text at the specified line.
    let getLine line rope = getLine line rope.Tree

    /// Returns an empty Rope.
    let empty = { Tree = E; TextLength = 0; LineCount = 0; }

    /// Creates a Rope with the specified string.
    let create str = insert 0 str empty

    /// Returns a string containing all text in the rope.
    /// Takes O(n) time. Recommended to use substring or getLine functions
    /// in most cases as they are much more performant.
    let text = text

    type Rope with
        member this.Insert(index, string) = insert index string this
        member this.Delete(startIndex, length) = delete startIndex length this
        member this.Substring(startIndex, length) = substring startIndex length this
        member this.GetLine lineNumber = getLine lineNumber this
        member this.IndicesOf string = indicesOf string this
        member this.Text() = text this
