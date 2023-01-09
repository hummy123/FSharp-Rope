open FsRope
open FsRope.Rope

module Program =
    [<EntryPoint>]
    let main _ =  
        let mutable rope = Rope.empty
        for i in [0..100] do
            rope <- Rope.insert 0 "hello" rope
        0