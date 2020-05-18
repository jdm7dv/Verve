#light

let xobj = (1 :> obj);;

let sobj = ("abc" :> obj);;

let boxedObject = box "abc";;

let downcastString = (boxedObject :?> string);;

let xobj = box 1;;

let x = (xobj :?> string);;

let checkObject (x: obj) =
    match x with
    | :? string -> printfn "The object is a string"
    | :? int -> printfn "The object is an integer"
    | _ -> printfn "The input is something else";;

checkObject (box "abc");;

let reportObject (x: obj) =
    match x with
    | :? string as s -> printfn "The input is the string '%s'" s
    | :? int as d -> printfn "The input is the integer '%d'" d
    | _ -> printfn "the input is something else";;

reportObject (box 17);;

open System.Windows.Forms

let setTextOfControl (c : #Control) (s:string) = c.Text <- s;;

let setTextOfControl (c : 'a when 'a :> Control) (s:string) = c.Text <- s;;

open System
open System.IO

let textReader =
    if DateTime.Today.DayOfWeek = DayOfWeek.Monday
    then Console.In
    else File.OpenText("input.txt");;

let textReader =
    if DateTime.Today.DayOfWeek = DayOfWeek.Monday
    then Console.In
    else (File.OpenText("input.txt") :> TextReader);;

