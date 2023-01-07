open FsRope.Rope
open System.Collections
open FsRope

module Program =
    [<EntryPoint>]
    let main _ =  
        let rope = Rope.create "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"


        let rope = rope.Delete(58, 16)
        // delete "tempor pretium,\nleo nulla\n"
        let rope = rope.Delete(69, 15)
        //delete "dolor sit amet,\n"
        let rope = rope.Delete(12, 16)
        printfn "%s" <| rope.Text()
        0