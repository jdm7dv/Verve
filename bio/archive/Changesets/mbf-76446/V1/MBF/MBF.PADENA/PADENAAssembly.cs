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
using System.Runtime.Serialization;

namespace MBF.Algorithms.Assembly.PaDeNA
{
    /// <summary>
    /// PaDeNAAssembly is the result of running PaDeNA on a set input sequences. 
    /// As part of assembled output, it gives contig and scaffold sequences.
    /// </summary>
    [Serializable]
    public class PaDeNAAssembly : IDeBruijnDeNovoAssembly
    {
        #region Fields
        /// <summary>
        /// Holds list of contigs created after Assembly.
        /// </summary>
        private List<ISequence> _contigSequences;

        /// <summary>
        /// Holds list of scaffolds created after Assembly.
        /// </summary>
        private List<ISequence> _scaffolds;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PaDeNAAssembly class.
        /// Default constructor.
        /// </summary>
        public PaDeNAAssembly()
        {
            _contigSequences = new List<ISequence>();
            _scaffolds = new List<ISequence>();
        }

        /// <summary>
        /// Initializes a new instance of the PaDeNAAssembly class.
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected PaDeNAAssembly(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _contigSequences = (List<ISequence>)info.GetValue("PaDeNAAssembly:ContigSequences", typeof(List<ISequence>));
            _scaffolds = (List<ISequence>)info.GetValue("PaDeNAAssembly:Scaffolds", typeof(List<ISequence>));
            Documentation = info.GetValue("PaDeNAAssembly:Documentation", typeof(object));
        }
        #endregion Constructors

        #region IDeNovoAssembly Members
        /// <summary>
        /// Gets the list of assembled sequences
        /// </summary>
        public IList<ISequence> AssembledSequences
        {
            get
            {
                if (_scaffolds != null && _scaffolds.Count > 0)
                {
                    return _scaffolds;
                }
                else
                {
                    return _contigSequences;
                }
            }
        }

        /// <summary>
        /// Gets or sets the associated documentation.
        /// The Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a PaDeNAAssembly. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        public object Documentation { get; set; }
        #endregion

        /// <summary>
        /// Gets list of contig sequences created by assembler
        /// </summary>
        public IList<ISequence> ContigSequences
        {
            get
            {
                return _contigSequences;
            }
        }

        /// <summary>
        /// Gets the list of assembler scaffolds
        /// </summary>
        public IList<ISequence> Scaffolds
        {
            get { return _scaffolds; }
        }

        /// <summary>
        /// Add list of contigs
        /// </summary>
        /// <param name="contigs">List of contig sequences</param>
        public void AddContigs(IEnumerable<ISequence> contigs)
        {
            if (contigs != null)
                _contigSequences.AddRange(contigs);
        }

        /// <summary>
        /// Add list of scaffolds
        /// </summary>
        /// <param name="scaffolds">List of scaffold sequences</param>
        public void AddScaffolds(IEnumerable<ISequence> scaffolds)
        {
            if (scaffolds != null)
                _scaffolds.AddRange(scaffolds);
        }

        #region ISerializable Members
        /// <summary>
        /// Method for serializing the PaDeNAAssembly.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("PaDeNAAssembly:ContigSequences", _contigSequences);
            info.AddValue("PaDeNAAssembly:Scaffolds", _scaffolds);
            if (Documentation != null && ((Documentation.GetType().Attributes &
               System.Reflection.TypeAttributes.Serializable) == System.Reflection.TypeAttributes.Serializable))
            {
                info.AddValue("PaDeNAAssembly:Documentation", Documentation);
            }
            else
            {
                info.AddValue("PaDeNAAssembly:Documentation", null);
            }
        }
        #endregion
    }
}
