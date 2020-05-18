#light

open System.IO;;

File.WriteAllLines("test.txt", [| "This is a test file.";
                                  "It is easy to read." |]);;

File.ReadAllLines("test.txt");;

File.ReadAllText("test.txt");;

[ for line in File.ReadAllLines("test.txt") do
    let words = line.Split [| ' ' |]
    if words.Length > 3 && words.[2] = "easy" then
        yield line ];;

let outp = File.CreateText("playlist.txt");;

outp.WriteLine("Enchanted");;

outp.WriteLine("Put your records on");;

outp.Close();;

let inp = File.OpenText("playlist.txt");;

inp.ReadLine();;

inp.ReadLine();;

inp.Close();;

System.Console.WriteLine("Hello World");;

System.Console.ReadLine();;

sprintf "Name: %s, Age: %d" "Anna" 3;;

sprintf "Name: %s, Age: %d" 3 10;;

System.DateTime.Now.ToString();;

sprintf "It is now %O" System.DateTime.Now;;

any_to_string (1,2);;

any_to_string [1;2;3];;

printf "The result is %A\n" [1;2;3];;

let myWriteStringToFile () =
    use outp = File.CreateText(@"playlist.txt")
    outp.WriteLine("Enchanted")
    outp.WriteLine("Put your records on");;

let myWriteStringToFile () =
    using (File.CreateText(@"playlist.txt")) (fun outp ->
    outp.WriteLine("Enchanted")
    outp.WriteLine("Put your records on"));;

let using (ie : #System.IDisposable) f =
    try f(ie)
    finally ie.Dispose();;

