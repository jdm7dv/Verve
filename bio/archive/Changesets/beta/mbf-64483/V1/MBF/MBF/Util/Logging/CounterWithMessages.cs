// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF.Util.Logging
{
    /// <summary>
    /// Writes messages to the console every so many increments.
    /// </summary>
    public class CounterWithMessages
    {
        private CounterWithMessages()
        {
        }

        private string FormatString;
        private int MessageInterval;

        /// <summary>
        /// The number of increments so far.
        /// </summary>
        public int Index { get; private set; }
        private int? CountOrNull;
        private bool Quiet = false;

        /// <summary>
        /// Create a counter that will will output messages to the console every so many increments. Incrementing is thread-safe.
        /// </summary>
        /// <param name="formatStringWithOneOrTwoPlaceholders">A format string with containing at least {0} and, optionally, {1}.</param>
        /// <param name="messageInterval">How often messages should be output, in increments.</param>
        /// <param name="totalCountOrNull">The total number of increments, or null if not known.</param>
        /// <returns>A counter</returns>
        public CounterWithMessages(string formatStringWithOneOrTwoPlaceholders, int messageInterval, int? totalCountOrNull)
            :this(formatStringWithOneOrTwoPlaceholders, messageInterval, totalCountOrNull, false)
        {
        }

        /// <summary>
        /// Create a counter that will will output messages to the console every so many increments. Incrementing is thread-safe.
        /// </summary>
        /// <param name="formatStringWithOneOrTwoPlaceholders">A format string with containing at least {0} and, optionally, {1}.</param>
        /// <param name="messageInterval">How often messages should be output, in increments.</param>
        /// <param name="totalCountOrNull">The total number of increments, or null if not known.</param>
        /// <param name="quiet">if true, doesn't output to the console.</param>
        /// <returns>A counter</returns>
        public CounterWithMessages (string formatStringWithOneOrTwoPlaceholders, int messageInterval, int? totalCountOrNull, bool quiet)
        {
            FormatString = formatStringWithOneOrTwoPlaceholders;
            MessageInterval = messageInterval;
            Index = -1;
            CountOrNull = totalCountOrNull;
            Quiet = quiet;
        }

        /// <summary>
        /// Increment the counter by one. Incrementing is thread-safe.
        /// </summary>
        public int Increment()
        {
            lock (this)
            {
                ++Index;
                if (Index % MessageInterval == 0 && !Quiet)
                {
                    if (null == CountOrNull)
                    {
                        Console.WriteLine(FormatString, Index);
                    }
                    else
                    {
                        Console.WriteLine(FormatString, Index, CountOrNull.Value);
                    }
                }
                return Index;
            }
        }
    }
}
