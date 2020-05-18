#light

// ----------------------------
// Listing 8-6.

open System
open System.IO

type LineChooser(fileName1, fileName2) =
    let file1 = File.OpenText(fileName1)
    let file2 = File.OpenText(fileName2)
    let rnd = new System.Random()

    let mutable disposed = false
    let cleanup() =
        if not disposed then
            disposed <- true;
            file1.Dispose();
            file2.Dispose();

    interface System.IDisposable with
        member x.Dispose() = cleanup()

    member obj.CloseAll() = cleanup()

    member obj.GetLine() =
        if not file1.EndOfStream &&
           (file2.EndOfStream  or rnd.Next() % 2 = 0) then file1.ReadLine()
        elif not file2.EndOfStream then file2.ReadLine()
        else raise (new EndOfStreamException())

do File.WriteAllLines("test1.txt", [| "Daisy, Daisy"; "Give me your hand oh do" |])

do File.WriteAllLines("test2.txt", [| "I'm a little teapot"; "Short and stout" |])

let chooser = new LineChooser ("test1.txt", "test2.txt")

chooser.GetLine()

chooser.GetLine()

(chooser :> IDisposable).Dispose()

chooser.GetLine()

