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
using Bio.Registration;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// SequenceAligners class is an abstraction class which provides instances
    /// and lists of all Aligners currently supported by MBF. 
    /// </summary>
    public static class SequenceAligners
    {
        /// <summary>
        /// A singleton instance of SmithWatermanAligner class which implements
        /// the SmithWaterman algorithm for partial alignment
        /// </summary>
        private static SmithWatermanAligner smithAlign = new SmithWatermanAligner();

        /// <summary>
        /// A singleton instance of NeedlemanWunschAligner class which implements
        /// the NeedlemanWunsch algorithm for global alignment.
        /// </summary>
        private static NeedlemanWunschAligner needlemanAlign = new NeedlemanWunschAligner();

        /// <summary>
        /// A singleton instance of PairwiseOverlapAligner class which implements
        /// pairwise overlap alignment algorithm.
        /// </summary>
        private static PairwiseOverlapAligner overlap = new PairwiseOverlapAligner();

        /// <summary>
        /// A singleton instance of MUMmer class which implements
        /// mummer alignment algorithm.
        /// </summary>
        private static MUMmer mummer3 = new MUMmer3();

        /// <summary>
        /// A singleton instance of NUCmer class which implements
        /// NUCmer alignment algorithm.
        /// </summary>
        private static NUCmer nucmer3 = new NUCmer3();

        /// <summary>
        /// List of supported sequence aligners.
        /// </summary>
        private static List<ISequenceAligner> all = new List<ISequenceAligner>() 
        { 
            SequenceAligners.smithAlign, 
            SequenceAligners.needlemanAlign, 
            SequenceAligners.overlap,
            SequenceAligners.mummer3,
            SequenceAligners.nucmer3
        };

        /// <summary>
        /// Initializes static members of the SequenceAligners class.
        /// Static constructor
        /// </summary>
        static SequenceAligners()
        {
            // Get the registered aligners
            IList<ISequenceAligner> registeredAligners = GetAligners(true);

            if (null != registeredAligners && registeredAligners.Count > 0)
            {
                foreach (ISequenceAligner aligner in registeredAligners)
                {
                    if (aligner != null && all.FirstOrDefault(IA => 
                        string.Compare(
                        IA.Name,
                        aligner.Name, 
                        StringComparison.OrdinalIgnoreCase) == 0) == null)
                    {
                        all.Add(aligner);
                    }
                }

                registeredAligners.Clear();
            }
        }

        /// <summary>
        /// Gets an instance of SmithWatermanAligner class which implements
        /// the SmithWaterman algorithm for partial alignment
        /// </summary>
        public static SmithWatermanAligner SmithWaterman
        {
            get
            {
                return smithAlign;
            }
        }

        /// <summary>
        /// Gets an instance of NeedlemanWunschAligner class which implements
        /// the NeedlemanWunsch algorithm for global alignment.
        /// </summary>
        public static NeedlemanWunschAligner NeedlemanWunsch
        {
            get
            {
                return needlemanAlign;
            }
        }

        /// <summary>
        /// Gets an instance of PairwiseOverlapAligner class which implements
        /// pairwise overlap alignment algorithm.
        /// </summary>
        public static PairwiseOverlapAligner PairwiseOverlap
        {
            get
            {
                return overlap;
            }
        }

        /// <summary>
        /// Gets an instance of MUMmer3 class which implements
        /// the MUMmer algorithm for partial alignment
        /// </summary>
        public static MUMmer MUMmer
        {
            get
            {
                return mummer3;
            }
        }

        /// <summary>
        /// Gets an instance of NUCmer3 class which implements
        /// the NUCmer algorithm for alignment
        /// </summary>
        public static NUCmer NUCmer
        {
            get
            {
                return nucmer3;
            }
        }

        /// <summary>
        /// Gets the list of all aligners which is supported by the framework.
        /// </summary>
        public static IList<ISequenceAligner> All
        {
            get
            {
                return all.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets all registered aligners in core folder and addins (optional) folders
        /// </summary>
        /// <param name="includeAddinFolder">include add-ins folder or not</param>
        /// <returns>List of registered aligners</returns>
        private static IList<ISequenceAligner> GetAligners(bool includeAddinFolder)
        {
            IList<ISequenceAligner> registeredAligners = new List<ISequenceAligner>();

            if (includeAddinFolder)
            {
                IList<ISequenceAligner> addInAligners;
                if (null != RegisteredAddIn.AddinFolderPath)
                {
                    addInAligners = RegisteredAddIn.GetInstancesFromAssemblyPath<ISequenceAligner>(RegisteredAddIn.AddinFolderPath, RegisteredAddIn.DLLFilter);
                    if (null != addInAligners && addInAligners.Count > 0)
                    {
                        foreach (ISequenceAligner aligner in addInAligners)
                        {
                            if (aligner != null &&
                                registeredAligners.FirstOrDefault(IA => string.Compare(IA.Name, aligner.Name,
                                    StringComparison.OrdinalIgnoreCase) == 0) == null)
                            {
                                registeredAligners.Add(aligner);
                            }
                        }
                    }
                }
            }
            return registeredAligners;
        }
    }
}
