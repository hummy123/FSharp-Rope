﻿namespace FsRope

open Types

module RopeNode =
    let inline create chr = {
        Char = chr;
        LeftIdx = 0;
        RightIdx = 0;
    }

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

    type RopeNode with
        member this.SetIndex leftIdx rightIdx = setIndex leftIdx rightIdx this
        member this.SetRight rightIdx = setRight rightIdx this
        member this.SetLeft leftIdx = setLeft leftIdx this