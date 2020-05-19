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
using System.IO;
using System.Text;

using MBF;
using MBF.IO;
using MBF.IO.Fasta;
using MBF.IO.GenBank;

namespace ReadGenerator
{
    /// <summary>
    /// Provides the glue logic between the UI and the data model
    /// </summary>
    internal class SimulatorController
    {
        private ISequence sequence;
        private List<ISequence> results;
        private Random random = new Random();

        /// <summary>
        /// The loaded in sequence to be split
        /// </summary>
        internal ISequence Sequence
        {
            get { return sequence; }
            set { sequence = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        internal SimulatorController()
        {

        }

        /// <summary>
        /// Parses a sequence given a file name. Uses built in mechanisms to detect the
        /// appropriate parser based on the file name.
        /// </summary>
        /// <param name="fileName">The path of the file to be parsed for a sequence</param>
        internal void ParseSequence(string fileName)
        {
            ISequenceParser parser = SequenceParsers.FindParserByFile(fileName);
            if (parser == null)
                throw new ArgumentException("Could not find an appropriate parser for " + fileName);

            Sequence = parser.ParseOne(fileName);
            if (Sequence == null)
                throw new ArgumentException("Unable to parse a sequence from file " + fileName);
        }

        /// <summary>
        /// Does the logic behind the sequence simulation
        /// </summary>
        internal void DoSimulation(SimulatorWindow window, string outputFileName, SimulatorSettings settings)
        {
            FileInfo file = new FileInfo(outputFileName);
            if (!file.Directory.Exists)
                throw new ArgumentException("Could not write to the output directory for " + outputFileName);

            if(settings.OutputSequenceCount <=0)
                throw new ArgumentException("'Max Output Sequences Per File' should be greater than zero.");

            if (settings.SequenceLength <= 0)
                throw new ArgumentException("'Mean Output Length' should be greater than zero.");

            string filePrefix;
            if (String.IsNullOrEmpty(file.Extension))
                filePrefix = file.FullName;
            else
                filePrefix = file.FullName.Substring(0, file.FullName.IndexOf(file.Extension));

            string filePostfix = "_{0}.fa";

            int seqCount = (settings.DepthOfCoverage * Sequence.Count) / settings.SequenceLength;
            int fileCount = seqCount / settings.OutputSequenceCount;
            if (seqCount % settings.OutputSequenceCount!= 0)
                fileCount++;

            window.UpdateSimulationStats(seqCount, fileCount);

            if (results == null)
                results = new List<ISequence>();
            else
                results.Clear();

            int fileIndex = 1;
            FastaFormatter formatter = new FastaFormatter();

            for (int i = 0; i < seqCount; i++)
            {
                results.Add(CreateSubsequence(settings, i));

                if (results.Count >= settings.OutputSequenceCount)
                {
                    FileInfo outFile = new FileInfo(filePrefix + string.Format(filePostfix, fileIndex++));
                    formatter.Format(results, outFile.FullName);
                    results.Clear();
                }
            }

            if (results.Count > 0)
            {
                FileInfo outFile = new FileInfo(filePrefix + string.Format(filePostfix, fileIndex++));
                formatter.Format(results, outFile.FullName);
            }

            window.NotifySimulationComplete();
        }

        // Creates a subsequence from a source sequence given the settings provided
        private ISequence CreateSubsequence(SimulatorSettings settings, int index)
        {
            double err = (double)settings.ErrorFrequency;

            // Set the length using the appropriate random number distribution type
            int subLength = settings.SequenceLength;
            if (settings.DistributionType == (int)Distribution.Uniform)
            {
                subLength += random.Next(settings.LengthVariation * 2) - settings.LengthVariation;
            }
            else if (settings.DistributionType == (int)Distribution.Normal)
            {
                subLength = (int)MBF.Util.Helper.GetNormalRandom((double)settings.SequenceLength,
                    (double)settings.LengthVariation);
            }

            // Quick sanity checks on the length of the subsequence
            if (subLength <= 0)
                subLength = 1;

            if (subLength > Sequence.Count)
                subLength = Sequence.Count;

            // Set the start position
            int startPosition = random.Next(Sequence.Count - subLength);

            Sequence result = new Sequence(Sequence.Alphabet);
            result.IsReadOnly = false;
            result.ID = Sequence.ID + " (Split " + (index + 1) + ")";

            List<ISequenceItem> errorSource = Sequence.Alphabet.LookupAll(true, false, settings.AllowAmbiguities, false);

            for (int i = 0; i < subLength; i++)
            {
                // Apply Errors if applicable
                if (random.NextDouble() < err)
                {
                    result.Add(errorSource[random.Next(errorSource.Count - 1)]);
                }
                else
                {
                    result.Add(Sequence[startPosition + i]);
                }
            }

            // Reverse Sequence if applicable
            if (settings.ReverseHalf && random.NextDouble() < 0.5f)
            {
                return result.Reverse;
            }

            return result;
        }
    }
}
