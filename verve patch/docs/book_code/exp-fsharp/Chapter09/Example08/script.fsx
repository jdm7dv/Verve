#light

// ----------------------------
// Listing 9-4.

let (|Named|Array|Ptr|Param|) (typ : System.Type) =
    if typ.IsGenericType
    then Named(typ.GetGenericTypeDefinition(),typ.GetGenericArguments())
    elif typ.IsGenericParameter then Param(typ.GenericParameterPosition)
    elif not typ.HasElementType then Named(typ, [| |])
    elif typ.IsArray then Array(typ.GetElementType(),typ.GetArrayRank())
    elif typ.IsByRef then Ptr(true,typ.GetElementType())
    elif typ.IsPointer then Ptr(false,typ.GetElementType())
    else failwith "MSDN says this can't happen"

open System

let rec formatType typ =
    match typ with
    | Named (con, [| |]) -> sprintf "%s" con.Name
    | Named (con, args) -> sprintf "%s<%s>" con.Name (formatTypes args)
    | Array (arg, rank) -> sprintf "Array(%d,%s)" rank (formatType arg)
    | Ptr(true,arg) -> sprintf "%s&" (formatType arg)
    | Ptr(false,arg) -> sprintf "%s*" (formatType arg)
    | Param(pos) -> sprintf "!%d" pos
and formatTypes typs =
    String.Join(",", Array.map formatType typs)

let rec freeVarsAcc typ acc =
    match typ with
    | Array (arg, rank) -> freeVarsAcc arg acc
    | Ptr (_,arg) -> freeVarsAcc arg acc
    | Param _ -> (typ :: acc)
    | Named (con, args) -> Array.fold_right freeVarsAcc args acc
let freeVars typ = freeVarsAcc typ []

