#light

seq {0 .. 2};;

seq {-100.0 .. 100.0};;

seq {1I .. 1000000000000I};;

seq { 1 .. 2 .. 5 };;

seq { 1 .. -2 .. -5 };;

seq { 0 .. 2 .. 5 };;

let range = seq {0 .. 2 .. 6};;

for i in range do
    printfn "i = %d" i;;

let range = seq {0 .. 10};;

range |> Seq.map (fun i -> (i,i*i));;

open System.IO

let rec allFiles dir =
    Seq.append
        (dir |> Directory.GetFiles)
        (dir |> Directory.GetDirectories |> Seq.map allFiles |> Seq.concat)

allFiles @"c:\WINDOWS\system32";;

let squares = seq { for i in 0 .. 10 -> (i,i*i) };;

squares;;

seq { for (i,isquared) in squares -> (i,isquared,i*isquared) };;

seq { for Some(nm) in [ Some("James"); None; Some("John") ] -> nm.Length };;

let checkerboardCoordinates n =
    seq { for row in 1 .. n do
              for col in 1 .. n do
                  if (row+col) % 2 = 0 then
                      yield (row,col) };;

checkerboardCoordinates 3;;

let fileInfo dir =
    seq { for file in Directory.GetFiles(dir) do
              let creationTime = File.GetCreationTime(file)
              let lastAccessTime = File.GetLastAccessTime(file)
              yield (file,creationTime,lastAccessTime) };;

let rec allFiles dir =
    seq { for file in Directory.GetFiles(dir) -> file
          for subdir in Directory.GetDirectories dir ->> (allFiles subdir) };;

[ 1 .. 4 ];;

[ for i in 0 .. 3 -> (i,i*i) ];;

[| for i in 0 .. 3 -> (i,i*i) |];;
