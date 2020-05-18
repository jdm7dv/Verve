#light

// ----------------------------
// Listing 13-2.

open System.ComponentModel
open System.Windows.Forms

/// An IterativeBackgroudWorker follows the BackgroundWorker design pattern
/// but instead of running an arbitrary computation it iterates a function
/// a fixed number of times and reports intermediate and final results.
/// The worker is paramaterized by its internal state type.
///
/// Percentage progress is based on the iteration number. Cancellation checks
/// are made at each iteration. Implemented via an internal BackgroundWorker.
type IterativeBackgroundWorker<'a>(oneStep:('a -> 'a),
                                   initialState:'a,
                                   numIterations:int) =

    let worker =
        new BackgroundWorker(WorkerReportsProgress=true,
                             WorkerSupportsCancellation=true)

    // Create the events that we will later trigger
    let triggerCompleted,completed = IEvent.create()
    let triggerError    ,error     = IEvent.create()
    let triggerCancelled,cancelled = IEvent.create()
    let triggerProgress ,progress  = IEvent.create()

    do worker.DoWork.Add(fun args ->
        // This recursive function represents the computation loop.
        // It runs at "maximum speed", i.e. is an active rather than
        // a reactive process, and can only be controlled by a
        // cancellation signal.
        let rec iterate state i =
            // At the end of the compuation terminate the recursive loop
            if worker.CancellationPending then
               args.Cancel <- true
            elif i < numIterations then
                // Compute the next result
                let state' = oneStep state

                // Report the percentage compuation and the internal state
                let percent = int ((float (i+1)/float numIterations) * 100.0)
                do worker.ReportProgress(percent, box state);

                // Compute the next result
                iterate state' (i+1)
            else
                args.Result <- box state

        iterate initialState 0)

    do worker.RunWorkerCompleted.Add(fun args ->
        if args.Cancelled       then triggerCancelled()
        elif args.Error <> null then triggerError args.Error
        else triggerCompleted (args.Result :?> 'a))

    do worker.ProgressChanged.Add(fun args ->
        triggerProgress (args.ProgressPercentage,(args.UserState :?> 'a)))

    member x.WorkerCompleted  = completed
    member x.WorkerCancelled  = cancelled
    member x.WorkerError      = error
    member x.ProgressChanged  = progress

    // Delegate the remaining members to the underlying worker
    member x.RunWorkerAsync()    = worker.RunWorkerAsync()
    member x.CancelAsync()       = worker.CancelAsync()

let fibOneStep (fibPrevPrev:bigint,fibPrev) = (fibPrev, fibPrevPrev+fibPrev)

// ----------------------------

let worker = new IterativeBackgroundWorker<_>( fibOneStep,(1I,1I),100)

worker.WorkerCompleted.Add(fun result ->
      MessageBox.Show(sprintf "Result = %A" result) |> ignore)

worker.ProgressChanged.Add(fun (percentage, state) ->
    printfn "%d%% complete, state = %A" percentage state)

worker.RunWorkerAsync()

// ----------------------------
