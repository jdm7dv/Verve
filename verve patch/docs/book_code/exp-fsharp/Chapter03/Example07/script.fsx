#light

let urlFilter url agent =
    match (url,agent) with
    | "http://www.control.org", 99 -> true
    | "http://www.kaos.org" , _ -> false
    | _, 86 -> true
    | _ -> false;;

let highLow a b =
    match (a,b) with
    | ("lo", lo), ("hi", hi) -> (lo,hi)
    | ("hi", hi), ("lo", lo) -> (lo,hi)
    | _ -> failwith "expected a both a high and low value";;

highLow ("hi",300) ("lo",100);;

let urlFilter3 url agent =
    match url,agent with
    | "http://www.control.org", 86 -> true
    | "http://www.kaos.org", _ -> false;;
    
let urlFilter4 url agent =
    match url,agent with
    | "http://www.control.org", 86 -> true
    | "http://www.kaos.org", _ -> false
    | _ -> failwith "unexpected input";;
    
let urlFilter2 url agent =
    match url,agent with
    | "http://www.control.org", _ -> true
    | "http://www.control.org", 86 -> true
    | _ -> false;;

let sign x =
    match x with
    | _ when x < 0 -> -1
    | _ when x > 0 -> 1
    | _ -> 0;;

let getValue a =
    match a with
    | (("lo" | "low") ,v) -> v
    | ("hi",v) | ("high",v) -> v
    | _ -> failwith "expected a both a high and low value";;

