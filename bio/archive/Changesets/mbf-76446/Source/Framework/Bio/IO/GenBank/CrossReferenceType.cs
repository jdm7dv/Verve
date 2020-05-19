// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;

namespace Bio.IO.GenBank
{
    /// <summary>
    /// A CrossReferenceType specifies whether the DBLink is 
    /// referring to project or a Trace Assembly Archive.
    /// </summary>
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
