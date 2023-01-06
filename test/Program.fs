open FsRope.Rope
open System.Collections
open FsRope

module Program =
    [<EntryPoint>]
    let main _ =  
       let rope = Rope.create "Lorem ipsum\ndolor sit amet,\nconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"
        // delete "\ndolor sit amet,\n"
       let rope = rope.Delete(11, 17)

        // current state of string after above deletion: 
        // "Lorem ipsumconsectetur\nadipiscing elit. \nAenean ornare, \nlacus vitae \ntempor pretium,\nleo nulla\nsollicitudin elit,\nin ultrices mi dui et\nipsum. Cras condimentum\npurus in metus \nsodales tincidunt. Praesent"

        // delete "\nlacus vitae \ntempor pretium,\n"
       let rope = rope.Delete(57, 30)

       printfn "%s" <| rope.Text()
       0