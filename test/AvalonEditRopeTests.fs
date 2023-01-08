module AvalonRopeTests

open System
open Xunit
open FsRope.Rope
open FsRope

[<Fact>]
let ``Empty Rope`` () =
    let rope = Rope.empty
    Assert.Equal(0, rope.TextLength)
    Assert.Equal("", empty.Text())

[<Fact>]
let ``Empty Rope From String`` () =
    let rope = Rope.create ""
    Assert.Equal(0, rope.TextLength)
    Assert.Equal("", empty.Text())
