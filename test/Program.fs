open FsRope
open FsRope.Rope
open System.Text
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

        let inputList = [
            (79, "0")
            (90, "1")
            (77, "2")
            (170, "3")
            (196, "4")
            (143, "5")
            (20, "6")
            (93, "7")
            (32, "8")
            (80, "9")
            (122, "10")
            (32, "11") // <- mismatch occurs here
        ]

        for (idx, str) in inputList do
            testString <- testString.Insert(idx, str)
            testRope <- testRope.Insert(idx, str)

        printfn "are same? %A" <| (testRope.Text() = testString)
        0