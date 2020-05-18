#define showToken

namespace Prolog
{
  using System;
  using System.IO;
  using System.Text;
  using System.Xml;
  using System.Collections;
  using System.Collections.Generic;
  using System.Security.Principal;
  using System.Diagnostics;
  using System.Globalization;

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

  // PrologParser Generator version 4.0 -- Date/Time: 22-12-2010 8:42:54

  public partial class PrologEngine
  {
    public static readonly CultureInfo CIC = CultureInfo.InvariantCulture;
    
    #region Exceptions
    class ParserException : ApplicationException
    {
      public ParserException (string msg) : base (msg) { }

      public ParserException (string msg, params object [] args) :
        base (string.Format (msg, args)) { }
    }

    class SyntaxException : ApplicationException
    {
      public SyntaxException (string msg) : base (msg) { }

      public SyntaxException (string msg, params object [] args) :
        base (string.Format (msg, args)) { }
    }

    class UserException : ApplicationException
    {
      string @class;
      public string Class { get { return @class; } }

      public UserException (string @class, string msg) :
        base (string.Format ("(user-thrown {0}exception:)\r\n{1}",
         (@class == null ? null : string.Format ("({0}) ", @class)), msg))
      {
        this.@class = @class;
      }


      public UserException (string @class, string msg, params object [] args) :
        base (string.Format ("(user-thrown {0}exception:)\r\n{1}",
          (@class == null ? null : string.Format ("({0}) ", @class)), string.Format (msg, args)))
      {
        this.@class = @class;
      }
    }

    class FatalException : ApplicationException
    {
      public FatalException (string msg) : base (msg) { }
    }

    class UnhandledParserException : ApplicationException
    {
      public UnhandledParserException (string msg) : base (msg) { }
    }
    #endregion Exceptions

    #region TerminalSet
    public class TerminalSet
    {
      bool [] x;

      public TerminalSet (int terminalCount, params int [] ta)
      {
        x = new bool [terminalCount];

        if (terminalCount == 22)
        {
          terminalCount = 22;
        }

        for (int i = 0; i < terminalCount; i++) x [i] = false;

        foreach (int terminal in ta) x [terminal] = true;
      }

      public TerminalSet (int terminalCount, bool [] y)
      {
        x = new bool [terminalCount];

        if (terminalCount == 22)
        {
          terminalCount = 22;
        }

        for (int i = 0; i < terminalCount; i++) x [i] = y [i];
      }


      public TerminalSet Union (int terminalCount, params int [] ta)
      {
        TerminalSet union = new TerminalSet (terminalCount, x);  // create an identical set

        foreach (int terminal in ta) union [terminal] = true; // ... and unify

        return union;
      }


      public bool this [int i]
      {
        set { x [i] = value; }
      }


      public bool Contains (int terminal)
      {
        return x [terminal];
      }


      public bool IsEmpty ()
      {
        for (int i = 0; i < x.Length; i++) if (x [i]) return false;

        return true;
      }


      public void ToIntArray (out int [] a)
      {
        int count = 0;

        for (int i = 0; i < x.Length; i++) if (x [i]) count++;

        a = new int [count];
        count = 0;

        for (int i = 0; i < x.Length; i++) if (x [i]) a [count++] = i;
      }
    }
    #endregion TerminalSet

    #region BaseParser
    public class BaseParser<T>
    {
      public BaseParser ()
      {
        cdh = new ConditionalDefinitionHandler (ppChar);
      }

      #region ConditionalDefinitionHandler
      protected class ConditionalDefinitionHandler
      {
        // An ifdef..endif statement is made up by one or more more blocks separated by else(if)s.
        // At most one of these blocks wil actually be processed; the others contain 'dead' code.
        enum IfStatStatus
        {
          Pristine, // No block in the ifdef..endif statement has been processed yet.
          Active,   // The current block is being processed.
          Done      // A previous block in the ifdef..endif statement has already been processed.
        }
        const int NONE = -1;
        List<string> definedSymbols;
        Stack<IfStatStatus> nestedBlockStatus; // for keeping track of conditional definitions nesting
        bool isExpectingId;
        IfStatStatus ifStatStatus;
        bool codeIsActive { get { return (ifStatStatus == IfStatStatus.Active); } }
        bool codeIsInactive { get { return (ifStatStatus != IfStatStatus.Active); } }
        int prevTerminalId;
        int prevSymLineNo;
        char ppChar;
        public bool IsExpectingId { get { return isExpectingId; } }
        public bool CodeIsInactive { get { return codeIsInactive; } }

        public ConditionalDefinitionHandler (char ppChar)
        {
          this.ppChar = ppChar;
          definedSymbols = new List<string> ();
          Initialize ();
        }


        public void Initialize ()
        {
          nestedBlockStatus = new Stack<IfStatStatus> ();
          ifStatStatus = IfStatStatus.Active; // default for outermost scope
          isExpectingId = false;
          prevTerminalId = NONE;
          nestedBlockStatus.Push (ifStatStatus);
        }


        public void HandleSymbol (Symbol symbol)
        {
          if (symbol.TerminalId == Undefined)
            IO.Error ("Unknown conditional definition symbol: {0}", symbol);

          if (isExpectingId)
          {
            if (symbol.Class == SymbolClass.Id && symbol.LineNo == prevSymLineNo)
            {
              symbol.Class = SymbolClass.Meta; // in order to get rid of it in NextSymbol loop
              string s = symbol.ToString ();

              if (prevTerminalId == ppDefine && codeIsActive)
                Define (s);
              else if (prevTerminalId == ppUndefine && codeIsActive)
                Undefine (s);
              else if (prevTerminalId == ppIf)
              {
                if (codeIsInactive)
                  EnterScope (ifStatStatus, symbol); // do nothing; keep non-active status
                else if (IsDefined (s)) //
                  EnterScope (IfStatStatus.Active, symbol);
                else
                  EnterScope (IfStatStatus.Pristine, symbol); // wait for potential else(if) block
              }
              else if (prevTerminalId == ppIfNot)
              {
                if (codeIsInactive)
                  EnterScope (ifStatStatus, symbol); // do nothing; keep non-active status
                else if (IsDefined (s)) //
                  EnterScope (IfStatStatus.Pristine, symbol); // wait for potential else(if) block
                else
                  EnterScope (IfStatStatus.Active, symbol);
              }
              else if (prevTerminalId == ppElseIf)
              {
                if (codeIsActive)
                {
                  ExitScope (symbol);
                  EnterScope (IfStatStatus.Done, symbol);
                }
                else if (ifStatStatus == IfStatStatus.Pristine && IsDefined (s))
                {
                  ExitScope (symbol);
                  EnterScope (IfStatStatus.Active, symbol);
                }
              } //else leave status untouched
            }
            else
              IO.Error (
                "Identifier missing after {0}if, {0}ifdef {0}ifndef, {0}elseif, {0}define or {0}undefine",
                ppChar);

            isExpectingId = false;
          }
          else if (symbol.TerminalId == EndOfInput && nestedBlockStatus.Count > 1)
            IO.Error ("Unexpected end of input -- {0}else or {0}endif expected", ppChar);
          else
          {
            switch (symbol.TerminalId)
            {
              case ppDefine:
              case ppUndefine:
              case ppIf:
              case ppIfNot:
                isExpectingId = true;
                break;
              case ppElseIf:
                if (prevTerminalId == ppIf || prevTerminalId == ppEndIf || prevTerminalId == ppElseIf)
                  isExpectingId = true;
                else
                  IO.Error ("Unexpected {0}elseif-directive", ppChar);
                break;
              case ppElse:
                if (prevTerminalId == ppIf || prevTerminalId == ppEndIf || prevTerminalId == ppElseIf)
                {
                  if (codeIsActive) // code is active ...
                  {
                    ExitScope (symbol);
                    EnterScope (IfStatStatus.Done, symbol); // ... deactivate it
                  }
                  else if (ifStatStatus == IfStatStatus.Pristine) // code is inactive ...
                  {
                    ExitScope (symbol);         // ... activate it (sets codeIsInactive !!)
                    EnterScope (IfStatStatus.Active, symbol); // ...
                  }
                }
                else
                  IO.Error ("Unexpected {0}else-directive", ppChar);
                break;
              case ppEndIf:
                ExitScope (symbol);
                break;
            }

            prevTerminalId = symbol.TerminalId;
            prevSymLineNo = symbol.LineNo;
          }
        }


        void Define (string sym)
        {
          if (!definedSymbols.Contains (sym))
            definedSymbols.Add (sym);
        }


        void Undefine (string sym)
        {
          if (definedSymbols.Contains (sym))
            definedSymbols.Remove (sym);
        }


        bool IsDefined (string sym)
        {
          return definedSymbols.Contains (sym);
        }


        bool AtOuterScope
        {
          get { return (nestedBlockStatus.Count == 1); }
        }


        void EnterScope (IfStatStatus mode, Symbol symbol)
        {
          nestedBlockStatus.Push (ifStatStatus = mode);
        }


        void ExitScope (Symbol symbol)
        {
          if (nestedBlockStatus.Count == 1)
            IO.Error ("Unexpected {0}-symbol", symbol);

          nestedBlockStatus.Pop ();
          ifStatStatus = nestedBlockStatus.Peek ();
        }
      }
      #endregion ConditionalDefinitionHandler

      #region Fields and properties
      int traceIndentLevel;
      // The const values below must be in strict accordance with the
      // order in procedure EnterPredefinedSymbols in uCommons.pas
      protected const int Undefined = 0;
      protected const int Comma = 1;
      protected const int LeftParen = 2;
      protected const int RightParen = 3;
      protected const int Identifier = 4;
      protected const int IntLiteral = 5;
      protected const int ppDefine = 6;
      protected const int ppUndefine = 7;
      protected const int ppIf = 8;
      protected const int ppIfNot = 9;
      protected const int ppElse = 10;
      protected const int ppElseIf = 11;
      protected const int ppEndIf = 12;
      protected const int RealLiteral = 13;
      protected const int ImagLiteral = 14;
      protected const int StringLiteral = 15;
      protected const int CharLiteral = 16;
      protected const int CommentStart = 17;
      protected const int CommentSingle = 18;
      protected const int EndOfLine = 19;
      protected const int ANYSYM = 20;
      protected const int EndOfInput = 21;
      // end of Terminal symbol const values
      protected virtual char ppChar { get { return '#'; } }

      protected const char SQUOTE = '\'';
      protected const char DQUOTE = '"';
      protected const char BSLASH = '\\';
      protected const int UNDEF = -1;

      protected BaseTrie terminalTable;
      protected ConditionalDefinitionHandler cdh;
      protected char ch;
      protected char prevCh;
      protected bool endText;
      protected Symbol symbol;
      protected int maxRuntime = 0; // maximum runtime in miliseconds; 0 means unlimited
      protected int actRuntime;     // actual runtime for Parse () in miliseconds
      protected Buffer inStream = null;
      protected Buffer outStream = null; // standard output
      protected string streamInPrefix;
      protected bool syntaxErrorStat;
      protected string errorMessage;
      protected int streamInLen;
      protected StreamPointer streamInPtr;
      protected bool stringMode; // iff a string is being read (i.e. ignore comments)
      protected bool parseAnyText;
      protected int prevTerminal;
      protected int anyTextStart;
      protected int anyTextFinal;
      protected int anyTextFPlus;
      protected int clipBegin;
      protected int clipEnd;
      protected bool seeEndOfLine;
      protected bool SeeEndOfLine { set { seeEndOfLine = value; } get { return seeEndOfLine; } }
      protected bool formatMode;
      /*
          Preprocessor symbols are retained in nested parser calls, i.e. the symbols
          defined in the outer Call are visible in the inner Call. At the end of the
          inner parse, the state is reset to the one at the start of the inner parse.
      */
      protected int eoLineCount;
      protected int lineCount = 0;
      protected bool showErrTrace = true;
      protected int streamInPreLen; // length of streamInPrefix
      protected positionMarker errMark;

      public int ClipBegin { get { return clipBegin; } }
      public int ClipEnd { get { return clipEnd; } }
      public bool FormatMode { get { return formatMode; } set { formatMode = value; } }
      public int LineCount { get { return lineCount; } }
      public bool ShowErrTrace { get { return showErrTrace; } set { showErrTrace = value; } }
      public int MaxRuntime { get { return maxRuntime; } set { maxRuntime = value; } }
      public int ActRuntime { get { return actRuntime; } }

      public string Prefix
      {
        get
        {
          return streamInPrefix;
        }
        set
        {
          streamInPrefix = value;
          streamInPreLen = streamInPrefix.Length;
        }
      }


      public string SyntaxError
      {
        set
        {
          if (parseAnyText) return;

          errorMessage = symbol.Context + value + Environment.NewLine;
          syntaxErrorStat = true;

          throw new SyntaxException (errorMessage);
        }
        get
        {
          return errorMessage;
        }
      }


      public string ErrorMessage
      {
        set
        {
          if (parseAnyText) return;

          throw new SyntaxException ("{0}{1}{2}", symbol.Context, value, Environment.NewLine);
        }
        get
        {
          return errorMessage;
        }
      }


      public void Warning (string msg)
      {
        if (parseAnyText) return;

        positionMarker currPos;

        InputStreamMark (out currPos);

        if (errMark.Start != UNDEF) InputStreamRedo (errMark, 0);

        IO.WriteLine ("{0}{1}{2}", symbol.Context, msg, Environment.NewLine);

        if (errMark.Start != UNDEF) InputStreamRedo (currPos, 0);
      }


      public void Warning (string msg, params Object [] o)
      {
        Warning (String.Format (msg, o));
      }


      public void SemanticError (string msg)
      {
        if (parseAnyText) return;

        if (errMark.Start != UNDEF) InputStreamRedo (errMark, 0);

        throw new Exception (String.Format ("{0}{1}{2}", symbol.Context, msg, Environment.NewLine));
      }


      public void SemanticError (string msg, params Object [] o)
      {
        SemanticError (String.Format (msg, o));
      }


      protected void Error (string msg)
      {
        if (parseAnyText) return;

        if (errMark.Start != UNDEF) InputStreamRedo (errMark, 0);

        throw new Exception (String.Format ("{0}{1}{2}", symbol.Context, msg, Environment.NewLine));
      }


      protected void Error (string msg, params Object [] o)
      {
        Error (String.Format (msg, o));
      }


      public void Fatal (string msg)
      {
        if (parseAnyText) return;

        if (errMark.Start != UNDEF) InputStreamRedo (errMark, 0);

        throw new FatalException (String.Format ("{0}{1}{2}", symbol.Context, msg, Environment.NewLine));
      }


      public void Fatal (string msg, params Object [] o)
      {
        Fatal (String.Format (msg, o));
      }


      public string StreamIn
      {
        get
        {
          return inStream.ToString ();
        }
        set
        {
          inStream = new StringReadBuffer (value);
          streamInLen = inStream.Length;
          Parse ();
        }
      }


      public void LoadFromFile (string fileName)
      {
        try
        {
          inStream = new FileReadBuffer (fileName);
          streamInLen = inStream.Length;
        }
        catch
        {
          Prefix = "";
          throw new Exception ("*** Unable to read file \"" + fileName + "\"");
        }

        Parse ();
      }


      public void SetInputStream (TextReader tr)
      {
        inStream = new StringReadBuffer (((StreamReader)tr).ReadToEnd ());
        streamInLen = inStream.Length;
      }


      public string StreamInClip (int n, int m)
      {
        return GetStreamInString (n, m);
      }

      public string StreamInClip ()
      {
        return GetStreamInString (clipBegin, clipEnd);
      }

      public bool AtEndOfInput
      {
        get { return (inStream == null); }
      }
      #endregion // Public fields and properties

      #region Structs and classes
      #region CharSet
      class CharSet
      {
        bool [] b = new bool [255];

        public CharSet (params byte [] a)
        {
          int i;

          for (i = 0; i < 255; b [i++] = false) ;

          for (i = 0; i < a.Length; b [a [i++]] = true) ;
        }


        public bool Contains (char c)
        {
          return b [(byte)c];
        }


        public void Add (string s)
        {
          foreach (char c in s.ToCharArray ()) b [(byte)c] = true;
        }


        public void Remove (string s)
        {
          foreach (char c in s.ToCharArray ()) b [(byte)c] = false;
        }
      }
      #endregion

      #region StreamPointer
      protected struct StreamPointer
      {
        public int Position;
        public int LineNo;
        public int LineStart;
        public bool EndLine;
        public bool FOnLine;  /// if the coming symbol is the first one on the current line
      }
      #endregion

      #region positionMarker
      protected struct positionMarker
      {
        public T Payload;
        public StreamPointer Pointer;
        public int Terminal;
        public SymbolClass Class;
        public int Start;
        public int Final;
        public int FinalPlus;
        public int PrevFinal;
        public int LineNo;
        public int AbsSeqNo;
        public int RelSeqNo;
        public int LineStart;
        public bool Processed;
        public bool IsSet; // initialized to false
      }
      #endregion
      #endregion Structs and classes

      #region Symbol
      public enum SymbolClass { None, Id, Text, Number, Meta, Group, Comment }

      protected class Symbol
      {
        BaseParser<T> parser;
        bool processed;
        public T Payload;
        public SymbolClass Class;
        public int TerminalId;
        public int Start;     // position in input stream
        public int Final;     // first position after symbol
        public int FinalPlus; // first position of next symbol
        public int PrevFinal; // final position of previous symbol
        public int LineNo;
        public int LineStart; // position of first char of line in input stream
        public int AbsSeqNo;  // sequence number of symbol // -- absolute value, invariant under MARK/REDO
        public int RelSeqNo;  // sequence number of symbol in current line // -- relative value, invariant under MARK/REDO
        public bool IsFollowedByLayoutChar; // true iff the symbol is followed by a layout character

        public Symbol (BaseParser<T> p)
        {
          parser = p;
          processed = false;
          Payload = default (T);
          Class = SymbolClass.None;
          TerminalId = PrologParser.Undefined;
          Start = UNDEF;
          Final = UNDEF;
          FinalPlus = UNDEF;
          PrevFinal = UNDEF;
          LineNo = UNDEF;
          LineStart = UNDEF;
          AbsSeqNo = UNDEF;
          RelSeqNo = UNDEF;
        }

        public bool IsNumber { get { return true; } }

        public override string ToString ()
        {
          if (this.Start == Final) return "";

          if (this.TerminalId == Undefined && Start > Final) return "<Undefined>";

          if (this.TerminalId == EndOfLine) return "<EndOfLine>";

          if (this.TerminalId == EndOfInput) return "<EndOfInput>";

          return parser.StreamInClip (Start, Final);
        }


        public int ColNo { get { return (Start >= LineStart) ? Start - LineStart + 1 : UNDEF; } }


        public string ToName ()
        {
          return parser.terminalTable.Name (TerminalId);
        }


        public string ToUnquoted ()
        {
          string s = this.ToString ();
          int len = s.Length;
          char c;

          if (len < 2) return s;

          if ((c = s [0]) == SQUOTE && s [len - 1] == SQUOTE ||
              (c = s [0]) == DQUOTE && s [len - 1] == DQUOTE)
            return s.Substring (1, len - 2).Replace (c.ToString () + c.ToString (), c.ToString ());
          else
            return s;
        }


        public int ToInt ()
        {
          try
          {
            return Convert.ToInt32 (ToString ());
          }
          catch
          {
            throw new ParserException ("*** Unable to convert '{0}' to an integer value", this);
          }
        }


        public int ToShort ()
        {
          try
          {
            return Convert.ToInt16 (ToString ());
          }
          catch
          {
            throw new ParserException ("*** Unable to convert '{0}' to a short value", this);
          }
        }


        public double ToDouble ()
        {
          try
          {
            return Double.Parse (ToString (), CIC);
          }
          catch
          {
            throw new ParserException ("*** Unable to convert '{0}' to a real (double) value", this);
          }
        }


        public decimal ToDecimal ()
        {
          try
          {
            return Decimal.Parse (ToString (), NumberStyles.Float, CIC);
          }
          catch
          {
            throw new ParserException ("*** Unable to convert '{0}' to a decimal value", this);
          }
        }


        public decimal ToImaginary ()
        {
          string imag = ToString ();

          try
          {
            return Decimal.Parse (imag.Substring (0, imag.Length - 1), NumberStyles.Float, CIC);
          }
          catch
          {
            throw new ParserException ("*** Unable to convert '{0}' to an imaginary value", imag);
          }
        }


        public string Dump ()
        {
          return String.Format ("Start={0} Final={1} LineStart={2}", Start, Final, LineStart);
        }


        public string Context
        {
          get
          {
            StringBuilder sb = new StringBuilder (String.Format ("{0}*** {1}: line {2}", Environment.NewLine,
                                                                 parser.inStream.Name, LineNo));

            if (this.Start >= this.Final) return sb.ToString () + Environment.NewLine;

            if (this.Start >= this.LineStart)
              sb.Append (" position " + ColNo.ToString ());

            if (parser.inStream.Name != "")
              sb.Append (Environment.NewLine + InputLine + Environment.NewLine);

            return sb.ToString ();
          }
        }


        public bool IsMemberOf (params int [] ts)
        {
          int lo, hi, mid;
          int n = ts.Length;

          if (n <= 8) // linear
          {
            for (lo = 0; lo < n; lo++) if (TerminalId <= ts [lo]) break;

            return (lo >= n) ? false : ts [lo] == TerminalId;
          }
          else // binary
          {
            lo = -1;
            hi = n;

            while (hi != lo + 1)
            {
              mid = (lo + hi) >> 1;

              if (TerminalId < ts [mid])
                hi = mid;
              else
                lo = mid;
            }

            return (lo < 0) ? false : ts [lo] == TerminalId;
          }
        }


        public bool IsProcessed
        {
          get { return processed; }
        }


        public void SetProcessed (bool status)
        {
          processed = status;
        }


        public void SetProcessed ()
        {
          SetProcessed (true);
        }


        public string TextPlus ()
        {
          if (TerminalId == Undefined && this.Start > this.FinalPlus)
            return "<Undefined>";
          else
            return parser.StreamInClip (this.Start, this.FinalPlus);
        }


        public string IntroText ()
        {
          if (TerminalId == Undefined && this.Start > this.FinalPlus)
            return "<Undefined>";
          else
            return parser.StreamInClip (this.PrevFinal, this.Start);
        }


        public string InputLine
        {
          get
          {
            char c;
            int i = this.LineStart;
            // find end of line
            while (i < parser.streamInLen)
            {
              parser.GetStreamInChar (i, out c);
              if (c == '\n') break;
              i++;
            }
            return parser.StreamInClip (LineStart, i);
          }
        }
      }
      #endregion

      #region TerminalDescr // Terminal descriptor
      public class TerminalDescr : IComparable
      {
        T payload;
        int iVal;
        string image;
        SymbolClass @class;

        public T Payload { get { return payload; } set { payload = value; } }
        public int IVal { get { return iVal; } set { iVal = value; } }
        public string Image { get { return image; } set { image = value; } }
        public SymbolClass Class { get { return @class; } set { @class = value; } }

        public TerminalDescr (int iVal, T payload, string image)
        {
          this.iVal = iVal;
          this.payload = payload;
          this.image = image;
          this.@class = 0;
        }

        public TerminalDescr (int iVal, T payload, string image, SymbolClass @class)
        {
          this.iVal = iVal;
          this.payload = payload;
          this.image = image;
          this.@class = @class;
        }

        public int CompareTo (object o)
        {
          return iVal.CompareTo ((int)o);
        }

        public override string ToString ()
        {
          if (payload == null)
            return String.Format ("{0} ({1})", iVal, image);
          else
            return String.Format ("{0}:{1} ({2}) [{3}]", iVal, payload.ToString (), image, @class);
        }
      }
      #endregion Payload

      #region CacheTrieNode
      public class TrieNode : IComparable
      {
        char keyChar;
        TerminalDescr termRec;
        ArrayList subTrie;

        public char KeyChar { get { return keyChar; } set { keyChar = value; } }
        public TerminalDescr TermRec { get { return termRec; } set { termRec = value; } }
        public ArrayList SubTrie { get { return subTrie; } set { subTrie = value; } }

        public TrieNode (char k, ArrayList s, TerminalDescr t)
        {
          keyChar = k;
          subTrie = s;
          termRec = t;
        }


        public int CompareTo (object o)
        {
          return keyChar.CompareTo ((char)o);
        }


        public override string ToString ()
        {
          StringBuilder sb = new StringBuilder ();

          //ToString (this, sb, 0);        // tree representation
          ToString (this, "", sb, 0);    // flat representation

          return sb.ToString ();
        }


        void ToString (TrieNode node, StringBuilder sb, int indent)
        {
          if (indent == 0)
            sb.Append ("<root>" + Environment.NewLine);
          else
          {
            sb.AppendFormat ("{0}{1} -- ", Spaces (indent), node.keyChar);

            if (node.termRec == null)
              sb.Append (Environment.NewLine);
            else
              sb.Append (String.Format (node.TermRec.ToString ()));
          }
          if (node.subTrie != null)
            foreach (TrieNode subTrie in node.subTrie) ToString (subTrie, sb, indent + 1);
        }


        public static void ToArrayList (TrieNode node, bool atRoot, ref ArrayList a)
        {
          if (!atRoot) // skip root
            if (node.termRec != null) a.Add (node.termRec.Payload);

          if (node.subTrie != null)
            foreach (TrieNode subTrie in node.subTrie) ToArrayList (subTrie, false, ref a);
        }


        void ToString (TrieNode node, string prefix, StringBuilder sb, int indent)
        {
          if (indent != 0) // skip root
            if (node.termRec != null) sb.AppendFormat ("{0} {1}\r\n", prefix, node.termRec.ToString ());

          if (node.subTrie != null)
            foreach (TrieNode subTrie in node.subTrie) ToString (subTrie, prefix, sb, indent + 1);
        }
      }
      #endregion CacheTrieNode

      #region BaseTrie
      public enum DupMode { dupIgnore, dupAccept, dupError };

      public class BaseTrie
      {
        public static readonly int UNDEF = -1;
        TrieNode root = new TrieNode ('\x0', null, null);
        int terminalCount;
        ArrayList indices = new ArrayList ();
        Hashtable names = new Hashtable ();
        ArrayList curr;
        DupMode dupMode = DupMode.dupError;
        bool caseSensitive = false; // every term to lowercase
        ArrayList currSub;
        public bool AtLeaf { get { return (currSub == null); } }

        public BaseTrie (int terminalCount, DupMode dm)
        {
          this.terminalCount = terminalCount;
          dupMode = dm;
        }


        public BaseTrie (int terminalCount, bool cs)
        {
          this.terminalCount = terminalCount;
          caseSensitive = cs;
        }


        public BaseTrie (int terminalCount, DupMode dm, bool cs)
        {
          this.terminalCount = terminalCount;
          dupMode = dm;
          caseSensitive = cs;
        }


        public void AddOrReplace (int iVal, string name, string image)
        {
          Remove (image);
          Add (iVal, name, image);
        }


        public void Add (int iVal, string name, params string [] images)
        {
          Add (iVal, 0, name, images);
        }


        public void Add (int iVal, SymbolClass @class, string name, params string [] images)
        {
          names [iVal] = name;

          foreach (string key in images) Add (key, iVal, default (T), @class);
        }


        public void Add (string key, int iVal, T payload)
        {
          Add (key, iVal, payload, 0);
        }


        public void Add (string key, int iVal, T payload, SymbolClass @class)
        {
          if (key == null || key == "")
            throw new Exception ("*** Trie.Add: Attempt to insert a null- or empty key");

          if (!caseSensitive) key = key.ToLower ();

          if (root.SubTrie == null) root.SubTrie = new ArrayList ();

          curr = root.SubTrie;
          int imax = key.Length - 1;
          TrieNode node;
          TrieNode next;
          int i = 0;

          while (i <= imax)
          {
            int k = (curr.Count == 0) ? -1 : curr.BinarySearch (key [i], null); // null req'd for MONO?

            if (k >= 0) // found
            {
              node = (TrieNode)curr [k];

              if (i == imax) // at end of key
              {
                if (node.TermRec == null)
                  AddToIndices (node.TermRec = new TerminalDescr (iVal, payload, key, @class));
                else if (dupMode == DupMode.dupAccept)
                {
                  TerminalDescr trec = node.TermRec;
                  trec.IVal = iVal;
                  trec.Payload = payload;
                  trec.Image = key;
                  trec.Class = @class;
                }
                else if (dupMode == DupMode.dupError)
                  throw new Exception (String.Format ("*** Attempt to insert duplicate key '{0}'", key));

                return;
              }

              if (node.SubTrie == null) node.SubTrie = new ArrayList ();
              curr = node.SubTrie;
              i++;
            }
            else // char not found => append chain of TrieNodes for rest of key
            {
              node = new TrieNode (key [i], null, null);
              curr.Insert (~k, node);
              while (true)
              {
                if (i == imax) // at end of key
                {
                  AddToIndices (node.TermRec = new TerminalDescr (iVal, payload, key, @class));

                  return;
                }
                node.SubTrie = new ArrayList ();
                node.SubTrie.Add (next = new TrieNode (key [++i], null, null));
                node = next;
              }
            }
          }
        }


        void AddToIndices (TerminalDescr td)
        {
          int k = indices.BinarySearch (td.IVal);
          indices.Insert ((k < 0) ? ~k : k, td);
        }


        public bool Find (string key, out TerminalDescr td)
        {
          if (key == null || key == "")
            throw new Exception ("*** Trie.Add: Attempt to search for a null- or empty key");

          int imax = key.Length - 1;
          int i = 0;
          int k;

          td = null;
          curr = root.SubTrie;

          while (curr != null)
          {
            if ((k = curr.BinarySearch (key [i])) < 0)
            {
              curr = null;

              return false;
            }

            TrieNode match = (TrieNode)curr [k];

            if (i++ == imax) return ((td = match.TermRec) != null);

            curr = match.SubTrie;
          }
          return false;
        }


        public int this [string key]
        {
          get
          {
            TerminalDescr td;

            return (Find (key, out td) ? td.IVal : UNDEF);
          }
          set
          {
            TerminalDescr td;

            if (Find (key, out td))
              td.IVal = value;
            else
              throw new Exception ("*** Trie indexer: key [" + key + "] not found");
          }
        }


        public void FindCharInSubtreeReset ()
        {
          currSub = root.SubTrie;
        }


        public bool FindCharInSubtree (char c, out TerminalDescr td)
        {
          int k;

          k = (currSub == null) ? -1 : k = currSub.BinarySearch (c);

          if (k >= 0)
          {
            TrieNode match = (TrieNode)currSub [k];
            td = match.TermRec;
            currSub = match.SubTrie;

            return true;
          }
          else
          {
            td = null;

            return false;
          }
        }


        public string Name (int i)
        {
          return (string)names [i];
        }


        public ArrayList TerminalsOf (int i)
        {
          int k = indices.BinarySearch (i);

          if (k < 0) return null;

          ArrayList result = new ArrayList ();
          int k0 = k;

          while (true)
          {
            result.Add (indices [k++]);
            if (k == indices.Count || ((TerminalDescr)indices [k]).CompareTo (i) != 0) break;
          }

          k = k0 - 1;

          while (k > 0 && ((TerminalDescr)indices [k]).CompareTo (i) == 0)
            result.Add (indices [k--]);

          return result;
        }


        public int IndexOf (string key)
        {
          TerminalDescr td;

          return Find (key, out td) ? td.IVal : -1;
        }


        public string TerminalImageSet (TerminalSet ts)
        {
          StringBuilder result = new StringBuilder ();
          bool isFirst = true;
          int [] ii;

          ts.ToIntArray (out ii);

          foreach (int i in ii)
          {
            ArrayList a = TerminalsOf (i);
            bool isImage = false;

            if (a != null)
              foreach (TerminalDescr td in a)
              {
                isImage = true;
                if (isFirst) isFirst = false; else result.Append (", ");
                result.Append (td.Image);
              }

            if (!isImage)
            {
              if (isFirst) isFirst = false; else result.Append (", ");
              result.Append ("<" + (string)names [i] + ">");
            }
          }
          return result.ToString ();
        }

        /*
                // public bool Remove (string key)

                Remove (keyChar)

                Stel je zit in een CacheTrieNode met KeyChar, Val, SubTrie. Huidige letter is keyChar[i]

                De volgende properties zijn gedefinieerd:

                • CMatch   = (KeyChar == keyChar[i])
                • IsTerm   = (CachedTerm != null)
                • IsLeaf   = (SubTrie == null)
                • IsKeyEnd = (i == imax)

                De vraag is nu bij welke combinaties van de bovenstaande properties een
                CacheTrieNode gedelete mag worden, dus dat een referentie ernaar in de CacheTrieNode
                hoger in de tree op null gezet mag worden.

                Het proces begint met het vinden van de keyChar(node). Als die er niet is,
                return met false.

                Vandaaruit (IsKeyEnd) terug naar de root (door terugkeer uit recursie).
                In een node wordt aangegeven of de parentnode hem mag deleten door in een
                node de outputparam mayDelete terug te geven aan de ancestor (= aanroeper).

                Een node mag weg als IsLeaf. Als de node de laatste is (IsEndKey) mag dit
                zonder meer, als hij niet de laatste is, mag het alleen wanneer hij geen
                terminal is (!IsTerm, d.w.z. een eindpunt van een kortere keyChar (subkey)).
        */

        public bool Remove (string key)
        {
          bool result;
          bool dummy;

          if (!caseSensitive) key = key.ToLower ();

          RemoveEx (root, key, 0, key.Length - 1, out result, out dummy);

          return result;
        }


        public void RemoveEx (TrieNode curr, string key, int i, int imax, out bool result, out bool mayDelete)
        {
          result = false;
          mayDelete = false;

          if (curr.SubTrie == null) return;

          int k = curr.SubTrie.BinarySearch (key [i]);

          if (k < 0) return;

          TrieNode match = (TrieNode)curr.SubTrie [k];

          if (i == imax) // last char of key -- now we work our way back to the root
          {
            if (match.TermRec != null)
            {
              indices.Remove (match.TermRec);
              match.TermRec = null;
              mayDelete = (curr.SubTrie == null);
              result = true;
            }
          }
          else
            RemoveEx (match, key, i + 1, imax, out result, out mayDelete);

          if (!result || !mayDelete) return;

          if (mayDelete) curr.SubTrie.RemoveAt (k);

          mayDelete = (curr.SubTrie == null && curr.TermRec != null);
        }


        public void Remove (int i) // remove all entries with index i
        {
          names.Remove (i);

          ArrayList a = TerminalsOf (i);

          if (a != null) foreach (TerminalDescr td in a) Remove (td.Image);
        }


        public void TrimToSize ()  // minimize memory overhead if no new elements will be added
        {
          TrimToSizeEx (root);
        }


        public void TrimToSizeEx (TrieNode n)
        {
          if (n.SubTrie != null)
          {
            n.SubTrie.TrimToSize ();
            foreach (TrieNode t in n.SubTrie) TrimToSizeEx (t);
          }
        }


        public ArrayList ToArrayList ()
        {
          ArrayList a = new ArrayList ();

          TrieNode.ToArrayList (root, true, ref a);

          return a;
        }


        public override string ToString ()
        {
          return root.ToString ();
        }
      }
      #endregion BaseTrie

      #region ScanNumber
      protected virtual void ScanNumber ()
      {
        bool isReal;
        StreamPointer savPosition;

        do { NextCh (); } while (Char.IsDigit (ch));

        symbol.TerminalId = IntLiteral;
        isReal = true; // assumed until proven conversily

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

        if (ch == 'i')
        {
          symbol.TerminalId = ImagLiteral;
          NextCh ();
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

              if (ch == 'i')
              {
                symbol.TerminalId = ImagLiteral;
                NextCh ();
              }
              else
                symbol.TerminalId = RealLiteral;
            }
            else if (!stringMode) // Error in real syntax
              InitCh (savPosition);
          }
        }

        symbol.Class = SymbolClass.Number;
        symbol.Final = streamInPtr.Position;
      }


      protected virtual bool ScanFraction ()
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
      #endregion

      #region ScanString
      protected virtual void ScanString ()
      {
        bool multiLine = ConfigSettings.NewlineInStringAllowed;

        do
        {
          NextCh ();

          if (!streamInPtr.EndLine)
          {
            if (ConfigSettings.ResolveEscapes)
            {
              if (ch == DQUOTE)
              {
                symbol.TerminalId = StringLiteral;
                NextCh ();
              }
              else if (ch == BSLASH)
                NextCh ();
            }
            else if (ch == DQUOTE)
            {
              NextCh ();

              if ((streamInPtr.EndLine && !multiLine) ||
                   ch != DQUOTE) symbol.TerminalId = StringLiteral;
            }
          }
        } while ((!streamInPtr.EndLine || multiLine) && !endText && symbol.TerminalId != StringLiteral);

        symbol.Class = SymbolClass.Text;
        symbol.Final = streamInPtr.Position;

        if (streamInPtr.EndLine && symbol.TerminalId != StringLiteral)
          SyntaxError = "Unterminated string: " + symbol.ToString ();
      }
      #endregion

      #region ScanIdOrTerminal
      protected virtual void ScanIdOrTerminalOrCommentStart ()
      {
        TerminalDescr tRec;
        StreamPointer iPtr = streamInPtr;
        StreamPointer tPtr = iPtr;
        int fCnt; // count of calls to FindCharAndSubtree
        int tLen; // length of longest Terminal sofar
        int iLen; // length of longest Identifier sofar

        iLen = (ch.IsIdStartChar ()) ? 1 : 0; // length of longest Identifier sofar }
        tLen = 0;
        fCnt = 0;
        terminalTable.FindCharInSubtreeReset ();

        while ((fCnt++ >= 0) && terminalTable.FindCharInSubtree (Char.ToLower (ch), out tRec))
        {
          if (tRec != null)
          {
            symbol.TerminalId = tRec.IVal;
            symbol.Payload = tRec.Payload;
            symbol.Class = tRec.Class;
            tLen = fCnt;
            tPtr = streamInPtr; // next char to be processed
            if (terminalTable.AtLeaf) break; // terminal cannot be extended
          }
          NextCh ();

          if (iLen == fCnt && ch.IsBaseIdChar ())
          {
            iLen++;
            iPtr = streamInPtr;
          }
        } // fCnt++ by last (i.e. failing) call
        /*
              At this point: (in the Prolog case, read 'Identifier and Atom made up
              from specials' for 'Identifier'):
              - tLen has length of Trie terminal (if any, 0 otherwise);
              - iLen has length of Identifier (if any, 0 otherwise);
              - fCnt is the number of characters inspected (for Id AND terminal)
              - The character pointer is on the last character inspected (for both)
              Iff iLen = fCnt then the entire sequence read sofar is an Identifier.
              Now try extending the Identifier, only meaningful if iLen = fCnt.
              Do not do this for an Atom made up from specials if a Terminal was recognized !!
        */
        if (iLen == fCnt)
        {
          while (true)
          {
            NextCh ();
            if (ch.IsBaseIdChar ())
            {
              iLen++;
              iPtr = streamInPtr;
            }
            else
              break;
          }
        }

        if (iLen > tLen) // tLen = 0 iff Terminal == Undefined
        {
          symbol.TerminalId = Identifier;
          symbol.Class = SymbolClass.Id;
          InitCh (iPtr);
        }
        else if (symbol.TerminalId == Undefined)
          InitCh (iPtr);
        else // we have a terminal != Identifier
        {
          if (iLen == tLen) symbol.Class = SymbolClass.Id;
          InitCh (tPtr);
        }

        NextCh ();
      }
      #endregion

      #region NextSymbol, GetSymbol
      protected virtual void NextSymbol ()
      {
        NextSymbol (null);
      }

      protected virtual void NextSymbol (string _Proc)
      {
        if (symbol.AbsSeqNo != 0 && streamInPtr.FOnLine) streamInPtr.FOnLine = false;

        symbol.PrevFinal = symbol.Final;

        if (symbol.TerminalId == EndOfInput)
          SyntaxError = "*** Trying to read beyond end of input";

        prevTerminal = symbol.TerminalId;
        symbol.Class = SymbolClass.None;
        symbol.Payload = default (T);
        bool Break = false;

        do
        {
          #region basic and conditional definition symbol handling
          do
          {
            while (Char.IsWhiteSpace (ch)) NextCh ();

            symbol.Start = streamInPtr.Position;
            symbol.LineNo = streamInPtr.LineNo;
            symbol.LineStart = streamInPtr.LineStart;
            symbol.TerminalId = Undefined;
            symbol.Class = SymbolClass.None;

            if (endText)
            {
              symbol.TerminalId = EndOfInput;
              cdh.HandleSymbol (symbol); // check for missing #endif missing
            }
            else if (streamInPtr.EndLine)
              symbol.TerminalId = EndOfLine;
            else if (Char.IsDigit (ch))
              ScanNumber ();
            else if (ch == DQUOTE)
              ScanString ();
            else if (ch == '.')
            {
              if (!ScanFraction ()) ScanIdOrTerminalOrCommentStart ();
            }
            else
              ScanIdOrTerminalOrCommentStart ();

            symbol.Final = symbol.FinalPlus = streamInPtr.Position;

            if (symbol.Class == SymbolClass.Comment) break;

            if (cdh.IsExpectingId || symbol.Class == SymbolClass.Meta)
              cdh.HandleSymbol (symbol); // if expecting: symbol must be an identifier

          } while (cdh.CodeIsInactive || symbol.Class == SymbolClass.Meta);
          #endregion

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
                if (seeEndOfLine && symbol.TerminalId != EndOfLine)
                  streamInPtr.FOnLine = false;

                Break = true;
                break;
            }
          }
        } while (!Break); // skip symbols while in preprocessing ignore mode

        symbol.AbsSeqNo++;
        symbol.RelSeqNo++;
#if showToken
        Console.WriteLine ("NextSymbol[{0}] line {1}: '{2}' [{3}]",
                       symbol.AbsSeqNo, symbol.LineNo, symbol.ToString (), symbol.ToName ());
#endif
      }

      protected virtual bool GetSymbol (TerminalSet followers, bool done, bool genXCPN)
      {
        throw new Exception ("GetSymbol must be overridden");
      }

      protected void EOL (TerminalSet followers, bool GenXCPN)
      {
        GetSymbol (new TerminalSet (EndOfLine), true, GenXCPN);

        while (symbol.TerminalId == EndOfLine)
        {
          NextSymbol (null);
        }
        symbol.SetProcessed (false);
      }

      protected void ANYTEXT (TerminalSet followers)
      {
        StreamPointer anyStart;
        StreamPointer follower;

        if (symbol.IsProcessed)
          anyTextStart = streamInPtr.Position;
        else
          anyTextStart = symbol.Start;

        if (followers.Contains (symbol.TerminalId) && !symbol.IsProcessed)
        {
          anyTextFinal = anyTextStart; // empty, nullstring
          anyTextFPlus = anyTextStart;

          return;
        }

        parseAnyText = true;
        anyStart = streamInPtr;

        do
        {
          // Follower will eventually be a symbol from Followers
          follower = streamInPtr;
          NextSymbol (null);
          symbol.FinalPlus = symbol.Start; // up to next symbol
          anyTextFPlus = symbol.Start;
        }

        while (symbol.TerminalId != EndOfInput && !followers.Contains (symbol.TerminalId));

        // "unread" last symbol
        InitCh (follower);
        // set AnyText "symbol" values
        symbol.Start = anyTextStart;
        symbol.LineNo = anyStart.LineNo;
        symbol.LineStart = anyStart.LineStart;
        symbol.Final = streamInPtr.Position;
        symbol.TerminalId = Undefined;
        symbol.SetProcessed (true);
        anyTextFinal = streamInPtr.Position;
        parseAnyText = false;
      }


      protected void InitPositionMarkers (positionMarker [] ma)
      {
        for (int i = 0; i < 4; i++) ma [i].Start = UNDEF;
      }


      protected void ReportParserProcEntry (string procName)
      {
        traceIndentLevel++;
        Console.WriteLine ("{0}=> {1} {2}", new string (' ', 2 * traceIndentLevel), procName, symbol.ToString ());
      }


      protected void ReportParserProcExit (string procName)
      {
        Console.WriteLine ("{0}<= {1} {2}", new string (' ', 2 * traceIndentLevel), procName, symbol.ToString ());
        traceIndentLevel--;
      }


      protected void InputStreamMark (out positionMarker m)
      {
        m.Pointer = streamInPtr;
        m.Terminal = symbol.TerminalId;
        m.Class = symbol.Class;
        m.Payload = symbol.Payload;
        m.Start = symbol.Start;
        m.Final = symbol.Final;
        m.FinalPlus = symbol.FinalPlus;
        m.PrevFinal = symbol.PrevFinal;
        m.LineNo = symbol.LineNo;
        m.AbsSeqNo = symbol.AbsSeqNo;
        m.RelSeqNo = symbol.RelSeqNo;
        m.LineStart = symbol.LineStart;
        m.Processed = symbol.IsProcessed;
        m.IsSet = true;
      }


      protected void InputStreamRedo (positionMarker m, int n)
      {
        if (!m.IsSet) throw new Exception ("REDO Error: positionMarker " + n.ToString () + " is not set");

        InitCh (m.Pointer);
        symbol.TerminalId = m.Terminal;
        symbol.Class = m.Class;
        symbol.Payload = m.Payload;
        symbol.Start = m.Start;
        symbol.Final = m.Final;
        symbol.FinalPlus = m.FinalPlus;
        symbol.PrevFinal = m.PrevFinal;
        symbol.LineNo = m.LineNo;
        symbol.AbsSeqNo = m.AbsSeqNo;
        symbol.RelSeqNo = m.RelSeqNo;
        symbol.LineStart = m.LineStart;
        symbol.SetProcessed (m.Processed);
      }
      #endregion NextSymbol, GetSymbol

      #region Parse
      public virtual void RootCall ()
      {
      }


      public virtual void Delegates ()
      {
      }

      public void InitParse ()
      {
        stringMode = false;
        parseAnyText = false;
        anyTextStart = 0;
        anyTextFinal = 0;
        anyTextFPlus = 0;
        seeEndOfLine = false;
        eoLineCount = 1;
        // For the very first symbol pretend that an EndOfLine preceded it (for formatting purposes)
        // This also means that leading blank lines will be formatted
        symbol.SetProcessed (true);
        symbol.AbsSeqNo = 0;
        symbol.RelSeqNo = 0;
        symbol.TerminalId = Undefined;
        streamInPtr.Position = UNDEF;
        streamInPtr.LineNo = 0;
        streamInPtr.LineStart = UNDEF;
        streamInPtr.EndLine = true;
        streamInPtr.FOnLine = true;
        InputStreamMark (out errMark); // just to make the compiler happy when there is no ERRMARK+
        errMark.Start = UNDEF;
        errorMessage = null;
        syntaxErrorStat = false;
        endText = false;
        prevCh = ' ';
        traceIndentLevel = -1;
        streamInLen += streamInPreLen;
        inStream.UpdateCache (0);
        cdh.Initialize ();
        NextCh ();
        Delegates ();
      }

      protected void Parse ()
      {
        WindowsPrincipal principal = new WindowsPrincipal (WindowsIdentity.GetCurrent ());
        bool isadmin = principal.IsInRole (WindowsBuiltInRole.Administrator);

        // TimeOfDay is less precise
        TimeSpan startProcTime = isadmin ?
          Process.GetCurrentProcess ().TotalProcessorTime : DateTime.Now.TimeOfDay;

        ParseEx ();

        actRuntime = ((isadmin ?
          Process.GetCurrentProcess ().TotalProcessorTime : DateTime.Now.TimeOfDay) -
          startProcTime).Milliseconds;
      }


      void ParseEx ()
      {
        InitParse ();

        try
        {
          RootCall ();
          lineCount = LineNo;

          if (symbol.IsProcessed) NextSymbol (null);

          if (symbol.TerminalId != EndOfInput)
            throw new Exception (String.Format ("Unexpected symbol {0} after end of input", symbol.ToString ()));
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
        finally
        {
          ExitParse ();
        }
      }


      public void ExitParse ()
      {
        if (inStream != null) inStream.Close ();

        inStream = null;
        Prefix = "";
        symbol.LineNo = UNDEF;
      }
      #endregion Parse

      #region Miscellaneous methods
      public void ClipStart ()
      {
        clipBegin = symbol.Start;
      }


      public void ClipStart (int clipBegin)
      {
        clipBegin = symbol.Start;
      }


      public void ClipFinal ()
      {
        clipEnd = symbol.Start;
      }


      public void ClipFinal (int clipBegin)
      {
        clipEnd = symbol.Start;
      }


      public void ClipFinalPlus () // includes current symbol
      {
        clipEnd = symbol.Final;
      }


      public void ClipFinalPlus (int clipEnd)
      {
        clipEnd = symbol.Final;
      }


      public void ClipFinalTrim ()  /// exclude any text (i.e. comment) after last symbol
      {
        clipEnd = symbol.PrevFinal;
      }


      public void ClipFinalTrim (int clipEnd)
      {
        clipEnd = symbol.PrevFinal;
      }


      public void GetStreamInChar (int n, out char c)
      {
        if (n < streamInPreLen)
          c = streamInPrefix [n];
        else
        {
          n -= streamInPreLen;
          c = inStream [n];
        }
      }


      string GetStreamInString (int n, int m)
      {
        string p;

        if (n < streamInPreLen) // start in Prefix
        {
          if (m < streamInPreLen) // end in Prefix
          {
            return streamInPrefix.Substring (n, m - n);
          }
          else // end beyond Prefix
          {
            p = streamInPrefix.Substring (n, streamInPreLen - n - 1); // part in Prefix
            // Overlap. Number of chars taken from prefix is streamInPreLen - n,
            // so decrease final position m accordingly. Set n to 0.
            n = 0;
            m -= streamInPreLen - n;
          }
        }
        else
        {
          n -= streamInPreLen;
          m -= streamInPreLen;
          p = "";
        }

        return p + inStream.Substring (n, m - n);
      }


      public int LineNo
      {
        get { return symbol.LineNo; }
      }


      public int ColNo
      {
        get { return symbol.ColNo; }
      }


      protected void NextCh ()
      {
        if (endText) return;

        if (streamInPtr.Position == streamInLen - 1)
        {
          endText = true;
          streamInPtr.EndLine = true;
          ch = '\0';
          streamInPtr.Position++;
        }
        else
        {
          if (streamInPtr.EndLine)
          {
            streamInPtr.LineNo++;
            streamInPtr.LineStart = streamInPtr.Position + 1;
            symbol.RelSeqNo = 0;
            streamInPtr.EndLine = false;
            streamInPtr.FOnLine = true;
          }

          prevCh = ch;
          streamInPtr.Position++;
          GetStreamInChar (streamInPtr.Position, out ch);
          streamInPtr.EndLine = (ch == '\n');
        }
      }


      protected void InitCh (StreamPointer c)
      {
        streamInPtr = c;
        GetStreamInChar (streamInPtr.Position, out ch);

        if (streamInPtr.Position <= 0)
          prevCh = ' ';
        else
          GetStreamInChar (streamInPtr.Position - 1, out prevCh);

        endText = (streamInPtr.Position > streamInLen - 1);

        if (endText) ch = '\x0';
      }


      public string ReadLine ()
      {
        if (endText) return null;

        int start = streamInPtr.Position;
        int final = start;

        while (!streamInPtr.EndLine) NextCh ();

        final = streamInPtr.Position;
        NextCh ();

        return StreamInClip (start, final).TrimEnd ('\r');
      }


      public int ReadChar ()
      {
        if (endText) return -1;

        int result = ch;

        NextCh ();

        return result;
      }


      protected bool SkipOverChars (string p)
      {
        if (p.Length == 0) return false;

        do
        {
          if (ch == p [0])
          {
            int i = 0;

            do
            {
              NextCh ();

              if (++i == p.Length) return true;

            } while (ch == p [i]);
          }
          else
            NextCh ();
        } while (!endText);

        return false;
      }

      protected bool DoComment (string p, bool multiLine, bool firstOnLine)
      {
        bool result = SkipOverChars (p);

        symbol.Final = streamInPtr.Position;

        return result;
      }
      #endregion Miscellaneous methods
    }
    #endregion BaseParser

    #region Buffer
    public class Buffer
    {
      Stack indentStack = new Stack ();
      protected char indentChar = '\u0020';
      protected int indentDelta = 2;
      protected string name;
      protected bool quietMode = false;
      protected bool firstSymbolOnLine = true;
      protected int positionOnLine = 0;
      protected int rightMargin = -1; // i.e. not set
      public int indentLength = 0;

      public int RightMargin { get { return rightMargin; } set { rightMargin = value; } }

      public virtual char this [int i]
      {
        get { return '\0'; }
      }


      public string Name
      {
        get { return name; }
      }


      public virtual string [] Lines
      {
        get { return null; }
      }


      public virtual string Substring (int n, int len)
      {
        return null; // gets overridden
      }


      public virtual int Length
      {
        get { return 0; } // gets overridden
      }


      public virtual void UpdateCache (int p)
      {
      }


      public void Indent ()
      {
        indentStack.Push (indentLength); // save current
        indentLength += indentDelta;
      }


      public void Undent ()
      {
        indentLength = (indentStack.Count == 0) ? 0 : (int)indentStack.Pop ();
      }


      public void Indent (int i)
      {
        indentStack.Push (indentLength); // save current
        indentLength = i;
      }


      public virtual void Write (string s, params object [] pa)
      {
      }


      public virtual void WriteLine (string s, params object [] pa)
      {
      }


      public virtual void WriteChar (char c)
      {
      }


      public virtual void NewLine ()
      {
      }


      public bool AtLeftMargin // at beginning of line
      {
        get
        {
          if (quietMode) return false;

          return (positionOnLine == 0);
        }
      }


      public virtual void Clear ()
      {
      }


      public virtual void SaveToFile (string fileName)
      {
      }


      public virtual void Close ()
      {
      }


      public bool QuietMode
      {
        get { return quietMode; }
        set { quietMode = value; }
      }


      public virtual bool FirstSymbolOnLine
      {
        get { return firstSymbolOnLine; }
      }


      public virtual int PositionOnLine
      {
        get { return positionOnLine; }
      }
    }

    #region StringBuffer
    public class StringBuffer : Buffer
    {
    }


    #region StringReadBuffer
    public class StringReadBuffer : StringBuffer
    {
      string buffer;

      public StringReadBuffer (string s)
      {
        buffer = s;
        name = "input string";
      }


      public override char this [int i]
      {
        get { return buffer [i]; }
      }


      public override int Length
      {
        get { return buffer.Length; }
      }


      public override string Substring (int n, int len)
      {
        try
        {
          return buffer.Substring (n, len);
        }
        catch
        {
          return null;
        }
      }
    }
    #endregion StringReadBuffer


    #region StringWriteBuffer
    public class StringWriteBuffer : StringBuffer
    {
      StringBuilder sb;

      public StringWriteBuffer (string s)
      {
        sb = new StringBuilder (s);
      }


      public StringWriteBuffer ()
      {
        sb = new StringBuilder ();
      }


      public override string [] Lines
      {
        get
        {
          return sb.ToString ().Split ('\r');
        }
      }


      public override void Clear ()
      {
        sb.Length = 0;
        firstSymbolOnLine = true;
        positionOnLine = 0;
      }


      public override void Write (string s, params object [] pa)
      {
        if (quietMode || s == "") return;

        if (s == null) s = "";

        if (firstSymbolOnLine)
        {
          sb.Append (new String (indentChar, indentLength));
          firstSymbolOnLine = false;
          positionOnLine = indentLength;
        }

        string xs = (pa.Length == 0) ? s : string.Format (s, pa); // expanded s

        if (rightMargin > 0 && positionOnLine + xs.Length > rightMargin)
        {
          sb.AppendFormat ("{0}{1}{2}", Environment.NewLine, new String (indentChar, indentLength), xs);
          positionOnLine = indentLength + xs.Length;
        }
        else
        {
          sb.Append (xs);
          positionOnLine += xs.Length;
        }
      }


      public override void WriteLine (string s, params object [] pa)
      {
        if (quietMode) return;

        if (s == null) s = "";

        if (firstSymbolOnLine) sb.Append (new String (indentChar, indentLength));

        string xs = (pa.Length == 0) ? s : string.Format (s, pa); // expanded s

        if (rightMargin > 0 && positionOnLine + xs.Length > rightMargin)
          sb.AppendFormat ("{0}{1}{0}{2}{0}", Environment.NewLine, new String (indentChar, indentLength), xs);
        else
          sb.AppendFormat ("{0}{1}", xs, Environment.NewLine);

        firstSymbolOnLine = true;
        positionOnLine = 0;
      }


      public override void WriteChar (char c)
      {
        if (quietMode) return;

        if (rightMargin > 0 && positionOnLine + 1 > rightMargin)
        {
          sb.AppendFormat ("{0}{1}{2}", Environment.NewLine, new String (indentChar, indentLength), c);
          positionOnLine = indentLength + 1;
        }
        else
        {
          sb.Append (c);
          positionOnLine++;
        }
      }


      public override void NewLine ()
      {
        if (quietMode) return;

        sb.Append (Environment.NewLine);
        firstSymbolOnLine = true;
        positionOnLine = 0;
      }


      public override char this [int i]
      {
        get { return sb [i]; }
      }


      public override string ToString ()
      {
        return sb.ToString ();
      }


      public override void SaveToFile (string fileName)
      {
        StreamWriter sw = new StreamWriter (fileName);
        sw.Write (sb.ToString ());
        sw.Close ();
      }


      public override int Length
      {
        get { return sb.Length; }
      }
    }
    #endregion StringWriteBuffer
    #endregion StringBuffer

    #region FileBuffer
    public class FileBuffer : Buffer
    {
      protected FileStream fs;
    }

    #region FileReadBuffer
    public class FileReadBuffer : FileBuffer
    {
      const int CACHESIZE = 256 * 1024;
      byte [] cache = new byte [CACHESIZE];
      int cacheOfs; // number of chars in fs before first char of cache
      int cacheLen; // cache length (normally CACHESIZE, less at eof)
      bool little_endian;
      StringBuilder sb;

      public FileReadBuffer (string fileName)
      {
        name = fileName;

        try
        {
          fs = new FileStream (fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
          sb = new StringBuilder ();
          cacheOfs = 0;
          cacheLen = 0;
        }
        catch
        {
          throw new ParserException (String.Format ("*** Could not open file '{0}' for reading", fileName));
        }

        if (fs.Length >= 2) // try to work out type of file (primitive approach)
        {
          fs.Read (cache, 0, 2);
          little_endian = (cache [0] == '\xFF' && cache [1] == '\xFE');
          fs.Position = 0; // rewind
        }
      }


      ~FileReadBuffer ()
      {
        if (fs != null) Close ();
      }


      public override void Close ()
      {
        fs.Close ();
      }


      public override void UpdateCache (int p)
      {
        int i;
        cacheOfs = CACHESIZE * (p / CACHESIZE);

        if (cacheOfs > fs.Length)
          throw new Exception (String.Format ("*** Attempt to read beyond end of FileReadBuffer '{0}'", name));
        else
          fs.Position = cacheOfs;

        cacheLen = fs.Read (cache, 0, CACHESIZE); // cacheLen is actual number of bytes read

        if (cacheLen < CACHESIZE)
        {
          for (i = cacheLen; i < CACHESIZE; cache [i++] = 32) ;
          //cacheLen += 2;
        }
      }


      public override char this [int i]
      {
        get
        {
          if (little_endian) i = 2 * i + 2;
          if ((i < cacheOfs) || (i >= cacheOfs + cacheLen)) UpdateCache (i);
          return (char)cache [i % CACHESIZE];  // no test on cacheLen
        }
      }


      public override string Substring (int n, int len)
      {
        sb.Length = 0;
        for (int i = n; i < n + len; i++) sb.Append (this [i]);

        return sb.ToString ();
      }


      public override int Length
      {
        get { return Convert.ToInt32 (((little_endian) ? (fs.Length / 2 - 1) : fs.Length)); }
      }
    }
    #endregion FileReadBuffer


    // FileWriteBuffer

    #region FileWriteBuffer
    public class FileWriteBuffer : FileBuffer
    {
      StreamWriter sw;
      bool isTemp;

      public FileWriteBuffer (string fileName)
      {
        name = fileName;

        try
        {
          fs = new FileStream (fileName, FileMode.Create, FileAccess.Write);
          sw = new StreamWriter (fs);
          isTemp = false;
        }
        catch
        {
          throw new ParserException (String.Format ("*** Could not create file '{0}'", fileName));
        }
      }


      public FileWriteBuffer ()
      {
        try
        {
          name = Path.GetTempFileName ();
          fs = new FileStream (name, FileMode.Create);
          sw = new StreamWriter (fs);
          isTemp = true;
        }
        catch
        {
          throw new Exception ("*** FileWriteBuffer constructor could not create temporary file");
        }
      }


      ~FileWriteBuffer ()
      {
        Close ();
      }


      public override void SaveToFile (string fileName)
      {
        FileStream f = new FileStream (fileName, FileMode.Create);
        byte [] b = new byte [fs.Length];
        fs.Read (b, 0, b.Length);
        f.Write (b, 0, b.Length);
        f.Close ();
      }


      public override string Substring (int n, int len)
      {
        byte [] b = new byte [len];
        fs.Position = n;
        fs.Read (b, 0, len);
        return new ASCIIEncoding ().GetString (b);
      }


      public override string ToString ()
      {
        byte [] b = new byte [fs.Length];
        fs.Read (b, 0, b.Length);
        ASCIIEncoding enc = new ASCIIEncoding ();
        return enc.GetString (b);
      }


      public override void Close ()
      {
        try { sw.Close (); }
        catch { return; }

        if (isTemp)
        {
          try
          {
            File.Delete (name);
          }
          catch
          {
            throw new Exception ("*** FileWriteBuffer Close() could not delete temporary file");
          }
        }
      }


      public void Append (FileWriteBuffer f)  // f is assumed open for reading
      {
        byte [] b = new byte [f.fs.Length];
        f.fs.Read (b, 0, b.Length);
        fs.Position = fs.Length;
        fs.Write (b, 0, b.Length);
        firstSymbolOnLine = false;
      }


      public new int Length
      {
        get { return Convert.ToInt32 (fs.Length); }
        set { fs.SetLength (value); }
      }


      public override void Clear ()
      {
        fs.SetLength (0);
        firstSymbolOnLine = true;
      }


      public override void Write (string s, params object [] pa)
      {
        if (quietMode || s == "") return;

        if (s == null) s = "";

        if (firstSymbolOnLine)
        {
          sw.Write (new String (indentChar, indentLength));
          firstSymbolOnLine = false;
          positionOnLine = indentLength;
        }

        string xs = (pa.Length == 0) ? s : string.Format (s, pa); // expanded s

        if (rightMargin > 0 && positionOnLine + xs.Length > rightMargin)
        {
          sw.Write ("{0}{1}{2}", Environment.NewLine, new String (indentChar, indentLength), xs);
          positionOnLine = indentLength + xs.Length;
        }
        else
        {
          sw.Write (xs);
          positionOnLine += xs.Length;
        }
      }


      public override void WriteLine (string s, params object [] pa)
      {
        if (quietMode) return;

        if (s == null) s = "";

        if (firstSymbolOnLine) sw.Write (new String (indentChar, indentLength));

        string xs = (pa.Length == 0) ? s : string.Format (s, pa); // expanded s

        if (rightMargin > 0 && positionOnLine + xs.Length > rightMargin)
          sw.WriteLine ("{0}{1}{0}{2}", Environment.NewLine, new String (indentChar, indentLength), xs);
        else
          sw.WriteLine (xs, pa);

        firstSymbolOnLine = true;
        positionOnLine = 0;
      }


      public override void WriteChar (char c)
      {
        if (quietMode) return;

        if (rightMargin > 0 && positionOnLine + 1 > rightMargin)
        {
          sw.Write ("{0}{1}{2}", Environment.NewLine, new String (indentChar, indentLength), c);
          positionOnLine = indentLength + 1;
        }
        else
        {
          sw.Write (c);
          positionOnLine++;
        }
      }


      public override void NewLine ()
      {
        if (quietMode) return;

        sw.Write (Environment.NewLine);
        firstSymbolOnLine = true;
        positionOnLine = 0;
      }
    }
    #endregion FileWriteBuffer
    #endregion FileBuffer

    #region XmlWriteBuffer
    public class XmlWriteBuffer
    {
      protected XmlTextWriter tw;
      protected Stack tagStack; // extra check on matching end tag

      protected void SetInitialValues (bool initialPI)
      {
        tagStack = new Stack ();
        tw.QuoteChar = '"';
        tw.Formatting = Formatting.Indented;

        if (initialPI) tw.WriteProcessingInstruction ("xml", "version=\"1.0\" encoding=\"ISO-8859-1\"");

        tw.WriteComment (String.Format (" Structure created at {0} ", DateTime.Now.ToString ()));
      }


      void WriteAttributes (params string [] av)
      {
        if (av.Length % 2 == 1)
          throw new ParserException ("*** WriteStartElement -- last attribute value is missing");

        for (int j = 0; j < av.Length; j += 2) tw.WriteAttributeString (av [j], av [j + 1]);
      }


      public void WriteStartElement (string tag, params string [] av)
      {
        tw.WriteStartElement (tag);
        WriteAttributes (av);
        tagStack.Push (tag);
      }


      public void WriteAttributeString (string name, string value, params string [] av)
      {
        tw.WriteAttributeString (name, value);
        WriteAttributes (av);
      }


      public void WriteEndElement (string tag)
      {
        if (tagStack.Count == 0)
          throw new ParserException (String.Format ("*** Spurious closing tag \"{0}\"", tag));

        string s = (string)tagStack.Peek ();

        if (tag != s)
          throw new ParserException (String.Format ("*** Closing tag \"{0}\" does not match opening tag \"{1}\"", tag, s));

        tw.WriteEndElement ();
        tagStack.Pop ();
      }


      public void WriteProcessingInstruction (string name, string text)
      {
        tw.WriteProcessingInstruction (name, text);
      }


      public void WriteComment (string text)
      {
        tw.WriteComment (text);
      }


      public void WriteString (string text)
      {
        tw.WriteString (text);
      }


      public void WriteRaw (string text)
      {
        tw.WriteRaw (text);
      }


      public void WriteCData (string text)
      {
        tw.WriteCData (text);
      }


      public void WriteSimpleElement (string elementName, string textContent, params string [] av)
      {
        tw.WriteStartElement (elementName);
        WriteAttributes (av);
        tw.WriteString (textContent);
        tw.WriteEndElement ();
      }


      public void WriteEmptyElement (string elementName, params string [] av)
      {
        tw.WriteStartElement (elementName);
        WriteAttributes (av);
        tw.WriteEndElement ();
      }


      public void Close ()
      {
        tw.Close ();
      }
    }
    #endregion XmlWriteBuffer

    #region XmlFileWriter

    public class XmlFileWriter : XmlWriteBuffer
    {
      public XmlFileWriter (string fileName, bool initialPI)
      {
        tw = new XmlTextWriter (fileName, System.Text.Encoding.GetEncoding (1252));
        SetInitialValues (initialPI);
      }


      public XmlFileWriter (string fileName)
      {
        tw = new XmlTextWriter (fileName, System.Text.Encoding.GetEncoding (1252));
        SetInitialValues (true);
      }
    }
    #endregion XmlFileWriter

    #region XmlStringWriter
    public class XmlStringWriter : XmlWriteBuffer
    {
      public XmlStringWriter (bool initialPI)
      {
        tw = new XmlTextWriter (new MemoryStream (), System.Text.Encoding.GetEncoding (1252));
        SetInitialValues (initialPI);
      }


      public XmlStringWriter ()
      {
        tw = new XmlTextWriter (new MemoryStream (), System.Text.Encoding.GetEncoding (1252));
        SetInitialValues (true);
      }


      public void SaveToFile (string fileName)
      {
        StreamWriter sw = new StreamWriter (fileName);
        sw.Write (this.ToString ());
        sw.Close ();
      }


      public override string ToString ()
      {
        Stream ms = tw.BaseStream;

        if (ms == null) return null;

        tw.Flush ();
        return new ASCIIEncoding ().GetString (((MemoryStream)ms).ToArray ());
      }
    }
    #endregion XmlStringWriter
    #endregion Buffer
  }

  #region Extensions class
  static class BaseParserExtensions
  {
    public static bool IsIdStartChar (this char c)
    {
      return (Char.IsLetter (c) || c == '_');
    }

    public static bool IsBaseIdChar (this char c)
    {
      return (Char.IsLetter (c) || char.IsDigit (c) || c == '_');
    }
  }
  #endregion Extensions class
}

