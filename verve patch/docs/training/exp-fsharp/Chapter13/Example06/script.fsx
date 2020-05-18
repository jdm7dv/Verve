#light

// ----------------------------
// Listing 13-5.

open System.Net
open System.IO
open Microsoft.FSharp.Control.CommonExtensions

let museums = ["MOMA",           "http://moma.org/";
               "British Museum", "http://www.thebritishmuseum.ac.uk/"
               "Prado",          "http://museoprado.mcu.es"]

let tprintfn fmt =
    printf "[.NET Thread %d]" System.Threading.Thread.CurrentThread.ManagedThreadId;
    printfn fmt

let fetchAsync(nm,url:string) =
    async { do tprintfn "Creating request for %s..." nm
            let req  = WebRequest.Create(url)

            let! resp  = req.GetResponseAsync()

            do tprintfn "Getting response stream for %s..." nm
            let stream = resp.GetResponseStream()

            do tprintfn "Reading response for %s..." nm
            let reader = new StreamReader(stream)
            let! html = reader.ReadToEndAsync()

            do tprintfn "Read %d characters for %s..." html.Length nm }

for nm,url in museums do
    Async.Spawn (fetchAsync(nm,url))

