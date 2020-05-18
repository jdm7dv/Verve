/*
 _________________________________________________________________________
|       Copyright (C) 1982                                                |
|                                                                         |
|       David Warren,                                                     |
|               SRI International, 333 Ravenswood Ave., Menlo Park,       |
|               California 94025, USA;                                    |
|                                                                         |
|       Fernando Pereira,                                                 |
|               Dept. of Architecture, University of Edinburgh,           |
|               20 Chambers St., Edinburgh EH1 1JZ, Scotland              |
|                                                                         |
|       This program may be used, copied, altered or included in other    |
|       programs only for academic purposes and provided that the         |
|       authorship of the initial program is aknowledged.                 |
|       Use for commercial purposes without the previous written          |
|       agreement of the authors is forbidden.                            |
|_________________________________________________________________________|

*/

measure_op(id,X,X,true).
measure_op(same,X,Y,X=Y).
measure_op(less,X,Y,exceeds(Y,X)).
measure_op(not+less,X,Y,\+exceeds(Y,X)).
measure_op(more,X,Y,exceeds(X,Y)).
measure_op(not+more,X,Y,\+exceeds(X,Y)).

i_sup_op(least,min).
i_sup_op(most,max).

inverse(most,-,least).
inverse(least,-,most).
inverse(same,-,same).
inverse(less,-,more).
inverse(more,-,less).
inverse(X,+,X).

deepen_case(prep(at),time).
deepen_case(s_subj,dir).
deepen_case(s_subj,ind).
deepen_case(prep(by),subj).
deepen_case(prep(to),ind).
deepen_case(prep(of),poss).
deepen_case(X,X).

indexable(the(plu)).
indexable(all).

indexable(de(plu)).
indexable(alle).

