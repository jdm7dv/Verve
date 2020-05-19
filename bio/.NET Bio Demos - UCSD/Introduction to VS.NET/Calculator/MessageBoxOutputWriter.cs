using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedContracts;

namespace Calculator
{
    class MessageBoxOutputWriter : IWriter
    {
        public void WriteLine(string line)
        {
            System.Windows.Forms.MessageBox.Show(line, "Message from App");
        }
    }
}
