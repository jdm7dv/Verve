#light

open System

type index = int
type flags = int64
type results = string * TimeSpan * int * int;;

type StringMap<'a> = Microsoft.FSharp.Collections.Map<string,'a>
type Projections<'a,'b> = ('a -> 'b) * ('b -> 'a);;

type Person =
    { Name: string;
      DateOfBirth: System.DateTime; }

{ Name = "Bill"; DateOfBirth = new System.DateTime(1962,09,02) };;

{ new Person
  with Name = "Anna"
  and DateOfBirth = new System.DateTime(1968,07,23) };;

type PageStats =
    { Site: string;
      Time: System.TimeSpan;
      Length: int;
      NumWords: int;
      NumHRefs: int };;

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

let time f =
    let start = DateTime.Now
    let res = f()
    let finish = DateTime.Now
    (res, finish - start);;

let delimiters = [ ' '; '\n'; '\t'; '<'; '>'; '=' ]
let getWords s = String.split delimiters s;;

let stats site =
    let url = "http://" + site
    let html,t = time (fun () -> http url)
    let hwords = html |> getWords
    let hrefs = hwords |> List.filter (fun s -> s = "href")
    { Site=site; Time=t; Length=html.Length;
      NumWords=hwords.Length; NumHRefs=hrefs.Length }

stats "www.live.com";;

type Person =
    { Name: string;
      DateOfBirth: System.DateTime; };;

type Company =
    { Name: string;
      Address: string; };;

type Dot = { X: int; Y: int }
type Point = { X: float; Y: float };;

let coords1 (p:Point) = (p.X,p.Y);;

let coords2 (d:Dot) = (d.X,d.Y);;

let dist p = sqrt (p.X * p.X + p.Y * p.Y);; // use of X and Y implies type "Point"

type Point3D = { X: float; Y: float; Z: float };;

let p1 = { X=3.0; Y=4.0; Z=5.0 };;

let p2 = { p1 with Y=0.0; Z=0.0 };;

let p2 = { X=p1.X; Y=0.0; Z=0.0 };;

type Route = int
type Make = string
type Model = string
type Transport =
    | Car of Make * Model
    | Bicycle
    | Bus of Route;;
    
let nick = Car("BMW","360");;

let don = [ Bicycle; Bus 8 ];;

let james = [ Car ("Ford","Fiesta"); Bicycle ];;

let averageSpeed (tr: Transport) =
    match tr with
    | Car _ -> 35
    | Bicycle -> 16
    | Bus _ -> 24;;

type 'a option =
    | None
    | Some of 'a;;

type Proposition =
    | True
    | And of Proposition * Proposition
    | Or of Proposition * Proposition
    | Not of Proposition;;

let rec eval (p: Proposition) =
    match p with
    | True -> true
    | And(p1,p2) -> eval p1 && eval p2
    | Or (p1,p2) -> eval p1 || eval p2
    | Not(p1) -> not (eval p1);;

type 'a list =
    | ([])
    | (::) of 'a * 'a list;;
    
type Tree<'a> =
    | Tree of 'a * Tree<'a> * Tree<'a>
    | Tip of 'a;;

let rec size tree =
    match tree with
    | Tree(_,l,r) -> 1 + size l + size r
    | Tip _ -> 1;;

let small = Tree("1",Tree("2",Tip("a"),Tip("b")),Tip("c"));;

small;;

size small;;

type Point3D = Vector3D of float * float * float
let origin = Vector3D(0.,0.,0.)
let unitX = Vector3D(1.,0.,0.)
let unitY = Vector3D(0.,1.,0.)
let unitZ = Vector3D(0.,0.,1.);;

let length (Vector3D(dx,dy,dz)) = sqrt(dx*dx+dy*dy+dz*dz);;

type node =
    { Name : string;
      Links : link list }
and link =
    | Dangling
    | Link of node;;

