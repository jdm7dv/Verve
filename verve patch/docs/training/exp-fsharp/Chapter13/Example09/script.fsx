#light

// ----------------------------
// Listing 13-8.

open System

let trylet f x = (try Choice2_1 (f x) with exn -> Choice2_2(exn))

let protect cont econt f x =
    match trylet f x with
    | Choice2_1 v -> cont v
    | Choice2_2 exn -> econt exn

type System.IO.Stream with
    member stream.ReadAsync (buffer,offset,count) =
       Async.Primitive (fun (cont,econt) ->
          stream.BeginRead
              (buffer=buffer,
               offset=offset,
               count=count,
               state=null,
               callback=AsyncCallback(protect cont econt stream.EndRead))
            |> ignore)
