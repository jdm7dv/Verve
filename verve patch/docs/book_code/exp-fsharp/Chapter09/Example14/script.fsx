#light

// ----------------------------
// Listing 9-10.

open System
open System.IO
open Microsoft.FSharp.Reflection

type ColumnAttribute(col:int) =
    inherit Attribute()
    member x.Column = col

/// SchemaReader builds an object that automatically transforms lines of text
/// files in comma-separated form into instances of the given type 'schema.
/// 'schema must be an F# record type where each field is attributed with a
/// ColumnAttribute attribute, indicating which column of the data the record
/// field is drawn from. This simple version of the reader understands
/// integer, string and DateTime values in the CSV format.
type SchemaReader<'schema>() =

    // Grab the object for the type that describes the schema
    let schemaType = typeof<'schema>

    // Grab the fields from that type
    let fields =
        match Type.GetInfo(schemaType) with
        | RecordType(fields) ->  fields
        | _ ->  failwithf "this schema compiler expects a record type"

    // For each field find the ColumnAttribute and compute a function
    // to build a value for the field
    let schema =
        fields |> List.mapi (fun fldIdx (fieldName,fieldType) ->
            let fieldInfo = schemaType.GetProperty(fieldName)
            let fieldConverter =
                match fieldType with
                |  ty when ty = typeof<string>   -> (fun (s:string) -> box s)
                |  ty when ty = typeof<int>      -> (System.Int32.Parse >> box)
                |  ty when ty = typeof<DateTime> -> (System.DateTime.Parse >> box)
                |  _ -> failwithf "Unknown primitive type %A" fieldType
            let attrib =
                match fieldInfo.GetCustomAttributes(typeof<ColumnAttribute>,
                                                    false) with
                | [| (:? ColumnAttribute as attrib) |] ->   attrib
                | _ -> failwithf "No column attribute found on field %s" fieldName
            (fldIdx,fieldName, attrib.Column, fieldConverter))
        |> List.to_array

    // Compute the permutation defined by the ColumnAttributes indexes
    let columnToFldIdxPermutation =
      Permutation(schema.Length,
                  schema |> Array.map (fun (fldIdx,_,colIdx,_) -> (colIdx,fldIdx)))

    // Drop the parts of the schema we don't need
    let schema =
      schema |> Array.map (fun (_,fldName,_ ,fldConv) -> (fldName,fldConv))

    // Compute a function to build instances of the schema type. This uses an
    // F# library function.
    let objectBuilder = Reflection.Value.GetRecordConstructor(schemaType)

    // OK, now we're ready to implement a line reader
    member reader.ReadLine(textReader: TextReader) =
        let line = textReader.ReadLine()
        let words = line.Split([|','|]) |> Array.map(fun s -> s.Trim())
        if words.Length <> schema.Length then
            failwith "unexpected number of columns in line %s" line
        let words = words |> Array.permute columnToFldIdxPermutation

        let convertColumn colText (fieldName, fieldConverter) =
           try fieldConverter colText
           with e ->
               failwithf "error converting '%s' to field '%s'" colText fieldName

        let obj = objectBuilder (Array.map2 convertColumn words schema)

        // OK, now we know we've dynamically built an object of the right type
        unbox<'schema>(obj)

    // OK, this read an entire file
    member reader.ReadFile(file) =
        seq { use textReader = File.OpenText(file)
              while not textReader.EndOfStream do
                  yield reader.ReadLine(textReader) }


// ----------------------------

type CheeseClub =
    { [<Column(0)>] Name            : string
      [<Column(2)>] FavouriteCheese : string
      [<Column(1)>] LastAttendance  : System.DateTime }

let reader = new SchemaReader<CheeseClub>()

fsi.AddPrinter(fun (c:System.DateTime) -> c.ToString())

System.IO.File.WriteAllLines("data.txt", [| "Steve, 12/03/2007, Cheddar";
                                            "Sally, 18/02/2007, Brie"; |])

reader.ReadFile("data.txt")

// ----------------------------

