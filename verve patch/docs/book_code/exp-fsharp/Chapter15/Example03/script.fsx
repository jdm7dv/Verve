#light
#I @"c:\Program Files\Reference Assemblies\Microsoft\Framework\v3.5" 
#r "System.Core.dll"
#r "System.Data.Linq.dll" 
#r "FSharp.Linq.dll"
#r "Northwind.dll"

open System
open System.Data.SqlClient
open Nwind

let db =
    let connB = new SqlConnectionStringBuilder()
    connB.AttachDBFilename <- __YOUR_SOURCE_DIRECTORY__ + @"\Northwnd.mdf"
    connB.IntegratedSecurity <- true
    connB.Enlist <- false
    connB.DataSource <- @".\SQLExpress"
    new Northwind(connB.ConnectionString)

// ----------------------------

open Microsoft.FSharp.Quotations.Typed
open Microsoft.FSharp.Data.Linq
open Microsoft.FSharp.Linq.SequenceOps

let res =
    SQL <@ { for emp in (db.Employees) 
                 when emp.BirthDate.Value.Year > 1960
                      && emp.LastName.StartsWith "S" 
                 -> (emp.FirstName, emp.LastName) }
           |> take 5 @>

let printRes() =
    for (first, last) in res do
        printfn "%s %s" first last
printRes()
