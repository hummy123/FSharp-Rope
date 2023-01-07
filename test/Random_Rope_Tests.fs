module RandomRopeTests

open System
open Xunit
open FsCheck
open FsCheck.Xunit
open FsRope
open FsRope.Rope

// Initial data
let lorem = "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"
let initRope = Rope.create lorem
let initString = lorem

// Generators
let charGen = Gen.sample 100 100 Arb.generate<char> |> Array.ofList
let idxGen maxLen = Gen.choose(0, maxLen) |> Gen.sample 1 1 |> List.head
let lengthGen min max = Gen.choose(min, max) |> Gen.sample 1 1 |> List.head

// Property tests
[<Property>]
let ``String and rope return same text after a series of inputs`` () =
    let mutable testString = initString
    let mutable testRope = initRope
    for i in [0..100] do
        let insStr = new string(charGen)
        let idx = idxGen insStr.Length
        testString <- testString.Insert(idx, insStr)
        testRope <- testRope.Insert(idx, insStr)
        Assert.Equal(testString, testRope.Text())

[<Property>]
let ``String and rope return same substring after a series of inputs`` () =
    let mutable testString = initString
    let mutable testRope = initRope
    for i in [0..100] do
        // Generate inputs
        let insStr = new string(charGen)
        let idx = idxGen testString.Length
        testString <- testString.Insert(idx, insStr)
        testRope <- testRope.Insert(idx, insStr)

        // Now generate substring ranges
        let startIdx = idxGen testString.Length
        let length = lengthGen 0 (testString.Length - startIdx)

        // Get substrings
        let strSub = testString.Substring(startIdx, length)
        let ropeSub = testRope.Substring(startIdx, length)

        Assert.Equal(strSub, ropeSub)