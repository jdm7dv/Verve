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
using System.Linq;
using System.Runtime.Serialization;

namespace MBF.Algorithms.Assembly
{
    /// <summary>
    /// OverlapDeNovoAssembly is a implementation of IOverlapDeNovoAssembly that stores the 
    /// assembly result.
    /// 
    /// This class contains list of contigs and list of unmerged sequences.
    /// To maintain the information like history or context, use Documentation property of this class.
    /// 
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and the stored files 
    /// can be de-serialized to restore the instances.
    /// </summary>
    [Serializable]
    public class OverlapDeNovoAssembly : IOverlapDeNovoAssembly
    {
        #region Fields
        /// <summary>
        /// Holds list of contigs created after Assembly.
        /// </summary>
        List<Contig> _contigs;

        /// <summary>
        /// Holds list of sequences that could not be merged into any contig.
        /// </summary>
        List<ISequence> _unmergedSequences;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the OverlapDeNovoAssembly class.
        /// Default constructor.
        /// </summary>
        public OverlapDeNovoAssembly()
        {
            _contigs = new List<Contig>();
            _unmergedSequences = new List<ISequence>();
        }
        #endregion Constructors

        #region IOverlapLayoutSequenceAssembly Members
        /// <summary>
        /// Gets list of contigs created after Assembly.
        /// </summary>
        public IList<Contig> Contigs
        {
            get
            {
                return _contigs;
            }
        }

        /// <summary>
        /// Gets list of sequences that could not be merged into any contig.
        /// </summary>
        public IList<ISequence> UnmergedSequences
        {
            get
            {
                return _unmergedSequences;
            }
        }

        /// <summary>
        /// Gets the list of assembled sequences
        /// </summary>
        public IList<ISequence> AssembledSequences
        {
            get { return new List<ISequence>(_contigs.Select(c => c.Consensus)); }
        }

        /// <summary>
        /// Gets or sets the Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a OverlapDeNovoAssembly. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        public object Documentation { get; set; }
        #endregion

        #region ISerializable Members
        /// <summary>
        /// Initializes a new instance of the OverlapDeNovoAssembly class.
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        internal OverlapDeNovoAssembly(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _contigs = (List<Contig>)info.GetValue("OverlapDeNovoAssembly:Contigs", typeof(List<Contig>));
            _unmergedSequences = (List<ISequence>)info.GetValue("OverlapDeNovoAssembly:UnmergedSequences", typeof(List<ISequence>));
            Documentation = info.GetValue("OverlapDeNovoAssembly:Documentation", typeof(object));
        }

        /// <summary>
        /// Method for serializing the OverlapDeNovoAssembly.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("OverlapDeNovoAssembly:Contigs", _contigs);
            info.AddValue("OverlapDeNovoAssembly:UnmergedSequences", _unmergedSequences);
            if (Documentation != null && ((Documentation.GetType().Attributes &
               System.Reflection.TypeAttributes.Serializable) == System.Reflection.TypeAttributes.Serializable))
            {
                info.AddValue("OverlapDeNovoAssembly:Documentation", Documentation);
            }
            else
            {
                info.AddValue("OverlapDeNovoAssembly:Documentation", null);
            }
        }
        #endregion
    }
}
