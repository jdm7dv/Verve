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

namespace Prolog
{
  public partial class PrologEngine
  {
    #region AltListTerm
    public class AltListTerm : ListTerm
    {
      public override string FunctorToString { get { return functor.ToString ().ToAtom (); } }

      public AltListTerm (string leftBracket, string rightBracket)
      {
        isAltList = true;
        functor = leftBracket + ".." + rightBracket;
        this.leftBracket = leftBracket;
        this.rightBracket = rightBracket;
      }

      public AltListTerm (string leftBracket, string rightBracket, BaseTerm t0, BaseTerm t1)
        : base (t0.ChainEnd (), t1.ChainEnd ())
      {
        isAltList = true;
        functor = leftBracket + ".." + rightBracket;
        this.leftBracket = leftBracket;
        this.rightBracket = rightBracket;
      }

      public AltListTerm (string leftBracket, string rightBracket, BaseTerm [] a)
        : base (a)
      {
        isAltList = true;
        functor = leftBracket + ".." + rightBracket;
        this.leftBracket = leftBracket;
        this.rightBracket = rightBracket;
      }

      public static AltListTerm ListFromArray (
        string leftBracket, string rightBracket, BaseTerm [] ta, BaseTerm afterBar)
      {
        AltListTerm result = null;

        for (int i = ta.Length - 1; i >= 0; i--)
          result = new AltListTerm (leftBracket, rightBracket, ta [i], result == null ? afterBar : result);

        return result;
      }


      public override ListTerm Reverse ()
      {
        AltListTerm result = new AltListTerm (leftBracket, rightBracket);

        foreach (BaseTerm t in this) result =
          new AltListTerm (leftBracket, rightBracket, t, result);

        return result;
      }


      public override ListTerm FlattenList ()
      {
        List<BaseTerm> a = FlattenListEx (functor);

        AltListTerm result = new AltListTerm (leftBracket, rightBracket);

        for (int i = a.Count - 1; i >= 0; i--)
          result = new AltListTerm (leftBracket, rightBracket, a [i], result); // [a0, a0, ...]

        return result;
      }

      List<BaseTerm> FlattenListEx ()
      {
        throw new NotImplementedException ();
      }
    }
    #endregion AltListTerm

    #region JsonTerm
    public class JsonTerm : ListTerm
    {
      int maxIndentLevel = 1; // string representation will no longer be indented beyond maxIndentLevel
      public int MaxIndentLevel { private get { return maxIndentLevel; } set { maxIndentLevel = value; } }
      bool noQuotes = false;
      static JsonTerm EMPTY = new JsonTerm ();
      JsonTextBuffer jtb;

      public JsonTerm () : base () { }

      public JsonTerm (BaseTerm t0, BaseTerm t1) : base (t0, t1) { }

      public JsonTerm (ListTerm lt)
      {
        functor = lt.Functor;
        args = lt.Args;
      }

      public static JsonTerm FromArray (BaseTerm [] ta)
      {
        JsonTerm result = new JsonTerm ();

        for (int i = ta.Length - 1; i >= 0; i--)
          result = new JsonTerm (ta [i], result);

        return result;
      }


      // JSON formatting
      #region JsonTextBuffer
      class JsonTextBuffer
      {
        StringBuilder sb; // sealed, cannot inheritate from
        int level;
        int indentDelta; // increment per indentation level
        int maxIndentLevel; // no indentation beyond this level (flat structures only)
        bool noCommas; // no commas between list items
        string Indentation { get { return new string (' ', level * indentDelta); } }

        public JsonTextBuffer (int indentDelta, int maxIndentLevel, bool noCommas)
        {
          sb = new StringBuilder ();
          level = 0;
          this.indentDelta = indentDelta;
          this.maxIndentLevel = maxIndentLevel;
          this.noCommas = noCommas;
        }


        public void EmitOpenBracket (char c)
        {
          sb.Append (c);

          if (level <= maxIndentLevel)
          {
            level++;
            sb.Append (Indentation);
          }
        }


        public void EmitCloseBracket (char c)
        {
          if (level <= maxIndentLevel)
          {
            level--;
            sb.AppendLine ();
            sb.Append (Indentation);
          }
          else
            sb.Append (' ');

          sb.Append (c);
        }


        public void Newline ()
        {
          if (level <= maxIndentLevel)
          {
            sb.AppendLine ();
            sb.Append (Indentation);
          }
          else
            sb.Append (' ');
        }


        public void AppendPossibleCommaAndNewLine (ref bool first, int MaxIndentLevel)
        {
          if (first)
            first = false;
          else if (!noCommas)
            sb.Append (',');

          Newline ();
        }


        public void EmitString (string value)
        {
          sb.Append (value);
        }


        public override string ToString ()
        {
          return sb.ToString ();
        }
      }
      #endregion JsonTextBuffer

      public string ToJsonString (int indentDelta, int maxIndentLevel, bool noCommas, bool noQuotes)
      {
        jtb = new JsonTextBuffer (indentDelta, maxIndentLevel, noCommas);
        this.noQuotes = noQuotes;
        DoJsonStruct (this);

        return jtb.ToString ().Dequoted ();
      }


      public override string ToWriteString (int maxIndentLevel)
      {
        return ToJsonString (0, 0, false, false);
      }


      void DoJsonStruct (BaseTerm t) // object or array
      {
        if (t.FunctorToString == "array")
          DoJsonArray (t.Arg (0));
        else if (t.IsProperList)
          DoJsonObject (t);
        else
          IO.Error ("Not a well-formed JSON-term: {0}", t.ToString ());
      }


      void DoJsonObject (BaseTerm t) // object, array or literal
      {
        jtb.EmitOpenBracket ('{');
        bool first = true;

        // traverse list
        foreach (BaseTerm e in (ListTerm)t)
        {
          jtb.AppendPossibleCommaAndNewLine (ref first, MaxIndentLevel); // '{' <pair>+ '}'
          DoJsonPair (e);
        }

        jtb.EmitCloseBracket ('}');
      }


      void DoJsonArray (BaseTerm t) // '[' <value>+ ']'
      {
        jtb.EmitOpenBracket ('[');
        bool first = true;

        // traverse list
        foreach (BaseTerm e in (ListTerm)t)
        {
          jtb.AppendPossibleCommaAndNewLine (ref first, MaxIndentLevel); // newline & indentation
          DoJsonValue (e);
        }

        jtb.EmitCloseBracket (']');
      }


      void DoJsonPair (BaseTerm t) // <string> ':' <value>
      {
        if (t.Arity < 2)
          IO.Error ("Not a well-formed JSON-term: {0}", t.ToString ());

        DoJsonLiteral (t.Arg (0));
        jtb.EmitString (": ");
        BaseTerm arg1 = t.Arg (1);

        if (arg1.Arity == 0)
          DoJsonLiteral (arg1);
        else
        {
          jtb.Newline ();
          DoJsonValue (arg1);
        }
      }


      void DoJsonValue (BaseTerm t) // <object> | <array> | <literal>
      {
        if (t.Arity == 0)
          DoJsonLiteral (t);
        else
          DoJsonStruct (t);
      }


      void DoJsonLiteral (BaseTerm t)
      {
        string s = t.FunctorToString; // not quoted

        if (noQuotes)
          jtb.EmitString (s);
        else
        {
          if (s.HasSignedRealNumberFormat () || s == "false" || s == "true" || s == "null")
            jtb.EmitString (s);
          else
            jtb.EmitString (t.ToString ());
        }
      }
    }
    #endregion JsonTerm

    #region IntRangeTerm // to accomodate integer ranges such as X = 1..100, R is X.
    public class IntRangeTerm : CompoundTerm
    {
      BaseTerm lowBound;
      BaseTerm hiBound;
      IEnumerator iEnum;
      public override bool IsCallable { get { return false; } }

      public IntRangeTerm (BaseTerm lowBound, BaseTerm hiBound)
        : base ("..", lowBound, hiBound)
      {
        this.lowBound = lowBound;
        this.hiBound = hiBound;
        iEnum = GetEnumerator ();
      }

      public IntRangeTerm (IntRangeTerm that) // for copying only
        : base ("..", that.lowBound, that.hiBound)
      {
        this.lowBound = that.lowBound;
        this.hiBound = that.hiBound;
        iEnum = GetEnumerator ();
      }

      public ListTerm ToList ()
      {
        ListTerm result = ListTerm.EMPTYLIST;

        int lo = lowBound.To<int> ();
        int hi = hiBound.To<int> ();

        for (int i = hi; i >= lo; i--)
          result = new ListTerm (new DecimalTerm (i), result);

        return result;
      }

      IEnumerator GetEnumerator ()
      {
        int lo = lowBound.To<int> ();
        int hi = hiBound.To<int> ();

        for (int i = lo; i <= hi; i++)
          yield return new DecimalTerm (i);
      }

      public bool GetNextValue (out DecimalTerm dt)
      {
        dt = null;

        if (!iEnum.MoveNext ()) return false;

        dt = (DecimalTerm)iEnum.Current;

        return true;
      }


      public override string ToWriteString (int level)
      {
        return string.Format ("{0}..{1}", lowBound.To<int> (), hiBound.To<int> ());
      }


      public override void TreePrint (int level, PrologEngine e)
      {
        e.WriteLine ("{0}{1}..{2}", Spaces (2 * level), lowBound.To<int> (), hiBound.To<int> ());
      }
    }
    #endregion IntRangeTerm

    #region DbConnectionTerm
    // to store database connection info before and between calls to sql_select/2 and sql_command/2/3
    public class DbConnectionTerm : StringTerm
    {
      DbCommand dbCommand;
      public DbCommand DbCommand { get { return dbCommand; } }
      public string Connectstring { get { return dbCommand.Connection.ConnectionString; } }
      public string CommandText { get { return dbCommand.CommandText; } }
      public override bool IsCallable { get { return false; } }

      public DbConnectionTerm (DbCommand dbCommand)
        : base (dbCommand.CommandText) // SQL-command
      {
        this.dbCommand = dbCommand;
      }

      public DbConnectionTerm (DbConnectionTerm t)
        : base (t.dbCommand.CommandText)
      {
        dbCommand = t.dbCommand;
      }

      public override string ToString ()
      {
        return string.Format ("Connectstring: '{0}'\r\nCommandText  : '{1}'",
          Connectstring, CommandText);
      }
    }
    #endregion DbConnectionTerm

    // Auxiliary code for supporting the SQL predicates
    #region DbCommandSet
    public class DbCommandSet : List<DbCommand>
    {
      const int maxConnections = 64; // arbitray choice

      public DbCommand GetCommand (BaseTerm provider, BaseTerm connectionArgs)
      {
        string sqlProvider = "(not set)";
        string sqlConnectstring = "(not set)";
        string providerKey = provider.FunctorToString;
        sqlProvider = "(not set)";
        sqlConnectstring = "(not set)";
        DbConnection dbConnection;
        DbProviderFactory dbProviderFactory;
        DbCommand dbCommand = null;

        if (Count == maxConnections)
          IO.Error ("Maximum number of database connections ({0}) has been reached", maxConnections);

        try
        {
          string connectInfo = ConfigSettings.GetConfigSetting (providerKey, null);

          if (connectInfo == null)
            IO.Error ("No SQL provider info found in config file for key '{0}'", providerKey);

          string [] s = connectInfo.Split ('|');

          if (s == null || s.Length != 2)
            IO.Error ("Ill-formatted connection string in config file:\r\n'{0}'", connectInfo);

          sqlProvider = s [0];
          sqlConnectstring = Utils.Format (s [1], connectionArgs);
          dbProviderFactory = DbProviderFactories.GetFactory (sqlProvider);
          dbConnection = dbProviderFactory.CreateConnection ();
          dbCommand = dbProviderFactory.CreateCommand ();
          dbCommand.Connection = dbConnection;
          dbConnection.ConnectionString = sqlConnectstring;
          dbConnection.Open ();
          Add (dbCommand);
        }
        catch (Exception e)
        {
          IO.Fatal (
@"Unable to open database connection.
Provider       : {0}
Connectstring  : {1}
System message : {2}",
            sqlProvider, sqlConnectstring, e.Message);
        }

        return dbCommand;
      }


      public void CloseAllConnections ()
      {
        foreach (DbCommand c in this)
          c.Connection.Close ();

        Clear ();
      }


      public void Close (BaseTerm t)
      {
        DbCommand dbCommand = ((DbConnectionTerm)t).DbCommand;

        for (int i = 0; i < Count; i++)
        {
          if (dbCommand == this [i])
          {
            dbCommand.Connection.Close ();
            Remove (this [i]);
          }
        }
      }
    }
    #endregion DbCommandSet

    #region ComplexTerm // can be used with the is-operator
    public class ComplexTerm : NumericalTerm
    {
      double re;
      double im;
      public double Re { get { return re; } }
      public double Im { get { return im; } }
      double magnitude { get { return Math.Sqrt ((double)(re * re + im * im)); } }
      double arg { get { return Math.Atan2 (im, re); } }
      public double Magnitude { get { return magnitude; } }
      public double Phi { get { return arg; } } // name Arg already in use
      public DecimalTerm ReTerm { get { return new DecimalTerm (re); } }
      public DecimalTerm ImTerm { get { return new DecimalTerm (im); } }
      ComplexTerm eposx;
      ComplexTerm enegx;
      static ComplexTerm ZERO = new ComplexTerm (0f, 0f);
      static ComplexTerm ONE = new ComplexTerm (1f, 0f);
      static ComplexTerm TWO = new ComplexTerm (2f, 0f);
      static ComplexTerm I = new ComplexTerm (0f, 1f);
      static ComplexTerm TWO_I = new ComplexTerm (0f, 2f);
      static ComplexTerm MINUS_I = new ComplexTerm (0f, -1f);
      static ComplexTerm MINUS_2I = new ComplexTerm (0f, -2f);

      public ComplexTerm (decimal re, decimal im)
      {
        this.re = (double)re;
        this.im = (double)im;
        functor = string.Format ("{0}+{1}i", re, im);
        termType = TermType.Number;
      }

      public ComplexTerm (DecimalTerm d)
      {
        this.re = d.ValueD;
        this.im = 0;
        functor = string.Format ("{0}+{1}i", re, im);
        termType = TermType.Number;
      }

      public ComplexTerm (double re, double im)
      {
        this.re = re;
        this.im = im;
        functor = string.Format ("{0}+{1}i", re, im);
        termType = TermType.Number;
      }


      public override int CompareTo (BaseTerm t)
      {
        ComplexTerm c;
        DecimalTerm d;

        if (t is ComplexTerm)
        {
          c = (ComplexTerm)t;

          if (re == c.re && im == c.im) return 0;

          return magnitude.CompareTo (((ComplexTerm)c).magnitude); // compare |this| and |c|
        }

        if (t is DecimalTerm)
        {
          d = (DecimalTerm)t;

          if (im == 0) return re.CompareTo (d.ValueD);

          return magnitude.CompareTo (d.ValueD);
        }

        IO.Error ("Relational operator cannot be applied to '{0}' and '{1}'", this, t);

        return 0;
      }


      public override bool Unify (BaseTerm t, VarStack varStack)
      {
        if (t is Variable) return t.Unify (this, varStack);

        NextUnifyCount ();
        const double eps = 1.0e-6; // arbitrary, cosmetic

        if (t is DecimalTerm)
          return (Math.Abs (im) < eps &&
                   Math.Abs (re - ((DecimalTerm)t).ValueD) < eps);

        if (t is ComplexTerm)
          return (Math.Abs (re - ((ComplexTerm)t).Re) < eps &&
                   Math.Abs (im - ((ComplexTerm)t).Im) < eps);

        //if (t is ComplexTerm)
        //  return ( re == ((ComplexTerm)t).Re && im == ((ComplexTerm)t).Im );

        return false;
      }


      ComplexTerm TimesI ()
      {
        return new ComplexTerm (-im, re);
      }


      ComplexTerm Plus1 ()
      {
        return new ComplexTerm (re + 1, im);
      }


      // sum
      public ComplexTerm Add (ComplexTerm c)
      {
        return new ComplexTerm (re + c.re, im + c.im);
      }

      public ComplexTerm Add (DecimalTerm d)
      {
        return new ComplexTerm (d.ValueD + re, im);
      }

      // difference
      public ComplexTerm Subtract (ComplexTerm c)
      {
        return new ComplexTerm (re - c.re, im - c.im);
      }


      public ComplexTerm Subtract (DecimalTerm d)
      {
        return new ComplexTerm (re - d.ValueD, im);
      }

      // product
      public ComplexTerm Multiply (ComplexTerm c)
      {
        return new ComplexTerm (re * c.re - im * c.im, re * c.im + c.re * im);
      }

      public ComplexTerm Multiply (DecimalTerm d)
      {
        return new ComplexTerm (re * d.ValueD, im * d.ValueD);
      }

      // quotient
      public ComplexTerm Divide (ComplexTerm c)
      {
        if (c.re == 0 && c.im == 0)
          IO.Error ("Division by zero complex number not allowed");

        double denominator = c.re * c.re + c.im * c.im;
        double newRe = (re * c.re + im * c.im) / denominator;
        double newIm = (im * c.re - re * c.im) / denominator;

        return new ComplexTerm (newRe, newIm);
      }


      public ComplexTerm Negative ()
      {
        return new ComplexTerm (-re, -im);
      }


      public ComplexTerm Conjugate ()
      {
        return new ComplexTerm (re, -im);
      }


      public ComplexTerm Divide (DecimalTerm d)
      {
        if (d.Value == 0)
          IO.Error ("Division by zero not allowed");

        return new ComplexTerm (re / d.ValueD, im / d.ValueD);
      }


      public ComplexTerm Log () // log|z| + i.arg(z), calculate the principal value
      {
        return new ComplexTerm (Math.Log (magnitude), arg);
      }


      public ComplexTerm Sqrt ()
      {
        return new ComplexTerm (Math.Sqrt ((magnitude + re) / 2), Math.Sqrt ((magnitude - re) / 2));
      }


      public ComplexTerm Sqr ()
      {
        return new ComplexTerm (re * re - im * im, 2 * re * im);
      }


      public ComplexTerm Exp () // engine^re * (cos (im) + i sin (im))
      {
        double exp = Math.Exp (re);

        return new ComplexTerm (exp * Math.Cos (im), exp * Math.Sin (im));
      }


      public ComplexTerm Abs ()
      {
        return new ComplexTerm (magnitude, 0);
      }


      public ComplexTerm Power (DecimalTerm d) // z^n = r^n * (cos (n*phi) + i*sin(n*phi))
      {
        if (d.IsInteger)
        {
          int n = d.To<int> ();

          if (re == 0 && im == 0)  // try Google 'what is 0 to the power of 0', etc.
            return new ComplexTerm (n == 0 ? 1 : 0, 0f);

          double rn = Math.Pow (magnitude, n);
          double nArg = n * arg;

          return new ComplexTerm (rn * Math.Cos (nArg), rn * Math.Sin (nArg));
        }
        else
          return (Log ().Multiply (d)).Exp (); // t^d = exp(log(t^d)) = exp(d*log(t))
      }

      // http://en.wikipedia.org/wiki/Exponentiation#Failure_of_power_and_logarithm_identities
      public ComplexTerm Power (ComplexTerm p) // also see IEEE 754-2008 floating point standard
      {
        if (re == 0 && im == 0)
          return new ComplexTerm ((p.re == 0 && p.im == 0 ? 1 : 0), 0f);

        return (Log ().Multiply (p)).Exp (); // t^p = exp(log(t^p)) = exp(p*log(t))
      }


      public ComplexTerm Sin () // (exp(iz) - exp(-iz)) / 2i
      {
        eposx = TimesI ().Exp ();
        enegx = (TimesI ().Negative ()).Exp ();

        return (eposx.Subtract (enegx)).Divide (TWO_I);
      }


      public ComplexTerm Cos () // (exp(iz) + exp(-iz)) / 2
      {
        eposx = TimesI ().Exp ();
        enegx = (TimesI ().Negative ()).Exp ();

        return (eposx.Add (enegx)).Divide (TWO);
      }


      public ComplexTerm Tan () //
      {
        eposx = TimesI ().Exp ();
        enegx = (TimesI ().Negative ()).Exp ();

        return (eposx.Subtract (enegx)).Divide ((eposx.Add (enegx)));
      }


      public ComplexTerm Sinh ()
      {
        eposx = Exp ();             // engine^z
        enegx = Negative ().Exp (); // engine^(-z)

        return (eposx.Subtract (enegx)).Divide (TWO);
      }


      public ComplexTerm Cosh ()
      {
        eposx = Exp ();             // engine^z
        enegx = Negative ().Exp (); // engine^(-z)

        return (eposx.Add (enegx)).Divide (TWO);
      }


      public ComplexTerm Tanh ()
      {
        eposx = Exp ();             // engine^z
        enegx = Negative ().Exp (); // engine^(-z)

        return (eposx.Subtract (enegx)).Divide (eposx.Add (enegx));
      }


      public ComplexTerm Asin () // log(z + sqrt(1+z^2))
      {
        return Add ((Sqr ().Plus1 ()).Sqrt ()).Log ();
      }


      public ComplexTerm Acos () // 2 * log( sqrt((z+1)/2) + sqrt((z-1)/2))
      {
        return ComplexTerm.ZERO;
      }


      public ComplexTerm Atan () // (log(1+z) - log(1-z)) / 2
      {
        ComplexTerm c0 = Plus1 ().Log ();
        ComplexTerm c1 = (Negative ().Plus1 ()).Log ();

        return (c0.Subtract (c1)).Divide (TWO);
      }


      public override string ToWriteString (int level)
      {
        // rounding: arbitrary, cosmetic, in order to prevent answers like 0+i or 1+0i
        double re = Math.Round (this.re, 6);
        double im = Math.Round (this.im, 6);

        if (im == 0)
          return (re == 0 ? "0" : re.ToString ("0.######", CIC));
        else // im != 0
        {
          string ims = null;

          if (im == -1)
            ims = "-";
          else if (im != 1)
            ims = im.ToString ("0.######", CIC);

          if (re == 0)
            return string.Format (CIC, "{0}i", ims);
          else
            return string.Format (CIC, "{0:0.######}{1}{2}i", re, (im < 0 ? null : "+"), ims);
        }
      }
    }
    #endregion ComplexTerm

  }
}
