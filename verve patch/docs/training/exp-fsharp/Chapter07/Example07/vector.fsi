#light

namespace Acme.Collections
    type SparseVector with
        new: unit -> SparseVector
        member Add: int * float -> unit
        member Item : int -> float with get
