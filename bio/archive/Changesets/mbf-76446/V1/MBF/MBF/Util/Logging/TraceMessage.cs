// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Globalization;

namespace MBF.Util.Logging
{
    /// <summary>
    /// A TraceMessage is a simple message holding class.
    /// </summary>
    public class TraceMessage
    {
        /// <summary>
        /// The context where the event occurred, such as a method name, or
        /// a particular point in a complex operation.
        /// </summary>
        private string context;
        /// <summary>
        /// A description of the event.
        /// </summary>
        private string message;
        /// <summary>
        /// Data associated with the event, such as argument values.
        /// </summary>
        private string data;
        /// <summary>
        /// When the event occurred.
        /// </summary>
        private DateTime when;

        /// <summary>
        /// Construct a message.
        /// </summary>
        /// <param name="c">The context.</param>
        /// <param name="m">The message.</param>
        /// <param name="d">The data.</param>
        /// <param name="w">When the event occurred.</param>
        public TraceMessage(string c, string m, string d, DateTime w)
        {
            context = c;
            message = m;
            data = d;
            when = w;
        }

        /// <summary>
        /// Construct a message, using the current date/time.
        /// </summary>
        /// <param name="c">The context.</param>
        /// <param name="m">The message.</param>
        /// <param name="d">The data.</param>
        public TraceMessage(string c, string m, string d)
            : this(c, m, d, DateTime.Now)
        {
        }

        /// <summary>
        /// Convert a Trace.Message into a user-friendly string.
        /// </summary>
        /// <returns>the string.</returns>
        public string Format()
        {
            return string.Format(CultureInfo.InvariantCulture, when.ToString("u", CultureInfo.InvariantCulture) + ": {0} ({1}, data {2})", message, context, data);
        }
    }
}
