namespace FsRope

open Types

module RopeNode =
    let inline create chr line = {
        Char = chr;
        Lines = line;
        LeftIdx = 0;
        RightIdx = 0;
        LeftLns = 0;
        RightLns = 0;
    }

    let inline setMetadata leftIdx leftLns rightIdx rightLns node =
        { node with
            LeftIdx = leftIdx;
            LeftLns = leftLns;
            RightIdx = rightIdx;
            RightLns = rightLns; }

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

    let inline plusLeft line node = 
        { node with
            LeftIdx = node.LeftIdx + 1;
            LeftLns = node.LeftLns + line }

    let inline plusRight line node = 
        { node with 
            RightIdx = node.RightIdx + 1;
            RightLns = node.RightLns + line }

    let inline decrLeft node = 
        { node with
            LeftIdx = node.LeftIdx - 1 }

    let inline decrRight node = 
        { node with
            RightIdx = node.RightIdx - 1 }

    type RopeNode with
        member inline this.SetIndex leftIdx rightIdx = setIndex leftIdx rightIdx this
        member inline this.SetRight rightIdx = setRight rightIdx this
        member inline this.SetLeft leftIdx = setLeft leftIdx this
        member inline this.PlusLeft line = plusLeft line this
        member inline this.PlusRight line = plusRight line this
        member inline this.DecrLeft() = decrLeft this
        member inline this.DecrRight() = decrRight this
        member inline this.SetChar chr = setChar chr this
        member inline this.SetData (leftIdx, leftLns) (rightIdx, rightLns) = setMetadata leftIdx leftLns rightIdx rightLns this