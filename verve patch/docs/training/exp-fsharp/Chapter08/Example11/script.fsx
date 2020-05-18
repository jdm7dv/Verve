#light

// ----------------------------
// Listing 8-7.

open System

type TicketGenerator() =
    let mutable free = []
    let mutable max = 0
    member h.Alloc() =
        match free with
        | [] -> max <- max + 1; max
        | h::t -> free <- t; h
    member h.Dealloc(n:int) =
        printfn "returning ticket %d" n
        free <- n :: free

let ticketGenerator = new TicketGenerator()

type  Customer() =
    let myTicket = ticketGenerator.Alloc()
    let mutable disposed = false
    let cleanup() =
         if not disposed then
             disposed <- true;
             ticketGenerator.Dealloc(myTicket)
    member x.Ticket = myTicket
    interface IDisposable with
         member x.Dispose() = cleanup(); GC.SuppressFinalize(x)
    override x.Finalize() = cleanup()


let bill = new Customer()

bill.Ticket

begin
    use joe = new Customer()
    printfn "joe.Ticket = %d" joe.Ticket
end

begin
    use jane = new Customer()
    printfn "jane.Ticket = %d" jane.Ticket
end

