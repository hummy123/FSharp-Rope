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

        (*
        
        Minimal mismatch case found:

        input: (79, "0")
        input: (90, "1")
        input: (77, "2")
        input: (170, "3")
        input: (196, "4")
        input: (143, "5")
        input: (20, "6")
        input: (93, "7")
        input: (32, "8")
        input: (80, "9")
        input: (122, "10")
        input: (32, "11") <-- error after this
        
        *)

        for i in [0..20] do
            // Generate inputs
            let insStr = i.ToString()
            let idx = idxGen testString.Length

            printfn "input: %A" (idx, insStr)

            let tempStr = testString.Insert(idx, insStr)
            let tempRope = testRope.Insert(idx, insStr)
            if tempRope.Text() <> tempStr then
                printfn "\n\n\n**MISMATCH DETECTED**"
                printfn "prev matching: \n%A\n" testString
                printfn "string: \n%A\n" tempStr
                printfn "rope: \n%A\n" <| tempRope.Text()
                let tempStr = testString.Insert(idx, insStr)
                let tempRope = testRope.Insert(idx, insStr)
                failwith ""

            // Insert and then assert
            testString <- testString.Insert(idx, insStr)
            testRope <- testRope.Insert(idx, insStr)

        printfn "are same? %A" <| (testRope.Text() = testString)
        0