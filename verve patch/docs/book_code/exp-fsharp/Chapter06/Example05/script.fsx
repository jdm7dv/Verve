#light

open System.Drawing

type IShape =
    abstract Contains : Point -> bool
    abstract BoundingBox : Rectangle;;

let circle(center:Point,radius:int) =
    { new IShape with
          member x.Contains(p:Point) =
              let dx = float32 (p.X - center.X)
              let dy = float32 (p.Y - center.Y)
              sqrt(dx*dx+dy*dy) <= float32 radius
          member x.BoundingBox =
              Rectangle(center.X-radius,center.Y-radius,2*radius+1,2*radius+1) };;

let square(center:Point,side:int) =
    { new IShape with
          member x.Contains(p:Point) =
              let dx = p.X - center.X
              let dy = p.Y - center.Y
              abs(dx) < side/2 && abs(dy) < side/2
          member x.BoundingBox =
              Rectangle(center.X-side,center.Y-side,side*2,side*2) };;

type MutableCircle() =
    let mutable center = Point(x=0,y=0)
    let mutable radius = 10
    member sq.Center with get() = center and set(v) = center <- v
    member sq.Radius with get() = radius and set(v) = radius <- v
    member c.Perimeter = 2.0 * System.Math.PI * float radius
    interface IShape with
        member x.Contains(p:Point) =
            let dx = float32 (p.X - center.X)
            let dy = float32 (p.Y - center.Y)
            sqrt(dx*dx+dy*dy) <= float32 radius
        member x.BoundingBox =
            Rectangle(center.X-radius,center.Y-radius,2*radius+1,2*radius+1);;

open System.Drawing

type IShape =
    abstract Contains : Point -> bool
    abstract BoundingBox : Rectangle;;
    
let circle(center:Point,radius:int) =
    { new IShape with
          member x.Contains(p:Point) =
              let dx = float32 (p.X - center.X)
              let dy = float32 (p.Y - center.Y)
              sqrt(dx*dx+dy*dy) <= float32 radius
          member x.BoundingBox =
              Rectangle(center.X-radius,center.Y-radius,2*radius+1,2*radius+1) };;

let bigCircle = circle(Point(0,0), 100);;

bigCircle.BoundingBox;;

bigCircle.Contains(Point(70,70));;

bigCircle.Contains(Point(71,71));;

let smallSquare = square(Point(1,1), 1);;

smallSquare.BoundingBox;;

smallSquare.Contains(Point(0,0));;

type MutableCircle() =
    let mutable center = Point(x=0,y=0)
    let mutable radius = 10
    member c.Center with get() = center and set(v) = center <- v
    member c.Radius with get() = radius and set(v) = radius <- v
    member c.Perimeter = 2.0 * System.Math.PI * float radius
    interface IShape with
        member c.Contains(p:Point) =
            let dx = float32 (p.X - center.X)
            let dy = float32 (p.Y - center.Y)
            sqrt(dx*dx+dy*dy) <= float32 radius
        member c.BoundingBox =
            Rectangle(center.X-radius,center.Y-radius,2*radius+1,2*radius+1);;

let circle2 = MutableCircle();;

circle2.Radius;;

(circle2 :> IShape).BoundingBox;;

