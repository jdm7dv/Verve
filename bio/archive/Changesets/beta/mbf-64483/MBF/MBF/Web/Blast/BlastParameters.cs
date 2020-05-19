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
using System.Globalization;
using MBF.Util.Logging;

namespace MBF.Web.Blast
{
    /// <summary>
    /// The parameter collection for the NCBI BLAST web service. Consists of a static set of
    /// allowed parameters and validation methods, and a collection of parameter/value
    /// pairs that have been validated and added to the instance.
    /// </summary>
    public class BlastParameters
    {
        #region Member Variables

        /// <summary>
        /// List of request parameter
        /// </summary>
        private static Dictionary<string, RequestParameter> _parameters = new Dictionary<string, RequestParameter>();

        /// <summary>
        /// List of configuration settings
        /// </summary>
        private Dictionary<string, string> _settings = new Dictionary<string, string>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the BlastParameters class.
        /// The static constructor defines the initial set of allowed parameters and values.
        /// </summary>
        /// <remarks>
        /// The following QBLAST parameters are not implemented, as they aren't applicable to
        /// the BLAST usage supported by this library (often because they aren't available in
        /// the XML BLAST results):
        /// ALIGNMENT_VIEW
        /// AUTO_FORMAT
        /// DESCRIPTIONS
        /// ENDPOINTS
        /// ENTREZ_LINKS_NEW_WINDOW
        /// FORMAT_ENTREZ_QUERY
        /// LAYOUT
        /// OTHER_ADVANCED
        /// PAGE
        /// QUERY_FILE
        /// RESULTS_FILE
        /// SHOW_OVERVIEW
        /// FORMAT_OBJECT
        /// NOHEADER
        /// NCBI_GI
        /// </remarks>
        static BlastParameters()
        {
            _parameters.Add(
                "Alignments",
                new RequestParameter(
                    "ALIGNMENTS",
                    "Number of alignments to return",
                    false,
                    "500",
                    "int",
                    new IntRangeValidator(1, 1000)));
            _parameters.Add(
                "Command",
                new RequestParameter(
                    "CMD",
                    "Command to execute",
                    true,
                    string.Empty,
                    "string",
                    new StringListValidator("Web", "Put", "Get", "Delete", "Info")));
            _parameters.Add(
                "CompositionBasedStatistics",
                new RequestParameter(
                    "COMPOSITION_BASED_STATISTICS",
                    "Type of composition based statistics to apply",
                    false,
                    "0",
                    "int",
                    new IntRangeValidator(0, 3)));
            _parameters.Add(
                "Database",
                new RequestParameter(
                    "DATABASE",
                    "Database name",
                    false,
                    "nt",
                    "string",
                    null));
            _parameters.Add(
                "Program",
                new RequestParameter(
                    "PROGRAM",
                    "Blast program name",
                    false,
                    "blastn",
                    "string",
                    new StringListValidator(
                        true,   // ignoreCase
                        "blastn", "blastp", "blastx", "tblastn", "tblastx")));
            _parameters.Add(
                "Query",
                new RequestParameter(
                    "QUERY",
                    "Query sequence",
                    false,
                    string.Empty,
                    "string",
                    null));
            _parameters.Add(
                "FormatType",
                new RequestParameter(
                    "FORMAT_TYPE",
                    "Type of data to return",
                    false,
                    string.Empty,
                    "string",
                    null));
            _parameters.Add(
                "EntrezQuery",
                new RequestParameter(
                    "ENTREZ_QUERY",
                    "Entrez query to limit BLAST search",
                    false,
                    string.Empty,
                    "string",
                    null));
            _parameters.Add(
                "Expect",
                new RequestParameter(
                    "EXPECT",
                    "Expect value",
                    false,
                    "10.0",
                    "double",
                    new DoubleRangeValidator(0, 1e100)));
            _parameters.Add(
                "ExpectLow",
                new RequestParameter(
                    "EXPECT_LOW",
                    "Expect lower threshold for formatting",
                    false,
                    "0",
                    "double",
                    new DoubleRangeValidator(0, 1e100)));
            _parameters.Add(
                "ExpectHigh",
                new RequestParameter(
                    "EXPECT_HIGH",
                    "Expect higher threshold for formatting",
                    false,
                    "1e100",
                    "double",
                    new DoubleRangeValidator(0, 1e100)));

            // Allowed Filter values are different between QBlast and WUBlast, so 
            // can't validate here.
            _parameters.Add(
                "Filter",
                new RequestParameter(
                    "FILTER",
                    "Sequence filter identifier",
                    false,
                    string.Empty,
                    "string",
                    null));
            _parameters.Add(
                "RequestIdentifier",
                new RequestParameter(
                    "RID",
                    "Identifier for stored request",
                    false,
                    "false",
                    "string",
                    null));
            _parameters.Add(
                "GapCosts",
                new RequestParameter(
                    "GAPCOSTS",
                    "Gap open and gap extend costs",
                    false,
                    "5 2",
                    "string",
                    null));
            _parameters.Add(
                "GeneticCode",
                new RequestParameter(
                    "GENETIC_CODE",
                    "Query genetic code",
                    false,
                    "1",
                    "int",
                    new IntRangeValidator(1, 22)));
            _parameters.Add(
                "HitlistSize",
                new RequestParameter(
                    "HITLIST_SIZE",
                    "Number of hits to keep",
                    false,
                    "500",
                    "int",
                    new IntRangeValidator(1, 100000)));
            _parameters.Add(
                "IThreshold",
                new RequestParameter(
                    "I_THRESH",
                    "Threshold for extending hits (PSI BLAST only)",
                    false,
                    "0.001",
                    "double",
                    null));
            _parameters.Add(
                "LowercaseMask",
                new RequestParameter(
                    "LCASE_MASK",
                    "Enable masking of lower case in query",
                    false,
                    "no",
                    "string",
                    new StringListValidator("yes", "no")));
            _parameters.Add(
                "MatrixName",
                new RequestParameter(
                    "MATRIX_NAME",
                    "Matrix name (protein search only)",
                    false,
                    "BLOSUM62",
                    "string",
                    null));
            _parameters.Add(
                "NucleotideMismatchPenalty",
                new RequestParameter(
                    "NUCL_PENALTY",
                    "Penalty for a nucleotide mismatch (blastn only)",
                    false,
                    "-3",
                    "int",
                    new IntRangeValidator(-100000, 0)));
            _parameters.Add(
                "NucleotideMatchReward",
                new RequestParameter(
                    "NUCL_REWARD",
                    "Reward for a nucleotide match (blastn only)",
                    false,
                    "1",
                    "int",
                    null));
            _parameters.Add(
                "PhiPattern",
                new RequestParameter(
                    "PHI_PATTERN",
                    "Phi Blast pattern",
                    false,
                    string.Empty,
                    "string",
                    null));
            _parameters.Add(
                "Pssm",
                new RequestParameter(
                    "PSSM",
                    "PSI BLAST checkpoint",
                    false,
                    string.Empty,
                    "string",
                    null));
            _parameters.Add(
                "QueryBelieveDefline",
                new RequestParameter(
                    "QUERY_BELIEVE_DEFLINE",
                    "Whether to believe defline in FASTA query",
                    false,
                    "no",
                    "string",
                    new StringListValidator("yes", "no")));
            _parameters.Add(
                "QueryFrom",
                new RequestParameter(
                    "QUERY_FROM",
                    "Start of subsequence (one offset)",
                    false,
                    "0",
                    "int",
                    null));
            _parameters.Add(
                "QueryTo",
                new RequestParameter(
                    "QUERY_TO",
                    "End of subsequence (one offset) - zero means ignore",
                    false,
                    "0",
                    "int",
                    null));
            _parameters.Add(
                "EffectiveSearchSpace",
                new RequestParameter(
                    "SEARCHSP_EFF",
                    "Effective length of the search space",
                    false,
                    "0",
                    "int",
                    null));
            _parameters.Add(
                "Service",
                new RequestParameter(
                    "SERVICE",
                    "Blast service which needs to be performed",
                    false,
                    "plain",
                    "string",
                    new StringListValidator("plain", "psi", "phi", "rpsblast", "megablast")));
            _parameters.Add(
                "Threshold",
                new RequestParameter(
                    "THRESHOLD",
                    "Threshold for extending hits",
                    false,
                    "0",
                    "int",
                    null));
            _parameters.Add(
                "UngappedAlignment",
                new RequestParameter(
                    "UNGAPPED_ALIGNMENT",
                    "Whether to perform an ungapped alignment",
                    false,
                    "no",
                    "string",
                    new StringListValidator("yes", "no")));
            _parameters.Add(
                "WordSize",
                new RequestParameter(
                    "WORD_SIZE",
                    "Word size - default is 3 for proteins, 11 for nuc-nuc, 28 for megablast",
                    false,
                    "7",
                    "int",
                    null));
            _parameters.Add(
                "Email",
                new RequestParameter(
                    "EMAIL",
                    "Email address for reporting job problems",
                    false,
                    string.Empty,
                    "string",
                    null));
            _parameters.Add(
                "Strand",
                new RequestParameter(
                    "STRAND",
                    "Which strand of DNA should be searched",
                    false,
                    "both",
                    "string",
                    new StringListValidator("both", "top", "bottom")));
            _parameters.Add(
                "Sensitivity",
                new RequestParameter(
                    "SENSITIVITY",
                    "Search sensitivity setting",
                    false,
                    "normal",
                    "string",
                    new StringListValidator("vlow", "low", "medium", "normal", "high")));

            // extra parameteres for BioHPC BLAST
            _parameters.Add(
                "MinQueryLength",
                new RequestParameter(
                    "MINQUERYLENGTH",
                    "Query sequences shorter than this will not be considered",
                    false,
                    "15",
                    "int",
                    null));
            _parameters.Add(
                "EmailNotify",
                new RequestParameter(
                    "EMAILNOTIFY",
                    "Send e-mail notifications about the job",
                    false,
                    "no",
                    "string",
                    new StringListValidator("yes", "no")));
            _parameters.Add(
                "JobName",
                new RequestParameter(
                    "JOBNAME",
                    "Name of the BioHPC BLAST job",
                    false,
                    "svcBLASTjob",
                    "string",
                    null));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the various parameters required for a BLAST service and the
        /// possible values for those parameters.
        /// </summary>
        public static Dictionary<string, RequestParameter> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        /// <summary>
        /// Gets or sets settings is the collection of parameter/value pairs that
        /// have been validated and added to this instance. 
        /// </summary>
        public Dictionary<string, string> Settings
        {
            get { return _settings; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds parameters to the settings if they are not present.
        /// Note that this method does validation of paramters before adding.
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="parameterValue">The parameter value</param>
        public void AddIfAbsent(string parameterName, string parameterValue)
        {
            Add(parameterName, parameterValue, false);
        }

        /// <summary>
        /// Validate a parameter/value pair, and add them to Settings,
        /// replacing any value already present for that parameter.
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="parameterValue">The parameter value</param>
        public void Add(string parameterName, string parameterValue)
        {
            Add(parameterName, parameterValue, true);  // overwrites any previous setting
        }

        /// <summary>
        /// Validate a parameter/value pair, then add it to the Settings collection. 
        /// The overwrite value determines whether the new value can overwrite an
        /// existing value for the same parameter.
        /// </summary>
        /// <param name="paramName">The parameter to set</param>
        /// <param name="paramValue">The parameter value</param>
        /// <param name="overwrite">If true, overwrite any existing value.</param>
        private void Add(string paramName, string paramValue, bool overwrite)
        {
            if (!_parameters.ContainsKey(paramName))
            {
                string message = String.Format(CultureInfo.InvariantCulture,
                        "NcbiBlastParameters: Unknown parameter name {0}.",
                        paramName);
                Trace.Report(message);
                throw new Exception(message);
            }

            RequestParameter param = _parameters[paramName];
            if (!param.IsValid(paramValue))
            {
                string message = String.Format(CultureInfo.InvariantCulture,
                        "NcbiBlastParameters: Invalid parameter value {0} for parameter {0}.",
                        paramValue,
                        paramName);
                Trace.Report(message);
                throw new Exception(message);
            }

            if (Settings.ContainsKey(param.SubmitName))
            {
                if (overwrite)
                {
                    Settings[param.SubmitName] = paramValue;
                }

                // else leave existing value
            }
            else
            {
                Settings.Add(param.SubmitName, paramValue);
            }
        }

        #endregion
    }
}
