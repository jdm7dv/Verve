//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System;

namespace Bio.IO.GenBank
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
