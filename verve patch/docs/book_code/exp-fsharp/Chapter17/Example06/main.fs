#light

open System
open System.Runtime.InteropServices

[<ProgId("Hwfs.FSComponent")>]
type FSCOMComponent =
    new() as x = {}
    member x.HelloWorld() = "Hello world from F#!"
