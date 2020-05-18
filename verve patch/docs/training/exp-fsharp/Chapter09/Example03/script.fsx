#light

// ----------------------------
// Listing 9-1.

open System.Drawing

type Scene =
    | Ellipse     of RectangleF
    | Rect        of RectangleF
    | Composite   of Scene list

    /// A derived constructor
    static member Circle(center:PointF,radius) =
        Ellipse(RectangleF(center.X-radius,center.Y-radius,
                           radius*2.0f,radius*2.0f))

    /// A derived constructor
    static member Square(left,top,side) =
        Rect(RectangleF(left,top,side,side))

let rec flatten scene =
    match scene with
    | Composite(scenes) -> seq { for x in scenes do yield! flatten x }
    | Ellipse _ | Rect _ -> seq { yield scene }

let rec flattenAux scene acc =
    match scene with
    | Composite(scenes) -> List.fold_right flattenAux scenes acc
    | Ellipse _
    | Rect _ -> scene :: acc

let flatten2 scene = flattenAux scene [] |> Seq.of_list

let flatten3 scene =
    let acc = new ResizeArray<_>()
    let rec flattenAux s =
        match s with
        | Composite(scenes) -> scenes.Iterate(flattenAux)
        | Ellipse _ | Rect _ -> acc.Add(s)
    flattenAux scene;
    Seq.readonly acc

let rec rectanglesOnly scene =
    match scene with
    | Composite(scenes) -> Composite(scenes.Map(rectanglesOnly))
    | Ellipse(rect) | Rect(rect) -> Rect(rect)

let rec mapRects f scene =
    match scene with
    | Composite(scenes) -> Composite(scenes.Map(mapRects f))
    | Ellipse(rect) -> Ellipse(f rect)
    | Rect(rect) -> Rect(f rect)

let adjustAspectRatio scene =
    mapRects (fun r -> RectangleF.Inflate(r,1.1f,1.0f/1.1f)) scene

