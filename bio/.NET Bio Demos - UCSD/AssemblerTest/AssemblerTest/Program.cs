using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Bio.Algorithms.Assembly;
using Bio.Algorithms.Assembly.Padena;
using Bio.Algorithms.Alignment;
using Bio.SimilarityMatrices;
using Bio.Util.ArgumentParser;

namespace AssemblerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<ISequence> inputSequences = ParseFastA(@"c:\users\mark\desktop\simpleDna_shorts.fa");

            //var result = DoDenovoAssembly(inputSequences.ToList());
            //result.AssembledSequences.ToList()
            //    .ForEach(Console.WriteLine);
            var result = DoSimpleSequenceAssemble(inputSequences.ToList());
            result.AssembledSequences.ToList().ForEach(Console.Write);
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

        /// <summary>
        /// Method to do a denovo assembly.
        /// This sample uses Padena Denovo assembler.
        /// </summary>
        /// <param name="sequences">List of seuqnces to assemble.</param>
        /// <returns>PadenaAssembly which contain the assembled result.</returns>
        static PadenaAssembly DoDenovoAssembly(List<ISequence> sequences)
        {
            // Create a denovo assembler
            ParallelDeNovoAssembler assembler = new ParallelDeNovoAssembler();

            #region Setting assembler parameters
            // Length of kmer
            assembler.KmerLength = 6;

            // Threshold to be used for error correction step where dangling links are removed.
            // All dangling links that have lengths less than specified length will be removed.
            assembler.DanglingLinksThreshold = 3;

            // Threshold to be used for error correction step where redundant paths are removed.
            // Paths that have same start and end points (redundant paths) and whose lengths are less 
            // than specified length will be removed. They will be replaced by a single 'best' path
            assembler.RedundantPathLengthThreshold = 7;

            // Enter the name of the library along with mean distance and standard deviation
            CloneLibrary.Instance.AddLibrary("abc", (float)4, (float)5);
            #endregion

            // Assemble
            return (PadenaAssembly)assembler.Assemble(sequences);
        }

        /// <summary>
        /// Do a simple sequence assembly.
        /// This sample uses NeedlemanWunschAligner.
        /// </summary>
        /// <param name="sequences">List of sequences to assemble.</param>
        /// <returns>IDeNovoAssembly which has the assembled result.</returns>
        static IDeNovoAssembly DoSimpleSequenceAssemble(List<ISequence> sequences)
        {
            // $TODO: Change the signature and initialization after OverlapDeNovoAssembler is migrated to PhaseOne
            // Create an assembler
            OverlapDeNovoAssembler assembler = new OverlapDeNovoAssembler();

            // Setup the parameters
            // $TODO: Change the NeedlemanWunschAligner, DiagonalSimilarityMatrix, SimpleConsensusResolve initialization 
            // after OverlapDeNovoAssembler is migrated to PhaseOne
            assembler.OverlapAlgorithm = new NeedlemanWunschAligner();
            assembler.OverlapAlgorithm.SimilarityMatrix = new DiagonalSimilarityMatrix(5, -4);
            assembler.OverlapAlgorithm.GapOpenCost = -10;
            assembler.ConsensusResolver = new Bio.SimpleConsensusResolver(66);
            assembler.AssumeStandardOrientation = false;

            return assembler.Assemble(sequences);
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
