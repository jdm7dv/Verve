#light

open System
open NUnit.Framework

let isPalindrome (str:string) =
    let rec check(s:int, e:int) =
        if s = e || s = e + 1 then true
        else if str.[s] <> str.[e] then false
        else check(s + 1, e - 1)

[<TestFixture>]
type Test() =
    let posTests(strings) =
        for s in strings do
        Assert.IsTrue(isPalindrome s,
            sprintf "isPalindrome(\"%s\") must return true" s)
    
    let negTests(strings) =
        for s in strings do
        Assert.IsFalse(isPalindrome s,
            sprintf "isPalindrome(\"%s\") must return false" s)

    [<Test>]
    member x.EmptyString () =
        Assert.IsTrue(isPalindrome(""),
            "isPalindrome must return true on an empty string")
    
    [<Test>]
    member x.SingleChar () = posTests ["a"]

    [<Test>]
    member x.EvenPalindrome () = posTests [ "aa"; "abba"; "abaaba" ]

    [<Test>]
    member x.OddPalindrome () = posTests [ "aba"; "abbba"; "abababa" ]

    [<Test>]
    member x.WrongString () = negTests [ "as"; "F# is wonderful"; "Nice" ]
