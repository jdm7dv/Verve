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
    #region JsonParser
    public partial class JsonParser : BaseParser<object>
    {
      BaseTerm jsonListTerm;
      public BaseTerm JsonListTerm { get { return jsonListTerm; } }
      OperatorTable opTable;
      public OperatorTable OpTable { set { opTable = value; } }

      #region ScanNumber
      protected override void ScanNumber () // overridden: also scans hex numbers
      {
        bool isReal;
        bool hexFound = false;
        StreamPointer savPosition;
        bool ok = true;

        do 
        { 
          NextCh ();

          if (!(ok = Char.IsDigit (ch)))
          {
            if (ok = ch.IsHexChar ()) hexFound = true;
          }
        }
        while (ok);

        symbol.Terminal = IntLiteral;
        isReal = true; // assumed until proven conversily

        if (!hexFound)
        {
          if (ch == '.') // fractional part?
          {
            // save dot position
            savPosition = streamInPtr;
            NextCh ();

            if (Char.IsDigit (ch))
            {
              symbol.Terminal = RealLiteral;

              do { NextCh (); } while (Char.IsDigit (ch));
            }
            else // not a digit after period
            {
              InitCh (savPosition); // 'unread' dot
              isReal = false;       // ... and remember this
            }
          }

          if (isReal) // integer or real, possibly with scale factor
          {
            savPosition = streamInPtr;

            if (ch == 'e' || ch == 'E')
            { // scale factor
              NextCh ();

              if (ch == '+' || ch == '-') NextCh ();

              if (Char.IsDigit (ch))
              {
                do
                {
                  NextCh ();
                }
                while (Char.IsDigit (ch));

                symbol.Terminal = RealLiteral;
              }
              else if (!stringMode) // Error in real syntax
                InitCh (savPosition);
            }
          }
        }

        symbol.Final = streamInPtr.Position;
      }


      protected override bool ScanFraction ()
      {
        StreamPointer savPosition = streamInPtr; // Position of '.'

        do NextCh (); while (Char.IsDigit (ch));

        bool result = (streamInPtr.Position > savPosition.Position + 1); // a fraction

        if (result)
        {
          if (ch == 'i')
          {
            symbol.Terminal = ImagLiteral;
            NextCh ();
          }
          else
            symbol.Terminal = RealLiteral;
        }
        else
          InitCh (savPosition);

        return result;
      }
      #endregion ScanNumber

      #region ScanIdOrTerminal
      protected override void ScanIdOrTerminal ()
      {
        TerminalDescr tRec;
        bool firstLow = Char.IsLower (ch);
        StreamPointer iPtr = streamInPtr;
        StreamPointer tPtr = iPtr;
        int aLen = (ch.IsIdStartChar ()) ? 1 : 0; // length of longest Identifier sofar
        int tLen = 0; // length of longest Terminal sofar
        int fCnt = 0; // count of calls to FindCharAndSubtree
        bool isDot = (ch == '.'); // remains valid only if symbol length is 1
        terminalTable.FindCharInSubtreeReset ();

        while (fCnt++ >= 0 && terminalTable.FindCharInSubtree (ch, out tRec))
        {
          if (tRec != null)
          {
            symbol.Terminal = tRec.IVal;
            symbol.Payload = tRec.Payload; // not used
            symbol.Type = tRec.Type;
            tLen = fCnt;
            tPtr = streamInPtr; // next char to be processed

            if (symbol.Terminal == CommentStart || symbol.Terminal == CommentSingle) return;

            if (terminalTable.AtLeaf) break; // terminal cannot be extended
          }

          NextCh ();

          if (aLen == fCnt && (Char.IsLetterOrDigit (ch) || ch == '_'))
          {
            aLen++;
            iPtr = streamInPtr;
          }
        } // fCnt++ by last (i.e. failing) Call

        /*
                At this point: (in the Prolog case, read 'Identifier and Atom made up
                from specials' for 'Identifier'):
                - tLen has length of BaseTrie terminal (if any, 0 otherwise);
                - aLen has length of Identifier (if any, 0 otherwise);
                - fCnt is the number of characters inspected (for Id AND terminal)
                - The character pointer is on the last character inspected (for both)
                Iff aLen = fCnt then the entire sequence read sofar is an Identifier.
                Now try extending the Identifier, only meaningful if iLen = fCnt.
                Do not do this for an Atom made up from specials if a Terminal was recognized !!
        */

        if (aLen == fCnt)
        {
          while (true)
          {
            NextCh ();

            if (Char.IsLetterOrDigit (ch) || ch == '_')
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
      }
      #endregion

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

          if (endText)
            symbol.Terminal = EndOfInput;
          else if (streamInPtr.EndLine)
            symbol.Terminal = EndOfLine;
          else if (Char.IsDigit (ch))
            ScanNumber ();
          else if (ch == DQUOTE)
            ScanString ();
          else
            ScanIdOrTerminal ();

          symbol.Final = streamInPtr.Position;

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
    }
    #endregion JsonParser
  }

  #region Extensions class
  static class JsonParserExtensions
  {
    public static bool IsHexChar (this char c)
    {
      return (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
    }
  }
  #endregion Extensions class
}

