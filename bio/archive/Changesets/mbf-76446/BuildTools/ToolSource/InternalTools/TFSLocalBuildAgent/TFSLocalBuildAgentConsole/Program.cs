using System;
using System.Globalization;
using TFSLocalBuildAgentLib;

namespace TFSLocalBuildAgentConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) 
            {
                Build.OutputReceivedEvent += new EventHandler<OutputReceivedEventArgs>(Build_OutputDataReceived);
            }

            Build.Start();
        }

        /// <summary>
        /// Handles the output data received events.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The sender event args.</param>
        private static void Build_OutputDataReceived(object sender, OutputReceivedEventArgs e)
        {
            Console.Write(e.Data);
        }
    }
}
