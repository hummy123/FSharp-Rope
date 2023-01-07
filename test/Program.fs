open FsRope.Rope
open System.Collections
open FsRope

module Program =
    [<EntryPoint>]
    let main _ =  
        let rope = Rope.create "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"
        // minimal case where Rope.getline 1 returns a "Lorem ipsum" instead of "Loem ipsum\n"
        let rope = rope.Delete(12, 4)
        printfn "%s" <| rope.Text()
        0