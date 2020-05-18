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

// Code for calculating expression values (is/2)

// Future: use Visual Studio's 2010 'big number' features ?

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Prolog
{
  public partial class PrologEngine
  {
    public partial class BaseTerm
    {
      public T To<T> () where T : struct
      {
        BaseTerm e = ChainEnd ();

        try
        {
          return (e is ValueTerm)
            ? (T)Convert.ChangeType (e.functor, typeof (T))
            : e.Eval ().To<T> ();
        }
        catch
        {
          if (e is NamedVariable)
            IO.Error ("Unable to convert unbound variable {0} to type {1}",
              ((NamedVariable)e).Name, typeof (T).UnderlyingSystemType.Name);
          else if (e is Variable)
            IO.Error ("Unable to convert an unbound variable to type {0}",
              typeof (T).UnderlyingSystemType.Name);
          else if (e is ListTerm)
            IO.Error ("Unable to convert list {0} to type {1}",
              e, typeof (T).UnderlyingSystemType.Name);
          else
            IO.Error ("Unable to convert '{0}' to type {1}",
              e.FunctorToString, typeof (T).UnderlyingSystemType.Name);

          return default (T); // IO.Error throws error, but compiler insists on a return value
        }
      }

      public int CompareValue (ValueTerm t)
      {
        return 0;
      }

      /// <summary>Retrieves the argument value of a term </summary>
      public T Arg<T> (int pos) where T : struct
      {
        return args [pos].To<T> ();
      }


      decimal Trunc (decimal d) // chop decimal part
      {
        return (d > 0) ? Math.Floor (d) : Math.Ceiling (d);
      }


      public BaseTerm Eval () // evaluate the term
      {
        BaseTerm t = ChainEnd ();

        if (!t.IsEvaluatable)
          IO.Error ("{0} cannot be evaluated by is/2", t);

        if (t is ValueTerm) return t; // a ValueTerm stands for itself

        if (t.IsProperList && !((ListTerm)t).IsEvaluated) // evaluate all members recursively
        {
          ListTerm result = ListTerm.EMPTYLIST;
          List<BaseTerm> tl = ((ListTerm)t).ToList ();

          for (int i = tl.Count-1; i >= 0; i--)
            result = new ListTerm (tl [i].Eval (), result);

          result.IsEvaluated = true;

          return result;
        }

        return t.Apply ();
      }


      BaseTerm Apply () // apply the functor to the arguments
      {
        BaseTerm a0, a1, a2, a3;

        if (this.IsVar) IO.Error ("Unable to evaluate '{0}'", ((Variable)this).Name);

        if (arity == 0)
        {
          switch (FunctorToString)
          {
            case "pi":
              return new DecimalTerm (Math.PI);
            case "e":
              return new DecimalTerm (Math.E);
            case "i":
              return new ComplexTerm (0f, 1f);
            case "now":
              return new DateTimeTerm (DateTime.Now);
            case "today":
              return new DateTimeTerm (DateTime.Now.Date);
            case "yesterday":
              return new DateTimeTerm (DateTime.Now.AddDays (-1).Date);
            case "tomorrow":
              return new DateTimeTerm (DateTime.Now.AddDays (1).Date);
            case "false":
              return new BoolTerm (false);
            case "true":
              return new BoolTerm (true);
            default:
              return new StringTerm (FunctorToString);
              //IO.Error ("Unable to evaluate '{0}'", FunctorToString);
              //break;
          }
        }
        else if (arity == 1)
        {
          // do not evaluate the first arg of string/1.
          // E.engine 'string( 1+1)' will evaluate to "1+1", not "2"
          a0 = (FunctorToString == "string") ? Arg (0) : Arg (0).Eval ();

          switch (FunctorToString)
          {
            case "+":
              if (a0 is ComplexTerm)
                return (a0);
              return new DecimalTerm (a0.To<decimal> ());
            case "-":
              if (a0 is ComplexTerm)
                return ((ComplexTerm)a0).Negative ();
              return new DecimalTerm (-a0.To<decimal> ());
            case "~":
              break;
            case @"\":
              if (a0 is BoolTerm)
                return new BoolTerm (!a0.To<bool> ());
              else
                return new DecimalTerm (~(long)a0.To<double> ());
            case "abs":
              if (a0 is ComplexTerm)
                return ((ComplexTerm)a0).Abs ();
              else
                return new DecimalTerm (Math.Abs (a0.To<decimal> ()));
            case "exp":
              if (a0 is ComplexTerm)
                return ((ComplexTerm)a0).Exp ();
              else
                return new DecimalTerm (Math.Exp (a0.To<double> ()));
            case "sin":
              if (a0 is ComplexTerm) return ((ComplexTerm)a0).Sin ();
              return new DecimalTerm (Math.Sin (a0.To<double> ()));
            case "cos":
              if (a0 is ComplexTerm) return ((ComplexTerm)a0).Cos ();
              return new DecimalTerm (Math.Cos (a0.To<double> ()));
            case "tan":
              if (a0 is ComplexTerm) return ((ComplexTerm)a0).Tan ();
              return new DecimalTerm (Math.Tan (a0.To<double> ()));
            case "sinh":
              if (a0 is ComplexTerm) return ((ComplexTerm)a0).Sinh ();
              return new DecimalTerm (Math.Sinh (a0.To<double> ()));
            case "cosh":
              if (a0 is ComplexTerm) return ((ComplexTerm)a0).Cosh ();
              return new DecimalTerm (Math.Cosh (a0.To<double> ()));
            case "tanh":
              if (a0 is ComplexTerm) return ((ComplexTerm)a0).Tanh ();
              return new DecimalTerm (Math.Tanh (a0.To<double> ()));
            case "asin":
              if (a0 is ComplexTerm) return ((ComplexTerm)a0).Asin ();
              return new DecimalTerm (Math.Asin (a0.To<double> ()));
            case "acos":
              if (a0 is ComplexTerm) return ((ComplexTerm)a0).Acos ();
              return new DecimalTerm (Math.Acos (a0.To<double> ()));
            case "atan":
              if (a0 is ComplexTerm) return ((ComplexTerm)a0).Atan ();
              return new DecimalTerm (Math.Atan (a0.To<double> ()));
            case "log":
              if (a0 is ComplexTerm)
                return ((ComplexTerm)a0).Log ();
              else if (a0 is DecimalTerm)
                return (new ComplexTerm ((DecimalTerm)a0)).Log ();
              else
                return new DecimalTerm (Math.Log (a0.To<double> ()));
            case "log10":
              return new DecimalTerm (Math.Log10 (a0.To<double> ()));
            case "round":
              // WARNING: The Round method follows the IEEE Standard 754, section 4 standard.
              // If the number being rounded is halfway between two numbers, the C# Round operation
              // will always round to the even number. E.i. Round(1.5) = Round(2.5) = 2.
              return new DecimalTerm (Math.Round (a0.To<decimal> ()));
            case "floor":
              return new DecimalTerm (Math.Floor (a0.To<decimal> ()));
            case "trunc":
              return new DecimalTerm (Trunc (a0.To<decimal> ()));
            case "ceil":
              return new DecimalTerm (Math.Ceiling (a0.To<decimal> ()));
            case "sign":
              return new DecimalTerm (Math.Sign (a0.To<decimal> ()));
            case "sqrt":
              if (a0 is ComplexTerm)
                return ((ComplexTerm)a0).Sqrt ();
              else if (a0 is DecimalTerm)
                return (new ComplexTerm ((DecimalTerm)a0)).Sqrt ();
              else
                return new DecimalTerm (Math.Sqrt (a0.To<double> ()));
            case "sqr":
              if (a0 is ComplexTerm)
                return ((ComplexTerm)a0).Sqr ();
              else
              {
                decimal d = a0.To<decimal> ();
                return new DecimalTerm (d * d);
              }
            case "re":
              if (a0 is ComplexTerm)
                return ((ComplexTerm)a0).ReTerm;
              else
                return new DecimalTerm (a0.To<decimal> ()); // catchall
            case "im":
              if (a0 is ComplexTerm)
                return ((ComplexTerm)a0).ImTerm;
              else if (a0 is DecimalTerm)
                return new DecimalTerm (0);
              else
              {
                IO.Error ("Cannot take the imaginary part of '{0}'", a0);
                break;
              }
            case "conj":
              if (a0 is ComplexTerm)
                return ((ComplexTerm)a0).Conjugate ();
              else if (a0 is DecimalTerm)
                return a0;
              else
              {
                IO.Error ("Cannot take the complex conjugate of '{0}'", a0);
                break;
              }
            case "arg":
            case "phase":
            case "phi":
              if (a0 is ComplexTerm)
                return new DecimalTerm (((ComplexTerm)a0).Phi);
              else if (a0 is DecimalTerm)
                return new DecimalTerm (0);
              else
              {
                IO.Error ("Cannot take the arg/phase/phi of '{0}'", a0);
                break;
              }
            case "magnitude":
              if (a0 is ComplexTerm)
                return new DecimalTerm (((ComplexTerm)a0).Magnitude);
              else if (a0 is DecimalTerm)
                return a0;
              else
              {
                IO.Error ("Cannot take the complex magnitude of '{0}'", a0);
                break;
              }
            // string handling
            case "string":
            case "string2":
              return new StringTerm (string.Format ("{0}", a0.ToString ()));
            case "length":
              return new DecimalTerm ((a0.FunctorToString).Length);
            case "upcase":
              return new StringTerm ((a0.FunctorToString).ToUpper ());
            case "upcase1": // upcase first char; rest unchanged
              {
                string s = a0.FunctorToString;
                return new StringTerm ((s.Length == 0) ? "" : Char.ToUpper (s [0]) + s.Substring (1));
              }
            case "lowcase":
              return new StringTerm (a0.FunctorToString.ToLower ());
            case "trim":
              return new StringTerm (a0.FunctorToString.Trim ());
            case "trimstart":
              return new StringTerm (a0.FunctorToString.TrimStart ());
            case "reverse":
              return new StringTerm (a0.FunctorToString.Reverse ());
            case "trimend":
              return new StringTerm (a0.FunctorToString.TrimEnd ());
            case "singleline": // replace newlines by a single space (or null if already followed by a space)
              return new StringTerm (Regex.Replace (a0.FunctorToString, "(\r(\n| )?|\n ?)", " "));
            // DateTime stuff
            case "year":
              return new DecimalTerm ((a0.To<DateTime> ()).Year);
            case "month":
              return new DecimalTerm ((a0.To<DateTime> ()).Month);
            case "day":
              return new DecimalTerm ((a0.To<DateTime> ()).Day);
            case "hour":
              return new DecimalTerm ((a0.To<DateTime> ()).Hour);
            case "minute":
              return new DecimalTerm ((a0.To<DateTime> ()).Minute);
            case "second":
              return new DecimalTerm ((a0.To<DateTime> ()).Second);
            case "millisecond":
              return new DecimalTerm ((a0.To<DateTime> ()).Millisecond);
            case "dayofweek":
              return new DecimalTerm ((int)((a0.To<DateTime> ()).DayOfWeek));
            case "dayofyear":
              return new DecimalTerm ((a0.To<DateTime> ()).DayOfYear);
            case "ticks":
              return new DecimalTerm ((a0.To<DateTime> ()).Ticks);
            case "today":
              return new DateTimeTerm (DateTime.Today);
            case "timeofday":
              return new TimeSpanTerm (DateTime.Now.TimeOfDay);
            case "weekno":
              return new DecimalTerm (Utils.WeekNo (a0.To<DateTime> ()));
            case "dayname":
              return new StringTerm (((a0.To<DateTime> ()).DayOfWeek).ToString ("G"));
            default:
              IO.Error ("Not a built-in function: {0}/1", FunctorToString);
              break;
          }
        }
        else if (arity == 2)
        {
          a0 = Arg (0).Eval ();

          // do not evaluate the second arg of format/2.
          // E.engine 'format( "{0}", 1+1)' will evaluate to "1+1", not "2"
          a1 = (FunctorToString == "format") ? Arg (1) : Arg (1).Eval ();

          switch (FunctorToString)
          {
            case "..": // range -> list
              int lo = a0.To<int> ();
              int hi = a1.To<int> ();
              // create a list with elements [lo, lo+1, ... hi]
              ListTerm result = ListTerm.EMPTYLIST;

              for (int i = hi; i >= lo; i--)
                result = new ListTerm (new DecimalTerm (i), result);

              return result;
            case "+":
              if ((a0 is StringTerm || a0 is AtomTerm) &&
                (a1 is StringTerm || a1 is AtomTerm))
                return new StringTerm (a0.FunctorToString.Unescaped () + a1.FunctorToString.Unescaped ());
              else if (a0 is DateTimeTerm && a1 is TimeSpanTerm)
                return new DateTimeTerm ((a0.To<DateTime> ()).Add (a1.To<TimeSpan> ()));
              else if (a0 is ComplexTerm)
              {
                if (a1 is ComplexTerm) return ((ComplexTerm)a0).Add ((ComplexTerm)a1);

                return ((ComplexTerm)a0).Add ((DecimalTerm)a1);
              }
              else if (a1 is ComplexTerm)
              {
                if (a0 is DecimalTerm) return ((DecimalTerm)a0).Add ((ComplexTerm)a1);
              }
              return new DecimalTerm (a0.To<decimal> () + a1.To<decimal> ());
            case "-":
              if (a0 is DateTimeTerm)
              {
                if (a1 is TimeSpanTerm)
                  return new DateTimeTerm ((a0.To<DateTime> ()).Subtract (a1.To<TimeSpan> ()));
                else if (a1 is DateTimeTerm)
                  return new TimeSpanTerm ((a0.To<DateTime> ()).Subtract (a1.To<DateTime> ()));
                else
                  break;
              }
              else if (a0 is ComplexTerm)
              {
                if (a1 is ComplexTerm) return ((ComplexTerm)a0).Subtract ((ComplexTerm)a1);

                return ((ComplexTerm)a0).Subtract ((DecimalTerm)a1);
              }
              else if (a1 is ComplexTerm)
              {
                if (a0 is DecimalTerm) return ((DecimalTerm)a0).Subtract ((ComplexTerm)a1);
              }
              return new DecimalTerm (a0.To<decimal> () - a1.To<decimal> ());
            case "*":
              if (a0 is ComplexTerm)
              {
                if (a1 is ComplexTerm) return ((ComplexTerm)a0).Multiply ((ComplexTerm)a1);

                return ((ComplexTerm)a0).Multiply ((DecimalTerm)a1);
              }
              else if (a1 is ComplexTerm)
              {
                if (a0 is DecimalTerm) return ((DecimalTerm)a0).Multiply ((ComplexTerm)a1);
              }
              return new DecimalTerm (a0.To<decimal> () * a1.To<decimal> ());
            case "/":
              if (a0 is ComplexTerm)
              {
                if (a1 is ComplexTerm) return ((ComplexTerm)a0).Divide ((ComplexTerm)a1);

                return ((ComplexTerm)a0).Divide ((DecimalTerm)a1);
              }
              else if (a1 is ComplexTerm)
              {
                if (a0 is DecimalTerm) return ((DecimalTerm)a0).Divide ((ComplexTerm)a1);
              }
              return new DecimalTerm (a0.To<decimal> () / a1.To<decimal> ());
            case "<<":
              return new DecimalTerm (a0.To<long> () << a1.To<int> ());
            case ">>":
              return new DecimalTerm (a0.To<long> () >> a1.To<int> ());
            case "=":
              return new BoolTerm (a0.CompareTo (a1) == 0);
            case "\\=":
              return new BoolTerm (a0.CompareTo (a1) != 0);
            case "<>":
              return new BoolTerm (a0.CompareTo (a1) != 0);
            case "<":
              return new BoolTerm (a0.CompareTo (a1) < 0);
            case "=<":
              return new BoolTerm (a0.CompareTo (a1) <= 0);
            case ">":
              return new BoolTerm (a0.CompareTo (a1) > 0);
            case ">=":
              return new BoolTerm (a0.CompareTo (a1) >= 0);
            case "//":
              return new DecimalTerm (Trunc (a0.To<decimal> () / a1.To<decimal> ()));
            case "#":
              return new DecimalTerm (a0.To<long> () ^ a1.To<long> ());
            case @"/\":
              if (a0 is BoolTerm && a1 is BoolTerm)
                return new BoolTerm (a0.To<bool> () && a1.To<bool> ());
              else
                return new DecimalTerm (a0.To<long> () & a1.To<long> ());
            case @"\/":
              if (a0 is BoolTerm && a1 is BoolTerm)
                return new BoolTerm (a0.To<bool> () || a1.To<bool> ());
              else
                return new DecimalTerm (a0.To<long> () | a1.To<long> ());
            case "^":
              if (a0 is BoolTerm && a1 is BoolTerm)
                return new BoolTerm (a0.To<bool> () ^ a1.To<bool> ());
              else if (a0 is ComplexTerm)
                return (a1 is ComplexTerm)
                  ? ((ComplexTerm)a0).Power ((ComplexTerm)a1)
                  : ((ComplexTerm)a0).Power ((DecimalTerm)a1);
              else if (a1 is ComplexTerm)
                return (new ComplexTerm ((DecimalTerm)a0)).Power ((ComplexTerm)a1);
              else
                return new DecimalTerm (Math.Pow (a0.To<double> (), a1.To<double> ()));
            case "mod":
              return new DecimalTerm (a0.To<decimal> () % a1.To<decimal> ());
            case "round":
              return new DecimalTerm (Math.Round (a0.To<decimal> (), a1.To<int> ()));
            case "atan2":
              return new DecimalTerm (
                Math.Atan2 (a0.To<double> (), a1.To<double> ()));
            case "max":
              return new DecimalTerm (Math.Max (a0.To<decimal> (), a1.To<decimal> ()));
            case "min":
              return new DecimalTerm (Math.Min (a0.To<decimal> (), a1.To<decimal> ()));
            // string handling
            case "format":  // format without argument evaluation before substitution
            case "format2": // format with ...
              if (a0 is StringTerm && a1 is ListTerm)
                return new StringTerm (string.Format (a0.FunctorToString, ((ListTerm)a1).ToStringArray ()));
              else if (a0 is DateTimeTerm)
                return new StringTerm ((a0.To<DateTime> ()).ToString (a1.FunctorToString));
              else if (a1 is DecimalTerm)
                return new StringTerm (string.Format (a0.FunctorToString, a1.To<decimal> ()));
              else
                return new StringTerm (string.Format (a0.FunctorToString, a1.ToString ()));
            case "indexof":
              return new DecimalTerm ((a0.FunctorToString).IndexOf (a1.FunctorToString));
            case "padleft":
              return new StringTerm ((a0.FunctorToString).PadLeft (a1.To<int> ()));
            case "padright":
              return new StringTerm ((a0.FunctorToString).PadRight (a1.To<int> ()));
            case "remove":
              int len = a1.To<int> ();
              return new StringTerm ((a0.FunctorToString).Remove (len, (a1.FunctorToString).Length - len));
            case "substring":
              len = a1.To<int> ();
              return new StringTerm ((a0.FunctorToString).Substring (len, (a0.FunctorToString).Length - len));
            case "wrap":
              return new StringTerm (Utils.ForceSpaces (a0.FunctorToString, (a1.To<int> ())));
            case "split":
              string splitChars = a1.FunctorToString;
              ListTerm splitList = ListTerm.EMPTYLIST;
              if (splitChars.Length == 0)
              {
                splitChars = a0.FunctorToString;
                for (int i = splitChars.Length-1; i >= 0; i--)
                  splitList = new ListTerm (new StringTerm (splitChars [i]), splitList);
              }
              else
              {
                string [] part = a0.FunctorToString.Split (splitChars.ToCharArray ());
                for (int i = part.Length - 1; i >= 0; i--)
                  splitList = new ListTerm (new StringTerm (part [i]), splitList);
              }
              return splitList;
            case "chain":
              if (!a0.IsProperList)
                IO.Error ("chain/2:first argument '{0}' is not a proper list", a0);
              StringBuilder chain = new StringBuilder ();
              string separator = a1.FunctorToString;
              foreach (BaseTerm t in (ListTerm)a0)
              {
                if (chain.Length != 0) chain.Append (separator);
                chain.Append (t.FunctorToString);
              }
              return new StringTerm (chain.ToString ());
            case "repeat":
              return new StringTerm (a0.FunctorToString.Repeat (a1.To<int> ()));
            case "levdist": // Levenshtein distance
              return new DecimalTerm (a0.FunctorToString.Levenshtein (a1.FunctorToString));
            // date/time
            case "addyears":
              return new DateTimeTerm ((a0.To<DateTime> ()).AddYears (a1.To<int> ()));
            case "addmonths":
              return new DateTimeTerm ((a0.To<DateTime> ()).AddMonths (a1.To<int> ()));
            case "adddays":
              return new DateTimeTerm ((a0.To<DateTime> ()).AddDays (a1.To<int> ()));
            case "addhours":
              return new DateTimeTerm ((a0.To<DateTime> ()).AddHours (a1.To<int> ()));
            case "addminutes":
              return new DateTimeTerm ((a0.To<DateTime> ()).AddMinutes (a1.To<int> ()));
            case "addseconds":
              return new DateTimeTerm ((a0.To<DateTime> ()).AddSeconds (a1.To<int> ()));
            default:
              IO.Error ("Not a built-in function: {0}/2", FunctorToString);
              break;
          }
        }
        else if (arity == 3)
        {
          a0 = Arg (0).Eval ();
          a1 = Arg (1).Eval ();
          a2 = Arg (2).Eval ();

          switch (FunctorToString)
          {
            case "indexof":
              return new DecimalTerm ((a0.FunctorToString).IndexOf (a1.FunctorToString, a2.To<int> ()));
            case "remove":
              return new StringTerm ((a0.FunctorToString).Remove (a1.To<int> (), a2.To<int> ()));
            case "substring":
              return new StringTerm ((a0.FunctorToString).Substring (a1.To<int> (), a2.To<int> ()));
            case "replace":
              return new StringTerm ((a0.FunctorToString).Replace (a1.FunctorToString, a2.FunctorToString));
            case "regexreplace":
              return new StringTerm (Regex.Replace (a0.FunctorToString, a1.FunctorToString, a2.FunctorToString));
            case "time":
            case "timespan":
              return new TimeSpanTerm (
                new TimeSpan (a0.To<int> (), a1.To<int> (), a2.To<int> ()));
            case "date":
            case "datetime":
              return new DateTimeTerm (
                new DateTime (a0.To<int> (), a1.To<int> (), a2.To<int> ()));
            case "if":
              return new StringTerm (a0.To<bool> () ? a1.FunctorToString : a2.FunctorToString);
            default:
              IO.Error ("Not a built-in function: {0}/3", FunctorToString);
              break;
          }
        }
        else if (arity == 4)
        {
          a0 = Arg (0).Eval ();
          a1 = Arg (1).Eval ();
          a2 = Arg (2).Eval ();
          a3 = Arg (3).Eval ();

          if (HasFunctor ("timespan"))
            return new TimeSpanTerm (
              new TimeSpan (a0.To<int> (), a1.To<int> (), a2.To<int> (), a3.To<int> ()));
          else
            IO.Error ("Not a built-in function: {0}/4", FunctorToString);
        }
        else
          IO.Error ("Not a built-in function: {0}/{1}", FunctorToString, Arity);

        return null;
      }
    }
  }
}
