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
    |  C#Prolog -- Copyright (C) 2007-2014 John Pool -- j.pool@ision.nl                                  |
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

        symbol.TerminalId = IntLiteral;
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
              symbol.TerminalId = RealLiteral;

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

                symbol.TerminalId = RealLiteral;
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
            symbol.TerminalId = ImagLiteral;
            NextCh ();
          }
          else
            symbol.TerminalId = RealLiteral;
        }
        else
          InitCh (savPosition);

        return result;
      }
      #endregion ScanNumber

      #region ScanIdOrTerminal
      protected override void ScanIdOrTerminalOrCommentStart ()
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
            symbol.TerminalId = tRec.IVal;
            symbol.Payload = tRec.Payload; // not used
            symbol.Class = tRec.Class;
            tLen = fCnt;
            tPtr = streamInPtr; // next char to be processed

            if (symbol.TerminalId == CommentStart || symbol.TerminalId == CommentSingle) return;

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
          symbol.TerminalId = Identifier;
          symbol.Class = SymbolClass.Id;
          InitCh (iPtr);
        }
        else if (symbol.TerminalId == Undefined)
          InitCh (iPtr);
        else // we have a terminal != Identifier
        {
          if (aLen == tLen) symbol.Class = SymbolClass.Id;
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

        if (symbol.TerminalId == EndOfInput)
          SyntaxError = "*** Trying to read beyond end of input";

        prevTerminal = symbol.TerminalId;
        symbol.Class = SymbolClass.None;
        symbol.Payload = null;
        bool Break = false;

        do
        {
          while (Char.IsWhiteSpace (ch)) NextCh ();

          symbol.Start = streamInPtr.Position;
          symbol.LineNo = streamInPtr.LineNo;
          symbol.LineStart = streamInPtr.LineStart;
          symbol.TerminalId = Undefined;

          if (endText)
            symbol.TerminalId = EndOfInput;
          else if (streamInPtr.EndLine)
            symbol.TerminalId = EndOfLine;
          else if (Char.IsDigit (ch))
            ScanNumber ();
          else if (ch == DQUOTE)
            ScanString ();
          else
            ScanIdOrTerminalOrCommentStart ();

          symbol.Final = streamInPtr.Position;

          if (symbol.TerminalId == EndOfLine)
          {
            eoLineCount++;
            NextCh ();
            Break = seeEndOfLine;
          }
          else
          {
            eoLineCount = 0;

            switch (symbol.TerminalId)
            {
              case Identifier:
                Break = true;
                break;
              case EndOfInput:
                Break = true;
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
                  symbol.TerminalId = EndOfLine;
                  Break = true;
                }

                break;
              default:
                if (seeEndOfLine && symbol.TerminalId != EndOfLine) streamInPtr.FOnLine = false;

                Break = true;
                break;
            }
          }
        } while (!Break);

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

