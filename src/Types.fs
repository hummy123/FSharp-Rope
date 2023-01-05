namespace FsRope

module Types =
    /// A RopeNode contains a character with index metadata.
    [<Struct>]
    type RopeNode = {
        (* We are storing a char array because we want to represent grapheme clusters
         * as a single char. *)
        Char: char array;
        IsLine: int;
        LeftIdx: int;
        RightIdx: int;
        LeftLns: int;
        RightLns: int;
    }

   /// A Rope is a data structure for efficiently storing and manipulating text.
    type Rope =
        | E
        | T of int * Rope * RopeNode * Rope

    (* A small optimisation attempt to get around branch prediction.
     * If we are inserting a character with a line, add 1; else add 0. *)
    [<Literal>]
    let HasLine = 1

    [<Literal>]
    let HasNoLine = 0
