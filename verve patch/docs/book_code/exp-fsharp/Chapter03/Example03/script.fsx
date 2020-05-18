#light

let oddPrimes = [3; 5; 7; 11]
let morePrimes = [13; 17]
let primes = 2 :: (oddPrimes @ morePrimes)

let people = [ "Adam"; "Dominic"; "James" ];;

people;;

"Chris" :: people;;

people;;

let printFirst primes =
    match primes with
    | h :: t -> printfn "The first prime in the list is %d" h
    | [] -> printfn "No primes found in the list";;
    
printFirst oddPrimes;;

List.hd [5; 4; 3];;

List.tl [5; 4; 3];;

List.map (fun x -> x*x) [1; 2; 3];;

List.filter (fun x -> x % 3 = 0) [2; 3; 5; 7; 9];;

type 'a option =
    | None
    | Some of 'a;;


let people = [ ("Adam", None);
               ("Eve" , None);
               ("Cain", Some("Adam","Eve"));
               ("Abel", Some("Adam","Eve")) ];;

let showParents (name,parents) =
    match parents with
    | Some(dad,mum) -> printfn "%s has father %s, mother %s" name dad mum
    | None -> printfn "%s has no parents!" name;;
    
showParents ("Adam",None);;

open System.IO;;

/// Get the contents of the URL via a web request
let http(url: string) =
    let req = System.Net.WebRequest.Create(url)
    let resp = req.GetResponse()
    let stream = resp.GetResponseStream()
    let reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    resp.Close()
    html

let fetch url =
    try Some(http(url))
    with :? System.Net.WebException -> None

match (fetch "http://www.nature.com") with
| Some(text) -> printfn "text = %s" text
| None -> printfn "**** no web page found";;
