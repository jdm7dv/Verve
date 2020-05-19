using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringProcessor
{
    /// <summary>
    /// This interface defines our string processing
    /// contract.
    /// </summary>
    public interface IStringProcessor
    {
        /// <summary>
        /// Method to process a string.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Modified (processed) results</returns>
        string Process(string input);
    }
}
