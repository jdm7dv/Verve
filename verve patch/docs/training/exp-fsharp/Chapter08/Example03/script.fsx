#light

let isWord (words: string list) =
    let wordTable = Set.Create(words)
    fun w -> wordTable.Contains(w)

let isCapital = isWord ["London";"Paris";"Warsaw";"Tokyo"]

isCapital "Paris"

isCapital "Manchester"

let isWordSlow2 (words: string list) (w:string) =
    List.mem w words

let isCapitalSlow2 inp = isWordSlow2 ["London";"Paris";"Warsaw";"Tokyo"] inp

let isWordSlow3 (words: string list) (w:string) =
    let wordTable = Set.Create(words)
    wordTable.Contains(w)

let isCapitalSlow3 inp = isWordSlow3 ["London";"Paris";"Warsaw";"Tokyo"] inp

let isWordHS (words: string list) =
    let wordTable = HashSet.Create(words)
    fun w -> wordTable.Contains(w)
