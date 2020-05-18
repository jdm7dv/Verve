#light

("abc","def") < ("abc","xyz");;

compare (10,30) (10,20);;

compare [10;30] [10;20];;

compare [| 10;30 |] [| 10;20 |];;

compare [| 10;20 |] [| 10;30 |];;

hash 100;;

hash "abc";;

hash (100,"abc");;

any_to_string (Some(100,[1.0;2.0;3.1415]));;

sprintf "result = %A" ([1], [true]);;

box 1;;

box "abc";;

let sobj = box "abc";;

(unbox<string> sobj);;

(unbox sobj : string);;

(unbox sobj : int);;

open System.IO;;
open System.Runtime.Serialization.Formatters.Binary;;

let writeValue outputStream (x: 'a) =
    let formatter = new BinaryFormatter()
    formatter.Serialize(outputStream,box x);;

let readValue inputStream =
    let formatter = new BinaryFormatter()
    let res = formatter.Deserialize(inputStream)
    unbox res;;

let addresses = Map.of_list [ "Jeff", "123 Main Street, Redmond, WA 98052";
                              "Fred", "987 Pine Road, Phila., PA 19116";
                              "Mary", "PO Box 112233, Palo Alto, CA 94301" ];;

let fsOut = new FileStream("Data.dat", FileMode.Create)
writeValue fsOut addresses
fsOut.Close()
let fsIn = new FileStream("Data.dat", FileMode.Open)
let res : Map<string,string> = readValue fsIn
fsIn.Close();;

