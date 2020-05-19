using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedContracts
{
    public interface IReader
    {
        string ReadLine();
    }

    public interface IWriter
    {
        void WriteLine(string line);
    }
}
