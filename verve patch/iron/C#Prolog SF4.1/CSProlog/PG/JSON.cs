//#define LL1_tracing
namespace Prolog
{
  using System;
  using System.IO;
  using System.Text;
  using System.Xml;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Globalization;
  using System.Threading;
  using System.Diagnostics;
  using System.Security.Principal;

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

// Parser Generator version 4.0 -- Date/Time: 10-Feb-14 08:40:11


  public partial class PrologEngine
  {
    #region JsonParser
    public partial class JsonParser : BaseParser<object>
    {
      public static readonly string VersionTimeStamp = "2014-02-10 08:40:11";
      
           
      #region Terminal definition
      
      /* The following constants are defined in BaseParser.cs:
      const int Undefined = 0;
      const int Comma = 1;
      const int LeftParen = 2;
      const int RightParen = 3;
      const int Identifier = 4;
      const int IntLiteral = 5;
      const int ppDefine = 6;
      const int ppUndefine = 7;
      const int ppIf = 8;
      const int ppIfNot = 9;
      const int ppElse = 10;
      const int ppElseIf = 11;
      const int ppEndIf = 12;
      const int RealLiteral = 13;
      const int ImagLiteral = 14;
      const int StringLiteral = 15;
      const int CharLiteral = 16;
      const int CommentStart = 17;
      const int CommentSingle = 18;
      const int EndOfLine = 19;
      const int ANYSYM = 20;
      const int EndOfInput = 21;
      */
      const int LSqBracket = 22;
      const int RSqBracket = 23;
      const int LCuBracket = 24;
      const int RCuBracket = 25;
      const int Colon = 26;
      const int TrueSym = 27;
      const int FalseSym = 28;
      const int NullSym = 29;
      // Total number of terminals:
      public const int terminalCount = 30;
      
      public static void FillTerminalTable (BaseTrie terminalTable)
      {
        terminalTable.Add (Undefined, SymbolClass.None, "Undefined");
        terminalTable.Add (Comma, SymbolClass.None, "Comma", ",");
        terminalTable.Add (LeftParen, SymbolClass.Group, "LeftParen", "(");
        terminalTable.Add (RightParen, SymbolClass.Group, "RightParen", ")");
        terminalTable.Add (Identifier, SymbolClass.Id, "Identifier");
        terminalTable.Add (IntLiteral, SymbolClass.Number, "IntLiteral");
        terminalTable.Add (ppDefine, SymbolClass.Meta, "ppDefine", "#define");
        terminalTable.Add (ppUndefine, SymbolClass.Meta, "ppUndefine", "#undefine");
        terminalTable.Add (ppIf, SymbolClass.Meta, "ppIf", "#if");
        terminalTable.Add (ppIfNot, SymbolClass.Meta, "ppIfNot", "#ifnot");
        terminalTable.Add (ppElse, SymbolClass.Meta, "ppElse", "#else");
        terminalTable.Add (ppElseIf, SymbolClass.Meta, "ppElseIf", "#elseif");
        terminalTable.Add (ppEndIf, SymbolClass.Meta, "ppEndIf", "#endif");
        terminalTable.Add (RealLiteral, SymbolClass.Number, "RealLiteral");
        terminalTable.Add (ImagLiteral, SymbolClass.Number, "ImagLiteral");
        terminalTable.Add (StringLiteral, SymbolClass.Text, "StringLiteral");
        terminalTable.Add (CharLiteral, SymbolClass.Text, "CharLiteral");
        terminalTable.Add (CommentStart, SymbolClass.Comment, "CommentStart", "/*");
        terminalTable.Add (CommentSingle, SymbolClass.Comment, "CommentSingle", "%");
        terminalTable.Add (EndOfLine, SymbolClass.None, "EndOfLine");
        terminalTable.Add (ANYSYM, SymbolClass.None, "ANYSYM");
        terminalTable.Add (EndOfInput, SymbolClass.None, "EndOfInput");
        terminalTable.Add (LSqBracket, SymbolClass.None, "LSqBracket", "[");
        terminalTable.Add (RSqBracket, SymbolClass.None, "RSqBracket", "]");
        terminalTable.Add (LCuBracket, SymbolClass.None, "LCuBracket", "{");
        terminalTable.Add (RCuBracket, SymbolClass.None, "RCuBracket", "}");
        terminalTable.Add (Colon, SymbolClass.None, "Colon", ":");
        terminalTable.Add (TrueSym, SymbolClass.None, "TrueSym", "true");
        terminalTable.Add (FalseSym, SymbolClass.None, "FalseSym", "false");
        terminalTable.Add (NullSym, SymbolClass.None, "NullSym", "null");
      }
      
      #endregion Terminal definition

      #region Constructor
      public JsonParser ()
      {
        terminalTable = new BaseTrie (terminalCount, false);
        FillTerminalTable (terminalTable);
        symbol = new Symbol (this);
        streamInPrefix = "";
        streamInPreLen = 0;
      }
      #endregion constructor

      #region NextSymbol, GetSymbol
      protected override bool GetSymbol (TerminalSet followers, bool done, bool genXCPN)
      {
        string s;

        if (symbol.IsProcessed) NextSymbol ();

        symbol.SetProcessed (done);
        if (parseAnyText || followers.IsEmpty ()) return true;

        if (syntaxErrorStat) return false;

        if (symbol.TerminalId == ANYSYM || followers.Contains (symbol.TerminalId)) return true;

        switch (symbol.TerminalId)
        {
          case EndOfLine:
            if (seeEndOfLine) s = "<EndOfLine>"; else goto default;
            s = "<EndOfLine>";
            break;
          case EndOfInput:
            s = "<EndOfInput>";
            break;
          default:
            s = String.Format ("\"{0}\"", symbol.ToString ());
            break;
        }

        s = String.Format ("*** Unexpected symbol: {0}{1}*** Expected one of: {2}", s,
                           Environment.NewLine, terminalTable.TerminalImageSet (followers));
        if (genXCPN)
          SyntaxError = s;
        else
          errorMessage = s;

        return true;
      }
      #endregion NextSymbol, GetSymbol

      #region PARSER PROCEDURES
      public override void RootCall ()
      {
        JsonStruct (new TerminalSet (terminalCount, EndOfInput));
      }


      public override void Delegates ()
      {
        
      }

      
      #region JsonStruct
      private void JsonStruct (TerminalSet _TS)
      {
        GetSymbol (new TerminalSet (terminalCount, LSqBracket, LCuBracket), false, true);
        if (symbol.TerminalId == LCuBracket)
        {
          JsonObject (_TS, out jsonListTerm);
        }
        else
        {
          JsonArray (_TS, out jsonListTerm);
        }
      }
      #endregion
      
      #region JsonObject
      private void JsonObject (TerminalSet _TS, out BaseTerm t)
      {
        BaseTerm e;
        List<BaseTerm> listItems = new List<BaseTerm> ();
        GetSymbol (new TerminalSet (terminalCount, LCuBracket), true, true);
        GetSymbol (new TerminalSet (terminalCount, StringLiteral, RCuBracket), false, true);
        if (symbol.TerminalId == StringLiteral)
        {
          while (true)
          {
            JsonPair (new TerminalSet (terminalCount, Comma, RCuBracket), out e);
            listItems.Add (e);
            GetSymbol (new TerminalSet (terminalCount, Comma, RCuBracket), false, true);
            if (symbol.TerminalId == Comma)
            {
              symbol.SetProcessed ();
            }
            else
              break;
          }
        }
        GetSymbol (new TerminalSet (terminalCount, RCuBracket), true, true);
        t = JsonTerm.FromArray (listItems.ToArray ());
      }
      #endregion
      
      #region JsonPair
      private void JsonPair (TerminalSet _TS, out BaseTerm t)
      {
        BaseTerm t0, t1;
        GetSymbol (new TerminalSet (terminalCount, StringLiteral), true, true);
        t0 = new StringTerm (symbol.ToString ().Dequoted ());
        GetSymbol (new TerminalSet (terminalCount, Colon), true, true);
        JsonValue (_TS, out t1);
        t = new OperatorTerm (opTable, PrologParser.COLON, t0, t1);
      }
      #endregion
      
      #region JsonArray
      private void JsonArray (TerminalSet _TS, out BaseTerm t)
      {
        BaseTerm e;
        List<BaseTerm> listItems = new List<BaseTerm> ();
        GetSymbol (new TerminalSet (terminalCount, LSqBracket), true, true);
        GetSymbol (new TerminalSet (terminalCount, IntLiteral, RealLiteral, StringLiteral, LSqBracket, RSqBracket, LCuBracket,
                                                   TrueSym, FalseSym, NullSym), false, true);
        if (symbol.IsMemberOf (IntLiteral, RealLiteral, StringLiteral, LSqBracket, LCuBracket, TrueSym, FalseSym, NullSym))
        {
          while (true)
          {
            JsonValue (new TerminalSet (terminalCount, Comma, RSqBracket), out e);
            listItems.Add (e);
            GetSymbol (new TerminalSet (terminalCount, Comma, RSqBracket), false, true);
            if (symbol.TerminalId == Comma)
            {
              symbol.SetProcessed ();
            }
            else
              break;
          }
        }
        GetSymbol (new TerminalSet (terminalCount, RSqBracket), true, true);
        t = new CompoundTerm ("array", ListTerm.ListFromArray (listItems.ToArray (), BaseTerm.EMPTYLIST));
      }
      #endregion
      
      #region JsonValue
      private void JsonValue (TerminalSet _TS, out BaseTerm t)
      {
        GetSymbol (new TerminalSet (terminalCount, IntLiteral, RealLiteral, StringLiteral, LSqBracket, LCuBracket, TrueSym,
                                                   FalseSym, NullSym), false, true);
        if (symbol.TerminalId == LCuBracket)
        {
          JsonObject (_TS, out t);
        }
        else if (symbol.TerminalId == LSqBracket)
        {
          JsonArray (_TS, out t);
        }
        else
        {
          JsonLiteral (_TS, out t);
        }
      }
      #endregion
      
      #region JsonLiteral
      private void JsonLiteral (TerminalSet _TS, out BaseTerm t)
      {
        GetSymbol (new TerminalSet (terminalCount, IntLiteral, RealLiteral, StringLiteral, TrueSym, FalseSym, NullSym), false,
                   true);
        if (symbol.IsMemberOf (IntLiteral, RealLiteral))
        {
          GetSymbol (new TerminalSet (terminalCount, IntLiteral, RealLiteral), false, true);
          if (symbol.TerminalId == IntLiteral)
          {
            symbol.SetProcessed ();
          }
          else
          {
            symbol.SetProcessed ();
          }
          t = new DecimalTerm (symbol.ToDecimal ());
        }
        else if (symbol.TerminalId == StringLiteral)
        {
          symbol.SetProcessed ();
          t = new StringTerm (symbol.ToString ().Dequoted ());
        }
        else if (symbol.TerminalId == TrueSym)
        {
          symbol.SetProcessed ();
          t = new AtomTerm ("true");
        }
        else if (symbol.TerminalId == FalseSym)
        {
          symbol.SetProcessed ();
          t = new AtomTerm ("false");
        }
        else
        {
          symbol.SetProcessed ();
          t = new AtomTerm ("null");
        }
      }
      #endregion
      
      
      #endregion PARSER PROCEDURES
    }
    #endregion JsonParser
  }
}
