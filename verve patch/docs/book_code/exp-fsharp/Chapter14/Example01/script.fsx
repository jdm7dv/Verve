// ----------------------------
// Listing 14-1.

#light

open System.Net
open System.Net.Sockets
open System.IO
open System.Text.RegularExpressions
open Microsoft.FSharp.Text.Printf
open System.Text

/// A table of MIME content types
let mimeTypes = 
    dict [".html", "text/html";
          ".htm",  "text/html";
          ".txt",  "text/plain";
          ".gif",  "image/gif";
          ".jpg",  "image/jpeg";
          ".png",  "image/png"]

/// Compute a MIME type from a file extension
let getMimeType(ext) =
    if mimeTypes.ContainsKey(ext) then mimeTypes.[ext]
    else "binary/octet"

/// The pattern Regex1 uses a regular expression to match
/// one element
let (|Regex1|_|) (patt: string) (inp: string) =
    try Some(Regex.Match(inp, patt).Groups.Item(1).Captures.Item(0).Value)
    with _ -> None

/// The root for the data we serve
let root = @"c:\inetpub\wwwroot"
 
/// Handle a TCP connection for a HTTP GET
let handleClient(client: TcpClient) =
    use stream = client.GetStream()
    let out = new StreamWriter(stream)
    let inp = new StreamReader(stream)
    match inp.ReadLine() with
    | Regex1 "GET (.*?) HTTP/1\\.[01]$" fileName -> 
        let fname = root + @"\" + fileName.Replace("/", @"\")
        let mimeType = getMimeType(Path.GetExtension(fname))
        let text = File.ReadAllBytes(fname)
        twprintfn out "HTTP/1.0 200 OK"
        twprintfn out "Content-Length: %d" text.Length
        twprintfn out "Content-Type: %s" mimeType
        twprintfn out ""
        out.Flush()
        stream.Write(text, 0, text.Length)
    | line -> 
        ()

/// The server as an asynchronous process. We handle requests 
/// sequentially. 
let server = 
    async { let socket = new TcpListener(IPAddress.Parse("127.0.0.1"), 8090)
            do socket.Start()
            while true do
                use client = socket.AcceptTcpClient()
                do try handleClient(client) with _ -> ()
          }

// ----------------------------

Async.Spawn server

http "http://127.0.0.1:8090/iisstart.htm"

// ----------------------------
