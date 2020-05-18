#light

type Vector2D =
    { DX: float; DY: float }
    member v.Length = sqrt(v.DX * v.DX + v.DY * v.DY)
    member v.Scale(k) = { DX=k*v.DX; DY=k*v.DY }
    member v.ShiftX(x) = { v with DX=v.DX+x }
    member v.ShiftY(y) = { v with DY=v.DY+y }
    member v.ShiftXY(x,y) = { DX=v.DX+x; DY=v.DY+y }
    static member Zero = { DX=0.0; DY=0.0 }
    static member ConstX(dx) = { DX=dx; DY=0.0 }
    static member ConstY(dy) = { DX=0.0; DY=dy };;

let v = {DX = 3.0; DY=4.0 };;

v.Length;;

v.Scale(2.0).Length;;

Vector2D.ConstX(3.0);;

type Vector2D =
    { DX: float; DY: float }
    member v.Length = sqrt(v.DX * v.DX + v.DY * v.DY)
    member v.Scale(k) = { DX=k*v.DX; DY=k*v.DY }
    member v.ShiftX(x) = { v with DX=v.DX+x }
    member v.ShiftY(y) = { v with DY=v.DY+y }
    member v.ShiftXY(x,y) = { DX=v.DX+x; DY=v.DY+y }
    static member Zero = { DX=0.0; DY=0.0 }
    static member ConstX(dx) = { DX=dx; DY=0.0 }
    static member ConstY(dy) = { DX=0.0; DY=dy }
    member v.LengthWithSideEffect =
        printfn "Computing!"
        sqrt(v.DX * v.DX + v.DY * v.DY)
;;

let x = {DX = 3.0; DY=4.0 };;

x.LengthWithSideEffect;;

x.LengthWithSideEffect;;

type Tree<'a> =
    | Tree of 'a * Tree<'a> * Tree<'a>
    | Tip of 'a
    member t.Size =
        match t with
        | Tree(_,l,r) -> 1 + l.Size + r.Size
        | Tip _ -> 1
;;

