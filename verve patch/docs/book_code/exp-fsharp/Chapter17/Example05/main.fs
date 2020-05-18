#light

open System

type FSCOMComponent =
    new() as x = {}
    member x.HelloWorld() = "Hello world from F#!"