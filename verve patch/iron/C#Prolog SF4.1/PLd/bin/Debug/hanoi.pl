:- op( 100, xfx, to).

hanoi( 1, A, B, C, [A to B]).
hanoi( N, A, B, C, Ms) :-
  N > 1,
  succ( N1, N),
  hanoi( N1, A, B, C, M1),
  hanoi( N1, C, B, A, M2),
  append( M1, [A to B|M2], Ms).

go( N, Moves) :-
  hanoi( N, a, b, c, Moves).
