namespace FsRope

module Types =
    /// A RopeNode contains a character with index metadata.
    [<Struct>]
    type RopeNode = {
        (* We are storing a char array because we want to represent grapheme clusters
         * as a single char. *)
        Char: char array;
        Lines: int;
        LeftIdx: int;
        RightIdx: int;
        LeftLns: int;
        RightLns: int;
    }

   /// A Rope is a data structure for efficiently storing and manipulating text.
    type RopeTree =
        | E
        | T of int * RopeTree * RopeNode * RopeTree

    type Rope = {
        Tree: RopeTree;
        TextLength: int;
        LineCount: int;
    }

    (* A small optimisation attempt to get around branch prediction.
     * If we are inserting a character with a line, add 1; else add 0. *)
    [<Literal>]
    let HasLine = 1

    [<Literal>]
    let HasNoLine = 0
