using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using Prolog;

namespace Prolog
{
  public enum GuiAction
  {
    None, ReadStart, ReadEnd, ReadLn, ReadCh, Write, WriteLn, NewLn, Clear, Reset, BtnsOn, BtnsOff
  }

  public partial class MainForm : Form
  {
    #region WinIO. Base class for Windows IO
    public class WinIO : BasicIo
    {
      BackgroundWorker bgw;
      ManualResetEvent semaGetInput;
      TextBox tbInput;
      Queue<int> charBuffer;

      public WinIO (BackgroundWorker bgw, ManualResetEvent semaGetInput,
        TextBox tbInput, Queue<int> charBuffer)
      {
        this.bgw = bgw;
        this.semaGetInput = semaGetInput;
        this.tbInput = tbInput;
        this.charBuffer = charBuffer;
      }


      public override string ReadLine ()
      {
        try
        {
          bgw.DoGuiAction (GuiAction.ReadStart);
          bgw.DoGuiAction (GuiAction.ReadLn);
          semaGetInput.WaitOne (); // wait until text has been entered in tbInput

          return tbInput.Text;
        }
        finally
        {
          semaGetInput.Reset ();
          bgw.DoGuiAction (GuiAction.ReadEnd);
        }
      }


      public override int ReadChar ()
      {
        if (charBuffer.Count == 0)
          try
          {
            bgw.DoGuiAction (GuiAction.ReadStart);
            bgw.DoGuiAction (GuiAction.ReadCh);
            semaGetInput.WaitOne (); // wait until charBuffer is not empty
          }
          finally
          {
            semaGetInput.Reset ();
            bgw.DoGuiAction (GuiAction.ReadEnd);
          }

        return charBuffer.Dequeue ();
      }


      public override void Write (string s)
      {
        bgw.DoGuiAction (GuiAction.Write, s);
      }


      public override void WriteLine (string s)
      {
        bgw.DoGuiAction (GuiAction.WriteLn, s);
      }


      public override void WriteLine ()
      {
        bgw.DoGuiAction (GuiAction.NewLn);
      }


      public override void Clear ()
      {
        bgw.DoGuiAction (GuiAction.Clear);
      }


      public override void Reset ()
      {
        bgw.DoGuiAction (GuiAction.Reset);
      }
    }
    #endregion WinIO

    #region BatIO. Base class for batch IO. Input not possible, output written to log file
    public class BatIO : BasicIo
    {
      StreamWriter sw = null;
      string pathName;
      string fileName;
      bool fileOpen;

      public BatIO ()
      {
        fileOpen = false; // file will be created if there is any output at all
      }

      private void CreateFile ()
      {
        try
        {
          pathName =
            Path.GetDirectoryName (Application.ExecutablePath) +
            Path.DirectorySeparatorChar + "batchlogs" + Path.DirectorySeparatorChar;

          if (!Directory.Exists (pathName)) Directory.CreateDirectory (pathName);
        }
        catch (Exception e)
        {
          MessageBox.Show (string.Format ("Error creating directory '{0}'.\r\nMessage was:\r\n{1}",
            pathName, e.Message));
        }

        try
        {
          fileName = pathName + "PLw" + DateTime.Now.ToString ("yyyy-MM-dd@HH.mm.ss") + ".log";
          sw = new StreamWriter (fileName);
        }
        catch (Exception e)
        {
          MessageBox.Show (string.Format ("Error opening log file '{0}'.\r\nMessage was:\r\n{1}",
            fileName, e.Message));
        }

        fileOpen = true;
      }


      public override string ReadLine ()
      {
        return "";
      }


      public override int ReadChar ()
      {
        return 0;
      }


      public override void Write (string s)
      {
        if (!fileOpen) CreateFile ();

        sw.Write (s);
      }


      public override void WriteLine (string s)
      {
        if (!fileOpen) CreateFile ();

        sw.WriteLine (s);
      }


      public override void WriteLine ()
      {
        if (!fileOpen) CreateFile ();

        sw.WriteLine ();
      }


      public override void Clear ()
      {
      }


      public override void Reset ()
      {
      }


      public void Close ()
      {
        if (sw != null) sw.Close ();
      }
    }
    #endregion BatIO

    Prolog.PrologEngine.ApplicationStorage persistentSettings;
    PrologEngine pe;
    bool? stop;
    ManualResetEvent semaMoreStop;
    ManualResetEvent semaGetInput;
    WinIO winIO;
    Queue<int> charBuffer;
    GuiAction readMode; // for distinguishing between various ways of reading input

    public MainForm ()
    {
      InitializeComponent ();
      Text = "C# Prolog -- basic Windows version";
      persistentSettings = new PrologEngine.ApplicationStorage ();
      stop = null;
      semaGetInput = new ManualResetEvent (false);
      charBuffer = new Queue<int> ();
      winIO = new WinIO (bgwExecuteQuery, semaGetInput, tbInput, charBuffer);
      bgwExecuteQuery.DoGuiAction (GuiAction.BtnsOff);
      pe = new PrologEngine (winIO);
      readMode = GuiAction.None;
    }


    void btnXeqQuery_Click (object sender, EventArgs e)
    {
      if (bgwExecuteQuery.IsBusy && !bgwExecuteQuery.CancellationPending) return;

      //tbAnswer.Clear ();
      btnCancelQuery.Enabled = true;
      btnMore.Enabled = btnStop.Enabled = false;
      lblMoreOrStop.Visible = false;
      bgwExecuteQuery.RunWorkerAsync (rtbQuery.Text.AddEndDot ());
    }


    void bgwExecuteQuery_DoWork (object sender, DoWorkEventArgs e)
    {
      try
      {
        pe.Query = e.Argument as string;
        semaMoreStop = new ManualResetEvent (false);

        foreach (PrologEngine.ISolution s in pe.SolutionIterator)
        {
          winIO.WriteLine ("{0}{1}", s, (s.IsLast ? null : ";"));

          if (s.IsLast) break;

          bool stop;
          WaitForMoreOrStopPressed (out stop);
          semaMoreStop.Reset ();

          if (stop) break;
        }
      }
      finally
      {
        pe.PersistCommandHistory ();
      }
    }


    void WaitForMoreOrStopPressed (out bool halt)
    {
      bgwExecuteQuery.DoGuiAction (GuiAction.BtnsOn);

      try
      {
        semaMoreStop.WaitOne ();
      }
      finally
      {
        halt = stop ?? false;
        stop = null;
        bgwExecuteQuery.DoGuiAction (GuiAction.BtnsOff);
      }
    }


    void btnMore_Click (object sender, EventArgs e)
    {
      stop = false;
      semaMoreStop.Set ();
    }


    void btnStop_Click (object sender, EventArgs e)
    {
      stop = true;
      semaMoreStop.Set ();
    }

    // click event for (now invisible) Cancel-button, which does not work as expected.
    // (execution does not get interrupted, have to sort out why this does not work)
    void btnCancelQuery_Click (object sender, EventArgs e)
    {
      bgwExecuteQuery.CancelAsync ();
      btnXeqQuery.Enabled = true;

      while (bgwExecuteQuery.CancellationPending)
      {
        Application.DoEvents ();
        Thread.Sleep (10);
      }
    }


    void bgwExecuteQuery_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
    {
      btnCancelQuery.Enabled = false;
      btnMore.Enabled = btnStop.Enabled = false;
      btnXeqQuery.Enabled = true;
    }


    void bgwExecuteQuery_ProgressChanged (object sender, ProgressChangedEventArgs e)
    {
      switch ((GuiAction)e.ProgressPercentage)
      {
        case GuiAction.ReadStart:
          tbInput.Text = null;
          tbInput.Enabled = true;
          btnXeqQuery.Enabled = false;
          pnlInput.BackColor = Color.Red;
          tbInput.Focus ();
          break;
        case GuiAction.ReadEnd:
          tbInput.Enabled = false;
          btnXeqQuery.Enabled = true;
          pnlInput.BackColor = tpInterpreter.BackColor;
          break;
        case GuiAction.ReadLn:
          readMode = GuiAction.ReadLn;
          break;
        case GuiAction.ReadCh:
          readMode = GuiAction.ReadCh;
          break;
        case GuiAction.Write:
          tbAnswer.Write (e.UserState as string);
          break;
        case GuiAction.WriteLn:
          tbAnswer.WriteLine (e.UserState as string);
          break;
        case GuiAction.NewLn:
          tbAnswer.WriteLine ();
          break;
        case GuiAction.Clear:
          tbAnswer.Clear ();
          break;
        case GuiAction.Reset:
          tbInput.Clear ();
          break;
        case GuiAction.BtnsOn:
          btnMore.Enabled = btnStop.Enabled = true;
          btnXeqQuery.Enabled = false;
          lblMoreOrStop.Visible = true;
          break;
        case GuiAction.BtnsOff:
          tbInput.Enabled = false;
          btnMore.Enabled = btnStop.Enabled = false;
          lblMoreOrStop.Visible = false;
          break;
      }
    }


    void exitToolStripMenuItem_Click (object sender, EventArgs e)
    {
      Close ();
    }


    void btnClear_Click (object sender, EventArgs e)
    {
      rtbQuery.Text = null;
    }


    void btnClose_Click (object sender, EventArgs e)
    {
      Close ();
    }


    private void tbInput_KeyDown (object sender, KeyEventArgs e)
    {

      if (cbNewLines.Checked && e.KeyCode == Keys.Enter)
      {
        if (readMode == GuiAction.ReadCh)
        {
          foreach (char c in tbInput.Text) charBuffer.Enqueue (c);
          foreach (char c in Environment.NewLine) charBuffer.Enqueue (c);
        }

        e.Handled = true;
        e.SuppressKeyPress = true;
        semaGetInput.Set ();
      }
    }


    private void btnEnter_Click (object sender, EventArgs e)
    {
      if (readMode == GuiAction.ReadCh)
        foreach (char c in Environment.NewLine) charBuffer.Enqueue (c);

      semaGetInput.Set ();
    }


    private void cbNewLines_CheckedChanged (object sender, EventArgs e)
    {
      btnEnter.Visible = (!cbNewLines.Checked);
    }


    private void btnClearA_Click (object sender, EventArgs e)
    {
      tbAnswer.Clear ();
    }


    //void LoadFileContentsInTextbox (string fileName, RichTextBox rtb)
    //{
    //  if (File.Exists (fileName))
    //  {
    //    rtb.Text = File.ReadAllText (fileName);
    //    rtb.Update ();
    //    rtb.Select (0, 0);
    //  }
    //}
  }


  public static class Extensions
  {
    static string CRLF = Environment.NewLine;

    public static void Write (this TextBox tb, object s)
    {
      tb.AppendText (s.ToString ());
    }

    public static void Write (this TextBox tb, string s, params object [] args)
    {
      tb.AppendText (string.Format (s, args));
    }

    public static void WriteLine (this TextBox tb)
    {
      tb.AppendText (CRLF);
    }

    public static void WriteLine (this TextBox tb, object s)
    {
      tb.AppendText (s.ToString () + CRLF);
    }

    public static void WriteLine (this TextBox tb, string s, params object [] args)
    {
      if (args.Length == 0)
        tb.AppendText (s);
      else
        tb.AppendText (string.Format (s, args));

      tb.AppendText (CRLF);
    }

    // BackgroundWorker
    public static void DoGuiAction (this BackgroundWorker bgw, GuiAction a)
    {
      bgw.ReportProgress ((int)a, null);
    }


    public static void DoGuiAction (this BackgroundWorker bgw, GuiAction a, string s)
    {
      bgw.ReportProgress ((int)a, s);
    }
  }

}