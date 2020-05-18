#light

let req = System.Net.WebRequest.Create("not a URL");;

raise (System.InvalidOperationException("not today thank you"));;

if false then 3 else failwith "hit the wall";;

if (System.DateTime.Now > failwith "not yet decided") then
    printfn "you’ve run out of time!";;
    
try
    raise (System.InvalidOperationException ("it's just not my day"))
with
    | :? System.InvalidOperationException -> printfn "caught!";;

open System.IO;;

let http(url: string) =
    try
        let req = System.Net.WebRequest.Create(url)
        let resp = req.GetResponse()
        let stream = resp.GetResponseStream()
        let reader = new StreamReader(stream)
        let html = reader.ReadToEnd()
        html
    with
        | :? System.UriFormatException -> ""
        | :? System.Net.WebException -> "";;


try
    raise (new System.InvalidOperationException ("invalid operation"))
with
    | err -> printfn "oops, msg = '%s'" err.Message;;

let httpViaTryFinally(url: string) =
    let req = System.Net.WebRequest.Create(url)
    let resp = req.GetResponse()
    try
        let stream = resp.GetResponseStream()
        let reader = new StreamReader(stream)
        let html = reader.ReadToEnd()
        html
    finally
        resp.Close();;

let httpViaUseBinding(url: string) =
    let req = System.Net.WebRequest.Create(url)
    use resp = req.GetResponse()
    let stream = resp.GetResponseStream()
    let reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    html;;

exception BlockedURL of string;;

let http2 url =
    if url = "http://www.kaos.org"
    then raise(BlockedURL(url))
    else http url;;

try
    raise(BlockedURL("http://www.kaos.org"))
with
    | BlockedURL(url) -> printf "blocked! url = '%s'\n" url;;

