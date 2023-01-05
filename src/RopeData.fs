namespace FsRope

open Types

 (* Internal module to organise functions that get data like index or line size. *)
 module internal RopeData =
    let inline size node =
        match node with
        | E -> 0
        | T(_, _, v, _) -> v.LeftIdx + v.RightIdx + 1

    let inline lines node =
        match node with
        | E -> 0
        | T(_, _, v, _) -> v.LeftLns + v.RightLns + v.Lines

    let inline lineLength node = 
        match node with
        | E -> 0
        | T(_, _, v, _) -> v.Lines

    let inline idxLnSize node =
        match node with
        | E -> 0, 0
        | T(_, _, v, _) -> v.LeftIdx + v.RightIdx + 1, v.LeftLns + v.RightLns + v.Lines

    let inline sizeLeft node = 
        match node with 
        | E -> 0
        | T(_, _, v, _) -> v.LeftIdx

    let inline linesLeft node =
        match node with
        | E -> 0
        | T(_, _, v, _) -> v.LeftLns

    let inline sizeRight node = 
        match node with 
        | E -> 0
        | T(_, _, v, _) -> v.RightIdx

    let inline linesRight node =
        match node with
        | E -> 0
        | T(_, _, v, _) -> v.RightLns
