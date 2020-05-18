#light

#load "simpleTokensLex.fs"

let lexbuf = Lexing.from_string "3.4 x 34 xyx"

SimpleTokensLex.token lexbuf

(lexbuf.StartPos.Line, lexbuf.StartPos.Column)

(lexbuf.EndPos.Line, lexbuf.EndPos.Column)

SimpleTokensLex.token lexbuf

(lexbuf.StartPos.Line, lexbuf.StartPos.Column)

