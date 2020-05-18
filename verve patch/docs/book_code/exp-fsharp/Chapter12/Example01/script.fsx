#light

// ------------------------------------------
// Figure 12-1.

type Expr =
    | Var
    | Num of int
    | Sum of Expr * Expr
    | Prod of Expr * Expr

let rec deriv expr =
    match expr with
    | Var           -> Num 1
    | Num _         -> Num 0
    | Sum (e1, e2)  -> Sum (deriv e1, deriv e2)
    | Prod (e1, e2) -> Sum (Prod (e1, deriv e2), Prod (e2, deriv e1))

// ------------------------------------------

let e1 = Sum (Num 1, Prod (Num 2, Var))

deriv e1

// ------------------------------------------

let precSum = 10
let precProd = 20

let rec stringOfExpr prec expr =
    match expr with
    | Var -> "x"
    | Num i -> Int32.to_string i
    | Sum (e1, e2) ->
        if prec > precSum
        then "(" + stringOfExpr precSum e1 + "+" + stringOfExpr precSum e2 + ")"
        else       stringOfExpr precSum e1 + "+" + stringOfExpr precSum e2
    | Prod (e1, e2) ->
        stringOfExpr precProd e1 + "*" + stringOfExpr precProd e2


// ------------------------------------------

fsi.AddPrinter (fun expr -> stringOfExpr 0 expr)

let e3 = Prod (Var, Prod (Var, Num 2))

deriv e3

// ------------------------------------------
// Figure 12-2.

let simpSum = function
    | Num n, Num m -> Num (n+m)     // constants!
    | Num 0, e | e, Num 0 -> e      // 0+e = e+0 = e
    | e1, e2 -> Sum(e1,e2)

let simpProd = function
    | Num n, Num m -> Num (n*m)     // constants!
    | Num 0, e | e, Num 0 -> Num 0  // 0*e=0
    | Num 1, e | e, Num 1 -> e      // 1*e = e*1 = e
    | e1, e2 -> Prod(e1,e2)

let rec simpDeriv = function
    | Var           -> Num 1
    | Num _         -> Num 0
    | Sum (e1, e2)  -> simpSum (simpDeriv e1, simpDeriv e2)
    | Prod (e1, e2) -> simpSum (simpProd (e1, simpDeriv e2),
                                simpProd (e2, simpDeriv e1))

simpDeriv e3
