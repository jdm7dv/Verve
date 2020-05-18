#light

open System
open NUnit.Framework
open Debug

let isPalindrome (str:string) =
    let rec check(s:int, e:int) =
        if s = e || s = e + 1 then true
        else if str.[s] <> str.[e] then false
        else check(s + 1, e - 1)

[<TestFixture;Description("Test fixture for the isPalindrome function")>]
type Test() =
    [<TestFixtureSetUp>]
    member x.InitTestFixture () =
        printfn "Before running Fixture"

    [<TestFixtureTearDown>]
    member x.DoneTestFixture () =
        printfn "After running Fixture"

    [<SetUp>]
    member x.InitTest () =
        printfn "Before running test"

    [<TearDown>]
    member x.DoneTest () =
        Console.WriteLine("After running test")

    [<Test;Category("Special case");Description("An empty string is palindrome")>]
    member x.EmptyString () =
        Assert.IsTrue(isPalindrome(""),
            "isPalindrome must return true on an empty string")
