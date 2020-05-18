#light

let mutable cell1 = 1;;

cell1;;

cell1 <- 3;;

cell1;;

let sum n m =
    let mutable res = 0
    for i = n to m do
        res <- res + i
    res;;

sum 3 6;;
