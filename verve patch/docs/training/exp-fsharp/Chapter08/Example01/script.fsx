#light

open Microsoft.FSharp.Math.Primitives

// ----------------------------
// Listing 8-1.

type BigInt =
    { sign : int; v : BigNat }
    override x.Equals(yobj:obj) =
        let y = unbox<BigInt>(yobj)
        (x.sign = y.sign) && (x.v = y.v) || BigNat.IsZero(x.v)

    interface System.IComparable with
        override x.CompareTo(yobj:obj) =
            let y = unbox<BigInt>(yobj)
            match x.sign,y.sign with
            |  1, 1 ->  compare x.v y.v
            | -1,-1 ->  compare y.v x.v
            | _ when BigNat.IsZero(x.v) && BigNat.IsZero(x.v) -> 0
            |  1, -1 -> 1
            | -1, 1 -> -1
            | _ -> invalid_arg "BigInt signs should be +/- 1"
