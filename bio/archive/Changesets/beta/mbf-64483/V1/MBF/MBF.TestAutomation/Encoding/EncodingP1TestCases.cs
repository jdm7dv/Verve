// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * EncodingP1TestCases.cs
 * 
 * This file contains the Encoding P1 Test Cases which includes Encoding Map
 * and different encoding test cases.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;

using MBF.Encoding;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.Encoding
{
    /// <summary>
    /// P1 test cases for MBF Encodings.
    /// </summary>
    [TestFixture]
    public class EncodingP1TestCases
    {

        #region Constructor

        static EncodingP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion

        #region Test Cases

        #region Ncbi4NAEncoding

        /// <summary>
        /// Ncbi4NAEncoding: Add a valid sequence item and 
        /// validates if Add() method is throwing exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void InValidateNcbi4NAEncodingAdd()
        {
            InValidateEncodingAdd(Ncbi4NAEncoding.Instance);
        }

        /// <summary>
        /// Ncbi4NAEncoding : Remove a Sequence Item from a ReadOnly Sequence list and 
        /// validates if Remove() method is throwing exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void InValidateNcbi4NAEncodingRemove()
        {
            InValidateEncodingRemove(Ncbi4NAEncoding.Instance);
        }

        /// <summary>
        /// Ncbi4NAEncoding : Validates if Ncbi4NAEncoding Clear() method is throwing an exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void InValidateNcbi4NAEncodingClear()
        {
            InValidateEncodingClear(Ncbi4NAEncoding.Instance);
        }

        /// <summary>
        /// Validates sequence item is present in Ncbi4NAEncoding using contains() method.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingContains()
        {
            ValidateEncodingContains(Ncbi4NAEncoding.Instance, Constants.LookUpInputString);
        }

        /// <summary>
        /// Copies the nucleotides collection of Ncbi4NAEncoding encoding into an array and 
        /// Validates the CopyTo() method.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingCopyTo()
        {
            ValidateEncodingCopyTo(Ncbi4NAEncoding.Instance, Constants.Alphabet4NAEncodingSymbol,
                Constants.Ncbi4NAEncodingByteValue, Constants.NucleotidesListLength);
        }

        /// <summary>
        /// Validates Ncbi4NAEncoding GetEnumerator() method.
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingGetEnumerator()
        {
            ValidateEncodingGetEnumerator(Ncbi4NAEncoding.Instance);
        }

        /// <summary>
        /// Validates if Ncbi4NAEncoding LookUpBySymbol() returns expected symbol by passing valid Byte.
        /// Input Data : Valid Byte Value - '68';
        /// OutPut Data : Look Up Symbol - 'D'
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookupValueByByte()
        {
            ValidateEncodingLookUpByByte(Ncbi4NAEncoding.Instance, Constants.Alphabet4NAEncodingSymbol,
                Constants.Ncbi4NAEncodingByteValue);
        }

        /// <summary>
        /// Validates if Ncbi4NAEncoding LookUpBySymbol() returns expected symbol by passing valid String.
        /// Input Data : Valid String - "A";
        /// OutPut Data : Look Up Symbol - "A"
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookupSymbolByString()
        {
            ValidateEncodingLookUpByString(Ncbi4NAEncoding.Instance, Constants.LookUpInputString,
                Constants.LookUpOutputString);
        }

        /// <summary>
        /// Validates if Ncbi4NAEncoding LookUpBySymbol() returns expected symbol by passing valid character.
        /// Input Data : Valid Char - 'A';
        /// OutPut Data : Look Up Symbol - 'A'
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookupByChar()
        {
            ValidateEncodingLookUpByChar(Ncbi4NAEncoding.Instance, Constants.LookUpInputString,
                Constants.LookUpOutputString);
        }

        /// <summary>
        /// Validates Dna sequence is present in Ncbi4NAEncoding using contains() method.
        /// Input Data : Valid Dna sequence.
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingContainsWithDnaSequence()
        {
            ValidateEncodingWithAlphabetContains(Ncbi4NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates Rna sequence is present in Ncbi4NAEncoding using contains() method.
        /// Input Data : Valid Rna sequence.
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingContainsWithRnaSequence()
        {
            ValidateEncodingWithAlphabetContains(Ncbi4NAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Copies the nucleotides collection of Ncbi4NAEncoding into an array and 
        /// Validates if copied collection contains Dna sequence.
        /// Input Data : Valid Dna sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingCopyToWithDnaSequence()
        {
            ValidateEncodingWithAlphabetCopyTo(Ncbi4NAEncoding.Instance, Constants.NucleotidesListLength,
                Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Copies the nucleotides collection of Ncbi4NAEncoding into an array and 
        /// Validates if copied collection contains Rna sequence.
        /// Input Data : Valid Rna sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingCopyToWithRnaSequence()
        {
            ValidateEncodingWithAlphabetCopyTo(Ncbi4NAEncoding.Instance, Constants.NucleotidesListLength,
                Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates if Ncbi4NAEncoding LookUpBySymbol() returns expected symbol by passing valid Dna char.
        /// Input Data : Valid Dna Char
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookBySymbolWithDnaChar()
        {
            ValidateEncodingLookUpByChar(Ncbi4NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates if Ncbi4NAEncoding LookUpBySymbol() returns expected symbol by passing valid Rna char.
        /// Input Data : Valid Rna Char
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookBySymbolWithRnaChar()
        {
            ValidateEncodingLookUpByChar(Ncbi4NAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates if Ncbi4NAEncoding LookUpBySymbol() returns expected symbol by passing 
        /// valid Dna sequence string.
        /// Input Data : Valid Dna Sequence string
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookBySymbolWithDnaString()
        {
            ValidateEncodingLookUpByString(Ncbi4NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates if Ncbi4NAEncoding LookUpBySymbol() returns expected symbol by passing 
        /// valid Rna sequence string.
        /// Input Data : Valid Rna Sequence string
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookBySymbolWithRnaString()
        {
            ValidateEncodingLookUpByString(Ncbi4NAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates if Ncbi4NAEncoding LookUpBySymbol() returns expected symbol by passing valid Dna byte.
        /// Input Data : Valid Dna byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookBySymbolWithDnaByte()
        {
            ValidateEncodingLookUpByByte(Ncbi4NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates if Ncbi4NAEncoding LookUpBySymbol() returns expected symbol by passing valid Rna byte.
        /// Input Data : Valid Rna byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookBySymbolWithRnaByte()
        {
            ValidateEncodingLookUpByByte(Ncbi4NAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates all properties of sequence item using Ncbi4NAEncoding LookUpBySymbol() 
        /// returns expected symbol by passing valid Dna byte.
        /// Input Data : Valid Dna byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi4NAEncodingLookBySymbolAllProperties()
        {
            ValidateEncodingLookUpBySymbol(Ncbi4NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        #endregion

        #region NcbiEAAEncoding

        /// <summary>
        /// NcbiEAAEncoding: Add a valid sequence item and 
        /// validates if Add() method is throwing exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void InValidateNcbiEAAEncodingAdd()
        {
            InValidateEncodingAdd(NcbiEAAEncoding.Instance);
        }

        /// <summary>
        /// NcbiEAAEncoding : Remove a Sequence Item from a ReadOnly Sequence list and 
        /// validates if Remove() method is throwing exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void InValidateNcbiEAAEncodingRemove()
        {
            InValidateEncodingRemove(NcbiEAAEncoding.Instance);
        }

        /// <summary>
        /// NcbiEAAEncoding : Validates if NcbiEAAEncoding Clear() method is throwing an exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void InValidateNcbiEAAEncodingClear()
        {
            InValidateEncodingClear(NcbiEAAEncoding.Instance);
        }

        /// <summary>
        /// Validates sequence item is present in NcbiEAAEncoding using contains() method.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingContains()
        {
            ValidateEncodingContains(NcbiEAAEncoding.Instance, Constants.LookUpInputChar);
        }

        /// <summary>
        /// Copies the nucleotides collection of NcbiEAAEncoding encoding into an array and 
        /// Validates the CopyTo() method.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingCopyTo()
        {
            ValidateEncodingCopyTo(NcbiEAAEncoding.Instance, Constants.AlphabetEAAEncodingSymbol,
                Constants.NcbiEAAEncodingByteValue, Constants.AminoAcidsListLength);
        }

        /// <summary>
        /// Validates NcbiEAAEncoding GetEnumerator() method.
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingGetEnumerator()
        {
            ValidateEncodingGetEnumerator(NcbiEAAEncoding.Instance);
        }

        /// <summary>
        /// Validates if NcbiEAAEncoding LookUpBySymbol() returns expected symbol by passing valid Byte.
        /// Input Data : Valid Byte Value;
        /// OutPut Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingLookupValueByByte()
        {
            ValidateEncodingLookUpByByte(NcbiEAAEncoding.Instance, Constants.AlphabetEAAEncodingSymbol,
                Constants.NcbiEAAEncodingByteValue);
        }

        /// <summary>
        /// Validates if NcbiEAAEncoding LookUpBySymbol() returns expected symbol by passing valid String.
        /// Input Data : Valid String;
        /// OutPut Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingLookupSymbolByString()
        {
            ValidateEncodingLookUpByString(NcbiEAAEncoding.Instance, Constants.LookUpInputChar,
                Constants.LookUpOutputChar);
        }

        /// <summary>
        /// Validates if NcbiEAAEncoding LookUpBySymbol() returns expected symbol by passing valid character.
        /// Input Data : Valid Char;
        /// OutPut Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingLookupByChar()
        {
            ValidateEncodingLookUpByChar(NcbiEAAEncoding.Instance, Constants.LookUpInputString,
                Constants.LookUpOutputString);
        }

        /// <summary>
        /// Validates Dna sequence is present in NcbiEAAEncoding using contains() method.
        /// Input Data : Valid Dna sequence.
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingContainsWithProteinSequence()
        {
            ValidateEncodingWithAlphabetContains(NcbiEAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Copies the amino acids collection of NcbiEAAEncoding into an array and 
        /// Validates if copied collection contains Protein sequence.
        /// Input Data : Valid Protein sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingCopyToWithProteinSequence()
        {
            ValidateEncodingWithAlphabetCopyTo(NcbiEAAEncoding.Instance, Constants.AminoAcidsListLength,
                Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Validates if NcbiEAAEncoding LookUpBySymbol() returns expected symbol by passing valid Protein char.
        /// Input Data : Valid Protein Char
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingLookBySymbolWithProteinChar()
        {
            ValidateEncodingLookUpByChar(NcbiEAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Validates if NcbiEAAEncoding LookUpBySymbol() returns expected symbol by passing
        /// valid Protein sequence string.
        /// Input Data : Valid Protein Sequence string
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingLookBySymbolWithProteinString()
        {
            ValidateEncodingLookUpByString(NcbiEAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Validates if NcbiEAAEncoding LookUpBySymbol() returns expected symbol by passing valid Dna byte.
        /// Input Data : Valid Protein byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingLookBySymbolWithProteinByte()
        {
            ValidateEncodingLookUpByByte(NcbiEAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Validates all properties of sequence item using NcbiEAAEncoding LookUpBySymbol() 
        /// returns expected symbol by passing valid Dna byte.
        /// Input Data : Valid Protein sequence
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiEAAEncodingLookBySymbolAllProperties()
        {
            ValidateEncodingLookUpBySymbol(NcbiEAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        #endregion

        #region NcbiStdAAEncoding

        /// <summary>
        /// NcbiStdAAEncoding: Add a valid sequence item and 
        /// validates if Add() method is throwing exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void InValidateNcbiStdAAEncodingAdd()
        {
            InValidateEncodingAdd(NcbiStdAAEncoding.Instance);
        }

        /// <summary>
        /// NcbiStdAAEncoding : Remove a Sequence Item from a ReadOnly Sequence list and 
        /// validates if Remove() method is throwing exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void InValidateNcbiStdAAEncodingRemove()
        {
            InValidateEncodingRemove(NcbiStdAAEncoding.Instance);
        }

        /// <summary>
        /// NcbiStdAAEncoding : Validates if NcbiStdAAEncoding Clear() method is throwing an exception.
        /// Input Data : Valid Sequence and sequence item
        /// Output Data : "Exception".
        /// </summary>
        [Test]
        public void InValidateNcbiStdAAEncodingClear()
        {
            InValidateEncodingClear(NcbiStdAAEncoding.Instance);
        }

        /// <summary>
        /// Validates sequence item is present in NcbiStdAAEncoding using contains() method.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingContains()
        {
            ValidateEncodingContains(NcbiStdAAEncoding.Instance, Constants.LookUpInputChar);
        }

        /// <summary>
        /// Copies the nucleotides collection of NcbiStdAAEncoding encoding into an array and 
        /// Validates the CopyTo() method.
        /// Input Data : Valid sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingCopyTo()
        {
            ValidateEncodingCopyTo(NcbiStdAAEncoding.Instance, Constants.AlphabetStdAAEncodingSymbol,
                Constants.NcbiStdAAEncodingByteValue, Constants.AminoAcidsListLength);
        }

        /// <summary>
        /// Validates NcbiStdAAEncoding GetEnumerator() method.
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingGetEnumerator()
        {
            ValidateEncodingGetEnumerator(NcbiStdAAEncoding.Instance);
        }

        /// <summary>
        /// Validates if NcbiStdAAEncoding LookUpBySymbol() returns expected symbol by passing valid Byte.
        /// Input Data : Valid Byte Value - '68';
        /// OutPut Data : Look Up Symbol - 'D'
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingLookupValueByByte()
        {
            ValidateEncodingLookUpByByte(NcbiStdAAEncoding.Instance, Constants.AlphabetStdAAEncodingSymbol,
                Constants.NcbiStdAAEncodingByteValue);
        }

        /// <summary>
        /// Validates if NcbiStdAAEncoding LookUpBySymbol() returns expected symbol by passing valid String.
        /// Input Data : Valid String - "A";
        /// OutPut Data : Look Up Symbol - "A"
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingLookupSymbolByString()
        {
            ValidateEncodingLookUpByString(NcbiStdAAEncoding.Instance, Constants.LookUpInputChar,
                Constants.LookUpOutputChar);
        }

        /// <summary>
        /// Validates if NcbiStdAAEncoding LookUpBySymbol() returns expected symbol by passing valid character.
        /// Input Data : Valid Char - 'A';
        /// OutPut Data : Look Up Symbol - 'A'
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingLookupByChar()
        {
            ValidateEncodingLookUpByChar(NcbiStdAAEncoding.Instance, Constants.LookUpInputString,
                Constants.LookUpOutputString);
        }

        /// <summary>
        /// Validates Dna sequence is present in NcbiStdAAEncoding using contains() method.
        /// Input Data : Valid Dna sequence.
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingContainsWithProteinSequence()
        {
            ValidateEncodingWithAlphabetContains(NcbiStdAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Copies the amino acids collection of NcbiStdAAEncoding into an array and 
        /// Validates if copied collection contains Protein sequence.
        /// Input Data : Valid Protein sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingCopyToWithProteinSequence()
        {
            ValidateEncodingWithAlphabetCopyTo(NcbiStdAAEncoding.Instance, Constants.AminoAcidsListLength,
                Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Validates if NcbiStdAAEncoding LookUpBySymbol() returns expected symbol by passing valid Protein char.
        /// Input Data : Valid Protein Char
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingLookBySymbolWithProteinChar()
        {
            ValidateEncodingLookUpByChar(NcbiStdAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Validates if NcbiStdAAEncoding LookUpBySymbol() returns expected symbol by passing valid
        /// Protein sequence string.
        /// Input Data : Valid Protein Sequence string
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingLookBySymbolWithProteinString()
        {
            ValidateEncodingLookUpByString(NcbiStdAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Validates if NcbiStdAAEncoding LookUpBySymbol() returns expected symbol by passing valid Dna byte.
        /// Input Data : Valid Protein byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingLookBySymbolWithProteinByte()
        {
            ValidateEncodingLookUpByByte(NcbiStdAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        /// <summary>
        /// Validates all properties of sequence item using NcbiStdAAEncoding LookUpBySymbol() returns 
        /// expected symbol by passing valid Dna byte.
        /// Input Data : Valid Protein sequence
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbiStdAAEncodingLookBySymbolAllProperties()
        {
            ValidateEncodingLookUpBySymbol(NcbiStdAAEncoding.Instance, Constants.EncodingSimpleProtein);
        }

        #endregion

        #region EncodingMap

        /// <summary>
        /// Converts a valid Dna alphabet item to an encoded item.
        /// Validates GetMaptoEncoding() method of EncodingMap.
        /// Input Data : Valid Dna Sequence with single character - "A".
        /// Output Data : Encoded byte value of an IsequenceItem - "65".
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertDnaAlphabetToEncodedItem()
        {
            ValidateEncodingMapConvertAlphabetToEncodedItem(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleDna, Alphabets.DNA, Encodings.IupacNA);
        }

        /// <summary>
        /// Converts a valid Rna alphabet item to an encoded item.
        /// Validates GetMaptoEncoding() method of EncodingMap.
        /// Input Data : Valid Rna Sequence with single character - "A".
        /// Output Data : Encoded byte value of an IsequenceItem - "65".
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertRnaAlphabetToEncodedItem()
        {
            ValidateEncodingMapConvertAlphabetToEncodedItem(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleRna, Alphabets.RNA, Encodings.IupacNA);
        }

        /// <summary>
        /// Converts a valid Protien alphabet item to an encoded item.
        /// Validates GetMaptoEncoding() method of EncodingMap.
        /// Input Data : Valid Protein Sequence with single character - "A".
        /// Output Data : Encoded byte value of an IsequenceItem - "1".
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertProteinAlphabetToEncodedItem()
        {
            ValidateEncodingMapConvertAlphabetToEncodedItem(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Alphabets.Protein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// Converts a valid Dna encoded item to an Alphabet item.
        /// Validates GetMaptoAlphabet() method of EncodingMap.
        /// Input Data : Encoded Dna Alphabet Item - "65".
        /// Output Data : Alphabet Item - "A".
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertDnaEncodedItemToAlphabet()
        {
            ValidateEncodingMapConvertEncodedItemToAlphabet(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleDna, Alphabets.DNA, Encodings.IupacNA);
        }

        /// <summary>
        /// Converts a valid Rna encoded item to an Alphabet item.
        /// Validates GetMaptoAlphabet() method of EncodingMap.
        /// Input Data : Encoded Rna Alphabet Item - "65".
        /// Output Data : Alphabet Item - "A".
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertRnaEncodedItemToAlphabet()
        {
            ValidateEncodingMapConvertEncodedItemToAlphabet(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleRna, Alphabets.RNA, Encodings.IupacNA);
        }

        /// <summary>
        /// Converts a valid Protein encoded item to an Alphabet item.
        /// Validates GetMaptoAlphabet() method of EncodingMap.
        /// Input Data : Encoded Protein Alphabet Item - "1".
        /// Output Data : Alphabet Item - "A".
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertProteinEncodedItemToAlphabet()
        {
            ValidateEncodingMapConvertEncodedItemToAlphabet(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Alphabets.Protein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// Converts a valid Dna sequence item to an encoding map and 
        /// validates it against expected encoded values array.
        /// Input Data : Valid Dna Sequence.
        /// Output Data : Encoded values list.
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertDnaAlphabetToEncodingMap()
        {
            ValidateEncodingMapConvertAlphabetToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleDna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// Converts a valid Rna sequence item to an encoding map and 
        /// validates it against expected encoded values array.
        /// Input Data : Valid Rna Sequence.
        /// Output Data : Encoded values list.
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertRnaAlphabetToEncodingMap()
        {
            ValidateEncodingMapConvertAlphabetToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleRna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// Converts a valid Protein sequence item to an encoding map and 
        /// validates it against expected encoded values array.
        /// Input Data : Valid Protein Sequence.
        /// Output Data : Encoded values list.
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertProteinAlphabetToEncodingMap()
        {
            ValidateEncodingMapConvertAlphabetToEncodingMap(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// Converts a valid Dna sequence encoded item to an encoding map and 
        /// validates it against expected alphabet symbol.
        /// Input Data : Valid Dna Sequence encoded item.
        /// Output Data : Valid Dna Sequence.
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertDnaEncodingToEncodingMap()
        {
            ValidateEncodingMapConvertEncodingToEncodingMap(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleDna, Encodings.IupacNA);
        }

        /// <summary>
        /// Converts a valid Rna sequence encoded item to an encoding map and 
        /// validates it against expected alphabet symbol.
        /// Input Data : Valid Rna Sequence encoded item.
        /// Output Data : Valid Rna Sequence.
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertRnaEncodingToEncodingMap()
        {
            ValidateEncodingMapConvertEncodingToEncodingMap(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleRna, Encodings.IupacNA);
        }

        /// <summary>
        /// Converts a valid Protein sequence encoded item to an encoding map and 
        /// validates it against expected alphabet symbol.
        /// Input Data : Valid Protein encoded Sequence item.
        /// Output Data : Valid Protein sequence
        /// </summary>
        [Test]
        public void ValidateEncodingMapConvertProteinEncodingToEncodingMap()
        {
            ValidateEncodingMapConvertEncodingToEncodingMap(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// Validate Encoding map alphabet property
        /// </summary>
        [Test]
        public void ValidateEncodingMapAlphabetProperty()
        {
            ValidateEncodingMapAlphabetProperty(Constants.EncodingSimpleDna, Encodings.IupacNA);
        }

        /// <summary>
        /// Validate Encoding map encoding property
        /// </summary>
        [Test]
        public void ValidateEncodingMapEncodingProperty()
        {
            ValidateEncodingMapEncodingProperty(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleDna, Encodings.IupacNA);
        }

        #endregion

        #region SequenceDecoder

        /// <summary>
        /// SequenceDecoder:Converts the decoded sequence for a byte array using Ncbi4NaEncoding
        /// and validate Sequence Decoder.
        /// Input Data : Byte value "1".
        /// Output Data : Isequence Item "A".
        /// </summary>
        [Test]
        public void ValidateSequenceDecoderWithNcbi4NAEncoding()
        {
            ValidateSequenceDecoder(Constants.SimpleDecoderNode, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceDecoder:Converts the decoded sequence for a byte array using Ncbi2NaEncoding
        /// and validate Sequence Decoder.
        /// Input Data : Byte value "1".
        /// Output Data : Isequence Item "C".
        /// </summary>
        [Test]
        public void ValidateSequenceDecoderWithNcbi2NAEncoding()
        {
            ValidateSequenceDecoder(Constants.Ncbi2NAEncodingDecoderNode, Encodings.Ncbi2NA);
        }

        /// <summary>
        /// SequenceDecoder:Converts the decoded sequence for a byte array using NcbiStdAAEncoding
        /// and validate Sequence Decoder.
        /// Input Data : Byte value "1".
        /// Output Data : Isequence Item "A".
        /// </summary>
        [Test]
        public void ValidateSequenceDecoderWithNcbiStdAAEncoding()
        {
            ValidateSequenceDecoder(Constants.NcbiStdAAEncodingDecoderNode, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// SequenceDecoder:Converts the decoded sequence for a byte array using IupacNAEncoding
        /// and validate Sequence Decoder.
        /// Input Data : Byte value "65".
        /// Output Data : Isequence Item "A".
        /// </summary>
        [Test]
        public void ValidateSequenceDecoderWithIupacNAEncoding()
        {
            ValidateSequenceDecoder(Constants.IupacNAEncodingDecoderNode, Encodings.IupacNA);
        }

        /// <summary>
        /// SequenceDecoder:Converts the decoded sequence for a byte array using NcbiStdAAEncoding
        /// and validate Sequence Decoder.
        /// Input Data : Byte value "1".
        /// Output Data : Isequence Item "A".
        /// </summary>
        [Test]
        public void ValidateSequenceDecoderWithNcbiEAAEncoding()
        {
            ValidateSequenceDecoder(Constants.NcbiEAAEncodingDecoderNode, Encodings.NcbiEAA);
        }

        /// <summary>
        /// SequenceDecoder: Validate Encoding is updated properly.
        /// Input Data : Encoding instance.
        /// Output Data: Encoding property of Sequence Decoder.
        /// </summary>
        [Test]
        public void ValidateSequenceDecoderEncodingProperty()
        {
            ValidateSequenceDecoderEncodingProperty(Encodings.IupacNA);
        }

        #endregion

        #region SequenceEncoder

        /// <summary>
        /// SequenceEncoder: Convert Dna Sequence to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Dna Sequence.
        /// Output Data : Dna encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertDnaSequenceToEncodingMap()
        {
            ValidateSequenceEncoderAlphabetToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleDna, false, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Convert Rna Sequence to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Rna Sequence.
        /// Output Data : Rna encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertRnaSequenceToEncodingMap()
        {
            ValidateSequenceEncoderAlphabetToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleRna, false, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Convert Protein Sequence to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Protein Sequence.
        /// Output Data : Protein encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertProteinSequenceToEncodingMap()
        {
            ValidateSequenceEncoderAlphabetToEncodingMap(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, false, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// SequenceEncoder: Convert Small size sequence to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Small size Sequence.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertSmallSizeSequenceToEncodingMap()
        {
            ValidateSequenceEncoderAlphabetToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.SimpleFastaDnaNodeName, false, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Convert medium size sequence to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Medium size Sequence i.e.less than 100 Kb.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertMediumSizeSequenceToEncodingMap()
        {
            ValidateSequenceEncoderAlphabetToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.MediumSizeFastaNodeName, true, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Convert large size sequence to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Large Size Sequence i.e. greater than 100 Kb and less than 350 Kb.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertLargeSizeSequenceToEncodingMap()
        {
            ValidateSequenceEncoderAlphabetToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.LargeSizeFasta, true, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Convert Dna sequence to encoded byte array; 
        /// decode the byte array back to sequence and validate it against expected Dna sequence.
        /// Input Data : Dna sequence
        /// Output Data : Dna sequence and Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderWithDecoderDnaSequence()
        {
            ValidateSequenceEncoderWithDecoder(Constants.EncodingSimpleDna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Convert Rna sequence to encoded byte array; 
        /// decode the byte array back to sequence and validate it against expected Dna sequence.
        /// Input Data : Rna sequence
        /// Output Data : Rna sequence and Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderWithDecoderRnaSequence()
        {
            ValidateSequenceEncoderWithDecoder(Constants.EncodingSimpleRna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Convert Protein sequence to encoded byte array; 
        /// decode the byte array back to sequence and validate it against expected Dna sequence.
        /// Input Data : Protein sequence
        /// Output Data : Protein sequence and Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderWithDecoderProteinSequence()
        {
            ValidateSequenceEncoderWithDecoder(Constants.EncodingSimpleProtein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// SequenceEncoder: Converts Dna character to encoded byte and 
        /// validates it against encoded byte.
        /// Input Data : Dna char
        /// Output Data : Encoded byte.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertDnaCharToByte()
        {
            ValidateSequenceEncoderConvertChartToByte(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleDna, Encodings.IupacNA);
        }

        /// <summary>
        /// SequenceEncoder: Converts Rna character to encoded byte and 
        /// validates it against encoded byte.
        /// Input Data : Rna char
        /// Output Data : Encoded byte.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertRnaCharToByte()
        {
            ValidateSequenceEncoderConvertChartToByte(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleRna, Encodings.IupacNA);
        }

        /// <summary>
        /// SequenceEncoder: Converts Protein character to encoded byte and 
        /// validates it against encoded byte.
        /// Input Data : Protein char
        /// Output Data : Encoded byte.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertProteinCharToByte()
        {
            ValidateSequenceEncoderConvertChartToByte(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// SequenceEncoder: Converts Dna sequence item to encoded byte and 
        /// validates it against encoded byte.
        /// Input Data : Dna sequence item
        /// Output Data : Encoded byte.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertDnaSequenceToByte()
        {
            ValidateSequenceEncoderConvertSequenceToByte(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleDna, Encodings.IupacNA);
        }

        /// <summary>
        /// SequenceEncoder: Converts Rna sequence item to encoded byte and 
        /// validates it against encoded byte.
        /// Input Data : Rna sequence item
        /// Output Data : Encoded byte.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertRnaSequenceToByte()
        {
            ValidateSequenceEncoderConvertSequenceToByte(Constants.IupacNaEncodedValue,
                Constants.EncodingSimpleRna, Encodings.IupacNA);
        }

        /// <summary>
        /// SequenceEncoder: Converts Protein sequence item to encoded byte and 
        /// validates it against encoded byte.
        /// Input Data : Protein sequence item.
        /// Output Data : Encoded byte.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertProteinSequenceToByte()
        {
            ValidateSequenceEncoderConvertSequenceToByte(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Dna sequence string to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Dna Sequence string.
        /// Output Data : Rna encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertDnaSequenceStringToEncodingMap()
        {
            ValidateSequenceEncoderSequenceStringToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleDna, false, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Rna sequence string to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Rna Sequence string.
        /// Output Data : Rna encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertRnaSequenceStringToEncodingMap()
        {
            ValidateSequenceEncoderSequenceStringToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleRna, false, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Protein Sequence string to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Protein Sequence string.
        /// Output Data : Protein encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertProteinSequenceStringToEncodingMap()
        {
            ValidateSequenceEncoderSequenceStringToEncodingMap(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, false, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Small size sequence string to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Small size Sequence string.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertSmallSizeSequenceStringToEncodingMap()
        {
            ValidateSequenceEncoderSequenceStringToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.SimpleFastaDnaNodeName, false, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid medium size sequence string to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Medium size Sequence string i.e.less than 100 Kb.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertMediumSizeSequenceStringToEncodingMap()
        {
            ValidateSequenceEncoderSequenceStringToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.MediumSizeFastaNodeName, true, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid large size sequence string to encoded byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid large size Sequence string i.e.greater than 100 Kb and less than 350 Kb.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertLargeSizeSequenceStringToEncodingMap()
        {
            ValidateSequenceEncoderSequenceStringToEncodingMap(Constants.Ncbi4NaEncodedValue,
                Constants.LargeSizeFasta, true, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Dna sequence to encoded byte array using target byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Dna Sequence and target byte array
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertDnaSequenceToTargetByteArray()
        {
            ValidateSequenceEncoderAlphabetToTargetByteArray(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleDna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Rna sequence to encoded byte array 
        /// using target byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Rna Sequence and target byte array
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertRnaSequenceToTargetByteArray()
        {
            ValidateSequenceEncoderAlphabetToTargetByteArray(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleRna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Protein sequence to encoded byte array 
        /// using target byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Protein Sequence and target byte array
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertProteinSequenceToTargetByteArray()
        {
            ValidateSequenceEncoderAlphabetToTargetByteArray(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Dna sequence string to encoded byte array 
        /// using target byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Dna Sequence string and target byte array
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertDnaSequenceStringToTargetByteArray()
        {
            ValidateSequenceEncoderSequenceStringToTargetByteArray(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleDna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Rna sequence string to encoded byte array 
        /// using target byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Rna Sequence string and target byte array
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertRnaSequenceStringToTargetByteArray()
        {
            ValidateSequenceEncoderSequenceStringToTargetByteArray(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleRna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Protein sequence string to encoded byte array 
        /// using target byte array and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Protein Sequence string and target byte array
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertProteinSequenceStringToTargetByteArray()
        {
            ValidateSequenceEncoderSequenceStringToTargetByteArray(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Dna sequence to encoded byte array using 
        /// target byte array with offset and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Dna Sequence, target byte array and offset.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertDnaSequenceToTargetByteArrayWithOffset()
        {
            ValidateSequenceEncoderAlphabetToTargetByteArrayWithOffset(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleDna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Rna sequence to encoded byte array using 
        /// target byte array with offset and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Rna Sequence, target byte array and offset.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertRnaSequenceToTargetByteArrayWithOffset()
        {
            ValidateSequenceEncoderAlphabetToTargetByteArrayWithOffset(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleRna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Protein sequence to encoded byte array using 
        /// target byte array with offset and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Protein Sequence, target byte array and offset.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertProteinSequenceToTargetByteArrayWithOffset()
        {
            ValidateSequenceEncoderAlphabetToTargetByteArrayWithOffset(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Encodings.NcbiStdAA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Dna sequence string to encoded byte array using
        /// target byte array with offset and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Dna Sequence string, target byte array and offset.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertDnaSequenceStringToTargetByteArrayWithOffset()
        {
            ValidateSequenceEncoderSequenceStringToTargetByteArrayWithOffset(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleDna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Rna sequence string to encoded byte array using 
        /// target byte array with offset and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Rna Sequence string, target byte array and offset.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertRnaSequenceStringToTargetByteArrayWithOffset()
        {
            ValidateSequenceEncoderSequenceStringToTargetByteArrayWithOffset(Constants.Ncbi4NaEncodedValue,
                Constants.EncodingSimpleRna, Encodings.Ncbi4NA);
        }

        /// <summary>
        /// SequenceEncoder: Converts valid Protein sequence string to encoded byte array using 
        /// target byte array with offset and 
        /// validates it against expected encoded values.
        /// Input Data : Valid Protein Sequence string, target byte array and offset.
        /// Output Data : Encoded byte array.
        /// </summary>
        [Test]
        public void ValidateSequenceEncoderConvertProteinSequenceStringToTargetByteArrayWithOffset()
        {
            ValidateSequenceEncoderSequenceStringToTargetByteArrayWithOffset(Constants.NcbiStdAAEncodedValue,
                Constants.EncodingSimpleProtein, Encodings.NcbiStdAA);
        }

        #endregion

        #region IupacNAEncoding

        /// <summary>
        /// Validates Dna sequence is present in IupacNAEncoding using contains() method.
        /// Input Data : Valid Dna sequence.
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingContainsWithDnaSequence()
        {
            ValidateEncodingWithAlphabetContains(IupacNAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates Rna sequence is present in IupacNAEncoding using contains() method.
        /// Input Data : Valid Rna sequence.
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingContainsWithRnaSequence()
        {
            ValidateEncodingWithAlphabetContains(IupacNAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Copies the nucleotides collection of IupacNAEncoding into an array and 
        /// Validates if copied collection contains Dna sequence.
        /// Input Data : Valid Dna sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingCopyToWithDnaSequence()
        {
            ValidateEncodingWithAlphabetCopyTo(IupacNAEncoding.Instance,
                Constants.NucleotidesListLength, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Copies the nucleotides collection of IupacNAEncoding into an array and 
        /// Validates if copied collection contains Rna sequence.
        /// Input Data : Valid Rna sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingCopyToWithRnaSequence()
        {
            ValidateEncodingWithAlphabetCopyTo(IupacNAEncoding.Instance, Constants.NucleotidesListLength,
                Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates if IupacNAEncoding LookUpBySymbol() returns expected symbol by passing valid Dna char.
        /// Input Data : Valid Dna Char
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingLookBySymbolWithDnaChar()
        {
            ValidateEncodingLookUpByChar(IupacNAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates if IupacNAEncoding LookUpBySymbol() returns expected symbol by passing valid Rna char.
        /// Input Data : Valid Rna Char
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingLookBySymbolWithRnaChar()
        {
            ValidateEncodingLookUpByChar(IupacNAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates if IupacNAEncoding LookUpBySymbol() returns expected symbol by passing valid 
        /// Dna sequence string.
        /// Input Data : Valid Dna Sequence string
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingLookBySymbolWithDnaString()
        {
            ValidateEncodingLookUpByString(IupacNAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates if IupacNAEncoding LookUpBySymbol() returns expected symbol by passing valid
        /// Rna sequence string.
        /// Input Data : Valid Rna Sequence string
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingLookBySymbolWithRnaString()
        {
            ValidateEncodingLookUpByString(IupacNAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates if IupacNAEncoding LookUpBySymbol() returns expected symbol by passing valid 
        /// Dna byte.
        /// Input Data : Valid Dna byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingLookBySymbolWithDnaByte()
        {
            ValidateEncodingLookUpByByte(IupacNAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates if IupacNAEncoding LookUpBySymbol() returns expected symbol by passing valid Rna byte.
        /// Input Data : Valid Rna byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingLookBySymbolWithRnaByte()
        {
            ValidateEncodingLookUpByByte(IupacNAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates all properties of sequence item using IupacNAEncoding LookUpBySymbol() 
        /// returns expected symbol by passing valid Dna byte.
        /// Input Data : Valid Dna byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateIupacNAEncodingLookBySymbolAllProperties()
        {
            ValidateEncodingLookUpBySymbol(IupacNAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        #endregion

        #region Ncbi2NAEncoding

        /// <summary>
        /// Validates Dna sequence is present in Ncbi2NAEncoding using contains() method.
        /// Input Data : Valid Dna sequence.
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingContainsWithDnaSequence()
        {
            ValidateEncodingWithAlphabetContains(Ncbi2NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates Rna sequence is present in Ncbi2NAEncoding using contains() method.
        /// Input Data : Valid Rna sequence.
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingContainsWithRnaSequence()
        {
            ValidateEncodingWithAlphabetContains(Ncbi2NAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Copies the nucleotides collection of Ncbi2NAEncoding into an array and 
        /// Validates if copied collection contains Dna sequence.
        /// Input Data : Valid Dna sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingCopyToWithDnaSequence()
        {
            ValidateEncodingWithAlphabetCopyTo(Ncbi2NAEncoding.Instance, Constants.NucleotidesListLength,
                Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Copies the nucleotides collection of Ncbi2NAEncoding into an array and 
        /// Validates if copied collection contains Rna sequence.
        /// Input Data : Valid Rna sequence item
        /// Output Data : Method should return true value.
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingCopyToWithRnaSequence()
        {
            ValidateEncodingWithAlphabetCopyTo(Ncbi2NAEncoding.Instance, Constants.NucleotidesListLength,
                Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates if Ncbi2NAEncoding LookUpBySymbol() returns expected symbol by passing valid Dna char.
        /// Input Data : Valid Dna Char
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingLookBySymbolWithDnaChar()
        {
            ValidateEncodingLookUpByChar(Ncbi2NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates if Ncbi2NAEncoding LookUpBySymbol() returns expected symbol by passing valid Rna char.
        /// Input Data : Valid Rna Char
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingLookBySymbolWithRnaChar()
        {
            ValidateEncodingLookUpByChar(Ncbi2NAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates if Ncbi2NAEncoding LookUpBySymbol() returns expected symbol by passing 
        /// valid Dna sequence string.
        /// Input Data : Valid Dna Sequence string
        /// Output Data : Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingLookBySymbolWithDnaString()
        {
            ValidateEncodingLookUpByString(Ncbi2NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates if Ncbi2NAEncoding LookUpBySymbol() returns expected symbol by passing valid 
        /// Rna sequence string.
        /// Input Data : Valid Rna Sequence string
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingLookBySymbolWithRnaString()
        {
            ValidateEncodingLookUpByString(Ncbi2NAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates if Ncbi2NAEncoding LookUpBySymbol() returns expected symbol by passing valid Dna byte.
        /// Input Data : Valid Dna byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingLookBySymbolWithDnaByte()
        {
            ValidateEncodingLookUpByByte(Ncbi2NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        /// <summary>
        /// Validates if Ncbi2NAEncoding LookUpBySymbol() returns expected symbol by passing valid Rna byte.
        /// Input Data : Valid Rna byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingLookBySymbolWithRnaByte()
        {
            ValidateEncodingLookUpByByte(Ncbi2NAEncoding.Instance, Constants.EncodingSimpleRna);
        }

        /// <summary>
        /// Validates all properties of sequence item using Ncbi2NAEncoding LookUpBySymbol() 
        /// returns expected symbol by passing valid Dna byte.
        /// Input Data : Valid Dna byte
        /// Output Data: Look Up Symbol
        /// </summary>
        [Test]
        public void ValidateNcbi2NAEncodingLookBySymbolAllProperties()
        {
            ValidateEncodingLookUpBySymbol(Ncbi2NAEncoding.Instance, Constants.EncodingSimpleDna);
        }

        #endregion

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates sequence using expected sequence and validates the same.
        /// </summary>
        /// <returns>It returns sequence object.</returns>
        Sequence CreateandValidateSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.EncodingSimpleDna,
              Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.EncodingSimpleDna,
              Constants.ExpectedSequence);


            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate Created Sequence.
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence {0} is expected.", createSequence.ToString()));

            return createSequence;
        }

        /// <summary>
        /// Add a valid sequence item to Encoding and 
        /// validates if Add() method is throwing exception.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        void InValidateEncodingAdd(IEncoding encoding)
        {
            bool exThrown = false;

            // Create and validate new sequence
            Sequence newSequence = CreateandValidateSequence();

            // Add sequence item to encoding and validate if 
            // it is throwing exception.
            try
            {
                encoding.Add(newSequence[0]);
            }
            catch (Exception)
            {
                exThrown = true;
            }

            // Validate if Add() method has thrown exception.
            Assert.IsTrue(exThrown);
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null, "Encoding P1 : Validation of Add() method completed"));
        }

        /// <summary>
        /// Validates Remove() method of Encoding is throwing exception using readonly sequence list.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        void InValidateEncodingRemove(IEncoding encoding)
        {
            bool exThrown = false;

            // Create and validate new sequence
            Sequence newSequence = CreateandValidateSequence();

            // Mark Sequence as Readonly.
            newSequence.IsReadOnly = true;

            // Validate if ReadOnly Sequence is throwing an error when try to delete sequence data.
            try
            {
                encoding.Remove(newSequence[0]);
            }
            catch (Exception)
            {
                exThrown = true;
            }

            // Validate if Remove method is throwing an exception.
            Assert.IsTrue(exThrown);
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of Remove() method using readonly sequence list is completed"));
        }

        /// <summary>
        /// Validates if Clear() method of Encoding is throwing excpetion.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        void InValidateEncodingClear(IEncoding encoding)
        {
            bool exThrown = false;

            // Create and validate new sequence
            CreateandValidateSequence();

            // Validate if Clear() method is throwing an exception.
            try
            {
                encoding.Clear();
            }
            catch (Exception)
            {
                exThrown = true;
            }

            // Validate if exception is thrown
            Assert.IsTrue(exThrown);
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of Clear() method is completed"));
        }

        /// <summary>
        /// Validates if sequence item is present in Encoding using Contains() method.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="lookUpInput">lookUpInput to get the expected char.</param>
        void ValidateEncodingContains(IEncoding encoding, string lookUpInput)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string expectedString = Utility._xmlUtil.GetTextValue(Constants.LookUpNode, lookUpInput);
            bool result = false;

            // Call Contains Method.
            foreach (char expectedChar in expectedString.ToCharArray())
            {
                ISequenceItem seqItem = encoding.LookupBySymbol(expectedChar);
                result = encoding.Contains(seqItem);

                // Validate whether or not IsequenceItem is in encoding.
                Assert.IsTrue(result);
            }

            Console.WriteLine(string.Format(null, "Encoding P1 : Validation of contains() method is completed sucessfully."));
        }

        /// <summary>
        /// Validates if valid alphabet sequence item is present in Encoding using Contains() method.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="lookUpInput">lookUpInput to get the expected char.</param>
        void ValidateEncodingWithAlphabetContains(IEncoding encoding, string alphabetNode)
        {
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.ExpectedSequence);
            bool result = false;

            // Call Contains Method.
            foreach (char inputchar in actualSequence.ToCharArray())
            {
                ISequenceItem seqItem = encoding.LookupBySymbol(inputchar);
                result = encoding.Contains(seqItem);

                // Validate whether or not IsequenceItem is in encoding.
                Assert.IsTrue(result);
            }

            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of contains() method for {0} is completed sucessfully.",
              alphabetName));
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Validation of contains() method for {0} is completed sucessfully.",
              alphabetName));
        }

        /// <summary>
        /// Validates GetEnumerator() method of Encoding.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        void ValidateEncodingGetEnumerator(IEncoding encoding)
        {
            // Gets the sequence list using GetEnumerator()
            IEnumerator<ISequenceItem> list;
            list = encoding.GetEnumerator();

            // Validates if enumerator list is not empty.
            Assert.IsNotNull(list);
            Console.WriteLine(string.Format(null, "Encoding P1 : Validation of GetEnumerator is completed."));
        }

        /// <summary>
        /// Validates if CopyTo() method of Encoding copies expected sequence list to array.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="alphabetSymbol">Alphabet Symbol</param>
        /// <param name="byteValue">Byte Value</param>
        /// <param name="itemLengthNode">Item length node</param>
        void ValidateEncodingCopyTo(IEncoding encoding, string alphabetSymbol,
          string byteValue, string itemLengthNode)
        {
            // Get Value from xml.
            string expectedSymbol = Utility._xmlUtil.GetTextValue(Constants.EncodingNode, alphabetSymbol);
            string encodedValue = Utility._xmlUtil.GetTextValue(Constants.EncodingNode, byteValue);
            string itemLength = Utility._xmlUtil.GetTextValue(Constants.EncodingNode, itemLengthNode);
            string[] expectedSymbols = expectedSymbol.Split(',');
            string[] encodedValues = encodedValue.Split(',');

            // Copies sequence list into an array.
            ISequenceItem[] seqItems = new ISequenceItem[int.Parse(itemLength, (IFormatProvider)null)];
            encoding.CopyTo(seqItems, 0);

            // Validate copied array.
            Assert.AreEqual(expectedSymbols.Length, seqItems.Length);
            Assert.AreEqual(encodedValues.Length, seqItems.Length);
            for (int seqCounter = 0; seqCounter < seqItems.Length; seqCounter++)
            {
                Assert.AreEqual(encodedValues[seqCounter],
                  seqItems[seqCounter].Value.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedSymbols[seqCounter], seqItems[seqCounter].Symbol.ToString());
            }

            Console.WriteLine(string.Format(null,
              "Encoding P1 : Copies of Nucleaotides to array is completed."));
        }

        /// <summary>
        /// Validates if CopyTo() method of Encoding copies alpha sequence list to array.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="itemLengthNode">Item length node</param>
        /// <param name="alphabetNode">Alphabet node</param>
        void ValidateEncodingWithAlphabetCopyTo(IEncoding encoding,
          string itemLengthNode, string alphabetNode)
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequence);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string itemLength = Utility._xmlUtil.GetTextValue(Constants.EncodingNode, itemLengthNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Get Encoded value.
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encoding);

            // Create a ISequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(mapToEncoding.Convert(item));
            }

            // Copies sequence list into an array.
            ISequenceItem[] seqItems = new ISequenceItem[int.Parse(itemLength, (IFormatProvider)null)];
            encoding.CopyTo(seqItems, 0);

            // Validate if copied array copies the alphabet sequence.
            int alphaSeqCount = 0;
            foreach (ISequenceItem alphaSeq in alphaList)
            {
                for (int iseqItem = 0; iseqItem < seqItems.Length; iseqItem++)
                {
                    if (seqItems[iseqItem] == alphaSeq)
                    {
                        alphaSeqCount++;
                    }
                }
            }

            Assert.AreEqual(alphaList.Count, alphaSeqCount);
            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validates copy of encoded sequence list contains {0} alphabet sequences as expected .", alphaList.Count));
            Console.WriteLine(string.Format(null,
              "Encoding P1 : Copy of encoded sequence list to array is validated."));
        }

        /// <summary>
        /// Validates if LookUp symbol returns expected symbol by passing char.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="lookUpInput">Node to get input char.</param>
        /// <param name="lookUpOutput">Node to get output symbol</param>
        void ValidateEncodingLookUpByChar(IEncoding encoding, string lookUpInput, string lookUpOutput)
        {
            // Gets LookUp() values from xml.
            string inputString = Utility._xmlUtil.GetTextValue(Constants.LookUpNode, lookUpInput);
            string expectedLookUpSymbol = Utility._xmlUtil.GetTextValue(Constants.LookUpNode, lookUpOutput);

            // Gets the symbol using LookUpBySymbol() method.
            // Create output string using each symbol string.
            int index = 0;
            foreach (char inputChar in inputString.ToCharArray())
            {
                ISequenceItem seqItem = encoding.LookupBySymbol(inputChar);

                // Validates LookUpBySymbol() if all symbols matches expected output symbol.
                Assert.AreEqual(seqItem.Symbol.ToString(), expectedLookUpSymbol[index].ToString());
                index++;
            }

            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of LookUp symbol by passing char is completed."));
        }

        /// <summary>
        /// Validates if LookUp symbol returns expected symbol by passing sequence char.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="alphabetNode">Alphabet xml node.</param>
        void ValidateEncodingLookUpByChar(IEncoding encoding, string alphabetNode)
        {
            // Gets sequence values from xml.
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.ExpectedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);

            // Get Encoded Sequence item list.
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encoding);
            List<ISequenceItem> expectedSeqItemList = new List<ISequenceItem>();
            foreach (ISequenceItem seqItem in seq)
            {
                expectedSeqItemList.Add(mapToEncoding.Convert(seqItem));
            }

            // Gets the symbol using LookUpBySymbol() method.
            int seqCounter = 0;
            foreach (char inputChar in actualSequence.ToCharArray())
            {
                ISequenceItem seqItem = encoding.LookupBySymbol(inputChar);

                // Validates LookUpBySymbol() if each symbol matches expected symbol
                Assert.AreEqual(seqItem.Symbol.ToString(null),
                  expectedSeqItemList[seqCounter].Symbol.ToString(null));
                seqCounter++;
            }

            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of LookUp symbol by passing sequence char is completed."));
        }

        /// <summary>
        /// Validates if LookUp symbol returns expected symbol by passing sequence string.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="alphabetNode">Alphabet xml node.</param>
        void ValidateEncodingLookUpByString(IEncoding encoding, string alphabetNode)
        {
            // Gets sequence values from xml.
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.ExpectedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);

            // Get Encoded Sequence item list.
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encoding);
            List<ISequenceItem> expectedSeqItemList = new List<ISequenceItem>();
            foreach (ISequenceItem seqItem in seq)
            {
                expectedSeqItemList.Add(mapToEncoding.Convert(seqItem));
            }

            // Gets the symbol using LookUpBySymbol() method.
            int seqCounter = 0;
            foreach (char inputChar in actualSequence.ToCharArray())
            {
                ISequenceItem seqItem = encoding.LookupBySymbol(inputChar.ToString());

                // Validates LookUpBySymbol() if each symbol matches expected symbol
                Assert.AreEqual(seqItem.Symbol.ToString(null),
                  expectedSeqItemList[seqCounter].Symbol.ToString(null));
                seqCounter++;
            }
            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of LookUp symbol by passing string is completed."));
        }

        /// <summary>
        /// Validates if LookUp symbol returns expected symbol by passing string.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="lookUpInput">Node to get input char.</param>
        /// <param name="lookUpOutput">Node to get output symbol</param>
        void ValidateEncodingLookUpByString(IEncoding encoding, string lookUpInput, string lookUpOutput)
        {
            // Gets LookUp() values from xml.
            string inputString = Utility._xmlUtil.GetTextValue(Constants.LookUpNode, lookUpInput);
            string expectedLookUpString = Utility._xmlUtil.GetTextValue(Constants.LookUpNode, lookUpOutput);

            int index = 0;
            foreach (char inputChar in inputString.ToCharArray())
            {
                ISequenceItem seqItem = encoding.LookupBySymbol(inputChar.ToString());

                // Validates LookUpBySymbol() if symbol matches expected symbol
                Assert.AreEqual(expectedLookUpString[index].ToString(), seqItem.Symbol.ToString());
                index++;
            }
            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of LookUp symbol by passing string is completed."));
        }

        /// <summary>
        /// Validates if LookUp symbol returns expected symbol by passing byte.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="lookUpSymbol">Node to get output symbol.</param>
        /// <param name="lookUpByte">Node to get input byte.</param>
        void ValidateEncodingLookUpByByte(IEncoding encoding, string outputSymbol, string inputByte)
        {
            // Gets LookUp() values from xml.
            string expectedSymbol = Utility._xmlUtil.GetTextValue(Constants.EncodingNode, outputSymbol);
            string inputByteValue = Utility._xmlUtil.GetTextValue(Constants.EncodingNode, inputByte);
            string[] inputByteValues = inputByteValue.Split(',');
            string[] expectedSymbols = expectedSymbol.Split(',');

            // Gets the symbol using LookUpBySymbol() method.
            for (int byteIndexer = 0; byteIndexer < inputByteValues.Length; byteIndexer++)
            {
                ISequenceItem seqItem =
                  encoding.LookupByValue(byte.Parse(inputByteValues[byteIndexer], (IFormatProvider)null));

                // Validates LookUpBySymbol() if symbol matches expected symbol
                Assert.AreEqual(expectedSymbols[byteIndexer], seqItem.Symbol.ToString());
            }

            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of LookUp symbol by passing byte is completed."));
        }

        /// <summary>
        /// Validates if LookUp symbol returns expected symbol by passing sequence byte.
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="alphabetNode">Alphabet xml node.</param>
        void ValidateEncodingLookUpByByte(IEncoding encoding, string alphabetNode)
        {
            // Gets sequence values from xml.
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.ExpectedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);

            foreach (ISequenceItem inputSeq in seq)
            {
                // Gets Encoded value.
                EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encoding);
                ISequenceItem expectedSeqItem = mapToEncoding.Convert(inputSeq);

                // Gets the symbol using LookUpBySymbol() method.
                ISequenceItem seqItem = null;
                seqItem = encoding.LookupByValue(expectedSeqItem.Value);

                // Validates LookUpBySymbol() if symbol matches expected symbol
                Assert.AreEqual(expectedSeqItem.Symbol.ToString(null), seqItem.Symbol.ToString(null));
            }
            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of LookUp symbol by passing byte is completed."));
        }

        /// <summary>
        /// Validates all properties of sequence item by executing LookUpBySymbol().
        /// </summary>
        /// <param name="encoding">Encoding instance.</param>
        /// <param name="alphabetNode">Alphabet xml node.</param>
        void ValidateEncodingLookUpBySymbol(IEncoding encoding, string alphabetNode)
        {
            // Gets sequence values from xml.
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.ExpectedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);

            foreach (ISequenceItem item in seq)
            {
                // Get encoded sequence item.
                EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encoding);
                ISequenceItem expectedSeqItem = mapToEncoding.Convert(item);

                // Get the sequence item using LookUpBySymbol() method.
                ISequenceItem seqItem = null;
                seqItem = encoding.LookupBySymbol(item.Symbol.ToString());

                // Validate all properties
                Assert.AreEqual(seqItem.Symbol, expectedSeqItem.Symbol);
                Assert.AreEqual(seqItem.Name, expectedSeqItem.Name);
                Assert.AreEqual(seqItem.Value, expectedSeqItem.Value);
            }
            Console.WriteLine(string.Format(null,
              "Encoding P1 : Validation of all properties of sequence item using LookUp symbol is completed."));
        }

        /// <summary>
        /// Converts Alphabet item to encoded item and validate the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="alphabet">alphabet instance.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateEncodingMapConvertAlphabetToEncodedItem(string encodingNode,
          string nodeName, IAlphabet alphabet, IEncoding encodings)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string encodedValue = Utility._xmlUtil.GetTextValue(nodeName, encodingNode);
            string[] encodedValues = encodedValue.Split(',');
            ISequenceItem convertedIseqItem = null;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : The Sequence {0} is expected.", actualSequence));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);
            int iencoded = 0;
            foreach (ISequenceItem seqItem in createSequence)
            {
                // Converts that Sequence items into encoded item
                convertedIseqItem = mapToEncoding.Convert(seqItem);

                // Validates if encoded IsequenceItem value matches expected encoded value.
                Assert.AreEqual(encodedValues[iencoded],
                  convertedIseqItem.Value.ToString((IFormatProvider)null));
                ApplicationLog.WriteLine(string.Format(null,
                  "Encoding P1 : Encoded value {0} is expected.", convertedIseqItem.Value));
                Console.WriteLine(string.Format(null,
                  "Encoding P1 : Encoded value {0} is expected.", convertedIseqItem.Value));
                iencoded++;
            }
            ApplicationLog.WriteLine(
              "Encoding P1 : Conversion of sequence item to encoded item is completed successfully.");
            Console.WriteLine(
              "Encoding P1 : Conversion of sequence item to encoded item is completed successfully.");
        }

        /// <summary>
        /// Converts a Valid encoded item to an Alphabet item and validate the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="alphabet">alphabet instance.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateEncodingMapConvertEncodedItemToAlphabet(string encodingNode,
          string nodeName, IAlphabet alphabet, IEncoding encodings)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string encodedValue = Utility._xmlUtil.GetTextValue(nodeName, encodingNode);
            string[] encodedValues = encodedValue.Split(',');
            ISequenceItem encodedSeqItem = null;
            ISequenceItem alphabetItem = null;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 :The Sequence {0} is expected.", actualSequence));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);
            EncodingMap mapToAlphabet = EncodingMap.GetMapToAlphabet(encodings, alphabet);

            int iencoded = 0;
            foreach (ISequenceItem seqItem in createSequence)
            {
                // Converts the Sequence items into encoded items
                encodedSeqItem = mapToEncoding.Convert(seqItem);

                // Validates if encoded IsequenceItem value matches expected encoded value.
                Assert.AreEqual(encodedValues[iencoded],
                  encodedSeqItem.Value.ToString((IFormatProvider)null));
                ApplicationLog.WriteLine(string.Format(null,
                  "Encoding P1 :Encoded value {0} is expected.", encodedSeqItem.Value));

                // Converts encoded item to alphabet Isequence item.
                alphabetItem = mapToAlphabet.Convert(encodedSeqItem);

                // Validates if alphabet sequence item matches expected alphabet item.
                Assert.AreEqual(seqItem.Symbol.ToString(), alphabetItem.Symbol.ToString());
                ApplicationLog.WriteLine(string.Format(null,
                  "Encoding P1 :Alphabet name {0} is expected.", alphabetItem.Symbol));
                Console.WriteLine(string.Format(null,
                  "Encoding P1 :Alphabet name {0} is expected.", alphabetItem.Symbol));
                iencoded++;
            }
            ApplicationLog.WriteLine(
              "Encoding P1 : Conversion of encoded item to alphabet item is completed successfully.");
            Console.WriteLine(
              "Encoding P1 : Conversion of encoded item to alphabet item is completed successfully.");
        }

        /// <summary>
        /// EncodingMap: Gets encoded values list for alphabet sequence using GetMapToEncoding() 
        /// and validates against expected encoding values.
        /// Input Data : Valid Alphabet and encoding.
        /// Output Data : Valid map.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">xml alphabet node.</param>
        /// <param name="encodings">encodings instance.</param>
        void ValidateEncodingMapConvertAlphabetToEncodingMap(string encodingNode,
          string alphabetNode, IEncoding encodings)
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequence);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string[] expectedArray = expectedByteArray.Split(',');

            // Get Encoded value.
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Create a ISequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(mapToEncoding.Convert(item));
            }

            // Encode IsequenceList Item to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encodings);
            byte[] encodedValue = iSeq.Encode(alphaList);

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(expectedArray.Length, encodedValue.Length);
            ApplicationLog.WriteLine(String.Format((IFormatProvider)null,
              "Encoding P1 : The length Isequence Item list matches the expected length {0}.", encodedValue.Length));
            Console.WriteLine(String.Format((IFormatProvider)null,
              "Encoding P1 : The length Isequence Item list matches the expected length {0}.", encodedValue.Length));

            for (int iencoded = 0; iencoded < encodedValue.Length; iencoded++)
            {
                Assert.AreEqual(encodedValue[iencoded].ToString((IFormatProvider)null), expectedArray[iencoded]);
            }
            ApplicationLog.WriteLine(
              "Encoding P1 : Conversion and Validation of Isequence Item list to byte array is complated.");
            Console.WriteLine(
              "Encoding P1 : Conversion and Validation of Isequence Item list to byte array is complated.");
        }

        /// <summary>
        /// EncodingMap: Converts alphabet sequence item from the encoded 
        /// sequence item using GetMapToAlphabet() 
        /// and validates against expected alphabet sequence item.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">xml alphabet node.</param>
        /// <param name="encodings">encodings instance.</param>
        void ValidateEncodingMapConvertEncodingToEncodingMap(string encodingNode,
          string alphabetNode, IEncoding encodings)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.ExpectedSequence);
            string encodedValue = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string[] encodedValues = encodedValue.Split(',');
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            ISequenceItem encodedSeqItem = null;
            ISequenceItem alphabetItem = null;

            // Create new sequence.
            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Get encoding map to alphabet and encoding instance.
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);
            EncodingMap mapToAlphabet = EncodingMap.GetMapToAlphabet(encodings, alphabet);
            int iencoded = 0;
            foreach (ISequenceItem seqItem in createSequence)
            {
                // Convert the Sequence items into encoded items
                encodedSeqItem = mapToEncoding.Convert(seqItem);

                // Validate the Converted IsequenceItem
                Assert.AreEqual(encodedValues[iencoded],
                  encodedSeqItem.Value.ToString((IFormatProvider)null));
                ApplicationLog.WriteLine(string.Format(null,
                  "Encoding P1 :Encoded value {0} is expected.", encodedSeqItem.Value));

                // Convert encoded items to alphabet Isequence item.
                alphabetItem = mapToAlphabet.Convert(encodedSeqItem);

                // Validate the alphabet sequence item.
                Assert.AreEqual(actualSequence[iencoded].ToString(), alphabetItem.Symbol.ToString());
                iencoded++;
            }
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 :Alphabet name {0} is expected.", alphabetItem.Symbol));
            ApplicationLog.WriteLine(
              "Encoding BVT: Conversion of encoded item to alphabet item is completed successfully.");
        }

        /// <summary>
        /// EncodingMap: Converts alphabet sequence item using GetMapToAlphabet() 
        /// and validates against expected alphabet sequence item.
        /// </summary>
        /// <param name="alphabetNode">xml alphabet node.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateEncodingMapAlphabetProperty(string alphabetNode, IEncoding encodings)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.ExpectedSequence);
            IAlphabet expectedAlphabet = Utility.GetAlphabet(alphabetName);

            ISequenceItem alphabetItem = null;

            // Create new sequence
            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            EncodingMap mapToAlphabet = EncodingMap.GetMapToAlphabet(encodings, expectedAlphabet);
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(expectedAlphabet, encodings);

            int seqCounter = 0;
            foreach (ISequenceItem item in createSequence)
            {
                // Convert the Sequence items into encoded items
                ISequenceItem encodedSeqItem = mapToEncoding.Convert(item);

                // Convert encoded items to alphabet Isequence item.
                alphabetItem = mapToAlphabet.Convert(encodedSeqItem);

                // Validate the alphabet sequence item.
                Assert.AreEqual(actualSequence[seqCounter].ToString(), alphabetItem.Symbol.ToString());
                seqCounter++;
            }
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 :Alphabet name {0} is expected.", alphabetItem.Symbol));
            Console.WriteLine(string.Format(null,
              "Encoding P1 :Alphabet name {0} is expected.", alphabetItem.Symbol));
            ApplicationLog.WriteLine("Encoding P1 : Validation of alphabet item is completed successfully.");
            Console.WriteLine("Encoding P1 : Validation of alphabet item is completed successfully.");
        }

        /// <summary>
        /// Converts encoded sequence item using GetMapToEncoding() 
        /// and validates against expected encoded sequence item.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="encodings">encodings instance.</param>
        void ValidateEncodingMapEncodingProperty(string encodingNode,
          string alphabetNode, IEncoding encodings)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.ExpectedSequence);
            string encodedValue = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            ISequenceItem encodedSeqItem = null;
            string[] encodedValues = encodedValue.Split(',');

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);
            int iencoded = 0;
            foreach (ISequenceItem seqItem in createSequence)
            {
                // Convert the Sequence items into encoded items
                encodedSeqItem = mapToEncoding.Convert(seqItem);

                // Validate the encoded item.
                Assert.AreEqual(encodedValues[iencoded],
                  encodedSeqItem.Value.ToString((IFormatProvider)null));
                iencoded++;
            }
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 :Encoded value {0} is expected.", encodedSeqItem.Value));
            Console.WriteLine(string.Format(null,
              "Encoding P1 :Encoded value {0} is expected.", encodedSeqItem.Value));
        }

        /// <summary>
        /// Converts the decoded sequence for a byte array using encodings 
        /// and validate Sequence Decoder.
        /// </summary>
        /// <param name="decoderNode">xml root node.</param>
        /// <param name="byteNode">xml node to get byte value</param>
        /// <param name="encodings">encodings instance.</param>
        void ValidateSequenceDecoder(string decoderNode, IEncoding encodings)
        {
            // Gets the byte value from xml.
            string alphabetName = Utility._xmlUtil.GetTextValue(decoderNode, Constants.DecoderAlphabetName);
            string alphabetSymbol = Utility._xmlUtil.GetTextValue(decoderNode, Constants.AlphabetSymbol);
            string inputByteValue = Utility._xmlUtil.GetTextValue(decoderNode, Constants.DecoderByteValue);
            string[] alphabetNames =
              alphabetName.Replace("\r", "").Replace("\n", "").Trim().Split(',');
            string[] alphabetSymbols =
              alphabetSymbol.Replace("\r", "").Replace("\n", "").Trim().Split(',');
            string[] inputByteValues =
              inputByteValue.Replace("\r", "").Replace("\n", "").Trim().Replace(" ", "").Split(',');

            Assert.AreEqual(alphabetNames.Length, alphabetSymbols.Length);
            Assert.AreEqual(alphabetNames.Length, inputByteValues.Length);
            for (int ibyte = 0; ibyte < inputByteValues.Length; ibyte++)
            {

                // Deocode a valid byte to alphabet representation.
                SequenceDecoder seq = new SequenceDecoder(encodings);
                ISequenceItem decodedItem = seq.Decode(byte.Parse(inputByteValues[ibyte],
                  (IFormatProvider)null));

                // Validate the alphabet for corresponding byte value passed.
                Assert.AreEqual(alphabetNames[ibyte], decodedItem.Name.ToString());
                Assert.AreEqual(alphabetSymbols[ibyte], decodedItem.Symbol.ToString());
                ApplicationLog.WriteLine(string.Format(null,
                  "Encoding P1 :Deocding Byte value {0} ", decodedItem));
                Console.WriteLine(string.Format(null,
                  "Encoding P1 :Deocding Byte value {0} ", decodedItem));
            }
            ApplicationLog.WriteLine("Encoding P1 : Decoding Byte value to Isequence Item is commpleted.");
            Console.WriteLine("Encoding P1 : Decoding Byte value to Isequence Item is commpleted.");
        }

        /// <summary>
        /// Validate Encoding property of Sequence Decoder whether it is updating properly.
        /// </summary>
        /// <param name="encoding">encoding instance.</param>
        void ValidateSequenceDecoderEncodingProperty(IEncoding encoding)
        {
            // Pass the encoding to Sequence Decoder.
            // Validate Sequence Decoder encoding property is updated accordingly.
            SequenceDecoder seq = new SequenceDecoder(encoding);
            IEncoding newEncoding = seq.Encoding;
            Assert.AreEqual(newEncoding, encoding);
            Console.WriteLine(string.Format((IFormatProvider)null,
              "Encoding P1 : Encoding Property of Sequence Decoder is as expected {0}", encoding));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
              "Encoding P1 : Encoding Property of Sequence Decoder is as expected {0}", encoding));
            ApplicationLog.WriteLine(
              "Encoding P1 : Encoding Property of Sequence Decoder validation is completed.");
            Console.WriteLine(
              "Encoding P1 : Encoding Property of Sequence Decoder validation is completed.");
        }

        /// <summary>
        /// Converts the sequence to byte array using Sequence Encoder Encode() method and validates the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="longerSequence">Is Longer sequence?</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateSequenceEncoderAlphabetToEncodingMap(string encodingNode,
          string alphabetNode, bool longerSequence, IEncoding encodings)
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequence);
            expectedSequence = expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            string expectedByteArray = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string[] expectedArray = expectedByteArray.Split(',');
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Create a ISequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(mapToEncoding.Convert(item));
            }

            // Encode IsequenceList Item to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encodings);
            byte[] encodedValue = iSeq.Encode(alphaList);
            ApplicationLog.WriteLine(
              "Encoding P1 : Conversion of Isequence Item list to byte array is completed.");

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(expectedArray.Length, encodedValue.Length);
            for (int iencode = 0; iencode < encodedValue.Length; iencode++)
            {
                Assert.AreEqual(encodedValue[iencode].ToString((IFormatProvider)null),
                  expectedArray[iencode]);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iencode]));
                Console.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iencode]));

                // For large and medium sequences only first four characters are unique.
                // Validate first four encoded values.
                if (longerSequence && iencode == Constants.LargeMediumUniqueSymbolCount)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Converts the sequence to byte array using Sequence Encoder Encode() method; 
        /// decode the byte array to sequence and validates the sequence.
        /// </summary>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateSequenceEncoderWithDecoder(string alphabetNode, IEncoding encodings)
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequenceNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Create a ISequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(mapToEncoding.Convert(item));
            }

            // Encode IsequenceList Item to Byte Array.
            SequenceEncoder SeqEncoder = new SequenceEncoder(encodings);
            byte[] encodedValue = SeqEncoder.Encode(alphaList);
            ApplicationLog.WriteLine(
              "Encoding P1 : Encoding of Isequence Item list to byte array is completed.");

            // Decode the Byte Array to IsequenceList item.
            List<ISequenceItem> newAlphaList = new List<ISequenceItem>();
            SequenceDecoder SeqDecoder = new SequenceDecoder(encodings);
            for (int iencode = 0; iencode < encodedValue.Length; iencode++)
            {
                newAlphaList.Add(SeqDecoder.Decode(encodedValue[iencode]));
            }
            ApplicationLog.WriteLine(
              "Encoding P1 : Decoding of byte array to isequence list is completed.");

            // Validate the decoded alphalist with expected alphalist
            Assert.AreEqual(alphaList.Count, newAlphaList.Count);
            for (int ialpha = 0; ialpha < alphaList.Count; ialpha++)
            {
                Assert.AreEqual(alphaList[ialpha], newAlphaList[ialpha]);
                Assert.AreEqual(alphaList[ialpha].Symbol, newAlphaList[ialpha].Symbol);
                Assert.AreEqual(alphaList[ialpha].Value, newAlphaList[ialpha].Value);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Alphabet sequence Item symbol {0} and value {1} is matching with the expected sequence item.", alphaList[ialpha].Symbol.ToString(), alphaList[ialpha].Value.ToString((IFormatProvider)null)));
                Console.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Alphabet sequence Item symbol {0} and value {1} is matching with the expected sequence item.", alphaList[ialpha].Symbol.ToString(), alphaList[ialpha].Value.ToString((IFormatProvider)null)));
            }
            ApplicationLog.WriteLine(
              "Encoding P1 : Validation of Sequence Encoder with decoder is completed.");
            Console.WriteLine(
              "Encoding P1 : Validation of Sequence Encoder with decoder is completed.");
        }

        /// <summary>
        /// SequenceEncoder: Encodes a char to byte and validates the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateSequenceEncoderConvertChartToByte(string encodingNode,
          string alphabetNode, IEncoding encodings)
        {
            // Gets LookUp() values from xml.
            string inputSymbol = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.ExpectedSequence);
            string expectedByte = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string[] expectedBytes = expectedByte.Split(',');

            // Encode IsequenceList Item to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encodings);
            int ibyte = 0;
            foreach (char inputchar in inputSymbol.ToCharArray())
            {
                byte encodedByte = iSeq.Encode(inputchar);
                ApplicationLog.WriteLine("Encoding P1 : Conversion of char to byte is completed.");

                // Validate the Encoded Isequence Item list.
                Assert.AreEqual(expectedBytes[ibyte],
                  encodedByte.ToString((IFormatProvider)null));
                ApplicationLog.WriteLine(String.Format((IFormatProvider)null,
                  "Encoding P1 : Conversion of char {0} to byte {1} is as expected.",
                  inputSymbol, expectedByte));
                ApplicationLog.WriteLine("Encoding P1 : Conversion of char to byte is completed.");
                Console.WriteLine(String.Format((IFormatProvider)null,
                  "Encoding P1 : Conversion of char {0} to byte {1} is as expected.",
                  inputSymbol, expectedByte));
                ibyte++;
            }
            Console.WriteLine("Encoding P1 : Conversion of char to byte is completed.");
        }

        /// <summary>
        /// SequenceEncoder: Encodes a sequence to byte and validates the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateSequenceEncoderConvertSequenceToByte(string encodingNode,
          string alphabetNode, IEncoding encodings)
        {
            // Gets LookUp() values from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequence);
            expectedSequence =
              expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);
            string expectedByte = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string[] expectedBytes = expectedByte.Split(',');

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);
            int ibyte = 0;
            foreach (ISequenceItem seqItem in seq)
            {
                // Encode Isequence Item to Byte .
                SequenceEncoder iSeq = new SequenceEncoder(encodings);
                byte encodedByte = iSeq.Encode(seqItem);
                ApplicationLog.WriteLine(
                  "Encoding P1 : Conversion of Isequence Item to byte is completed.");

                // Validate the Encoded Isequence Item list.
                Assert.AreEqual(expectedBytes[ibyte], encodedByte.ToString((IFormatProvider)null));
                ApplicationLog.WriteLine(String.Format((IFormatProvider)null,
                  "Encoding P1 : Conversion of char {0} to byte {1} is as expected.",
                  expectedSequence, expectedByte));
                ApplicationLog.WriteLine("Encoding P1 : Conversion of char to byte is completed.");
                Console.WriteLine(String.Format((IFormatProvider)null,
                  "Encoding P1 : Conversion of char {0} to byte {1} is as expected.",
                  expectedSequence, expectedByte));
                ibyte++;
            }
            Console.WriteLine("Encoding P1 : Conversion of char to byte is completed.");

        }

        /// <summary>
        /// Converts the sequence string to byte array using Sequence Encoder 
        /// Encode() method and validates the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateSequenceEncoderSequenceStringToEncodingMap(string encodingNode,
          string alphabetNode, bool longerSequence, IEncoding encodings)
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequenceNode);
            expectedSequence =
              expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string[] expectedArray = expectedByteArray.Split(',');

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Encode Sequence string to Byte Array.
            SequenceEncoder iSeq = new SequenceEncoder(encodings);
            byte[] encodedValue = iSeq.Encode(seq.ToString());
            ApplicationLog.WriteLine(
              "Encoding P1 : Conversion of valid sequence string to byte array is completed.");

            // Validate the Encoded Isequence Item list.
            Assert.AreEqual(expectedArray.Length, encodedValue.Length);
            for (int iencode = 0; iencode < encodedValue.Length; iencode++)
            {
                Assert.AreEqual(encodedValue[iencode].ToString((IFormatProvider)null),
                  expectedArray[iencode]);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iencode]));
                Console.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iencode]));

                // For large and medium sequences only first four characters are unique.
                // Validate first four encoded values.
                if (longerSequence && iencode == 4)
                {
                    break;
                }
            }

        }

        /// <summary>
        /// Converts the sequence item list to byte array using target byte array and validates the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateSequenceEncoderAlphabetToTargetByteArray(string encodingNode,
          string alphabetNode, IEncoding encodings)
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequenceNode);
            expectedSequence =
              expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string[] expectedArray = expectedByteArray.Split(',');
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Create a ISequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(mapToEncoding.Convert(item));
            }

            // Encode IsequenceList Item to byte array.
            SequenceEncoder iSeq = new SequenceEncoder(encodings);
            byte[] encodedValue = new byte[expectedArray.Length];
            iSeq.Encode(alphaList, encodedValue);
            ApplicationLog.WriteLine(
              "Encoding P1 : Conversion of Isequence Item list to byte array using target byte array is completed.");

            // Validate the Encoded Isequence Item list.
            for (int iencode = 0; iencode < encodedValue.Length; iencode++)
            {
                Assert.AreEqual(encodedValue[iencode].ToString((IFormatProvider)null),
                  expectedArray[iencode]);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  encodedValue[iencode].ToString((IFormatProvider)null)));
                Console.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.", encodedValue[iencode].ToString((IFormatProvider)null)));
            }
        }

        /// <summary>
        /// Converts the sequence string to byte array using target byte array and validates the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateSequenceEncoderSequenceStringToTargetByteArray(string encodingNode,
          string alphabetNode, IEncoding encodings)
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequenceNode);
            expectedSequence =
              expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string[] expectedArray = expectedByteArray.Split(',');

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Encode sequence string to byte array using target byte array.
            SequenceEncoder iSeq = new SequenceEncoder(encodings);
            byte[] encodedValue = new byte[expectedArray.Length];
            iSeq.Encode(seq.ToString(), encodedValue);
            ApplicationLog.WriteLine(
              "Encoding P1 : Conversion of valid sequence string to byte array using target byte array is completed.");

            // Validate the Encoded Isequence Item list.
            for (int iencode = 0; iencode < encodedValue.Length; iencode++)
            {
                Assert.AreEqual(encodedValue[iencode].ToString((IFormatProvider)null),
                  expectedArray[iencode]);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iencode]));
                Console.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iencode]));
            }

        }

        /// <summary>
        /// Converts the sequence to byte array using target byte array with offset and validates the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateSequenceEncoderAlphabetToTargetByteArrayWithOffset(string encodingNode,
          string alphabetNode, IEncoding encodings)
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequenceNode);
            expectedSequence =
              expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);
            string[] expectedArray = expectedByteArray.Split(',');
            EncodingMap mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encodings);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Create a ISequence List.
            List<ISequenceItem> alphaList = new List<ISequenceItem>();
            foreach (ISequenceItem item in seq)
            {
                alphaList.Add(mapToEncoding.Convert(item));
            }

            // Encode IsequenceList Item to byte array.
            SequenceEncoder iSeq = new SequenceEncoder(encodings);
            int offset = Constants.Offset;
            byte[] encodedValue = new byte[expectedArray.Length + offset];
            iSeq.Encode(alphaList, encodedValue, offset);
            ApplicationLog.WriteLine(
              "Encoding P1 : Conversion of Isequence Item list to byte array using target byte array with offset is completed.");

            // Validate the Encoded Isequence Item list.
            int iOffset = offset;
            for (int iexpected = 0; iexpected < expectedArray.Length; iexpected++)
            {
                Assert.AreEqual(expectedArray[iexpected],
                  encodedValue[iOffset].ToString((IFormatProvider)null));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iexpected]));
                Console.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iexpected]));
                iOffset++;
            }
        }

        /// <summary>
        /// Converts the valid sequence string to byte array using target byte array 
        /// with offset and validates the same.
        /// </summary>
        /// <param name="encodingNode">expected encoded values.</param>
        /// <param name="alphabetNode">alphabet xml node.</param>
        /// <param name="encodings">encoding instance.</param>
        void ValidateSequenceEncoderSequenceStringToTargetByteArrayWithOffset(string encodingNode,
          string alphabetNode, IEncoding encodings)
        {
            // Gets the Sequence value from xml.
            string expectedSequence = Utility._xmlUtil.GetTextValue(alphabetNode,
              Constants.ExpectedSequenceNode);
            expectedSequence =
              expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);
            string expectedByteArray = Utility._xmlUtil.GetTextValue(alphabetNode, encodingNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(alphabetNode, Constants.AlphabetNameNode);
            string[] expectedArray = expectedByteArray.Split(',');

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
              "Encoding P1 : Sequence '{0}' and Alphabet '{1}'.", expectedSequence, alphabetName));

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSequence);

            // Encode Sequence string to byte array.
            SequenceEncoder iSeq = new SequenceEncoder(encodings);
            int offset = Constants.Offset;
            byte[] encodedValue = new byte[expectedArray.Length + offset];
            iSeq.Encode(seq.ToString(), encodedValue, offset);
            ApplicationLog.WriteLine("Encoding P1 : Conversion of Isequence Item list to byte array is completed using target byte array woth offset.");

            // Validate the Encoded Isequence Item list.
            int iOffset = offset;
            for (int iexpected = 0; iexpected < expectedArray.Length; iexpected++)
            {
                Assert.AreEqual(expectedArray[iexpected],
                  encodedValue[iOffset].ToString((IFormatProvider)null));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iexpected]));
                Console.WriteLine(string.Format((IFormatProvider)null,
                  "Encoding P1 : Encoded byte {0} is matching with the expected byte.",
                  expectedArray[iexpected]));
                iOffset++;
            }
        }

        #endregion
    }
}
