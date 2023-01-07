open FsRope.Rope
open System.Collections
open FsRope

module Program =
    [<EntryPoint>]
    let main _ =  
        let rope = Rope.create (String.replicate (65535 * 100) "a")
        0