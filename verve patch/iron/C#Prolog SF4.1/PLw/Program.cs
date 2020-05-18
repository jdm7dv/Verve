using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prolog
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main (string [] args)
    {
      if (args.Length > 0) // batch processing assumed if arguments supplied
      {
        Prolog.MainForm.BatIO batIO = null;

        try
        {
          PrologEngine e = new PrologEngine (batIO = new Prolog.MainForm.BatIO ());
          e.ProcessArgs (args, true);
          Application.Exit ();

          return;
        }
        finally
        {
          if (batIO != null) batIO.Close ();
        }
      }

      Application.EnableVisualStyles ();
      Application.SetCompatibleTextRenderingDefault (false);
      Application.Run (new MainForm ());
    }
  }
}