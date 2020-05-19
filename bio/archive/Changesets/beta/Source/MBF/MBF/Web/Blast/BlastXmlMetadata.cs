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
    /// Container for the Output segment of the XML BLAST format. This
    /// contains metadata for the search.
    /// </summary>
    [Serializable]
    public class BlastXmlMetadata : ISerializable
    {
        #region Constructors

        /// <summary>
        /// Default Constructor: Initializes an instance of class BlastXmlMetadata
        /// </summary>
        public BlastXmlMetadata()
        { }

        #endregion Constructors

        # region Properties

        /// <summary>
        /// The name of the program invoked (blastp, etc.)
        /// </summary>
        public string Program { get; set; }

        /// <summary>
        /// The BLAST version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Literature reference for BLAST (always the same)
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// The database(s) searched
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// The query identifier defined by the service
        /// </summary>
        public string QueryId { get; set; }

        /// <summary>
        /// The query definition line (if any)
        /// </summary>
        public string QueryDefinition { get; set; }

        /// <summary>
        /// The length of the query sequence
        /// </summary>
        public int QueryLength { get; set; }

        /// <summary>
        /// The query sequence (optional, may not be returned)
        /// </summary>
        public string QuerySequence { get; set; }

        // note: the following attributes are nested in the <Parameters>
        // section inside <BlastOutput>.

        /// <summary>
        /// The name of the similarity matrix used
        /// </summary>
        public string ParameterMatrix { get; set; }

        /// <summary>
        /// The Expect value used for the search
        /// </summary>
        public double ParameterExpect { get; set; }

        /// <summary>
        /// The inclusion threshold for a PSI-Blast iteration
        /// </summary>
        public double ParameterInclude { get; set; }

        /// <summary>
        /// The match score for nucleotide-nucleotide comparisons
        /// </summary>
        public int ParameterMatchScore { get; set; }

        /// <summary>
        /// The mismatch score for nucleotide-nucleotide comparisons
        /// </summary>
        public int ParameterMismatchScore { get; set; }

        /// <summary>
        /// The gap open penalty
        /// </summary>
        public int ParameterGapOpen { get; set; }

        /// <summary>
        /// The Gap extension penalty.
        /// </summary>
        public int ParameterGapExtend { get; set; }

        /// <summary>
        /// The filtering options used for the search
        /// </summary>
        public string ParameterFilter { get; set; }

        /// <summary>
        /// The pattern used for PHI-Blast
        /// </summary>
        public string ParameterPattern { get; set; }

        /// <summary>
        /// The ENTREZ query used to limit the search
        /// </summary>
        public string ParameterEntrezQuery { get; set; }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected BlastXmlMetadata(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Program = info.GetString("BlastXmlMetadata:Program");
            Version = info.GetString("BlastXmlMetadata:Version");
            Reference = info.GetString("BlastXmlMetadata:Reference");
            Database = info.GetString("BlastXmlMetadata:Database");
            QueryId = info.GetString("BlastXmlMetadata:QueryId");
            QueryDefinition = info.GetString("BlastXmlMetadata:QueryDefinition");
            QueryLength = info.GetInt32("BlastXmlMetadata:QueryLength");
            QuerySequence = info.GetString("BlastXmlMetadata:QuerySequence");
            ParameterMatrix = info.GetString("BlastXmlMetadata:ParameterMatrix");
            ParameterExpect = info.GetDouble("BlastXmlMetadata:ParameterExpect");
            ParameterInclude = info.GetDouble("BlastXmlMetadata:ParameterInclude");
            ParameterMatchScore = info.GetInt32("BlastXmlMetadata:ParameterMatchScore");
            ParameterMismatchScore = info.GetInt32("BlastXmlMetadata:ParameterMismatchScore");
            ParameterGapOpen = info.GetInt32("BlastXmlMetadata:ParameterGapOpen");
            ParameterGapExtend = info.GetInt32("BlastXmlMetadata:ParameterGapExtend");
            ParameterFilter = info.GetString("BlastXmlMetadata:ParameterFilter");
            ParameterPattern = info.GetString("BlastXmlMetadata:ParameterPattern");
            ParameterEntrezQuery = info.GetString("BlastXmlMetadata:ParameterEntrezQuery");
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

            info.AddValue("BlastXmlMetadata:Program", Program);
            info.AddValue("BlastXmlMetadata:Version", Version);
            info.AddValue("BlastXmlMetadata:Reference", Reference);
            info.AddValue("BlastXmlMetadata:Database", Database);
            info.AddValue("BlastXmlMetadata:QueryId", QueryId);
            info.AddValue("BlastXmlMetadata:QueryDefinition", QueryDefinition);
            info.AddValue("BlastXmlMetadata:QueryLength", QueryLength);
            info.AddValue("BlastXmlMetadata:QuerySequence", QuerySequence);
            info.AddValue("BlastXmlMetadata:ParameterMatrix", ParameterMatrix);
            info.AddValue("BlastXmlMetadata:ParameterExpect", ParameterExpect);
            info.AddValue("BlastXmlMetadata:ParameterInclude", ParameterInclude);
            info.AddValue("BlastXmlMetadata:ParameterMatchScore", ParameterMatchScore);
            info.AddValue("BlastXmlMetadata:ParameterMismatchScore", ParameterMismatchScore);
            info.AddValue("BlastXmlMetadata:ParameterGapOpen", ParameterGapOpen);
            info.AddValue("BlastXmlMetadata:ParameterGapExtend", ParameterGapExtend);
            info.AddValue("BlastXmlMetadata:ParameterFilter", ParameterFilter);
            info.AddValue("BlastXmlMetadata:ParameterPattern", ParameterPattern);
            info.AddValue("BlastXmlMetadata:ParameterEntrezQuery", ParameterEntrezQuery);
        }

        #endregion
    }
}