#light

2147483647+1;;

let doubleAndAdd a b = a * a + b;;

let doubleAndAdd (a:float) b = a * a + b;;

let doubleAndAdd (a:float) (b:float) : float = a * a + b;;

let encode (n: int32) =
    if (n >= 0 && n <= 0x7F) then [ n ]
    elif (n >= 0x80 && n <= 0x3FFF) then [ (0x80 ||| (n >>> 8)) &&& 0xFF;
                                           (n &&& 0xFF) ]
    else [ 0xC0; ((n >>> 24) &&& 0xFF);
                 ((n >>> 16) &&& 0xFF);
                 ((n >>> 8) &&& 0xFF);
                 (n &&& 0xFF) ];;

encode 32;;

encode 320;;

encode 32000;;

