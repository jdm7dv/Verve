/*-----------------------------------------------------------------------------------------

  C#Prolog -- Copyright (C) 2007-2015 John Pool -- j.pool@ision.nl

  This library is free software; you can redistribute it and/or modify it under the terms of
  the GNU Lesser General Public License as published by the Free Software Foundation; either 
  version 3.0 of the License, or any later version.

  This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
  See the GNU Lesser General Public License (http://www.gnu.org/licenses/lgpl-3.0.html), or 
  enter 'license' at the command prompt.

-------------------------------------------------------------------------------------------*/

namespace Prolog
{
  /// <summary>
  /// Contains the predefined predicates, also required to provide the Prolog engine with basic capabilities
  /// </summary>
  public class Bootstrap
  {
    public static string LicenseUrl = "http://www.gnu.org/licenses/lgpl-3.0.html";

    public static string PredefinedPredicates =
      @"&builtin
       :- op( 900,  fy, [\+, not, once, help]).
       :- op( 700, xfx, [=, \=, ==, \==, is, :=, =:, =:=, =\=, <, >, =<, >=, @<, @>, @=<, @>=, =.., ?=]).
       :- op( 600, xfy, :).
       :- op( 500, yfx, [+, -, #, xor, \/]). % '+' also for string concatenation (is-operator)
       :- op( 500,  fx, [spy, nospy]).
       :- op( 400, yfx, [*, /, //, <<, >>, /\]).
       :- op( 300, xfx, [mod, ..]).
       :- op( 200,  fy, [+, -, \]).
       :- op( 200, xfx, **).
       :- op( 200, xfy, ^).
       :- op(  99, xfy, '{}').

       license :== license.

       % Note: '$' can be part (not start) of an unquoted atom in this startup code only.
       % The purpose is to create internal names that cannot be overwritten by user code.

       !             :== cut.
       call( X)      :== call.
       % extra args for implementation of maplist. A1..An will be added to the argument list of X
       call( X, A1) :== call.
       call( X, A1, A2) :== call.
       call( X, A1, A2, A3) :== call.
       call( X, A1, A2, A3, A4) :== call.
       call( X, A1, A2, A3, A4, A5) :== call.
       call( X, A1, A2, A3, A4, A5, A6) :== call.
       call( X, A1, A2, A3, A4, A5, A6, A7) :== call.
       meta$call( X) :== call.
       fail          :== fail.

       X = X.
       true.

       \+( X) :- X, !, fail.
       \+( X).

       not( X) :- X, !, fail.
       not( X).

%%     maplist(:Goal, +List1, ?List2)
%
%      True if Goal can successfully be applied to all
%      successive pairs of elements from List1 and List2.
%
       maplist( Goal, L1, L2) :-
         map$list( L1, L2, Goal).
       map$list( [], [], _).
       map$list( [H0|T0], [H|T], Goal) :-
         call( Goal, H0, H),
         map$list( T0, T, Goal).

       %make_array( A, [_|_]) :== make_array. % later

/*
       A naive implementation for the ; (or) operator would be:

       X ; Y :- call( X).
       X ; Y :- call( Y).

       This definition is incorrect. If X is compound and ends with a cut, Y will be selected on
       backtracking anyway, since the effect of a cut in a call argument is limited to the call itself
       (and according to ISO does not extend to the ';'. If it did, the second clause would not be tried).

       This has been solved by handling ; directly in ExecuteGoalList()

       The if-then-else below works correctly but could be treated similarly (i.e. in ExecuteGoalList())
*/

       X ; Y  :== or. % just for preventing ';' from redefinition

       C -> X ; Y :- C, !, X.
       C -> X ; Y :- !, Y.
       C -> X :- C, !, X.

       once( X) :- X, !.

       repeat.
       repeat :- repeat.

       consult( X) :== consult.

       asserta( X) :== asserta.
       assert( X)  :== assert.
       assertz( X) :== assertz.

       retract( X) :== retract.
       retract( X) :- retract( X).
       retractall( X) :== retractall.

       help        :== help.   % help hints
       help( P)    :== help.   % show help for predicate P (arity optional)

       spy( X)          :== spy.
       spy( X, [_|_])   :== spy.
       nospy( X)        :== spy.
       nospyall         :== nospyall.
       verbose          :== verbose.
       noverbose        :== noverbose.
       silent           :== silent.
       trace            :== trace.
       notrace          :== notrace.
       debug            :== debug.
       nodebug          :== nodebug.
       crossref( F)     :== crossref.     % generate a cross reference table and save it to file F
       profile          :== profile.      % switch profiling on: count all calls made during query execution
       noprofile        :== noprofile.    % switch profiling off
       showprofile      :== showprofile.  % show profiling results (hit count per predicate)
       showprofile( N)  :== showprofile.  % ... top N values only
       clearprofile     :== clearprofile. % reset profile counters to zero
       workingdir( D)   :== workingdir.   % set/get working directory
       workingdir       :== workingdir.   % ... restore default value (config file, or executable dir if missing)
       stacktrace( M)   :== stacktrace.   % show C# exception stacktrace (M = 'on' or 'off')
       
       % CACHEING CURRENTLY NOT USED
       % caching (tabling)
%       nocacheall     :== nocache. % unset caching for any predicate. Clear all caches
%
%       cache( []) :- !, nonvar( P).
%       cache( [P|Rest]) :-
%         !,
%         cache( P),
%         cache( Rest).
%       cache( P)      :== cache.   % set caching for predicate P (arity optional)
%
%       nocache( []) :- !, nonvar( P).
%       nocache( [P|Rest]) :-
%         !,
%         nocache( P),
%         nocache( Rest).
%       nocache( P)    :== nocache. % unset caching for predicate P
       % end of caching

       collection$( T, X, P, L) :-
         collection$init( T, S),
         ( call( P),
           collection$add( T, S, X),
           fail % undoes all var bindings in X
         ;
           true
         ),
         collection$exit( T, S, L).

       collection$init( T, S)    :== collection_init.
       collection$add( T, S, X)  :== collection_add.
       collection$exit( T, S, L) :== collection_exit.

       bagof( X, P, L) :-
         collection$( bagof, X, P, L).

       setof( X, P, L) :-
         collection$( setof, X, P, L).

       findall( X, P, L) :-
         collection$( findall, X, P, L).

       version( V, R)    :== version.
       halt              :== halt.
       length( L, N)     :== length.
       reverse( X, R)    :== reverse.
       sort( L, S)       :== sort.
       functor( T, F, N) :== functor.
       arg( N, T, A)     :== arg.
       abolish( X/N)     :== abolish.
       gensym( A, X)     :== gensym.
       gensym( X)        :== gensym.

       var( V)           :== var.
       var( V, N)        :== var. % N is the name of the var
       nonvar( V)        :== nonvar.
       atom( A)          :== atom_.
       atomic( A)        :== atomic.
       float( N)         :== float_.
       number( N)        :== number.
       integer( N)       :== integer.
       compound( V)      :== compound.
       list( L)          :== list.
       string( V)        :== string_.
       bool( V)          :== bool_.
       datetime( DT)                     :== datetime.
       datetime( DT, Y, Mo, D, H, Mi, S) :== datetime.
       datetime( DT, Y, Mo, D)           :== datetime.
       timespan( T)           :== timespan.
       timespan( T, H, Mi, S) :== timespan.
       date_part( DT, D) :== date_part.
       time_part( DT, T) :== time_part.
       succ( N0, N1)     :== succ.
       string_datetime( X, D) :== string_datetime.
       string_datetime( X, Y, M, D) :== string_datetime.
       string_datetime( X, Y, Mo, D, H, Mi, S) :== string_datetime.

       X is Y            :== is_.
       X \= Y            :== ne_uni.
       X == Y            :== eq_str.
       X \== Y           :== ne_str.
       X =:= Y           :== eq_num.
       X =\= Y           :== ne_num.
       X < Y             :== lt_num.
       X =< Y            :== le_num.
       X > Y             :== gt_num.
       X >= Y            :== ge_num.
       X @< Y            :== lt_ord.
       X @=< Y           :== le_ord.
       X @> Y            :== gt_ord.
       X @>= Y           :== ge_ord.
       X =.. Y           :== univ.
       unifiable( X, Y)  :== unifiable.

%       X ?= Y :-
%         (A == B ; A \= B), !. % SWI-Prolog

       % I/O

       fileexists( F)    :== fileexists.
       see( F)           :== see.
       seeing( F)        :== seeing.
       read( X)          :== read.
       readatom( X)      :== readatom.
       readatoms( L)     :== readatoms.
       readeof( F, T)    :== readeof. % unify the entire contents of file F with T
       readln( X)        :== readln.
       get0( C)          :== get0.
       get( C)           :== get.
       seen              :== seen.
       tell( F)          :== tell.
       telling( F)       :== telling.
       showfile( F)      :== showfile.
       write( X)         :== write.
       print( X)         :== print.
       treeprint( X)     :== treeprint.
       writeln( X)       :== writeln.
       writef( S, L)     :== writef.  % formatted write, à la C#. L single arg or list of args.
       writef( S)        :== write.
       writelnf( S, L)   :== writelnf.
       writelnf( S)      :== writeln.
       console( S)       :== console.
       console( S, L)    :== console.
       put( C)           :== put.
       nl                :== nl.
       tab( N)           :== tab.
       display( X)       :== display.
       told              :== told.
       cls               :== cls. % clear screen
       errorlevel( N)    :== errorlevel. % set DOS ERRORLEVEL (for batch processing)
       getenvvar( N, V)  :== getenvvar.  % unify V with the value of environment variable N
       setenvvar( N, V)  :== setenvvar.  % set the value of environment variable N to V

       maxwritedepth( N) :== maxwritedepth.
       % maxwritedepth/1: subterms beyond level N are written as '...'.
       % If set to -1 (default): no limit. The effect remains for the duration of the query.

    /* Backtrackable predicates
       ------------------------
       These are all implemented according to the same pattern: a predicate with an
       extra first State argument is introduced. This argument maintains the state
       between two successive backtracking calls. It is initialized at the first call,
       and reset to an unbound variable after the last successful call.
       UserClassTerm (in DerivedTerms.cs) can be used for creating a State term with
       an arbitrary class type content. In my experience, an enumerator is eminently
       suited for this task, since in fact it can be considered a finite state machine
       that yields one value at a time and saves state between calls.
    */

       clause( H, B) :-              % returns body B for clause head H
         clause$( State, H, B),
         !,
         clause$( State, H, B).

       clause$( State, H, B) :== clause.
       clause$( State, H, B) :-
         !,
         nonvar( State),             % 'clause' resets State to var upon failure
         clause$( State, H, B).

       combination( P0, K, P1) :-         % returns next K-combination P1 for list P0.
         combination$( State, P0, K, P1), % First returned value of P1 is P0 sorted
         !,
         combination$( State, P0, K, P1).
 
       combination$( State, P0, K, P1) :== combination.
       combination$( State, P0, K, P1) :-
         !,
         nonvar( State),             % 'combination' resets State to var upon failure
         combination$( State, P0, K, P1).

       permutation( P0, P1) :-         % returns next permutation P1 for list P0.
         permutation$( State, P0, P1), % First returned value of P1 is P0 sorted
         !,
         permutation$( State, P0, P1).
 
       permutation$( State, P0, P1) :== permutation.
       permutation$( State, P0, P1) :-
         !,
         nonvar( State),             % 'permutation' resets State to var upon failure
         permutation$( State, P0, P1).

       current_op( P, F, N) :-       % return operators matching ?P(recedence), ?F(ix), ?N(name)
         current$op( State, P, F, _),
         !,
         current$op( State, P, F, N).

       current$op( State, P, F, N) :== current_op.
       current$op( State, P, F, N) :-
         !,
         nonvar( State),             % 'current_op' resets State to var upon failure
         current$op( State, P, F, N).

       config_setting( K, V) :-      % show configuration settings (csprolog.exe.config)
         config_setting$( State, K, V),
         !,
         config_setting$( State, K, V).

       config_setting$( State, K, V) :== config_setting.
       config_setting$( State, K, V) :-
         !,
         nonvar( State),
         config_setting$( State, K, V).

       % find pattern P in T, between depths Dmin and Dmax (inclusive)
       term_pattern( T, P, Dmin, Dmax) :-
         term_pattern$( State, T, P, Dmin, Dmax, !),
         !,
         term_pattern$( State, T, P, Dmin, Dmax, !).

       term_pattern$( State, T, P, Dmin, Dmax, Loc) :== term_pattern.  % Loc is path to P in T
       term_pattern$( State, T, P, Dmin, Dmax, Loc) :-
         !,
         nonvar( State),
         term_pattern$( State, T, P, Dmin, Dmax, Loc).

       term_pattern( T, P) :-
         !,
         term_pattern( T, P, _, _).

       % Loc as extra arg
       term_pattern( T, P, Dmin, Dmax, Loc) :-
         term_pattern$( State, T, P, Dmin, Dmax, Loc),
         !,
         term_pattern$( State, T, P, Dmin, Dmax, Loc).

       term_pattern( T, P, Loc) :-
         !,
         term_pattern( T, P, _, _, Loc).

       between( L, H, N) :-          % returns N = L, L+1, ... H upon backtracking,
         between$( State, L, H, _),  % first call only initializes State,
         !,                          % ... which maintains state between subsequent calls
         between$( State, L, H, N).

       between$( State, L, H, N) :== between.
       between$( State, L, H, N) :-
         !,
         nonvar( State),             % 'between' resets State to var upon failure
         between$( State, L, H, N).

       % SQL SELECT --- one row per successful call in list X; SQL NULL -> db_null
       % See submap SQL for some examples.

       sql_select( CI, S, X) :-      % SELECT-statement(s) in string S, multiple result sets allowed
         sql$select( State, CI, S, false), % true/false only to distinguish between select and select2
         !,
         sql$select( State, CI, S, X).

       sql$select( State, CI, S, X) :== sql_select. % next record from the SqlReader, fails at end
       sql$select( State, CI, S, X) :-
         !,
         nonvar( State),                        % sql_select resets State to var upon failure
         sql$select( State, CI, S, X).

       sql_select2( CI, S, X) :-
         sql$select( State, CI, S, true), % show column names
         !,
         sql$select( State, CI, S, X).

       %%% End of backtrackable predicates

       % Remaining SQL.

       % sql_connect( +<provider>, +<connect string arguments>, -<connection info>)
       sql_connect( P, A, CI) :== sql_connect. % see 'help( sql_connect).'
       sql_disconnect( CI)    :== sql_disconnect.
       sql_connection( CI, P, CS) :== sql_connection. % gives the provider and connectring value for connection CI
       
       sql_command( CI, Cmd)     :== sql_command. % arbitrary SQL-command(s) in string Cmd, ...
       sql_command( CI, Cmd, N)  :== sql_command. % ... output N is the number of rows affected

       name( A, L)               :== name.
       atom_string( A, S)        :== atom_string. % conversion between atom and string
       string_term( S, T)        :== string_term. % convert string S to Prolog term T and v.v.
       string_words( S, L)       :== string_words.
       expand_term( (P-->Q), R)  :== expand_term.
       numbervars( X, B, E)      :== numbervars.
       username( X)              :== username.
       shell                     :== shell_d.       % asynchronous call -- open a DOS-box
       shell( dos(Cmd))          :== shell_d.       % ... -- run DOS command Cmd
       shell( dos(Cmd), A)       :== shell_d.       % ... -- run DOS command Cmd with argument(s) A
       shell( Path/Cmd, A)       :== shell_p.       % ... -- run command Path\Cmd with argument(s) A
       shell( Cmd, A)            :== shell_x.       % ... -- run command Cmd with argument(s) A
       shell( dos(Cmd), A, E)    :== shell_sync_d.  % synchronous call, E is Exit code
       shell( Path/Cmd, A, E)    :== shell_sync_p.  % ...
       shell( Cmd, A, E)         :== shell_sync_x.  % ...
       predicate( P/N)           :== predicatePN.
       predicate( T)             :== predicateX.
       ground( X)                :== ground.
       throw( S)                 :== throw_. % raise an exception, show text S
       throw( S, L)              :== throw_. % ...; L single arg or list of args.
       throw( C, S, L)           :== throw_. % ...; C exception class name (atom or integer); L single arg or list of args.
       sendmail( ToAddr, Subject, Body) :== sendmail.
       sendmail( Smtp, ToAddr, Subject, Body) :== sendmail.
       sendmail( Smtp, Port, ToAddr, Subject, Body) :== sendmail.
       clipboard( T)            :== clipboard. % put string value of T on the clipboard

       format( S, L, X)         :== format.
       now( H, M, S)            :== now.
       validtime( H, M, S)      :== validtime.
       today( Y, M, D)          :== today.
       validdate( Y, M, D)      :== validdate.
       dayname( Y, M, D, N)     :== dayname.
       dayofweek( Y, M, D, N)   :== dayofweek.
       dayofyear( Y, M, D, N)   :== dayofyear.
       leapyear( Y)             :== leapyear.
       weekno( Y, M, D, N)      :== weekno.
       weekno( N)               :== weekno.
       append2( X, Y, Z)        :== append2.      % deterministic append
       flat( X, Y)              :== flat.         % deterministic flatten
       dcg_flat( X, Y)          :== dcg_flat.     % flatten curly bracket list
       bw_transform( P, E, I)   :== bw_transform. % Burroughs-Wheeler transform

       % XML (PLXML -- Google ""PLXML Prolog"")

      /* xml_term: also for reading & writing files:
         xml_term( see(+FileName), ?Term) or xml_term( tell(+FileName), +Term)
       */
       xml_term( X, T)          :== xml_term. % X is string or text file containing xml, T is Prolog term
       xml_term( X, T, Options) :== xml_term. 
       

       % 'xmldocument$'/3 is the last argument of xml_term/2/3
       % '#define fullyTagged' must be commented out in SimpleDOMParser.cs.
       xml_prolog( 'xmldocument$'( P, _, _), P).  % 'prolog' in the XML definition sense
       xml_root(   'xmldocument$'( _, R, _), R).  % The root element of the XML document
       xml_misc(   'xmldocument$'( _, _, M), M).  % Anything after the root element
       xml_element_name( E, Name) :-        E =.. [Name, _Attrs, _Content].
       xml_element_attributes( E, Attrs) :- E =.. [_Name, Attrs, _Content].
       xml_element_content( E, Content) :-  E =.. [_Name, _Attrs, Content].

       % XML-transformation of file 'Xml' using stylesheet 'Xsl' yielding file 'Html'
       xml_transform( Xml, Xsl, Html) :== xml_transform.
       xml_transform( Xml, Xsl)      :== xml_transform. % Htm-file gets name of Xml-file, but extension .html
       xml_transform( Xml)           :== xml_transform. % Htm-file and Xsl-file get name of Xml-file, but extensions .html and .xsl

       json_term( J, T)         :== json_term. % J is string or text file containing json, T is Prolog term
       json_term( J, T, [_|_])  :== json_term. % options
       
       % json_xml( +Term, +RootName, ?String) or json_xml( +Term, +RootName, tell(+FileName))
       json_xml( T, X)          :== json_xml. % default root name ""XML_ROOT""
       json_xml( T, R, X)       :== json_xml. 
       json_xml( T, R, X, A)    :== json_xml. % list with attributes (to be converted to attr=value)

       % list functions

       listing                :== listing.
       listing( X/N)          :== listingXN.
       listing( [X|Rest]) :-
         listing( X),
         !,
         listing( Rest).
       listing( []).
       listing( X)            :== listingX.

       % same, but for predefined predicates (mainly for testing & debugging)

       listing0               :== listing0.
       listing0( X/N)         :== listing0XN.
       listing0( [X|Rest]) :-
         listing0( X),
         !,
         listing0( Rest).
       listing0( []).
       listing0( X)           :== listing0X.

       pp_defines( X)         :== pp_defines. % preprocessor symbol definitions
       undefineds             :== undefineds.
       copy_term( X, Y)       :== copy_term.
       clearall               :== clearall.
       spypoints              :== spypoints.
       stringstyle( X)        :== stringstyle.
       %next version:
       %callstack( S)         :== callstack. % string S contains current call stack
       %callstack( S, L)      :== callstack. % ... L is list representation

       memberchk( X, [Y|Rest]) :- nonvar( X), member( X, [Y|Rest]).
       % Actually member/2 given below is memberchk/2.
       % As member/2, it is not completely correct, e.i. it should backtrack on member( 1, [1,1,1]) !!!
       member( X, L)           :== member. % fails for unbound X or L. Disables backtracking upon success
       member( X, [X|_]).
       member( X, [_|L]) :- member( X, L).

       append( [], X, X).
       append( [X|Y], U, [X|V]):-
         append(  Y, U, V).

       regex_match( S, P, L)        :== regex_match. % Find all occurances of match pattern P in string S
       regex_match( S, P, L, [_|_]) :== regex_match. % Optional list of regex options (a la C#)

       regex_replace( S, P, R, T) :== regex_replace. % String T is the result of replacing all ...
                                                     % ... occurances of pattern P in S with R

       xmltrace( X)             :== xmltrace.
       xmltrace( X, N)          :== xmltrace.
       numcols( N)              :== numcols. % number of columns in the DOS-box

       userroles( X)            :== userroles.
       statistics( T, [MSec,_]) :== statistics.
       environment( X, Y)       :== environment.
       ip_address( X)           :== ip_address.  % local IP-address. X is string.
       ip_address( X, L)        :== ip_address.  % ..., L is list of numbers

       get_counter( N, V)       :== get_counter. % Get current integer value V of counter number N
       set_counter( N, V)       :== set_counter. % N is set to integer V, and not unbound in backtracking
       inc_counter( N)          :== inc_counter. % Value of N is increased at each call and not unbound in backtracking
       inc_counter( N, V)       :== inc_counter. % ... return new value in V
       dec_counter( N)          :== dec_counter. % Value of N is decreased at each call and not unbound in backtracking
       dec_counter( N, V)       :== dec_counter. % ... return new value in V
       setvar( N, V)            :== setvar.      % Store a copy of V in a global symbol table under the name N
       getvar( N, V)            :== getvar.      % Get the term stored under the name N from the global symbol table

       query_timeout( MSecs)    :== query_timeout. % must be entered as a separate query *before* the query you want to limit

       make_help_resx           :== make_help_resx. % create help resource file from the file name specified in the config file

%      %%%%%%%%%%%%%%%%%%%%%%%%%%%%%% SAMPLES, TESTING & EXPERIMENTAL

       age( peter, 7).
       age( ann, 5).
       age( ann, 6).
       age( pat, 8).
       age( tom, 5).

       variation( L0, K, L1) :-
         combination( L0, K, L),
         permutation( L, L1).

% Ivan Bratko, ""PROLOG Programming for Artificial Intelligence"", example at 3rd edition 2001, ch.21.1 p.560

       sentence( Number) --> (noun_phrase(Number), verb_phrase(Number)).
       verb_phrase( Number) --> verb(Number), noun_phrase(Number).
       noun_phrase( Number) --> determiner(Number), noun(Number).
       determiner( singular) --> [a].
       determiner( singular) --> [the].
       determiner( plural) --> [the].
       noun( singular) --> [cat].
       noun( singular) --> [mouse].
       noun( plural) --> [cats].
       noun( plural) --> [mice].
       verb( singular) --> [scares].
       verb( singular) --> [hates].
       verb( plural) --> [scare].
       verb( plural) --> [hate].
       verb( plural) --> [hate];[love].

% TALK (Fernando Pereira, Stuart Shieber), ""Prolog and Natural Language Analysis"", pp. 149+

       % A PDF-version of this book (obtained from Internet after googling
       % ""Fernando Pereira TALK-program"") can be found in the TALK directory.

       talk :- ['TALK\\talk'].

       % Start the program by entering 'go.', end by entering an empty line.
       %
       % Sample dialog (TALK-responses not shown) (notice: no terminating dots):
       %
       % >> principia is a book
       % >> bertrand wrote every book
       % >> what did bertrand write


% CHAT-80 (Fernando Pereira) (http://www.cis.upenn.edu/~pereira/oldies.html)

       % See demo.txt and docu in the CHAT-directory for examples.
       % Start CHAT by entering 'hi.', end by entering 'bye.'
       % The CHAT-software is copyrighted !!!  (although I do not think Fernando
       % Pereira still cares -- unfortunately he never responded to mails)

       chat :- consult([
        'chat-80\\xgrun',     % XG (eXtended Grammar)
        'chat-80\\newg',      % clone + lex
        'chat-80\\clotab',    % attachment tables
        'chat-80\\slots1',    % fits arguments into predicates
        'chat-80\\scopes',    % quantification and scoping
        'chat-80\\qplan',     % query planning
        'chat-80\\talkr',     % query evaluation
        'chat-80\\readin',    % sentence input
        'chat-80\\ptree',     % print trees
        'chat-80\\aggreg',    % aggregation operators
        'chat-80\\templa',    % semantic dictionary
        'chat-80\\slots2',    % fits arguments into predicates
        'chat-80\\newdict',   % syntactic dictionary
        'chat-80\\world',     % geographical data base (with data from 25 years ago!)
        'chat-80\\rivers',    % ...
        'chat-80\\cities',    % ...
        'chat-80\\countries', % ...
        'chat-80\\contain',   % ...
        'chat-80\\borders',   % ...
        'chat-80\\ndtabl',    % relation info
        'chat-80\\newtop']).  % top level

       % also required for CHAT:

       keysort(L, S) :- sort(L, S).

       conc([], L, L).
       conc([X|L1], L2, [X|L3]) :- conc(L1, L2, L3).

       %%%%%%%%%%%%%%%%%%%%%%%%%%%%%% END

       ";
  }
}
