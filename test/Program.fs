open FsRope
open FsRope.Rope
open System.Text

module Program =
    [<EntryPoint>]
    let main _ =  
        let b = StringBuilder()
        let mutable rope = Rope.empty
        for i in [0..10] do
            b.Append(i.ToString()) |> ignore
            rope <- rope.Apeend(i.ToString())
            b.Append(" ") |> ignore
            rope <- rope.Apeend(" ")
            printfn "%A" <| b.ToString()
            printfn "%A" <| rope.Text()
        0