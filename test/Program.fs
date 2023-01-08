open FsRope.Rope
open System.Collections
open FsRope.RopeData
open FsRope
open System
open FsCheck

// Initial data
let lorem = "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"
let initRope = Rope.create lorem
let initString = lorem

// Generators
let charGen = 
    let chars = Gen.sample 100 100 Arb.generate<char> |> Array.ofList
    new string(chars)

let idxGen maxLen = Gen.choose(0, maxLen) |> Gen.sample 1 1 |> List.head
let lengthGen min max = Gen.choose(min, max) |> Gen.sample 1 1 |> List.head

module Program =
    [<EntryPoint>]
    let main _ =  
        let mutable testString = initString
        let mutable testRope = initRope

        for i in [0..65535] do
            // Generate inputs
            let insStr = charGen
            let idx = idxGen testString.Length
            // Insert and then assert
            testString <- testString.Insert(idx, insStr)
            testRope <- testRope.Insert(idx, insStr)
        0