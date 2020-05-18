/*
  A grammar example: the langage { a^n b^n c^n } which is known to be context-sensitive
  
  Example:
  
  1 ?- anbncn( L, []).

   L = []  more? (y/n) ;

   L = [a, b, c]  more? (y/n) ;

   L = [a, a, b, b, c, c]  more? (y/n) ;

   L = [a, a, a, b, b, b, c, c, c]  more? (y/n)

  2 ?- 
  
*/
 
anbncn --> 
  n_x( N, a), 
  n_x( N, b), 
  n_x( N, c).
 
n_x( 0, _)    --> [].
n_x( s(N), X) --> [X], n_x( N, X).

