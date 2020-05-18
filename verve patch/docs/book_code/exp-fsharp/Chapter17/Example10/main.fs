#light

open System.Runtime.InteropServices

module CInterop =
    [<DllImport("CInteropDLL", CallingConvention=CallingConvention.Cdecl)>]
    extern int Sum(int i, int j)

printf "Sum(1, 1) = %d\n" (CInterop.Sum(1, 1));