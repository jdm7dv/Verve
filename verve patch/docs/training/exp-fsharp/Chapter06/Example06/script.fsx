#light

type ITextOutputSink =
    abstract WriteChar : char -> unit
    abstract WriteString : string -> unit;;

let simpleOutputSink(writeCharFunction) =
    { new ITextOutputSink with
          member x.WriteChar(c) = writeCharFunction(c)
          member x.WriteString(s) = s |> String.iter x.WriteChar };;

open System.Text

let stringBuilderOuputSink (buf : StringBuilder ) =
    simpleOutputSink (fun c -> buf.Append(c) |> ignore);;

let buf = new System.Text.StringBuilder();;

let c = stringBuilderOuputSink(buf);;

["Incy"; " "; "Wincy"; " "; "Spider"] |> List.iter c.WriteString;;

buf.ToString();;

type CountingOutputSink(writeCharFunction: char -> unit) =
    let mutable count = 0

    interface ITextOutputSink with
        member x.WriteChar(c) = count <- count + 1; writeCharFunction(c)
        member x.WriteString(s) = s |> String.iter (x :> ITextOutputSink).WriteChar
    
    member x.Count = count;;

type TextOutputSink() =
    abstract WriteChar : char -> unit
    abstract WriteString : string -> unit
    default x.WriteString(s) = s |> String.iter x.WriteChar;;

{ new TextOutputSink() with
      member x.WriteChar(c) = System.Console.Write(c) };;

type HtmlWriter() =
    let mutable count = 0
    let sink =
        { new TextOutputSink() with
              member x.WriteChar(c) =
                  count <- count + 1;
                  System.Console.Write(c) }

    member x.CharCount = count
    member x.WriteHeader() = sink.WriteString("<html>")
    member x.WriteFooter() = sink.WriteString("</html>")
    member x.WriteString(s) = sink.WriteString(s);;

type CountingOutputSinkByInheritance() =
    inherit TextOutputSink()
    let mutable count = 0
    member sink.Count = count
    default sink.WriteChar(c) = count <- count + 1; System.Console.Write(c);;

{ new TextOutputSink() with
      member sink.WriteChar(c) = System.Console.Write(c)
      member sink.WriteString(s) = System.Console.Write(s) };;

open System.Text

type ByteOutputSink() =
    inherit TextOutputSink()
    abstract WriteByte : byte -> unit
    abstract WriteBytes : byte[] -> unit
    default sink.WriteChar(c) = sink.WriteBytes(Encoding.UTF8.GetBytes([|c|]))
    override sink.WriteString(s) = sink.WriteBytes(Encoding.UTF8.GetBytes(s))
    default sink.WriteBytes(b) = b |> Array.iter (fun c -> sink.WriteByte(c));;
