// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF
{
    /// <summary>
    /// UpdateType specifies the type of update.
    /// </summary>
    public enum UpdateType
    {
        /// <summary>
        /// Not a valid update type.
        /// </summary>
        None,

        /// <summary>
        /// Sequence item inserted.
        /// </summary>
        Inserted,

        /// <summary>
        /// Sequence item removed.
        /// </summary>
        Removed,

        /// <summary>
        /// Sequence item replaced.
        /// </summary>
        Replaced
    }
}
