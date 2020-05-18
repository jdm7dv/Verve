#light

// ----------------------------
// Listing 8-3.

open System

type NameLookupService =
    abstract Contains : string -> bool
    abstract ClosestPrefixMatch : string -> string  option

let buildSimpleNameLookup (words: string list) =
    let wordTable = Set.Create(words)
    let score (w1:string) (w2:string) =
        let lim = (min w1.Length w2.Length)
        let rec loop i acc =
            if i >= lim then acc
            else loop (i+1) (Char.code w1.[i] - Char.code w2.[i] + acc)
        loop 0 0

    { new NameLookupService with
        member t.Contains(w) = wordTable.Contains(w)
        member t.ClosestPrefixMatch(w) =
            if wordTable.Contains(w) then Some(w) else
            let above =
                match wordTable.GetNextElement(w) with
                | Some w2 when w2.StartsWith(w) -> Some w2
                | _ -> None
            let below =
                match wordTable.GetPreviousElement(w) with
                | Some w2 when w2.StartsWith(w) -> Some w2
                | _ -> None
            match above, below with
            | Some w1,Some w2 -> Some(if score w w1 > score w w2 then w2 else w1)
            | Some res,None
            | None,Some res -> Some res
            | None,None -> None }

let capitalLookup = buildSimpleNameLookup ["London";"Paris";"Warsaw";"Tokyo"]

capitalLookup.Contains "Paris"

capitalLookup.ClosestPrefixMatch "Wars"

capitalLookup.ClosestPrefixMatch "We"
