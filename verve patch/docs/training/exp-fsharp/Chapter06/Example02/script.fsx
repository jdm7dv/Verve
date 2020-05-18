#light

type Vector2D(dx: float, dy: float) =
    let len = sqrt(dx * dx + dy * dy)
    member v.DX = dx
    member v.DY = dy
    member v.Length = len
    member v.Scale(k) = Vector2D(k*dx, k*dy)
    member v.ShiftX(x) = Vector2D(dx=dx+x, dy=dy)
    member v.ShiftY(y) = Vector2D(dx=dx, dy=dy+y)
    member v.ShiftXY(x,y) = Vector2D(dx=dx+x, dy=dy+y)
    static member Zero = Vector2D(dx=0.0, dy=0.0)
    static member OneX = Vector2D(dx=1.0, dy=0.0)
    static member OneY = Vector2D(dx=0.0, dy=1.0)
;;

let v = Vector2D(3.0, 4.0);;

v.Length;;

v.Scale(2.0).Length;;

type UnitVector2D(dx,dy) =
    let tolerance = 0.000001
    let length = sqrt(dx * dx + dy * dy)
    do if abs(length - 1.0) >= tolerance then failwith "not a unit vector";
    member v.DX = dx
    member v.DY = dy
    new() = UnitVector2D (1.0,0.0)
;;

