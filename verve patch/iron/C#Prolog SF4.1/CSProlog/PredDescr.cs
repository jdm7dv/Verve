#define enableSpying
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
using System.Collections.Generic;

namespace Prolog
{
  [Flags]
  public enum SpyPort : short { None=0, Call=1, Exit=2, Fail=4, Redo=8, Full=15 }
  public enum CachePort { Exit, Fail }

  public partial class PrologEngine
  {
    public partial class PredicateDescr
    {
      string module; // TO BE IMPLEMENTED
      string definitionFile;
      protected string functor;
      public string Functor { get { return functor; } }
      protected int arity;
      public int Arity { get { return arity; } }
      public string Name { get { return functor + '/' + arity; } }
      public string Key { get { return arity + functor; } }
      const short ARG0COUNT_MIN = 8;
      Dictionary<object, ClauseNode> arg0Index = null; // for first argument indexing
      ClauseNode clauseList;         // start of list of clauses making up this predicate
      public ClauseNode ClauseList { get { return clauseList; } }
      ClauseNode lastCachedClause;   // if the predicate is cacheable, each 'calculated' answer is inserted after this node
      ClauseNode clauseListEnd;      // keeps track of the position of the last clause
      bool isDiscontiguous = false;  // pertains to a single definitionFile only. A predicate must always be in a single definitionFile
      bool isCacheable = false;      // indicates whether in principle the result of the predicate evaluation can be cached (tabled)
      int profileCount = 0;          // number of times the predicate was called
      public bool IsDiscontiguous { get { return isDiscontiguous; } set { isDiscontiguous = value; } }
      public bool IsCacheable { get { return isCacheable; } set { isCacheable = value; } }
      public bool HasCachedValues { get { return lastCachedClause != null; } }
      public int ProfileCount { get { return profileCount; } set { profileCount = value; } }
      public void IncProfileCount ()
      {
        profileCount++;
      }
#if enableSpying
      bool spied;
      public bool Spied { get { return spied; } }
      SpyPort spyMode;
      public SpyPort SpyPort { get { return spyMode; } }
#endif
      public string Module { get { return module; } set { module = value; } }
      public string DefinitionFile { get { return definitionFile; } }

      public PredicateDescr (string module, string definitionFile, string functor, int arity, ClauseNode clauseList)
      {
        this.module = module;
        this.definitionFile = (definitionFile == null) ? "predefined or asserted predicate" : definitionFile;
        this.functor = functor;
        this.arity = arity;
#if enableSpying
        spyMode = SpyPort.None;
#endif
        this.clauseList = clauseListEnd = clauseList;
        this.lastCachedClause = null; // no cached clauses (yet)
      }


      public void SetClauseListHead (ClauseNode c)
      {
        clauseList = clauseListEnd = c;

        while (clauseListEnd.NextClause != null) clauseListEnd = clauseListEnd.NextClause;

        DestroyFirstArgIndex ();
      }


      public void AdjustClauseListEnd () // forward clauseListEnd to the last clause
      {
        if ((clauseListEnd = clauseList) != null)
          while (clauseListEnd.NextClause != null) clauseListEnd = clauseListEnd.NextClause;
      }


      public void AppendToClauseList (ClauseNode c) // NextClause and ClauseListEnd are != null
      {
        clauseListEnd.NextClause = c;

        do
          clauseListEnd = clauseListEnd.NextClause;
        while
          (clauseListEnd.NextClause != null);

        DestroyFirstArgIndex ();
      }

      // CACHEING CURRENTLY NOT USED
      #region Cacheing
      // Cache -- analogous to asserting a fact, but specifically for cacheing.
      // Cached terms are inserted at the very beginning of the predicate's clause
      // chain, in the order in which they were determined.
      public void Cache (BaseTerm cacheTerm, bool succeeds)
      {
        IO.WriteLine ("Cacheing {0}{1}", cacheTerm, succeeds ? null : " :- !, fail");

        CachedClauseNode newCachedClause = new CachedClauseNode (cacheTerm, null, succeeds);

        if (lastCachedClause == null) // about to add the first cached term
        {
          newCachedClause.NextClause = clauseList;
          clauseList = newCachedClause;
        }
        else
        {
          newCachedClause.NextClause = lastCachedClause.NextClause;
          lastCachedClause.NextClause = newCachedClause;
        }

        lastCachedClause = newCachedClause;
      }


      public void Uncache ()
      {
        if (HasCachedValues) // let clauseList start at the first non-cached clause again
        {
          clauseList = lastCachedClause.NextClause;
          lastCachedClause = null;
        }
      }
      #endregion Cacheing

      /* First-argument indexing

         A dictionary is maintained of clauses that have a nonvar first argument.
         Each predicate has its own dictionary. The functors of these first arguments are
         distinct (that is, in the dictionary). For predicate clauses that have identical
         first argument functors, only the first of these clauses is included in the
         dictionary. The clauses are traversed in the order in which they were read in.

         If a clause with a *variable* first argument is encountered, it is added
         to the dictionary as well, but no more entries wil be added after that.
         It will serve as a catch-all for instantiated arguments that do not match with
         any of the other dictionary entries.

         First-argument indexing is applied when a goal is about to be resolved. If the
         first argument of the goal is a bound term, it will be looked up in the
         dictionary. If it is found, the first clause of the defining predicate (cf.
         ExecuteGoalList()) is set to the dictionary entry. If it is not found, and the
         dictionary has a nonvar-entry, then the first clause is set to that
         entry. The (nonvar) goal fails if it is not found in the dictionary.

         First-argument indexing is not used upon backtracking. Choicepoints are simply
         the next clauses after the current one.

         Notice that this implementation of first-argument indexing is of limited use if
         there are many similar first arguments that are scattered throughout the
         predicate clauses, especially when backtracking takes place.
         A more sopisticated scheme could be envisaged where all similar fa-clauses are
         chained together. Now, for optimal use the user has to take care that all similar
         fa-clauses are grouped together (which seems advisable anyway).
      */

      const bool VARARG = true;

      public bool CreateFirstArgIndex ()
      {
        return CreateFirstArgIndex (false); // false: do not create if it already exists
      }


      public bool CreateFirstArgIndex (bool force) // Create the index if the predicate qualifies
      {
        if (arg0Index != null && !force) return false; // index already exists

        // Check each nextClause whether with the addition of this nextClause the predicate
        // still qualifies for first argument indexing.
        // Indexing y/n must be (re)determined after a file consult or an assert.

        ClauseNode c  = clauseList;
        short arg0Count = 0;

        while (c != null)
        {
          if (c.Head.Arity != 0) // no first arg
          {
            arg0Count++; // Indexing not worthwile if only a few number of clauses

            if (c.Head.Arg (0).IsVar) break;
          }

          c = c.NextClause;
        }

        if (arg0Count < ARG0COUNT_MIN) return false;

        // second pass: build the index

        arg0Index = new Dictionary<object, ClauseNode> ();
        c = clauseList;

        while (c != null)
        {
          string s;

          BaseTerm t = c.Head.Arg (0);

          if (t.IsVar)
          {
            arg0Index [VARARG] = c; // stop immediately after having included the first variable

            break;
          }
          else if (!arg0Index.ContainsKey (s = t.FunctorToString))
            arg0Index [s] = c;

          c = c.NextClause;
        }

        if (arg0Index.Count == 1) // e.g. c(a(1)), c(a(2)), c(a(3)), ...
        {
          arg0Index = null;

          return false;
        }
        else
          return true;
      }


      public bool IsFirstArgIndexed { get { return (arg0Index != null); } }

      public bool IsFirstArgMarked (ClauseNode c)
      {
        if (arg0Index == null) return false;

        BaseTerm t = c.Term.Arg (0);
        ClauseNode result;

        if (t.IsVar)
          arg0Index.TryGetValue (VARARG, out result);
        else
          arg0Index.TryGetValue (t.FunctorToString, out result);

        return (result == c);
      }


      public ClauseNode FirstArgNonvarClause (string arg)
      {
        ClauseNode result;
        arg0Index.TryGetValue (arg, out result);

        return result;
      }


      public ClauseNode FirstArgVarClause ()
      {
        ClauseNode result;
        arg0Index.TryGetValue (VARARG, out result);

        return result;
      }


      public void DestroyFirstArgIndex ()
      {
        arg0Index = null;
      }

      //public void DumpClauseList()
      //{
      //  NextClause c = clauseList;

      //  while (c != null)
      //  {
      //    IO.WriteLine("DumpValues -- {0}", c);
      //    c = c.NextClause;
      //  }
      //}

      public override string ToString ()
      {
        return string.Format ("pd[{0}/{1} clauselist {2}]", functor, arity, clauseList);
      }


#if enableSpying
      public void SetSpy (bool enabled, string functor, int arity, SpyPort setPorts, bool warn)
      {
        string spySet = "[";

        if (enabled)
        {
          spied = true;
          spyMode = SpyPort.None;

          foreach (SpyPort port in Enum.GetValues (typeof (SpyPort)))
            if (setPorts > 0 && (setPorts | port) == setPorts)
            {
              spyMode |= port;

              if (port != SpyPort.Full)
                spySet += port.ToString () + (port == SpyPort.Redo ? "]" : ",");
            }

          if (setPorts != SpyPort.None)
            IO.Message ("Spying {0} enabled for {1}/{2}", spySet, functor, arity);
        }
        else if (spied) // nospy
        {
          spied = false;
          IO.Message ("Spying disabled for {0}/{1}", functor, arity);
        }
        else if (warn)
          IO.Message ("There was no spypoint on {0}/{1}", functor, arity);
      }


      public void ShowSpypoint ()
      {
        if (spyMode == SpyPort.None) return;

        string spySet = "[";

        foreach (SpyPort port in Enum.GetValues (typeof (SpyPort)))
          if (port > 0 && port != SpyPort.Full && (spyMode | port) == spyMode)
            spySet += port.ToString () + (port == SpyPort.Redo ? "]" : ",");

        IO.WriteLine ("{0}/{1}: {2}", Functor, Arity, spySet);
      }
#endif
    }
  }
}
