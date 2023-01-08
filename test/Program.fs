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

        for i in [0..20] do
            // Generate deletion idx and length
            let idx = idxGen <| Math.Max(testString.Length - 1, 0)
            let remainLength = testString.Length - idx
            let length = lengthGen idx <| Math.Max(remainLength, 0)

            let idx = Math.Min(idx, lorem.Length - 1)
            let idx = Math.Max(idx, 0)

            let length = Math.Min(length, remainLength)
            let length = Math.Max(length, 0)

            printfn "idx, len, strLength = %A" (idx, length, testString.Length)

            // Insert and then assert
            testString <- testString.Remove(idx, length)
            testRope <- testRope.Delete(idx, length)

        0