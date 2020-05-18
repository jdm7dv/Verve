#light

let rec map (f : 'a -> 'b) (l : 'a list) =
    match l with
    | h :: t -> f h :: map f t
    | [] -> [];;

let rec map<'a,'b> (f : 'a -> 'b) (l : 'a list) =
    match l with
    | h :: t -> f h :: map f t
    | [] -> [];;
