#light

// ----------------------------
// Listing 13-10.

open Microsoft.FSharp.Control.Mailboxes

let counter =
    MailboxProcessor.Create(fun inbox ->
        let rec loop(n) =
            async { do printfn "n = %d, waiting..." n
                    let! msg = inbox.Receive()
                    return! loop(n+msg) }
        loop(0))

// ----------------------------

counter.Start()

counter.Post(1)

counter.Post(2)

counter.Post(1)
