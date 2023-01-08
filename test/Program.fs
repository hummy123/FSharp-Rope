open FsRope.Rope
open System.Collections
open FsRope.RopeData
open FsRope
open System
open FsCheck

let lorem = "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"
let initRope = Rope.create lorem
let initString = lorem

module Program =
    [<EntryPoint>]
    let main _ =  
        let mutable rope = initRope
        let mutable str = initString
        for i in [0..lorem.Length - 1] do
            rope <- rope.Insert(i, "a")
            str <- initString.Insert(i, "a")
            printfn "\n\n\nloop"
            printfn "str:\n%s\n" str
            printfn "rope:\n%s\n" <| rope.Text()

            if not (str = rope.Text())
            then failwith "error"
        0