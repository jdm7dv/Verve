#light

// ----------------------------
// Listing 9-11.

open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Typed
open Microsoft.FSharp.Quotations.Raw

type Error = Err of float

let rec errorEstimateAux t (env : Map<_,_>) =

    match t with
    | GenericTopDefnApp <@@ (+) @@> (tyargs,[xt;yt]) ->
        let x,Err(xerr) = errorEstimateAux xt env
        let y,Err(yerr) = errorEstimateAux yt env
        (x+y,Err(xerr+yerr))

    | GenericTopDefnApp <@@ (-) @@> (tyargs,[xt;yt]) ->
        let x,Err(xerr) = errorEstimateAux xt env
        let y,Err(yerr) = errorEstimateAux yt env
        (x-y,Err(xerr+yerr))

    | GenericTopDefnApp <@@ ( * ) @@> (tyargs,[xt;yt]) ->
        let x,Err(xerr) = errorEstimateAux xt env
        let y,Err(yerr) = errorEstimateAux yt env
        (x*y,Err(xerr*abs(x)+yerr*abs(y)+xerr*yerr))

    | GenericTopDefnApp <@@ ( / ) @@> (tyargs,[xt;yt]) ->
        let x,Err(xerr) = errorEstimateAux xt env
        let y,Err(yerr) = errorEstimateAux yt env
        (x/y,Err(xerr*abs(x)+abs(1.0/y)/yerr+xerr/yerr))

    | GenericTopDefnApp <@@ abs @@> (tyargs,[xt]) ->
        let x,Err(xerr) = errorEstimateAux xt env
        (abs(x),Err(xerr))

    | Let((var,vet), bodyt) ->
        let varv,verr = errorEstimateAux vet env
        errorEstimateAux bodyt (env.Add(var.Name,(varv,verr)))

    | App(ResolvedTopDefnUse(info,Lambda(v,body)),arg) ->
        errorEstimateAux  (MkLet((v,arg),body)) env

    | Var(x) -> env.[x]

    | Double(n) -> (n,Err(0.0))

    | _ -> failwithf "unrecognized term: %A" t

let rec errorEstimateRaw (t : Expr) =
    match t with
    | Lambda(x,t) ->
        (fun xv -> errorEstimateAux t (Map.of_seq [(x.Name,xv)]))
    | ResolvedTopDefnUse(info,body) ->
        errorEstimateRaw body
    | _ -> failwithf "unrecognized term: %A - expected a lambda" t

let rec errorEstimate (t : Expr<float -> float>) = errorEstimateRaw t.Raw

// ----------------------------

let (±) x = Err(x)

fsi.AddPrinter (fun (x:float,Err(v)) -> sprintf "%g±%g" x v)

errorEstimate <@ fun x -> x+2.0*x+3.0*x*x @> (1.0,±0.1)

errorEstimate <@ fun x -> let y = x + x in y*y + 2.0 @> (1.0,±0.1)

[<ReflectedDefinition>]
let poly x = x+2.0*x+3.0/(x*x)

errorEstimate <@ poly @> (3.0,±0.1)

errorEstimate <@ poly @> (30271.3,±0.0001)

// ----------------------------
