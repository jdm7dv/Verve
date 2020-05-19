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



namespace Bio
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
