module RopeeGetLineTests

open System
open Xunit
open FsRope
open FsRope.Rope

[<Fact>]
let ``Rope.GetLine returns line we inserted`` () =
    let rope = Rope.create "0\n1\n2\n3\n4\n5\n6\n7\n8\n9\n"
    for i in [0..9] do
        let line = Rope.getLine i rope
        Assert.Equal(sprintf "%i\n" i, line)