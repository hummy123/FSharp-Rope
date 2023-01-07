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

[<Fact>]
let ``Rope.GetLine returns correct segments when we delete multiple line breaks at last half`` () =
    let rope = Rope.create "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"

    // delete "\npurus in metus \n"
    let rope = rope.Delete(177, 17)
    Assert.Equal("ipsum. Cras condimentumsodales tincidunt. Praesent", rope.GetLine 10)

    // delete " elit,\nin "
    let rope = rope.Delete(123, 9)
    Assert.Equal("sollicitudin ultrices mi dui et\n", rope.GetLine 8)

    // delete "\ntempor pretium,\nleo nulla"
    let rope = rope.Delete(83, 25)

    // assert that all lines are as expected
    Assert.Equal("Lorem ipsum\n", rope.GetLine 0)
    Assert.Equal("dolor sit amet,\n", rope.GetLine 1)
    Assert.Equal("consectetur\n", rope.GetLine 2)
    Assert.Equal("adipiscing elit. \n", rope.GetLine 3)
    Assert.Equal("Aenean ornare, \n", rope.GetLine 4)
    Assert.Equal("lacus vitulla\n", rope.GetLine 5)
    Assert.Equal("sollicitudin ultrices mi dui et\n", rope.GetLine 6)
    Assert.Equal("ipsum. Cras condimentumsodales tincidunt. Praesent", rope.GetLine 7)

[<Fact>]
let ``Rope.GetLine returns correct segments when we delete multiple line breaks in middle`` () =
    let rope = Rope.create "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"

    // delete "Aenean ornare, \n"
    let rope = rope.Delete(58, 16)
    // delete "tempor pretium,\nleo nulla\n"
    let rope = rope.Delete(69, 15)
    //delete "dolor sit amet,\n"
    let rope = rope.Delete(12, 16)
    
    // test we retrieve all lines as expected
    Assert.Equal("Lorem ipsum\n", rope.GetLine 0)
    Assert.Equal("consectetur\n", rope.GetLine 1)
    Assert.Equal("adipiscing elit. \n", rope.GetLine 2)
    Assert.Equal("lacus vitium,\n", rope.GetLine 3)
    Assert.Equal("leo nulla\n", rope.GetLine 4)
    Assert.Equal("sollicitudin elit,\n", rope.GetLine 5)
    Assert.Equal("in ultrices mi dui et\n", rope.GetLine 6)
    Assert.Equal("ipsum. Cras condimentum\n", rope.GetLine 7)
    Assert.Equal("purus in metus \n", rope.GetLine 8)
    Assert.Equal("sodales tincidunt. Praesent", rope.GetLine 9)