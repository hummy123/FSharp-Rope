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

[<Fact>]
let ``Rope.GetLine returns whole string when we delete line break at middle.`` () =
    let rope = Rope.create "abcde\nfghij"
    Assert.Equal("abcde\n", rope.GetLine 0)
    Assert.Equal("fghij", rope.GetLine 1)

    let rope = rope.Delete(5, 1)
    Assert.Equal("abcdefghij", rope.GetLine 0)

[<Fact>]
let ``Rope.GetLine returns correct segments when we delete line breaks in complex string`` () =
    let rope = Rope.create "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"
    
    // Delete first line break and see if we can get expected string from result.
    let rope = rope.Delete(11, 1)
    Assert.Equal("Lorem ipsumdolor sit amet,\n", rope.GetLine 0)
    Assert.Equal("consectetur\n", rope.GetLine 1)

    // Delete third line break.
    let rope = rope.Delete(38, 1)
    Assert.Equal("Lorem ipsumdolor sit amet,\n", rope.GetLine 0)
    Assert.Equal("consecteturadipiscing elit. \n", rope.GetLine 1)

    // Delete fifth line break
    let rope = rope.Delete(71, 1)
    Assert.Equal("Lorem ipsumdolor sit amet,\n", rope.GetLine 0)
    Assert.Equal("consecteturadipiscing elit. \n", rope.GetLine 1)
    Assert.Equal("Aenean ornare, lacus vitae \n", rope.GetLine 2)
    
    // The above tests are enough to give me confidence in the GetLine function. 


[<Fact>]
let ``Rope.GetLine returns correct segments when we delete multiple line breaks at first half`` () =
    let rope = Rope.create "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"
    // delete "\ndolor sit amet,\n"
    let rope = rope.Delete(11, 17)
    Assert.Equal("Lorem ipsumconsectetur\n", rope.GetLine 0)
    Assert.Equal("adipiscing elit. \n", rope.GetLine 1)

    // current state of string after above deletion: 
    // "Lorem ipsumconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"

    // delete "\nlacus vitae \ntempor pretium,\n"
    let rope = rope.Delete(57, 30)
    // previos assertions to check they still work
    Assert.Equal("Lorem ipsumconsectetur\n", rope.GetLine 0)
    Assert.Equal("adipiscing elit. \n", rope.GetLine 1)

    // current assertion
    Assert.Equal("Aenean ornare, leo nulla\n", rope.GetLine 2)

    // assertions for lines after to check they still work as expected
    Assert.Equal("sollicitudin elit,\n", rope.GetLine 3)
    Assert.Equal("in ultrices mi dui et\n", rope.GetLine 4)
    Assert.Equal("ipsum. Cras condimentum\n", rope.GetLine 5)
    Assert.Equal("purus in metus \n", rope.GetLine 6)
    Assert.Equal("sodales tincidunt. Praesent", rope.GetLine 7)