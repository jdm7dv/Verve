//#define showToken

namespace Prolog
{
  using System;
  using System.IO;
  using System.Text;
  using System.Xml;
  using System.Collections;
  using System.Globalization;
  using System.Security.Principal;
  using System.Diagnostics;

  /* _______________________________________________________________________________________________
    |                                                                                               |
    |  C#Prolog -- Copyright (C) 2007 John Pool -- j.pool@ision.nl                                  |
    |                                                                                               |
    |  This library is free software; you can redistribute it and/or modify it under the terms of   |
    |  the GNU General Public License as published by the Free Software Foundation; either version  |
    |  2 of the License, or any later version.                                                      |
    |                                                                                               |
    |  This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;    |
    |  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.    |
    |  See the GNU General Public License for details, or enter 'license.' at the command prompt.   |
    |_______________________________________________________________________________________________|
  */

  public partial class PrologEngine
  {
    #region PrologParser
    public partial class PrologParser : BaseParser<OpDescrTriplet>
    {
      #region Variables and initialization
      const char USCORE = '_';

      PrologEngine engine;
      PredicateTable ps;
      OperatorTable opTable;
      TermNode queryNode = null;
      
      public TermNode QueryNode { get { return queryNode; } }
      public int nargs;

      public const string IMPLIES = ":-";
      public const string DCGIMPL = "-->";
      public const string ARROW = "->";
      public const string DOT = ".";
      public const string CUT = "!";
      public const string OP = "op";
      public const string WRAP = "wrap";
      public const string STRINGSTYLE = "stringstyle";
      public const string CURL = "{}";
      public const string EQ = "=";
      public const string COLON = ":";
      public const string COMMA = ",";
      public const string QCOMMA = "','";
      public const string SEMI = ";";
      public const string SLASH = "/";
      public const string LISTPATOPEN = "[!";
      public const string LISTPATCLOSE = "!]";
      public const string TREEPATOPEN = "{!";
      public const string TREEPATCLOSE = "!}";
      public const string ELLIPSIS = "..";
      public const string SUBTREE = "\\";
      public const string NEGATE = "~";
      public const string PLUSSYM = "+";
      public const string TIMESSYM = "*";
      public const string QUESTIONMARK = "?";

      int savePlusSym;
      int saveTimesSym;
      int saveQuestionMark;

      public static readonly int Infinite = int.MaxValue;

      BaseTerm readTerm; // result of read (X)
      public BaseTerm ReadTerm { get { return readTerm; } }
      bool inQueryMode;
      bool jsonMode; // determines how curly brackets will be interpreted ('normal' vs. list)
      bool readingDcgClause; // determines how to interpret {t1, ... , tn}
      public bool InQueryMode { get { return inQueryMode; } }

      void Initialize ()
      {
        SetReservedOperators (false);
        terminalTable [PLUSSYM] = Operator;
        terminalTable [TIMESSYM] = Operator;
        terminalTable [QUESTIONMARK] = Atom;
        terminalTable.Remove ("module");            // will be temporarily set after an initial ':-' only
        terminalTable.Remove ("discontiguous");
        terminalTable.Remove ("alldiscontiguous");
        //terminalTable.Remove ("stringstyle");
        jsonMode = false; // Standard Prolog interpretation

        if (!ConfigSettings.VerbatimStringsAllowed)
          terminalTable.Remove (@"@""");
      }


      void Terminate ()
      {
        inQueryMode = true;
      }
      #endregion Variables and initialization

      #region Operator handling
      public OperatorDescr AddPrologOperator (int prec, string type, string name, bool user)
      {
        AssocType assoc = AssocType.None;

        try
        {
          assoc = (AssocType)Enum.Parse (typeof (AssocType), type);
        }
        catch
        {
          IO.Error ("Illegal operator type '{0}'", type);
        }

        if (prec < 0 || prec > 1200)
          IO.Error ("Illegal precedence value {0} for operator '{1}'", prec, name);

        TerminalDescr td;
        OpDescrTriplet triplet;

        if (terminalTable.Find (name, out td)) 
        
        { // some operator symbols (:-, -->, +, ...) already exist prior to their op/3 definition
          if (td.Payload == null)
          {
            td.Payload = opTable.Add (prec, type, name, user);
            td.IVal = Operator;
          }
          else
            ((OpDescrTriplet)td.Payload).Assign (name, prec, assoc, user);

          triplet = (OpDescrTriplet)td.Payload;
        }
        else // new operator
        {
          triplet = opTable.Add (prec, type, name, user);
          terminalTable.Add (name, Operator, triplet);
        }

        return triplet [assoc];
      }


      public void RemovePrologOperator (string type, string name, bool user)
      {
        AssocType assoc = AssocType.None;

        try
        {
          assoc = (AssocType)Enum.Parse (typeof (AssocType), type);
        }
        catch
        {
          IO.Error ("Illegal operator type '{0}'", type);
        }

        TerminalDescr td;

        if (terminalTable.Find (name, out td) && td.Payload != null)
        {
          if (!user)
            IO.Error ("Undefine of operator ({0}, {1}) not allowed", type, name);
          else
          {
            ((OpDescrTriplet)td.Payload).Unassign (name, assoc);

            foreach (OperatorDescr od in ((OpDescrTriplet)td.Payload))
              if (od.IsDefined) return;

            terminalTable.Remove (name); // name no longer used
          }
        }
        else // unknown operator
          IO.Error ("Operator not found: ({0}, {1})", type, name);
      }


      public bool IsOperator (string key)
      {
        return (terminalTable [key] == Operator);
      }


      public OpDescrTriplet GetOperatorDescr (string key)
      {
        TerminalDescr td;

        if (terminalTable.Find (key, out td)) return (OpDescrTriplet)td.Payload;

        return null;
      }


      void AddReservedOperators ()
      {
        AddPrologOperator (1200, "xfx", IMPLIES, false);
        AddPrologOperator (1200, "fx", IMPLIES, false);
        AddPrologOperator (1200, "xfx", DCGIMPL, false);
        AddPrologOperator (1150, "xfy", ARROW, false);
        CommaOpDescr = AddPrologOperator (1050, "xfy", COMMA, false);
        opTable.Find (COMMA, out CommaOpTriplet);
        SemiOpDescr = AddPrologOperator (1100, "xfy", SEMI, false);
      }


      bool isReservedOperatorSetting;

      void SetReservedOperators (bool asOpr)
      {
        if (asOpr) // parsed as Operator
        {
          terminalTable [IMPLIES] = Operator;
          terminalTable [DCGIMPL] = Operator;
        }
        else // parsed 'normally'
        {
          terminalTable [IMPLIES] = ImpliesSym;
          terminalTable [DCGIMPL] = DCGArrowSym;
          terminalTable [OP] = OpSym;
        }

        isReservedOperatorSetting = asOpr;
      }

      bool SetCommaAsSeparator (bool mode)
      {
        bool result = (terminalTable [COMMA] == Comma); // returnvalue is *current* status

        terminalTable [COMMA] = mode ? Comma : Operator; // 'Comma' is separator

        return result;
      }
      #endregion Operator handling

      #region Wrapper handling
      public void AddBracketPair (string openBracket, string closeBracket, bool useAsList)
      {
        if (openBracket == closeBracket)
          IO.Error ("Wrapper open and wrapper close token must be different");

        if (useAsList)
        {
          engine.AltListTable.Add (ref openBracket, ref closeBracket); // possible quotes are removed
          terminalTable.AddOrReplace (AltListOpen, "AltListOpen", openBracket);
          terminalTable.AddOrReplace (AltListClose, "AltListClose", closeBracket);
        }
        else
        {
          engine.WrapTable.Add (ref openBracket, ref closeBracket);
          terminalTable.AddOrReplace (WrapOpen, "WrapOpen", openBracket);
          terminalTable.AddOrReplace (WrapClose, "WrapClose", closeBracket);
        }
      }


      // specific method for interpreting '{' and '}' as list delimiters
      public void SetJsonMode (bool mode)
      {
        if (jsonMode = mode)
          AddBracketPair ("{", "}", true);
        else
        {
          terminalTable.AddOrReplace (LCuBracket, "LCuBracket", "{");
          terminalTable.AddOrReplace (RCuBracket, "RCuBracket", "}");
          engine.AltListTable.Remove ("{");
        }
      }

      public bool GetJsonMode ()
      {
        return jsonMode;
      }
      #endregion Wrapper handling

      #region ScanVerbatimString
      protected void ScanVerbatimString ()
      {
        do
        {
          if (ch == DQUOTE)
          {
            NextCh ();

            if (ch != DQUOTE)
            {
              symbol.Terminal = VerbatimStringLiteral;

              break;
            }
          }

          NextCh ();
        } while (!endText);

        symbol.Start++;
        symbol.Final = streamInPtr.Position;

        if (symbol.Terminal != VerbatimStringLiteral)
          SyntaxError = string.Format ("Unterminated verbatim string: {0}\r\n(remember to use \"\" instead of \\\" for an embedded \")",
            symbol.ToString ());
      }
      #endregion

      #region ScanIdOrTerminal
      protected override void ScanIdOrTerminal ()
      {
        TerminalDescr tRec;
        bool special = ch.IsSpecialAtomChar ();
        bool firstLow = Char.IsLower (ch);
        StreamPointer iPtr = streamInPtr;
        StreamPointer tPtr = iPtr;
        int aLen = (ch.IsIdStartChar () || special) ? 1 : 0; // length of longest Atom sofar
        int tLen = 0; // length of longest Terminal sofar
        int fCnt = 0; // count of calls to FindCharAndSubtree
        bool isDot = (ch == '.'); // remains valid only if symbol length is 1
        terminalTable.FindCharInSubtreeReset ();

        while (fCnt++ >= 0 && terminalTable.FindCharInSubtree (ch, out tRec))
        {
          if (tRec != null)
          {
            symbol.Terminal = tRec.IVal;
            symbol.Payload = tRec.Payload;
            symbol.Type = tRec.Type;
            tLen = fCnt;
            tPtr = streamInPtr; // next char to be processed

            if (symbol.Terminal == CommentStart || symbol.Terminal == CommentSingle) return;

            if (terminalTable.AtLeaf) break; // terminal cannot be extended
          }

          NextCh ();

          if (aLen == fCnt &&
               (special && ch.IsSpecialAtomChar () ||
                 !special && ch.IsIdAtomContinueChar (extraUnquotedAtomChar)
               )
             )
          {
            aLen++;
            iPtr = streamInPtr;
          }
        } // fCnt++ by last (i.e. failing) Call

        // At this point: (in the Prolog case, read 'Identifier and Atom made up
        // from specials' for 'Identifier'):
        // - tLen has length of BaseTrie terminal (if any, 0 otherwise);
        // - aLen has length of Identifier (if any, 0 otherwise);
        // - fCnt is the number of characters inspected (for Id AND terminal)
        // - The character pointer is on the last character inspected (for both)
        // Iff aLen = fCnt then the entire sequence read sofar is an Identifier.
        // Now try extending the Identifier, only meaningful if iLen = fCnt.
        // Do not do this for an Atom made up from specials if a Terminal was recognized !!

        if (aLen == fCnt)
        {
          while (true)
          {
            NextCh ();

            if (special ? ch.IsSpecialAtomChar () : ch.IsIdAtomContinueChar (extraUnquotedAtomChar))
            {
              aLen++;
              iPtr = streamInPtr;
            }
            else
              break;
          }
        }

        if (aLen > tLen) // tLen = 0 iff Terminal == Undefined
        {
          if (firstLow || special)
            symbol.Terminal = Atom;
          else
            symbol.Terminal = Identifier;

          symbol.HasIdFormat = true;
          InitCh (iPtr);
        }
        else if (symbol.Terminal == Undefined)
          InitCh (iPtr);
        else // we have a terminal != Identifier
        {
          symbol.HasIdFormat = (aLen == tLen);
          InitCh (tPtr);
        }

        NextCh ();

        // Dot-patch: a '.' is a Dot only if it is followed by layout,
        // otherwise it is an atom (or an operator if defined as such)
        if (isDot && aLen == 1 && (ch == '\0' || ch == '%' || Char.IsWhiteSpace (ch)))
          symbol.Terminal = Dot;

      }
      #endregion

      #region ScanQuotedAtom
      void ScanQuotedAtom (out bool canUnquote)
      {
        canUnquote = true;
        bool specialsOnly = true;
        bool first = true;

        do
        {
          NextCh ();

          if (!streamInPtr.EndLine)
          {
            if (ch == '\'') // possibly an escape
            {
              NextCh ();

              if (streamInPtr.EndLine || ch != '\'') // done
              {
                specialsOnly &= !first;
                canUnquote &= !first; // '' is an atom !!!
                symbol.Terminal = Atom;
              }
              else // atom contains quote
                canUnquote = false;
            }
            else
            {
              if (first)
                canUnquote = Char.IsLower (ch);
              else
                canUnquote = canUnquote && (ch == '_' || Char.IsLetterOrDigit (ch));

              specialsOnly = specialsOnly && (ch.IsSpecialAtomChar ());
            }

            first = false;
          }
        } while (!(streamInPtr.EndLine) && symbol.Terminal != Atom);

        if (streamInPtr.EndLine && symbol.Terminal != StringLiteral)
          SyntaxError = "Unterminated atom: " + symbol.ToString ();

        canUnquote |= specialsOnly;

        // check whether the atom is an operator:
        int start = symbol.Start + (canUnquote ? 1 : 0);
        int stop = streamInPtr.Position - (canUnquote ? 1 : 0);
        TerminalDescr tRec;

        if (terminalTable.Find (StreamInClip (start, stop), out tRec))
          if (tRec.Payload != null)
          {
            symbol.Terminal = Operator;
            symbol.Payload = tRec.Payload;
          }
      }
      #endregion ScanQuotedAtom

      #region NextSymbol
      protected override void NextSymbol (string _Proc)
      {
        if (symbol.AbsSeqNo != 0 && streamInPtr.FOnLine) streamInPtr.FOnLine = false;

        symbol.PrevFinal = symbol.Final;

        if (symbol.Terminal == EndOfInput)
          SyntaxError = "*** Trying to read beyond end of input";

        prevTerminal = symbol.Terminal;
        symbol.HasIdFormat = false;
        symbol.Payload = null;
        bool Break = false;

        do
        {
          while (Char.IsWhiteSpace (ch)) NextCh ();

          symbol.Start = streamInPtr.Position;
          symbol.LineNo = streamInPtr.LineNo;
          symbol.LineStart = streamInPtr.LineStart;
          symbol.Terminal = Undefined;
          bool canUnquote = false; // used for quoted atoms only

          if (endText)
            symbol.Terminal = EndOfInput;
          else if (streamInPtr.EndLine)
            symbol.Terminal = EndOfLine;
          else if (Char.IsDigit (ch))
            ScanNumber ();
          else if (ch == SQUOTE)
            ScanQuotedAtom (out canUnquote);
          else if (ch == DQUOTE)
            ScanString ();
          else
            ScanIdOrTerminal ();

          symbol.Final = streamInPtr.Position;
          symbol.IsFollowedByLayoutChar = (ch == '%' || Char.IsWhiteSpace (ch)); // '/*' not covered

          if (canUnquote)
          {
            symbol.Start++;
            symbol.Final--;
          }

          if (symbol.Terminal == EndOfLine)
          {
            eoLineCount++;
            NextCh ();
            Break = seeEndOfLine;
          }
          else
          {
            eoLineCount = 0;

            switch (symbol.Terminal)
            {
              case ppDefine:
                CheckPpIllegalSymbol ();
                ppDefineSymbol = true;
                break;
              case ppUndefine:
                CheckPpIllegalSymbol ();
                ppUndefineSymbol = true;
                break;
              case ppIf:
              case ppIfDef:
                CheckPpIllegalSymbol ();
                ppDoIfSymbol = true;
                ppElseOK.Push (true); // block is open
                break;
              case VerbatimStringStart:
                ScanVerbatimString ();
                Break = true;
                break;
              case ppIfNot:
              case ppIfNDef:
                CheckPpIllegalSymbol ();
                ppDoIfNotSymbol = true;
                ppElseOK.Push (true); // block is open
                break;
              case ppElse:
                CheckPpIllegalSymbol ();

                if (!(bool)ppElseOK.Pop ()) Error ("Unexpected #else");

                ppElseOK.Push (false); // no else allowed after an else
                ppXeqStack.Pop (); // remove the current value of ppProcessSource (pushed by the if-branch)

                // if the if-branch was executed, then this branch should not
                if (ppProcessSource) // ... it was executed
                  ppProcessSource = !ppProcessSource;
                else // ... it was not. But execute this branch only if the outer scope value of ppProcessSource is true
                  if ((bool)ppXeqStack.Peek ()) ppProcessSource = true;

                ppXeqStack.Push (ppProcessSource); // push the new value for this scope
                break;
              case ppEndIf:
                if (ppElseOK.Count == 0) Error ("Unexpected #endif");
                ppElseOK.Pop (); // go to outer scope
                ppXeqStack.Pop ();
                ppProcessSource = (bool)ppXeqStack.Peek ();
                break;
              case Identifier:
              case Atom:
                if (ppProcessSource && ppDefineSymbol)
                {
                  ppSymbols [symbol.ToString ().ToLower ()] = true; // any non-null value will do
                  ppDefineSymbol = false;
                }
                else if (ppProcessSource && ppUndefineSymbol)
                {
                  ppSymbols.Remove (symbol.ToString ().ToLower ());
                  ppUndefineSymbol = false;
                }
                else if (ppDoIfSymbol) // identifier following #if
                {
                  // do not alter ppProcessSource here if the outer scope value of ppProcessSource is false
                  if (ppProcessSource && (bool)ppXeqStack.Peek ()) // ... value is true
                    if (ppSymbols [symbol.ToString ().ToLower ()] == null)
                      ppProcessSource = false; // set to false if symbol is not defined

                  ppXeqStack.Push (ppProcessSource);
                  ppDoIfSymbol = false;
                }
                else if (ppDoIfNotSymbol) // identifier following #ifnot
                {
                  // do not alter ppProcessSource here if the outer scope value of ppProcessSource is false
                  if (ppProcessSource && (bool)ppXeqStack.Peek ()) // ... value is true
                    if (ppSymbols [symbol.ToString ().ToLower ()] != null)
                      ppProcessSource = false; // set to false if symbol is defined

                  ppXeqStack.Push (ppProcessSource);
                  ppDoIfNotSymbol = false;
                }
                else
                  Break = true; // 'regular' identifier
                break;
              case EndOfInput:
                Break = true;
                ppProcessSource = true; // force while-loop termination
                break;
              case CommentStart:
                if (stringMode)
                  Break = true;

                if (!DoComment ("*/", true, streamInPtr.FOnLine))
                  ErrorMessage = "Unterminated comment starting at line " + symbol.LineNo.ToString ();

                break;
              case CommentSingle:
                if (stringMode) Break = true; else Break = false;
                DoComment ("\n", false, streamInPtr.FOnLine);
                eoLineCount = 1;

                if (seeEndOfLine)
                {
                  symbol.Terminal = EndOfLine;
                  Break = true;
                }

                break;
              default:
                if (seeEndOfLine && symbol.Terminal != EndOfLine) streamInPtr.FOnLine = false;

                Break = true;
                break;
            }
          }
        } while (!Break || !ppProcessSource);

        symbol.AbsSeqNo++;
        symbol.RelSeqNo++;
#if showToken // a Console.Clear () will wipe out this output!
        IO.WriteLine ("NextSymbol[{0}] line {1}: '{2}' [{3}]",
                           symbol.AbsSeqNo, symbol.LineNo, symbol.ToString (), symbol.ToName ());
#endif
      }
      #endregion NextSymbol

      #region ParseTerm
      public BaseTerm ParseTerm ()
      {
        if (symbol.Terminal == EndOfInput) return null;

        BaseTerm result;

        try
        {
          OptionalPrologTerm (new TerminalSet (terminalCount), out result);
          lineCount = LineNo;

          return result;
        }
        catch (UnhandledParserException)
        {
          throw;
        }
        catch (ParserException e)
        {
          throw new Exception (e.Message);
        }
        catch (SyntaxException e)
        {
          throw new Exception (e.Message);
        }
        catch (Exception e) // other errors
        {
          errorMessage = String.Format ("*** Line {0}: {1}{2}", LineNo, e.Message,
                                        showErrTrace ? Environment.NewLine + e.StackTrace : null);

          throw new Exception (errorMessage);
        }
      }
      #endregion ParseTerm
    }
    #endregion PrologParser
  }

  #region Extensions class
  static class PrologParserExtensions
  {
    public static bool IsSpecialAtomChar (this char c)
    {
      return (@"+-*/\^<=>`~:.?@#$&".IndexOf (c) != -1);
    }

    public static bool IsIdAtomContinueChar (this char c, char extraUnquotedAtomChar)
    {
      return (Char.IsLetterOrDigit (c) || c == '_' || c == extraUnquotedAtomChar);
    }
  }
  #endregion Extensions class
}

