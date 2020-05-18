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
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Prolog
{
  /*
     All Prolog data structures are called terms. A term is either:
     - A variable (unbound, named).
     - A constant, which can be either an Atom, Number, String, DateTime, TimeSpan or Bool
     - A compound term (a functor (an atom) and one or more arguments)
  */
  public enum TermType
  { // order is important for term comparison. See CompareTo below.
    None,
    Var,
    UnboundVar,
    NamedVar,
    Number,
    ImagNumber,
    Atom,
    String,
    DateTime,
    TimeSpan,
    Bool,
    FileReader,
    FileWriter,
    SqlCommand,
    Binary,
    Compound
  }

  public interface ITermNode
  {
    string Functor { get; }
    ITermNode [] Args { get; }
    T To<T> () where T : struct;
    string ToString ();
  }

  public partial class PrologEngine
  {
    public class NodeIterator : IEnumerable<BaseTerm>
    {
      BaseTerm root;
      BaseTerm pattern;
      BaseTerm minLenTerm;
      BaseTerm maxLenTerm;
      BaseTerm path;
      Stack<int> pos;
      Stack<BaseTerm> terms;
      VarStack varStack;
      IEnumerator<BaseTerm> iterator;
      bool skipVars; // iff true variables in the Term tree will never match the pattern

      public NodeIterator (BaseTerm root, BaseTerm pattern, BaseTerm minLenTerm, 
        BaseTerm maxLenTerm, bool skipVars, BaseTerm path, VarStack varStack)
      {
        this.root = root;
        this.pattern = pattern;
        this.varStack = varStack;
        this.minLenTerm = minLenTerm;
        this.maxLenTerm = maxLenTerm;
        this.skipVars = skipVars;
        this.path = path;
        iterator = GetEnumerator ();
      }

      /*
        GetEnumerator () traverses the Term tree and returns each node in a
        depth-first fashion. I have chosen to implemement it non-recursively,
        using a stack mechanism, as I also wanted the 'path' in the tree to
        be determined. Pseudo-code:

        TraverseAllNodes (root)
        {
          push (<root, 0>) // index of next root-child to be traversed
       L: pop (<term, childNo>)
          if (childNo == 0) Process (term) // contains test & yield
          if ({no such child}) goto L
          push (<term, childNo+1>)
          push (<term.Arg(childNo), 0>)
          if ({stack not empty}) goto L
        }

        // recursive version (not tested)
        IEnumerable<BaseTerm> TraverseAllNodes (BaseTerm root, BaseTerm pattern)
        {
          if (root.IsUnifiableWith (pattern))
            yield return root.LinkEnd;

          foreach (BaseTerm arg in root.args)
            foreach (BaseTerm node in TraverseTree (arg, pattern))
               yield return node;
        }
      */

      public IEnumerator<BaseTerm> GetEnumerator ()
      {
        pos = new Stack<int> (0);
        terms = new Stack<BaseTerm> ();
        BaseTerm term = root;
        int childNo = 0;
        int minLevel = minLenTerm.IsVar ? 0 : minLenTerm.To<int> ();
        int maxLevel = maxLenTerm.IsVar ? int.MaxValue : maxLenTerm.To<int> ();

        terms.Push (term);
        pos.Push (childNo);

        do
        {
          term = terms.Peek ();
          childNo = pos.Pop ();

          // do not match the pattern with single variables, except if the pattern is an atom
          if ((!(skipVars && term is Variable) || pattern.Arity == 0) &&
              childNo == 0 &&
              pos.Count >= minLevel &&
              pos.Count <= maxLevel &&
              pattern.Unify (term, varStack))
            {
              if (minLenTerm.IsVar)
                minLenTerm.Unify (new DecimalTerm (NodePath.ProperLength), varStack);

              if (maxLenTerm.IsVar)
                maxLenTerm.Unify (new DecimalTerm (NodePath.ProperLength), varStack);

              if (!path.IsCut && !path.Unify (NodePath, varStack))
                continue;

              yield return pattern;
            }

          if (childNo < term.Arity)
          {
            pos.Push (childNo + 1); // next child to be processed ...

            terms.Push (term.Arg (childNo)); // .. but process its children first
            pos.Push (0);
          }
          else
            terms.Pop ();
        }
        while (terms.Count > 0);
      }


      // compiler complains if this is missing -- cf. Jon Skeet C# in Depth p.91
      IEnumerator IEnumerable.GetEnumerator ()
      {
        return GetEnumerator ();
      }


      public bool MoveNext ()
      {
        return iterator.MoveNext ();
      }

      // NodePath returns the position in the tree (i.e. [1,2] means: 2nd arg of 1st arg of root)
      public ListTerm NodePath
      {
        get
        {
          ListTerm result = ListTerm.EMPTYLIST;

          foreach (int i in pos.ToArray ()) result = new ListTerm (new DecimalTerm (i - 1), result);

          return result;
        }
      }
    }


    public partial class BaseTerm : ITermNode, IComparable<BaseTerm>
    {
      #region Fields and properties
      protected int termId; // for variables: the varNo, for (some) other term types: unique int for functor+arity combination
      protected short precedence;
      protected AssocType assocType; // i.e. fx, fy, xfx, xfy, yfx, xf, yf.
      protected BaseTerm [] args; // compound term arguments
      protected TermType termType;
      protected object functor;

      protected int arity { get { return (args == null) ? 0 : args.Length; } }
      public int TermId { get { return termId; }  set { termId = value; } }

      // ChainEnd () is the end term of a unification chain, i.e. the term with which all the
      // other terms in the chain are unified. The ChainEnd () does not necessarily have a value! (i.e. it can be nonvar)
      public virtual BaseTerm ChainEnd () { return this; }
      public virtual BaseTerm ChainEnd (int unifyCount) { return this; }

      // could move all prop's referring to ChainEnd () to Variable, and leave the base types here
      public virtual object Functor { get { return ChainEnd ().functor; } set { ChainEnd ().functor = value; } }
      public virtual int Arity { get { return ChainEnd ().arity; } }
      public TermType TermType { get { return ChainEnd ().termType; } }
      public virtual string FunctorToString { get { return functor == null ? null : ChainEnd ().functor.ToString (); } }
      public bool HasFunctor (string s) { return (FunctorToString == s); }
      public AssocType AssocType { get { return ChainEnd ().assocType; } }
      public int Precedence { get { return ChainEnd ().precedence; } }
      public BaseTerm [] Args { get { return ChainEnd ().args; } } // Args [i] != Arg (i) !!!!
      public string Index { get { return ChainEnd ().FunctorToString + "/" + ChainEnd ().arity; } } // for readability only
      public override string ToString () { return ChainEnd ().ToWriteString (0); }
      public string ToDisplayString () { return ChainEnd ().ToDisplayString (0); }
      public virtual string ToDisplayString (int level) { return ChainEnd ().ToWriteString (level); }

      public bool FunctorIsDot { get { return (FunctorToString == "."); } }
      public string Key { get { return MakeKey (FunctorToString, arity); } }
      public virtual string Name { get { return FunctorToString + '/' + arity; } }
      public bool FunctorIsBinaryComma { get { return (FunctorToString == ","); } }
      public virtual bool IsCallable { get { return false; } } // i.e. if it can be a predicate head
      public virtual bool IsEvaluatable { get { return false; } } // i.e. if it can be evaluated by is/2
      public virtual bool IsUnified { get { return false; } }
      public virtual string ToWriteString (int level) { return FunctorToString; }

      static int verNoMax;
      protected static int varNoMax;
      protected static string NUMVAR;
      public static int unboundVarCount;
      public static ListTerm EMPTYLIST;
      public static DcgTerm NULLCURL;
      public static Variable VAR;
      public static BaseTerm TREEROOT;
      public static BaseTerm CUT;
      public static BaseTerm FAIL;
      public static BaseTerm DBNULL;
      public static bool trace;
      public static string MakeKey (string f, int a) { return a + f; }

      public bool IsVar { get { return (ChainEnd () is Variable); } }
      public bool IsAtomic { get { return (ChainEnd ().IsAtom || ChainEnd () is ValueTerm); } }
      public bool IsString { get { return (ChainEnd () is StringTerm); } }
      public bool IsBool { get { return (ChainEnd () is BoolTerm); } }
      public bool IsDateTime { get { return (ChainEnd () is DateTimeTerm); } }
      public bool IsTimeSpan { get { return (ChainEnd () is TimeSpanTerm); } }
      public bool IsCompound { get { return (ChainEnd () is CompoundTerm); } }
      public bool IsOperator { get { return (ChainEnd () is OperatorTerm); } }
      public bool IsNamedVar { get { return (ChainEnd () is NamedVariable); } }
      public bool IsUnboundTerm { get { return (ChainEnd () is Variable); } }
      public bool IsEmptyList { get { return (ChainEnd () is ListTerm && Arity == 0); } }
      public bool IsRange { get { return (arity == 2 && functor as string == ".."); } }
      public bool IsCut { get { return (ChainEnd () is Cut); } }
      public bool IsAtom { get { return (Arity == 0 && !(ChainEnd () is ValueTerm)); } } // Var has -1
      public bool IsAtomOrString { get { return (IsAtom || IsString); } }
      public bool IsInteger { get { return (IsNumber && Decimal.Remainder (To<decimal> (), 1) == 0); } }
      public bool IsNatural { get { return (IsNumber && To<decimal> () >= 0); } }
      public bool IsFloat { get { return (IsNumber && Decimal.Remainder (To<decimal> (), 1) != 0); } }
      public bool IsNumber
      {
        get
        {
          BaseTerm t = ChainEnd ();

          return (t is DecimalTerm || t is ComplexTerm ||
                  t is OperatorTerm && t.arity == 1 && t.Arg (0).IsNumber && t.HasUnaryOperator ("+", "-"));
        }
      }

      protected TermType Rank { get { return TermType; } }
      protected virtual int CompareValue (BaseTerm t) { return FunctorToString.CompareTo (t.FunctorToString); }
      public virtual bool IsProperList { get { return false; } }
      public virtual bool IsPartialList { get { return false; } }
      public virtual bool IsPseudoList { get { return false; } }
      public virtual bool IsProperOrPartialList { get { return false; } }
      public virtual bool IsListNode { get { return false; } }
      public virtual bool IsDcgList { get { return false; } }
      public virtual bool HasUnaryOperator () { return false; }
      public virtual bool HasBinaryOperator () { return false; }
      public virtual bool HasUnaryOperator (params string [] names) { return false; }
      public virtual bool HasBinaryOperator (params string [] names) { return false; }

      #region setting and retrieving an argument value
      public BaseTerm Arg (int pos) { return args [pos].ChainEnd (); }
      public void SetArg (int pos, BaseTerm t) { ChainEnd ().args [pos] = t; }
      #endregion


      public bool IsGround
      {
        get
        {
          if (IsVar) return false;

          for (int i = 0; i < arity; i++)
            if (Arg (i).IsVar) return false;

          return true;
        }
      }

      // interface properties
      string ITermNode.Functor { get { return FunctorToString; } }
      ITermNode [] ITermNode.Args { get { return Args; } }

      protected string CommaAtLevel (int level) { return (level == 0 ? ", " : ","); }
      protected string SpaceAtLevel (int level) { return (level == 0 ? " " : ""); }
      #endregion Fields and properties

      // BaseTerm comparison according to the ISO standard (apart from the extra data types)
      // 1. Variables < Numbers < Atoms < Strings < Compound Terms
      // 2. Variables are sorted by address.
      // 3. Atoms are compared alphabetically.
      // 4. Strings are compared alphabetically.
      // 5. Numbers are compared by value. Integers and floats are treated identically.
      // 6. Compound terms are first checked on their FunctorString-name (alphabetically), then on their arity
      //    and finally recursively on their arguments, leftmost argument first.
      public virtual int CompareTo (BaseTerm t) // for terms of identical subtype
      {
        BaseTerm t0 = this.ChainEnd ();
        BaseTerm t1 = t.ChainEnd ();
        int result = t0.Rank.CompareTo (t1.Rank); // BaseTerm types are ranked according to TermType enum order

        return (result == 0) ? t0.CompareValue (t1) : result;
      }

      public void CopyValuesFrom (BaseTerm t)
      {
        functor = t.functor;
        args = t.args;
        termType = t.termType;
        assocType = t.assocType;
        precedence = t.precedence;
      }


      static BaseTerm ()
      {
        EMPTYLIST = new ListTerm ();
        NULLCURL = new DcgTerm ();
        DBNULL = new AtomTerm ("db_null");
        VAR = new Variable ();
        verNoMax = 0;
        varNoMax = 0;
        NUMVAR = "'$VAR'";
        CUT = new Cut (0);
        FAIL = new AtomTerm ("fail");
        trace = false;
      }


      protected BaseTerm () // base() constructor
      {
        assocType = AssocType.None;
        precedence = 0;
      }


      public bool OneOfArgsIsVar (params int [] args)
      {
        foreach (int i in args)
          if (Arg (i).IsVar)
          {
            IO.Error (
              "Argument {0} of {1}/{2} is not sufficiently instantiated",
              i, FunctorToString, arity);

            return true;
          }

        return false;
      }


      public TermNode ToGoalList () // called during consult
      {
        return ToGoalList (0, 0);
      }


      public TermNode ToGoalList (int stackSize, int level) // called during execution (when there is a stack)
      {
        TermNode result = null;
        BaseTerm t0, t1;

        TermType tt = this.TermType;
        AssocType at = this.AssocType;
        int pr = this.Precedence;

        if (this is Cut)
        {
          if (stackSize == 0)
            return new TermNode (this, null, level);
          else
            return new TermNode (new Cut (stackSize), null, level);
        }

        switch (this.Functor as string)
        {
          case PrologParser.IMPLIES:
            t0 = Arg (0);
            if (!t0.IsCallable)
              IO.Error ("Illegal predicate head: {0}", t0);
            t1 = Arg (1);
            result = new TermNode (t0, t1.ToGoalList (stackSize, level));
            break;
          case PrologParser.DCGIMPL:
            t0 = Arg (0);
            if (!t0.IsCallable) IO.Error ("Illegal DCG head: {0}", t0);
            t1 = Arg (1);
            result = new TermNode (t0, t1.ToGoalList (stackSize, level));
            break;
          case PrologParser.COMMA:
            t0 = Arg (0);
            t1 = Arg (1);
            result = t0.ToGoalList (stackSize, level);
            result.Append (t1.ToGoalList (stackSize, level));
            break;
          case PrologParser.DOT:
            t0 = Arg (0);
            t1 = Arg (1);
            result = (new CompoundTerm ("consult", new ListTerm (t0, t1))).ToGoalList (stackSize, level);
            break;
          case PrologParser.CURL:
            t0 = Arg (0);
            result = t0.ToGoalList (stackSize, level);
            break;
          default:
            if (this.IsVar)
              result = new TermNode (new CompoundTerm ("meta$call", this), null, level);
            else if (this.IsCallable)
              result = new TermNode (this, null, level);
            else
              IO.Error ("Illegal term {0} in goal list", this);
            break;
        }

        return result;
      }


      // DCG stuff

      public TermNode ToDCG (ref BaseTerm lhs) // called from parser
      {
        TermNode body = new TermNode ();
        BaseTerm result = null;

        BaseTerm inVar = new Variable ();
        BaseTerm inVarSave = inVar;
        BaseTerm outVar = inVar;
        lhs = new DcgTerm (lhs, ref outVar); // outVar becomes new term
        BaseTerm remainder;

        List<BaseTerm> alternatives = AlternativesToArrayList ();

        for (int i = 0; i < alternatives.Count; i++)
        {
          BaseTerm alt = alternatives [i];
          bool embedded = (alternatives.Count > 1);
          List<BaseTerm> terms = alt.ToTermList ();

          body.Clear ();
          remainder = inVarSave;

          for (int ii = 0; ii < terms.Count; ii++)
            DCGGoal (terms [ii], ref body, ref remainder, ref embedded);

          // create a term-tree from the array
          if (i == 0)
            result = body.TermSeq ();
          else
            result = new OperatorTerm (SemiOpDescr, result, body.TermSeq ());

          ((Variable)remainder).Bind (outVar);
        }

        return (result == null) ? null : result.ToGoalList (); // empty body treated similar to null
      }


      public List<BaseTerm> AlternativesToArrayList ()
      {
        BaseTerm t = this;
        List<BaseTerm> a = new List<BaseTerm> ();

        while (t.HasFunctor (PrologParser.SEMI) && t.Arity == 2)
        {
          a.Add (t.Arg (0));
          t = t.Arg (1); // xfy
        }

        a.Add (t);

        return a;
      }


      public List<BaseTerm> ToTermList ()
      {
        BaseTerm t = this;
        List<BaseTerm> a = new List<BaseTerm> ();

        while (t.HasFunctor (PrologParser.COMMA) && t.Arity == 2)
        {
          a.AddRange (t.Arg (0).ToTermList ());
          t = t.Arg (1); // xfy
        }

        a.Add (t);

        return a;
      }


      static void DCGGoal (BaseTerm t, ref TermNode body, ref BaseTerm remainder, ref bool embedded)
      {
        BaseTerm temp;

        if (t.IsString || t is Cut)
          body.Append (t);
        else if (t.HasFunctor (PrologParser.CURL))
        {
          while (t.Arity == 2)
          {
            body.Append (t.Arg (0));
            t = t.Arg (1);
            embedded = true;
          }
        }
        else if (t.IsProperList)
        {
          temp = new Variable ();

          t = (t.IsEmptyList) ? temp : ((ListTerm)t).Append (temp);

          if (embedded)
          {
            body.Append (new CompoundTerm (PrologParser.EQ, remainder, t));
            embedded = false;
          }
          else
            ((Variable)remainder).Bind (t);
          // in this case, nothing is appended to body, which may be left empty (e.g. t-->[x])

          remainder = temp;
        }
        else if (t.IsAtom || t.IsCompound)
        {
          t = new DcgTerm (t, ref remainder);
          body.Append (t);
        }
        else if (t.IsNamedVar)
          IO.Error ("Variable not allowed in DCG-clause: {0}", ((NamedVariable)t).Name);
        else if (t.IsUnboundTerm)
          IO.Error ("Unbound variable not allowed in DCG-clause");
        else
          IO.Error ("Illegal term in DCG-clause: {0}", t);
      }


      // UNIFICATION
      // The stack is used to store the variables and choice points that are bound
      // by the unification. This is required for backtracking. Unify does not do
      // any unbinding in case of failure. This will be done during backtracking.
      // refUnifyCount: can be used for calculating the 'cost' of a predicate Call
      public virtual bool Unify (BaseTerm t, VarStack varStack)
      {
        NextUnifyCount ();

        if (t.IsUnified) return this.Unify (t.ChainEnd (), varStack);

        if (t is Variable) // t not unified
        {
          ((Variable)t).Bind (this);
          varStack.Push (t);

          return true;
        }

        if (t is ListPatternTerm)
          return t.Unify (this, varStack);

        if (termType != t.termType) return false; // gives a slight improvement

        if (functor.Equals (t.functor) && arity == t.arity)
        {
          for (int i = 0; i < arity; i++)
            if (!args [i].Unify (t.args [i], varStack)) return false;

          return true;
        }

        return false;
      }


      public bool IsUnifiableWith (BaseTerm t, VarStack varStack) // as Unify, but does not actually bind
      {
        int marker = varStack.Count;
        bool result = Unify (t, varStack);
        UnbindToMarker (varStack, marker);

        return result;
      }


      public static void UnbindToMarker (VarStack varStack, int marker)
      {
        for (int i = varStack.Count - marker; i > 0; i--) // unbind all vars that got bound by Unify
        {
          Variable v = varStack.Pop () as Variable;

          if (v != null) v.Unbind ();
        }
      }


      public static string VarList (VarStack varStack) // debugging only
      {
        StringBuilder result = new StringBuilder ();

        foreach (object v in varStack.ToArray ())
          if (v != null && v is Variable)
            result.AppendLine (string.Format (">> {0} = {1}", ((Variable)v).Name, (Variable)v));

        return result.ToString ();
      }

      /* TERM COPYING

         A term is copied recursively, by first copying its arguments and then creating
         a new term consisting of the term functor and its copied arguments. The term’s
         type is taken into account. A variable is copied by creating a new instance.

         In this latter process, there is a complication. If a term contains more
         instances of the same variable, then one has to make sure that all copies refer
         to one and the same new instance. Therefore, in copying a var, it is necessary
         to check whether that same var has been copied before and to let the copy point
         to that instance rather than creating a new one.

         A solution to this problem is to set a boolean indicator in a term’s variable
         to denote whether it has already been copied. This, however, is not sufficient,
         as one also needs to know what the copied instance is. Therefore, the solution
         that has been adopted, is to equip the variable with an extra field newVar,
         that contains the newly created copy.

         In order to make this also work for copying a nextClause (which is implemented as
         a list of terms which are handled consecutively) one must make sure that this
         principle of one copy for all identical variables holds for all terms making up
         the nextClause. Therefor, another int attribute verNo (version number) has been
         introduced. When (bool) newVersion is switched off, the already created
         newVar is used. Copy(false) is only used when the nextClause head is copied.
         The variables in the nextClause body will then implicitly point to the same
         newVars that where created by copying the nextClause head. These vars will then
         subsequently be unified with the current goal (in ExecuteGoalList () at the
         call to cleanClauseHead.Unify ().
      */

      // Create an identical new term, with verNo+varNo value that does not occur
      // in any other term created sofar. The uLinks are resolved.
      // A new term is constructed by creating a new instance for each unbound term.
      // This new instance gets a version number that is one higher than its original.

      public BaseTerm Copy ()
      {
        return Copy (true);
      }

      public BaseTerm Copy (bool newVersion)
      {
        if (newVersion) verNoMax++;

        return CopyEx (verNoMax, true);
      }

      public BaseTerm Copy (bool newVersion, bool mustBeNamed) // called by copy_term
      {
        if (newVersion) verNoMax++;

        return CopyEx (verNoMax, mustBeNamed);
      }

      BaseTerm CopyEx (int newVerNo, bool mustBeNamed)
      {
        if (IsUnified) return ChainEnd ().CopyEx (newVerNo, mustBeNamed);

        // A neater solution would be to use overrides for each term subtype.
        if (this is Variable)
        {
          Variable v = (Variable)this;

          if (newVerNo == v.verNo) return v.newVar;

          v.verNo = newVerNo;

          return v.newVar = (mustBeNamed && this is NamedVariable)
            ? new NamedVariable (((NamedVariable)v).Name)
            : new Variable ();
        }
        else if (this is CatchOpenTerm)
        {
          CatchOpenTerm c = (CatchOpenTerm)this;

          return new CatchOpenTerm (c.Id, c.ExceptionClass, c.MsgVar.CopyEx (newVerNo, mustBeNamed),
            c.SeqNo, c.SaveStackSize);
        }
        else
        {
          if (arity == 0) return this;

          BaseTerm t = null;
          BaseTerm [] a = new BaseTerm [arity];

          for (int i = 0; i < arity; i++)
            if (args [i] != null) // may be null for a GapTerm
              a [i] = args [i].CopyEx (newVerNo, mustBeNamed); // recursively refresh arguments

          if (this is ListPatternTerm)
            t = new ListPatternTerm (a);
          else if (this is AltListTerm)
          {
            AltListTerm alt = (AltListTerm)this;
            t = new AltListTerm (alt.LeftBracket, alt.RightBracket, a [0], a [1]);
          }
          else if (this is ListTerm)
          {
            if (((ListTerm)this).CharCodeString == null)
              t = new ListTerm (a [0], a [1]);
            else // it's an ISO-style string
              t = new ListTerm (((ListTerm)this).CharCodeString);
          }
          else if (this is OperatorTerm)
            t = new OperatorTerm (((OperatorTerm)this).od, a);
          else if (this is DcgTerm)
            t = new DcgTerm (functor, a);
          else if (this is WrapperTerm)
            t = new WrapperTerm ((WrapperTerm)this, a);
          else if (this is IntRangeTerm)
            t = new IntRangeTerm ((IntRangeTerm)this);
          else if (this is DbConnectionTerm)
            t = new DbConnectionTerm ((DbConnectionTerm)this);
          else if (this is ListPatternElem)
            t = new ListPatternElem (a, ((ListPatternElem)this).downRepFactor, ((ListPatternElem)this).IsNegSearch);
          else if (this is CompoundTerm)
            t = new CompoundTerm (functor, a);
          else
            IO.Error ("CopyEx(): type '{0}' not handled explicitly", this.GetType ());

          return t;
        }
      }


      public void NumberVars (ref int k, VarStack s)
      {
        if (IsVar)
          this.Unify (new CompoundTerm (NUMVAR, new DecimalTerm (k++)), s);
        else
        {
          if (IsUnified)
            ChainEnd ().NumberVars (ref k, s);
          else if (arity != 0) // nonvar & not isUnified
            for (int i = 0; i < arity; i++) args [i].NumberVars (ref k, s);
        }
      }


      public static BaseTerm MakeMatchTerm (Match m, bool asAtom)
      {
        BaseTerm [] args = new BaseTerm [4];

        if (asAtom)
          args [0] = new AtomTerm (m.Value.ToAtom ());
        else
          args [0] = new StringTerm (m.Value.ToString ());

        args [1] = new DecimalTerm (m.Index);
        args [2] = new DecimalTerm (m.Length);
        args [3] = new AtomTerm ("m.Groups");

        return new CompoundTerm ("match", args);
      }


      public virtual void TreePrint (int level, PrologEngine e)
      {
        e.Write (Spaces (2*level));

        if (this is Variable)
          e.WriteLine (this.ToString ());
        else
        {
          e.WriteLine (FunctorToString);

          if (arity > 0)
            foreach (BaseTerm a in args)
              a.ChainEnd ().TreePrint (level + 1, e);
        }
      }

    }
  }
}
