#light

let failingTask = async { do failwith "fail" }

Async.Run failingTask

let failingTasks = [ async { do failwith "fail A" }
                     async { do failwith "fail B" }; ]

Async.Run (Async.Parallel failingTasks)

Async.Run (Async.Parallel failingTasks)

Async.Run (Async.Catch failingTask)
