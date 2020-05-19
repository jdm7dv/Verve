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



namespace Bio.Algorithms.Assembly.PaDeNA.Scaffold
{
    /// <summary>
    ///  Class storing information of a single map between read and contig.
    /// </summary>
    public class ReadMap
    {
        /// <summary>
        /// Gets or sets start position of contig.
        /// </summary>
        public int StartPositionOfContig { get; set; }

        /// <summary>
        /// Gets or sets start position of read. 
        /// </summary>
        public int StartPositionOfRead { get; set; }

        /// <summary>
        /// Gets or sets length of map between read and contig.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets overlap of read and contig
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
