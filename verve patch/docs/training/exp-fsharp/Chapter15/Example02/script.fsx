#light

open System.Data
open System.Data.SqlClient

let connString = @"Server='.\SQLEXPRESS';Integrated Security=SSPI"
let conn = new SqlConnection(connString)

conn.Open()

let execNonQuery conn s =
    let comm = new SqlCommand(s, conn, CommandTimeout = 10)
    comm.ExecuteNonQuery() |> ignore

execNonQuery conn "CREATE DATABASE company"

execNonQuery conn "CREATE TABLE Employees (
    EmpID int NOT NULL,
    FirstName varchar(50) NOT NULL,
    LastName varchar(50) NOT NULL,
    Birthday datetime,
    PRIMARY KEY (EmpID))"

execNonQuery conn "INSERT INTO Employees (EmpId, FirstName, LastName, Birthday)
    VALUES (1001, 'Joe', 'Smith', '02/14/1965')"

execNonQuery conn "INSERT INTO Employees (EmpId, FirstName, LastName, Birthday)
    VALUES (1002, 'Mary', 'Jones', '09/15/1985')"

execNonQuery conn "
    CREATE PROCEDURE dbo.GetEmployeesByLastName 
            (
            @Name nvarchar(10)
            )
    AS
        SELECT
            Employees.FirstName, Employees.LastName
        FROM Employees 
        WHERE Employees.LastName LIKE @Name"

let query() = 
    seq { use conn = new SqlConnection(connString)
          do conn.Open()
          use comm = new SqlCommand("SELECT FirstName, Birthday FROM Employees",
                                     conn)
          use reader = comm.ExecuteReader() 
          while reader.Read() do
              yield (reader.GetString 0, reader.GetDateTime 1)  }

// ----------------------------

fsi.AddPrinter(fun (d: System.DateTime) -> d.ToString())

// Choose one command to execute below
// query()
// query() |> Seq.iter (fun (fn, bday) -> printfn "%s has birthday %O" fn bday)
// query()
//    |> Seq.filter (fun (nm, bday) -> bday < System.DateTime.Parse("01/01/1985"))
//    |> Seq.length

// ----------------------------

let dataAdapter = new SqlDataAdapter()

let buildDataSet conn queryString =
    dataAdapter.SelectCommand <- new SqlCommand(queryString, conn)
    let dataSet = new DataSet()
    // This line is needed to configure the command
    let _ = new SqlCommandBuilder(dataAdapter)
    dataAdapter.Fill(dataSet) |> ignore  // ignore the number of records returned
    dataSet

let dataSet = 
    buildDataSet conn "SELECT EmpID, FirstName, LastName, Birthday from Employees"

let table = dataSet.Tables.Item(0)

let printRows() =
    for row in table.Rows do
        printfn "%O, %O - %O" 
            (row.Item "LastName") 
            (row.Item "FirstName")
            (row.Item "Birthday")
printRows()

let row = table.NewRow()
row.Item("EmpID") <- 1003
row.Item("FirstName") <- "Eve"
row.Item("LastName") <- "Smith"
row.Item("Birthday") <- System.DateTime.Today
table.Rows.Add row
dataAdapter.Update(dataSet) |> ignore  // ignore the number of affected rows

// ----------------------------

open System.IO

let dataSet2 = buildDataSet conn "SELECT * FROM Employees"
let file name = Path.Combine(@"c:\fsharp", name)

do File.WriteAllText(file "employees.xsd", dataSet2.GetXmlSchema())

// ----------------------------

#r @"employees.dll"

let employeesTable = new Employees.Data.NewDataSet()

dataAdapter.Fill(employeesTable) |> ignore // ignore the number of records

let printEmps() =
    for emp in employeesTable._Table do
        printfn "%s, %s - %O" emp.LastName emp.FirstName emp.Birthday
printEmps()

// ----------------------------

let GetEmployeesByLastName (name: string) =
    use comm = new SqlCommand("GetEmployeesByLastName", conn,
                              CommandType=CommandType.StoredProcedure)
    comm.Parameters.AddWithValue("@Name", name) |> ignore
    use adapter = new SqlDataAdapter(comm)
    let table = new DataTable()
    adapter.Fill(table) |> ignore
    table

let printSmiths() =
    for row in (GetEmployeesByLastName "Smith").Rows do
        printfn "row = %O, %O" (row.Item("FirstName")) (row.Item("LastName"))
printSmiths()

// ----------------------------

open System.Windows.Forms

let emps = GetEmployeesByLastName "Smith"
let grid = new DataGrid(Width=300, Height=200, DataSource=emps)
let form = new Form(Visible=true, TopMost=true)
form.Controls.Add(grid)
