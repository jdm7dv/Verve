#light

// ----------------------------
// Listing 8-4.

let memoize (f: 'a -> 'b) =
    let t = new System.Collections.Generic.Dictionary<'a,'b>()
    fun n ->
        if t.ContainsKey(n) then t.[n]
        else let res = f n
             t.Add(n,res)
             res

let rec fibFast =
    memoize (fun n -> if n <= 2 then 1 else fibFast(n-1) + fibFast(n-2))

let rec fibNotFast n =
    memoize (fun n -> if n <= 2 then 1 else fibNotFast(n-1) + fibNotFast(n-2)) n
