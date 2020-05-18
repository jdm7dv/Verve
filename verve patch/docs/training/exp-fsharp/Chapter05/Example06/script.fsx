#light

let transformData inp =
    inp |> Seq.map (fun (x,y) -> (x,y.Length)) ;;
    
let transformData inp =
    inp |> Seq.map (fun (x,y:string) -> (x,y.Length));;

let printSecondElements (inp : #seq<'a * int>) =
    inp
    |> Seq.iter (fun (x,y) -> printfn "y = %d" x);;

type PingPong = Ping | Pong

let printSecondElements (inp : #seq<PingPong * int>) =
    inp
    |> Seq.iter (fun (x,y) -> printfn "y = %d" x);;

let empties = Array.create 100 [];;

let emptyList = [];;
let initialLists = ([],[2]);;
let listOfEmptyLists = [[];[]];;
let makeArray () = Array.create 100 [];;

let empties = Array.create 100 [];;

let empties : int list [] = Array.create 100 [];;

let mapFirst = List.map fst;;

let mapFirst inp = List.map fst inp;;

let mapFirst inp = inp |> List.map (fun (x,y) -> x);;

let printFstElements = List.map fst >> List.iter (printf "res = %d");;

let printFstElements inp = inp |> List.map fst |> List.iter (printf "res = %d");;

let empties = Array.create 100 [];;

let empties () = Array.create 100 [];;
let intEmpties : int list [] = empties();;
let stringEmpties : string list [] = empties();;

let emptyLists = Seq.init_finite 100 (fun _ -> []);;

let emptyLists<'a> : seq<'a list> = Seq.init_finite 100 (fun _ -> []);;

Seq.length emptyLists;;

emptyLists<int>;;

emptyLists<string>;;

let twice x = (x + x);;

let twiceFloat (x:float) = x + x;;

let threeTimes x = (x + x + x)
let sixTimesInt64 (x:int64) = threeTimes x + threeTimes x;;

