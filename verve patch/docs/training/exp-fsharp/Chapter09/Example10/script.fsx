#light

type Prop =
    | Prop of int
and internal PropRepr =
    | AndRepr of Prop * Prop
    | OrRepr  of Prop * Prop
    | NotRepr of Prop
    | VarRepr of string
    | TrueRepr

open System.Collections.Generic

module PropOps =

    let internal uniqStamp = ref 0
    type internal PropTable() =
        let fwdTable = new Dictionary<PropRepr,Prop>(HashIdentity.Structural)
        let bwdTable = new Dictionary<int,PropRepr>(HashIdentity.Structural)
        member t.ToUnique(repr) =
            if fwdTable.ContainsKey(repr) then fwdTable.[repr]
            else let stamp = incr uniqStamp; !uniqStamp
                 let prop = Prop(stamp)
                 fwdTable.Add(repr,prop)
                 bwdTable.Add(stamp,repr)
                 prop
        member t.FromUnique(Prop(stamp)) =
            bwdTable.[stamp]

    let internal table = PropTable()

    // public construction functions
    let And(p1,p2) = table.ToUnique(AndRepr(p1,p2))
    let Not(p)     = table.ToUnique(NotRepr(p))
    let Or(p1,p2)  = table.ToUnique(OrRepr(p1,p2))
    let Var(p)     = table.ToUnique(VarRepr(p))
    let True       = table.ToUnique(TrueRepr)
    let False = Not(True)

    // deconstruction function
    let getRepr(p) = table.FromUnique(p)

// ----------------------------
// Listing 9-5.

    let (|And|Or|Not|Var|True|) prop =
        match table.FromUnique(prop) with
        | AndRepr(x,y) -> And(x,y)
        | OrRepr(x,y) -> Or(x,y)
        | NotRepr(x) -> Not(x)
        | VarRepr(v) -> Var(v)
        | TrueRepr -> True

open PropOps

let rec showProp prec prop =
    let parenIfPrec lim s = if prec < lim then "(" + s + ")" else s
    match prop with
    | Or(p1,p2)  -> parenIfPrec 4 (showProp 4 p1 + " || " + showProp 4 p2)
    | And(p1,p2) -> parenIfPrec 3 (showProp 3 p1 + " && " + showProp 3 p2)
    | Not(p)     -> parenIfPrec 2 ("not "+showProp 1 p)
    | Var(v)     -> v
    | True       -> "T"

let rec nnf sign prop =
    match prop with
    | And(p1,p2) -> if sign then And(nnf sign p1, nnf sign p2)
                    else Or(nnf sign p1, nnf sign p2)
    | Or(p1,p2)  -> if sign then Or(nnf sign p1, nnf sign p2)
                    else And(nnf sign p1, nnf sign p2)
    | Not(p) -> nnf (not sign) p
    | Var(_) | True -> if sign then prop else Not(prop)

let NNF prop = nnf true prop

// ----------------------------

let t1 = Not(And(Not(Var("x")),Not(Var("y"))))

fsi.AddPrinter(showProp 10)

t1

let t2 = Or(Not(Not(Var("x"))),Var("y"))

t2

(t1 = t2)

NNF t1

NNF t2

NNF t1 = NNF t2

// ----------------------------
