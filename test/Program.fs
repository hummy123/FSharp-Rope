open FsRope
open FsRope.Rope
open System.Text

module Program =
    [<EntryPoint>]
    let main _ =  
        let mutable table = Rope.create ""
        let mutable runningStr = ""
        for i in [0..10] do
            let halfLength = runningStr.Length / 2
            table <- table.Insert(halfLength, "hello")
            runningStr <- runningStr.Substring(0,halfLength) + "hello" + runningStr.Substring(halfLength)
            printfn "rope: \t\t%A" <| table.Text()
            printfn "expected: \t%A" <| runningStr
        0