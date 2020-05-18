#light

type MutableVector2D(dx:float,dy:float) =
    let mutable currDX = dx
    let mutable currDY = dy
    
    member v.DX with get() = currDX and set(v) = currDX <- v
    member v.DY with get() = currDY and set(v) = currDY <- v
    member v.Length
        with get () = sqrt(currDX*currDX+currDY*currDY)
        and set len =
            let theta = v.Angle
            currDX <- cos(theta)*len
            currDY <- sin(theta)*len

    member v.Angle
        with get () = atan2 currDY currDX
        and set theta =
            let len = v.Length
            currDX <- cos(theta)*len
            currDY <- sin(theta)*len;;

let v = MutableVector2D(3.0,4.0);;

(v.DX, v.DY);;

(v.Length, v.Angle);;

v.Angle <- System.Math.PI / 6.0;; // "30 degrees"

(v.DX, v.DY);;

(v.Length, v.Angle);;
