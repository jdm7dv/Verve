#light

open System.IO

let firstTwoLines(file) =
    { use s = File.OpenText(file)
      yield s.ReadLine()
      yield s.ReadLine() }

do File.WriteAllLines("test1.txt", [| "Es kommt ein Schiff";
                                      "Hoch soll Sie leben" |])

let seq = firstTwoLines("test1.txt")

seq |> Seq.iter (printfn "line = '%s'")
