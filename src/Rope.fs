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
            (* Explicit mutability and a while-loop is faster than 
             * tail recursion in my tests for some reason. *)
            let mutable tree = rope.Tree
            let mutable idx = insIndex
            let mutable line = HasNoLine
            let mutable cur = ""
            let enumerator = StringInfo.GetTextElementEnumerator(str)

            while enumerator.MoveNext() do
                cur <- enumerator.GetTextElement()
                line <-
                        if cur.Contains("\n") || cur.Contains("\r")
                        then HasLine
                        else HasNoLine
                tree <- insertChr idx cur line tree
                idx <- idx + 1

            let (totalTextLen, totalLineLen) = idxLnSize tree
            { Tree = tree; TextLength = totalTextLen; LineCount = totalLineLen }
        else
            rope

    /// Returns a rope with the given string.
    let append str rope = 
        if str <> ""
            then
                (* Explicit mutability and a while-loop is faster than 
                 * tail recursion in my tests for some reason. *)
                let mutable tree = rope.Tree
                let mutable line = HasNoLine
                let mutable cur = ""
                let enumerator = StringInfo.GetTextElementEnumerator(str)

                while enumerator.MoveNext() do
                    cur <- enumerator.GetTextElement()
                    line <-
                            if cur.Contains("\n") || cur.Contains("\r")
                            then HasLine
                            else HasNoLine
                    tree <- insMax cur line tree

                let (totalTextLen, totalLineLen) = idxLnSize tree
                { Tree = tree; TextLength = totalTextLen; LineCount = totalLineLen }
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
    let create str = append str empty

    /// Returns a string containing all text in the rope.
    /// Takes O(n) time. Recommended to use substring or getLine functions
    /// in most cases as they are much more performant.
    let text = text

    type Rope with
        member this.Insert(index, string) = insert index string this
        member this.Apeend(string) = append string this
        member this.Delete(startIndex, length) = delete startIndex length this
        member this.Substring(startIndex, length) = substring startIndex length this
        member this.GetLine lineNumber = getLine lineNumber this
        member this.IndicesOf string = indicesOf string this
        member this.Text() = text this
