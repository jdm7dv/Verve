#light

type DiscreteEventCounter =
    { mutable Total: int;
      mutable Positive: int;
      Name : string };;

let recordEvent (s: DiscreteEventCounter) isPositive =
    s.Total <- s.Total+1
    if isPositive then s.Positive <- s.Positive+1;;

let reportStatus (s: DiscreteEventCounter) =
    printfn "We have %d %s out of %d" s.Positive s.Name s.Total;;

let newCounter nm =
    { Total = 0;
      Positive = 0;
      Name = nm };;

let longPageCounter = newCounter "long page(s)";;

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

let fetch url =
    let page = http url
    recordEvent longPageCounter (page.Length > 10000)
    page;;

fetch "http://www.smh.com.au" |> ignore;;

fetch "http://www.theage.com.au" |> ignore;;

reportStatus longPageCounter;;

let cell1 = ref 1;;

cell1;;

!cell1;;

cell1 := 3;;

cell1;;

!cell1;;

let cell2 = cell1;;

!cell2;;

cell1 := 7;;

!cell2;;

let generateStamp =
    let count = ref 0
    (fun () -> count := !count + 1; !count);;

generateStamp();;

generateStamp();;

