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

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Diagnostics;
#if mswindows
using System.Security.Principal;
using System.Collections.Generic;
using System.Configuration;
using System.Resources;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Data.Common;
using System.Data;
using System.Net;
using System.Net.Sockets;
#endif

namespace Prolog
{
  public enum BI // builtins
  {
    none, abolish, arg, append, append2, assert, asserta, assertz, atom_,
    atom_string, atomic, between, bool_, bw_transform, cache, call, clause, clearall, 
    clearprofile, clipboard, cls, collection_add, collection_exit, collection_init, 
    combination, compound, config_setting, console, consult, copy_term, crossref, 
    current_op, cut, dayname, dcg_flat, date_part, datetime, dayofweek, dayofyear, 
    debug, dec_counter, display, environment, eq_num, eq_str, errorlevel, expand_term, 
    fail, fileexists, flat, float_, format, functor, ge_num, ge_ord, gensym, genvar, 
    get, get_counter, get0, getenvvar, getvar, ground, gt_num, gt_ord, halt, help, 
    inc_counter, integer, ip_address, is_, json_term, json_xml, le_num, le_ord, 
    leapyear, length, license, list, listing, listing0, listing0X, listing0XN, listingX, 
    listingXN, lt_num, lt_ord, make_help_resx, maxwritedepth, member, name, ne_num, 
    ne_str, ne_uni, nl, nocache, nodebug, nonvar, noprofile, nospy, nospyall, notrace, 
    noverbose, now, number, numbervars, numcols, or, permutation, pp_defines, 
    predicatePN, predicateX, print, profile, put, query_timeout, read, readatoms, 
    readatom, readeof, readln, regex_match, regex_replace, retract, retractall, 
    reverse, sendmail, see, seeing, seen, set_counter, setenvvar, setvar, shell_d, 
    shell_p, shell_x, shell_sync_d, shell_sync_p, shell_sync_x, shell_dos, shell_exe, 
    showfile, showprofile, silent, sort, spy, spypoints, sql_connect, sql_connection, 
    sql_command, sql_disconnect, sql_select, stacktrace, callstack, statistics, 
    string_, string_datetime, string_term, string_words, stringstyle, succ, tab, tell, 
    telling, term_pattern, throw_, time_part, timespan, today, told, trace, treeprint, 
    undefineds, unifiable, univ, username, userroles, validdate, validtime, var, 
    verbose, version, weekno, workingdir, write, writef, writeln, writelnf, xml_term,
    xmltrace, xml_transform
  }

  public partial class PrologEngine
  {
    class PredicateCallOptions
    {
      Dictionary<string, List<object>> table;
      List<object> o;

      public PredicateCallOptions ()
      {
        table = new Dictionary<string, List<object>> ();
      }

      public void Set (BaseTerm list)
      {
        table = new Dictionary<string, List<object>> ();
        CopyFromList ((ListTerm)list);
      }

      public void Clear ()
      {
        table.Clear ();
      }

      void Register<T> (string s, T value)
      {
        if (table.TryGetValue (s, out o))
          o.Add (value);
        else
          table [s] = new List<object> () { value };
      }


      public void Register (string s)
      {
        table [s] = null;
      }

      public bool Get<T> (string s, int argNo, ref T value) where T : struct
      {
        if (table.TryGetValue (s, out o))
        {
          if (o == null)
            IO.Error ("No argument list allowed for option '{0}'", s);
          else if (argNo <= o.Count)
            value = (T)o [argNo - 1];

          return true;
        }
        else
          return false;
      }


      public bool Get<T> (string s, ref T value) where T : struct
      {
        return Get<T> (s, 1, ref value);
      }


      public bool Get (string s)
      {
        return table.TryGetValue (s, out o);
      }


      public void CopyFromList (ListTerm list)
      {
        foreach (BaseTerm t in list)
        {
          string optionName = t.FunctorToString;

          if (t.Arity == 0)
            Register (optionName);
          else
            foreach (BaseTerm a in t.Args)
            {
              if (a.IsNumber)
                Register<int> (optionName, a.To<int> ());
              else if (a.IsAtom)
                Register<string> (optionName, a.FunctorToString);
              else if (a.IsString)
                Register<string> (optionName, a.FunctorToString.ToAtom ());
            }
        }
      }


      public override string ToString ()
      {
        StringBuilder sb = new StringBuilder ();
        bool first0 = true;
        sb.Append ('[');

        foreach (KeyValuePair<string, List<object>> entry in table)
        {
          if (first0) first0 = false; else sb.Append (", ");

          sb.Append (entry.Key);

          if (entry.Value != null)
          {
            sb.Append ('(');
            bool first1 = true;

            foreach (object o in entry.Value)
            {
              if (first1) first1 = false; else sb.Append (",");

              sb.Append (o.ToString ());
            }

            sb.Append (')');
          }
        }

        sb.Append (']');

        return sb.ToString ();
      }
    }

    string currentInputName = null;
    string currentOutputName = null;

    bool DoBuiltin (BI biId, out bool findFirstClause)
    {
      findFirstClause = false;
      BaseTerm term = goalListHead.Term;
      BaseTerm t0, t1, t2, t3, t4, t5;
      SqlTerm sqlt;
      int n, y, m, d, h, s;
      int arity;
      string functor;
      bool result;
      bool inFile = false;
      bool outFile = false;
      DupMode dupMode = DupMode.DupAccept; // for setof, bagoff, findall
      TermType type;
      string a, x;
      string fileName;
      string cmd = null;
      int cntrValue;
      DateTime dati;
      TimeSpan ti;
      Process proc;
      bool mustWait = false;
      DbConnectionTerm cnt;
      DbCommand dbCommand;

      predicateCallOptions.Clear ();

      #region switch
      switch (biId)
      {
        case BI.license:
          Process.Start (Bootstrap.LicenseUrl);
          IO.Message ("Opening your browser ...");
          break;
        case BI.consult: // individual file or list of files
          t0 = term.Arg (0);

          if (t0.IsProperList)
          {
            int lines = 0;
            int files = 0;

            while (t0.Arity == 2)
            {
              fileName = Utils.FileNameFromTerm (t0.Arg (0), ".pl");

              if (fileName == null) return false;

              lines += predTable.Consult (fileName);
              files++;
              t0 = t0.Arg (1);
            }

            if (files > 1) IO.Message ("Grand total is {0} lines", lines);

            predTable.ResolveIndices ();

            break;
          }

          if (t0.IsAtomOrString)
          {
            fileName = Utils.FileNameFromTerm (t0, ".pl");

            if (fileName == null) return false;

            IO.Write ("--- Consulting {0} ... ", fileName);
            predTable.Consult (fileName);
            IO.WriteLine ("{0} lines read", parser.LineCount);
            predTable.ResolveIndices ();

            break;
          }

          return IO.Error ("Unable to read file '{0}'", t0.Arg (0));

        case BI.asserta:
          predTable.Assert (term.Arg (0), true); // true: at beginning
          break;

        case BI.assert:
        case BI.assertz:
          predTable.Assert (term.Arg (0), false);
          break;

        case BI.retract:
          if (predTable.Retract (term.Arg (0), varStack, null))
            currentCp.NextClause = retractClause;
          else
          {
            CanBacktrack ();
            return false;
          }
          break;

        case BI.retractall: // retractall
          predTable.RetractAll (term.Arg (0), varStack);
          break;

        case BI.spy: // leash modes [call, exit, redo, fail]
        case BI.nospy:
          result = true;
          t0 = term.Arg (0);

          if (term.Arity == 2) t3 = term.Arg (1); else t3 = null; // leash t

          if (t0.HasFunctor ("/") && t0.Arity == 2 && (t1 = t0.Arg (0)).IsAtom && (t2 = t0.Arg (1)).IsInteger)
            result = predTable.SetSpy (term.HasFunctor ("spy"), t1.FunctorToString, t2.To<int> (), t3);
          else if (t0.Arity == 0)
            result = predTable.SetSpy (term.HasFunctor ("spy"), t0.FunctorToString, -1, t3);

          if (!result) return false;

          if (!debug)
          {
            debug = true;
            IO.Message ("Debugging switched on");
          }
          break;

        case BI.nospyall:
          predTable.SetNoSpyAll ();
          break;

        case BI.verbose:
          IO.Verbose = true;
          break;

        case BI.noverbose:
        case BI.silent:
          IO.Verbose = false;
          break;

        case BI.trace:
        case BI.notrace:
          SetSwitch ("Tracing", ref trace, term.HasFunctor ("trace"));
          if (trace) debug = true;
          reporting = debug || xmlTrace;
          break;

        case BI.debug:
        case BI.nodebug:
          SetSwitch ("Debugging", ref debug, term.HasFunctor ("debug"));
          reporting = debug || xmlTrace;
          break;

        case BI.help:
          string suggestion = null;

          if (term.Arity == 0) // help on help
            predTable.ShowHelp (null, -1, out suggestion);
          else if (term.Arity == 1) // specific predicate, potentially with an arity
          {
            result = false;

            if ((t0 = term.Arg (0)).HasFunctor ("/") && t0.Arity == 2 && t0.Arg (0).IsAtom && t0.Arg (1).IsInteger)
              result = predTable.ShowHelp (t0.Arg (0).FunctorToString, t0.Arg<int> (1), out suggestion);
            else if (term.Arg (0).IsAtom) // predicate functor without arity: take all arities
              result = predTable.ShowHelp (term.Arg (0).FunctorToString, -1, out suggestion);

            if (!result)
              IO.Warning ("Predicate '{0}' not found.{1}", term.Arg (0), suggestion);
          }
          break;

        case BI.cache:
          if (SetCaching (term, true)) break;
          return false;

        case BI.nocache:
          if (SetCaching (term, false)) break;
          return false;

        // bagof, setof, findall
        case BI.collection_init:
          if (term.Arg (0).HasFunctor ("setof"))
            dupMode = DupMode.DupIgnore;
          else
            dupMode = DupMode.DupAccept;

          term.Arg (1).Unify (new CollectionTerm (dupMode), varStack);

          break;

        case BI.collection_add:
          if ((t2 = term.Arg (2)).IsVar) return false;

          // t2 must be copied because it is unbound during backtracking
          if (term.Arg (0).HasFunctor ("setof"))
            ((CollectionTerm)term.Arg (1)).Insert (t2.Copy ());
          else
            ((CollectionTerm)term.Arg (1)).Add (t2.Copy ());

          break;

        case BI.collection_exit:
          CollectionTerm ct = ((CollectionTerm)term.Arg (1));

          // bagof and setof must Fail if there are no matches; findall will Succeed
          if (term.Arg (0).FunctorToString != "findall" && ct.Count == 0) return false;

          ListTerm ccc = ct.ToList ();

          term.Arg (2).Unify (ct.ToList (), varStack);

          break;

        case BI.version: // version(V, R)
          if (!term.Arg (0).Unify (new AtomTerm (VERSION), varStack)) return false;
          if (!term.Arg (1).Unify (new AtomTerm (RELEASE), varStack)) return false;
          break;

        case BI.halt:
          halted = true;
          break;

        case BI.reverse: // reverse( ?X, ?R) -- proper list X is the reversed version of list R
          t0 = term.Arg (0);
          t1 = term.Arg (1);

          if (t0.IsVar)
          {
            if (t1.IsVar || !t1.IsProperList) return false;

            t0.Unify (((ListTerm)t1).Reverse (), varStack);
          }
          else // t0 has a value
          {
            if (!t0.IsProperList) return false;

            if (!t1.Unify (((ListTerm)t0).Reverse (), varStack))
              return false;
          }

          break;

        case BI.combination: // combination( +P, +K, ?Q) -- list Q is the 'next' K-combination of list P
          t1 = term.Arg (1);
          t2 = term.Arg (2); // combination size (k)

          if (!t1.IsProperList || !t2.IsInteger) return false;

          if (t1.IsEmptyList)
          {
            if (term.Arg (3).Unify (ListTerm.EMPTYLIST, varStack)) break;

            return false;
          }

          Combination cmb;
          IEnumerator<ListTerm> iCombi = null;
          t0 = term.Arg (0);

          if (t0.IsVar) // first call only, Arg(0) contains State info
          {
            cmb = new Combination ((ListTerm)t1, t2.To<int> ());
            iCombi = cmb.Iterator;
            t0.Unify (new UserClassTerm<IEnumerator<ListTerm>> (iCombi), varStack);

            break;
          }

          iCombi = ((UserClassTerm<IEnumerator<ListTerm>>)t0).UserObject;

          while (true)
          {
            if (!iCombi.MoveNext ())
            {
              term.SetArg (0, Variable.VAR);

              return false;
            }

            if (term.Arg (3).Unify (iCombi.Current, varStack)) break;

            return false;
          }

          break;

        case BI.permutation: // permutation( +P, ?Q) -- list Q is the 'next' permutation of list P
          t1 = term.Arg (1);

          if (!t1.IsProperList) return false;

          if (t1.IsEmptyList)
          {
            if (term.Arg (2).Unify (ListTerm.EMPTYLIST, varStack)) break;

            return false;
          }

          Permutation pmt;
          IEnumerator<ListTerm> iPermut = null;
          t0 = term.Arg (0);

          if (t0.IsVar) // first call only, Arg(0) contains State info
          {
            pmt = new Permutation ((ListTerm)t1);
            iPermut = pmt.GetEnumerator ();
            t0.Unify (new UserClassTerm<IEnumerator<ListTerm>> (iPermut), varStack);

            break;
          }

          iPermut = ((UserClassTerm<IEnumerator<ListTerm>>)t0).UserObject;

          while (true)
          {
            if (!iPermut.MoveNext ())
            {
              term.SetArg (0, Variable.VAR);

              return false;
            }

            if (term.Arg (2).Unify (iPermut.Current, varStack)) break;

            return false;
          }

          break;

        case BI.length: // properLength( L, N)
          t0 = term.Arg (0);
          t1 = term.Arg (1);

          if (t0.IsProperOrPartialList)
          {
            n = 0;

            while (t0.IsListNode)
            {
              n++;
              t0 = t0.Arg (1);
            }

            if (t0.IsVar && t1.IsNatural) // cope with calls such as properLength( [1,2,3|T], 9)
            {
              if ((n = t1.To<int> () - n) < 0) return false;

              t2 = ListTerm.EMPTYLIST;

              for (int i = 0; i < n; i++)
                t2 = new ListTerm (new Variable (), t2);

              t0.Unify (t2, varStack);

              break;
            }

            if (!term.Arg (1).Unify (new DecimalTerm (n), varStack)) return false;
          }
          else if (t0.IsAtomOrString)
          {
            if (!term.Arg (1).Unify (new DecimalTerm (t0.FunctorToString.Length), varStack))
              return false;
          }
          else // create a list with N elements
          {
            if (!t1.IsNatural) return false;

            arity = t1.To<int> ();
            t1 = ListTerm.EMPTYLIST;

            for (int i = 0; i < arity; i++)
              t1 = new ListTerm (new Variable (), t1);

            t0.Unify (t1, varStack);
          }

          break;


        case BI.sort: // sort( L, S)
          t0 = term.Arg (0);
          t1 = term.Arg (1);

          if (t0.IsProperList)
          {
            if (!(t1.IsProperList || t1.IsVar)) return false;

            BaseTermSet tlist = new BaseTermSet (t0);
            tlist.Sort ();

            if (!t1.Unify (tlist.ToList (), varStack)) return false;
          }
          else
            return false;

          break;

        case BI.succ: // succ(?N0, ?N1) -- succeeds if N1-N0 = 1
          t0 = term.Arg (0);
          t1 = term.Arg (1);

          if (t0.IsVar)
          {
            if (t1.IsVar || !t1.IsInteger) return false;

            t0.Unify (new DecimalTerm (t1.To<int> () - 1), varStack);
          }
          else if (t1.IsVar)
            t1.Unify (new DecimalTerm (t0.To<int> () + 1), varStack);
          else if (!t0.IsInteger || !t1.IsInteger || t0.To<int> () != t1.To<int> () - 1)
            return false;

          break;

        case BI.functor: // functor( T, F, N)
          t0 = term.Arg (0);

          if (t0.IsVar)
          {
            t1 = term.Arg (1);

            if (t1.IsVar) return false;

            functor = t1.FunctorToString;
            t2 = term.Arg (2);

            if (t2.IsNatural)
              arity = t2.To<int> ();
            else
              return false;

            BaseTerm [] args = new BaseTerm [arity];

            for (int i = 0; i < arity; i++) args [i] = new Variable ();

            if (!t0.Unify (CreateNewTerm (t2, arity, functor, args), varStack)) return false;

            break;
          }
          else
          {
            if (!term.Arg (1).Unify (new AtomTerm (t0.Functor), varStack)) return false;

            if (!term.Arg (2).Unify (new DecimalTerm (t0.Arity), varStack)) return false;

            break;
          }

        case BI.arg: // arg( N, BaseTerm, A)
          t0 = term.Arg (0);
          t1 = term.Arg (1);

          if (t0.IsVar || t1.IsVar ) return false;
 
          n = t0.To<int> ();  // N is 1-based

          if (n <= 0 || n > t1.Arity) return false; 

          if (!t1.Arg (n - 1).Unify (term.Arg (2), varStack)) return false;
          
          break;

        case BI.abolish: // abolish( X/N)
          t0 = term.Arg (0);
          result = true;
          if (t0.HasFunctor ("/") && t0.Arity == 2 && t0.Arg (0).IsAtom && t0.Arg (1).IsInteger)
            result = predTable.Abolish (t0.Arg (0).FunctorToString, t0.Arg (1).To<short> ());
          else
            result = false;
          if (!result) return false;
          break;

        case BI.gensym: // gensym( X)
          if (term.Arity == 1)
          {
            t0 = new AtomTerm ("v" + gensymInt++);

            if (t0.Unify (term.Arg (0), varStack))
              break;
            else
              return false;
          }
          else
          {
            if (!term.Arg (0).IsAtom) return false;

            t0 = new AtomTerm (term.Arg (0).FunctorToString + gensymInt++);

            if (t0.Unify (term.Arg (1), varStack))
              break;
            else
              return false;
          }

        case BI.var:
          if ((!term.Arg (0).IsVar) ||
               (term.Arity == 2 &&
                !term.Arg (1).Unify (new StringTerm (term.Arg (0).Name), varStack)))
            return false;
          break;

        case BI.nonvar:
          if (!term.Arg (0).IsVar) break;
          return false;

        case BI.atom_:
          if (term.Arg (0).IsAtom) break;
          return false;

        case BI.atomic:
          if (term.Arg (0).IsAtomic) break;
          return false;

        case BI.integer:
          if (term.Arg (0).IsInteger) break;
          return false;

        case BI.float_:
          if (term.Arg (0).IsFloat) break;
          return false;

        case BI.number:
          if (term.Arg (0).IsNumber) break;
          return false;

        case BI.compound:
          if (term.Arg (0).IsCompound) break;
          return false;

        case BI.list:
          if (term.Arg (0).IsProperList) break;
          return false;

        case BI.string_:
          if (term.Arg (0).IsString) break;
          return false;

        case BI.bool_:
          if (term.Arg (0).IsBool) break;
          return false;

        case BI.datetime: // datetime/1/4/7
          t0 = term.Arg (0);

          if (term.Arity == 1)
          {
            if (!t0.IsDateTime) return false;
          }
          else if (t0.IsDateTime)
          {
            dati = t0.To<DateTime> ();

            if (!term.Arg (1).Unify (new DecimalTerm (dati.Year), varStack) ||
                !term.Arg (2).Unify (new DecimalTerm (dati.Month), varStack) ||
                !term.Arg (3).Unify (new DecimalTerm (dati.Day), varStack) ||
                (term.Arity == 7 &&
                  (!term.Arg (4).Unify (new DecimalTerm (dati.Hour), varStack) ||
                   !term.Arg (5).Unify (new DecimalTerm (dati.Minute), varStack) ||
                   !term.Arg (6).Unify (new DecimalTerm (dati.Second), varStack)
               )))
              return false;
          }
          else if (t0.IsVar)
          {
            if (term.Arity == 4)
            {
              dati = new DateTime (
                term.Arg (1).To<int> (),
                term.Arg (2).To<int> (),
                term.Arg (3).To<int> ());
            }
            else
            {
              dati = new DateTime (
                term.Arg (1).To<int> (),
                term.Arg (2).To<int> (),
                term.Arg (3).To<int> (),
                term.Arg (4).To<int> (),
                term.Arg (5).To<int> (),
                term.Arg (6).To<int> ());
            }

            if (!t0.Unify (new DateTimeTerm (dati), varStack)) return false;
          }
          else
          {
            IO.Error ("datetime/4/7: first argument must be either a DateTime or a var");

            return false;
          }
          break;

        case BI.timespan:
          t0 = term.Arg (0);

          if (term.Arity == 1)
          {
            if (!t0.IsTimeSpan) return false;
          }
          else if (t0.IsTimeSpan)
          {
            ti = t0.To<TimeSpan> ();

            if (!term.Arg (4).Unify (new DecimalTerm (ti.Hours), varStack) ||
                !term.Arg (5).Unify (new DecimalTerm (ti.Minutes), varStack) ||
                !term.Arg (6).Unify (new DecimalTerm (ti.Seconds), varStack)
               )
              return false;
          }
          else if (t0.IsVar)
          {
            ti = new TimeSpan (
              term.Arg (1).To<int> (),
              term.Arg (2).To<int> (),
              term.Arg (3).To<int> ());

            if (!t0.Unify (new TimeSpanTerm (ti), varStack)) return false;
          }
          else
          {
            IO.Error ("timespan/4: first argument must be either a TimeSpan or a var");

            return false;
          }
          break;

        case BI.is_: // X is Y
          t0 = term.Arg (1).Eval ();
          if (term.Arg (0).Unify (t0, varStack)) break;
          return false;

        case BI.ne_uni: // X \= Y
          if (term.Arg (0).Unify (term.Arg (1), varStack)) return false;
          break;

        case BI.eq_num: // X =:=
          if (term.Arg<decimal> (0) == term.Arg<decimal> (1)) break;
          return false;

        case BI.ne_num: // X =\= Y
          if (term.Arg<decimal> (0) != term.Arg<decimal> (1)) break;
          return false;

        case BI.lt_num:  // X < Y
          if (term.Arg<decimal> (0) < term.Arg<decimal> (1)) break;
          return false;

        case BI.le_num: // X =< Y
          if (term.Arg<decimal> (0) <= term.Arg<decimal> (1)) break;
          return false;

        case BI.gt_num: // X > Y
          if (term.Arg<decimal> (0) > term.Arg<decimal> (1)) break;
          return false;

        case BI.ge_num: // X >= Y
          if (term.Arg<decimal> (0) >= term.Arg<decimal> (1)) break;
          return false;

        case BI.eq_str: // X == Y
          if (term.Arg (0).CompareTo (term.Arg (1)) == 0) break;
          return false;

        case BI.ne_str: // X \== Y
          if (term.Arg (0).CompareTo (term.Arg (1)) != 0) break;
          return false;

        case BI.lt_ord: // X @< Y
          if (term.Arg (0).CompareTo (term.Arg (1)) < 0) break;
          return false;

        case BI.le_ord: // X @=< Y
          if (term.Arg (0).CompareTo (term.Arg (1)) <= 0) break;
          return false;

        case BI.gt_ord: // X @> Y
          if (term.Arg (0).CompareTo (term.Arg (1)) > 0) break;
          return false;

        case BI.ge_ord: // X @>= Y
          if (term.Arg (0).CompareTo (term.Arg (1)) >= 0) break;
          return false;

        case BI.univ: // X =.. Y
          t0 = term.Arg (0);

          if (t0.IsVar) // create a function or operator representation of the term rhs, and bind that to the lhs
          {
            t1 = term.Arg (1);

            if (t1.IsVar || !t1.IsProperList) return false;

            if (t1.Arg (0).IsVar) return false; // not a valid functor

            functor = t1.Arg (0).FunctorToString.ToAtom ();
            // convert rest of term to arguments: calculate arity first
            t1 = t1.Arg (1);
            arity = 0;
            t2 = t1;

            while (t2.Arity == 2)
            {
              arity++;
              t2 = t2.Arg (1);
            }

            // create arguments
            BaseTerm [] args = new BaseTerm [arity];

            for (int i = 0; i < arity; i++)
            {
              args [i] = t1.Arg (0);
              t1 = t1.Arg (1);
            }

            t0.Unify (CreateNewTerm (t1, arity, functor, args), varStack);

            break;
          }
          else // create a list representation of the lhs and unify that with the rhs
          {
            arity = t0.Arity;
            BaseTerm [] args = new BaseTerm [arity];
            t1 = ListTerm.EMPTYLIST;

            for (int i = arity; i > 0; i--)
              t1 = new ListTerm (t0.Arg (i - 1), t1); // [arg1, arg2, ...]

            t1 = new ListTerm (new AtomTerm (t0.FunctorToString), t1); // [functor, arg1, arg2, ...]

            if (!t1.Unify (term.Arg (1), varStack)) return false;

            break;
          }

        case BI.unifiable: // X can be unified with Y, but without variable bindings
          if (!term.Arg (0).IsUnifiableWith (term.Arg (1), varStack)) return false;
          break;

        #region IO
        #region Reading
        case BI.fileexists:
          t0 = term.Arg (0);

          fileName = Utils.FileNameFromTerm (t0, ".pl");

          if (fileName == null || !File.Exists (fileName)) return false;
          break;

        case BI.see: // see( F)
          t0 = term.Arg (0);

          if (!t0.IsAtomOrString)
            IO.Error ("see/1 argument must be an atom or a string");

          if (t0.HasFunctor ("user"))
          {
            currentFileReader = null;

            break;
          }
          
          if (t0 is FileReaderTerm) // functor previously saved with seeing/1
          {
            currentFileReader = (FileReaderTerm)t0;

            break;
          }

          if (t0.HasFunctor ("user"))
          {
            currentFileReader = null;

            break;
          }

          currentInputName = Utils.FileNameFromTerm (t0, ".pl");
          currentFileReader = (FileReaderTerm)openFiles.GetFileReader (currentInputName);

          if (currentFileReader == null)
          {
            currentFileReader = new FileReaderTerm (this, currentInputName);
            openFiles.Add (currentInputName, currentFileReader);
            currentFileReader.Open ();
          }
          break;

        case BI.seeing:
          if (currentFileReader == null ||
              !term.Arg (0).Unify (currentFileReader, varStack)) return false;

          break;

        case BI.read: // read( ?Term)
          t0 = ReadTerm ();

          if (!term.Arg (0).Unify (t0, varStack)) return false;

          break;

        case BI.readatoms: // readatoms( ?List)
          string line = ReadLine ();

          if (String.IsNullOrEmpty (line = line.Trim ()))
            t0 = ListTerm.EMPTYLIST;
          else
          {
            string [] words = line.Tokens ();
            BaseTerm [] terms = new BaseTerm [words.Length];

            for (int i = 0; i < words.Length; i++)
              terms [i] = TermFromWord (words [i]);

            t0 = ListTerm.ListFromArray (terms);
          }

          if (!term.Arg (0).Unify (t0, varStack)) return false;

          break;

        case BI.readatom: // readatom( A)
          t0 = TermFromWord (ReadLine ());

          if (!term.Arg (0).Unify (t0, varStack)) return false;

          break;

        case BI.readln: // readln( L)
          line = ReadLine ();

          if (line == null || !term.Arg (0).Unify (new StringTerm (line), varStack)) return false;

          break;

        case BI.readeof: // readeof( +F, ?T) -- unify the entire contents of file F with string T
          if ((t0 = term.Arg (0)).IsVar) return false;

          x = Utils.FileNameFromTerm (t0, ".txt");
          string fileContents = null;

          try
          {
            fileContents = File.ReadAllText (x);
          }
          catch (Exception e)
          {
            IO.Error ("Error reading file {0}. Message was:\r\n{1}", x, e.Message);
          }

          if (!term.Arg (1).Unify (new StringTerm (fileContents), varStack)) return false;

          break;

        case BI.get0: // get0( C): any character
          n = ReadChar ();

          if (!term.Arg (0).Unify (new DecimalTerm (n), varStack)) return false;

          break;

        case BI.get: // get( C): skip non-printables
          while (true)
          {
            n = ReadChar ();

            if (!Char.IsControl ((char)n)) break; // break if printable
          }

          if (!term.Arg (0).Unify (new DecimalTerm (n), varStack)) return false;

          break;

        case BI.seen:
          if (currentFileReader != null)
          {
            currentFileReader.Close ();
            openFiles.Remove (currentInputName);
          }

          currentFileReader = null;

          break;
        #endregion Reading

        #region Writing
        case BI.tell: // tell( F)
          t0 = term.Arg (0);
          
          if (!t0.IsAtomOrString)
            IO.Error ("tell/1 argument must be an atom or a string");

          if (t0.HasFunctor ("user"))
          {
            currentFileWriter = null;

            break;
          }

          currentOutputName = Utils.FileNameFromTerm (t0, ".pl");
          currentFileWriter = (FileWriterTerm)openFiles.GetFileWriter (currentOutputName);

          if (currentFileWriter == null)
          {
            currentFileWriter = new FileWriterTerm (this, currentOutputName);
            openFiles.Add (currentOutputName, currentFileWriter);
            currentFileWriter.Open ();
          }
          break;

        case BI.telling:
          if (currentFileWriter == null ||
              !term.Arg (0).Unify (currentFileWriter, varStack)) return false;

          break;

        case BI.write:
          Write (term.Arg (0), true);
          break;

        case BI.writeln: // writeln( X)
          Write (term.Arg (0), true);
          NewLine ();
          break;

        case BI.writef: // writef( X, L) // formatted write, L last
          string ln = null;
          goto case BI.writelnf;
        case BI.writelnf: // writef( X, L) // formatted writeln, L last
          ln = "ln";
          if (!(term.Arg (0) is StringTerm))
            IO.Error ("First argument of write(0}f/2 must be a string", ln);

          if (!(term.Arg (1) is ListTerm))
            IO.Error ("Second argument of write{0}f/2 must be a list", ln);

          string fs = Utils.Format (term.Arg (0), term.Arg (1));

          if (fs == null) return false;

          Write (fs);

          if (term.FunctorToString == "writelnf") NewLine ();

          break;

        case BI.put: // put( C)
          n = term.Arg<int> (0);
          Write (((char)n).ToString ());
          break;

        case BI.nl:
          NewLine ();
          break;

        case BI.tab: // tab( +N)
          n = term.Arg<int> (0);

          if (n > 0) Write (Spaces (n));
          break;

        case BI.errorlevel: // errorlevel( +N) % sets DOS ERRORLEVEL (0..255)
          Environment.ExitCode = term.Arg<int> (0);
          break;

        case BI.print: // print( X)
          Write (term.Arg (0), true);
          break;

        case BI.treeprint: //
          term.Arg (0).TreePrint (0, this);
          break;

        case BI.display:
          Write (term.Arg (0).ToDisplayString (), false);
          NewLine ();
          break;

        case BI.told:
          if (currentFileWriter != null)
          {
            currentFileWriter.Close ();
            openFiles.Remove (currentOutputName);
          }

          currentFileWriter = null;
          break;

        case BI.console:
          if (term.Arity == 2 && !(term.Arg (0) is StringTerm))
            IO.Error ("First argument of console/1/2 must be a string");

          if (term.Arity == 2)
          {
            if (!(term.Arg (1) is ListTerm))
              IO.Error ("Second argument of console/2 must be a list");

            a = Utils.Format (term.Arg (0), term.Arg (1));
            IO.WriteLine (a);
          }
          else
            IO.WriteLine ("{0}", term.Arg (0));

          break;

        case BI.maxwritedepth:
          t0 = term.Arg (0);

          if (t0.IsVar)
            term.Arg (0).Unify (new DecimalTerm (maxWriteDepth), varStack);
          else if (t0.IsNatural)
            maxWriteDepth = t0.To<int> ();
          else
            return false;

          break;

        case BI.cls:
          IO.ClearScreen ();
          break;

        case BI.showfile:
          t0 = term.Arg (0);
          fileName = Utils.FileNameFromTerm (t0, ".pl");

          if (fileName == null || !File.Exists (fileName)) return false;

          IO.WriteLine (File.ReadAllText (fileName));
          break;
        #endregion Writing
        #endregion IO

        #region SQL
        case BI.sql_connect: // sql_connect( +Provider, +Args, -ConnectionInfo)
          t0 = term.Arg (0); // msaccess, sqlserver, ...
          t1 = term.Arg (1); // Uid, Pwd, Server, Dataset, ..., depending on provider
          t2 = term.Arg (2); // Output argument, connection info

          if (dbCommandSet == null)
            dbCommandSet = new DbCommandSet ();

          if (t1.IsAtom || t1.IsString)
            t1 = new ListTerm (t1);
          else if (!t0.IsAtom || !t1.IsProperList)
            return false;

          if (!t2.IsVar)
            IO.Error ("Third argument of console/1/2 must be an uninstantiated variable");

          dbCommand = dbCommandSet.GetCommand (t0, t1);
          t2.Unify (new DbConnectionTerm (dbCommand), varStack);
          break;


        case BI.sql_connection: // sql_connection( +ConnectionInfo, ?Connectstring, ?Command)
          t0 = term.Arg (0); // ConnectionInfo as provided by sql_connect
          t1 = term.Arg (1); // Output argument: connection string
          t2 = term.Arg (2); // Output argument: current command text

          if (!(t0 is DbConnectionTerm))
            IO.Error ("Argument of sql_disconnect/1 must be a ConnectionInfo term produced by sql_connect/3");

          dbCommand = ((DbConnectionTerm)t0).DbCommand;

          if (!t1.Unify (new StringTerm (dbCommand.Connection.ConnectionString), varStack) ||
              !t2.Unify (new StringTerm (dbCommand.CommandText), varStack))
            return false;
          break;


        case BI.sql_disconnect: // sql_disconnect( +ConnectionInfo) disconnect from the database
          t0 = term.Arg (0); // msaccess, sqlserver, ...

          if (!(t0 is DbConnectionTerm))
            IO.Error ("Argument of sql_disconnect/1 must be a ConnectionInfo term produced by sql_connect/3");

          if (dbCommandSet != null) // should always be the case
            dbCommandSet.Close (t0);

          break;


        // sql_command( CI, Cmd) - execute DB command Cmd (insert, delete, update, ...)
        case BI.sql_command: // sql_command( CI, Cmd[, N]) -- Execute SQL-command Cmd. Number of rows afected is N
          t0 = term.Arg (0);
          t1 = term.Arg (1);
          string commandText;
          int rowsAffected;

          if (!(t0 is DbConnectionTerm))
            IO.Error ("First argument of sql_command/2/3 must be a ConnectionInfo term created with sql_connect/3");
          else if ((cnt = (DbConnectionTerm)t0).DbCommand.Connection.State != ConnectionState.Open)
            IO.Error ("Connection is not open. ConnectionInfo:\r\n{0}", cnt.Connectstring);

          if (t1 is ListTerm)
          {
            if ((commandText = Utils.Format ((ListTerm)t1)) == null) return false;
          }
          else
            commandText = t1.ToString ().Dequoted ();

          sqlt = new SqlTerm (t0, commandText, false);
          rowsAffected = sqlt.ExecuteNonQuery ();

          if (term.Arity == 3 && !term.Arg (2).Unify (new DecimalTerm (rowsAffected), varStack))
            return false;

          break;


        case BI.sql_select: // sql_select( State, CI, SelStat, X) - get the next X from State
          t0 = term.Arg (0);

          if (t0.IsVar) // first call only
          {
            t1 = term.Arg (1); // Connection info

            if (!(t1 is DbConnectionTerm))
              IO.Error ("First argument of sql_select/3 must be a ConnectionInfo term created with sql_connect/3");
            else if ((cnt = (DbConnectionTerm)t1).DbCommand.Connection.State != ConnectionState.Open)
              IO.Error ("Connection is not open. ConnectionInfo:\r\n{0}", cnt.Connectstring);

            t2 = term.Arg (2); // SELECT-statement
            string queryText;

            if (t2 is ListTerm)
            {
              if ((queryText = Utils.Format ((ListTerm)t2)) == null) return false;
            }
            else
              queryText = t2.ToString ().Dequoted ();

            sqlt = new SqlTerm (t1, queryText, (term.Arg (3).HasFunctor ("true"))); // cf. help sql_select2
            sqlt.ExecuteQuery ();
            t0.Unify (sqlt, varStack); // bind the SqlTerm to T

            break;
          }

          sqlt = (SqlTerm)t0;

          while (true)
          {
            ListTerm lt = sqlt.SqlNextRecordToListTerm ();

            if (lt == null)
            {
              term.SetArg (0, Variable.VAR);

              return false; // done, no (more) records in resultset
            }

            if (term.Arg (3).Unify (lt, varStack)) break;
          }

          break;
        #endregion SQL

        case BI.config_setting:
          IEnumerator configEnum = null;
          t0 = term.Arg (0);

          if (t0.IsVar) // first call only, Arg(0) contains State info
          {
            configEnum = ConfigurationSettings.AppSettings.AllKeys.GetEnumerator ();
            term.Arg (0).Unify (new UserClassTerm<IEnumerator> (configEnum), varStack);

            break;
          }

          configEnum = ((UserClassTerm<IEnumerator>)t0).UserObject;

          while (true)
          {
            if (!configEnum.MoveNext ())
            {
              term.SetArg (0, Variable.VAR);

              return false;
            }

            string key = configEnum.Current as string;
            AtomTerm at;
            StringTerm st;

            if ((term.Arg (1).IsUnifiableWith (at = new AtomTerm (key.ToAtom ()), varStack)) &&
                 term.Arg (2).IsUnifiableWith (st = new StringTerm (ConfigurationSettings.AppSettings [key]), varStack))
            {
              term.Arg (1).Unify (at, varStack);
              term.Arg (2).Unify (st, varStack);

              break;
            }
          }
          break;

        case BI.between:
          IntRangeTerm irt;

          if (term.Arg (0).IsVar) // first Call only, Arg(0) contains State info
          {
            t1 = term.Arg (1);
            t2 = term.Arg (2);
            bool inf = t2.HasFunctor ("inf") || t2.HasFunctor ("infinity"); // stolen from SWI

            if (term.OneOfArgsIsVar (1, 2)) return false;

            if (!t1.IsInteger || !(t2.IsInteger || inf)) return false;

            irt = new IntRangeTerm (t1, inf ? new DecimalTerm (int.MaxValue) : t2);
            term.Arg (0).Unify (irt, varStack);

            break;
          }

          irt = (IntRangeTerm)term.Arg (0);
          DecimalTerm dt;

          if (!irt.GetNextValue (out dt) ||
              !term.Arg (3).Unify (dt, varStack)) // done
          {
            term.SetArg (0, Variable.VAR);

            return false;
          }
          break;

        case BI.current_op: // current_op( ?Precedence, ?Assoc, ?Functor)
          IEnumerator<OperatorDescr> iEnum = null;
          t0 = term.Arg (0);

          if (t0.IsVar) // first call only, Arg(0) contains State info
          {
            iEnum = opTable.GetEnumerator ();
            term.Arg (0).Unify (new UserClassTerm<IEnumerator<OperatorDescr>> (iEnum), varStack);

            break;
          }

          iEnum = ((UserClassTerm<IEnumerator<OperatorDescr>>)t0).UserObject;

          while (true)
          {
            if (!iEnum.MoveNext ())
            {
              term.SetArg (0, Variable.VAR);

              return false;
            }

            OperatorDescr opDescr = iEnum.Current;

            DecimalTerm it;
            AtomTerm at, nt;

            if (term.Arg (1).IsUnifiableWith (it = new DecimalTerm (opDescr.Prec), varStack) &&
                term.Arg (2).IsUnifiableWith (at = new AtomTerm (opDescr.Assoc.ToString ()), varStack) &&
                term.Arg (3).IsUnifiableWith (nt = new AtomTerm (opDescr.Name), varStack))
            {
              term.Arg (1).Unify (it, varStack);
              term.Arg (2).Unify (at, varStack);
              term.Arg (3).Unify (nt, varStack);

              break;
            }
          }

          break;

        case BI.atom_string: // atom_string( ?A, ?S)
          t1 = term.Arg (1);

          if (t1.IsVar) // create a list containing A's characters or character codes
          {
            t0 = term.Arg (0);

            if (!t0.IsAtomic) return false;

            t2 = NewIsoOrCsStringTerm (t0.FunctorToString.Dequoted ());

            if (!t1.Unify (t2, varStack)) return false;
          }
          else // t1 is string
          {
            if (t1.IsProperList)
            {
              StringBuilder sb = new StringBuilder ();

              while (t1.Arity == 2)
              {
                t2 = t1.Arg (0);

                if (!t2.IsInteger) return false;

                sb.Append ((char)t2.To<int> ());
                t1 = t1.Arg (1);
              }

              if (!term.Arg (0).Unify (TermFromWord (sb.ToString ()), varStack)) return false;
            }
            else if (t1.IsString && (a = t1.FunctorToString).Length > 0)
            {
              if (!term.Arg (0).Unify (TermFromWord (a.ToString ().Dequoted ()), varStack)) return false;
            }
            else
              return false;
          }
          break;

        case BI.string_term: // string_term( ?S, ?T) -- convert string S to Prolog term T and v.v.
          t0 = term.Arg (0);
          t1 = term.Arg (1);

          if (t0.IsString)
          {
            PrologParser p = new PrologParser (this);
            p.StreamIn = "&reading\r\n" + t0.FunctorToString.AddEndDot ();

            if (!t1.Unify (p.ReadTerm, varStack)) return false;
          }
          else if (!t0.Unify (new StringTerm (t1.ToString ()), varStack))
            return false;
          break;

        case BI.string_words:
          t0 = term.Arg (0);
          t1 = term.Arg (1);

          if (t0.IsString)
          {
            ListTerm list;
            line = ((StringTerm)t0).Value;

            if (String.IsNullOrEmpty (line = line.Trim ()))
              list = ListTerm.EMPTYLIST;
            else
            {
              string [] words = line.Tokens ();
              BaseTerm [] terms = new BaseTerm [words.Length];

              for (int i = 0; i < words.Length; i++)
                terms [i] = TermFromWord (words [i]);

              list = ListTerm.ListFromArray (terms);
            }

            if (list.Unify (t1, varStack)) break;
          }
          else if (t1.IsProperList)
          {
            StringBuilder sb = new StringBuilder ();
            bool first = true;

            foreach (BaseTerm t in (ListTerm)t1)
            {
              if (first) first = false; else sb.Append (' ');

              sb.Append (t);
            }

            if (t0.Unify (new StringTerm (sb.ToString ()), varStack)) break;
          }
          return false;

        case BI.stringstyle:
          t0 = term.Arg (0);
          if (t0.IsVar)
            t0.Unify (new AtomTerm (csharpStrings ? "csharp" : "iso"), varStack);
          else
            SetStringStyle (t0);
          break;

        case BI.name: // name( ?A, ?L)
          t1 = term.Arg (1);

          if (t1.IsVar) // create a list containing atom A's characters or character codes
          {
            t0 = term.Arg (0);

            if (!t0.IsAtomic) return false;

            char [] chars = t0.FunctorToString.Dequoted ("'").ToCharArray ();
            t0 = ListTerm.EMPTYLIST;

            for (int i = chars.Length - 1; i >= 0; i--)
            {
              t2 = new DecimalTerm (chars [i]);
              t0 = new ListTerm (t2, t0);
            }

            t1.Unify (t0, varStack);
          }
          else
          {
            if (t1.IsProperList)
            {
              StringBuilder sb = new StringBuilder ();

              while (t1.Arity == 2)
              {
                t2 = t1.Arg (0);

                if (!t2.IsInteger) return false;

                sb.Append ((char)t2.To<int> ());
                t1 = t1.Arg (1);
              }

              a = sb.ToString ().ToAtomic (out type);

              if (type == TermType.Number)
                t2 = new DecimalTerm (int.Parse (a));
              else if (type == TermType.String)
                t2 = NewIsoOrCsStringTerm (a);
              else
                t2 = new AtomTerm (a);

              if (!term.Arg (0).Unify (t2, varStack)) return false;
            }
            else
              return false;
          }
          break;

        case BI.expand_term: // expand_term( +(P-->Q), -R)
          t0 = term.Arg (0); // P-->Q
          t1 = term.Arg (1); // R
          BaseTerm head = t0.Arg (0);
          TermNode body = t0.Arg (1).ToDCG (ref head);
          t2 = new ClauseTerm (new ClauseNode (head, body)).Copy ();

          if (!t1.Unify (t2, varStack)) return false;
          break;

        case BI.numbervars: // numbervars(+X, +B, -E)
          t0 = term.Arg (0);
          t1 = term.Arg (1);
          t2 = term.Arg (2);

          if (!t1.IsInteger || !t2.IsVar) return false;

          int k = (int)t1.To<int> ();
          t0.NumberVars (ref k, varStack);
          t2.Unify (new DecimalTerm (k), varStack);
          break;

        case BI.format: // format/3
          fs = Utils.Format (term.Arg (0), term.Arg (1));

          if (fs == null) return false;

          if (!term.Arg (2).Unify (new StringTerm (fs), varStack)) return false;
          break;


        case BI.username: // username( X)
          if (!term.Arg (0).Unify (new AtomTerm (Environment.UserName), varStack))
            return false;
          break;


        case BI.shell_sync_d:  // shell( dos(Cmd), A, E)
          mustWait = true;
          goto case BI.shell_d;

        case BI.shell_d:  // shell, shell( dos(Cmd) [, A])
          if (term.Arity > 0 && term.Arg (0).Arity > 0)
          {
            t0 = term.Arg (0).Arg (0);

            if (!t0.IsAtomOrString)
              IO.Error ("Illegal command name '{0}'", t0);

            cmd = t0.FunctorToString.Dequoted ();
          }
          goto case BI.shell_dos;

        case BI.shell_dos:
          proc = new Process ();


          if (cmd == null) // just open a DOS-box
          {
            proc.StartInfo.FileName = "cmd.exe";
            proc.Start ();

            break;
          }

          proc.StartInfo.FileName = "cmd.exe";
          cmd = " /k " + cmd + ((term.Arity == 1) ? null : ConvertToCmdArgs (term.Arg (1)));
          proc.StartInfo.Arguments = cmd;
          proc.Start ();

          if (mustWait)
          {
            proc.WaitForExit ();
            // unify the last argument with the value for DOS ERRORLEVEL

            if (!term.Arg (2).Unify (new DecimalTerm (proc.ExitCode), varStack))
              return false;
          }
          break;


        case BI.shell_sync_p:  // shell( Path/Cmd, A, E)
          mustWait = true;
          goto case BI.shell_p;

        case BI.shell_p:  // shell( Path/Cmd, A)
          t0 = term.Arg (0).Arg (0);

          if (!t0.IsAtomOrString)
            IO.Error ("Illegal path name '{0}'", t0);

          t1 = term.Arg (0).Arg (1);

          if (!t1.IsAtomOrString)
            IO.Error ("Illegal command name '{0}'", t0);

          cmd = t0.FunctorToString.Dequoted ();

          if (!cmd.EndsWith (Path.DirectorySeparatorChar.ToString ()))
            cmd = cmd + Path.DirectorySeparatorChar;

          cmd += t1.FunctorToString.Dequoted ();
          goto case BI.shell_exe;

        case BI.shell_sync_x:  // shell( Cmd, A, E)
          mustWait = true;
          goto case BI.shell_x;

        case BI.shell_x:  // shell( Cmd, A)
          t0 = term.Arg (0);

          if (!t0.IsAtomOrString)
            IO.Error ("Illegal command name '{0}'", t0);

          cmd = t0.FunctorToString.Dequoted ();
          goto case BI.shell_exe;

        case BI.shell_exe: // shell_exe( X [,Args] [,wait ,E]). 
          proc = new Process ();
          proc.StartInfo.FileName = cmd;
          proc.StartInfo.Arguments = ConvertToCmdArgs (term.Arg (1)).Trim ().Dequoted ();
          proc.Start ();

          if (mustWait)
          {
            proc.WaitForExit ();
            // unify the last argument with the value for DOS ERRORLEVEL

            if (!term.Arg (2).Unify (new DecimalTerm (proc.ExitCode), varStack))
              return false;
          }
          break;

        case BI.predicatePN: // predicate( +P/N)
          t0 = term.Arg (0);
          result = true;

          if (t0.HasFunctor ("/") && t0.Arity == 2 && t0.Arg (0).IsAtom && t0.Arg (1).IsInteger)
            result = predTable.IsPredicate (t0.Arg (0).FunctorToString, (int)t0.Arg<int> (1));
          else
            result = false;
          if (!result)
            return false;
          break;

        case BI.predicateX: // predicate( +T)
          t0 = term.Arg (0);

          if (t0.IsVar || !predTable.IsPredicate (t0.FunctorToString, t0.Arity)) return false;
          break;

        // term_pattern( T, P, Dmin, Dmax)
        // find pattern P in term T between depths Dmin and Dmax (incl), and unify result with P
        case BI.term_pattern:
          t0 = term.Arg (0); // State
          t1 = term.Arg (1); // term

          if (t1.IsVar)
            IO.Error ("term_pattern: uninstantiated first argument not allowed");

          t2 = term.Arg (2); // pattern
          t3 = term.Arg (3); // Dmin (0 if var)
          t4 = term.Arg (4); // Dmax (inf if var)
          t5 = term.Arg (5); // Path ('!' if not wanted)
          bool skipVars = true; // i.e. the search pattern will not match a term variable in t

          NodeIterator iterable = null;

          if (t0.IsVar) // first call only, Arg(0) contains State info
          {
            iterable = new NodeIterator (t1, t2, t3, t4, skipVars, t5, varStack);
            term.Arg (0).Unify (new UserClassTerm<NodeIterator> (iterable), varStack);

            break;
          }

          iterable = ((UserClassTerm<NodeIterator>)t0).UserObject;

          if (!iterable.MoveNext ()) // if success, pattern gets bound (and its vars get instantiated)
          {
            term.SetArg (0, Variable.VAR);

            return false;
          }

          break;

        case BI.ground: // ground( +T)
          if (!term.Arg (0).IsGround) return false;
          break;

        case BI.throw_: // throw( [+C,] +T [,+L])
          t0 = term.Arg (0);
          t1 = null;
          string exceptionClass = null;
          string exceptionMessage;

          if (term.Arity == 2)
          {
            if (t0 is AtomTerm || t0 is DecimalTerm) // exception class
            {
              exceptionClass = t0.FunctorToString;
              t0 = term.Arg (1);
              t1 = (term.Arity == 2) ? null : term.Arg (2);
            }
            else
              t1 = term.Arg (1);
          }
          else if (term.Arity == 3) // something is wrong
            IO.Error ("First argument of throw/3 ({0}) is not an atom or an integer", t0);

          if (!(t0 is StringTerm))
            IO.Error ("Throw/3: string expected instead of '{0}'", t0);

          exceptionMessage = (t1 == null) ? t0.FunctorToString : Utils.Format (t0, t1);
          Throw (exceptionClass, exceptionMessage);
          break;

        case BI.sendmail:
          string smtp = null;
          string to = null;
          string subj = null;
          string text = null;
          int port = 0;

          if (term.Arity == 3)
          {
            string config = ConfigSettings.SmtpHost;

            if (config == null)
              IO.Error ("No value found for SmtpHost in config file");
            else if (config.IndexOf (':') > 0)
            {
              smtp = config.Split (':') [0];

              if (!int.TryParse (config.Split (':') [1], out port) || port <= 0)
                IO.Error ("Illegal port value '{0}' found for SmtpHost in config file");
            }
            else
            {
              smtp = config;
              port = 25;
            }

            to = term.Arg (0).FunctorToString;
            subj = term.Arg (1).FunctorToString;
            text = term.Arg (2).FunctorToString;
          }
          else if (term.Arity == 4)
          {
            smtp = term.Arg (0).FunctorToString;
            to = term.Arg (1).FunctorToString;
            subj = term.Arg (2).FunctorToString;
            text = term.Arg (3).FunctorToString;
            port = 25;
          }
          else if (term.Arity == 5)
          {
            smtp = term.Arg (0).FunctorToString;
            port = term.Arg (1).To<int> ();
            to = term.Arg (2).FunctorToString;
            subj = term.Arg (3).FunctorToString;
            text = term.Arg (4).FunctorToString;
          }

          if (SendMail (smtp, port, to, subj, text)) break;

          return false;

        case BI.clipboard:
          try
          {
            Utils.SetClipboardData (term.Arg (0).ToString ().Dequoted ());
          }
          catch (System.Runtime.InteropServices.ExternalException e)
          {
            IO.Error ("Copy to clipboard failed, message was:\r\n{0}", e.Message);

            return false;
          };
          break;

        case BI.today: // date( ?Y, ?M, ?D)
          y = DateTime.Today.Year;
          m = DateTime.Today.Month;
          d = DateTime.Today.Day;

          if (!term.Arg (0).Unify (new DecimalTerm (y), varStack)) return false;

          if (!term.Arg (1).Unify (new DecimalTerm (m), varStack)) return false;

          if (!term.Arg (2).Unify (new DecimalTerm (d), varStack)) return false;
          break;

        case BI.now: // time( ?H, ?M, ?S)
          h = DateTime.Now.Hour;
          m = DateTime.Now.Minute;
          s = DateTime.Now.Second;

          if (!term.Arg (0).Unify (new DecimalTerm (h), varStack)) return false;

          if (!term.Arg (1).Unify (new DecimalTerm (m), varStack)) return false;

          if (!term.Arg (2).Unify (new DecimalTerm (s), varStack)) return false;
          break;

        case BI.validdate: // validdate( +Y, +M, +D)
          t0 = term.Arg (0);

          if (t0.IsInteger) y = t0.To<int> (); else return false;

          t1 = term.Arg (1);

          if (t1.IsInteger) m = t1.To<int> (); else return false;

          t2 = term.Arg (2);

          if (t2.IsInteger) d = t2.To<int> (); else return false;

          try
          {
            new DateTime (y, m, d);
          }
          catch
          {
            return false;
          }
          break;

        case BI.validtime: // validtime( +H, +M, +S)
          t0 = term.Arg (0);

          if (t0.IsInteger) h = t0.To<int> (); else return false;

          t1 = term.Arg (1);

          if (t1.IsInteger) m = t1.To<int> (); else return false;

          t2 = term.Arg (2);

          if (t2.IsInteger) s = t2.To<int> (); else return false;

          try
          {
            new DateTime (2000, 1, 1, h, m, s);
          }
          catch
          {
            return false;
          }
          break;

        case BI.string_datetime: // convert a string to a DateTime term
          t0 = term.Arg (0);

          if (term.Arity > 2)
          {
            if (!t0.IsString || !DateTime.TryParse (t0.FunctorToString, out dati))
            {
              IO.Error ("string_datetime: invalid date format: '{0}' for first argument", t0);

              return false;
            }

            if (!term.Arg (1).Unify (new DecimalTerm (dati.Year), varStack) ||
                 !term.Arg (2).Unify (new DecimalTerm (dati.Month), varStack) ||
                 !term.Arg (3).Unify (new DecimalTerm (dati.Day), varStack) ||
                 (term.Arity == 7 &&
                   (!term.Arg (4).Unify (new DecimalTerm (dati.Hour), varStack) ||
                    !term.Arg (5).Unify (new DecimalTerm (dati.Minute), varStack) ||
                    !term.Arg (6).Unify (new DecimalTerm (dati.Second), varStack)
               )))
              return false;
          }
          else
          {
            t1 = term.Arg (1);

            if (t0.IsString)
            {
              if (DateTime.TryParse (t0.FunctorToString, out dati))
              {
                if (!t1.Unify (new DateTimeTerm (dati), varStack))
                  return false;
              }
              else
              {
                IO.Error ("string_datetime: error while parsing first argument: '{0}'", t0);

                return false;
              }
            }
            else if (t0.IsVar)
            {
              if (t1.IsDateTime)
              {
                if (!t0.Unify (new StringTerm (t1.FunctorToString), varStack))
                  return false;
              }
              else
              {
                IO.Error ("string_datetime: second argument is not a DateTime term: '{0}'", t0);

                return false;
              }
            }
            else
            {
              IO.Error ("string_datetime: first argument not a string or var: '{0}'", t0);

              return false;
            }
          }
          break;

        case BI.date_part: // get the date part of a DateTime (time set to 00:00:00)
          t0 = term.Arg (0);

          if (!t0.IsDateTime)
          {
            IO.Error ("date_part: first argument not a DateTime: '{0}'", t0);

            return false;
          }

          if (!term.Arg (1).Unify (new DateTimeTerm (t0.To<DateTime> ().Date), varStack))
            return false;

          break;

        case BI.time_part: // get the time part of a DateTime (time set to 00:00:00)
          t0 = term.Arg (0);

          if (!t0.IsDateTime)
          {
            IO.Error ("date_part: first argument not a DateTime: '{0}'", t0);

            return false;
          }

          if (!term.Arg (1).Unify (new TimeSpanTerm (t0.To<DateTime> ().TimeOfDay), varStack))
            return false;

          break;

        case BI.dayname: // dayname( +Y, +M, +D, ?N)
          DayOfWeek dow;
          t0 = term.Arg (0);

          if (t0.IsInteger) y = t0.To<int> (); else return false;

          t1 = term.Arg (1);

          if (t1.IsInteger) m = t1.To<int> (); else return false;

          t2 = term.Arg (2);

          if (t2.IsInteger) d = t2.To<int> (); else return false;

          try
          {
            dow = new DateTime (y, m, d).DayOfWeek;
          }
          catch
          {
            return false;
          }

          if (!term.Arg (3).Unify (new StringTerm (dow.ToString ("G")), varStack)) return false;
          break;

        case BI.dayofweek: // dayofweek( +Y, +M, +D, ?N)
          t0 = term.Arg (0);

          if (t0.IsInteger) y = t0.To<int> (); else return false;

          t1 = term.Arg (1);

          if (t1.IsInteger) m = t1.To<int> (); else return false;

          t2 = term.Arg (2);

          if (t2.IsInteger) d = t2.To<int> (); else return false;

          try
          {
            n = (int)new DateTime (y, m, d).DayOfWeek;
          }
          catch
          {
            return false;
          }

          if (!term.Arg (3).Unify (new DecimalTerm (n), varStack)) return false;
          break;

        case BI.dayofyear: // dayofyear( +Y, +M, +D, ?N)
          t0 = term.Arg (0);

          if (t0.IsInteger) y = t0.To<int> (); else return false;

          t1 = term.Arg (1);

          if (t1.IsInteger) m = t1.To<int> (); else return false;

          t2 = term.Arg (2);

          if (t2.IsInteger) d = t2.To<int> (); else return false;

          try
          {
            n = (int)new DateTime (y, m, d).DayOfYear;
          }
          catch
          {
            IO.Error ("dayofyear: Invalid date (Y:{0} M:{1} D:{2})", y, m, d);

            return false;
          }

          if (!term.Arg (3).Unify (new DecimalTerm (n), varStack)) return false;

          break;


        case BI.leapyear: // leapyear( +Y)
          t0 = term.Arg (0);

          if (t0.IsInteger) y = (int)t0.To<int> (); else return false;

          if (!DateTime.IsLeapYear (y)) return false;

          break;


        case BI.weekno: // weekno(+Y, +M, +D, ?N) // week Number of date Y-M-D, or current week Number
          if (term.Arity == 4)
          {
            t0 = term.Arg (0);

            if (t0.IsInteger) y = t0.To<int> (); else return false;

            t1 = term.Arg (1);

            if (t1.IsInteger) m = t1.To<int> (); else return false;

            t2 = term.Arg (2);

            if (t2.IsInteger) d = t2.To<int> (); else return false;

            try
            {
              n = Utils.WeekNo (new DateTime (y, m, d));
            }
            catch  // invalid date
            {
              IO.Error ("weekno: Invalid date (Y:{0} M:{1} D:{2})", y, m, d);

              return false;
            }
          }
          else
            n = Utils.WeekNo (DateTime.Today);

          if (!term.Arg (term.Arity - 1).Unify (new DecimalTerm (n), varStack)) return false;

          break;


        case BI.flat: // flat( +X, ?Y)
          t0 = term.Arg (0);

          if (!t0.IsProperOrPartialList) return false;

          if (!term.Arg (1).Unify (((ListTerm)t0).FlattenList (), varStack)) return false;

          break;


        case BI.dcg_flat: // dcg_flat( +X, ?Y)
          t0 = term.Arg (0);

          if (!t0.IsDcgList) return false;

          if (!term.Arg (1).Unify (((DcgTerm)t0).FlattenDcgList (), varStack)) return false;

          break;


        case BI.append2: // conc2( +X, +Y, ?Z), X proper or partial t, Y anything
          t0 = term.Arg (0);

          if (!t0.IsProperOrPartialList) return false;

          if (!term.Arg (2).Unify (((ListTerm)t0).Append (term.Arg (1)), varStack))
            return false;

          break;


        case BI.xml_term: // xml_term( ?X, ?P [,C]) converts between XML and Prolog representation
          BaseTerm ss = null;
          bool settings = (term.Arity == 3);

          t0 = term.Arg (0);
          t1 = term.Arg (1);

          if (settings && !(ss = term.Arg (2)).IsProperList)
            IO.Error ("xml_term: options must be in a list");

          if (t0.IsVar)
          {
            if (t1.IsVar)
            {
              IO.Error ("xml_term: at least one of the arguments must be instantiated");

              return false;
            }

            x = null;

            Node.TermToXml (ss, t1, ref x);

            t2 = NewIsoOrCsStringTerm (x);
            //t2 = new StringTerm (x);

            if (!t0.Unify (t2, varStack)) return false;

            break;
          }
          else
          {
            x = t0.FunctorToString;
            inFile = (t0.Arity == 1 && x == "see");  // is it the functor of a source file containing the XML structure?
            outFile = (t0.Arity == 1 && x == "tell"); // ... or the functor of a destination file containing the XML structure?

            if (inFile || outFile)
            {
              x = Utils.FileNameFromTerm (t0.Arg (0), ".xml");

              if (x == null) return false;
            }

            if (outFile)
            {
              if (t1.IsVar) return false;

              Node.TermToXml (ss, t1, ref x); // t1 is Prolog term
            }
            else
            {
              t2 = Node.XmlToTerm (ss, x, inFile);

              if (!t1.Unify (t2, varStack)) return false;
            }

            break;
          }


        case BI.xml_transform:
          string xmlFileName = term.Arg (0).FunctorToString;

          if (term.Arity == 1)
            XmlTransform (xmlFileName);
          else
          {
            string xslFileName = term.Arg (1).FunctorToString;

            if (term.Arity == 2)
              XmlTransform (xmlFileName, xslFileName);
            else
              XmlTransform (xmlFileName, xslFileName, term.Arg (2).FunctorToString);
          }
          break;


        case BI.json_term: // json_term( ?J, ?T) converts between JSON file/string and Prolog representation
          int indentDelta = 2;
          int maxIndentLevel = int.MaxValue;
          bool noCommas = false;
          bool noQuotes = false;
          JsonTerm jt = null;

          if (term.Arity == 3)
            predicateCallOptions.Set (term.Arg (2));

          t0 = term.Arg (0);
          t1 = term.Arg (1);

          if (t0.IsVar)
          {
            if (t1 is JsonTerm)
              jt = (JsonTerm)t1;
            else
            {
              if (t1.IsProperList)
                jt = new JsonTerm ((ListTerm)t1);
              else
                IO.Error (
                  "json_term/2/3 -- second argument cannot be converted to a JSON-structure:\r\n'{0}'", t1);
            }

            predicateCallOptions.Get<int> ("indent", 1, ref indentDelta);
            predicateCallOptions.Get<int> ("indent", 2, ref maxIndentLevel);
            noCommas = predicateCallOptions.Get ("nocommas");
            noQuotes = predicateCallOptions.Get ("noquotes");

            if (!t0.Unify (
              new StringTerm (jt.ToJsonString (indentDelta, maxIndentLevel, noCommas, noQuotes)),
              varStack)) return false;
            break;
          }
          else // t0 contains string in JSON-format (or file name of JSON text file)
          {
            //if (options != null)
            //  jt.Atomize = options.Get ("atomize");

            x = t0.FunctorToString;
            inFile = (t0.Arity == 1 && x == "see");  // is it the functor of a source file containing the XML structure?
            outFile = (t0.Arity == 1 && x == "tell"); // ... or the functor of a destination file containing the XML structure?

            if (inFile || outFile)
            {
              x = Utils.FileNameFromTerm (t0.Arg (0), ".json");

              if (x == null) return false;
            }

            if (outFile)
            {
              if (t1 is JsonTerm)
                jt = (JsonTerm)t1;
              else
              {
                if (t1.IsProperList)
                  jt = new JsonTerm ((ListTerm)t1);
                else
                  IO.Error (
                    "json_term/2/3 -- second argument cannot be converted to a JSON-structure:\r\n'{0}'", t1);
              }

              predicateCallOptions.Get<int> ("indent", 1, ref indentDelta);
              predicateCallOptions.Get<int> ("indent", 2, ref maxIndentLevel);
              noCommas = predicateCallOptions.Get ("nocommas");
              noQuotes = predicateCallOptions.Get ("noquotes");

              File.WriteAllText (x,
                ((JsonTerm)jt).ToJsonString (indentDelta, maxIndentLevel, noCommas, noQuotes));
            }
            else // parse JSON-string into JsonTerm
            {
              JsonParser p = new JsonParser ();
              p.OpTable = opTable;

              if (inFile)
                p.StreamIn = File.ReadAllText (x);
              else
                p.StreamIn = x;

              if (!t1.Unify (p.JsonListTerm, varStack)) return false;
            }

            break;
          }


        case BI.listing: // listing
          if (!predTable.ListAll (null, -1, false, true)) return false; // i.e. no predefined, all user

          break;


        case BI.crossref: // create cross reference table and write it to a spreadsheet
          t0 = term.Arg (0);

          if (t0.IsAtomOrString)
          {
            fileName = Utils.FileNameFromTerm (t0, ".csv");

            if (fileName == null) return false;

            IO.Write ("\r\n--- Creating cross reference spreadsheet '{0}' ... ", fileName);
            predTable.CrossRefTableToSpreadsheet (fileName);
            IO.WriteLine ("Done");

            break;
          }

          return IO.Error ("Illegal spreadsheet file name '{0}'", t0.Arg (0));


        case BI.profile:
          SetProfiling (true);
          break;


        case BI.noprofile:
          SetProfiling (false);
          break;


        case BI.showprofile:
          if (!profiling)
          {
            IO.Message ("Profiling is not on. Use profile/0 to switch it on");

            return false;
          }

          if (term.Arity == 0)
            predTable.ShowProfileCounts (int.MaxValue);
          else
          {
            t0 = term.Arg (0);

            if (t0.IsNatural)
              predTable.ShowProfileCounts (t0.To<int> ());
            else
              return IO.Error ("Argument for profile/0 must be a positive integer value");
          }

          break;


        case BI.clearprofile:
          predTable.ClearProfileCounts ();
          break;


        case BI.workingdir: // workingdir[( ?D)] -- gets or sets the working directory
          if (!ConfigSettings.SetWorkingDirectory (term, varStack)) return false;

          break;


        case BI.listingXN: // listing( X/N)
          t0 = term.Arg (0);
          result = true;

          if (t0.HasFunctor ("/") && t0.Arity == 2 && t0.Arg (0).IsAtom && t0.Arg (1).IsInteger)
            result = predTable.ListAll (t0.Arg (0).FunctorToString, t0.Arg<int> (1), false, true);
          else
            result = false;

          if (!result) return false;

          break;


        case BI.listingX: // listing( X) -- t all predicates X/N (i.e. for each N)
          t0 = term.Arg (0);

          if (!t0.IsAtom) return false;

          if (!predTable.ListAll (t0.FunctorToString, -1, false, true)) return false;

          break;

        case BI.listing0: // listing0
          if (!predTable.ListAll (null, -1, true, false)) return false; // i.e. no user, all predefined

          break;


        case BI.listing0XN: // listing0( X/N)
          t0 = term.Arg (0);
          result = true;

          if (t0.HasFunctor ("/") && t0.Arity == 2 && t0.Arg (0).IsAtom && t0.Arg (1).IsInteger)
            result = predTable.ListAll (t0.Arg (0).FunctorToString, t0.Arg<int> (1), true, false);
          else
            result = false;

          if (!result) return false;

          break;


        case BI.listing0X: // listing0( X)
          t0 = term.Arg (0);

          if (!t0.IsAtom) return false;

          if (!predTable.ListAll (t0.FunctorToString, -1, true, false)) return false;

          break;


        //case BI.pp_defines: // pp_defines( X) -- preprocessor symbol definitions -- mainly useful for debugging in nested calls
        //  t0 = term.Arg (0);
        //  if (!t0.IsVar) return false;
        //  t1 = ListTerm.EMPTYLIST;
        //  //IO.WriteLine ("PrologParser.PpSymbols.count = {0}", PrologParser.PpSymbols.Count);
        //  foreach (DictionaryEntry de in PrologParser.PpSymbols)
        //    t1 = new CompoundTerm ((de.Key as string).ToAtom (), new Variable (), t1);
        //  t0.Unify (t1, varStack);
        //  break;

        case BI.undefineds:
          predTable.FindUndefineds ();
          break;


        case BI.copy_term: // copy_term( X, Y)
          if (!term.Arg (1).Unify (term.Arg (0).Copy (true, false), varStack)) return false;
          break;


        case BI.clearall: // clearall
          Reset ();
          break;


        case BI.spypoints: // spypoints
          predTable.ShowSpypoints ();
          break;


        case BI.clause: // clause (Head,Body)
          BaseTerm state = term.Arg (0); // State
          t1 = term.Arg (1); // head
          t2 = term.Arg (2); // body

          if (t1.IsVar)
          {
            IO.Error ("First argument of clause/2 is not sufficiently instantiated");

            return false;
          }

          ClauseIterator iterator = null;

          if (state.IsVar) // first call only
          {
            iterator = new ClauseIterator (predTable, t1, varStack);
            state.Unify (new UserClassTerm<ClauseIterator> (iterator), varStack);

            break;
          }

          iterator = ((UserClassTerm<ClauseIterator>)state).UserObject;

          if (!iterator.MoveNext ()) // sets iterator.ClauseBody
          {
            term.SetArg (0, Variable.VAR);

            return false;
          }

          if (!t2.Unify (iterator.ClauseBody, varStack)) return false;

          break;


        case BI.member: // member( X, L)
          if ((t0 = term.Arg (0)).IsVar || !(t1 = term.Arg (1)).IsListNode)
            return false;

          result = false;

          while (t1.Arity == 2)
          {
            if (result = t0.Unify (t1.Arg (0), varStack)) break;

            t1 = t1.Arg (1);
          }

          currentCp.Kill (); // no backtracking to follow -> remove the choicepoint for the alternative clauses

          if (!result) return false;

          break;

        // BACKTRACKING VERSION
        //          while (t1.Arity == 2)
        //          {
        //            if (result = t0.Unify (t1.Arg (0), varStack))
        //            {
        //              if ((t0 = t1.Arg (1)).Arity == 0) // empty t
        //                currentCp.Kill (); // no backtracking to follow -> remove the choicepoint for the alternative clauses
        //              else
        //                head.Arg (2).Bind (t0);  // set Rest to remainder of t (for backtracking)
        //
        //              break;
        //            }
        //            t1 = t1.Arg (1);
        //          }

        //case BI.append: // append( [_|_], [_|_], L)
        //  if (!(t0 = head.Arg (0)).IsProperOrPartialList) return false;

        //  t1 = head.Arg (1);

        //  if (!head.Arg (2).Unify (((BaseTermListTerm)t0).Append (t1), varStack))
        //    return false;

        //  break;


        case BI.regex_match: // regex_match( +Source, +Pattern, ?Result [, Options])
          // String 'Source' is matched against C# regular expression string 'Pattern'.
          // 'Result' is a list containing the matching regular expression groups (a C# regex group
          // is a subpattern enclosed in parentheses, with an optional name or number). 
          // In 'Result' a group is represented as a label (group number or group name)
          // followed by a ':', followed by a list of strings (captures) belonging to that group.
          // 
          // Example:
          //
          // regex_match("21-02-1951", "(?<Month>\\d{1,2})-(\\d{1,2})-(?<Year>(?:\\d{4}|\\d{2}))", L).
          //
          // L = [1:["02"], "Month":["21"], "Year":["1951"]]
          //
          // If there is only one group, only the group name and the list of captures will be returned.
          // If the group has no name, only the list of captures will be returned.
          // If no group was present in the pattern, the empty list will be returned in case of a match.
          // If there was no match, the predicate will fail.
          // 
          // 'Options' is an optional list containing regex options a la C#. The following options are 
          // supported: ignorecase, multiline, singleline, explicitcapture, cultureinvariant
          //
          if (!((t0 = term.Arg (0)).IsString || t0.IsAtom) || !((t1 = term.Arg (1)).IsString || !t1.IsAtom)) return false;

          string [] optionsOLD = (term.Arity == 4) ? ((ListTerm)term.Arg (3)).ToStringArray () : null;
          BaseTerm groups = Utils.FindRegexMatches (opTable, t0.FunctorToString, t1.FunctorToString, optionsOLD);

          if (groups == null || !term.Arg (2).Unify (groups, varStack)) return false;

          break;


        case BI.regex_replace: // regex_replace( S, P, R, T)
          t0 = term.Arg (0);
          t1 = term.Arg (1);
          t2 = term.Arg (2);
          t3 = term.Arg (3);

          if (!(t0 is StringTerm && t1 is StringTerm && t2 is StringTerm))
            IO.Error ("regex_replace/4 -- first three arguments must be strings");

          string input = t0.FunctorToString;
          string pattern = t1.FunctorToString;
          string replacement = t2.FunctorToString;

          if (!t3.Unify (new StringTerm (Regex.Replace (input, pattern, replacement)), varStack))
            return false;

          break;


        case BI.xmltrace: // xmltrace( X) -- send the execution tree of the next command to file X
          // Must be the first goal of a command, in order to avoid problems that
          // arise when it gets involved in backtracking.
          if (xmlTrace)
            IO.Error ("Execution trace is already being logged in {0}", xmlFile);
          else if (!(t0 = term.Arg (0)).IsAtom && !t0.IsString)
            IO.Error ("Not a valid file name: '{0}'", t0);
          else if (!firstGoal)
          {
            IO.WriteLine ("{0}*** {1}/1/2 must be the first goal of the query -- ignored{0}",
                          Environment.NewLine, term.FunctorToString);
            break;
          }
          else if ((xmlFile = Utils.FileNameFromTerm (t0, ".xml")) == null)
            return false;

          n = (term.Arity == 2) ? term.Arg<int> (1) : INF;

          if (n < 1) IO.Error ("Maximum number of elements must exceed 0");

          XmlTraceOpen (term.FunctorToString, n);

          break;


        case BI.numcols: // numcols( N) -- Number of columns in the DOS-box
#if mswindows
          if (!term.Arg (0).Unify (new DecimalTerm (Utils.NumCols), varStack)) return false;

          break;
#else
          return false;
#endif

        case BI.userroles:
#if mswindows
          WindowsIdentity ident = WindowsIdentity.GetCurrent ();
          WindowsPrincipal principal = new WindowsPrincipal (ident);
          //bool admin = principal.IsInRole (WindowsBuiltInRole.Administrator);
          IO.Message ("{0} belongs to: ", principal.Identity.Name.ToString ());

          Array wbirFields = Enum.GetValues (typeof (WindowsBuiltInRole));

          foreach (object roleName in wbirFields)
          {
            try
            {
              IO.Message ("{0}? {1}.", roleName, principal.IsInRole ((WindowsBuiltInRole)roleName));
            }
            catch (Exception)
            {
              IO.Message ("Could not obtain role for RID {0}", roleName);
            }
          }
#else
          IO.Error ("userroles only available if compiler symbol mswindows is defined");
#endif
          break;


        // this version actually only returns the number of ClockTicks
        case BI.statistics: // statistics( X, [MSec,_])
          t1 = term.Arg (1).Arg (0);
          long time = ClockTicksMSecs ();

          if (!t1.Unify (new DecimalTerm (time), varStack)) return false;

          break;

        // not operational yet, some next version
        case BI.callstack: // callstack( S, L) -- S is string, L is list representation of current call stack
          //string str;
          //ListTerm lst;
          ////CallStack (out str, out lst);
          //if (!term.Arg (0).Unify (new StringTerm (str), varStack) ||
          //   (term.Arity == 2 && !term.Arg (1).Unify (lst, varStack)))
          //  return false;
          break;


        case BI.stacktrace: // stacktrace( Mode). If on: show C# exception stacktrace; if off: don't
          t0 = term.Arg (0);

          if (t0.IsVar && !t0.Unify (new AtomTerm (userSetShowStackTrace ? "on" : "off"), varStack)) return false;

          string mode = t0.FunctorToString;

          if (mode == "on")
            userSetShowStackTrace = true;
          else if (mode == "off")
            userSetShowStackTrace = false;
          else
            IO.Error (":- stacktrace: illegal argument '{0}'; use 'on' or 'off' instead", mode);

          break;

        case BI.ip_address: // return local IP-address
          IPHostEntry host = Dns.GetHostEntry (Dns.GetHostName ());

          foreach (IPAddress ip in host.AddressList)
          {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
              if (!term.Arg (0).Unify (new AtomTerm (ip.ToString ()), varStack))
                return false;

              if (term.Arity == 2)
              {
                string [] mask = ip.ToString ().Split (new char [] { '.' });
                ListTerm fields = ListTerm.EMPTYLIST;

                for (int i = mask.Length - 1; i >= 0; i--)
                  fields = new ListTerm (new DecimalTerm (decimal.Parse (mask [i])), fields);

                if (!term.Arg (1).Unify (fields, varStack))
                  return false;
              }

              break;
            }
          }
          break;

        case BI.environment: // environment( X, Y) -- unify Y with Atom value of environment variable X
          t0 = term.Arg (0);
          string es;

          if (!t0.IsAtom) return false;

          switch (t0.FunctorToString.ToLower ())
          {
            case "applicationdata":
              es = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
              break;
            case "localapplicationdata":
              es = Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData);
              break;
            case "cookies":
              es = Environment.GetFolderPath (Environment.SpecialFolder.Cookies);
              break;
            case "desktopdirectory":
              es = Environment.GetFolderPath (Environment.SpecialFolder.DesktopDirectory);
              break;
            case "internetcache":
              es = Environment.GetFolderPath (Environment.SpecialFolder.InternetCache);
              break;
            case "programfiles":
              es = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFiles);
              break;
            case "startup":
              es = Environment.GetFolderPath (Environment.SpecialFolder.Startup);
              break;
            case "commandline":
              es = Environment.CommandLine;
              break;
            case "currentdirectory":
              es = Environment.CurrentDirectory;
              break;
            case "machinename":
              es = Environment.MachineName;
              break;
            case "newline":
              es = Environment.NewLine;
              break;
            case "osversion":
              es = Environment.OSVersion.ToString ();
              break;
            case "stacktrace":
              es = Environment.StackTrace;
              break;
            case "systemdirectory":
              es = Environment.SystemDirectory;
              break;
            case "tickcount":
              es = Environment.TickCount.ToString ();
              break;
            case "userdomainname":
              es = Environment.UserDomainName;
              break;
            case "userinteractive":
              es = Environment.UserInteractive.ToString ();
              break;
            case "username":
              es = Environment.UserName;
              break;
            case "version":
              es = Environment.Version.ToString ();
              break;
            case "workingset":
              es = Environment.WorkingSet.ToString ();
              break;
            default:
              IO.Warning ("Unrecognized first argument '{0}' for environment/2", term.Arg (0));
              return false;
          }

          if (!term.Arg (1).Unify (new StringTerm (es), varStack)) return false;

          break;


        case BI.query_timeout:
          t0 = term.Arg (0);

          if (t0.IsVar)
          {
            t0.Unify (new DecimalTerm (queryTimeout), varStack);

            break;
          }

          if (!t0.IsInteger || (queryTimeout = t0.To<int> ()) < 0) return false;

          break;

        case BI.make_help_resx:
          string resxFileName;
          Utils.CreateHelpResourceFile (out resxFileName);
          IO.WriteLine ("\r\n  Resource file '{0}' created.", Path.GetFullPath (resxFileName));
          IO.WriteLine ("\r\n  Exclude the previous version from the project, add it again, and build.");
          break;

        case BI.get_counter:
          globalTermsTable.getctr (term.Arg (0).FunctorToString, out cntrValue);

          if (!term.Arg (1).Unify (new DecimalTerm (cntrValue), varStack)) return false;
          break;

        case BI.set_counter:
          t0 = term.Arg (0);

          if (!(t0.IsAtom || t0.IsNatural))
            IO.Error ("set_counter: first argument ({0}) must be an atom or a non-negative integer", t0);

          globalTermsTable.setctr (t0.FunctorToString, term.Arg (1).To<int> ());
          break;

        case BI.inc_counter:
          a = term.Arg (0).FunctorToString;
          globalTermsTable.getctr (a, out cntrValue);

          if (term.Arity == 2 &&
              !term.Arg (1).Unify (new DecimalTerm (cntrValue + 1), varStack))
            return false;

          globalTermsTable.incctr (a);
          break;

        case BI.dec_counter:
          a = term.Arg (0).FunctorToString;
          globalTermsTable.getctr (a, out cntrValue);

          if (term.Arity == 2 &&
              !term.Arg (1).Unify (new DecimalTerm (cntrValue - 1), varStack))
            return false;

          globalTermsTable.decctr (a);
          break;

        case BI.getenvvar:
          t0 = term.Arg (0);

          if (!(t0 is StringTerm || t0 is AtomTerm))
          {
            IO.Error ("setenvvar: first argument ({0}) must be an atom or a string", t0);

            return false;
          }

          a = Environment.GetEnvironmentVariable (t0.FunctorToString, EnvironmentVariableTarget.Machine);
          decimal ev;

          if (Decimal.TryParse (a, out ev))
            t1 = new DecimalTerm (ev);
          else
            t1 = new StringTerm (a);

          if (!term.Arg (1).Unify (t1, varStack)) return false;
          break;

        case BI.setenvvar:
          t0 = term.Arg (0);

          if (!(t0 is StringTerm || t0 is AtomTerm))
          {
            IO.Error ("setenvvar: first argument ({0}) must be an atom or a string", t0);

            return false;
          }

          Environment.SetEnvironmentVariable (
            t0.FunctorToString, term.Arg (1).ToString (), EnvironmentVariableTarget.Machine);
          break;

        case BI.getvar:
          globalTermsTable.getvar (term.Arg (0).FunctorToString, out t1);
          if (!term.Arg (1).Unify (t1, varStack)) return false;
          break;

        case BI.setvar:
          if (!(term.Arg (0) is AtomTerm)) return false;

          globalTermsTable.setvar (term.Arg (0).FunctorToString, term.Arg (1).Copy ());
          break;

        /* Main program sample
   
          const string str = "Better ask the way than to go astray!";

          byte[] buffer_in = Encoding.UTF8.GetBytes(str);
          byte[] buffer_out = new byte[buffer_in.Length];
          byte[] buffer_decode = new byte[buffer_in.Length];

          BWTImplementation bwt = new BWTImplementation();

          int primary_index = 0;
          bwt.bwt_encode(buffer_in, buffer_out, buffer_in.Length, ref primary_index);
          bwt.bwt_decode(buffer_out, buffer_decode, buffer_in.Length, primary_index);

          Console.WriteLine("Decoded string: {0}", Encoding.UTF8.GetString(buffer_decode));

       */

        // Burrows–Wheeler transform, cf. https://en.wikipedia.org/wiki/Burrows%E2%80%93Wheeler_transform
        case BI.bw_transform: // bw_transform( ?Plain, ?Encoded, ?Index)
          byte [] buffer_in;
          byte [] buffer_out;
          byte [] buffer_decode;
          t0 = term.Arg (0);
          t1 = term.Arg (1);
          t2 = term.Arg (2);
          BWTImplementation bwt = new BWTImplementation ();       

          if (t1.IsVar) // encode t0 yielding t1
          {
            if (!t0.IsString || !t2.IsVar) return false;

            buffer_in = Encoding.UTF8.GetBytes (t0.FunctorToString);
            buffer_out = new byte [buffer_in.Length];
            int primary_index = 0;
            bwt.bwt_encode (buffer_in, buffer_out, buffer_in.Length, ref primary_index);
            t1.Unify (new StringTerm (Encoding.UTF8.GetString (buffer_out)), varStack);
            t2.Unify (new DecimalTerm (primary_index), varStack);
          }
          else if (t1.IsString && t2.IsInteger)
          {
            buffer_out = Encoding.UTF8.GetBytes (t1.FunctorToString);
            buffer_decode = new byte [buffer_out.Length];
            int primary_index = t2.To<int> ();
            bwt.bwt_decode (buffer_out, buffer_decode, buffer_out.Length, primary_index);
            string decoded = Encoding.UTF8.GetString (buffer_decode);

            if (t0.IsVar) // decode t1 into t0
              t0.Unify (new StringTerm (decoded), varStack);
            else if (t0.IsString) // t1 decoded must be equal to t0
              return (t0.FunctorToString == decoded);
            else
              return false;
          }
          else
            return false;
          break;      
      }
      #endregion switch

      goalListHead = goalListHead.NextNode;

      if (reporting) // advance the goalList until a non-spypoint is encountered. Show spy(-Exit)-info.
      {
        TermNode sp = null;

        while (goalListHead is SpyPoint)
        {
          sp = ((SpyPoint)goalListHead).SaveGoal;

          if (Debugger (SpyPort.Exit, sp, null, false, 1)) break;

          goalListHead = sp.NextNode;
        }
      }

      findFirstClause = true;

      return true;
    }


    string ConvertToCmdArgs (BaseTerm t)
    {
      if (t.IsProperList)
      {
        StringBuilder sb = new StringBuilder ();

        foreach (string s in ((ListTerm)t).ToStringArray ())
          sb.AppendFormat (" {0}", s);

        return sb.ToString ();
      }

      if (t is Variable) return null;

      return ' ' + t.FunctorToString;
    }


    BaseTerm TypedTerm (string a)
    {
      BaseTerm t2;
      TermType type;

      a = a.ToAtomic (out type);

      if (type == TermType.Number)
        t2 = new DecimalTerm (decimal.Parse (a, CIC));
      else
        t2 = new AtomTerm (a);

      return t2;
    }


    BaseTerm TermFromWord (string word)
    {
      TermType type;
      word = word.ToAtomic (out type);

      if (type == TermType.Number)
        try
        {
          return new DecimalTerm (decimal.Parse (word, NumberStyles.Any, CIC));
        }
        catch
        {
          throw new ParserException ("*** Unable to convert \"" + word + "\" to a number");
        }
      else
        return new AtomTerm (word);
    }


    bool SetCaching (BaseTerm term, bool value)
    {
      bool result = false;

      if (term.Arity == 0) // all predicates
      {
        predTable.SetCaching (null, 0, value);
        result = true;
      }
      else if (term.Arity == 1) // specific predicate, potentially with an arity
      {
        BaseTerm t0 = term.Arg (0);

        if (t0.HasFunctor ("/") && t0.Arity == 2 && t0.Arg (0).IsAtom && t0.Arg (1).IsInteger)
          result = predTable.SetCaching (t0.Arg (0).FunctorToString, t0.Arg<int> (1), value);
        else if (term.Arg (0).IsAtom) // predicate functor without arity: take all arities
          result = predTable.SetCaching (term.Arg (0).FunctorToString, -1, value);
      }

      return result;
    }


    BaseTerm CreateNewTerm (BaseTerm t, int arity, string functor, BaseTerm [] args)
    {
      OperatorDescr od;

      if (arity == 0)
      {
        if (opTable.HasOpDef (functor))
          t = new OperatorTerm (functor);
        else
          t = new AtomTerm (functor.ToAtom ());
      }
      else if (arity == 1 && opTable.IsUnaryOperator (functor, out od))
        t = new OperatorTerm (od, args [0]);
      else if (arity == 2 && opTable.IsBinaryOperator (functor, out od))
        t = new OperatorTerm (od, args [0], args [1]);
      else if (arity == 2 && functor == PrologParser.DOT)
        t = new ListTerm (args [0], args [1]);
      else
        t = new CompoundTerm (functor, args);

      return t;
    }
  }
}
