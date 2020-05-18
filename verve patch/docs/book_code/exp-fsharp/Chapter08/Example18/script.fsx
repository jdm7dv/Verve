#light

// ----------------------------
// Listing 8-13.

open System
open System.Windows.Forms

type RandomTicker(approxInterval) =
    let timer = new Timer()
    let rnd = new System.Random(99)
    let triggerTickEvent, tickEvent = IEvent.create()

    let chooseInterval() :int =
        approxInterval + approxInterval/4 - rnd.Next(approxInterval/2)

    do timer.Interval <- chooseInterval()

    do timer.Tick.Add(fun args ->
        let interval = chooseInterval()
        triggerTickEvent(interval);
        timer.Interval <- interval)

    member x.RandomTick = tickEvent
    member x.Start() = timer.Start()
    member x.Stop() = timer.Stop()
    interface IDisposable with
        member x.Dispose() = timer.Dispose()


let rt = new RandomTicker(1000)

rt.RandomTick.Add(fun nextInterval -> printfn "Tick, next = %A" nextInterval)

rt.Start()
