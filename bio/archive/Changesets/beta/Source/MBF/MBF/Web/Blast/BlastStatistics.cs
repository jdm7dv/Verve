// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Runtime.Serialization;

namespace MBF.Web.Blast
{
    /// <summary>
    /// Container for the Statistics segment of the XML BLAST format.
    /// </summary>
    [Serializable]
    public class BlastStatistics : ISerializable
    {
        #region Constructors

        /// <summary>
        /// Default Constructor: Initializes an instance of class BlastStatistics
        /// </summary>
        public BlastStatistics()
        { }

        #endregion

        # region Properties

        /// <summary>
        /// The number of sequences in the iteration
        /// </summary>
        public int SequenceCount { get; set; }

        /// <summary>
        /// Database size, for correction
        /// </summary>
        public long DatabaseLength { get; set; }

        /// <summary>
        /// Effective HSP length
        /// </summary>
        public long HspLength { get; set; }

        /// <summary>
        /// Effective search space
        /// </summary>
        public double EffectiveSearchSpace { get; set; }

        /// <summary>
        /// Karlin-Altschul parameter K
        /// </summary>
        public double Kappa { get; set; }

        /// <summary>
        /// Karlin-Altschul parameter Lambda
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>
        /// Karlin-Altschul parameter H
        /// </summary>
        public double Entropy { get; set; }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected BlastStatistics(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            SequenceCount = info.GetInt32("BlastStatistics:SequenceCount");
            DatabaseLength = info.GetInt64("BlastStatistics:DatabaseLength");
            HspLength = info.GetInt64("BlastStatistics:HspLength");
            EffectiveSearchSpace = info.GetDouble("BlastStatistics:EffectiveSearchSpace");
            Kappa = info.GetDouble("BlastStatistics:Kappa");
            Lambda = info.GetDouble("BlastStatistics:Lambda");
            Entropy = info.GetDouble("BlastStatistics:Entropy");
        }

        /// <summary>
        /// Method for serializing the sequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("BlastStatistics:SequenceCount", SequenceCount);
            info.AddValue("BlastStatistics:DatabaseLength", DatabaseLength);
            info.AddValue("BlastStatistics:HspLength", HspLength);
            info.AddValue("BlastStatistics:EffectiveSearchSpace", EffectiveSearchSpace);
            info.AddValue("BlastStatistics:Kappa", Kappa);
            info.AddValue("BlastStatistics:Lambda", Lambda);
            info.AddValue("BlastStatistics:Entropy", Entropy);

        }

        #endregion
    }
}
