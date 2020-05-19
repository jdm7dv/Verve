// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;

namespace MBF.Util.Logging
{
    /// <summary>
    /// The Trace class implements a mechanism for logging messages, both to a Log object,
    /// and to a simple message queue that can be used for GUI display or other purposes.
    /// </summary>
    public static class Trace
    {
        /// <summary>
        /// Flag to report non-fatal sequence parsing/formatting errors.
        /// </summary>
        public const ulong SeqWarnings = 0x1;

        /// <summary>
        /// Flag to report details of sequence assembly into the log.
        /// </summary>
        public const ulong AssemblyDetails = 0x2;

        private static ulong s_flags = 0;
        const int DEFAULT_MAX_MESSAGES = 20;
        private static int _max_messages = DEFAULT_MAX_MESSAGES;
        private static List<TraceMessage> s_messages = new List<TraceMessage>();

        private static void trimToSize()
        {
            int ct = s_messages.Count;
            if (ct > _max_messages)
            {
                s_messages.RemoveRange(_max_messages, ct - _max_messages);
            }
        }

        /// <summary>
        /// Test to see if a flag is in the set of flags currently turned on.
        /// </summary>
        /// <param name="flag">a flag, encoded as a single bit in a ulong.</param>
        /// <returns>true if the flag is set.</returns>
        public static bool Want(ulong flag)
        {
            return (flag & s_flags) != 0;
        }

        /// <summary>
        /// Report a TraceMessage, by adding it to the front of the message
        /// queue, as well as logging it.
        /// </summary>
        /// <param name="m"></param>
        public static void Report(TraceMessage m)
        {
            s_messages.Insert(0, m);
            ApplicationLog.WriteLine(m.Format());
            trimToSize();
        }

        /// <summary>
        /// Overload that constructs the TraceMessage from its parts.
        /// </summary>
        /// <param name="context">Where the incident occurred.</param>
        /// <param name="message">The details of what happened.</param>
        /// <param name="data">Pertinent data such as argument values.</param>
        public static void Report(string context, string message, string data)
        {
            DateTime when = DateTime.Now;
            TraceMessage m = new TraceMessage(context, message, data, when);
            Report(m);
        }

        /// <summary>
        /// Overload to report from a plain string.
        /// </summary>
        /// <param name="message">the message.</param>
        public static void Report(string message)
        {
            Report("", message, "");
        }

        /// <summary>
        /// Return the newest message in the queue (or null, if none).
        /// </summary>
        /// <returns>a TraceMessage.</returns>
        public static TraceMessage LatestMessage()
        {
            return GetMessage(0);
        }

        /// <summary>
        /// return the ith message in the queue (0 = newest).
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>the TraceMessage.</returns>
        public static TraceMessage GetMessage(int i)
        {
            if (i < 0 || i >= s_messages.Count)
            {
                return null;
            }
            return s_messages[i];
        }

        /// <summary>
        /// Turn on a flag, expressed as a set bit in a ulong.
        /// </summary>
        /// <param name="flag">The bit to set.</param>
        public static void Set(ulong flag)
        {
            s_flags = s_flags | flag;
        }

        /// <summary>
        /// Clear a flag, expressed as a set bit in a ulong.
        /// </summary>
        /// <param name="flag">The bit to clear.</param>
        public static void Clear(ulong flag)
        {
            s_flags = s_flags & ~flag;
        }
    }
}
