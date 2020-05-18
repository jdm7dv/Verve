#light

let factorizeImperative n =
    let mutable primefactor1 = 1
    let mutable primefactor2 = n
    let mutable i = 2
    let mutable fin = false
    while (i < n && not fin) do
        if (n % i = 0) then
            primefactor1 <- i
            primefactor2 <- n / i
            fin <- true
        i <- i + 1

    if (primefactor1 = 1) then None
    else Some (primefactor1, primefactor2);;

let factorizeRecursive n =
    let rec find i =
        if i >= n then None
        elif (n % i = 0) then Some(i,n / i)
        else find (i+1)
    find 2;;
    
open System.Collections.Generic

type ResizeArray<'a> = System.Collections.Generic.List<'a>;;

let divideIntoEquivalenceClasses keyf seq =
    // The dictionary to hold the equivalence classes
    let dict = new Dictionary<'key,ResizeArray<'a>>()

    // Build the groupings
    seq |> Seq.iter (fun v ->
        let key = keyf v
        let ok,prev = dict.TryGetValue(key)
        if ok then prev.Add(v)
        else
            let prev = new ResizeArray<'a>()
            dict.[key] <- prev
            prev.Add(v))
    
    // Return the sequence-of-sequences. Don't reveal the
    // internal collections: just reveal them as sequences
    dict |> Seq.map (fun group -> group.Key, Seq.readonly group.Value);;

divideIntoEquivalenceClasses (fun n -> n % 3) [ 0 .. 10 ];;

open System.IO

let reader1, reader2 =
    let reader = new StreamReader(File.OpenRead("test.txt"))
    let firstReader() = reader.ReadLine()
    let secondReader() = reader.ReadLine()

    // Note: we close the stream reader here!
    // But we are returning function values which use the reader
    // This is very bad!
    reader.Close()
    firstReader, secondReader

// Note: stream reader is now closed! The next line will fail!
let firstLine = reader1()
let secondLine = reader2()
firstLine, secondLine;;

let line1, line2 =
    let reader = new StreamReader(File.OpenRead("test.txt"))
    let firstLine = reader.ReadLine()
    let secondLine = reader.ReadLine()
    reader.Close()
    firstLine, secondLine;;

let reader =
    Seq.generate_using
        (fun () -> new StreamReader(File.OpenRead("test.txt")))
        (fun reader -> if reader.EndOfStream then None else Some(reader.ReadLine()));;

let reader =
    seq { use reader = new StreamReader(File.OpenRead("test.txt"))
          while not reader.EndOfStream do
          yield reader.ReadLine() };;

