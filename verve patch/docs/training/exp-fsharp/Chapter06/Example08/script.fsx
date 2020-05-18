#light

module NumberTheoryExtensions =
    let isPrime(i) =
        let lim = int(sqrt(float(i)))
        let rec check j =
            j > lim or (i % j <> 0 && check (j+1))
        check 2

    type System.Int32 with
        member i.IsPrime = isPrime(i);;

open NumberTheoryExtensions;;

(3).IsPrime;;

(6093711).IsPrime;;

module List =
    let rec pairwise l =
        match l with
        | [] | [_] -> []
        | h1::(h2::_ as t) -> (h1,h2) :: pairwise t;;

List.pairwise [1;2;3;4];;
