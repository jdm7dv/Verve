#light

open System.Runtime.InteropServices

module CInterop =
    [<DllImport("CInteropDLL", CallingConvention=CallingConvention.Cdecl)>]
    extern void HelloWorld()

CInterop.HelloWorld()