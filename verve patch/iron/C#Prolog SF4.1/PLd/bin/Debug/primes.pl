primes( Limit, Ps) :-
  integers( 2, Limit, Is),
  sift( Is, Ps).

integers( Low, High, [Low|Rest]) :-
  Low =< High,
  !,
  M is Low + 1,
  integers( M, High, Rest).
integers( _, _, []).

sift( [I|Is], [I|Ps]) :-
  !,
  remove( I, Is, New),
  sift( New, Ps).
sift( [], []).

remove( P, [I|Is], Nis0) :-
  I mod P =\= 0,
  !,
  Nis0 = [I|Nis],
  remove( P, Is, Nis).
remove( P,[_|Is], Nis) :-
  !,
  remove( P, Is, Nis).
remove( _, [], []).
