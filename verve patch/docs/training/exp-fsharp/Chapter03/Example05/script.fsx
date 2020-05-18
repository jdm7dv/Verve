#light

let rec factorial n = if n <= 1 then 1 else n * factorial (n-1);;

factorial 5;;

let rec length l =
    match l with
    | [] -> 0
    | h :: t -> 1 + length t;;

open System.IO

/// Get the contents of the URL via a web request
let http(url: string) =
    let req = System.Net.WebRequest.Create(url)
    let resp = req.GetResponse()
    let stream = resp.GetResponseStream()
    let reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    resp.Close()
    html;;

let rec repeatFetch url n =
    if n > 0 then
        let html = http url
        printfn "fetched <<< %s >>> on iteration %d" html n
        repeatFetch url (n-1);;

let rec badFactorial n = if n <= 0 then 1 else n * badFactorial n;;

let rec even n = (n = 0u) || odd(n-1u)
and odd n = (n <> 0u) && even(n-1u);;

let even (n:uint32) = (n % 2u) = 0u;;
let odd (n:uint32) = (n % 2u) = 1u;;

