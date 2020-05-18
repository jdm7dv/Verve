//#define LL1_tracing
namespace <NameSpace/>
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
  using System.Security.Principal;<CsUsing/>
<Banner/>

  public partial class PrologEngine
  {
    #region <ParserClassPrefix/>Parser
    public partial class <ParserClassPrefix/>Parser : BaseParser<<TerminalDescrPayload/>>
    {
      public static readonly string VersionTimeStamp = "<Now/>";
      <ppChar/>
      <CsMembers/>     
      #region Terminal definition
      <TTerminal/>
      <TTerminalTableFill/>
      #endregion Terminal definition

      #region Constructor
<Prolog>
      public <ParserClassPrefix/>Parser (PrologEngine engine)
      {
        this.engine = engine;
        ps = engine.Ps;
        terminalTable = engine.terminalTable;
        opTable = engine.OpTable;
        symbol = new Symbol (this);
        streamInPrefix = "";
        streamInPreLen = 0;
        AddReservedOperators ();
      }

</Prolog>
      public <ParserClassPrefix/>Parser ()
      {
        terminalTable = new BaseTrie (terminalCount, false);
        FillTerminalTable (terminalTable);
        symbol = new Symbol (this);
        streamInPrefix = "";
        streamInPreLen = 0;
<Prolog>
        AddReservedOperators ();
</Prolog>
      }
      #endregion constructor

      #region NextSymbol, GetSymbol
      protected override bool GetSymbol (TerminalSet followers, bool done, bool genXCPN<;RDPDecl/>)
      {
        string s;

        if (symbol.IsProcessed) NextSymbol (<RDPCall/>);

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
        <RootCall/>
      }


      public override void Delegates ()
      {
        <CsDelegates/>
      }

      <ParserProcs/>
      #endregion PARSER PROCEDURES
    }
    #endregion <ParserClassPrefix/>Parser
  }
}
