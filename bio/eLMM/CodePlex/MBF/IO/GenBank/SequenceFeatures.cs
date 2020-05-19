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

namespace Bio.IO.GenBank
{
    /// <summary>
    /// Contains information about genes and gene products,
    /// as well as regions of biological significance reported 
    /// in the sequence.
    /// </summary>
    [Serializable]
    public class SequenceFeatures : ICloneable
    {
        #region Constructors
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SequenceFeatures()
        {
            All = new List<FeatureItem>();
        }

        /// <summary>
        /// Private Constructor for clone method.
        /// </summary>
        /// <param name="other">SequenceFeatures instance to clone.</param>
        private SequenceFeatures(SequenceFeatures other)
        {
            All = new List<FeatureItem>();
            foreach (FeatureItem feature in other.All)
            {
                All.Add(feature.Clone());
            }
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Lists all features.
        /// </summary>
        public List<FeatureItem> All { get; private set; }

        /// <summary>
        /// Returns list of Minus10Signal (-10_signal) features.
        /// </summary>
        public List<Minus10Signal> Minus10Signals
        {
            get
            {
                return All.FindAll(F => F is Minus10Signal).ConvertAll(F => F as Minus10Signal);
            }
        }

        /// <summary>
        /// Returns list of Minus35Signal (-35_signal) features.
        /// </summary>
        public List<Minus35Signal> Minus35Signals
        {
            get
            {
                return All.FindAll(F => F is Minus35Signal).ConvertAll(F => F as Minus35Signal);
            }
        }

        /// <summary>
        /// Returns list of ThreePrimeUTR (3'UTR) features.
        /// </summary>
        public List<ThreePrimeUtr> ThreePrimeUTRs
        {
            get
            {
                return All.FindAll(F => F is ThreePrimeUtr).ConvertAll(F => F as ThreePrimeUtr);
            }
        }

        /// <summary>
        /// Returns list of FivePrimeUTR (5'UTR) features.
        /// </summary>
        public List<FivePrimeUtr> FivePrimeUTRs
        {
            get
            {
                return All.FindAll(F => F is FivePrimeUtr).ConvertAll(F => F as FivePrimeUtr);
            }
        }

        /// <summary>
        /// Returns list of Attenuator features.
        /// </summary>
        public List<Attenuator> Attenuators
        {
            get
            {
                return All.FindAll(F => F is Attenuator).ConvertAll(F => F as Attenuator);
            }
        }

        /// <summary>
        /// Returns list of CAATSignal (CAAT_signal) features.
        /// </summary>
        public List<CaatSignal> CAATSignals
        {
            get
            {
                return All.FindAll(F => F is CaatSignal).ConvertAll(F => F as CaatSignal);
            }
        }

        /// <summary>
        /// Returns list of CodingSequence (CDS) features.
        /// </summary>
        public List<CodingSequence> CodingSequences
        {
            get
            {
                return All.FindAll(F => F is CodingSequence).ConvertAll(F => F as CodingSequence);
            }
        }

        /// <summary>
        /// Returns list of DisplacementLoop (D-loop) features.
        /// </summary>
        public List<DisplacementLoop> DisplacementLoops
        {
            get
            {
                return All.FindAll(F => F is DisplacementLoop).ConvertAll(F => F as DisplacementLoop);
            }
        }

        /// <summary>
        /// Returns list of Enhancer features.
        /// </summary>
        public List<Enhancer> Enhancers
        {
            get
            {
                return All.FindAll(F => F is Enhancer).ConvertAll(F => F as Enhancer);
            }
        }

        /// <summary>
        /// Returns list of Exon features.
        /// </summary>
        public List<Exon> Exons
        {
            get
            {
                return All.FindAll(F => F is Exon).ConvertAll(F => F as Exon);
            }
        }

        /// <summary>
        /// Returns list of GCSingal (GC_signal) features.
        /// </summary>
        public List<GCSingal> GCSignals
        {
            get
            {
                return All.FindAll(F => F is GCSingal).ConvertAll(F => F as GCSingal);
            }
        }

        /// <summary>
        /// Returns list of Gene features.
        /// </summary>
        public List<Gene> Genes
        {
            get
            {
                return All.FindAll(F => F is Gene).ConvertAll(F => F as Gene);
            }
        }

        /// <summary>
        /// Returns list of InterveningDNA (iDNA) features.
        /// </summary>
        public List<InterveningDNA> InterveningDNAs
        {
            get
            {
                return All.FindAll(F => F is InterveningDNA).ConvertAll(F => F as InterveningDNA);
            }
        }

        /// <summary>
        /// Returns list of Intron features.
        /// </summary>
        public List<Intron> Introns
        {
            get
            {
                return All.FindAll(F => F is Intron).ConvertAll(F => F as Intron);
            }
        }

        /// <summary>
        /// Returns list of LongTerminalRepeat (LTR) features.
        /// </summary>
        public List<LongTerminalRepeat> LongTerminalRepeats
        {
            get
            {
                return All.FindAll(F => F is LongTerminalRepeat).ConvertAll(F => F as LongTerminalRepeat);
            }
        }

        /// <summary>
        /// Returns list of MaturePeptide (mat_peptide) features.
        /// </summary>
        public List<MaturePeptide> MaturePeptides
        {
            get
            {
                return All.FindAll(F => F is MaturePeptide).ConvertAll(F => F as MaturePeptide);
            }
        }

        /// <summary>
        /// Returns list of MiscBinding (misc_binding) features.
        /// </summary>
        public List<MiscBinding> MiscBindings
        {
            get
            {
                return All.FindAll(F => F is MiscBinding).ConvertAll(F => F as MiscBinding);
            }
        }

        /// <summary>
        /// Returns list of MiscDifference (misc_difference) features.
        /// </summary>
        public List<MiscDifference> MiscDifferences
        {
            get
            {
                return All.FindAll(F => F is MiscDifference).ConvertAll(F => F as MiscDifference);
            }
        }

        /// <summary>
        /// Returns list of MiscFeature (misc_feature) features.
        /// </summary>
        public List<MiscFeature> MiscFeatures
        {
            get
            {
                return All.FindAll(F => F is MiscFeature).ConvertAll(F => F as MiscFeature);
            }
        }

        /// <summary>
        /// Returns list of MiscRecombination (misc_recomb) features.
        /// </summary>
        public List<MiscRecombination> MiscRecombinations
        {
            get
            {
                return All.FindAll(F => F is MiscRecombination).ConvertAll(F => F as MiscRecombination);
            }
        }

        /// <summary>
        /// Returns list of MiscRNA (misc_RNA) features.
        /// </summary>
        public List<MiscRNA> MiscRNAs
        {
            get
            {
                return All.FindAll(F => F is MiscRNA).ConvertAll(F => F as MiscRNA);
            }
        }

        /// <summary>
        /// Returns list of MiscSignal (misc_signal) features.
        /// </summary>
        public List<MiscSignal> MiscSignals
        {
            get
            {
                return All.FindAll(F => F is MiscSignal).ConvertAll(F => F as MiscSignal);
            }
        }

        /// <summary>
        /// Returns list of MiscStructure (misc_structure) features.
        /// </summary>
        public List<MiscStructure> MiscStructures
        {
            get
            {
                return All.FindAll(F => F is MiscStructure).ConvertAll(F => F as MiscStructure);
            }
        }

        /// <summary>
        /// Returns list of ModifiedBase (modified_base) features.
        /// </summary>
        public List<ModifiedBase> ModifiedBases
        {
            get
            {
                return All.FindAll(F => F is ModifiedBase).ConvertAll(F => F as ModifiedBase);
            }
        }

        /// <summary>
        /// Returns list of MessengerRNA (mRNA) features.
        /// </summary>
        public List<MessengerRNA> MessengerRNAs
        {
            get
            {
                return All.FindAll(F => F is MessengerRNA).ConvertAll(F => F as MessengerRNA);
            }
        }

        /// <summary>
        /// Returns list of NonCodingRNA (ncRNA) features.
        /// </summary>
        public List<NonCodingRNA> NonCodingRNAs
        {
            get
            {
                return All.FindAll(F => F is NonCodingRNA).ConvertAll(F => F as NonCodingRNA);
            }
        }

        /// <summary>
        /// Returns list of OperonRegion (Operon) features.
        /// </summary>
        public List<OperonRegion> OperonRegions
        {
            get
            {
                return All.FindAll(F => F is OperonRegion).ConvertAll(F => F as OperonRegion);
            }
        }

        /// <summary>
        /// Returns list of PolyASignal (polyA_signal) features.
        /// </summary>
        public List<PolyASignal> PolyASignals
        {
            get
            {
                return All.FindAll(F => F is PolyASignal).ConvertAll(F => F as PolyASignal);
            }
        }

        /// <summary>
        /// Returns list of PolyASite (polyA_site) features.
        /// </summary>
        public List<PolyASite> PolyASites
        {
            get
            {
                return All.FindAll(F => F is PolyASite).ConvertAll(F => F as PolyASite);
            }
        }

        /// <summary>
        /// Returns list of PrecursorRNA (precursor_RNA) features.
        /// </summary>
        public List<PrecursorRNA> PrecursorRNAs
        {
            get
            {
                return All.FindAll(F => F is PrecursorRNA).ConvertAll(F => F as PrecursorRNA);
            }
        }

        /// <summary>
        /// Returns list of Promoter features.
        /// </summary>
        public List<Promoter> Promoters
        {
            get
            {
                return All.FindAll(F => F is Promoter).ConvertAll(F => F as Promoter);
            }
        }

        /// <summary>
        /// Returns list of ProteinBindingSite (protein_bind) features.
        /// </summary>
        public List<ProteinBindingSite> ProteinBindingSites
        {
            get
            {
                return All.FindAll(F => F is ProteinBindingSite).ConvertAll(F => F as ProteinBindingSite);
            }
        }

        /// <summary>
        /// Returns list of RibosomeBindingSite (RBS) features.
        /// </summary>
        public List<RibosomeBindingSite> RibosomeBindingSites
        {
            get
            {
                return All.FindAll(F => F is RibosomeBindingSite).ConvertAll(F => F as RibosomeBindingSite);
            }
        }

        /// <summary>
        /// Returns list of ReplicationOrigin (rep_origin) features.
        /// </summary>
        public List<ReplicationOrigin> ReplicationOrigins
        {
            get
            {
                return All.FindAll(F => F is ReplicationOrigin).ConvertAll(F => F as ReplicationOrigin);
            }
        }

        /// <summary>
        /// Returns list of RepeatRegion (repeat_region) features.
        /// </summary>
        public List<RepeatRegion> RepeatRegions
        {
            get
            {
                return All.FindAll(F => F is RepeatRegion).ConvertAll(F => F as RepeatRegion);
            }
        }

        /// <summary>
        /// Returns list of RibosomalRNA (rRNA) features.
        /// </summary>
        public List<RibosomalRNA> RibosomalRNAs
        {
            get
            {
                return All.FindAll(F => F is RibosomalRNA).ConvertAll(F => F as RibosomalRNA);
            }
        }

        /// <summary>
        /// Returns list of SignalPeptide (sig_peptide) features.
        /// </summary>
        public List<SignalPeptide> SignalPeptides
        {
            get
            {
                return All.FindAll(F => F is SignalPeptide).ConvertAll(F => F as SignalPeptide);
            }
        }

        /// <summary>
        /// Returns list of StemLoop (stem_loop) features.
        /// </summary>
        public List<StemLoop> StemLoops
        {
            get
            {
                return All.FindAll(F => F is StemLoop).ConvertAll(F => F as StemLoop);
            }
        }

        /// <summary>
        /// Returns list of TATASignal (TATA_signal) features.
        /// </summary>
        public List<TataSignal> TATASignals
        {
            get
            {
                return All.FindAll(F => F is TataSignal).ConvertAll(F => F as TataSignal);
            }
        }

        /// <summary>
        /// Returns list of Terminator features.
        /// </summary>
        public List<Terminator> Terminators
        {
            get
            {
                return All.FindAll(F => F is Terminator).ConvertAll(F => F as Terminator);
            }
        }

        /// <summary>
        /// Returns list of TransferMessengerRNA (tmRNA) features.
        /// </summary>
        public List<TransferMessengerRNA> TransferMessengerRNAs
        {
            get
            {
                return All.FindAll(F => F is TransferMessengerRNA).ConvertAll(F => F as TransferMessengerRNA);
            }
        }

        /// <summary>
        /// Returns list of TransitPeptide (transit_peptide) features.
        /// </summary>
        public List<TransitPeptide> TransitPeptides
        {
            get
            {
                return All.FindAll(F => F is TransitPeptide).ConvertAll(F => F as TransitPeptide);
            }
        }

        /// <summary>
        /// Returns list of TransferRNA (tRNA) features.
        /// </summary>
        public List<TransferRNA> TransferRNAs
        {
            get
            {
                return All.FindAll(F => F is TransferRNA).ConvertAll(F => F as TransferRNA);
            }
        }

        /// <summary>
        /// Returns list of UnsureSequenceRegion (unsure) features.
        /// </summary>
        public List<UnsureSequenceRegion> UnsureSequenceRegions
        {
            get
            {
                return All.FindAll(F => F is UnsureSequenceRegion).ConvertAll(F => F as UnsureSequenceRegion);
            }
        }

        /// <summary>
        /// Returns list of Variation features.
        /// </summary>
        public List<Variation> Variations
        {
            get
            {
                return All.FindAll(F => F is Variation).ConvertAll(F => F as Variation);
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Returns list of feature items for the specified feature key.
        /// </summary>
        /// <param name="featureKey">Feature key.</param>
        /// <returns>Returns List of feature items.</returns>
        public IList<FeatureItem> GetFeatures(string featureKey)
        {
            return All.Where(F => string.Compare(F.Key, featureKey, StringComparison.InvariantCultureIgnoreCase) == 0).ToList();
        }

        /// <summary>
        /// Creates a new SequenceFeatures that is a copy of the current SequenceFeatures.
        /// </summary>
        /// <returns>A new SequenceFeatures that is a copy of this SequenceFeatures.</returns>
        public SequenceFeatures Clone()
        {
            return new SequenceFeatures(this);
        }
        #endregion Methods

        #region ICloneable Members
        /// <summary>
        /// Creates a new SequenceFeatures that is a copy of the current SequenceFeatures.
        /// </summary>
        /// <returns>A new object that is a copy of this SequenceFeatures.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion ICloneable Members
    }
}
