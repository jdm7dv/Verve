% Ackermann function

ackermann(0, N, A) :- 
  !,
  A is N + 1.

ackermann(M, 0, A) :-
  M > 0,
  !,
  M1 is M - 1,
  ackermann(M1, 1, A).

ackermann(M, N, A) :-
  M > 0,
  N > 0,
  !,
  M1 is M - 1,
  N1 is N - 1,
  ackermann(M, N1, T),
  ackermann(M1, T, A).

/*
?- ['ackermann.pl'].
% ackermann.pl compiled 0.00 sec, 1,112 bytes

Yes
?- ackermann(0, 0, A).

A = 1

Yes

?- ackermann(1, 6, A).

A = 8

Yes

?- ackermann(2, 3, A).

A = 9

Yes

?- ackermann(3, 3, A).

A = 61

Yes

?- ackermann(4, 3, A).
ERROR: Out of local stack
?-
*/

