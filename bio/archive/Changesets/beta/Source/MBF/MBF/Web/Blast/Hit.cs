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

namespace MBF.Web.Blast
{
    /// <summary>
    /// A database sequence with high similarity to the query sequence.
    /// </summary>
    [Serializable]
    public class Hit : ISerializable
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

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected Hit(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _hsps = (IList<Hsp>)info.GetValue("Hit:Hsps", typeof(IList<Hsp>));
            Id = info.GetString("Hit:Id");
            Def = info.GetString("Hit:Def");
            Accession = info.GetString("Hit:Accession");
            Length = info.GetInt64("Hit:Length");
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

            info.AddValue("Hit:Hsps", _hsps);
            info.AddValue("Hit:Id", Id);
            info.AddValue("Hit:Def", Def);
            info.AddValue("Hit:Accession", Accession);
            info.AddValue("Hit:Length", Length);
        }

        #endregion
    }
}