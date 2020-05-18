#light

// ----------------------------
// Listing 9-3.

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

open PropOps

True

And(Var("x"),Var("y"))

getRepr(it)

And(Var("x"),Var("y"))

// ----------------------------
