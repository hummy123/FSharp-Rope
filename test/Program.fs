open FsRope.Rope
open System.Collections
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

        let idx, length = (52, 117)
        testString <- testString.Remove(idx, length)
        testRope <- testRope.Delete(idx, length)
        printfn "first pass: %A" (testString = testRope.Text())

        let idx, length = (45, 50)
        testString <- testString.Remove(idx, length)
        testRope <- testRope.Delete(idx, length)
        printfn "sec pass: %A" (testString = testRope.Text())

        0