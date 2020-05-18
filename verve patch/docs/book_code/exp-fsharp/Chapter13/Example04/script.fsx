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

// ----------------------------
// Listing 13-4.

open System.Drawing
open System.Windows.Forms

let form = new Form(Visible=true,TopMost=true)

let panel = new FlowLayoutPanel(Visible=true,
                                Height = 20,
                                Dock=DockStyle.Bottom,
                                BorderStyle=BorderStyle.FixedSingle)

let progress = new ProgressBar(Visible=false,
                               Anchor=(AnchorStyles.Bottom ||| AnchorStyles.Top),
                               Value=0)

let text = new Label(Text="Paused",
                     Anchor=AnchorStyles.Left,
                     Height=20,
                     TextAlign= ContentAlignment.MiddleLeft)

panel.Controls.Add(progress)
panel.Controls.Add(text)
form.Controls.Add(panel)

let fibOneStep (fibPrevPrev:bigint,fibPrev) = (fibPrev, fibPrevPrev+fibPrev)

// Run the iterative algorithm 500 times before reporting intermediate results
// Burn some additional cycles to make sure it runs slowly enough
let rec RepeatN n f s = if n <= 0 then s else RepeatN (n-1) f (f s)
let rec BurnN n f s = if n <= 0 then f s else ignore (f s); BurnN (n-1) f s
let step = (RepeatN 500 (BurnN 1000 fibOneStep))

// Create the iterative worker.
let worker = new IterativeBackgroundWorker<_>(step,(1I,1I),100)

worker.ProgressChanged.Add(fun (progressPercentage,state)->
    progress.Value <- progressPercentage)

worker.WorkerCompleted.Add(fun (_,result) ->
    progress.Visible <- false;
    text.Text <- "Paused";
    MessageBox.Show(sprintf "Result = %A" result) |> ignore)

worker.WorkerCancelled.Add(fun () ->
    progress.Visible <- false;
    text.Text <- "Paused";
    MessageBox.Show(sprintf "Cancelled OK!") |> ignore)

worker.WorkerError.Add(fun exn ->
    text.Text <- "Paused";
    MessageBox.Show(sprintf "Error: %A" exn) |> ignore)

form.Menu <- new MainMenu()
let workerMenu = form.Menu.MenuItems.Add("&Worker")

workerMenu.MenuItems.Add(new MenuItem("Run",onClick=(fun _ args ->
    text.Text <- "Running";
    progress.Visible <- true;
    worker.RunWorkerAsync())))

workerMenu.MenuItems.Add(new MenuItem("Cancel",onClick=(fun _ args ->
    text.Text <- "Cancelling";
    worker.CancelAsync())))

form.Closed.Add(fun _ -> worker.CancelAsync())
