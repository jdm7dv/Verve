#light

type Vector2D =
    { DX: float; DY: float };;

module Vector2DOps =
    let length v = sqrt(v.DX * v.DX + v.DY * v.DY)
    let scale k v = { DX=k*v.DX; DY=k*v.DY }
    let shiftX x v = { v with DX=v.DX+x }
    let shiftY y v = { v with DY=v.DY+y }
    let shiftXY (x,y) v = { DX=v.DX+x; DY=v.DY+y }
    let zero = { DX=0.0; DY=0.0 }
    let constX dx = { DX=dx; DY=0.0 }
    let constY dy = { DX=0.0; DY=dy }

type Vector2D =
    { DX: float; DY: float };;
    
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vector2D =
    let length v = sqrt(v.DX * v.DX + v.DY * v.DY);;

