namespace FsRope

module Types =
    /// A RopeNode contains a character with index metadata.
    [<Struct>]
    type RopeNode = {
        (* We are storing a char array because we want to represent grapheme clusters
         * as a single char. *)
        Char: char array;
        LeftIdx: int;
        RightIdx: int;
    }

   /// A Rope is a data structure for efficiently storing and manipulating text.
    type Rope =
        | E
        | T of int * Rope * RopeNode * Rope
