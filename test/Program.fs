open FsRope
open FsRope.Rope

module Program =
    [<EntryPoint>]
    let main _ =  
        let mutable rope = Rope.empty
        for i in [0..999999999] do
            rope <- rope.Insert(0, "hello")
        0