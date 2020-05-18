#light

// ----------------------------

let line = "Smith, John, 20 January 1986, Software Developer"

line.Split [| ',' |];;

line.Split [| ',' |] |> Array.map (fun s -> s.Trim())

// ----------------------------

let splitLine (line: string) = 
    line.Split [| ',' |] |> Array.map (fun s -> s.Trim())

let parseEmployee (line: string) =
    match line.Split [| ',' |] with
    | [| last; first; startDate; title |] ->
        last, first, System.DateTime.Parse(startDate), title
    | _ ->
        failwithf "invalid employee format: '%s'" line

// ----------------------------

parseEmployee line

// ----------------------------

open System.IO

let readEmployees (fileName : string) = 
    Seq.generate_using
       (fun () -> File.OpenText(fileName))
       (fun reader -> 
             if reader.EndOfStream then None
             else Some(parseEmployee(reader.ReadLine()) ))

// ----------------------------

File.WriteAllLines("employees.txt", Array.create 10000 line)

let firstThree = readEmployees("employees.txt") |> Seq.take 3

for (last,first,startDate,title) in firstThree do
    printfn "%s %s started on %A" first last startDate

// ----------------------------

open System.Text.RegularExpressions

let parseHttpRequest line =
    let result = Regex.Match(line, @"GET (.*?) HTTP/1\.([01])$")
    let file = result.Groups.Item(1).Value
    let version = result.Groups.Item(2).Value
    file, version
