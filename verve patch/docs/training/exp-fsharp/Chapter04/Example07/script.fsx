#light

let parents = [("Adam",None); ("Cain",Some("Adam","Eve"))];;

match System.Environment.GetEnvironmentVariable("PATH") with
| null -> printf "the environment variable PATH is not defined\n"
| res -> printf "the environment variable PATH is set to %s\n" res;;

let switchOnType (a:obj) =
    match a with
    | null -> printf "null!"
    | :? System.Exception as e -> printf "An exception: %s!" e.Message
    | :? System.Int32 as i -> printf "An integer: %d!" i
    | :? System.DateTime as d -> printf "A date/time: %O!" d
    | _ -> printf "Some other kind of object\n";;

