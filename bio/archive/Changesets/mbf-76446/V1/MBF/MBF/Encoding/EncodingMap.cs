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
using MBF.Util.Logging;
using MBF.Properties;

namespace MBF.Encoding
{
    /// <summary>
    /// Indicates which direction the encoding map provides mapping for.
    /// AlphabetToEncoding converts ISequenceItems from an IAlphabet and
    /// turns the into ISequenceItems from an IEncoding. EncodingToAlphabet
    /// does the reverse.
    /// </summary>
    public enum EncodingMapDirection
    {
        /// <summary>
        /// Converts ISequenceItems from an IAlphabet into ISequenceItems from an Encoding.
        /// </summary>
        AlphabetToEncoding,
        /// <summary>
        /// Converts ISequenceItems from an Encoding into ISequenceItems from an IAlphabet.
        /// </summary>
        EncodingToAlphabet
    }

    /// <summary>
    /// An encoding map provides the translation between items in an Alphabet and
    /// items in an Encoding. This is important because many alphabets have several
    /// encodings associated with them and thus you need an explicit map from one
    /// to the other.
    /// 
    /// EncodingMap provides static methods for looking up known maps from Alphabet
    /// to Encoding and from Encoding to Alphabet. It also provides the means for
    /// defining your own mapping.
    /// </summary>
    public class EncodingMap
    {
        private IAlphabet alphabet;
        private IEncoding encoding;
        private EncodingMapDirection direction;
        private ISequenceItem[] map;

        private static List<EncodingMap> alphaMaps = new List<EncodingMap>();
        private static List<EncodingMap> encMaps = new List<EncodingMap>();

        #region Built-in DNA/RNA Nucleotide Encoding Maps
        /// <summary>
        /// Map from Encoding NCBI4na to Alphabet DNA
        /// </summary>
        public static EncodingMap Ncbi4NAToDna
            = CreateBasicMap(Alphabets.DNA, Encodings.Ncbi4NA, EncodingMapDirection.EncodingToAlphabet);

        /// <summary>
        /// Map from Encoding NCBI2na to Alphabet DNA
        /// </summary>
        public static EncodingMap Ncbi2NAToDna
            = CreateBasicMap(Alphabets.DNA, Encodings.Ncbi2NA, EncodingMapDirection.EncodingToAlphabet);

        /// <summary>
        /// Map from Encoding IUPACna to Alphabet DNA
        /// </summary>
        public static EncodingMap IupacNAToDna
            = CreateBasicMap(Alphabets.DNA, Encodings.IupacNA, EncodingMapDirection.EncodingToAlphabet);

        /// <summary>
        /// Map from Encoding NCBI4na to Alphabet RNA
        /// </summary>
        public static EncodingMap Ncbi4NAToRna
            = CreateBasicMap(Alphabets.RNA, Encodings.Ncbi4NA, EncodingMapDirection.EncodingToAlphabet);

        /// <summary>
        /// Map from Encoding NCBI2na to Alphabet RNA
        /// </summary>
        public static EncodingMap Ncbi2NAToRna
            = CreateBasicMap(Alphabets.RNA, Encodings.Ncbi2NA, EncodingMapDirection.EncodingToAlphabet);

        /// <summary>
        /// Map from Encoding IUPACna to Alphabet RNA
        /// </summary>
        public static EncodingMap IupacNAToRna
            = CreateBasicMap(Alphabets.RNA, Encodings.IupacNA, EncodingMapDirection.EncodingToAlphabet);

        /// <summary>
        /// Map from Alphabet DNA to Encoding NCBI4na
        /// </summary>
        public static EncodingMap DnaToNcbi4NA
            = CreateBasicMap(Alphabets.DNA, Encodings.Ncbi4NA, EncodingMapDirection.AlphabetToEncoding);

        /// <summary>
        /// Map from Alphabet DNA to Encoding NCBI2na
        /// </summary>
        public static EncodingMap DnaToNcbi2NA
            = CreateBasicMap(Alphabets.DNA, Encodings.Ncbi2NA, EncodingMapDirection.AlphabetToEncoding);

        /// <summary>
        /// Map from Alphabet DNA to Encoding IUPACna
        /// </summary>
        public static EncodingMap DnaToIupacNA
            = CreateBasicMap(Alphabets.DNA, Encodings.IupacNA, EncodingMapDirection.AlphabetToEncoding);

        /// <summary>
        /// Map from Alphabet RNA to Encoding NCBI4na
        /// </summary>
        public static EncodingMap RnaToNcbi4NA
            = CreateBasicMap(Alphabets.RNA, Encodings.Ncbi4NA, EncodingMapDirection.AlphabetToEncoding);

        /// <summary>
        /// Map from Alphabet RNA to Encoding NCBI2na
        /// </summary>
        public static EncodingMap RnaToNcbi2NA
            = CreateBasicMap(Alphabets.RNA, Encodings.Ncbi2NA, EncodingMapDirection.AlphabetToEncoding);

        /// <summary>
        /// Map from Alphabet RNA to Encoding IUPACna
        /// </summary>
        public static EncodingMap RnaToIupacNA
            = CreateBasicMap(Alphabets.RNA, Encodings.IupacNA, EncodingMapDirection.AlphabetToEncoding);


        #endregion

        #region Built-in Protein Amino Acid Encoding Maps
        /// <summary>
        /// Map from Alphabet Protein to Encoding NCBIstdaa
        /// </summary>
        public static EncodingMap ProteinToNcbiStdAA
            = CreateBasicMap(Alphabets.Protein, Encodings.NcbiStdAA, EncodingMapDirection.AlphabetToEncoding);

        /// <summary>
        /// Map from Alphabet Protein to Encoding NCBIeaa
        /// </summary>
        public static EncodingMap ProteinToNcbiEAA
            = CreateBasicMap(Alphabets.Protein, Encodings.NcbiEAA, EncodingMapDirection.AlphabetToEncoding);

        /// <summary>
        /// Map from Encoding NCBIstdaa to Alphabet Protein
        /// </summary>
        public static EncodingMap NcbiStdAAToProtein
            = CreateBasicMap(Alphabets.Protein, Encodings.NcbiStdAA, EncodingMapDirection.EncodingToAlphabet);

        /// <summary>
        /// Map from Encoding NCBIstdaa to Alphabet Protein
        /// </summary>
        public static EncodingMap NcbiEAAToProtein
            = CreateBasicMap(Alphabets.Protein, Encodings.NcbiEAA, EncodingMapDirection.EncodingToAlphabet);
        #endregion

        static EncodingMap()
        {
            // Fill in the list of encodings
            encMaps.Add(Ncbi4NAToDna);
            encMaps.Add(Ncbi2NAToDna);
            encMaps.Add(IupacNAToDna);
            encMaps.Add(Ncbi4NAToRna);
            encMaps.Add(Ncbi2NAToRna);
            encMaps.Add(IupacNAToRna);

            encMaps.Add(NcbiStdAAToProtein);
            encMaps.Add(NcbiEAAToProtein);

            alphaMaps.Add(DnaToNcbi4NA);
            alphaMaps.Add(DnaToNcbi2NA);
            alphaMaps.Add(DnaToIupacNA);
            alphaMaps.Add(RnaToNcbi4NA);
            alphaMaps.Add(RnaToNcbi2NA);
            alphaMaps.Add(RnaToIupacNA);

            alphaMaps.Add(ProteinToNcbiStdAA);
            alphaMaps.Add(ProteinToNcbiEAA);

            // Add special cases not handled by direct symbol comparison
            RnaToIupacNA.map[Alphabets.RNA.U.Symbol] = Encodings.IupacNA.T;
            RnaToNcbi4NA.map[Alphabets.RNA.U.Symbol] = Encodings.Ncbi4NA.T;
            RnaToNcbi2NA.map[Alphabets.RNA.U.Symbol] = Encodings.Ncbi2NA.T;

            IupacNAToRna.map[Encodings.IupacNA.T.Symbol] = Alphabets.RNA.U;
            Ncbi4NAToRna.map[Encodings.Ncbi4NA.T.Symbol] = Alphabets.RNA.U;
            Ncbi2NAToRna.map[Encodings.Ncbi2NA.T.Symbol] = Alphabets.RNA.U;
        }

        /// <summary>
        /// Looks up the default encoding map for known alphabets to the default
        /// encoding for that alphabet. Several encodings may exist for any one
        /// particular alphabet. If you want to select a particular encoding,
        /// consider using the GetMapToEncoding() method.
        /// </summary>
        public static EncodingMap GetDefaultMap(IAlphabet alphabet)
        {
            if (alphabet == Alphabets.DNA)
                return DnaToNcbi4NA;
            else if (alphabet == Alphabets.RNA)
                return RnaToNcbi4NA;
            else if (alphabet == Alphabets.Protein)
                return ProteinToNcbiStdAA;

            Trace.Report(Resource.ParameterContainsNullValue);
            throw new ArgumentNullException(Resource.ParameterNameAlphabet);
        }

        /// <summary>
        /// Looks up the default encoding map for a known encoding to the
        /// default alphabet for that encoding. Several alphabets may exist
        /// for any one particular encoding. If you want to select a particular
        /// alphabet, consider using the GetMapToAlphabet() method.
        /// </summary>
        public static EncodingMap GetDefaultMap(IEncoding encoding)
        {
            if (encoding == Encodings.IupacNA)
                return IupacNAToDna;
            else if (encoding == Encodings.Ncbi4NA)
                return Ncbi4NAToDna;
            else if (encoding == Encodings.Ncbi2NA)
                return Ncbi2NAToDna;
            else if (encoding == Encodings.NcbiStdAA)
                return NcbiStdAAToProtein;
            else if (encoding == Encodings.NcbiEAA)
                return NcbiEAAToProtein;

            Trace.Report(Resource.ParameterContainsNullValue);
            throw new ArgumentNullException(Resource.ParameterNameEncoding);
        }

        /// <summary>
        /// Looks amongst all the known encoding maps for one that provides a mapping
        /// from the alphabet passed in as a parameter to the encoding passed in as a
        /// parameter.
        /// </summary>
        /// <returns>The known encoding map, or null if one is not available</returns>
        public static EncodingMap GetMapToEncoding(IAlphabet alphabet, IEncoding encoding)
        {
            if (null == alphabet)
            {
                Trace.Report(Resource.ParameterContainsNullValue);
                throw new ArgumentNullException(Resource.ParameterNameAlphabet);
            }

            if (null == encoding)
            {
                Trace.Report(Resource.ParameterContainsNullValue);
                throw new ArgumentNullException(Resource.ParameterNameEncoding);
            }

            foreach (EncodingMap map in alphaMaps)
            {
                if (map.Alphabet == alphabet && map.Encoding == encoding)
                    return map;
            }

            return null;
        }

        /// <summary>
        /// Looks amongst all the known encoding maps for one that provides a mapping
        /// from the encoding passed in as a parameter to the alphabet passed in as a
        /// parameter.
        /// </summary>
        /// <returns>The known encoding map, or null if one is not available</returns>
        public static EncodingMap GetMapToAlphabet(IEncoding encoding, IAlphabet alphabet)
        {
            foreach (EncodingMap map in encMaps)
            {
                if (map.Alphabet == alphabet && map.Encoding == encoding)
                    return map;
            }

            return null;
        }

        // Hide the default constructor
        internal EncodingMap() { }

        // Does not set any of the mapping information. Use the CreateBasicMap()
        // factory method if you want the mapping dictionary filled in for symbols
        // that match in both the alphabet and the encoding.
        internal EncodingMap(IAlphabet alphabet, IEncoding encoding, EncodingMapDirection direction)
        {
            this.alphabet = alphabet;
            this.encoding = encoding;
            this.direction = direction;
        }

        /// <summary>
        /// The alphabet source for the encoding map
        /// </summary>
        public IAlphabet Alphabet
        {
            get { return alphabet; }
            internal set { alphabet = value; }
        }

        /// <summary>
        /// The encoding source for the encoding map
        /// </summary>
        public IEncoding Encoding
        {
            get { return encoding; }
            internal set { encoding = value; }
        }

        /// <summary>
        /// Converts the ISequenceItem passed in as a parameter from one collection
        /// of such items to another. This is useful in going from encoded items to
        /// alphabet items for instance.
        /// </summary>
        public ISequenceItem Convert(ISequenceItem item)
        {
            int symbol = item.Symbol;
            if (direction == EncodingMapDirection.AlphabetToEncoding)
            {
               if (symbol > map.Length || map[symbol] == null)
                  throw new ArgumentException("The alphabet does not contain the item being mapped");

               return map[symbol];
            }
            else if (direction == EncodingMapDirection.EncodingToAlphabet)
            {
               if (symbol > map.Length || map[symbol] == null)
                  throw new ArgumentException("The encoding does not contain the item being mapped");

               return map[symbol];
            }

            throw new Exception("Internal Error: Unrecognized encoding map direction");
        }

        // Does a direct symbol comparison to create the dictionary used in the mapping
        // This is a useful starting point for any map. The dictionary can then be customized
        // by adding or removing entries.
        internal static EncodingMap CreateBasicMap(IAlphabet alphabet, IEncoding encoding, EncodingMapDirection direction)
        {
            EncodingMap result = new EncodingMap(alphabet, encoding, direction);

            if (direction == EncodingMapDirection.AlphabetToEncoding)
            {
               result.map = GetMap(
                 alphabet,
                 item => encoding.LookupBySymbol(item.Symbol));
            }
            else if (direction == EncodingMapDirection.EncodingToAlphabet)
            {
               result.map = GetMap(
                encoding,
                item => alphabet.LookupBySymbol(item.Symbol));

            }

            return result;
        }

        /// <summary>
        /// Instead of using a Dictionary to map a known set of character IDs create an
        /// array the length of sequence items for that map.  This is immensely faster for a large
        /// number of access operations than using a hash function.  Since the Convert method will
        /// be used every time a sequence item is accessed from a sequence this needs to be as 
        /// performant as possible.
        /// 
        /// Originaly to serialize 100 sequences of 11,000 items a piece time was 13.2 seconds.  After
        /// switching to this method it is 5.0 seconds. (Times are with JetBrain's dotTrace profiler attached)
        /// </summary>
        /// <returns></returns>
        private static ISequenceItem[] GetMap(IEnumerable<ISequenceItem> sourceItems, Func<ISequenceItem, ISequenceItem> lookupItem)
        {
           ISequenceItem maxItem = null;
           foreach (var item in sourceItems)
           {
              if (maxItem == null || item.Symbol > maxItem.Symbol)
                 maxItem = item;
           }

           if (maxItem == null) return new ISequenceItem[0];

           var itemMap = new ISequenceItem[maxItem.Symbol + 1];
           Array.Clear(itemMap, 0, itemMap.Length);

           foreach (var item in sourceItems)
           {
              try
              {
                 itemMap[item.Symbol] = lookupItem(item);
              }
              catch (ArgumentException)
              {
                 // Lookup failed.
              }
           }

           return itemMap;
        }
    }
}
