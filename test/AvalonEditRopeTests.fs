module AvalonRopeTests

open System
open Xunit
open FsRope.Rope
open FsRope
open System.IO
open System.Text

[<Fact>]
let ``Empty Rope`` () =
    let rope = Rope.empty
    Assert.Equal(0, rope.TextLength)
    Assert.Equal("", empty.Text())

[<Fact>]
let ``Empty Rope from String`` () =
    let rope = Rope.create ""
    Assert.Equal(0, rope.TextLength)
    Assert.Equal("", empty.Text())

[<Fact>]
let ``Initialise Rope from Short String`` () =
    let rope = Rope.create "Hello, World"
    Assert.Equal(12, rope.TextLength)
    Assert.Equal("Hello, World", rope.Text())

let buildLongString lines =
    let w = new StringWriter()
    w.NewLine <- "\n"
    for i in [1..lines] do
        w.WriteLine(i.ToString())
    w.ToString()

[<Fact>]
let ``Initialise Rope from Long String`` () =
    let text = buildLongString 1000
    let rope = Rope.create text
    Assert.Equal(text.Length, rope.TextLength)
    Assert.Equal(text, rope.Text())
    
[<Fact>]
let ``Text to String with Parts`` () =
    let text = buildLongString 1000
    let rope = Rope.create text

    let textPart = text.Substring(1200, 600)
    let ropePart = rope.Substring(1200, 600)

    Assert.Equal(textPart, ropePart)

[<Fact>]
let ``Concatenate String to Rope`` () =
    let b = StringBuilder()
    let mutable rope = Rope.empty
    for i in [0..100] do
        b.Append(i.ToString()) |> ignore
        rope <- rope.Insert(i, i.ToString())
        b.Append(" ") |> ignore
        rope <- rope.Insert(i + 1, " ")
    Assert.Equal(b.ToString(), rope.Text())

[<Fact>]
let ``Append Long Text to Empty Rope`` () =
    let text = buildLongString 1000
    let rope = Rope.empty
    let rope = rope.Insert(0, text)
    Assert.Equal(text, rope.Text())

[<Fact>]
let ``Concatenate String to Rope by Insertion at Middle`` () =
    let b = StringBuilder()
    let mutable rope = Rope.empty
    for i in [1..998] do
        b.Append(i.ToString("d3")) |> ignore
        b.Append(" ") |> ignore
    let mutable middle = 0
    for i in [1..499] do
        rope <- rope.Insert(middle, i.ToString("d3"))
        middle <- middle + 3
        rope <- rope.Insert(middle, " ")
        middle <- middle + 1
        rope <- rope.Insert(middle, (999 - i).ToString("d3"))
        rope <- rope.Insert(middle + 3, " ")
    Assert.Equal(b.ToString(), rope.Text());

