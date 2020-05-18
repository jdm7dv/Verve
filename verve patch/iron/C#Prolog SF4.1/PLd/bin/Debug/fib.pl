% Fibonacci numbers

qaz(1).

fib( 0,0) :- !.
fib( 1,1) :- !.
fib( N, F) :-
  !,
  N > 1,
  N1 is N - 1,
  fib( N1, F1),
  N2 is N1 - 1,
  fib( N2, F2),
  F is F1 + F2.

try :-
  qaz( A),
  fib( 2, 9).

:- cache(fib).
