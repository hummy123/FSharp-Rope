open FsRope
open FsRope.Rope
open System.Text

module Program =
    [<EntryPoint>]
    let main _ =  
        let b = StringBuilder()
        let mutable rope = Rope.empty
        let mutable middle = 0
        for i in [1..7] do
            b.Insert(middle, i.ToString("d3")) |> ignore
            rope <- rope.Insert(middle, i.ToString("d3"))

            middle <- middle + 3
            b.Insert(middle, " ") |> ignore
            rope <- rope.Insert(middle, " ")

            middle <- middle + 1
            rope <- rope.Insert(middle, (999 - i).ToString("d3"))
            rope <- rope.Insert(middle + 3, " ")
            b.Insert(middle, (999 - i).ToString("d3")) |> ignore
            b.Insert(middle + 3, " ") |> ignore

        printfn "rope: %A" <| rope.Text()
        printfn "text: %A" <| b.ToString()
        0