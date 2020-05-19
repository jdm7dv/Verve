//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MBT.Escience;
using Bio.Util;

namespace MBT.Escience.Biology
{
    /// <summary>
    /// Summary description for Biology.
    /// </summary>
    [Serializable()]
    public class Biology
    {
        public Dictionary<string, GeneticCodeMapping> CodonToAminoAcid;
        public Dictionary<char, string> Ambiguous1LetterNucCodeToChoices;
        public Dictionary<HashableSet<char>, char> NucSetToAmbiguous1LetterNucCode;
        public Dictionary<string, char> SortedNucCodeChoicesToAmbiguous1LetterNucCode;
        public Dictionary<char, char> DnaComplementarityCode;
        public Dictionary<char, string> OneLetterAminoAcidAbbrevTo3Letter;
        public Dictionary<string, char> ThreeLetterAminoAcidAbbrevTo1Letter;
        public Dictionary<string, bool> AminoAcidCollection;
        /// <summary>
        /// Use 'Unambiguous1LetterNucSet' instead, if possible
        /// </summary>
        public Set<char> Unambiguous1LetterNucCodes;
        public HashSet<char> Unambiguous1LetterNucSet;
        public Dictionary<string, string> AminoAcidEquivalence;
        public Dictionary<string, int> AminoAcidEquivalenceToIndex;


        static public Biology GetInstance()
        {
            return _singleton.Value;
        }
        static Lazy<Biology> _singleton = new Lazy<Biology>(() => new Biology());


        private Biology()
        {
            Unambiguous1LetterNucCodes = Set<char>.GetInstance("ACGT");
            Unambiguous1LetterNucSet = new HashSet<char>("ACGT");

            ReadTheGeneticCode();
            ReadAmbiguousCode();
            ReadOneLetterAminoAcidAbbrev();
            ReadAminoAcidEquivalence();

            ReadComplementarityCode();
        }



        private void ReadAmbiguousCode()
        {
            Helper.CheckCondition(Ambiguous1LetterNucCodeToChoices == null); //!!!raise error
            Ambiguous1LetterNucCodeToChoices = new Dictionary<char, string>();
            SortedNucCodeChoicesToAmbiguous1LetterNucCode = new Dictionary<string, char>();
            NucSetToAmbiguous1LetterNucCode = new Dictionary<HashableSet<char>, char>();

            string sLine = null;
            using (StreamReader streamreaderAmbigFile = OpenResource("AmbiguousCodes.txt"))
            {
                while (null != (sLine = streamreaderAmbigFile.ReadLine()))
                {
                    string[] rgFields = sLine.Split('\t');
                    Helper.CheckCondition(rgFields.Length == 2); //!!!raise error
                    Helper.CheckCondition(rgFields[0].Length == 1); //!!!raise error
                    char cAmbiguousCode = rgFields[0][0];
                    Ambiguous1LetterNucCodeToChoices.Add(cAmbiguousCode, rgFields[1]);

                    string options = rgFields[1];
                    var optionsAsHashableSet = new HashableSet<char>(options.ToCharArray());
                    if (!NucSetToAmbiguous1LetterNucCode.ContainsKey(optionsAsHashableSet))
                    {
                        NucSetToAmbiguous1LetterNucCode.Add(optionsAsHashableSet, cAmbiguousCode);

                        string sortedNucs = new string(options.ToCharArray().OrderBy(c => c).ToArray());
                        SortedNucCodeChoicesToAmbiguous1LetterNucCode.Add(sortedNucs, cAmbiguousCode);
                    }
                }
            }
            foreach (char nuc in Unambiguous1LetterNucSet)
            {
                SortedNucCodeChoicesToAmbiguous1LetterNucCode.Add(nuc.ToString(), nuc);
            }
        }

        private void ReadComplementarityCode()
        {
            Helper.CheckCondition(DnaComplementarityCode == null); //!!!raise error
            DnaComplementarityCode = new Dictionary<char, char>(15, new SpecialFunctions.CharEqualityIgnoreCase());

            string sLine = null;
            using (StreamReader streamreaderCompFile = OpenResource("DnaComplementarity.txt"))
            {
                while (null != (sLine = streamreaderCompFile.ReadLine()))
                {
                    string[] rgFields = sLine.Split('\t');
                    Helper.CheckCondition(rgFields.Length == 2); //!!!raise error
                    Helper.CheckCondition(rgFields[0].Length == 1); //!!!raise error
                    Helper.CheckCondition(rgFields[1].Length == 1); //!!!raise error
                    char cPrimary = rgFields[0][0];
                    char cComplementary = rgFields[1][0];
                    DnaComplementarityCode.Add(cPrimary, cComplementary);
                }
            }
        }




        /// <summary>
        /// Read table from http://users.rcn.com/jkimball.ma.ultranet/BiologyPages/C/Codons.html
        /// </summary>
        private void ReadTheGeneticCode()
        {
            Debug.Assert(CodonToAminoAcid == null); // real assert
            CodonToAminoAcid = new Dictionary<string, GeneticCodeMapping>();
            AminoAcidCollection = new Dictionary<string, bool>();

            //!!!could load these into memory instead of reading them over and over again
            string sTheGenenicCodeFile = @"GeneticCodeDna.txt"; //!!!const
            using (StreamReader streamreaderTheGenenicCodeFile = OpenResource(sTheGenenicCodeFile))
            {
                string sLine = null;
                while (null != (sLine = streamreaderTheGenenicCodeFile.ReadLine()))
                {
                    string[] rgFields = sLine.Split('\t');
                    Helper.CheckCondition(rgFields.Length == 3); //!!!raise error
                    string sCodon = rgFields[0];
                    Helper.CheckCondition(sCodon.Length == 3); //!!!raise error
                    string sAminoAcid = rgFields[1];
                    bool bNormal = bool.Parse(rgFields[2]); //!!!could raise error
                    GeneticCodeMapping aGeneticCodeMapping = new GeneticCodeMapping(sCodon, sAminoAcid, bNormal);
                    CodonToAminoAcid.Add(sCodon, aGeneticCodeMapping);

                    if (!AminoAcidCollection.ContainsKey(sAminoAcid))
                    {
                        AminoAcidCollection.Add(sAminoAcid, true);
                    }
                }
            }
        }


        //!!!this code is repeated in PhyloTree and Biology
        private void ReadOneLetterAminoAcidAbbrev()
        {
            Debug.Assert(OneLetterAminoAcidAbbrevTo3Letter == null); // real assert
            Debug.Assert(ThreeLetterAminoAcidAbbrevTo1Letter == null);
            OneLetterAminoAcidAbbrevTo3Letter = new Dictionary<char, string>();
            ThreeLetterAminoAcidAbbrevTo1Letter = new Dictionary<string, char>();

            string sInputFile = @"OneLetterAAAbrev.txt"; //!!!const
            using (StreamReader streamreaderInputFile = OpenResource(sInputFile))
            {
                string sLine = null;
                while (null != (sLine = streamreaderInputFile.ReadLine()))
                {
                    string[] rgFields = sLine.Split(' ');
                    Helper.CheckCondition(rgFields.Length == 2); //!!!raise error
                    string sOneLetter = rgFields[0];
                    Helper.CheckCondition(sOneLetter.Length == 1); //!!!raise error
                    string sAminoAcid = rgFields[1];
                    OneLetterAminoAcidAbbrevTo3Letter.Add(sOneLetter[0], sAminoAcid);
                    ThreeLetterAminoAcidAbbrevTo1Letter.Add(sAminoAcid, sOneLetter[0]);
                }
            }
        }

        //!!!this could be made faster 
        //!!! should this be IsKnownAminoAcid
        public bool KnownAminoAcid(string aminoAcid)
        {
            bool b = AminoAcidCollection.ContainsKey(aminoAcid);
            return b;
        }
        //!!!could be faster and more data driver
        //!!! should this be IsKnownAminoAcid
        public bool KnownAminoAcid(char aminoAcid)
        {
            switch (aminoAcid)
            {
                case 'B':
                case 'X':
                case 'Z':
                case '*':
                case '-':
                    return false;

                default:
                    bool b = OneLetterAminoAcidAbbrevTo3Letter.ContainsKey(aminoAcid);
                    return b;
            }
        }


        /// <summary>
        /// '-' means delete and must be part of "---"
        /// ' ' means missing and is treated just like 'N'
        /// null means that it could be anything
        /// </summary>
        public SimpleAminoAcidSet GenticCode(int ifCodeHasBeenReviewedAndWorksOKWithDeleteAndSpaceAndDashAsMissingSetTo22, string codon, bool dashAsMissing)
        {
            Helper.CheckCondition(ifCodeHasBeenReviewedAndWorksOKWithDeleteAndSpaceAndDashAsMissingSetTo22 == 22, "Need to review code to be sure that it is OK to treat '---' as the amino acid DELETE");

            if (codon.Contains("X") || codon.Contains("*") || codon.Contains("?"))
            {
                return null;
            }
            if (codon.Contains("-"))
            {
                if (dashAsMissing)
                {
                    return null;
                }
                else
                {
                    codon = "---";
                }
            }


            //Get the set of values
            SimpleAminoAcidSet aminoAcidCollection = SimpleAminoAcidSet.GetInstance();
            GenticCode(codon, ref aminoAcidCollection);

            if (aminoAcidCollection.PositiveCount == 21)
            {
                return null;
            }
            return aminoAcidCollection;
        }

        public bool CanResolveToAminoAcidOrAcids(string codon)
        {
            Helper.CheckCondition(codon.Length == 3); //!!!raise error
            if (codon == "---")
            {
                return true;
            }
            for (int i = 0; i < codon.Length; ++i)
            {
                char c = codon[i];
                bool b = IsLegalNuc(c);
                if (!b)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsLegalNuc(char c)
        {
            bool b = (Ambiguous1LetterNucCodeToChoices.ContainsKey(c) || Unambiguous1LetterNucCodes.Contains(c));
            return b;
        }

        private void ReadAminoAcidEquivalence()
        {
            Helper.CheckCondition(AminoAcidEquivalence == null); //!!!raise error
            AminoAcidEquivalence = new Dictionary<string, string>();
            AminoAcidEquivalenceToIndex = new Dictionary<string, int>();

            string sInput = @"eq-classes.txt"; //!!!const
            int iEq = -1;
            using (StreamReader streamreaderInput = OpenResource(sInput))
            {
                string sTopLine = streamreaderInput.ReadLine();
                Helper.CheckCondition(sTopLine == "amino acid	letter	group	number"); //!!!raise error
                string sLine = null;
                while (null != (sLine = streamreaderInput.ReadLine()))
                {
                    string[] rgFields = sLine.Split('\t');
                    Helper.CheckCondition(rgFields.Length == 4); //!!!raise error
                    Helper.CheckCondition(rgFields[0].Length == 3); //!!!raise error
                    Helper.CheckCondition(rgFields[1].Length == 1); //!!!raise error
                    string sAminoAcid = rgFields[0];
                    string sClass = rgFields[2];
                    int iClassNumber = int.Parse(rgFields[3]); //!!!could raise error
                    AminoAcidEquivalence.Add(sAminoAcid, sClass);
                    if (!AminoAcidEquivalenceToIndex.ContainsKey(sClass))
                    {
                        ++iEq;
                        AminoAcidEquivalenceToIndex.Add(sClass, iEq + 1);
                    }
                    Debug.Assert((int)AminoAcidEquivalenceToIndex[sClass] == iClassNumber);
                }
            }
        }

        internal static StreamReader OpenResource(string fileName)
        {
#if !SILVERLIGHT
            string prefix = "MBT.Escience.Biology.DataFiles.";
#else
            string prefix = "EscienceSL.Biology.DataFiles.";
#endif

            return SpecialFunctions.OpenResource(Assembly.GetExecutingAssembly(), prefix, fileName); //!!!const
        }

        public void GenticCode(string codon, ref SimpleAminoAcidSet aminoAcidCollection)
        {
            Helper.CheckCondition(codon.Length == 3); //!!!raise error
            Debug.Assert(aminoAcidCollection != null); //real assert

            //If unambiguous, look it up and return it
            if (CodonToAminoAcid.ContainsKey(codon))
            {
                GeneticCodeMapping aGeneticCodeMapping = (GeneticCodeMapping)CodonToAminoAcid[codon];
                string sAminoAcid = aGeneticCodeMapping.AminoAcid;
                aminoAcidCollection.AddOrCheck(sAminoAcid);
                return;
            }

            //If ambiguous, try every possiblity for this 1st ambiguity and see if the results are the same (this is recursive)

            int iFirstAmbiguity = 0;
            char c = char.MinValue;
            for (; iFirstAmbiguity < codon.Length; ++iFirstAmbiguity)
            {
                c = codon[iFirstAmbiguity];
                if (Ambiguous1LetterNucCodeToChoices.ContainsKey(c) && Ambiguous1LetterNucCodeToChoices[c].Length > 1)
                {
                    break;
                }
                Helper.CheckCondition("ATCG".Contains(c.ToString()), string.Format("Illegal nucleotide of '{0}'", c));
            }
            Helper.CheckCondition(iFirstAmbiguity < codon.Length); //!!!raise error - Is CodonToAminoAcid table missing a value?

            foreach (char cNucleotide in (string)Ambiguous1LetterNucCodeToChoices[c])
            {
                string sNewCodon = string.Format("{0}{1}{2}", codon.Substring(0, iFirstAmbiguity), cNucleotide, codon.Substring(iFirstAmbiguity + 1));
                Debug.Assert(sNewCodon.Length == 3); //real assert
                Debug.Assert(sNewCodon != codon); // real assert
                GenticCode(sNewCodon, ref aminoAcidCollection);
            }
        }

        public string GenticCode(string codon, out bool bAcidOrStop)
        {
            Helper.CheckCondition(codon.Length == 3); //!!!raise error

            //If unambiguous, look it up and return it
            if (CodonToAminoAcid.ContainsKey(codon))
            {
                GeneticCodeMapping aGeneticCodeMapping = (GeneticCodeMapping)CodonToAminoAcid[codon];
                string sAminoAcid = aGeneticCodeMapping.AminoAcid;
                bAcidOrStop = true;
                return sAminoAcid;
            }

            //If ambiguous, try every possiblity for this 1st ambiguity and see if the results are the same (this is recursive)
            for (int i = 0; i < codon.Length; ++i)
            {
                char c = codon[i];
                if (Ambiguous1LetterNucCodeToChoices.ContainsKey(c))
                {
                    string sAminoAcidAll = null;
                    bAcidOrStop = false;
                    foreach (char cNucleotide in (string)Ambiguous1LetterNucCodeToChoices[c])
                    {
                        string sNewCodon = string.Format("{0}{1}{2}", codon.Substring(0, i), cNucleotide, codon.Substring(i + 1));
                        Debug.Assert(sNewCodon.Length == 3); //real assert
                        Debug.Assert(sNewCodon != codon); // real assert
                        string sAminoAcid = GenticCode(sNewCodon, out bAcidOrStop);
                        if (!bAcidOrStop)
                        {
                            goto CantFixAmbiguity;
                        }
                        if (sAminoAcidAll == null)
                        {
                            sAminoAcidAll = sAminoAcid;
                        }
                        else
                        {
                            if (sAminoAcidAll != sAminoAcid)
                            {
                                //goto CantFixAmbiguity;
                                sAminoAcidAll = "Ambiguous Amino Acid"; //!!!const
                            }
                        }
                    }
                    Debug.Assert(sAminoAcidAll != null); //!!!???
                    return sAminoAcidAll;
                }
            }

        CantFixAmbiguity:

            bAcidOrStop = false;
            return "Not an amino acid: '" + codon + "'";
        }






        public string ConvertToOneLetterSequence(string sSequence3Letter)
        {
            string[] threeLetterArray = sSequence3Letter.Split(',');
            return ConvertToOneLetterSequence(threeLetterArray);
        }

        public string ConvertToOneLetterSequence(string[] threeLetterArray)
        {
            StringBuilder aStringBuilder = new StringBuilder(threeLetterArray.Length, threeLetterArray.Length);
            foreach (string threeLetterAA in threeLetterArray)
            {
                char c = (char)ThreeLetterAminoAcidAbbrevTo1Letter[threeLetterAA];
                aStringBuilder.Append(c);
            }

            string sSequence1Letter = aStringBuilder.ToString();
            return sSequence1Letter;
        }


        //!!!should this be IsLegalPeptide  ???
        public bool LegalPeptide(string peptide)
        {
            foreach (char c in peptide)
            {
                if (!KnownAminoAcid(c))
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<char> NucCodeExpandAsNeeded(char nuc, bool dashIsOK)
        {
            if (Unambiguous1LetterNucCodes.Contains(nuc))
            {
                yield return nuc;
            }
            else if (nuc == '-')
            {
                Helper.CheckCondition(dashIsOK, "Nuc '-' found, but not OK");
                yield return nuc;
            }
            else
            {
                foreach (char c in Ambiguous1LetterNucCodeToChoices[nuc])
                {
                    yield return c;
                }
            }
        }

        public string OneLetterAmbOrUnToChoices(char ambOrUnNuc)
        {
            if (Unambiguous1LetterNucSet.Contains(ambOrUnNuc))
            {
                return ambOrUnNuc.ToString();
            }
            else
            {
                return Ambiguous1LetterNucCodeToChoices[ambOrUnNuc];
            }

        }
    }
}
