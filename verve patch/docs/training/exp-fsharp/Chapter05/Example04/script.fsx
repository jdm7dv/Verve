#light

let rec hcf a b =
    if a=0 then b
    elif a<b then hcf a (b-a)
    else hcf (a-b) b;;

hcf 18 12;;

hcf 33 24;;

let hcfGeneric (zero,sub,lessThan) =
    let rec hcf a b =
        if a=zero then b
        elif lessThan a b then hcf a (sub b a)
        else hcf (sub a b) b
    hcf;;

let hcfInt = hcfGeneric (0, (-),(<));;
let hcfInt64 = hcfGeneric (0L,(-),(<));;
let hcfBigInt = hcfGeneric (0I,(-),(<));;

hcfInt 18 12;;

hcfBigInt 1810287116162232383039576I 1239028178293092830480239032I;;

type Numeric<'a> =
    { Zero: 'a;
      Subtract: ('a -> 'a -> 'a);
      LessThan: ('a -> 'a -> bool); };;

let intOps    = { Zero=0 ; Subtract=(-); LessThan=(<) };;
let bigintOps = { Zero=0I; Subtract=(-); LessThan=(<) };;
let int64Ops  = { Zero=0L; Subtract=(-); LessThan=(<) };;

let hcfGeneric (ops : Numeric<'a>) =
    let rec hcf a b =
        if a= ops.Zero then b
        elif ops.LessThan a b then hcf a (ops.Subtract b a)
        else hcf (ops.Subtract a b) b
    hcf;;

let hcfInt    = hcfGeneric intOps;;
let hcfBigInt = hcfGeneric bigintOps;;

hcfInt 18 12;;

hcfBigInt 1810287116162232383039576I 1239028178293092830480239032I;;

type INumeric<'a> =
    abstract Zero : 'a
    abstract Subtract: 'a * 'a -> 'a
    abstract LessThan: 'a * 'a -> bool;;

let intOps =
    { new INumeric<int> with
      member ops.Zero = 0
      member ops.Subtract(x,y) = x - y
      member ops.LessThan(x,y) = x < y };;

let hcfGeneric (ops : INumeric<'a>) =
    let rec hcf a b =
        if a= ops.Zero then b
        elif ops.LessThan(a,b) then hcf a (ops.Subtract(b,a))
        else hcf (ops.Subtract(a,b)) b
    hcf;;

