#light

let sixty = lazy (30+30)

sixty.Force()

let sixtyWithSideEffect = lazy (printfn "Hello world"; 30+30)

sixtyWithSideEffect.Force()

sixtyWithSideEffect.Force()

