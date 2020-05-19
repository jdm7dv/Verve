// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.IO;
using MBF.IO.Bed;

namespace MBF
{
    /// <summary>
    /// SequenceRangeParsers class is an abstraction class which provides instances
    /// and lists of all Range-Parsers currently supported by MBF.
    /// </summary>
    public static class SequenceRangeParsers
    {
        /// <summary>
        /// A singleton instance of BedParser class which is capable of
        /// parsing BED format files.
        /// </summary>
        private static BedParser bed = new BedParser();

        /// <summary>
        /// List of all supported Range-Parsers.
        /// </summary>
        private static List<ISequenceRangeParser> all = new List<ISequenceRangeParser>() { bed };

        /// <summary>
        /// Gets an instance of BedParser class which is capable of
        /// parsing BED format files.
        /// </summary>
        public static BedParser Bed
        {
            get
            {
                return bed;
            }
        }

        /// <summary>
        /// Gets the list of all Range-parsers which is supported by the framework.
        /// </summary>
        public static IList<ISequenceRangeParser> All
        {
            get
            {
                return all.AsReadOnly();
            }
        }
    }
}
