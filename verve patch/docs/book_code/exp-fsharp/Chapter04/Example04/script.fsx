#light

let arr = [| 1.0; 1.0; 1.0 |];;

arr.[1];;

arr.[1] <- 3.0;;

arr;;

let (r : int[]) = Array.zero_create 100000000;;

let arr = [| for i in 0 .. 5 -> (i,i*i) |];;

arr;;

let arr = [| for i in 0 .. 5 -> (i,i*i) |];;

arr;;

arr.[1..3];;

arr.[..2];;

arr.[3..];;

type ResizeArray<'a> = System.Collections.Generic.List<'a>;;

let names = new ResizeArray<string>();;

for name in ["Claire"; "Sophie"; "Jane"] do
    names.Add(name);;

names.Count;;

names.[0];;

names.[1];;

names.[2];;

let squares = new ResizeArray<int>(seq { for i in 0 .. 100 -> i*i });;

for x in squares do
    printfn "square: %d" x;;
    
open System.Collections.Generic;;

let capitals = new Dictionary<string, string>();;

capitals.["USA"] <- "Washington";;

capitals.["Bangladesh"] <- "Dhaka";;

capitals.ContainsKey("USA");;

capitals.ContainsKey("Australia");;

capitals.Keys;;

capitals.["USA"];;

for kvp in capitals do
    printf "%s has capital %s\n" kvp.Key kvp.Value;;

let lookupName nm (dict : Dictionary<string,string>) =
    let mutable res = ""
    let foundIt = dict.TryGetValue(nm, &res)
    if foundIt then res
    else failwithf "Didn’t find %s" nm;;

let res = ref "";;

capitals.TryGetValue("Australia", res);;

capitals.TryGetValue("USA", res);;

res;;

capitals.TryGetValue("Australia");;

capitals.TryGetValue("USA");;

open Microsoft.FSharp.Collections;;

let sparseMap = new Dictionary<(int * int), float>();;

sparseMap.[(0,2)] <- 4.0;;

sparseMap.[(1021,1847)] <- 9.0;;

sparseMap.Keys;;

