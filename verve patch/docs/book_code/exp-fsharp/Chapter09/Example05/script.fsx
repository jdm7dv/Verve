#light

open System.Drawing

// ----------------------------
// Listing 9-2.

type SceneWithCachedBoundingBox =
    | Ellipse of RectangleF
    | Rect    of RectangleF
    | CompositeRepr   of SceneWithCachedBoundingBox list * RectangleF option ref

    member x.BoundingBox =
        match x with
        | Ellipse(rect) | Rect(rect) -> rect
        | CompositeRepr(scenes,cache) ->
            match !cache with
            | Some v -> v
            | None ->
                let bbox =
                    scenes
                    |> List.map (fun s -> s.BoundingBox)
                    |> List.fold1_left (fun r1 r2 -> RectangleF.Union(r1,r2))
                cache := Some bbox
                bbox

    // Create a Composite node with an initially empty cache
    static member Composite(scenes)  = CompositeRepr(scenes,ref None)
