#light

open Microsoft.FSharp.Math
let (|Rect|) (x:complex) = (x.RealPart, x.ImaginaryPart)

let (|Polar|) (x:complex) = (x.Magnitude, x.Phase)

let addViaRect a b =
    match a, b with
    | Rect(ar,ai), Rect(br,bi) -> Complex.mkRect(ar+br, ai+bi)

let mulViaRect a b =
    match a, b with
    | Rect(ar,ai), Rect(br,bi) -> Complex.mkRect(ar*br - ai*bi, ai*br + bi*ar)

let mulViaPolar a b =
    match a, b with
    | Polar(m,p), Polar(n,q) -> Complex.mkPolar(m*n, (p+q))


let add2 (Rect(ar,ai)) (Rect(br,bi))   = Complex.mkRect(ar+br, ai+bi)
let mul2 (Polar(r1,th1)) (Polar(r2,th2)) = Complex.mkPolar(r1*r2, th1+th2)

// ----------------------------

let c = Complex.mkRect(3.0, 4.0)

c

do match c with
   | Rect(x,y) -> printfn "x = %g, y = %g" x y

do match c with
   | Polar(x,y) -> printfn "x = %g, y = %g" x y

addViaRect c c

mulViaRect c c

mulViaPolar c c

// ----------------------------