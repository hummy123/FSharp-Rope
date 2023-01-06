open FsRope.Rope
open System.Collections
open FsRope

module Program =
    [<EntryPoint>]
    let main _ =  
        let rope = Rope.create "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"
        let rope = rope.Delete(177, 17)
        let rope = rope.Delete(123, 9)
        let rope = rope.Delete(83, 25)

        printfn "%s" <| rope.Text()
        0