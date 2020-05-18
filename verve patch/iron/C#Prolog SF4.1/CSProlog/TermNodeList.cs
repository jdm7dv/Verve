//#define arg1index // if (un)defined, do the same in PredStorage.cs !!!
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

namespace Prolog
{
  /*
    --------
    TermNode
    --------

    A TermNode serves two purposes:
    - to store the goals of a command
    - to store predicates (clauses)

    A goal is constructed as a simple chained term of TermNodes. This makes it easy
    (and also more efficient in terms of GC) to revert to a previous state upon backtracking
    (in contrast to i.e. an ArrayList).

    A predicate consists of one or more clauses. A clause consist of a head and optionally a
    body. A head is a term, the body is a sequence of terms. A predicate is stored as a chain
    of TermNodes, where each TermNode represents a clause. These TermNodes are linked via the
    nextClause field. In each nextClause/TermNode the clause head is stored in term, and the
    clause body (which may be null) in NextNode.

  */


  public partial class PrologEngine
  {
    #region TermNode
    public class TermNode
    {
      BI builtinId = BI.none;
      protected BaseTerm term;                  // for a NextClause: predicate head
      protected TermNode nextNode = null;       // next node in the chain
      protected ClauseNode nextClause = null;   // next predicate clause (advanced upon backtracking)
      protected PredicateDescr predDescr;       // points to the predicate definition for term
      public BaseTerm Head { get { return term; } set { term = value; } }
      public ClauseNode NextClause { get { return nextClause; } set { nextClause = value; } }
      protected int level = 0;                  // debugging and tracing (for indentation)

      #region public properties
      public int Level { get { return level; } set { level = value; } }
      public BaseTerm Term { get { return term; } }
      public TermNode NextNode { get { return nextNode; } set { nextNode = value; } }
      public TermNode NextGoal { get { return nextNode; } set { nextNode = value; } }
      public bool Spied { get { return PredDescr == null ? false : PredDescr.Spied; } }
      public SpyPort SpyPort { get { return PredDescr == null ? SpyPort.None : PredDescr.SpyPort; } }
      public BI BuiltinId { get { return builtinId; } }
      public PredicateDescr PredDescr { get { return predDescr; } set { predDescr = value; } }
      #endregion

      public TermNode ()
      {
      }


      public TermNode (BaseTerm term, PredicateDescr predDescr)
      {
        this.predDescr = predDescr;
        this.term = term;
      }


      public TermNode (BaseTerm term, PredicateDescr predDescr, int level)
      {
        this.term = term;
        this.predDescr = predDescr;
        this.level = level;
      }


      public TermNode (string tag) // builtin predicates
      {
        try
        {
          builtinId = (BI)Enum.Parse (typeof (BI), tag, false);
        }
        catch
        {
          IO.Error ("Bootstrap.cs: unknown BI enum value '{0}'", tag);
        }
      }


      public TermNode (BaseTerm term, TermNode nextNode)
      {
        this.term = term;
        this.nextNode = nextNode;
      }


      // put the predicate definition (if found) into the TermNode if it is not already there
      public bool FindPredicateDefinition (PredicateTable predicateTable)
      {
        if (predDescr == null)
        {
          if ((predDescr = predicateTable [term.Key]) == null) 
            return false;
        }

#if arg1index // first-argument indexing enabled
        BaseTerm arg;

        // caching would disturb the search process (since caching does not
        // cause the arg0Index to be rebuild, since this might be to costly)
        if (predDescr.IsFirstArgIndexed && !predDescr.HasCachedValues)
        {
          if ((arg = term.Arg (0)).IsVar)
            nextClause = predDescr.FirstArgVarClause ();
          else // not a variable
          {
            nextClause = predDescr.FirstArgNonvarClause (arg.FunctorToString);

            // check whether there is an indexed var clause
            if (nextClause == null)
              nextClause = predDescr.FirstArgVarClause ();

            // if the above failed, the entire predicate fails (no unification possible)
            if (nextClause == null)
              nextClause = ClauseNode.FAIL;
          }

          if (nextClause == null)
            nextClause = predDescr.ClauseList;
        }
        else // not indexed
#endif
          nextClause = predDescr.ClauseList;

        return true;
      }


      public void Append (BaseTerm t)
      {
        if (term == null)  // empty term
        {
          term = t;

          return;
        }

        TermNode tail = this;
        TermNode next = nextNode;

        while (next != null)
        {
          tail = next;
          next = next.nextNode;
        }

        tail.nextNode = new TermNode (t, (PredicateDescr)null);
      }


      public TermNode Append (TermNode t)
      {
        TermNode tail = this;
        TermNode next = nextNode;

        while (next != null) // get the last TermNode
        {
          tail = next;
          next = next.nextNode;
        }

        tail.nextNode = t;

        return this;
      }


      public void Clear ()
      {
        term = null;
        nextNode = null;
        nextClause = null;
        level = 0;
      }


      public BaseTerm TermSeq ()
      {
        return (NextNode == null)
        ? Term  // last term of TermNode
        : new OperatorTerm (CommaOpDescr, Term, NextNode.TermSeq ());
      }


      public override string ToString ()
      {
        StringBuilder sb = new StringBuilder ();
        bool first = true;
        int indent = 2;
        TermNode tn = this;
        BaseTerm t;

        while (tn != null)
        {
          if ((t = tn.term) is TryOpenTerm)
          {
            if (!first) sb.Append (',');

            sb.AppendFormat ("{0}{1}TRY{0}{1}(", Environment.NewLine, Spaces (2*indent++));
            first = true;
          }
          else if (t is CatchOpenTerm)
          {
            CatchOpenTerm co = (CatchOpenTerm)t;
            string msgVar = (co.MsgVar is AnonymousVariable) ? null : co.MsgVar.Name;
            string comma = (co.ExceptionClass == null || msgVar == null) ? null : ", ";

            sb.AppendFormat ("{0}{1}){0}{1}CATCH {2}{3}{4}{0}{1}(",
              Environment.NewLine, Spaces (2*(indent-1)), co.ExceptionClass, comma, msgVar);
            first = true;
          }
          else if (t == TC_CLOSE)
          {
            sb.AppendFormat ("{0}{1})", Environment.NewLine, Spaces (2*--indent));
            first = false;
          }
          else
          {
            if (first) first = false; else sb.Append (',');

            sb.AppendFormat ("{0}{1}{2}", Environment.NewLine, Spaces (2*indent), t);
          }

          tn = tn.nextNode;
        }

        return sb.ToString ();
      }
    }
    #endregion TermNode

    #region clause
    // the terms (connected by NextNode) of a single clause
    public class ClauseNode : TermNode
    {
      public ClauseNode ()
      {
      }

      public ClauseNode (BaseTerm t, TermNode body)
        : base (t, body)
      {
      }


      public override string ToString ()
      {
        string NL = Environment.NewLine;

        StringBuilder sb = new StringBuilder (NL + term.ToString ());

        bool first = true;
        TermNode tl = nextNode;

        if (tl == null) return sb.ToString () + '.' + NL;

        while (true)
        {
          if (first) sb.Append (" :-");

          sb.Append (NL + "  " + tl.Term);

          if ((tl = tl.NextNode) == null)
            return sb.ToString () + '.' + NL;
          else if (!first)
            sb.AppendFormat (",");

          first = false;
        }
      }


      public static ClauseNode FAIL = new ClauseNode (BaseTerm.FAIL, null);
    }


    // CACHEING CURRENTLY NOT USED
    public class CachedClauseNode : ClauseNode
    {
      bool succeeds; // indicates whether the cached fact results in a failure or in a success
      // i.e. in doing so fib(1,9999) will effecively be cached as 'fib( 1, 9999) :- !, fail'
      // (the ! is necessary to prevent the resolution process from recalculating the same result
      // again).
      public bool Succeeds { get { return succeeds; } }

      public CachedClauseNode (BaseTerm t, TermNode body, bool succeeds) : base (t, body)
      {
        this.succeeds = succeeds;
      }
    }
    #endregion clause

    #region SpyPoint
    //  pushed on the VarStack to detect failure, inserted in the saveGoal to detect Exit.
    class SpyPoint : TermNode // TermNode only used as at-compatible vehicle for port and saveGoal
    {
      SpyPort port;
      public SpyPort Port { get { return port; } }
      TermNode saveGoal;
      public TermNode SaveGoal { get { return saveGoal; } }

      public SpyPoint (SpyPort p, TermNode g)
        : base ()
      {
        port = p;
        saveGoal = g;
        level = g.Level;
      }

      public void Kill ()
      {
        port = SpyPort.None;
      }

      public override string ToString ()
      {
        return "[" + port + "-spypoint] " + saveGoal.Term.ToString () + " ...";
      }
    }
    #endregion SpyPoint
  }
}
