namespace FsRopeBenchmarks

open FsRope
open FsRope.Rope

open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open BenchmarkDotNet.Jobs


[<MemoryDiagnoser; HtmlExporter; MarkdownExporter>]
type CreateDocument() =
    [<Params(100, 1_000, 10_000)>]
    member val stringLength = 0 with get, set

    [<Benchmark>]
    member this.CreateRopeOfSize() = 
        let str = (String.replicate this.stringLength "a")
        Rope.create str


[<MemoryDiagnoser; HtmlExporter; MarkdownExporter>]
type InsertIntoDocument() =
    [<Params(100, 1000, 10_000)>]
    member val insertTimes = 0 with get, set

    member val rope = Rope.empty with get, set
    member val docLength = 0 with get, set

    [<IterationSetup>]
    member this.CreateDocument() =
        this.rope <- Rope.empty
        this.docLength <- 0
        for i in [0..this.insertTimes] do
            this.rope <- this.rope.Insert(0, "hello")
            this.docLength <- this.docLength + 5

    [<Benchmark; InvocationCount(1000)>]
    member this.InsertIntoRopeAtStart() = 
        this.rope.Insert(0, "A")

    [<Benchmark; InvocationCount(1000)>]
    member this.InsertIntoRopeAtMiddle() =
        this.rope.Insert(this.docLength / 2, "A")

    [<Benchmark; InvocationCount(1000)>]
    member this.InsertIntoRopeAtEnd() = 
        this.rope.Insert(this.docLength, "A")

[<MemoryDiagnoser; HtmlExporter; MarkdownExporter>]
type DeleteFromDocument() =
    [<Params(100, 1000, 10_000)>]
    member val insertTimes = 0 with get, set

    member val rope = Rope.empty with get, set
    member val docLength = 0 with get, set

    [<IterationSetup>]
    member this.CreateDocument() =
        this.rope <- Rope.empty
        this.docLength <- 0
        for i in [0..this.insertTimes] do
            this.rope <- this.rope.Insert(0, "hello")
            this.docLength <- this.docLength + 5

    [<Benchmark; InvocationCount(1000)>]
    member this.DeleteFromStartOfrope() = 
        this.rope.Delete(0, 10)

    [<Benchmark; InvocationCount(1000)>]
    member this.DeleteFromMiddleOfrope() =
        this.rope.Delete(this.docLength / 2, 10)

    [<Benchmark; InvocationCount(1000)>]
    member this.DeleteFromEndOfrope() = 
        this.rope.Delete(this.docLength - 10, 9)

[<MemoryDiagnoser; HtmlExporter; MarkdownExporter>]
type GetSubstring() =
    [<Params(100, 1000, 10_000)>]
    member val insertTimes = 0 with get, set

    member val rope = Rope.empty with get, set
    member val docLength = 0 with get, set

    [<IterationSetup>]
    member this.CreateDocument() =
        this.rope <- Rope.empty
        this.docLength <- 0
        for i in [0..this.insertTimes] do
            this.rope <- this.rope.Insert(0, "hello")
            this.docLength <- this.docLength + 5

    [<Benchmark; InvocationCount(1000)>]
    member this.GetSubstringAtStartOfrope() = 
        this.rope.Substring(0, 10)

    [<Benchmark; InvocationCount(1000)>]
    member this.GetSubstringAtMiddleOfrope() =
        this.rope.Substring(this.docLength / 2, 10)

    [<Benchmark; InvocationCount(1000)>]
    member this.GetSubstringAtEndOfrope() = 
        this.rope.Substring(this.docLength - 10, 9)

module Main = 
    [<EntryPoint>]
    let Main _ =
        // BenchmarkRunner.Run<CreateDocument>() |> ignore
        BenchmarkRunner.Run<InsertIntoDocument>() |> ignore
        BenchmarkRunner.Run<DeleteFromDocument>() |> ignore
        BenchmarkRunner.Run<GetSubstring>() |> ignore
        0