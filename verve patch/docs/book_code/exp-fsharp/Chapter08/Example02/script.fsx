#light

open Microsoft.FSharp.Math.Primitives

// ----------------------------
// Listing 8-2.

type BigInt =
    { sign : int; v : BigNat }
    interface System.IComparable

module BigIntOps =
    let equal x y = (x.sign = y.sign) && (x.v = y.v) || BigNat.IsZero(x.v)

    let hashBigInt x = if BigNat.IsZero(x.v) then 0 else hash x.sign + hash x.v

    let compareBigInt x y =
            match x.sign,y.sign with
            |  1, 1 ->  compare x.v  y.v
            | -1,-1 ->  compare y.v x.v
            | _ when BigNat.IsZero(x.v) && BigNat.IsZero(x.v) -> 0
            |  1, -1 -> 1
            | -1, 1 -> -1
            | _ -> invalid_arg "BigInt signs should be +/- 1"

// OK, let's augment the type with generic hash/compare/print behavior
type BigInt with
    override x.GetHashCode() = BigIntOps.hashBigInt(x)
    override x.Equals(y:obj) = BigIntOps.equal x (unbox y)
    override x.ToString() =
        sprintf "%s%A"
            (if x.sign < 0 && not (BigNat.IsZero(x.v))  then "-" else "")
            x.v

    interface System.IComparable with
        member x.CompareTo(y:obj) = BigIntOps.compareBigInt x (unbox y)
