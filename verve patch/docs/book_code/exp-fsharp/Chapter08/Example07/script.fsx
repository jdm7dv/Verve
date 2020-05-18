#light

// ----------------------------
// Listing 8-5.

type Table<'a,'b> =
    abstract Item : 'a -> 'b with get
    abstract Discard : unit -> unit

let memoizeAndPermitDiscard f =
    let lookasideTable = new System.Collections.Generic.Dictionary<_,_>()
    { new Table<_,_> with
          member t.Item
             with get(n) =
                 if lookasideTable.ContainsKey(n)
                 then lookasideTable.[n]
                 else let res = f n
                      lookasideTable.Add(n,res)
                      res
          member t.Discard() =
              lookasideTable.Clear() }


let rec fibFast =
    memoizeAndPermitDiscard
        (fun n ->
            printfn "computing fibFast %d" n
            if n <= 2 then 1 else fibFast.[n-1] + fibFast.[n-2])

fibFast.[3]

fibFast.[5]

fibFast.Discard()

fibFast.[5]

