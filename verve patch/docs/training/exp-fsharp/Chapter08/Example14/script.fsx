#light

type Chain =
    | ChainNode of int * string * Chain
    | ChainEnd of string

    member chain.LengthNotTailRecursive =
        match chain with
        | ChainNode(_,_,subChain) -> 1 + subChain.LengthNotTailRecursive
        | ChainEnd _ -> 0

// ----------------------------
// Listing 8-9.

type Chain2 =
    | ChainNode of int * string * Chain2
    | ChainEnd of string
    // The implementation of this property is tail recursive.
    member chain.Length =
        let rec loop c acc =
            match c with
            | ChainNode(_,_,subChain) -> loop subChain (acc+1)
            | ChainEnd _ -> acc
        loop chain

