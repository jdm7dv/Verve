// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.Web.Blast
{
    /// <summary>
    /// A database sequence with high similarity to the query sequence.
    /// </summary>
    public class Hit
    {
        /// <summary>
        /// list of HSPs returned for this Hit.
        /// </summary>
        private IList<Hsp> _hsps = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Hit()
        {
            _hsps = new List<Hsp>();
        }

        /// <summary>
        /// The string identifying the hit sequence
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The "defline" or definition line for the hit
        /// </summary>
        public string Def { get; set; }

        /// <summary>
        /// The accession number of the hit, as string
        /// </summary>
        public string Accession { get; set; }

        /// <summary>
        /// The length of the hit sequence
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// The list of HSPs returned for this Hit.
        /// </summary>
        public IList<Hsp> Hsps 
        {
            get { return _hsps; }
        }
    }
}