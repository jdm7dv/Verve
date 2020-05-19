using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Bio.Algorithms.Alignment;
using Bio.SimilarityMatrices;
using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
using Bio.Util.ArgumentParser;

namespace AlgorithmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ISequence refSequence = ParseFastA(@"c:\users\mark\desktop\data\E.coli.refseq.fasta").First();
            ISequence querySequence = ParseFastA(@"c:\users\mark\desktop\data\5S.A.fasta").Skip(5).First();

            var results = AlignSequences(refSequence, querySequence)[0] as PairwiseSequenceAlignment;

            Console.WriteLine("1: {0}", refSequence);
            Console.WriteLine("2: {0}", querySequence);
            Console.WriteLine("C: {0}", results.AlignedSequences[0]);
        }

        /// <summary>
        /// Exports a given sequence to a file in FastA format
        /// </summary>
        /// <param name="sequence">Sequence to be exported.</param>
        /// <param name="filename">Target filename.</param>
        static void ExportFastA(ISequence sequence, string filename)
        {
            // A formatter to export the output
            FastAFormatter formatter = new FastAFormatter(filename);

            // Exports the sequence to a file
            formatter.Write(sequence);
        }

        /// <summary>
        /// Exports a given list of sequences to a file in FastA format
        /// </summary>
        /// <param name="sequences">List of Sequences to be exported.</param>
        /// <param name="filename">Target filename.</param>
        static void ExportFastA(ICollection<ISequence> sequences, string filename)
        {
            // A formatter to export the output
            FastAFormatter formatter = new FastAFormatter(filename);

            // Exports the sequences to a file
            formatter.Write(sequences);
        }

        /// <summary>
        /// Parses a FastA file which has one or more sequences.
        /// </summary>
        /// <param name="filename">Path to the file to be parsed.</param>
        /// <returns>ISequence objects</returns>
        static IEnumerable<ISequence> ParseFastA(string filename)
        {
            // A new parser to import a file
            FastAParser parser = new FastAParser(filename);
            return parser.Parse();
        }

        // $TODO: Change the above namespace after PhaseOne changes
        /// <summary>
        /// Aligns multiple sequences using a multiple sequence aligner.
        /// This sample uses PAMSAM with a set of default parameters.
        /// </summary>
        /// <param name="sequences">List of sequences to align.</param>
        /// <returns>List of ISequenceAlignment</returns>
        static IList<ISequence> DoMultipleSequenceAlignment(List<ISequence> sequences)
        {
            // $TODO: Change the signature after PAMSAM PhaseOne is checked in

            // Initialise objects for constructor
            // $TODO: Change this after PAMSAM PhaseOne is checked in
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            int gapOpenPenalty = -4;
            int gapExtendPenalty = -1;
            int kmerLength = 3;

            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;

            // Call aligner
            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
                (sequences, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
                Environment.ProcessorCount * 2, Environment.ProcessorCount);

            return msa.AlignedSequences;
        }

        /// <summary>
        /// Method to align two sequences, this sample code uses NeedlemanWunschAligner.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence for alignment</param>
        /// <param name="querySequence">Query sequence for alignment</param>
        /// <returns>List of IPairwiseSequenceAlignment</returns>
        public static IList<IPairwiseSequenceAlignment> AlignSequences(ISequence referenceSequence, ISequence querySequence)
        {
            // Initialize the Aligner
            NeedlemanWunschAligner aligner = 
                new NeedlemanWunschAligner();

            aligner.GapExtensionCost = -10;
            aligner.GapOpenCost = -20;
            //aligner.SimilarityMatrix = new SimilarityMatrix(Bio.SimilarityMatrices.SimilarityMatrix.StandardSimilarityMatrix.AmbiguousRna);

            // Calling align method to do the alignment.
            return aligner.Align(referenceSequence, querySequence);
        }
    }

    public class Parameters
    {
        /// <summary>
        /// Contains the input file name to be parsed.
        /// </summary>
        public string InputFile;

        /// <summary>
        /// Contains the output file name where the sequences are to be exported.
        /// </summary>
        public string OutputFile;
    }

}
