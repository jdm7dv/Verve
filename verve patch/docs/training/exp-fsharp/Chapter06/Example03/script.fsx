#light

open System.Collections.Generic

type SparseVector(items: seq<int * float>)=
    let elems = new SortedDictionary<_,_>()
    do items |> Seq.iter (fun (k,v) -> elems.Add(k,v))
    
    /// This defines an indexer property
    member t.Item
        with get(idx) =
            if elems.ContainsKey(idx) then elems.[idx]
            else 0.0;;

let v = SparseVector [(3,547.0)];;

v.[4];;

v.[3];;

type Vector2DWithOperators(dx:float,dy:float) =
    member x.DX = dx
    member x.DY = dy

    static member (+) (v1: Vector2DWithOperators ,v2: Vector2DWithOperators) =
        Vector2DWithOperators(v1.DX + v2.DX, v1.DY + v2.DY)
    
    static member (-) (v1: Vector2DWithOperators ,v2: Vector2DWithOperators) =
        Vector2DWithOperators (v1.DX - v2.DX, v1.DY - v2.DY);;

let v1 = new Vector2DWithOperators (3.0,4.0);;

v1 + v1;;

v1 - v1;;

open System.Drawing

type LabelInfo(?text:string, ?font:Font) =
    let text = defaultArg text ""
    let font = match font with
               | None -> new Font(FontFamily.GenericSansSerif,12.0f)
               | Some v -> v
    member x.Text = text
    member x.Font = font;;

LabelInfo (text="Hello World");;

LabelInfo("Goodbye Lenin");;

LabelInfo(font=new Font(FontFamily.GenericMonospace,36.0f),
          text="Imagine");;
          
open System.Windows.Forms

let form = new Form(Visible=true,TopMost=true,Text="Welcome to F#");;

let form =
    let tmp = new Form()
    tmp.Visible <- true
    tmp.TopMost <- true
    tmp.Text <- "Welcome to F#"
    tmp;;

type LabelInfoWithPropertySetting() =
    let mutable text = "" // the default
    let mutable font = new Font(FontFamily.GenericSansSerif,12.0f)
    member x.Text with get() = text and set(v) = text <- v
    member x.Font with get() = font and set(v) = font <- v;;

LabelInfoWithPropertySetting(Text="Hello World");;

/// Interval(lo,hi) represents the range of numbers from lo to hi,
/// but not including either lo or hi.
type Interval(lo,hi) =
    member r.Lo = lo
    member r.Hi = hi
    member r.IsEmpty = hi <= lo
    member r.Contains(v) = lo < v && v < hi
    
    static member Empty = Interval(0.0,0.0)
    
    /// Return the smallest interval that covers both the intervals
    /// This method is overloaded.
    static member Span(r1:Interval,r2:Interval) =
        if r1.IsEmpty then r2 else
        if r2.IsEmpty then r1 else
        Interval(min r1.Lo r2.Lo,max r1.Hi r2.Hi)

    /// Return the smallest interval that covers all the intervals
    /// This method is overloaded.
    static member Span(ranges: #seq<Interval>) =
        Seq.fold (fun r1 r2 -> Interval.Span(r1,r2)) Interval.Empty ranges;;

type Vector =
    { DX:float; DY:float }
    member v.Length = sqrt(v.DX*v.DX+v.DY*v.DY)
 
type Point =
    { X:float; Y:float }

    [<OverloadID("SubtractPointPoint")>]
    static member (-) (p1:Point,p2:Point) = { DX=p1.X-p2.X; DY=p1.Y-p2.Y }

    [<OverloadID("subtractPointVector")>]
    static member (-) (p:Point,v:Vector) = { X=p.X-v.DX; Y=p.Y-v.DY };;

