#light

// Listing 7-1

type IPeekPoke =
    abstract member Peek: unit -> int
    abstract member Poke: int -> unit

let Counter(initialState) =
    let state = ref initialState
    { new IPeekPoke with
        member x.Poke(n) = state := !state + n
        member x.Peek() = !state };;

// Listing 7-2

type TicketGenerator() =
    // Note: let bindings in a type definition are implicitly private to the object
    // being constructed. Members are implicitly public.
    let mutable count = 0

    member x.Next() =
        count <- count + 1;
        count

    member x.Reset () =
        count <- 0;;


///////////////////////

type IStatistic<'a,'b> =
    abstract Record : 'a -> unit
    abstract Value : 'b

let Averager(toFloat: 'a -> float) =
    let count = ref 0
    let total = ref 0.0
    { new IStatistic<'a,float> with
          member stat.Record(x) = incr count; total := !total + toFloat x
          member stat.Value = (!total / float !count) };;


// Listing 7-3

open System

module public VisitorCredentials =
    /// The internal table of permitted visitors and the
    /// days they are allowed to visit.
    let private visitorTable =
        dict [ ("Anna", set [DayOfWeek.Tuesday; DayOfWeek.Wednesday]);
               ("Carolyn", set [DayOfWeek.Friday]) ]

    /// This is the function to check if a person is a permitted visitor.
    /// Note: this is public and can be used by external code
    let public Check(person) =
        visitorTable.ContainsKey(person) &&
        visitorTable.[person].Contains(DateTime.Today.DayOfWeek)
    
    /// This is the function to return all known permitted visitors.
    /// Note: this is internal and can only be used by code in this assembly.
    let internal AllKnownVisitors() =
        visitorTable.Keys;;

// Listing 7-4

module public GlobalClock =
    type TickTock = Tick | Tock
    type time = float
    let private clock = ref Tick
    let (private fireTickEvent,public TickEvent) = IEvent.create<TickTock>()
    let internal oneTick() =
        (clock := match !clock with Tick -> Tock | Tock -> Tick)
        fireTickEvent (!clock)

module internal TickTockDriver =
    open System.Threading
    let timer = new Timer(callback=(fun _ -> GlobalClock.oneTick()),
                          state=null,dueTime=0,period=100)

////////////////////////////////

    let (private fireTickEvent,public TickEvent) = IEvent.create<GlobalClock.TickTock>();;

module TickTockListener =
    do GlobalClock.TickEvent.Add(function
        | GlobalClock.Tick -> printf "tick!"
        | GlobalClock.Tock -> System.Windows.Forms.MessageBox.Show "tock!" |> ignore);;

// Listing 7-5

open System.Collections.Generic

type public SparseVector() =
    let elems = new SortedDictionary<int,float>()

    member internal v.Add(k,v) = elems.Add(k,v)

    member public v.Count = elems.Keys.Count
    member v.Item
        with public get i =
            if elems.ContainsKey(i) then elems.[i]
            else 0.0
        and internal set i v =
            elems.[i] <- v;;

////////////////////////////////
