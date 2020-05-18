#light

type outstate = System.IO.BinaryWriter
type instate  = System.IO.BinaryReader

type pickler<'a> = 'a -> outstate -> unit
type unpickler<'a> = instate -> 'a

let byteP (b: byte) (st: outstate) = st.Write(b)
let byteU (st: instate) = st.ReadByte()

let boolP b st = byteP (if b then 1uy else 0uy) st
let boolU st = let b = byteU st in (b = 1uy) 

let int32P i st = 
    byteP (byte (i &&& 0xFF)) st
    byteP (byte ((i >>> 8) &&& 0xFF)) st
    byteP (byte ((i >>> 16) &&& 0xFF)) st
    byteP (byte ((i >>> 24) &&& 0xFF)) st

let int32U st = 
    let b0 = int (byteU st)
    let b1 = int (byteU st)
    let b2 = int (byteU st)
    let b3 = int (byteU st)
    b0 ||| (b1 <<< 8) ||| (b2 <<< 16) ||| (b3 <<< 24)

let tup2P p1 p2 (a, b) (st: outstate) = 
    (p1 a st : unit)
    (p2 b st : unit)

let tup3P p1 p2 p3 (a, b, c) (st: outstate) = 
    (p1 a st : unit)
    (p2 b st : unit)
    (p3 c st : unit)

let tup2U p1 p2 (st: instate) = 
    let a = p1 st
    let b = p2 st
    (a, b)

let tup3U p1 p2 p3 (st: instate) =
    let a = p1 st
    let b = p2 st
    let c = p3 st
    (a, b, c)

// Outputs a list into the given output stream by pickling each element via f.
let rec listP f lst st =
    match lst with 
    | [] -> byteP 0uy st
    | h :: t -> byteP 1uy st; f h st; listP f t st      

// Reads a list from a given input stream by unpickling each element via f.
let listU f st = 
    let rec ulist_aux acc = 
        let tag = byteU st 
        match tag with
        | 0uy -> List.rev acc
        | 1uy -> let a = f st in ulist_aux (a::acc)  
        | n -> failwithf "listU: found number %d" n
    ulist_aux [] 

type format = list<int32 * bool>
let formatP = listP (tup2P int32P boolP) 
let formatU = listU (tup2U int32U boolU) 

open System.IO

let writeData file data = 
    use outStream = new BinaryWriter(File.OpenWrite(file))
    formatP data outStream
    
let readData file  = 
    use inStream = new BinaryReader(File.OpenRead(file))
    formatU inStream    

// ----------------------------

writeData "out.bin" [(102, true); (108, false)] ;;

readData "out.bin"
