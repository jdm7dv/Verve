/// Analyze a string for duplicate words
let wordCount text =
    let words = String.split [' '] text in
    let wordSet = Set.of_list words in
    let nWords = words.Length in
    let nDups = words.Length - wordSet.Count in
    (nWords,nDups)