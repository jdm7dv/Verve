#light

let sites = [ "http://www.live.com";
              "http://www.google.com" ];;

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
    
let fetch url = (url, http url);;

List.map fetch sites;;

List.map;;

let primes = [2; 3; 5; 7];;

let primeCubes = List.map (fun n -> n * n * n) primes;;

primeCubes;;

let resultsOfFetch = List.map (fun url -> (url, http url)) sites;;

List.map (fun (_,p) -> String.length p) resultsOfFetch;;

let delimiters = [ ' '; '\n'; '\t'; '<'; '>'; '=' ]
let getWords s = String.split delimiters s
let getStats site =
    let url = "http://" + site
    let html = http url
    let hwords = html |> getWords
    let hrefs = html |> getWords |> List.filter (fun s -> s = "href")
    (site,html.Length, hwords.Length, hrefs.Length);;

let sites = [ "www.live.com";"www.google.com";"search.yahoo.com" ];;

sites |> List.map getStats;;

sites.Map(getStats);;

let google = http "http://www.google.com";;
google |> getWords |> List.filter (fun s -> s = "href") |> List.length;;

let countLinks = getWords >> List.filter (fun s -> s = "href") >> List.length;;
google |> countLinks;;

let shift (dx,dy) (px,py) = (px + dx, py + dy);;
let shiftRight = shift (1,0);;
let shiftUp = shift (0,1);;
let shiftLeft = shift (-1,0);;
let shiftDown = shift (0,-1);;

shiftRight (10,10);;

List.map (shift (2,2)) [ (0,0); (1,0); (1,1); (0,1) ];;

open System.Drawing;;
let remap (r1: Rectangle) (r2: Rectangle) =
    let scalex = float r2.Width / float r1.Width
    let scaley = float r2.Height / float r1.Height
    let mapx x = r2.Left + truncate (float (x - r1.Left) * scalex)
    let mapy y = r2.Top + truncate (float (y - r1.Top) * scaley)
    let mapp (p: Point) = Point(mapx p.X, mapy p.Y)
    mapp;;

let mapp = remap (Rectangle(100,100,100,100)) (Rectangle(50,50,200,200));;

mapp (Point(100,100));;

mapp (Point(150,150));;

mapp (Point(200,200));;

let sites = [ "http://www.live.com";
              "http://www.google.com";
              "http://search.yahoo.com" ];;

sites |> List.iter (fun site -> printfn "%s, length = %d" site (http site).Length);;

open System;;

let start = DateTime.Now;;

http "http://www.newscientist.com";;

let finish = DateTime.Now;;

let elapsed = finish - start;;

elapsed;;

let time f =
    let start = DateTime.Now
    let res = f()
    let finish = DateTime.Now
    (res, finish - start);;

time (fun () -> http "http://www.newscientist.com");;

open System.IO;;

[@"C:\Program Files"; @"C:\Windows"] |> List.map Directory.GetDirectories;;

let f = Console.WriteLine;;

let f = (Console.WriteLine : string -> unit);;

