#light

/// Analyze a string for duplicate words
let wordCount text =
    let words = String.split [' '] text
    let wordSet = Set.of_list words
    let nWords = words.Length
    let nDups = words.Length - wordSet.Count
    (nWords,nDups)

let site1 = ("www.cnn.com",10)
let site2 = ("news.bbc.com",5)
let site3 = ("www.msnbc.com",4)
let sites = (site1,site2,site3)

fst site1;;

let relevance = snd site1;;

relevance;;

let url, relevance = site1;;

let site1,site2,site3 = sites;;

let showResults (nWords,nDups) =
    printfn "--> %d words in the text" nWords
    printfn "--> %d duplicate words" nDups;;

let showWordCount text = showResults (wordCount text);;

let a,b = (1,2,3);; /// Error!
