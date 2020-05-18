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
using System.IO;
using System.Text;
#if mswindows
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
#endif

namespace Prolog
{
  #region SolutionSet
  public class SolutionSet
  {
    string query;
    public string Query { get { return query; } internal set { query = value; } }
    bool success;
    public bool Success { get { return success; } internal set { success = value; } }
    string errorMsg;
    public string ErrMsg { get { return errorMsg; } internal set { errorMsg = value; } }
    public bool HasError { get { return errorMsg != null; } }
    List<Solution> solutionSet;
    public int Count { get { return solutionSet.Count; } }
    Solution currVarSet;

    public SolutionSet ()
    {
      solutionSet = new List<Solution> ();
      success = false;
      errorMsg = null;
    }

    internal void CreateVarSet ()
    {
      solutionSet.Add (currVarSet = new Solution ());
    }

    internal void AddToVarSet (string name, string type, string value)
    {
      currVarSet.Add (name, type, value);
      success = true;
    }

    public IEnumerable<Solution> NextSolution
    {
      get
      {
        foreach (Solution s in solutionSet)
          yield return s;
      }
    }

    public Solution this [int i]
    {
      get
      {
        return solutionSet [i];
      }
    }

    public override string ToString ()
    {
      if (errorMsg != null)
        return errorMsg;

      if (success)
      {
        if (solutionSet.Count == 0)
          return "yes";
        else
        {
          StringBuilder sb = new StringBuilder ();
          int i = 0;
          foreach (Solution s in solutionSet)
            sb.AppendLine ("Solution {0}\r\n{1}", ++i, s.ToString ());

          return sb.ToString ();
        }
      }
      else
        return "no";
    }
  }
  #endregion SolutionSet

  #region Solution
  public class Solution // a solution is a set of variables
  {
    List<Variable> variables;
    int Count { get { return variables.Count; } }

    public Solution ()
    {
      variables = new List<Variable> ();
    }

    internal void Add (string name, string type, string value)
    {
      variables.Add (new Variable (name, type, value));
    }

    public IEnumerable<Variable> NextVariable
    {
      get
      {
        foreach (Variable v in variables)
          yield return v;
      }
    }

    public Variable this [int i]
    {
      get
      {
        return variables [i];
      }
    }

    public override string ToString ()
    {
      StringBuilder sb = new StringBuilder ();

      foreach (Variable v in variables)
        sb.AppendLine (v.ToString ());

      return sb.ToString ();
    }
  }
  #endregion Solution

  #region Variable
  public class Variable
  {
    string name;
    public string Name { get { return name; } }
    string type;
    public string Type { get { return type; } }
    string value;
    public string Value { get { return value; } }

    public Variable (string name, string type, string value)
    {
      this.name = name;
      this.type = type;
      this.value = value;
    }

    public override string ToString ()
    {
      return string.Format ("{0} ({1}) = {2}", name, type, value);
    }
  }
  #endregion Variable

  public partial class PrologEngine
  {
    #region Batch Processing
    public bool ProcessArgs (string [] args, bool windowsMode)
    {
      if (args.Length == 0) return false;

      // Process command line arguments. First is a query, optional second
      // arg is number of backtrack attempts (default is 0, 'infinite' = *)
      // If the first argument contains spaces, it must be enclosed in double quotes.
      // Example: pld "['path.pl'], solve( p(2,2), L)" 4
      string command = args [0].Dequoted ().Trim ();
      Query = command + (command.EndsWith (".") ? null : "."); // append a dot if necessary
      int solutionCount = 0; // i.e. find all solutions (backtrack until failure; value corresponds to '*')
      string msg = null;

      if (args.Length > 2)
        msg = string.Format ("Superfluous argument '{0}'", args [2]);
      else if (args.Length == 2 && !(int.TryParse (args [1], out solutionCount) || args [1] == "*"))
        msg = string.Format ("Illegal value '{0}' for maximum number of solutions", args [1]);
      else
        solutionCount = 1;

      if (msg == null)
      {
        int i = 0;
        bool found = false; // true if at least one solution found

        foreach (PrologEngine.ISolution s in SolutionIterator)
        {
          if (Error || (!found && !s.Solved)) // only an immediate 'no' give rise to an error
          {
            msg = s.ToString ();

            break;
          }

          if (++i == solutionCount) break;

          found = true;
        }
      }

      if (msg != null)
      {
        if (windowsMode)
          MessageBox.Show (msg);
        else
          Console.WriteLine (msg);

        Environment.ExitCode = 1; // sets DOS ERRORLEVEL to 1
      }

      return true;
    }
    #endregion Batch Processing


    #region GetAllSolutionsXml
    // Store solutions in an xml structure
    public string GetAllSolutionsXml (string sourceFileName, string destinFileName, string query)
    {
      return GetAllSolutionsXml (sourceFileName, destinFileName, query, 0);
    }

    public string GetAllSolutionsXml (string sourceFileName, string destinFileName, string query, int maxSolutionCount)
    {
      Reset ();
      XmlTextWriter xtw = null;
      StreamWriter sw = null;

      if (destinFileName == null)
        xtw = new XmlTextWriter (new MemoryStream (), System.Text.Encoding.GetEncoding (1252));
      else
        xtw = new XmlTextWriter (destinFileName, null);

      xtw.Formatting = Formatting.Indented;
      xtw.WriteStartDocument ();

      try
      {
        if (sourceFileName != null)
          Consult (sourceFileName);

        Query = query + (query.EndsWith (".") ? null : "."); // append a dot if necessary
        int i = 0;
        bool found = false; // true if at least one solution found
        bool firstSol = true;

        foreach (PrologEngine.ISolution s in SolutionIterator)
        {
          if (Error)
          {
            xtw.WriteStartElement ("error");
            xtw.WriteCData (s.ToString ());
            xtw.WriteEndElement ();

            break;
          }
          else if (!found && !s.Solved)
          {
            xtw.WriteStartElement ("solutions");
            xtw.WriteAttributeString ("success", "false");
            xtw.WriteStartElement ("query");
            xtw.WriteString (query);
            xtw.WriteEndElement ();

            break;
          }

          if (firstSol)
          {
            firstSol = false;
            xtw.WriteStartElement ("solutions");
            xtw.WriteAttributeString ("success", "true");
            xtw.WriteStartElement ("query");
            xtw.WriteString (query);
            xtw.WriteEndElement ();
          }

          bool firstVar = true;

          foreach (PrologEngine.IVarValue varValue in s.VarValuesIterator)
          {
            if (varValue.DataType == "none") break;

            if (firstVar)
            {
              firstVar = false;
              xtw.WriteStartElement ("solution");
            }

            xtw.WriteStartElement ("variable");
            xtw.WriteAttributeString ("name", varValue.Name);
            xtw.WriteAttributeString ("type", varValue.DataType);
            xtw.WriteString (varValue.Value.ToString ());
            xtw.WriteEndElement (); // variable
          }

          if (!firstVar) xtw.WriteEndElement (); // solution

          if (++i == maxSolutionCount) break;

          found = true;
        }

        if (!Error) xtw.WriteEndElement (); // solutions

        xtw.WriteEndDocument ();
        xtw.Flush ();

        if (destinFileName == null)
        {
          if (xtw.BaseStream == null) return null;

          return new ASCIIEncoding ().GetString (((MemoryStream)xtw.BaseStream).ToArray ());
        }
        else
          return null;
      }
      finally
      {
        if (xtw != null) xtw.Close ();
        if (sw != null) sw.Close ();
      }
    }
    #endregion GetAllSolutionsXml


    #region GetAllSolutions
    // Store solutions in an GetAllSolutions class
    public SolutionSet GetAllSolutions (string sourceFileName, string query)
    {
      return GetAllSolutions (sourceFileName, query, 0);
    }

    public SolutionSet GetAllSolutions (string sourceFileName, string query, int maxSolutionCount)
    {
      Reset ();
      SolutionSet solutions = new SolutionSet ();

      try
      {
        if (sourceFileName != null) Consult (sourceFileName);

        Query = solutions.Query = query + (query.EndsWith (".") ? null : "."); // append a dot if necessary
        int i = 0;
        bool found = false;
        bool varFound = false;

        foreach (PrologEngine.ISolution s in SolutionIterator)
        {
          if (Error)
          {
            solutions.ErrMsg = s.ToString ();

            break;
          }
          else if (!found && !s.Solved)
            break;

          solutions.Success = true;
          bool firstVar = true;

          foreach (PrologEngine.IVarValue varValue in s.VarValuesIterator)
          {
            if (varValue.DataType == "none") break;

            if (firstVar)
            {
              firstVar = false;
              solutions.CreateVarSet ();
            }

            solutions.AddToVarSet (varValue.Name, varValue.DataType, varValue.Value.ToString ());
            varFound = true;
          }

          if (++i == maxSolutionCount || !varFound) break;

          found = true;
        }
      }
      catch (Exception e)
      {
        solutions.ErrMsg = e.Message;
      }

      return solutions;
    }
    #endregion GetAllSolutions
  }
}
