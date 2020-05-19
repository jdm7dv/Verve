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



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bio
{
    /// <summary>
    /// Calculate the consensus for a list of symbols using simple frequency fraction method.
    /// Normal (non-gap) symbols are given a weight of 100. 
    /// The confidence of a symbol is the sum of weights for that symbol, 
    /// divided by the total number of symbols occuring at that position. 
    /// If symbols have confidence >= threshold, symbol corresponding 
    /// to set of these high confidence symbols is used.
    /// If no symbol meets the threshold, symbol corresponding 
    /// to set of all the symbols at that position is used.
    /// <para>
    /// For ambiguous symbols, the corresponding set of base symbols are retrived.
    /// And for frequency calculation, each base symbol is given a weight of 
    /// (100 / number of base symbols).
    /// </para>
    /// </summary>
    public class SimpleConsensusResolver : IConsensusResolver
    {
        /// <summary>
        /// Initializes a new instance of the SimpleConsensusResolver class.
        /// Sets user parameter threshold.
        /// </summary>
        /// <param name="threshold">Threshold Value</param>
        public SimpleConsensusResolver(double threshold)
        {
            Threshold = threshold;
        }

        /// <summary>
        /// Initializes a new instance of the SimpleConsensusResolver class.
        /// </summary>
        /// <param name="seqAlphabet">Sequence Alphabet</param>
        /// <param name="threshold">Threshold Value</param>
        public SimpleConsensusResolver(IAlphabet seqAlphabet, double threshold)
        {
            SequenceAlphabet = seqAlphabet;
            Threshold = threshold;
        }

        /// <summary>
        /// Initializes a new instance of the SimpleConsensusResolver class.
        /// Sets default value for threshold.
        /// </summary>
        /// <param name="seqAlphabet">Sequence Alphabet</param>
        public SimpleConsensusResolver(IAlphabet seqAlphabet)
        {
            SequenceAlphabet = seqAlphabet;
            Threshold = 99;
        }

        /// <summary>
        /// Gets or sets sequence alphabet
        /// </summary>
        public IAlphabet SequenceAlphabet { get; set; }

        /// <summary>
        /// Gets or sets threshold value - used when generating consensus symbol
        /// The confidence level for a position must equal or exceed Threshold for
        /// a non-gap symbol to appear in the consensus at that position.
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// Gets consensus symbols for the input list, 
        /// using frequency fraction method.
        /// Refer class summary for more details.
        /// </summary>
        /// <param name="items">List of input symbols</param>
        /// <returns>Consensus Symbol</returns>
        public ISequenceItem GetConsensus(List<ISequenceItem> items)
        {
            if (SequenceAlphabet == null)
            {
                throw new Exception(Properties.Resource.ALPHABET_NULL);
            }

            if (items.Count == 0)
            {
                throw new ArgumentException(Properties.Resource.LIST_EMPTY);
            }

            Dictionary<ISequenceItem, double> symbolFrequency = new Dictionary<ISequenceItem, double>();
            int symbolsCount = 0; 

            foreach (ISequenceItem item in items)
            {
                if (item.IsGap)
                {
                    // ignore gaps
                    continue; 
                }

                symbolsCount++;
                if (item.IsAmbiguous)
                {
                    HashSet<ISequenceItem> baseSymbols = SequenceAlphabet.GetBasicSymbols(item);
                    double baseProbability = 1 / (double) baseSymbols.Count;
                    foreach (ISequenceItem s in baseSymbols)
                    {
                        symbolFrequency[s] =
                            (symbolFrequency.ContainsKey(s) ? symbolFrequency[s] : 0) + baseProbability;
                    }
                }
                else
                {
                    symbolFrequency[item] =
                    (symbolFrequency.ContainsKey(item) ? symbolFrequency[item] : 0) + 1;
                }
            }

            if (symbolsCount == 0)
            {
                // All symbols were gaps
                return SequenceAlphabet.DefaultGap;
            }
            else
            {
                // Check which characters are above threshold
                HashSet<ISequenceItem> aboveThresholdSymbols = new HashSet<ISequenceItem>(); 
                double frequency;
                
                foreach (KeyValuePair<ISequenceItem, double> item in symbolFrequency)
                {
                    frequency = (item.Value * 100) / ((double) symbolsCount);
                    if (frequency > Threshold)
                    {
                        aboveThresholdSymbols.Add(item.Key);
                    }
                }

                // If there are characters above threshold, consider those characters for consensus
                // Else, consider all characters 
                if (aboveThresholdSymbols.Count > 0)
                {
                    return SequenceAlphabet.GetConsensusSymbol(aboveThresholdSymbols);
                }
                else
                {
                    return SequenceAlphabet.GetConsensusSymbol(new HashSet<ISequenceItem>(symbolFrequency.Keys));
                }
            }
        }
    }
}
