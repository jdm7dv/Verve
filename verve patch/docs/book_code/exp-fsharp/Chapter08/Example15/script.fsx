#light

type Tree =
    | Node of string * Tree * Tree
    | Tip of string

let rec sizeNotTailRecursive tree =
    match tree with
    | Tip _ -> 1
    | Node(_,treeLeft,treeRight) ->
        sizeNotTailRecursive treeLeft + sizeNotTailRecursive treeRight

let rec mkBigUnbalancedTree n tree =
    if n = 0 then tree
    else Node("node",Tip("tip"),mkBigUnbalancedTree (n-1) tree)

let tree1 = Tip("tip")
let tree2 = mkBigUnbalancedTree 10000 tree1
let tree3 = mkBigUnbalancedTree 10000 tree2
let tree4 = mkBigUnbalancedTree 10000 tree3
let tree5 = mkBigUnbalancedTree 10000 tree4
let tree6 = mkBigUnbalancedTree 10000 tree5

let rec sizeAcc acc tree =
    match tree with
    | Tip _ -> 1+acc
    | Node(_,treeLeft,treeRight) ->
        let acc = sizeAcc acc treeLeft
        sizeAcc acc treeRight

let size tree = sizeAcc 0 tree

// ----------------------------
// Listing 8-10.

let rec sizeCont tree cont =
    match tree with
    | Tip _ -> cont 1
    | Node(_,treeLeft,treeRight) ->
        sizeCont treeLeft (fun leftSize ->
          sizeCont treeRight (fun rightSize ->
            cont (leftSize + rightSize)))

let sizeC tree = sizeCont tree (fun x -> x)

// ----------------------------
// Listing 8-10.

let rec sizeContAcc acc tree cont =
    match tree with
    | Tip _ -> cont (1+acc)
    | Node(_,treeLeft,treeRight) ->
        sizeContAcc acc treeLeft (fun accLeftSize ->
        sizeContAcc accLeftSize treeRight cont)

let sizeA tree = sizeContAcc 0 tree (fun x -> x)
