// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * EncodingBVTTestCases.cs
 * 
 * This file contains the Encoding BVT Test Cases which includes Encoding Map,
 * Sequence decoder and Sequence encoder.
 * 
******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

using MBF.Encoding;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.Encoding
{
    /// <summary>
    /// Test Automation code for MBF Encodings and BVT level validations.
    /// </summary>
    [TestFixture]
    public class EncodingBvtTestCases
    {

        #region Global variables

        EncodingMap _mapToEncoding;
        EncodingMap _mapToAlphaBet;

        #endregion Global variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static EncodingBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region Encoding Bvt TestCases

        /// <summary>
        /// Convert a Valid Alphabet item to an encoded item.
        /// Input Data : Valid DNA Sequence with single character - "A".
        /// Output Data : Encoded byte value of an IsequenceItem - "65".
        /// </summary>
        [Test]
        public void ValidateAlphabetToEncodedItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.EncodingSimpleDna,
                Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.EncodingSimpleDna,
                Constants.ExpectedSequence);
            string encodedValue = Utility._xmlUtil.GetTextValue(Constants.EncodingSimpleDna,
                Constants.IupacNaEncodedValue);

            string[] encodedValues = encodedValue.Split(',');
            IAlphabet alphabet = Alphabets.DNA;
            IEncoding encodings = Encodings.IupacNA;
            ISequenceItem convertedIseqItem;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: The Sequence {0} is expected.", actualSequence));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);
            this._mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);

            int index = 0;
            foreach (ISequenceItem item in createSequence)
            {
                // Convert that Sequence items into encoded items
                convertedIseqItem = _mapToEncoding.Convert(item);

                // Validate the Converted IsequenceItem
                Assert.AreEqual(encodedValues[index],
                    convertedIseqItem.Value.ToString((IFormatProvider)null));
                ApplicationLog.WriteLine(string.Format(null,
                    "Encoding BVT: Encoded value {0} is expected.", convertedIseqItem.Value));
                index++;
            }
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of sequence item to encoded item is completed successfully.");
        }

        /// <summary>
        /// Convert a Valid encoded item to an Alphabet item.
        /// Input Data : Encoded DNA Alphabet Item - "65".
        /// Output Data : Alphabet Item - "A".
        /// </summary>
        [Test]
        public void ValidateEncodedItemToAlphabetItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedSingleChar);
            IAlphabet alphabet = Alphabets.DNA;
            IEncoding encode = Encodings.IupacNA;
            ISequenceItem encodedSeqItem;
            ISequenceItem alphabetItem;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:The Sequence {0} is expected.", actualSequence));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);
            this._mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encode);
            this._mapToAlphaBet = EncodingMap.GetMapToAlphabet(encode, alphabet);

            // Convert the Sequence items into encoded items
            encodedSeqItem = _mapToEncoding.Convert(createSequence[0]);

            // Validate the Converted IsequenceItem
            Assert.AreEqual(65, encodedSeqItem.Value);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Encoded value {0} is expected.", encodedSeqItem.Value));

            // Convert encoded items to alphabet Isequence item.
            alphabetItem = _mapToAlphaBet.Convert(encodedSeqItem);

            // Validate the alphabet sequence item.
            Assert.AreEqual(actualSequence, alphabetItem.Symbol.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Alphabet name {0} is expected.", alphabetItem.Symbol));
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of encoded item to alphabet item is completed successfully.");
        }

        /// <summary>
        /// Get a Default map for DNA Alphabet item.
        /// Input Data :DNA Alphabet Item.
        /// Output Data : "Ncbi4NA".
        /// </summary>
        [Test]
        public void DnaDefaultMap()
        {
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string expectedDNAMap = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.DnaDefaultMap);
            string dnaDefaultmap = string.Empty;

            // Get the DefaultMap for DNA.
            EncodingMap defaultMap = EncodingMap.GetDefaultMap(alphabet);
            dnaDefaultmap = defaultMap.Encoding.Name.ToString();

            // Validate DNA Default Map.
            Assert.AreEqual(dnaDefaultmap, expectedDNAMap);
            ApplicationLog.WriteLine(string.Format(null, "Encoding BVT:DNA Default Map {0} is expected.",
                dnaDefaultmap));
        }

        /// <summary>
        /// Get a Default map for RNA Alphabet item.
        /// Input Data :RNA Alphabet Item.
        /// Output Data : "Ncbi4NA".
        /// </summary>
        [Test]
        public void RnaDefaultMap()
        {
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string expectedRNAMap = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.RnaDefaultMap);
            string rnaDefaultmap = string.Empty;

            // Get the DefaultMap for RNA.
            EncodingMap defaultMap = EncodingMap.GetDefaultMap(alphabet);
            rnaDefaultmap = defaultMap.Encoding.Name.ToString();

            // Validate RNA Default Map.
            Assert.AreEqual(rnaDefaultmap, expectedRNAMap);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT : RNA Default Map {0} is expected.", rnaDefaultmap));
        }

        /// <summary>
        /// Get a Default map for PROTEIN Alphabet item.
        /// Input Data : PROTEIN Alphabet Item.
        /// Output Data : "NCBIstdaa" .
        /// </summary>
        [Test]
        public void ProteinDefaultMap()
        {
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleProteinAlphabetNode,
                Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string expectedPROTEINMap = Utility._xmlUtil.GetTextValue(Constants.SimpleProteinAlphabetNode,
                Constants.ProteinDefaultMap);
            string proteinDefaultmap = string.Empty;

            // Get the DefaultMap for PROTEIN.
            EncodingMap defaultMap = EncodingMap.GetDefaultMap(alphabet);
            proteinDefaultmap = defaultMap.Encoding.Name.ToString();

            // Validate PROTEIN Default Map.
            Assert.AreEqual(proteinDefaultmap, expectedPROTEINMap);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT : Protein Default Map {0} is expected.", proteinDefaultmap));
        }

        /// <summary>
        /// Get a Default map for Nucleotides Encoding(IupacNA).
        /// Input Data : IupacNA Encoded Item.
        /// Output Data : "Dna" .
        /// </summary>
        [Test]
        public void DefaultEncodingIupacNAMap()
        {
            IEncoding encodedDNAItem = Encodings.IupacNA;
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string dnaAlphabets = string.Empty;

            // Get the DefaultMap for Encoding.
            EncodingMap encodedMap = EncodingMap.GetDefaultMap(encodedDNAItem);
            dnaAlphabets = encodedMap.Alphabet.Name.ToString();

            // Validate Encoding Default Map.
            Assert.AreEqual(dnaAlphabets, alphabet.Name.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Encoding Default Map {0} is expected.", dnaAlphabets));
        }

        /// <summary>
        /// Get a Default map for Nucleotides Encoding(Ncbi4NA).
        /// Input Data : Ncbi4NA Encoded Item.
        /// Output Data : "Dna" .
        /// </summary>
        [Test]
        public void DefaultEncodingNcbi4NAMap()
        {
            IEncoding encodedDNAItem = Encodings.Ncbi4NA;
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string dnaAlphabets = string.Empty;

            // Get the DefaultMap for Encoding.
            EncodingMap encodedMap = EncodingMap.GetDefaultMap(encodedDNAItem);
            dnaAlphabets = encodedMap.Alphabet.Name.ToString();

            // Validate Encoding Default Map.
            Assert.AreEqual(dnaAlphabets, alphabet.Name.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Encoding Default Map {0} is expected.", dnaAlphabets));
        }

        /// <summary>
        /// Get a Default map for Nucleotides Encoding(Ncbi2NA).
        /// Input Data : Ncbi2NA Encoded Item.
        /// Output Data : "Dna" .
        /// </summary>
        [Test]
        public void DefaultEncodingNcbi2NAMap()
        {
            IEncoding encodedDNAItem = Encodings.Ncbi2NA;
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string dnaAlphabets = string.Empty;

            // Get the DefaultMap for Encoding.
            EncodingMap encodedMap = EncodingMap.GetDefaultMap(encodedDNAItem);
            dnaAlphabets = encodedMap.Alphabet.Name.ToString();

            // Validate Encoding Default Map.
            Assert.AreEqual(dnaAlphabets, alphabet.Name.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Encoding Default Map {0} is expected.", dnaAlphabets));
        }

        /// <summary>
        /// Get a Default map for Amino Acids Encoding(NcbiStdAA).
        /// Input Data : NcbiStdAA Encoded Item.
        /// Output Data : "Protein" .
        /// </summary>
        [Test]
        public void DefaultEncodingNcbiStdAAMap()
        {
            IEncoding encodedDNAItem = Encodings.NcbiStdAA;
            string proteinAlphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleProteinAlphabetNode,
                Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(proteinAlphabetName);
            string proteinAlphabets = string.Empty;

            // Get the DefaultMap for Encoding.
            EncodingMap encodedMap = EncodingMap.GetDefaultMap(encodedDNAItem);
            proteinAlphabets = encodedMap.Alphabet.Name.ToString();

            // Validate Encoding Default Map.
            Assert.AreEqual(proteinAlphabets, alphabet.Name.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Encoding Default Map {0} is expected.", proteinAlphabets));
        }

        /// <summary>
        /// Get a Default map for Amino Acids Encoding(NcbiEAA).
        /// Input Data : NcbiStdAA Encoded Item.
        /// Output Data : "Protein" .
        /// </summary>
        [Test]
        public void DefaultEncodingNcbieaaMap()
        {
            IEncoding encodedDNAItem = Encodings.NcbiEAA;
            string proteinAlphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleProteinAlphabetNode,
                Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(proteinAlphabetName);
            string proteinAlphabets = string.Empty;

            // Get the DefaultMap for Encoding.
            EncodingMap encodedMap = EncodingMap.GetDefaultMap(encodedDNAItem);
            proteinAlphabets = encodedMap.Alphabet.Name.ToString();

            // Validate Encoding Default Map.
            Assert.AreEqual(proteinAlphabets, alphabet.Name.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Encoding Default Map {0} is expected.", proteinAlphabets));
        }

        /// <summary>
        /// Decode a Valid Byte.
        /// Input Data : Byte value "1".
        /// Output Data : Isequence Item "A".
        /// </summary>
        [Test]
        public void ValidateSequenceDecoder()
        {
            // Gets the byte value from xml.
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDecoderNode,
                Constants.DecoderAlphabetName);
            string alphabetSymbol = Utility._xmlUtil.GetTextValue(Constants.SimpleDecoderNode,
                Constants.AlphabetSymbol);
            string byteValue = Utility._xmlUtil.GetTextValue(Constants.SimpleDecoderNode,
                Constants.DecoderByteValue);
            string[] alphabetSymbolArray = alphabetSymbol.Split(',');
            string[] byteValueArray = byteValue.Split(',');
            string[] alphabetNames = alphabetName.Split(',');
            IEncoding encodedDNAItem = Encodings.Ncbi4NA;

            // Deocode a valid byte to alphabet representation.
            Assert.AreEqual(alphabetSymbolArray.Length, byteValueArray.Length);
            Assert.AreEqual(alphabetSymbolArray.Length, alphabetNames.Length);
            for (int index = 0; index < byteValueArray.Length; index++)
            {
                SequenceDecoder seq = new SequenceDecoder(encodedDNAItem);
                ISequenceItem decodedItem = seq.Decode(byte.Parse(byteValueArray[index], null));
                ApplicationLog.WriteLine(string.Format(null, "Encoding BVT: Decode Item {0} is expected.",
                    decodedItem));

                // Validate the alphabet for corresponding byte value passed.
                Assert.AreEqual(alphabetNames[index], decodedItem.Name.ToString());
                Assert.AreEqual(alphabetSymbolArray[index], decodedItem.Symbol.ToString());

                ApplicationLog.WriteLine(string.Format(null, "Encoding BVT:Deocding Byte value {0} ",
                    decodedItem));
            }
            ApplicationLog.WriteLine("Encoding BVT: Decoding Byte value to Isequence Item is commpleted.");
        }

        /// <summary>
        /// Encode a Valid Sequence to an byte array.
        /// Input Data : Valid Sequence "GA"
        /// Output Data : Byte Array "4,1"
        /// </summary>
        [Test]
        public void EncodeSequenceToByteArray()
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.ExpectedNormalString);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.ByteArray);
            string[] expectedArray = expectedByteArray.Split(',');
            IEncoding encodedDNAItem = Encodings.Ncbi4NA;

            // Encode a valid Sequence to byte array representation.
            SequenceEncoder seq = new SequenceEncoder(encodedDNAItem);
            byte[] encoded = seq.Encode(expectedSequence);

            // Validate the EncodedSequence.
            Assert.AreEqual(encoded[0].ToString((IFormatProvider)null), expectedArray[0]);
            Assert.AreEqual(encoded[1].ToString((IFormatProvider)null), expectedArray[1]);

            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Encoding Default Map {0} is expected.", encoded.ToString()));
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of a byte value to an alphaabet is completed.");
        }

        /// <summary>
        /// Encode a Valid Char to an byte value.
        /// Input Data : Valid Char "A".
        /// Output Data : Byte Value "1".
        /// </summary>
        [Test]
        public void EncodeSingleCharToByte()
        {
            // Gets the byte value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleDecoderNode,
                Constants.AlphabetSymbol);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(Constants.SimpleDecoderNode,
                Constants.DecoderByteValue);
            IEncoding encodedDNAItem = Encodings.Ncbi4NA;
            string[] expectedSymbol = expectedSequence.Split(',');
            string[] byteArray = expectedByteArray.Split(',');

            Assert.AreEqual(expectedSymbol.Length, byteArray.Length);

            for (int index = 0; index < expectedSymbol.Length; index++)
            {
                // Encode a valid Sequence to byte array representation.
                SequenceEncoder seq = new SequenceEncoder(encodedDNAItem);
                byte encoded = seq.Encode(expectedSymbol[index][0]);

                // Validate the Encoded Value.
                Assert.AreEqual(encoded, Convert.ToInt32(byteArray[index], null));
            }
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of a byte value to an alphaabet is completed.");
        }

        /// <summary>
        /// Get a valid Encoding Map for Alphabet to Encoding.
        /// Input Data : Valid Alphabet and encoding.
        /// Output Data : Valid map.
        /// </summary>
        [Test]
        public void AlphabetToEncodingMap()
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.ExpectedSequenceNode);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.SequenceByteArray);
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            IEncoding encode = Encodings.Ncbi4NA;
            string[] expectedArray = expectedByteArray.Split(',');
            this._mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encode);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Create a ISequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(_mapToEncoding.Convert(item));
            }

            // Encode IsequenceList Item to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encode);
            byte[] encodedValue = iSeq.Encode(alphaList);

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(encodedValue[0].ToString((IFormatProvider)null), expectedArray[0]);
            Assert.AreEqual(encodedValue[1].ToString((IFormatProvider)null), expectedArray[1]);
            Assert.AreEqual(encodedValue[2].ToString((IFormatProvider)null), expectedArray[2]);
            Assert.AreEqual(encodedValue[3].ToString((IFormatProvider)null), expectedArray[3]);
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of Isequence Item list to byte array is completed.");
        }

        /// <summary>
        /// Get a valid Encoding Map for Encoding to Alphabet.
        /// Input Data : Valid Alphabet and encoding.
        /// Output Data : Valid map.
        /// </summary>
        [Test]
        public void EncodingToAlphabet()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedSingleChar);
            IAlphabet alphabet = Alphabets.DNA;
            IEncoding encode = Encodings.IupacNA;
            ISequenceItem encodedSeqItem;
            ISequenceItem alphabetItem;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Encoding BVT:The Sequence {0} is expected.",
                actualSequence));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);
            this._mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encode);
            this._mapToAlphaBet = EncodingMap.GetMapToAlphabet(encode, alphabet);

            // Convert the Sequence items into encoded items
            encodedSeqItem = _mapToEncoding.Convert(createSequence[0]);

            // Validate the Converted IsequenceItem
            Assert.AreEqual(65, encodedSeqItem.Value);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Encoded value {0} is expected.", encodedSeqItem.Value));

            // Convert encoded items to alphabet Isequence item.
            alphabetItem = _mapToAlphaBet.Convert(encodedSeqItem);

            // Validate the alphabet sequence item.
            Assert.AreEqual(actualSequence, alphabetItem.Symbol.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT:Alphabet name {0} is expected.", alphabetItem.Symbol));
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of encoded item to alphabet item is completed successfully.");
        }

        /// <summary>
        /// Encode a IsequenceItem list to an Byte Array.
        /// Input Data : Valid Isequence "ACGA".
        /// Output Data : Byte Array  "1,2,4,1".
        /// </summary>
        [Test]
        public void EncodeISequenceItemListToByteArray()
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleEncoderName, Constants.ExpectedSequenceNode);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(
                Constants.SimpleEncoderName, Constants.SequenceByteArray);
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            IEncoding encode = Encodings.Ncbi4NA;
            string[] expectedArray = expectedByteArray.Split(',');
            this._mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encode);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Create a ISequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(_mapToEncoding.Convert(item));
            }

            // Encode IsequenceList Item to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encode);
            byte[] encodedValue = iSeq.Encode(alphaList);

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(encodedValue[0].ToString((IFormatProvider)null), expectedArray[0]);
            Assert.AreEqual(encodedValue[1].ToString((IFormatProvider)null), expectedArray[1]);
            Assert.AreEqual(encodedValue[2].ToString((IFormatProvider)null), expectedArray[2]);
            Assert.AreEqual(encodedValue[3].ToString((IFormatProvider)null), expectedArray[3]);
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of Isequence Item list to byte array is completed.");
        }

        /// <summary>
        /// Encode a List of ISequence items to byte array..
        /// Input Data : Valid Isequence "ACGA" and valid target byte array.
        /// Output Data : Byte Array  "1,2,4,1".
        /// </summary>
        [Test]
        public void EncodeISequenceListToTargetByteArray()
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleEncoderName, Constants.ExpectedSequenceNode);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(
                Constants.SimpleEncoderName, Constants.SequenceByteArray);
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            IEncoding encode = Encodings.Ncbi4NA;
            string[] expectedArray = expectedByteArray.Split(',');
            this._mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encode);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Create a Sequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(_mapToEncoding.Convert(item));
            }

            // Encode IsequenceList Item to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encode);
            byte[] encodedValue = new byte[10];
            iSeq.Encode(alphaList, encodedValue);

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(encodedValue[0].ToString((IFormatProvider)null), expectedArray[0]);
            Assert.AreEqual(encodedValue[1].ToString((IFormatProvider)null), expectedArray[1]);
            Assert.AreEqual(encodedValue[2].ToString((IFormatProvider)null), expectedArray[2]);
            Assert.AreEqual(encodedValue[3].ToString((IFormatProvider)null), expectedArray[3]);
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of Isequence Item list to byte array is completed.");
        }

        /// <summary>
        /// Encode a Sequence string to an byte array..
        /// Input Data : Valid string "ACGA" and valid target byte array.
        /// Output Data : Byte Array  "1,2,4,1".
        /// </summary>
        [Test]
        public void EncodeSequenceToTargetByteArray()
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleEncoderName, Constants.ExpectedSequenceNode);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(
                Constants.SimpleEncoderName, Constants.SequenceByteArray);
            IEncoding encode = Encodings.Ncbi4NA;
            string[] expectedArray = expectedByteArray.Split(',');

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}'.", expectedSequence));

            // Encode IsequenceList Item to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encode);
            byte[] encodedValue = new byte[10];
            iSeq.Encode(expectedSequence, encodedValue);

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(encodedValue[0].ToString((IFormatProvider)null), expectedArray[0]);
            Assert.AreEqual(encodedValue[1].ToString((IFormatProvider)null), expectedArray[1]);
            Assert.AreEqual(encodedValue[2].ToString((IFormatProvider)null), expectedArray[2]);
            Assert.AreEqual(encodedValue[3].ToString((IFormatProvider)null), expectedArray[3]);
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of Isequence Item list to byte array is completed.");
        }

        /// <summary>
        /// Encode a IsequenceItem list with valid target array and valid offset are parameters.
        /// Input Data : Valid Isequence "ACGA" and valid target byte array.
        /// Output Data : Byte Array  "1,2,4,1".
        /// </summary>
        [Test]
        public void EncodeIsequenceItemListWithOffsetValue()
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.ExpectedSequenceNode);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.SequenceByteArray);
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            IEncoding encode = Encodings.Ncbi4NA;
            string[] expectedArray = expectedByteArray.Split(',');
            this._mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encode);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Create a Sequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(_mapToEncoding.Convert(item));
            }

            // Encode IsequenceList Item to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encode);
            byte[] encodedValue = new byte[10];
            iSeq.Encode(alphaList, encodedValue, 3);

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(encodedValue[3].ToString((IFormatProvider)null), expectedArray[0]);
            Assert.AreEqual(encodedValue[4].ToString((IFormatProvider)null), expectedArray[1]);
            Assert.AreEqual(encodedValue[5].ToString((IFormatProvider)null), expectedArray[2]);
            Assert.AreEqual(encodedValue[6].ToString((IFormatProvider)null), expectedArray[3]);
            ApplicationLog.WriteLine(
                "Encoding BVT: Encoding Isequence Item list to byte array is completed.");
        }

        /// <summary>
        /// Encode a Sequence string to Byte array by passing valid offset..
        /// Input Data : Sequence string "ACGA" and valid target byte array.
        /// Output Data : Byte Array  "1,2,4,1".
        /// </summary>
        [Test]
        public void EncodeSequenceWithOffsetValue()
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.ExpectedSequenceNode);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.SequenceByteArray);
            IEncoding encode = Encodings.Ncbi4NA;
            string[] expectedArray = expectedByteArray.Split(',');

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Encoding BVT: Sequence '{0}'.",
                expectedSequence));


            // Encode Sequence string to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encode);
            byte[] encodedValue = new byte[10];
            iSeq.Encode(expectedSequence, encodedValue, 3);

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(encodedValue[3].ToString((IFormatProvider)null), expectedArray[0]);
            Assert.AreEqual(encodedValue[4].ToString((IFormatProvider)null), expectedArray[1]);
            Assert.AreEqual(encodedValue[5].ToString((IFormatProvider)null), expectedArray[2]);
            Assert.AreEqual(encodedValue[6].ToString((IFormatProvider)null), expectedArray[3]);
            ApplicationLog.WriteLine(
                "Encoding BVT: Encoding sequence string to byte array is completed.");
        }

        /// <summary>
        /// Encode a string with valid target array and valid offset are parameters.
        /// Input Data : Valid string "ABCD" and valid target byte array.
        /// Output Data : Byte Array  "1,2,3,4".
        /// </summary>
        [Test]
        public void EncodeStringToByteArray()
        {
            // Gets the string value from xml.
            string expectedString = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.ExpectedString);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(Constants.SimpleEncoderName,
                Constants.StringByteArray);
            IEncoding encodeItem = Encodings.Ncbi4NA;
            string[] expectedArray = expectedByteArray.Split(',');

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Encoding BVT: Sequence '{0}'", expectedString));

            // Encode a String to an Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encodeItem);

            byte[] encodedValue = new byte[10];
            iSeq.Encode(expectedString, encodedValue, 3);

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(encodedValue[3].ToString((IFormatProvider)null), expectedArray[0]);
            Assert.AreEqual(encodedValue[4].ToString((IFormatProvider)null), expectedArray[1]);
            Assert.AreEqual(encodedValue[5].ToString((IFormatProvider)null), expectedArray[2]);
            ApplicationLog.WriteLine(
                "Encoding BVT: Conversion of Isequence Item list to byte array is completed.");
        }

        /// <summary>
        /// Add a sequence Item to ReadOnly sequence and validate the exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void ValidateAddSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedNormalString);
            bool Exthrown = false;


            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate Created Sequence.
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence {0} is expected.", createSequence.ToString()));

            // Mark Sequence as Readonly.
            createSequence.IsReadOnly = true;

            // Validate if ReadOnly Sequence is throwing an error when try to add Sequence item.
            try
            {
                IupacNAEncoding.Instance.Add(createSequence[0]);
            }
            catch (Exception)
            {
                Exthrown = true;
            }

            // Validate if Clear method is throwing an exception.
            Assert.IsTrue(Exthrown);
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of Read only exception is completed"));
        }

        /// <summary>
        /// Clear a Sequence list from a ReadOnly sequence and validate the exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void ValidateClearSequenceItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedNormalString);
            bool Exthrown = false;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate Created Sequence.
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence {0} is expected.", createSequence.ToString()));

            // Mark Sequence as Readonly.
            createSequence.IsReadOnly = true;

            // Validate if ReadOnly Sequence is throwing an error when try to delete sequence data.
            try
            {
                createSequence.Clear();
            }
            catch (Exception)
            {
                Exthrown = true;
            }
            // Validate if Clear method is throwing an exception.
            Assert.IsTrue(Exthrown);
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of Read only exception is completed"));
        }

        /// <summary>
        /// Remove a Sequence Item from a ReadOnly Sequence list and validate the exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingRemoveSequenceItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedNormalString);
            bool Exthrown = false;


            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate Created Sequence.
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence {0} is expected.", createSequence.ToString()));

            // Mark Sequence as Readonly.
            createSequence.IsReadOnly = true;

            // Validate if ReadOnly Sequence is throwing an error when try to delete sequence data.
            try
            {
                IupacNAEncoding.Instance.Remove(createSequence[0]);
            }
            catch (Exception)
            {
                Exthrown = true;
            }

            // Validate if Remove method is throwing an exception.
            Assert.IsTrue(Exthrown);
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of Read only exception is completed"));
        }

        /// <summary>
        /// Validate wheather or not IsequenceItem is in IupacNAEncoding using contains() method.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingContains()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string expectedChar = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpInputChar);
            bool result = false;

            // Call Contains Method.
            IupacNAEncoding encodingMethod = IupacNAEncoding.Instance;
            ISequenceItem seqItem = encodingMethod.LookupBySymbol(expectedChar[0]);
            result = encodingMethod.Contains(seqItem);

            // Validate wheather or not IsequenceItem is in encoding.
            Assert.IsTrue(result);

            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation od contains() method is completed sucessfully."));
        }

        /// <summary>
        /// Copies the nucleotides in IupacNAEncoding encoding into an array.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void IupacNAEncodingCopyTo()
        {
            // Get Value from xml.
            string expectedAlphabet = Utility._xmlUtil.GetTextValue(Constants.EncodingNode,
                Constants.AlphabetSymbol);
            string encodedValue = Utility._xmlUtil.GetTextValue(Constants.EncodingNode,
                Constants.DecoderByteValue);

            // Copy nucleotides into an array.
            ISequenceItem[] seqItems = new ISequenceItem[20];
            IupacNAEncoding.Instance.CopyTo(seqItems, 0);

            // Validate copied array.
            Assert.AreEqual(seqItems[14].Value.ToString((IFormatProvider)null), encodedValue);
            Assert.AreEqual(seqItems[0].Symbol.ToString(), expectedAlphabet);
            Assert.IsNull(seqItems[16]);

            Console.WriteLine(string.Format(null,
                "Encoding BVT: Copies of Nucleaotides to array is completed."));
        }

        /// <summary>
        /// Validate GetEnumerator
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingGetEnumerator()
        {
            // Validate GetEnumerator
            IEnumerator<ISequenceItem> list;
            list = IupacNAEncoding.Instance.GetEnumerator();

            // Validate eneumerator list.
            Assert.IsNotNull(list);
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of GetEnumerator is completed."));
        }

        /// <summary>
        /// Validate IupacNAEncoding LookUpBySymbol() by passing valid character.
        /// Input Data : Valid Char - 'A';
        /// OutPut Data : Look Up Symbol - 'A'
        /// </summary>
        [Test]
        public void IupacNAEncodingLookupByChar()
        {
            // Get LookUp() values from xml.
            string expectedChar = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpInputChar);
            string expectedLookUpSymbol = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpOutputChar);
            char ch = expectedChar[0];

            // Call a LookUp method.
            ISequenceItem seqItem = null;
            seqItem = IupacNAEncoding.Instance.LookupBySymbol(ch);

            // Validate LookUp Symbol.
            Assert.AreEqual(seqItem.Symbol.ToString(), expectedLookUpSymbol);
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of LookUp symbol is completed."));
        }

        /// <summary>
        /// Validate IupacNAEncoding LookUpBySymbol() by passing valid Byte.
        /// Input Data : Valid Byte Value - '68';
        /// OutPut Data : Look Up Symbol - 'D'
        /// </summary>
        [Test]
        public void IupacNAEncodingLookupByByte()
        {
            // Get LookUp() values from xml.
            string expectedSymbol = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpOutputSymbol);

            // Call a LookUp method.
            ISequenceItem seqItem = null;
            seqItem = IupacNAEncoding.Instance.LookupByValue(68);

            // Validate LookUp Symbol.
            Assert.AreEqual(seqItem.Symbol.ToString(), expectedSymbol);
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of LookUp symbol by passing byte is completed."));
        }

        /// <summary>
        /// Validate IupacNAEncoding LookUpBySymbol() by passing valid String.
        /// Input Data : Valid String - "G";
        /// OutPut Data : Look Up Symbol - "G"
        /// </summary>
        [Test]
        public void IupacNAEncodingLookupByString()
        {
            // Get LookUp() values from xml.
            string expectedString = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpInputChar);
            string expectedLookUpString = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpOutputChar);


            // Call a IupacNAEncoding LookUp method.
            ISequenceItem seqItem = null;
            seqItem = IupacNAEncoding.Instance.LookupBySymbol(expectedString[0]);

            // Validate LookUp Symbol.
            Assert.AreEqual(seqItem.Symbol.ToString(), expectedLookUpString);
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of LookUp symbol by passing string is completed."));
        }

        /// <summary>
        /// Validate wheather Ncbi2NAEncoding  Add() method is throwing an exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingAdd()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedNormalString);
            bool Exthrown = false;


            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate Created Sequence.
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence {0} is expected.", createSequence.ToString()));

            // Validate if Add() method is throwing an exception.
            try
            {
                Ncbi2NAEncoding.Instance.Add(createSequence[0]);
            }
            catch (Exception)
            {
                Exthrown = true;
            }

            // Validate if Add() method is throwing an exception.
            Assert.IsTrue(Exthrown);
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of Read only exception is completed"));
        }

        /// <summary>
        /// Validate wheather Ncbi2NAEncoding  Clear() method is throwing an exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingClear()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedNormalString);
            bool Exthrown = false;


            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate Created Sequence.
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence {0} is expected.", createSequence.ToString()));

            // Validate if Clear() method is throwing an exception.
            try
            {
                Ncbi2NAEncoding.Instance.Clear();
            }
            catch (Exception)
            {
                Exthrown = true;
            }

            // Validate if Clear() method is throwing an exception.
            Assert.IsTrue(Exthrown);
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of Read only exception is completed"));
        }

        /// <summary>
        /// Validate wheather or not IsequenceItem is in Ncbi2NAEncoding using contains() method.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingContains()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string expectedChar = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpInputChar);
            bool result = false;

            // Call Contains Method.
            Ncbi2NAEncoding encodingMethod = Ncbi2NAEncoding.Instance;
            ISequenceItem seqItem = encodingMethod.LookupBySymbol(expectedChar[0]);
            result = encodingMethod.Contains(seqItem);

            // Validate wheather or not IsequenceItem is in encoding.
            Assert.IsTrue(result);

            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation od contains() method is completed sucessfully."));
        }

        /// <summary>
        /// Copies the nucleotides in Ncbi2NAEncoding encoding into an array.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void Ncbi2NAEncodingCopyTo()
        {
            // Get Value from xml.
            string expectedAlphabet = Utility._xmlUtil.GetTextValue(Constants.EncodingNode,
                Constants.AlphabetSymbol);
            string encodedValue = Utility._xmlUtil.GetTextValue(Constants.EncodingNode,
                Constants.Ncbi2NAEncodingByteValue);

            // Copy nucleotides into an array.
            ISequenceItem[] seqItems = new ISequenceItem[20];
            Ncbi2NAEncoding.Instance.CopyTo(seqItems, 0);

            // Validate copied array.
            Assert.AreEqual(seqItems[0].Value.ToString((IFormatProvider)null), encodedValue);
            Assert.AreEqual(seqItems[0].Symbol.ToString(), expectedAlphabet);
            Assert.IsNull(seqItems[16]);

            Console.WriteLine(string.Format(null,
                "Encoding BVT: Copies of Nucleaotides to array is completed."));
        }

        /// <summary>
        /// Validate GetEnumerator for Ncbi2NAEncoding.
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingGetEnumerator()
        {
            // Validate GetEnumerator
            IEnumerator<ISequenceItem> list;
            list = Ncbi2NAEncoding.Instance.GetEnumerator();

            // Validate eneumerator list.
            Assert.IsNotNull(list);
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of GetEnumerator is completed."));
        }

        /// <summary>
        /// Validate Ncbi2NAEncoding LookUpBySymbol() by passing valid character.
        /// Input Data : Valid Char - 'A';
        /// OutPut Data : Look Up Symbol - 'A'
        /// </summary>
        [Test]
        public void Ncbi2NAEncodingLookupByChar()
        {
            // Get LookUp() values from xml.
            string expectedChar = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpInputChar);
            string expectedLookUpSymbol = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpOutputChar);
            char ch = expectedChar[0];

            // Call a LookUp method.
            ISequenceItem seqItem = null;
            seqItem = Ncbi2NAEncoding.Instance.LookupBySymbol(ch);

            // Validate LookUp Symbol.
            Assert.AreEqual(seqItem.Symbol.ToString(), expectedLookUpSymbol);
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of LookUp symbol is completed."));
        }

        /// <summary>
        /// Validate Ncbi2NAEncoding LookUpBySymbol() by passing valid Byte.
        /// Input Data : Valid Byte Value - '68';
        /// OutPut Data : Look Up Symbol - 'D'
        /// </summary>
        [Test]
        public void Ncbi2NAEncodingLookupByByte()
        {
            // Call a LookUp method.
            ISequenceItem seqItem = null;
            seqItem = Ncbi2NAEncoding.Instance.LookupByValue(3);

            // Validate LookUp Symbol.
            Assert.AreEqual(seqItem.Symbol.ToString(), "T");
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of LookUp symbol by passing byte is completed."));
        }

        /// <summary>
        /// Validate Ncbi2NAEncoding LookUpBySymbol() by passing valid String.
        /// Input Data : Valid String - "G";
        /// OutPut Data : Look Up Symbol - "G"
        /// </summary>
        [Test]
        public void Ncbi2NAEncodingLookupByString()
        {
            // Get LookUp() values from xml.
            string expectedString = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpInputChar);
            string expectedLookUpString = Utility._xmlUtil.GetTextValue(Constants.LookUpNode,
                Constants.LookUpOutputChar);

            // Call a LookUp method.
            ISequenceItem seqItem = null;
            seqItem = Ncbi2NAEncoding.Instance.LookupBySymbol(expectedString[0]);

            // Validate LookUp Symbol.
            Assert.AreEqual(seqItem.Symbol.ToString(), expectedLookUpString);
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of LookUp symbol by passing string is completed."));
        }

        /// <summary>
        /// Ncbi2NAEncoding : Remove a Sequence Item from a ReadOnly Sequence list and validate the exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingRemoveSequenceItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedNormalString);
            bool Exthrown = false;


            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate Created Sequence.
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Encoding BVT: Sequence {0} is expected.", createSequence.ToString()));

            // Mark Sequence as Readonly.
            createSequence.IsReadOnly = true;

            // Validate if ReadOnly Sequence is throwing an error when try to delete sequence data.
            try
            {
                Ncbi2NAEncoding.Instance.Remove(createSequence[0]);
            }
            catch (Exception)
            {
                Exthrown = true;
            }

            // Validate if Remove method is throwing an exception.
            Assert.IsTrue(Exthrown);
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Encoding BVT: Validation of Read only exception is completed"));
        }

        #endregion Encoding Bvt TestCases
    }
}
