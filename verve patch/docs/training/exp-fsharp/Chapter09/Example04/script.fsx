#light

// ----------------------------
// Listing 9-1.

open System.Xml
open System.Drawing

type SceneWithDelay =
    | Ellipse   of RectangleF
    | Rect      of RectangleF
    | Composite of SceneWithDelay list
    | Delay     of Lazy<SceneWithDelay>

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
        SceneWithDelay.Circle(extractPointF(attribs), extractFloat32 "radius" attribs)
    | "Ellipse"   ->
        SceneWithDelay.Ellipse(extractRectangleF(attribs))
    | "Rectangle" ->
        SceneWithDelay.Rect(extractRectangleF(attribs))
    | "Square"    ->
        SceneWithDelay.Square(extractFloat32 "left" attribs,extractFloat32 "top" attribs,
                              extractFloat32 "side" attribs)
    | "Composite" ->
        SceneWithDelay.Composite [ for child in childNodes -> extractScene(child) ]
    | "File"   ->
        let file = attribs.GetNamedItem("file").Value
        let scene = lazy (let d = XmlDocument()
                          d.Load(file)
                          extractScene(d :> XmlNode))
        SceneWithDelay.Delay scene
    | _ -> failwithf "unable to convert XML '%s'" node.OuterXml

let extractScenes (doc: XmlDocument) =
   [ for node in doc.ChildNodes do
       if node.Name = "Scene" then
          yield (Composite
                     [ for child in node.ChildNodes -> extractScene(child) ]) ]

let rec getScene scene =
    match scene with
    | Delay d -> getScene (d.Force())
    | _ -> scene

let rec flattenAux scene acc =
    match getScene(scene) with
    | Composite   scenes -> List.fold_right flattenAux scenes acc
    | Ellipse _ | Rect _ -> scene :: acc
    | Delay _ -> failwith "this lazy value should have been eliminated by getScene"

let flatten2 scene = flattenAux scene []

