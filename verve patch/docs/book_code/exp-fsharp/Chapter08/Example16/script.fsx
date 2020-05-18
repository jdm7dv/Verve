#light

type Expr =
    | Add  of Expr * Expr
    | Bind of string * Expr * Expr
    | Var  of string
    | Num  of int

type env = Map<string,int>

let rec eval (env: env) expr =
    match expr with
    | Add(e1,e2)         -> eval env e1 + eval env e2
    | Bind(var,rhs,body) -> eval (env.Add(var, eval env rhs)) body
    | Var(var)           -> env.[var]
    | Num(n)             -> n

// ----------------------------
// Listing 8-12.

let rec evalCont (env: env) expr cont =
    match expr with
    | Add(e1,e2)         ->
        evalCont env e1 (fun v1 ->
        evalCont env e2 (fun v2 ->
        cont (v1+v2)))
    | Bind(var,rhs,body) ->
        evalCont env rhs (fun v1 ->
        evalCont (env.Add(var,v1)) body cont)
    | Num(n)             ->
        cont(n)
    | Var(var)           ->
        cont(env.[var])

let evalC env expr = evalCont env expr (fun x -> x)
