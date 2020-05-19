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

namespace MBF.IO
{
    /// <summary>
    /// SequenceRangeFormatter class is an abstraction class which provides instances
    /// and lists of all Range-Formatter currently supported by MBF.
    /// </summary>
    public static class SequenceRangeFormatters
    {
        /// <summary>
        /// A singleton instance of BedFormatter class which is capable of
        /// saving a ISequenceRange according to the BED file format.
        /// </summary>
        private static BedFormatter bed = new BedFormatter();

        /// <summary>
        /// List of all supported Range-Formatters.
        /// </summary>
        private static List<ISequenceRangeFormatter> all = new List<ISequenceRangeFormatter>() { bed };

        /// <summary>
        /// Gets an instance of BedFormatter class which is capable of
        /// saving a ISequenceRange according to the BED file format.
        /// </summary>
        public static BedFormatter Bed
        {
            get
            {
                return bed;
            }
        }

        /// <summary>
        /// Gets the list of all range-formatters supported by the framework.
        /// </summary>
        public static IList<ISequenceRangeFormatter> All
        {
            get
            {
                return all.AsReadOnly();
            }
        }

    }
}
