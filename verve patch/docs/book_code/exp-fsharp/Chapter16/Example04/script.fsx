#light

open SimpleTokenLex
open Lexing

type term =
    | Term  of int * string * int
    | Const of int

type polynomial = term list
type tokenStream = LazyList<token * position * position>

let tryToken (src: tokenStream) =
    match src with 
    | LazyList.Cons ((tok, startPos, endPos), rest) -> Some(tok, rest)
    | _ -> None

let parseIndex src = 
    match tryToken src with 
    | Some (HAT, src) ->
        match tryToken src with
        | Some (INT num2, src) ->
            num2, src
        | _ -> failwith "expected an integer after '^'"
    | _ -> 1, src

let parseTerm src =
    match tryToken src with 
    | Some (INT num, src) ->
        match tryToken src with 
        | Some (ID id, src) ->
           let idx, src = parseIndex src
           Term (num, id, idx), src
        | _ -> Const num, src
    | Some (ID id, src) ->
         let idx, src = parseIndex src
         Term(1, id, idx), src
    | _ -> failwith "end of token stream in term"

let rec parsePolynomial src =
    let t1, src = parseTerm src 
    match tryToken src with  
    | Some (PLUS, src) -> 
        let p2, src = parsePolynomial src 
        (t1 :: p2), src 
    | _ -> [t1], src

let tokenStream inp : tokenStream = 
    let lexbuf = Lexing.from_string inp

    // Generate the token stream as a seq<token>
    Seq.generate (fun () -> lexbuf) (fun lexbuf -> 
        if lexbuf.IsPastEndOfStream then None else 
        match SimpleTokenLex.token lexbuf with 
        | EOF -> None
        | token -> Some(token, lexbuf.StartPos, lexbuf.EndPos))
 
        (fun lexbuf -> ())

    // Convert to a lazy list 
    |> LazyList.of_seq

let parse input = 
    let src = tokenStream input
    let result, src = parsePolynomial src
    match tryToken src with 
    | Some _ -> failwith "unexpected input at end of token stream!"
    | None -> result

// ----------------------------

parse "1+3"

parse "2x^2+3x+5"

// ----------------------------
