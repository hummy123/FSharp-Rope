namespace FsRope

open Types

module RopeNode =
    let inline create chr = {
        Char = chr;
        LeftIdx = 0;
        RightIdx = 0;
    }

    let inline setChar chr node =
        { node with Char = chr }

    let inline setIndex leftIdx rightIdx node = 
        { node with
            LeftIdx = leftIdx;
            RightIdx = rightIdx; }

    let inline setLeft leftIdx node = 
        { node with
            LeftIdx = leftIdx; }

    let inline setRight rightIdx node = 
        { node with
            RightIdx = rightIdx; }

    let inline incrLeft node = 
        { node with
            LeftIdx = node.LeftIdx + 1 }

    let inline incrRight node = 
        { node with 
            RightIdx = node.RightIdx + 1 }

    let inline decrLeft node = 
        { node with
            LeftIdx = node.LeftIdx - 1 }

    let inline decrRight node = 
        { node with
            RightIdx = node.RightIdx - 1 }

    type RopeNode with
        member this.SetIndex leftIdx rightIdx = setIndex leftIdx rightIdx this
        member this.SetRight rightIdx = setRight rightIdx this
        member this.SetLeft leftIdx = setLeft leftIdx this
        member this.IncrLeft() = incrLeft this
        member this.IncrRight() = incrRight this
        member this.DecrLeft() = decrLeft this
        member this.DecrRight() = decrRight this
        member this.SetChar chr = setChar chr this