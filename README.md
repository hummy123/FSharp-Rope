# Fs-Rope

This is an immutable implementation of the (Rope data structure)[https://en.wikipedia.org/wiki/Rope_(data_structure)] used for efficient text processing written in F#.

This particular rope uses an (AA Tree)[https://en.wikipedia.org/wiki/AA_tree] as a backing structure and stores text data on every node rather than just the leaves for improved performance.

Unicode grapheme clusters are automatically handled for consumers, so there is never any need for worrying about inserting between a grapheme cluster or retrieving a substring with a malformed character at the start or end.

## API

```

// An easier-to-use C# API is currently being worked on.

// F#
open FsRope
open FsRope.Rope

let rope = Rope.create " name" // creates rope with text " name"
let rope = Rope.insert 0 "my \n" // rope has text "my \n name"
let line = Rope.getLine 0 rope // returns string with text "my \n"
let str = Rope.substring 0 1 rope // returns string with text "m"

// OOP-style API also available.

let rope2 = Rope.empty // creates an empty rope
let rope2 = rope2.Insert(0, "name")
let line = rope2.GetLine 0
let str = rope2.Substring(0, 1)
```