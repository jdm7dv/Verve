// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF.IO.GenBank
{
    /// <summary>
    /// A CrossReferenceType specifies whether the DBLink is 
    /// refering to project or a Trace Assembly Archive.
    /// </summary>
    [Serializable]
    public enum CrossReferenceType
    {
        /// <summary>
        /// None - CrossReferenceType is unspecified.
        /// </summary>
        None,

        /// <summary>
        /// Project.
        /// </summary>
        Project,

        /// <summary>
        /// Trace Assembly Archive.
        /// </summary>
        TraceAssemblyArchive
    }
}
