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
namespace Bio.Algorithms.Assembly.Padena.Scaffold
{
    /// <summary>
    ///  Class storing information of a single map between read and contig.
    /// </summary>
    public class ReadMap
    {
        /// <summary>
        /// Gets or sets start position of contig.
        /// </summary>
        public long StartPositionOfContig { get; set; }

        /// <summary>
        /// Gets or sets start position of read. 
        /// </summary>
        public long StartPositionOfRead { get; set; }

        /// <summary>
        /// Gets or sets length of map between read and contig.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Gets or sets overlap of read and contig.
        /// FullOverlap
        /// ------------- Contig
        ///    ------     Read
        /// PartialOverlap
        /// -------------       Contig
        ///            ------   Read
        /// </summary>
        public ContigReadOverlapType ReadOverlap { get; set; }
    }
}
