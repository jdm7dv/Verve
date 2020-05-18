#light

let inp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>
           <Scene>
              <Composite>
                <Circle radius='2' x='1' y='0'/>
                <Composite>
                  <Circle radius='2' x='4' y='0'/>
                  <Square side='2' left='-3' top='0'/>
                </Composite>
                <Ellipse top='2' left='-2' width='3' height='4'/>
              </Composite>
           </Scene>"

open System.Xml

let doc = new XmlDocument()
doc.LoadXml(inp)

// ----------------------------
// Listing 9-1.

open System.Xml
open System.Drawing

type Scene =
    | Ellipse of RectangleF
    | Rect    of RectangleF
    | Composite   of Scene list

    /// A derived constructor
    static member Circle(center:PointF,radius) =
        Ellipse(RectangleF(center.X-radius,center.Y-radius,
                           radius*2.0f,radius*2.0f))

    /// A derived constructor
    static member Square(left,top,side) =
        Rect(RectangleF(left,top,side,side))

let extractFloat32 attrName (attribs: XmlAttributeCollection) =
    Float32.of_string(attribs.GetNamedItem(attrName).Value)

let extractPointF (attribs: XmlAttributeCollection) =
    PointF(extractFloat32 "x" attribs,extractFloat32 "y" attribs)

let extractRectangleF (attribs: XmlAttributeCollection) =
    RectangleF(extractFloat32 "left" attribs,extractFloat32 "top" attribs,
               extractFloat32 "width" attribs,extractFloat32 "height" attribs)

let rec extractScene (node: XmlNode) =
    let attribs = node.Attributes
    let childNodes = node.ChildNodes
    match node.Name with
    | "Circle"    ->
        Scene.Circle(extractPointF(attribs), extractFloat32 "radius" attribs)
    | "Ellipse"   ->
        Scene.Ellipse(extractRectangleF(attribs))
    | "Rectangle" ->
        Scene.Rect(extractRectangleF(attribs))
    | "Square"    ->
        Scene.Square(extractFloat32 "left" attribs,extractFloat32 "top" attribs,
                     extractFloat32 "side" attribs)
    | "Composite" ->
        Scene.Composite [ for child in childNodes -> extractScene(child) ]
    | _ -> failwithf "unable to convert XML '%s'" node.OuterXml

let extractScenes (doc: XmlDocument) =
   [ for node in doc.ChildNodes do
       if node.Name = "Scene" then
          yield (Composite
                     [ for child in node.ChildNodes -> extractScene(child) ]) ]

fsi.AddPrinter(fun (r:RectangleF) ->
      sprintf "[%A,%A,%A,%A]" r.Left r.Top r.Width r.Height)

extractScenes doc
