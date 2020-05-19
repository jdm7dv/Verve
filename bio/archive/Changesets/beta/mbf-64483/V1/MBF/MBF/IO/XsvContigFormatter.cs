// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MBF.Algorithms.Assembly;


namespace MBF.IO 
{
  
    /// <summary>
    /// This class will write a contig as a list of sparse sequences using the 
    /// XsvSparseFormatter. The first sequence is the consensus, and the rest are
    /// the assembled sequences offset from the consensus. 
    /// E.g. formatting a Contig with 2 assembled sequences, using '#' as sequence prefix and ',' as character separator.
    /// # 0,100,Consensus
    /// 12,A
    /// 29,T
    /// 39,G
    /// #3,10,Fragment1
    /// 9,A
    /// #25,20,Fragment2
    /// 4,T
    /// 14,G
    /// </summary>
    public class XsvContigFormatter : XsvSparseFormatter 
    {
        ///<summary>
        /// Creates a formatter for contigs using the given separator and 
        /// sequence start line prefix characacter.
        ///</summary>
        ///<param name="separator_">The character to separate position of the sequence 
        /// item from its symbol, and separate the offset, count and sequence ID in the 
        /// sequence start line.</param>
        ///<param name="sequenceIDPrefix_">The character to refix the sequence start line.</param>
        public XsvContigFormatter (char separator_, char sequenceIDPrefix_)
            : base(separator_, sequenceIDPrefix_) 
        { }

        /// <summary>
        /// Formats a (sparse) contig to a charcter separated value file,
        /// writing the consensus first, followed by the sequence separator,
        /// and each assembled sequences followed by the sequence separator.
        /// The consensus has an offet of 0, while the assembed sequences have the
        /// offset as present in AssembledSequence.Position.
        /// </summary>
        /// <param name="contig">The contig to format as a set of sparse sequences.</param>
        /// <param name="writer">The text writer to write the formatted output to.</param>
        public void Format (Contig contig, TextWriter writer) 
        {
            Format(contig.Consensus, writer);
            foreach (Contig.AssembledSequence aSeq in contig.Sequences) 
            {
                Format(aSeq.Sequence, aSeq.Position, writer);
            }
        }
    }
}
