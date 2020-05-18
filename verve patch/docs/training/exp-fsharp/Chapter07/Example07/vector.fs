#light

namespace Acme.Collections
    open System.Collections.Generic
    type SparseVector() =
        let elems = new SortedDictionary<int,float>()
        member v.Add(k,v) = elems.Add(k,v)
        member v.Item
            with get i =
                if elems.ContainsKey(i) then elems.[i]
                else 0.0
            and set i v =
                elems.[i] <- v
