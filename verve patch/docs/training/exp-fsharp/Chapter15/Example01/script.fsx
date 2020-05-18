#light

let select f seq = Seq.map f seq
let where f seq  = Seq.filter f seq

let people = [| ("Joe", 27, "Sales");  ("Rick", 35, "Marketing");
                ("Mark", 40, "Sales"); ("Rob", 31, "Administration");
                ("Bob", 34, "Marketing") |]

let namesR =
    people |> select (fun (name, age, dept) -> name)
           |> where  (fun name -> name.StartsWith "R")

// ----------------------------

let namesSalesOver30 =
    people |> where  (fun (_, age, _)  -> age >= 30)
           |> where  (fun (_, _, dept) -> dept = "Sales")
           |> select (fun (name, _, _) -> name);;

namesSalesOver30

// ----------------------------

let sortBy comp seq = seq |> Seq.to_list |> List.sort comp |> Seq.of_list
let revOrder i j = -(compare i j)

let rand = System.Random()
let numbers = seq { while true do yield rand.Next(1000) }

numbers |> Seq.filter (fun i -> i % 2 = 0)  // "where"
        |> Seq.truncate 3
        |> sortBy revOrder
        |> Seq.map (fun i -> i, i*i)        // "select"

// ----------------------------

List.fold_left (fun acc x -> acc + x) 0 [4; 5; 6]

Seq.fold (fun acc x -> acc + x) 0.0 [4.0; 5.0; 6.0]

List.fold_right (fun x acc -> min x acc) [4; 5; 6; 3; 5] System.Int32.MaxValue

List.fold_left (+) 0 [4; 5; 6]

Seq.fold (+) 0.0 [4.0; 5.0; 6.0]

List.fold_right min [4; 5; 6; 3; 5] System.Int32.MaxValue

List.fold_right (fst >> min) [(3, "three"); (5, "five")] System.Int32.MaxValue

// ----------------------------

seq { for (name, age, dept) in people 
      when (age >= 30 && dept = "Sales") 
      -> name }

seq { for (name, age, dept) in people do
         if (age >= 30 && dept = "Sales") then
             yield name }

seq { for i in numbers do
         if i % 2 = 0 then
             yield (i, i*i) }
|> Seq.truncate 3
|> sortBy revOrder

