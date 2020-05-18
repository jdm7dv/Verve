#light

module MMap =
    open System
    open System.IO
    open System.Runtime.InteropServices
    open Microsoft.FSharp.NativeInterop
    open Printf

    type HANDLE = nativeint
    type ADDR = nativeint

    [<DllImport("kernel32", SetLastError=true)>]
    extern bool CloseHandle(HANDLE handler)
    
    [<DllImport("kernel32", SetLastError=true, CharSet=CharSet.Auto)>]
    extern HANDLE CreateFile(string lpFileName,
                             int dwDesiredAccess,
                             int dwShareMode,
                             HANDLE lpSecurityAttributes,
                             int dwCreationDisposition,
                             int dwFlagsAndAttributes,
                             HANDLE hTemplateFile)

    [<DllImport("kernel32", SetLastError=true, CharSet=CharSet.Auto)>]
    extern HANDLE CreateFileMapping(HANDLE hFile,
                                    HANDLE lpAttributes,
                                    int flProtect,
                                    int dwMaximumSizeLow,
                                    int dwMaximumSizeHigh,
                                    string lpName)

    [<DllImport("kernel32", SetLastError=true)>]
    extern ADDR MapViewOfFile(HANDLE hFileMappingObject,
                              int dwDesiredAccess,
                              int dwFileOffsetHigh,
                              int dwFileOffsetLow,
                              int dwNumBytesToMap)

    [<DllImport("kernel32", SetLastError=true, CharSet=CharSet.Auto)>]
    extern HANDLE OpenFileMapping(int dwDesiredAccess,
                                  bool bInheritHandle,
                                  string lpName)

    [<DllImport("kernel32", SetLastError=true)>]
    extern bool UnmapViewOfFile(ADDR lpBaseAddress)

    let INVALID_HANDLE = new IntPtr(-1)
    let MAP_READ = 0x0004
    let GENERIC_READ = 0x80000000
    let NULL_HANDLE = IntPtr.Zero
    let FILE_SHARE_NONE = 0x0000
    let FILE_SHARE_READ = 0x0001
    let FILE_SHARE_WRITE = 0x0002
    let FILE_SHARE_READ_WRITE = 0x0003
    let CREATE_ALWAYS = 0x0002
    let OPEN_EXISTING = 0x0003
    let OPEN_ALWAYS = 0x0004
    let READONLY = 0x00000002

    type MemMap<'a> (fileName) =

        let ok =
            match (type 'a) with
            | ty when ty = (type int) -> true
            | ty when ty = (type int32) -> true
            | ty when ty = (type byte) -> true
            | ty when ty = (type sbyte) -> true
            | ty when ty = (type int16) -> true
            | ty when ty = (type uint16) -> true
            | ty when ty = (type int64) -> true
            | ty when ty = (type uint64) -> true
            | _ -> false

        do if not ok then 
                    failwithf "the type %s is not a basic blittable type" ((type 'a).ToString())

        let hFile =
            CreateFile (fileName,
                                    GENERIC_READ,
                                    FILE_SHARE_READ_WRITE,
                                    IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero )

        do if ( hFile.Equals(INVALID_HANDLE) ) then
                 Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        
        let hMap = CreateFileMapping (hFile, IntPtr.Zero, READONLY, 0,0, null )
        do CloseHandle(hFile) |> ignore
        do if hMap.Equals(NULL_HANDLE) then
                 Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

        let start = MapViewOfFile (hMap, MAP_READ,0,0,0)

        do if ( start.Equals(IntPtr.Zero) ) then
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error())

        member m.AddressOf(i: int) : 'a nativeptr =
            NativePtr.of_nativeint(start + Int32.to_nativeint i)

        member m.GetBaseAddress (i:int) : int -> 'a =
            NativePtr.get (m.AddressOf(i))

        member m.Item
            with get(i : int) : 'a = m.GetBaseAddress 0 i

        member m.Close() =
            UnmapViewOfFile(start) |> ignore
            CloseHandle(hMap) |> ignore

        interface IDisposable with
            member m.Dispose() =
                m.Close()

let mm = new MMap.MemMap<byte>("somefile.txt")
printf "%A\n" (mm.[0])
mm.Close()