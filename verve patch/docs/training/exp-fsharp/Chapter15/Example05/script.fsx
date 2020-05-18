#light
#I @"c:\Program Files\Reference Assemblies\Microsoft\Framework\v3.5"
#r "System.Xml.Linq.dll"

open System
open System.Xml.Linq 

let xml =
   "<contacts>
      <contact>
         <name>John Smith</name>
         <phone type=\"home\">+1 626-123-4321</phone>
      </contact>
   </contacts>"
let doc = XDocument.Parse xml

doc.Save (file "contacts2.xml")

// ----------------------------

let xname n                    = XName.op_Implicit(n)
let xdoc (el : #seq<XElement>) = new XDocument(Array.map box (Array.of_seq el))
let xelem s el                 = new XElement(xname s, box el)
let xatt  a b                  = new XAttribute(xname a, b) |> box
let xstr  s                    = box s 

let doc2 =
    xdoc
        [ xelem "contacts"
            [ xelem "contact"
                [ (xelem "name" (xstr "John Smith"))
                  (xelem "phone"
                       [ xatt "type" "home"
                         xstr "+1 626-123-4321" ])
                ]
            ]
        ]

// ----------------------------

let contacts = doc.Element(xname "contacts")  // Get the first contact
let printElems() =
    for elem in contacts.Elements() do
        printfn "Tag=%s, Value=%A" elem.Name.LocalName elem.Value
printElems()

// ----------------------------

let elem (e: XElement) s       = e.Element(xname s)
let elemv e s                  = (elem e s).Value

let contactsXml = XElement.Load(file "contacts2.xml")
let contacts = contactsXml.Elements ()

// ----------------------------

contacts |> Seq.filter (fun e -> (elemv e "name").StartsWith "J")
         |> Seq.map (fun e -> (elemv e "name"), (elemv e "phone"))

// ----------------------------

let xml1 =
    xelem "results"
       [ contacts |> Seq.filter  (fun e -> (elemv e "name").StartsWith "J")  ]

let xml2 =
    xelem "results"
       [ for e in contacts do
             if (elemv e "name").StartsWith "J" then
                 yield e ]
