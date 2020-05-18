#light

let round x =
    if x >= 100 then 100
    elif x < 0 then 0
    else x;;
    
let round x =
    match x with
    | _ when x >= 100 -> 100
    | _ when x < 0 -> 0
    | _ -> x;;
    
let round2 (x,y) =
    if x >= 100 || y >= 100 then 100,100
    elif x < 0 || y < 0 then 0,0
    else x,y;;

