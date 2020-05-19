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

namespace MBF
{
    /// <summary>
    /// Implementations of IQualitativeSequence interface will hold sequence 
    /// items along with their quality scores.
    /// </summary>
    public interface IQualitativeSequence : ISequence
    {
        /// <summary>
        /// Gets the FastQFormatType of this IQualitativeSequence.
        /// </summary>
        FastQFormatType Type { get; }

        /// <summary>
        /// Gets the quality scores.
        /// </summary>
        byte[] Scores { get; }

        /// <summary>
        /// Adds a sequence item and its quality score to the end of the sequence. The Sequence
        /// must not be marked as read only in order to make this change.
        /// </summary>
        /// <param name="item">The item to add to the end of the sequence</param>
        /// <param name="qualScore">Quality score.</param>
        void Add(ISequenceItem item, byte qualScore);

        /// <summary>
        /// Indicates if the specified quality value is contained in the sequence anywhere.
        /// </summary>
        /// <param name="qualScore">Quality score to be verified.</param>
        /// <returns>If found returns true else returns false.</returns>
        bool ContainsQualityScore(byte qualScore);

        /// <summary>
        /// Inserts the specified sequence item to a specified positon in this sequence.
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be inserted.</param>
        /// <param name="item">Sequence item to be inserted.</param>
        /// <param name="qualScore">Quality score.</param>
        void Insert(int position, ISequenceItem item, byte qualScore);

        /// <summary>
        /// Inserts specified character at the specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="character">A character which indicates a sequence item.</param>
        /// <param name="qualScore">Quality score.</param>
        void Insert(int position, char character, byte qualScore);

        /// <summary>
        /// Inserts specified sequence string at specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        /// <param name="qualScore">Quality score.</param>
        void InsertRange(int position, string sequence, byte qualScore);

        /// <summary>
        /// Inserts specified sequence string at specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        /// <param name="qualScores">Quality scores.</param>
        void InsertRange(int position, string sequence, IEnumerable<byte> qualScores);

        /// <summary>
        /// Replaces the sequence item present in the specified position in this sequence 
        /// with a sequence item which is represented by specified character. 
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be replaced.</param>
        /// <param name="character">Character which represent a sequence item.</param>
        /// <param name="qualScore">Quality score.</param>
        void Replace(int position, char character, byte qualScore);

        /// <summary>
        /// Replaces the sequence item present in the specified position in this sequence with the specified sequence item. 
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be replaced.</param>
        /// <param name="item">Sequence item to be placed at the specified position.</param>
        /// /// <param name="qualScore">Quality score.</param>
        void Replace(int position, ISequenceItem item, byte qualScore);

        /// <summary>
        /// Replaces the sequence items present in the specified position in this sequence with the specified sequence.
        /// </summary>
        /// <param name="position">Position from which the replace of sequence items has to be started.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        /// <param name="qualScore">Quality score.</param>
        void ReplaceRange(int position, string sequence, byte qualScore);

        /// <summary>
        /// Replaces the sequence items present in the specified position in this sequence with the specified sequence.
        /// </summary>
        /// <param name="position">Position from which the replace of sequence items has to be started.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        /// <param name="qualScores">Quality scores.</param>
        void ReplaceRange(int position, string sequence, IEnumerable<byte> qualScores);

        /// <summary>
        /// Creates a new QualitativeSequence that is a copy of the current QualitativeSequence.
        /// </summary>
        /// <returns>A new IQualitativeSequence that is a copy of this QualitativeSequence.</returns>
        new IQualitativeSequence Clone();
    }
}
