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
using System.IO;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Linq;
using System.Globalization;

namespace Prolog
{
  public partial class PrologEngine
  {
    #region Try/Catch
    public class TryCatchTerm : AtomTerm
    {
      public TryCatchTerm (string a) : base (a) { }
    }

    static TryCatchTerm TC_CLOSE = new TryCatchTerm (")");

    public class TryOpenTerm : TryCatchTerm
    {
      int id; // unique identification (on the Goal List) for each TRY/CATCH statement
      public int Id { get { return id; } set { id = value; } }

      public TryOpenTerm () : base ("TRY") { }
    }

    public class CatchOpenTerm : TryCatchTerm
    {
      string exceptionClass;
      int id;
      int seqNo;
      int saveStackSize;
      Variable msgVar;
      public string ExceptionClass { get { return exceptionClass; } }
      public int Id { get { return id; } set { id = value; } }
      public int SeqNo { get { return seqNo; } } // CATCH-clauses for one TRY are number from 0 onwards
      public int SaveStackSize { get { return saveStackSize; } set { saveStackSize = value; } } // not used
      public Variable MsgVar { get { return msgVar; } }

      public CatchOpenTerm (string exceptionClass, BaseTerm msgVar, int seqNo)
        : base ("CATCH")
      {
        this.exceptionClass = exceptionClass;
        this.seqNo = seqNo;
        this.msgVar = (Variable)msgVar;
      }

      public CatchOpenTerm (int id, string exceptionClass, BaseTerm msgVar, int seqNo, int saveStackSize)
        : base ("CATCH")
      {
        this.id = id;
        this.exceptionClass = exceptionClass;
        this.seqNo = seqNo;
        this.saveStackSize = saveStackSize;
        this.msgVar = (Variable)msgVar;
      }
    }
    #endregion Try/Catch

    #region NewIsoOrCsStringTerm
    public BaseTerm NewIsoOrCsStringTerm (string s)
    {
      if (csharpStrings)
        return new StringTerm (s);
      else
        return new ListTerm (s);
    }
    #endregion NewIsoOrCsStringTerm

    #region Variable
    public class Variable : BaseTerm
    {
      protected BaseTerm uLink;
      public int verNo;
      protected int unifyCount; // used for tabling
      protected int visitNo; // for cycle detection only
      public BaseTerm newVar;
      public BaseTerm ULink { get { return uLink; } }
      protected int varNo { get { return termId; } set { termId = value; } }
      public override string Name { get { return "_" + varNo; } }
      public override int Arity { get { return -1; } }
      public int UnifyCount { get { return unifyCount; } }
      public int VarNo { get { return varNo; } }
      public int VisitNo { get { return visitNo; } } // for cycle detection only -- not yet operational

      public bool IsUnifiedWith (Variable v)
      {
        return ChainEnd () == v.ChainEnd ();
      }

      public Variable ()
      {
        varNo = varNoMax++;
        verNo = 0;
        unifyCount = 0;
        termType = TermType.UnboundVar;
      }

      public Variable (int i)
      {
        varNo = i;
        verNo = 0;
        unifyCount = 0;
        termType = TermType.UnboundVar;
      }

      public override BaseTerm ChainEnd () { return (IsUnified) ? uLink.ChainEnd () : this; }
      // - this is a non-unified var, or is is a var with a refUnifyCount > arg  -> return this
      // - return ChainEnd (refUnifyCount)
      public override BaseTerm ChainEnd (int refUnifyCount)
      {
        return (uLink == null || unifyCount > refUnifyCount)
               ? this
               : uLink.ChainEnd (refUnifyCount); // resolves to uLink for a nonvar
      }

      public override bool IsUnified { get { return (uLink != null); } }
      protected override int CompareValue (BaseTerm t) { return varNo.CompareTo (((Variable)t).varNo); }

      public override bool Unify (BaseTerm t, VarStack varStack)
      {
        if (IsUnified) return ChainEnd ().Unify (t, varStack);

        if (t.IsUnified) return this.Unify (t.ChainEnd (), varStack);

        NextUnifyCount ();
        ((Variable)this).Bind (t);
        varStack.Push (this);

        return true;
      }


      public void Bind (BaseTerm t)
      {
        if (this == t) return; // cannot bind to self

        uLink = t;
        unifyCount = CurrUnifyCount;
      }


      public void Unbind ()
      {
        uLink = null;
        unifyCount = 0;
      }


      public override string ToWriteString (int level)
      {
        if (uLink == null) return Name;

        return uLink.ToWriteString (level);
      }
    }

    // carries a variable's symbolic name as found in the source
    public class NamedVariable : Variable
    {
      protected string name;
      public override string Name { get { return name; } }

      public NamedVariable (string name)
      {
        this.name = name;
        termType = TermType.NamedVar;
      }

      public NamedVariable () 
      { 
        termType = TermType.NamedVar; 
      }

      protected override int CompareValue (BaseTerm t) { return name.CompareTo (((NamedVariable)t).name); }
    }

    // not really necessary, but it can be convenient to recognize one
    public class AnonymousVariable : Variable
    {
    }
    #endregion Variable

    #region AtomTerm
    public class AtomTerm : BaseTerm
    {
      public override bool IsCallable { get { return true; } }
      public override bool IsEvaluatable { get { return true; } } // engine, pi, i, today, ...

      public AtomTerm (object functor)
      {
        this.functor = functor;
        termType = TermType.Atom;
      }


      public AtomTerm (string value)
      {
        functor = value.Unescaped ();
        termType = TermType.Atom;
      }


      public AtomTerm (char value)
      {
        functor = value.ToString ().Unescaped ();
        termType = TermType.Atom;
      }


      protected override int CompareValue (BaseTerm t)
      {
        return FunctorToString.CompareTo (t.FunctorToString);
      }


      public override string ToWriteString (int level)
      {
        return (FunctorToString == PrologParser.DOT) ? "'.'" : FunctorToString;
      }
    }
    #endregion AtomTerm

    #region ClauseTerm
    public class ClauseTerm : BaseTerm
    {
      public ClauseTerm (ClauseNode c)  // Create a BaseTerm from a NextClause (= Head + Body)
      {
        if (c.NextNode == null) // fact
          CopyValuesFrom (c.Head);
        else
        {
          functor = PrologParser.IMPLIES;
          args = new BaseTerm [2];
          args [0] = c.Head;
          termType = TermType.Atom;
          assocType = AssocType.xfx;
          args [1] = c.NextNode.TermSeq ();
          precedence = 1200;
        }
      }
    }
    #endregion ClauseTerm

    #region SqlTerm
    public class SqlTerm : BaseTerm
    {
      DbCommand dbCommand;
      DbDataReader dr;
      bool showColNames;
      AtomTerm [] colNames; // only needed if showColNames

      public SqlTerm (BaseTerm ci, string commandText, bool showColNames)
      {
        dbCommand = ((DbConnectionTerm)ci).DbCommand;
        functor = dbCommand.CommandText = commandText;
        this.showColNames = showColNames;
        termType = TermType.SqlCommand;
      }

      protected override int CompareValue (BaseTerm t)
      {
        return FunctorToString.CompareTo (t.FunctorToString);
      }

      public void ExecuteQuery ()
      {
        try
        {
          dbCommand.Connection.Close ();   // These lines appear to be necessary when you
          dbCommand.Connection.Open ();    // want to assign a new DataReader to the dbCommand.
          dr = dbCommand.ExecuteReader (); 
        }
        catch (Exception x)
        {
          IO.Fatal ("Error while attempting to execute SQL command.\r\n" +
                    "Command: {0}\r\nConnectstring: {1}\r\nMessage: {2}",
                    dbCommand.CommandText, dbCommand.Connection.ConnectionString, x.Message);
        }

        if (showColNames) // get the column names
        {
          DataTable schema = dr.GetSchemaTable ();
          colNames = new AtomTerm [schema.Rows.Count];
          int i = 0;

          // Each row describes a field. field[0] contains the column name; other info
          // is available in rest of fields (ColumnName, ColumnOrdinal, ColumnSize,
          // NumericPrecision, NumericScale, DataType, ProviderType, IsLong, AllowDBNull,
          // IsReadOnly, IsRowVersion, IsUnique, IsKey, IsAutoIncrement, BaseSchemaName,
          // BaseCatalogName, BaseTableName, BaseColumnName)

          foreach (DataRow field in schema.Rows)
            colNames [i++] = new AtomTerm ((field ["ColumnName"] as string).ToAtom ());
        }
      }

      // cf. msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlcommand.executenonquery.aspx
      public int ExecuteNonQuery ()
      {
        int rowsAffected = 0;

        try
        {
          rowsAffected = dbCommand.ExecuteNonQuery ();
        }
        catch (Exception x)
        {
          IO.Fatal ("Error while attempting to execute SQL command.\r\n" +
                    "Command: {0}\r\nConnectstring: {1}\r\nMessage: {2}",
                    dbCommand.CommandText, dbCommand.Connection.ConnectionString, x.Message);
        }

        return rowsAffected;
      }


      public ListTerm SqlNextRecordToListTerm ()
      {
        if (dr == null) IO.Fatal ("SqlNextRecordToListTerm: DbDataReader is null");

        List<Type> numericTypes = new List<Type>
        {
          typeof (Byte), typeof (Decimal), typeof (Double),  typeof (Int16), typeof (Int32),
          typeof (Int64), typeof (SByte), typeof (Single), typeof (UInt16), typeof (UInt32),
          typeof (UInt64)
        };

        BaseTerm result = ListTerm.EMPTYLIST;

        if (!dr.IsClosed && (dr.Read () || (dr.NextResult () && dr.Read ()))) // handle multiple result sets
        {
          for (int i = dr.FieldCount - 1; i >= 0; i--)
          {
            Type type = dr [i].GetType ();
            BaseTerm t = null;

            if (type == typeof (string))
            {
              string s = dr [i] as string;

              if (string.IsNullOrEmpty (s) || s.Length <= 64) // arbitrary choice
                t = new AtomTerm (s.ToAtom ());
              else
                t = new StringTerm (s);
            }
            else if (numericTypes.Contains (type))
              t = new DecimalTerm ((Decimal)Convert.ChangeType (dr [i], typeof (Decimal)));
            else if (type == typeof (DateTime))
              t = new DateTimeTerm ((DateTime)dr [i]);
            else if (type == typeof (TimeSpan))
              t = new TimeSpanTerm ((TimeSpan)dr [i]);
            else if (type == typeof (bool))
              t = new BoolTerm ((bool)dr [i]);
            else if (type == typeof (Byte [])) // MS-Access
              t = new BinaryTerm ((byte [])dr [i]);
            else if (type == typeof (DBNull))
              t = BaseTerm.DBNULL;
            else
              t = new StringTerm (string.Format ("(unmapped type '{0}')", type.Name));

            if (showColNames)
              t = new OperatorTerm (EqualOpDescr, colNames [i], t);

            result = new ListTerm (t, result);
          }

          return (ListTerm)result;
        }
        else
        {
          if (!dr.IsClosed) dr.Close ();

          return null;
        }
      }
    }
    #endregion SqlTerm

    #region WrapperTerm
    public class WrapperTerm : CompoundTerm
    {
      string wrapOpen;
      string wrapClose;
      string wrapFunctor { get { return (wrapOpen + ".." + wrapClose).ToAtom (); } }

      public WrapperTerm (string wrapOpen, string wrapClose, BaseTerm [] a)
        : base ((wrapOpen + ".." + wrapClose).ToAtom (), a)
      {
        this.wrapOpen = wrapOpen;
        this.wrapClose = wrapClose;
        termType = TermType.Compound;
      }

      public WrapperTerm (WrapperTerm that, BaseTerm [] a) // for Copy only
        : base ((that.wrapOpen + ".." + that.wrapClose).ToAtom (), a)
      {
        wrapOpen = that.wrapOpen;
        wrapClose = that.wrapClose;
        termType = TermType.Compound;
      }

      public override string ToWriteString (int level)
      {
        if (MaxWriteDepthExceeded (level)) return "...";

        StringBuilder sb = new StringBuilder (wrapOpen + SpaceAtLevel (level));
        bool first = true;

        for (int i = 0; i < arity; i++)
        {
          if (first) first = false; else sb.Append (CommaAtLevel (level));

          sb.AppendPacked (Arg (i).ToWriteString (level + 1), Arg (i).FunctorIsBinaryComma);
        }

        sb.Append (SpaceAtLevel (level) + wrapClose);

        return sb.ToString ();
      }


      public override string ToDisplayString (int level)
      {
        if (MaxWriteDepthExceeded (level)) return "...";

        StringBuilder sb = new StringBuilder (wrapFunctor);

        bool first = true;

        sb.Append ("(");

        foreach (BaseTerm a in Args)
        {
          if (first) first = false; else sb.Append (CommaAtLevel (level));

          sb.Append (a.ToDisplayString (level + 1));
        }

        sb.Append (")");

        return sb.ToString ();
      }

      public override void TreePrint (int level, PrologEngine e)
      {
        string margin = Spaces (2 * level);

        if (arity == 0)
        {
          e.WriteLine ("{0}{1}", margin, wrapFunctor);

          return;
        }

        e.WriteLine ("{0}{1}", margin, wrapOpen);

        foreach (BaseTerm a in args)
          a.TreePrint (level + 1, e);

        e.WriteLine ("{0}{1}", margin, wrapClose);
      }
    }
    #endregion WrapperTerm

    #region CompoundTerm
    public class CompoundTerm : BaseTerm
    {
      public override bool IsCallable { get { return true; } }
      public override bool IsEvaluatable { get { return true; } }

      public CompoundTerm (string functor, BaseTerm [] args)
      {
        this.functor = functor;
        this.args = args;
        termType = TermType.Compound;
      }


      public CompoundTerm (string functor, BaseTerm a)
      {
        this.functor = functor;
        args = new BaseTerm [1];
        args [0] = a;
        termType = TermType.Compound;
      }


      public CompoundTerm (string functor, BaseTerm a0, BaseTerm a1)
      {
        this.functor = functor;
        args = new BaseTerm [2];
        args [0] = a0;
        args [1] = a1;
        termType = TermType.Compound;
      }


      public CompoundTerm (object functor, BaseTerm [] args)
      {
        this.functor = functor;
        this.args = args;
        termType = TermType.Compound;
      }


      public CompoundTerm (string functor) // degenerated case (for EMPTYLIST and operator)
      {
        this.functor = functor;
        termType = TermType.Atom;
      }


      public CompoundTerm ()
      {
      }


      public CompoundTerm (CompoundTerm that)
      {
        functor = that.functor;
        args = that.args;
      }


      protected override int CompareValue (BaseTerm t)
      {
        int result = Arity.CompareTo (t.Arity); // same FunctorString: lowest arity first

        if (result != 0) return result; // different arities

        if (Arity == 0) return FunctorToString.CompareTo (t.FunctorToString);

        for (int i = 0; i < Arity; i++)
          if ((result = Arg (i).CompareTo (t.Arg (i))) != 0) return result;

        return 0;
      }


      public override string ToWriteString (int level)
      {
        if (MaxWriteDepthExceeded (level)) return "...";

        StringBuilder sb = new StringBuilder ();

        if (FunctorToString == PrologParser.COMMA && arity == 2)
          sb = new StringBuilder ("(" + Arg (0).ToWriteString (level) + CommaAtLevel (level)
          + Arg (1).ToWriteString (level) + ")");
        else if (this == NULLCURL)
          return PrologParser.CURL;
        else if (FunctorToString == PrologParser.CURL)
        {
          sb.Append ("{");
          bool first = true;

          foreach (BaseTerm arg in Args)
          {
            if (first) first = false; else sb.Append (CommaAtLevel (level));

            sb.Append (arg.ToWriteString (level + 1).Packed (arg.FunctorIsBinaryComma));
          }

          sb.Append ("}");
        }
        else
        {
          sb.AppendPossiblySpaced (FunctorIsBinaryComma ? "','" : FunctorToString);
          sb.Append ("(");
          bool first = true;

          for (int i = 0; i < arity; i++)
          {
            if (first) first = false; else sb.Append (CommaAtLevel (level));

            sb.AppendPacked (Arg (i).ToWriteString (level + 1), Arg (i).FunctorIsBinaryComma);
          }

          sb.Append (")");
        }

        return sb.ToString ();
      }


      public override string ToDisplayString (int level)
      {
        if (MaxWriteDepthExceeded (level)) return "...";

        string functor = (FunctorToString == PrologParser.CURL) ? "'{{}}'" : FunctorToString;

        StringBuilder sb = new StringBuilder (FunctorIsBinaryComma ? "','" : functor);
        bool first = true;

        sb.Append ("(");

        foreach (BaseTerm a in Args)
        {
          if (first) first = false; else sb.Append (CommaAtLevel (level));

          sb.Append (a.ToDisplayString (level + 1));
        }

        sb.Append (")");

        return sb.ToString ();
      }
    }
    #endregion CompoundTerm

    #region OperatorTerm
    public class OperatorTerm : CompoundTerm
    {
      // ConfigSettings.VerbatimStringsAllowed
      public OperatorDescr od;
      AssocType assoc;

      public OperatorTerm (OperatorTable opTable, string name, BaseTerm a0, BaseTerm a1)
        : base (name, a0, a1)
      {
        if (!opTable.IsBinaryOperator (name, out od))
          IO.Fatal ("OperatorTerm/4: not a binary operator: '{0}'", name);

        assoc = od.Assoc;
        precedence = (short)od.Prec;
      }


      public OperatorTerm (OperatorDescr od, BaseTerm a0, BaseTerm a1)
        : base (od.Name, a0, a1)
      {
        this.od = od;
        assoc = od.Assoc;
        precedence = (short)od.Prec;
      }


      public OperatorTerm (OperatorDescr od, BaseTerm a)
        : base (od.Name, a)
      {
        this.od = od;
        assoc = od.Assoc;
        precedence = (short)od.Prec;
      }


      public OperatorTerm (OperatorDescr od, BaseTerm [] a)
        : base (od.Name, a)
      {
        this.od = od;
        assoc = od.Assoc;
        precedence = (short)od.Prec;
      }


      public OperatorTerm (string name) // stand-alone operator used as term
        : base (name)
      {
        this.od = null;
        assoc = AssocType.None;
        precedence = 1001;
      }


      public override bool HasUnaryOperator ()
      {
        return (od.IsPostfix || od.IsPrefix);
      }


      public override bool HasBinaryOperator ()
      {
        return (od.IsInfix);
      }


      public override bool HasUnaryOperator (params string [] names)
      {
        if (!(od.IsPrefix || od.IsPostfix)) return false;

        foreach (string name in names)
          if (od.Name == name) return true;

        return false;
      }


      public override bool HasBinaryOperator (params string [] names)
      {
        if (!od.IsInfix) return false;

        foreach (string name in names)
          if (od.Name == name) return true;

        return false;
      }


      public override string ToWriteString (int level)
      {
        if (MaxWriteDepthExceeded (level)) return "...";

        StringBuilder sb = new StringBuilder ();
        bool mustPack;

        if (arity == 2)
        {
          mustPack = (precedence < Arg (0).Precedence ||
          (precedence == Arg (0).Precedence && (assoc == AssocType.xfx || assoc == AssocType.xfy)));
          sb.AppendPacked (Arg (0).ToWriteString (level + 1), mustPack);

          sb.AppendPossiblySpaced (FunctorToString);

          mustPack =
            (precedence < Arg (1).Precedence ||
            (precedence == Arg (1).Precedence && (assoc == AssocType.xfx || assoc == AssocType.yfx)));
          sb.AppendPacked (Arg (1).ToWriteString (level + 1), mustPack);

          return sb.ToString ();
        }
        else if (arity == 1)
        {
          switch (assoc)
          {
            case AssocType.fx:
              sb.Append (FunctorToString);
              sb.AppendPacked (Arg (0).ToWriteString (level + 1), (precedence <= Arg (0).Precedence));
              break;
            case AssocType.fy:
              sb.Append (FunctorToString);
              sb.AppendPacked (Arg (0).ToWriteString (level + 1), (precedence < Arg (0).Precedence));
              break;
            case AssocType.xf:
              sb.AppendPacked (Arg (0).ToWriteString (level + 1), (precedence <= Arg (0).Precedence));
              sb.AppendPossiblySpaced (FunctorToString);
              break;
            case AssocType.yf:
              sb.AppendPacked (Arg (0).ToWriteString (level + 1), (precedence < Arg (0).Precedence));
              sb.AppendPossiblySpaced (FunctorToString);
              break;
          }

          return sb.ToString ();
        }
        else // arity == 0
          return FunctorToString;
      }


      public override string ToDisplayString (int level)
      {
        if (MaxWriteDepthExceeded (level)) return "...";

        StringBuilder sb = new StringBuilder (FunctorIsBinaryComma ? "','" : FunctorToString);

        if (Arity > 0)
        {
          bool first = true;

          sb.Append ("(");

          foreach (BaseTerm a in Args)
          {
            if (first) first = false; else sb.Append (CommaAtLevel (level));

            sb.Append (a.ToDisplayString (level + 1));
          }

          sb.Append (")");
        }

        return sb.ToString ();
      }
    }
    #endregion OperatorTerm

    #region ValueTerm
    public class ValueTerm : BaseTerm // a BaseTerm that can be expression-evaluated by is/2.
    {
      public override bool IsEvaluatable { get { return true; } }
    }

    #region StringTerm
    public class StringTerm : ValueTerm
    {
      string value;
      public string Value { get { return this.value; } set { this.value = value; } }

      public StringTerm (string value)
      {
        functor = value;
        this.value = value;
        termType = TermType.String;
      }

      public StringTerm (char value)
      {
        functor = value.ToString ();
        termType = TermType.String;
      }

      public StringTerm ()
      {
        functor = string.Empty;
        termType = TermType.String;
      }


      public override string ToWriteString (int level)
      {
        return '"' + FunctorToString.Replace (@"\", @"\\").Replace (@"""", @"\""") + '"';
      }
    }
    #endregion

    #region NumericalTerm
    public class NumericalTerm : ValueTerm
    {
      public NumericalTerm ()
      {
        termType = TermType.Number;
      }
    }
    #endregion NumericalTerm

    #region DecimalTerm
    public class DecimalTerm : ValueTerm
    {
      decimal value;
      public decimal Value { get { return value; } }
      public double ValueD { get { return (double)value; } }
      public override string FunctorToString { get { return value.ToString (CIC); } }
      public static DecimalTerm ZERO;
      public static DecimalTerm ONE;
      public static DecimalTerm MINUS_ONE;
      const double EPS = 1.0e-6; // arbitrary, cosmetic

      public DecimalTerm () // required for ComplexTerm
      {
        termType = TermType.Number;
      }

      public DecimalTerm (decimal value)
      {
        this.value = value;
        functor = value;
        termType = TermType.Number;
      }

      public DecimalTerm (int value)
      {
        functor = this.value = (decimal)value;
        termType = TermType.Number;
      }

      public DecimalTerm (double value)
      {
        functor = this.value = (decimal)value;
        termType = TermType.Number;
      }

      public DecimalTerm (long value)
      {
        functor = this.value = (decimal)value;
        termType = TermType.Number;
      }

      static DecimalTerm ()
      {
        ZERO = new DecimalTerm (0);
        ONE = new DecimalTerm (1);
        MINUS_ONE = new DecimalTerm (-1);
      }


      public override bool Unify (BaseTerm t, VarStack varStack)
      {
        if (t is Variable) return t.Unify (this, varStack);

        NextUnifyCount ();

        if (t is DecimalTerm)
          return (value == ((DecimalTerm)t).value);

        if (t is ComplexTerm)
          return (Math.Abs (((ComplexTerm)t).Im) < EPS &&
                  Math.Abs (ValueD - ((ComplexTerm)t).Re) < EPS);

        return false;
      }


      protected override int CompareValue (BaseTerm t)
      { return (To<decimal> ().CompareTo (t.To<decimal> ())); }


      // sum
      public DecimalTerm Add (DecimalTerm d)
      {
        return new DecimalTerm (value + value);
      }

      public ComplexTerm Add (ComplexTerm c)
      {
        return new ComplexTerm (ValueD + c.Re, c.Im);
      }

      // difference
      public DecimalTerm Subtract (DecimalTerm d)
      {
        return new DecimalTerm (ValueD - d.ValueD);
      }

      public ComplexTerm Subtract (ComplexTerm c)
      {
        return new ComplexTerm (ValueD - c.Re, -c.Im);
      }

      // product
      public DecimalTerm Multiply (DecimalTerm d)
      {
        return new DecimalTerm (value * d.value);
      }

      public ComplexTerm Multiply (ComplexTerm c)
      {
        return new ComplexTerm (ValueD * c.Re, ValueD * c.Im);
      }

      // quotient
      public DecimalTerm Divide (DecimalTerm d)
      {
        if (d.value == 0)
          IO.Error ("Division by zero not allowed");

        return new DecimalTerm (value / d.value);
      }

      public virtual ComplexTerm Divide (ComplexTerm c)
      {
        if (c.Re == 0 && c.Im == 0)
          IO.Error ("Division by zero complex number not allowed");

        double denominator = c.Re * c.Re + c.Im * c.Im;
        double newRe = (ValueD * c.Re) / denominator;
        double newIm = (-ValueD * c.Im) / denominator;

        return new ComplexTerm (newRe, newIm);
      }


      public virtual DecimalTerm Exp ()
      {
        return new DecimalTerm (Math.Exp (ValueD));
      }


      public override string ToWriteString (int level)
      {
        if (value == Math.Truncate (value)) return value.ToString ();

        return (value.ToString (Math.Abs (value) < (decimal)EPS ? "e" : "0.######", CIC));
      }
    }
    #endregion

    #region DateTimeTerm
    public class DateTimeTerm : ValueTerm
    {
      public DateTimeTerm (DateTime value)
      {
        functor = value;
        termType = TermType.DateTime;
      }

      protected override int CompareValue (BaseTerm t)
      { return (To<DateTime> ().CompareTo (t.To<DateTime> ())); }

      public override string ToWriteString (int level)
      {
        return "'" + ((DateTime)functor).ToString (ConfigSettings.DefaultDateTimeFormat) + "'";
      }
    }
    #endregion

    #region TimeSpanTerm
    public class TimeSpanTerm : ValueTerm
    {
      public TimeSpanTerm (TimeSpan value)
      {
        functor = value;
        termType = TermType.TimeSpan;
      }

      protected override int CompareValue (BaseTerm t)
      { return (To<TimeSpan> ().CompareTo (t.To<TimeSpan> ())); }

      public override string ToWriteString (int level)
      {
        return "'" + ((TimeSpan)functor).ToString () + "'";
      }
    }
    #endregion

    #region BoolTerm
    public class BoolTerm : ValueTerm
    {
      //static byte orderPosition = 7;

      public BoolTerm (bool value)
      {
        functor = value;
        termType = TermType.Bool;
      }

      protected override int CompareValue (BaseTerm t)
      { return (To<bool> ().CompareTo (t.To<bool> ())); }

      public override string ToWriteString (int level)
      {
        return ((bool)functor ? "true" : "false");
      }
    }
    #endregion

    #region FileTerm
    public class FileTerm : BaseTerm
    {
      // in order to be able to close all open streams after command termination:
      public static AtomTerm END_OF_FILE;
      protected PrologEngine engine;
      protected string fileName;
      public virtual bool IsOpen { get { return false; } }

      static FileTerm ()
      {
        END_OF_FILE = new AtomTerm ("end_of_file");
      }

      public virtual void Close ()
      {
      }
    }


    public class FileReaderTerm : FileTerm
    {
      TextReader tr = null;
      FileStream fs;

      PrologParser p = null;
      public override bool IsOpen { get { return (tr != null); } }
      public bool Eof { get { return (tr == null || tr.Peek () == -1); } }

      public FileReaderTerm (PrologEngine engine, string fileName)
      {
        this.engine = engine;
        functor = this.fileName = fileName;
        termType = TermType.FileReader;
      }

      public FileReaderTerm (PrologEngine engine, TextReader tr)
      {
        this.engine = engine;
        functor = this.fileName = "<standard input>";
        this.tr = tr;
        termType = TermType.FileReader;
      }

      public void Open ()
      {
        try
        {
          if (tr == null)
          {
            fs = new FileStream (fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            tr = new StreamReader (fs);
          }

          p = new PrologParser (engine);
          p.SetInputStream (tr);
          p.InitParse ();
        }
        catch (Exception e)
        {
          engine.Throw (PrologEngine.IOException,
            "Error while opening file '{0}' for input.\r\nMessage was:\r\n{1}",
            fileName, e.Message);
        }
      }


      public int ReadChar () // returns -1 at end of file
      {
        return p.ReadChar ();
      }


      public string ReadLine () // returns null at end of file
      {
        return p.ReadLine ();
      }


      public BaseTerm ReadTerm ()
      {
        BaseTerm result = p.ParseTerm ();

        return (result == null) ? END_OF_FILE : result;
      }


      public override void Close ()
      {
        if (p != null) p.ExitParse ();

        if (tr != null)
          tr.Close ();
      }
    }


    public class FileWriterTerm : FileTerm
    {
      TextWriter tw = null;
      FileStream fs;
      public override bool IsOpen { get { return (tw != null); } }

      public FileWriterTerm (PrologEngine engine, string fileName)
      {
        this.engine = engine;
        functor = this.fileName = fileName;
        termType = TermType.FileWriter;
      }

      public FileWriterTerm (TextWriter tw)
      {
        functor = this.fileName = "<standard output>";
        this.tw = tw;
        termType = TermType.FileWriter;
      }

      public void Open ()
      {
        try
        {
          if (tw == null)
          {
            fs = new FileStream (fileName, FileMode.Create, FileAccess.Write);
            tw = new StreamWriter (fs);
          }
        }
        catch (Exception e)
        {
          engine.Throw (PrologEngine.IOException,
           "Error while opening file '{0}' for output.\r\nMessage was:\r\n{1}",
           fileName, e.Message);
        }
      }


      public void WriteTerm (BaseTerm t)
      {
        tw.WriteLine ("{0}.", t);
      }


      public void Write (string s)
      {
        tw.Write (s);
      }


      public void Write (string s, params object [] args)
      {
        tw.Write (s, args);
      }


      public void NewLine ()
      {
        tw.Write (Environment.NewLine);
      }


      public override void Close ()
      {
        if (tw != null)
          tw.Close ();
      }
    }
    #endregion FileTerm
    #endregion ValueTerm

    #region CollectionTerm
    public class CollectionTerm : AtomTerm // for creating collections of terms (e.g. setof)
    {
      BaseTermSet set;
      public int Count { get { return set.Count; } }
      public override bool IsCallable { get { return false; } }

      public CollectionTerm (DupMode dupMode)
        : base ("<term collection>")
      {
        set = new BaseTermSet (dupMode);
      }

      public void Add (BaseTerm t)
      {
        set.Add (t);
      }

      public void Insert (BaseTerm t)
      {
        set.Insert (t);
      }

      public ListTerm ToList ()
      {
        return set.ToList ();
      }
    }
    #endregion CollectionTerm

    #region BaseTermListTerm
    public class BaseTermListTerm<T> : AtomTerm
    {
      List<T> list;
      public List<T> List { get { return list; } }
      public int Count { get { return list.Count; } }
      public T this [int n] { get { return list [n]; } }
      public override bool IsCallable { get { return false; } }

      public BaseTermListTerm ()
        : base ("<term list>")
      {
        list = new List<T> ();
      }

      public BaseTermListTerm (List<T> list)
        : base ("<term collection>")
      {
        this.list = list;
      }
    }
    #endregion BaseTermListTerm

    #region Cut
    public class Cut : BaseTerm
    {
      public override bool IsCallable { get { return false; } }
      public override bool IsEvaluatable { get { return false; } }

      public Cut (int stackSize)
      {
        functor = PrologParser.CUT;
        TermId = stackSize;
      }

      public override string ToWriteString (int level)
      {
        return PrologParser.CUT;
      }
    }
    #endregion Cut

    #region ListTerm
    public class ListTerm : CompoundTerm
    {
      bool isEvaluated = false; // for the is-operator
      protected string leftBracket = "[";
      protected string rightBracket = "]";
      string charCodeString = null;  // for ISO-strings only
      protected ListTerm EmptyList = EMPTYLIST;
      protected bool isAltList = false;
      public override bool IsEvaluatable { get { return true; } } // evaluate all members

      public string LeftBracket { get { return leftBracket; } }
      public string RightBracket { get { return rightBracket; } }
      public string CharCodeString { get { return charCodeString; } }
      public bool IsEvaluated { get { return isEvaluated; } set { isEvaluated = value; } }

      public ListTerm ()
        : base ("[]") { }

      public ListTerm (BaseTerm t)
        : base (PrologParser.DOT, t.ChainEnd (), EMPTYLIST) { }

      public ListTerm (BaseTerm t0, BaseTerm t1)
        : base (PrologParser.DOT, t0.ChainEnd (), t1.ChainEnd ()) { }

      // for ListPattern; *not* intended for creating a list from an array, use ListFromArray
      public ListTerm (BaseTerm [] a) 
        : base (PrologParser.DOT, a) { }

      public ListTerm (string charCodeString)
        : base (PrologParser.DOT)
      {
        if (charCodeString.Length == 0)
        {
          functor = "[]";

          return;
        }

        this.charCodeString = charCodeString;
        args = new BaseTerm [2];

        args [0] = new DecimalTerm ((decimal)charCodeString [0]);
        args [1] = new ListTerm (charCodeString.Substring (1));
      }


      public static ListTerm ListFromArray (BaseTerm [] ta, BaseTerm afterBar)
      {
        ListTerm result = null;

        for (int i = ta.Length - 1; i >= 0; i--)
          result = new ListTerm (ta [i], result == null ? afterBar : result);

        return result;
      }


      public List<BaseTerm> ToList ()
      {
        List<BaseTerm> result = new List<BaseTerm> ();

        foreach (BaseTerm t in this)
          result.Add (t);

        return result;
      }

      //public override bool IsCallable { get { return false; } }
      int properLength // only defined for proper lists
      {
        get
        {
          BaseTerm t = ChainEnd ();
          int len = 0;

          while (t.Arity == 2 && t is ListTerm)
          {
            t = t.Arg (1);
            len++;
          }

          return (t.IsEmptyList) ? len : -1;
        }
      }
      public int ProperLength { get { return properLength; } }

      public override bool IsListNode
      { get { BaseTerm t = ChainEnd (); return (t is ListTerm && t.Arity == 2); } }

      public override bool IsProperOrPartialList
      {
        get
        {
          BaseTerm t = ChainEnd ();

          while (t.Arity == 2 && t is ListTerm) t = t.Arg (1);

          return (t.IsEmptyList || t is Variable);
        }
      }


      public override bool IsProperList // e.g. [foo] (= [foo|[]])
      {
        get
        {
          BaseTerm t = ChainEnd ();

          while (t.Arity == 2 && t is ListTerm) t = t.Arg (1);

          return (t.IsEmptyList);
        }
      }


      public override bool IsPartialList // e.g. [foo|Atom]
      {
        get
        {
          BaseTerm t = ChainEnd ();

          while (t.Arity == 2 && t is ListTerm) t = t.Arg (1);

          return (t is Variable);
        }
      }


      public override bool IsPseudoList // e.g.: [foo|baz]
      {
        get
        {
          BaseTerm t = ChainEnd ();

          while (t.Arity == 2 && t is ListTerm) t = t.Arg (1);

          return (!(t.IsEmptyList || t is Variable));
        }
      }


      public BaseTerm [] ToTermArray ()
      {
        int length = 0;
            
        foreach (BaseTerm t in this) length++;

        BaseTerm [] result = new BaseTerm [length];

        length = 0;

        foreach (BaseTerm t in this) result [length++] = t;

        return result;
      }


      public static ListTerm ListFromArray (BaseTerm [] ta)
      {
        return ListFromArray (ta, EMPTYLIST);
      }


      public IEnumerator GetEnumerator ()
      {
        BaseTerm t = ChainEnd ();

        while (t.Arity == 2)
        {
          yield return t.Arg (0);

          t = t.Arg (1);
        }
      }


      public virtual ListTerm Reverse ()
      {
        ListTerm result = EmptyList;

        foreach (BaseTerm t in this) result = new ListTerm (t, result);

        return result;
      }


      public BaseTerm Append (BaseTerm list) // append t to 'this'
      {
        if (this.IsEmptyList) return list; // not necessarily a ListTerm

        if (list.IsEmptyList) return this;

        BaseTerm t0, t1;
        t1 = t0 = this;

        // find rightmost '.'-term and replace its right arg by t
        while (t1.Arity == 2)
        {
          t0 = t1;
          t1 = t1.Arg (1);
        }

        ((ListTerm)t0.ChainEnd ()).SetArg (1, list);

        return this;
      }


      public BaseTerm AppendElement (BaseTerm last) // append last to 'this'
      {
        return Append (new ListTerm (last));
      }


      public virtual ListTerm FlattenList ()
      {
        List<BaseTerm> a = FlattenListEx (functor); // only sublists with the same functor

        ListTerm result = EmptyList;

        for (int i = a.Count - 1; i >= 0; i--)
          result = new ListTerm (a [i], result); // [a0, a0, ...]

        return result;
      }


      protected List<BaseTerm> FlattenListEx (object functor)
      {
        BaseTerm t = this;
        BaseTerm t0;
        List<BaseTerm> result = new List<BaseTerm> ();

        while (t.IsListNode)
        {
          if ((t0 = t.Arg (0)).IsProperOrPartialList && ((ListTerm)t0).functor.Equals (functor))
            result.AddRange (((ListTerm)t0).FlattenListEx (functor));
          else
            result.Add (t0);

          t = t.Arg (1);
        }

        if (t.IsVar) result.Add (t); // open tail, i.e. [1|M]

        return result;
      }

      // Intersection and Union: nice excercise for later.
      // Should also cope with partial and pseudo lists (cf. SWI-Prolog)
      public ListTerm Intersection (ListTerm that)
      {
        ListTerm result = EmptyList;

        foreach (ListTerm t0 in this)
        {
          foreach (ListTerm t1 in that)
          {
          }
        }

        return result;
      }


      public ListTerm Union (ListTerm that)
      {
        ListTerm result = EmptyList;

        return result;
      }


      public bool ContainsAtom (string atom)
      {
        BaseTerm t = ChainEnd ();

        while (t.Arity == 2)
        {
          if (t.Arg (0).FunctorToString == atom) return true;

          t = t.Arg (1);
        }

        return false;
      }


      public override string ToWriteString (int level)
      {
        // insert an extra space in case of non-standard list brackets
        string altListSpace = (isAltList ? " " : null);

        if (IsEmptyList)
          return leftBracket + altListSpace + rightBracket;

        if (MaxWriteDepthExceeded (level)) return "[...]";

        StringBuilder sb = new StringBuilder (leftBracket + altListSpace);
        BaseTerm t = ChainEnd ();

        bool first = true;

        while (t.IsListNode)
        {
          if (first) first = false; else sb.Append (CommaAtLevel (level));

          sb.AppendPacked (t.Arg (0).ToWriteString (level + 1), t.Arg (0).FunctorIsBinaryComma);
          t = t.Arg (1);
        }

        if (!t.IsEmptyList)
          sb.AppendFormat ("|{0}", t.ToWriteString (level + 1).Packed (t.FunctorIsBinaryComma));

        sb.Append (altListSpace + rightBracket);

        if (charCodeString != null) // show string value in comment
          sb.AppendFormat ("  /*{0}*/", charCodeString.Replace ("*/", "\\x2A/"));

        return sb.ToString ();
      }


      public override string ToDisplayString (int level)
      {
        if (IsEmptyList) return "leftBracket + rightBracket";

        StringBuilder sb = new StringBuilder (".(");
        sb.Append (Arg (0).ToDisplayString (level));
        sb.Append (CommaAtLevel (level));
        sb.Append (Arg (1).ToDisplayString (level));
        sb.Append (")");

        return sb.ToString ();
      }


      public override void TreePrint (int level, PrologEngine e)
      {
        string margin = Spaces (2 * level);

        if (IsEmptyList)
        {
          e.WriteLine ("{0}{1}", margin, EMPTYLIST);

          return;
        }

        e.WriteLine ("{0}{1}", margin, leftBracket);

        BaseTerm t = ChainEnd ();

        while (t.IsListNode)
        {
          t.Arg (0).TreePrint (level + 1, e);
          t = t.Arg (1);
        }

        e.WriteLine ("{0}{1}", margin, rightBracket);
      }


      public string [] ToStringArray ()
      {
        if (!IsProperList) return null;

        string [] result = new string [properLength];

        BaseTerm t = ChainEnd ();
        int i = 0;

        while (t.Arity == 2 && t is ListTerm)
        {
          string s = t.Arg (0).ToString ();
          result [i++] = s.Dequoted ("'").Dequoted ("\"").Unescaped ();
          t = t.Arg (1);
        }

        return result;
      }

      /*
          From: http://www.sics.se/isl/quintus/html/quintus/lib-lis-prl.html

          What is a "Proper" List?

          Several of the predicate descriptions below indicate that a particular predicate
          only works when a particular argument "is a proper list".

          A proper list is either the Atom [] or else it is of the form [_|L] where L is a
          proper list.

          X is a partial list if and only if var(X) or X is [_|L] where L is a partial list.
          A term is a list if it is either a proper list or a partial list; that is, [_|foo]
          is not normally considered to be a list because its tail is neither a variable nor [].

          Note that the predicate list(X) defined in library(lists) really tests whether
          X is a proper list. The name is retained for compatibility with earlier releases
          of the library. Similarly, is_set(X) and is_ordset(X) test whether X is a proper
          list that possesses the additional properties defining sets and ordered sets.

          The point of the definition of a proper list is that a recursive procedure working
          its way down a proper list can be certain of terminating. Let us take the case of
          list/2 as an example. list(X, L) ought to be true when append(_, [X], L) is true.
          The obvious way of doing this is

               list(List, [List]).
               list(List, [_|End]) :-
                       list(List, End).

          If called with the second argument a proper list, this definition can be sure of
          terminating (though it will leave an extra choice point behind). However, if you Call

               | ?- list(X, L), properLength(L, 0).

          where L is a variable, it will backtrack forever, trying ever longer lists.
          Therefore, users should be sure that only proper lists are used in those argument
          positions that require them.

         */
    }
    #endregion ListTerm

    #region DcgTerm
    public class DcgTerm : CompoundTerm
    {
      public DcgTerm (BaseTerm t, ref BaseTerm z)
        : base (t.FunctorToString, new BaseTerm [t.Arity + 2])
      {
        for (int i = 0; i < t.Arity; i++) args [i] = t.Arg (i);

        args [arity - 2] = z;
        args [arity - 1] = z = new Variable ();
      }

      public DcgTerm (BaseTerm t) : base (PrologParser.CURL, t, NULLCURL) { }

      public DcgTerm (BaseTerm t0, BaseTerm t1) : base (PrologParser.CURL, t0, t1) { }

      public DcgTerm () : base (PrologParser.CURL) { }

      public DcgTerm (object functor, BaseTerm [] args) : base (functor, args) { }


      public override bool IsDcgList
      { get { return (ChainEnd () == NULLCURL || ChainEnd () is DcgTerm); } }

      public DcgTerm FlattenDcgList ()
      {
        List<BaseTerm> a = FlattenDcgListEx ();

        DcgTerm result = NULLCURL; // {}

        for (int i = a.Count - 1; i >= 0; i--)
          result = new DcgTerm (a [i], result); // {a0, a0, ...}

        return result;
      }

      List<BaseTerm> FlattenDcgListEx ()
      {
        BaseTerm t = this;
        BaseTerm t0;
        List<BaseTerm> result = new List<BaseTerm> ();

        while (t.FunctorToString == PrologParser.CURL && t.Arity == 2)
        {
          if ((t0 = t.Arg (0)).IsDcgList)
            result.AddRange (((DcgTerm)t0).FlattenDcgListEx ());
          else
            result.Add (t0);

          t = t.Arg (1);
        }

        if (t.IsVar) result.Add (t);

        return result;
      }


      public override string ToDisplayString (int level)
      {
        if (this == NULLCURL) return PrologParser.CURL;

        StringBuilder sb = new StringBuilder ("'{}'(");
        sb.Append (Arg (0).ToDisplayString (level));
        sb.Append (CommaAtLevel (level));
        sb.Append (Arg (1).ToDisplayString (level));
        sb.Append (")");

        return sb.ToString ();
      }


      public override void TreePrint (int level, PrologEngine e)
      {
        string margin = Spaces (2 * level);

        e.WriteLine ("{0}{1}", margin, '{');

        BaseTerm t = ChainEnd ();

        while (t.IsListNode)
        {
          t.Arg (0).TreePrint (level + 1, e);
          t = t.Arg (1);
        }

        e.WriteLine ("{0}{1}", margin, '}');
      }
    }
    #endregion DcgTerm

    #region BinaryTerm // for accomodating binary data i.e. from SQL-queries
    public class BinaryTerm : BaseTerm
    {
      byte [] data;

      public override bool IsCallable { get { return false; } }

      public BinaryTerm (byte [] data)
      {
        functor = "(binary data)";
        this.data = data;
        termType = TermType.Binary;
      }


      protected override int CompareValue (BaseTerm t)
      {
        return FunctorToString.CompareTo (t.FunctorToString);
      }


      public override string ToWriteString (int level)
      {
        return string.Format ("\"(byte[{0}] binary data)\"", data.LongLength);
      }

    }
    #endregion BinaryTerm

    #region UserClassTerm
    public class UserClassTerm<T> : BaseTerm
    {
      T userObject;
      public T UserObject { get { return userObject; } set { userObject = value; } }

      public UserClassTerm (T obj)
        : base ()
      {
        this.userObject = obj;
      }
    }
    #endregion UserClassTerm
  }
}
