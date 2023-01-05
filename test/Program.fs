open FsRope
open System.Collections
open FsRope.Rope

[<Literal>]
let text =
    "During the development of the .NET Framework, the class libraries were originally written using a managed code compiler system called \"Simple Managed C\" (SMC)."

[<Literal>]
let insText = "TEST!"

let initialTable = Rope.create text

module Program =
    [<EntryPoint>]
    let main _ =  
        let rope = Rope.create "0\n123\n46\n89"
        for i in [0..0] do
            let line = Rope.getLine 0 rope
            printfn "%A" line
        0