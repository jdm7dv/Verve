#light

open System

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

let repeatFetch url n =
    for i = 1 to n do
        let html = http url
        printf "fetched <<< %s >>>\n" html
    printf "Done!\n";;

let loopUntilSaturday() =
    while (DateTime.Now.DayOfWeek <> DayOfWeek.Saturday) do
        printf "Still working!\n"
    printf "Saturday at last!\n";;

for (b,pj) in [ ("Banana 1",true); ("Banana 2",false) ] do
    if pj then printfn "%s is in pyjamas today!" b;;

open System.Text.RegularExpressions;;

for m in (Regex.Matches("All the Pretty Horses","[a-zA-Z]+")) do
    printf "res = %s\n" m.Value;;
