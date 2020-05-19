//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Threading;
using Bio.Util;
using System.IO;

namespace MBT.Escience
{
    /// <summary>
    /// A simple progress meter than will output updates. To use, create an instance, then call Increment() each time an item is completed.
    /// Progress will periodically be printed to Console.Out, or a TextWriter provided at creation, using carriage return so that only one line is used. To just use as a counter, 
    /// specify Quiet = true. Default messages, with a user supplied prefix, will be used unless otherwise specified by overriding FormatString
    /// and FinalOutputFormatString.
    /// </summary>
    public class CounterWithMessages
    {
        const string DEFAULT_FORMAT_STRING_WITH_TOTAL = " {0} of {1} ({2}%) complete. Estimated {3} remaining.";
        const string DEFAULT_FORMAT_STRING_NO_TOTAL = " {0} complete.";

        DateTime StartTime;
        private int MessageInterval;
        private int TotalCount;
        private bool _useLocks;
        private object _lockObject;
        public bool Quiet { get; set; }

        public volatile int _countComplete;
        public int CountComplete { get { return _countComplete; } }

        private TextWriter Out;

        private string _formatString;
        /// <summary>
        /// The format string for the messages has up to 4 arguments:
        /// {0} Is required. This is the CountComplete
        /// {1} The total count (-1 if none supplied)
        /// {2} The percent complete (-1 if no total)
        /// {3} The estimated time to completion (0 if no total)
        /// </summary>
        public string IncrementFormatString
        {
            get { return _formatString; }
            set
            {
                Helper.CheckCondition(value.Contains("{0"), "Format string must at minimum have a spot for the number of completed items.");
                _formatString = value;
            }
        }

        public string CompletedFormatString { get; set; }

        public CounterWithMessages(string outputPrefixOrNull, int? messageIntervalOrNull = null, int? totalCountOrNull = null, bool quiet = false, TextWriter writer = null, bool useLocks = false)
        {
            _useLocks = useLocks;
            if (_useLocks)
                _lockObject = new object();
            Init(outputPrefixOrNull, messageIntervalOrNull, totalCountOrNull, quiet, writer);
        }

        private void Init(string outputPrefixOrNull, int? messageIntervalOrNull, int? totalCountOrNull, bool quiet, TextWriter writerOrNullForConsole)
        {
            Helper.CheckCondition(totalCountOrNull.HasValue || messageIntervalOrNull.HasValue, "You must specify at least one of messageIntervalOrNull or totalCountOrNull");
            if (writerOrNullForConsole == null)
                Out = Console.Out;
            else
                Out = writerOrNullForConsole;

            IncrementFormatString = (string.IsNullOrEmpty(outputPrefixOrNull) ? "" : outputPrefixOrNull) + (totalCountOrNull.HasValue ? DEFAULT_FORMAT_STRING_WITH_TOTAL : DEFAULT_FORMAT_STRING_NO_TOTAL);
            CompletedFormatString = (string.IsNullOrEmpty(outputPrefixOrNull) ? "" : outputPrefixOrNull) + " completed in {0}.";
            TotalCount = totalCountOrNull.HasValue ? totalCountOrNull.Value : -1;

            MessageInterval = messageIntervalOrNull == null ? (int)(0.01 * totalCountOrNull.Value) : messageIntervalOrNull.Value;
            MessageInterval = Math.Max(MessageInterval, 1);

            _countComplete = 0;
            Quiet = quiet;
            StartTime = DateTime.Now;
        }

        public CounterWithMessages(FileInfo fileInfo, int? messageIntervalOrNull = 10000, double compressionRatio = 0.0, bool quiet = false, TextWriter writer = null)
        {
            int? totalLineCountOrNull = null;

            double uncompressedFileLength = fileInfo.Length / (1.0 - compressionRatio);
            
            using (TextReader textReader = 
#if !SILVERLIGHT

                (compressionRatio != 0) ? fileInfo.UnGZip() :
#endif
                fileInfo.OpenText())
            {
                string line = textReader.ReadLine();
                if (null != line || line.Length == 0)
                {
                    totalLineCountOrNull = (int?)(uncompressedFileLength / (long)(line.Length + 2 /*line end*/));
                    messageIntervalOrNull = null;
                }
            }

            Init("Reading " + fileInfo.Name, messageIntervalOrNull, totalLineCountOrNull, quiet, writer);
        }

        public void Increment()
        {
            if (_useLocks) lock (_lockObject) IncrementInternal();
            else IncrementInternal();
        }

        public void IncrementInternal()
        {
            int thisCount = ++_countComplete;   // update then save to a local variable to minimize the chance that PrintProgress prints the results of a different thread.
            if (thisCount == TotalCount)
                PrintComplete();
            else
                PrintProgress(thisCount);
        }

        bool finalLinePrinted = false;
        private void PrintComplete()
        {
            while (printing) Thread.Sleep(100); // if this is the last one, wait for anyone who's currently writing to finish
            if (!finalLinePrinted && !Quiet)
            {
                finalLinePrinted = true;
                TimeSpan totalTime = DateTime.Now - StartTime;
                Out.WriteLine("\r" + CompletedFormatString.PadRight(IncrementFormatString.Length), FormatTimeString(totalTime));
            }
        }

        /// <summary>
        /// Call this to mark that everything is finished. If the final line has already been printed, nothing will hapen. Else, the counter
        /// will be set to the finished state and the final output will be printed. Useful if you're running in a parallel context such that
        /// the counter may miss some Increment calls.
        /// </summary>
        public void Finished()
        {
            _countComplete = TotalCount;
            PrintComplete();
        }

        bool printing = false;
        private System.IO.FileInfo SnpFile;
        private void PrintProgress(int countComplete)
        {
            if (!Quiet && !printing && countComplete % MessageInterval == 0)
            {
                printing = true;

                int percComplete = TotalCount >= 0 ? (int)Math.Floor((double)countComplete / TotalCount * 100) : -1;

                long timerPer = (DateTime.Now - StartTime).Ticks / countComplete;
                TimeSpan timeRemaining = TotalCount >= 0 ? new TimeSpan(timerPer * (TotalCount - countComplete)) : new TimeSpan();

                Out.Write("\r" + IncrementFormatString, countComplete, TotalCount, percComplete, FormatTimeString(timeRemaining));

                printing = false;
            }
        }

        private string FormatTimeString(TimeSpan timeRemaining)
        {
            return string.Format("{0}h{1}m{2}s", 24 * timeRemaining.Days + timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
        }
    }
}
