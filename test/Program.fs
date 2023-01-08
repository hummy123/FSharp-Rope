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
        let str = "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"
        let rope = Rope.create str


        // delete "Aenean ornare, \n"
        let rope = rope.Delete(58, 16)
        let str = str.Remove(58, 16)

        // delete "tempor pretium,\nleo nulla\n"
        let rope = rope.Delete(71, 26)
        let str = str.Remove(71, 26)

        //delete "dolor sit amet,\n"
        let rope = rope.Delete(12, 16)
        let str = str.Remove(12, 16)
        printfn "%A" str
        0