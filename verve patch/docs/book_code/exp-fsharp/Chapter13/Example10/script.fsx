#light

// ----------------------------
// Listing 13-9.

let Parallel(taskSeq) =
    Async.Primitive (fun (cont,econt) ->
        let tasks = Seq.to_array taskSeq
        let count = ref tasks.Length
        let results = Array.zero_create tasks.Length
        tasks |> Array.iteri (fun i p ->
            Async.Spawn
               (async { let! res = p
                        do results.[i] <- res;
                        let n = System.Threading.Interlocked.Decrement(count)
                        do if n=0 then cont results })))
