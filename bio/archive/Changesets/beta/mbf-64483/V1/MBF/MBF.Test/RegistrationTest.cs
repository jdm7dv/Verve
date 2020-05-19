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
using MBF.Algorithms.Alignment;
using MBF.Algorithms.Assembly;
using MBF.IO;
using MBF.Registration;
using MBF.Util.Logging;
using NUnit.Framework;
using MBF.SimilarityMatrices;

namespace MBF.Test
{
    /// <summary>
    /// Plugin Registration test cases.
    /// Example of an ALIGNER registration
    /// Step 1: Creating sample class for Aligner - call it as New1Aligner4TestingRegistration : ISequenceAligner
    ///         Since registration follows interface driven, 
    ///         the new aligner class must be extended from ISequenceAligner.
    ///         
    /// Step 2: Add registration attribute on top of the class for auto-plugin
    ///         Here is the attribute [RegistrableAttribute(true)]
    ///        
    /// Step 3: Compile the project and copy the MBF.Test.dll under ...\MBF\MBF.Test\bin\Add-ins
    ///         or ..\Microsoft Biology Framework\Add-ins where MBF is installed.
    ///         
    /// Step 4: Now, the either from GetAlignersTest test case or different application, call
    ///         RegisteredAddIn.GetAligners(true) which returns list of aligner including 
    ///         newly registered New1Aligner4TestingRegistration
    /// 
    /// The same steps are followed for assemblers, formatters, parsers and alphabets. 
    /// See the following sample classes and test cases for assemblers, formatters, parsers and alphabets. 
    /// </summary>
    [TestFixture]
    public class RegistrationTest
    {
        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static RegistrationTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// get registered alingers test.
        /// </summary>
        [Test]
        public void GetAlignersTest()
        {
            //IMPORTANT: To pass this test case, it required two folders under...\MBF\MBF.Test\bin
            // 1. ...\MBF\MBF.Test\bin\Core
            // 2. ...\MBF\MBF.Test\bin\Add-ins
            // MBF.Test.dll needs to be copied under Add-ins folder.
            IList<ISequenceAligner> finalValue = new List<ISequenceAligner>();
            finalValue.Add(new New1Aligner4TestingRegistration());
            finalValue.Add(new New2Aligner4TestingRegistration());

            IList<ISequenceAligner> registeredAligners = GetClasses<ISequenceAligner>(true);
            if (null != registeredAligners && registeredAligners.Count > 0)
            {
                foreach (ISequenceAligner aligner in finalValue)
                {
                    string name = null;
                    registeredAligners.FirstOrDefault(IA => string.Compare(name = IA.Name, aligner.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
                    Assert.AreSame(aligner.Name, name);
                }
            }
        }

        /// <summary>
        /// get registered assemblers test.
        /// </summary>
        [Test]
        public void GetAssemblersTest()
        {
            //IMPORTANT: To pass this test case, it required two folders under...\MBF\MBF.Test\bin
            // 1. ...\MBF\MBF.Test\bin\Core
            // 2. ...\MBF\MBF.Test\bin\Add-ins
            // MBF.Test.dll needs to be copied under Add-ins folder.
            IList<IDeNovoAssembler> finalValue = new List<IDeNovoAssembler>();
            finalValue.Add(new NewAssembler4TestingRegistration());

            IList<IDeNovoAssembler> registeredAssemblers = GetClasses<IDeNovoAssembler>(true);
            if (null != registeredAssemblers && registeredAssemblers.Count > 0)
            {
                foreach (IDeNovoAssembler assembler in finalValue)
                {
                    string name = null;
                    registeredAssemblers.FirstOrDefault(IA => string.Compare(name = IA.Name, assembler.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
                    Assert.AreSame(assembler.Name, name);
                }
            }
        }

        /// <summary>
        /// get registered alphabets test.
        /// </summary>
        [Test]
        public void GetAlphabetsTest()
        {
            //IMPORTANT: To pass this test case, it required two folders under...\MBF\MBF.Test\bin
            // 1. ...\MBF\MBF.Test\bin\Core
            // 2. ...\MBF\MBF.Test\bin\Add-ins
            // MBF.Test.dll needs to be copied under Add-ins folder.
            IList<IAlphabet> finalValue = new List<IAlphabet>();
            finalValue.Add(new NewAlphabet4TestingRegistration());

            IList<IAlphabet> registeredAlphabets = RegisteredAddIn.GetAlphabets(true);
            if (null != registeredAlphabets && registeredAlphabets.Count > 0)
            {
                foreach (IAlphabet alphabet in finalValue)
                {
                    string name = null;
                    registeredAlphabets.FirstOrDefault(IA => string.Compare(name = IA.Name, alphabet.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
                    Assert.AreSame(alphabet.Name, name);
                }
            }
        }

        /// <summary>
        /// get registered formatters test.
        /// </summary>
        [Test]
        public void GetFormattersTest()
        {
            //IMPORTANT: To pass this test case, it required two folders under...\MBF\MBF.Test\bin
            // 1. ...\MBF\MBF.Test\bin\Core
            // 2. ...\MBF\MBF.Test\bin\Add-ins
            // MBF.Test.dll needs to be copied under Add-ins folder.
            IList<ISequenceFormatter> finalValue = new List<ISequenceFormatter>();
            finalValue.Add(new NewFormatter4TestingRegistration());

            IList<ISequenceFormatter> registeredFormatters = GetClasses<ISequenceFormatter>(true);
            if (null != registeredFormatters && registeredFormatters.Count > 0)
            {
                foreach (ISequenceFormatter formatter in finalValue)
                {
                    string name = null;
                    registeredFormatters.FirstOrDefault(IA => string.Compare(name = IA.Name, formatter.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
                    Assert.AreSame(formatter.Name, name);
                }
            }
        }

        /// <summary>
        /// get registered parsers test.
        /// </summary>
        [Test]
        public void GetParsersTest()
        {
            //IMPORTANT: To pass this test case, it required two folders under...\MBF\MBF.Test\bin
            // 1. ...\MBF\MBF.Test\bin\Core
            // 2. ...\MBF\MBF.Test\bin\Add-ins
            // MBF.Test.dll needs to be copied under Add-ins folder.
            IList<ISequenceParser> finalValue = new List<ISequenceParser>();
            finalValue.Add(new NewParser4TestingRegistration());

            IList<ISequenceParser> registeredParsers = GetClasses<ISequenceParser>(true);
            if (null != registeredParsers && registeredParsers.Count > 0)
            {
                foreach (ISequenceParser parser in finalValue)
                {
                    string name = null;
                    registeredParsers.FirstOrDefault(IA => string.Compare(name = IA.Name, parser.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
                    Assert.AreSame(parser.Name, name);
                }
            }
        }

        /// <summary>
        /// Gets all registered specified classes in core folder and addins (optional) folders
        /// </summary>
        /// <param name="includeAddinFolder">include add-ins folder or not</param>
        /// <returns>List of registered classes</returns>
        private static IList<T> GetClasses<T>(bool includeAddinFolder)
        {
            IList<T> registeredAligners = new List<T>();

            if (includeAddinFolder)
            {
                IList<T> addInAligners;
                if (null != RegisteredAddIn.AddinFolderPath)
                {
                    addInAligners = RegisteredAddIn.GetInstancesFromAssemblyPath<T>(RegisteredAddIn.AddinFolderPath, RegisteredAddIn.DLLFilter);
                    if (null != addInAligners && addInAligners.Count > 0)
                    {
                        foreach (T aligner in addInAligners)
                        {
                            if (aligner != null)
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

    /// <summary>
    /// Creating new aligner class which is extended from ISequenceAligner. 
    /// Also registered for auto-plugin by the registration attribute as true
    /// </summary>
    [RegistrableAttribute(true)]
    public class New1Aligner4TestingRegistration : ISequenceAligner
    {
        #region ISequenceAligner Members

        string ISequenceAligner.Name
        {
            get { return "New1 Aligner4TestingRegistration"; }
        }

        string ISequenceAligner.Description
        {
            get { throw new NotImplementedException(); }
        }

        IConsensusResolver ISequenceAligner.ConsensusResolver
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        SimilarityMatrix ISequenceAligner.SimilarityMatrix
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        int ISequenceAligner.GapOpenCost
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        int ISequenceAligner.GapExtensionCost
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        IList<ISequenceAlignment> ISequenceAligner.AlignSimple(IList<ISequence> inputSequences)
        {
            throw new NotImplementedException();
        }

        IList<ISequenceAlignment> ISequenceAligner.Align(IList<ISequence> inputSequences)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Creating new pairwise aligner class which is extended from IPairwiseSequenceAligner. 
    /// Also registered for auto-plugin by the registration attribute as true   
    /// </summary>
    [RegistrableAttribute(true)]
    public class New2Aligner4TestingRegistration : IPairwiseSequenceAligner
    {
        #region IPairwiseSequenceAligner Members
        /// <summary>
        /// 
        /// </summary>
        public SimilarityMatrix SimilarityMatrix
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GapOpenCost
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int GapExtensionCost
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence1"></param>
        /// <param name="sequence2"></param>
        /// <returns></returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(ISequence sequence1, ISequence sequence2)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence1"></param>
        /// <param name="sequence2"></param>
        /// <returns></returns>
        public IList<IPairwiseSequenceAlignment> Align(ISequence sequence1, ISequence sequence2)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ISequenceAligner Members

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return "New2 Aligner4TestingRegistration"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public IConsensusResolver ConsensusResolver
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSequences"></param>
        /// <returns></returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(System.Collections.Generic.List<ISequence> inputSequences)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSequences"></param>
        /// <returns></returns>
        public IList<IPairwiseSequenceAlignment> Align(System.Collections.Generic.IList<ISequence> inputSequences)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSequences"></param>
        /// <returns></returns>
        IList<ISequenceAlignment> ISequenceAligner.AlignSimple(System.Collections.Generic.IList<ISequence> inputSequences)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSequences"></param>
        /// <returns></returns>
        IList<ISequenceAlignment> ISequenceAligner.Align(System.Collections.Generic.IList<ISequence> inputSequences)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// Creating new assembler class which is extended from IDeNovoAssembler. 
    /// Also registered for auto-plugin by the registration attribute as true
    /// </summary>
    [RegistrableAttribute(true)]
    public class NewAssembler4TestingRegistration : IDeNovoAssembler
    {
        #region IDeNovoAssembler Members

        string IDeNovoAssembler.Name
        {
            get { return "New Assembler4TestingRegistration"; }
        }

        string IDeNovoAssembler.Description
        {
            get { throw new System.NotImplementedException(); }
        }

        IDeNovoAssembly IDeNovoAssembler.Assemble(IList<ISequence> inputSequences)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Creating new alphabet class which is extended from IAlphabet. 
    /// Also registered for auto-plugin by the registration attribute as true
    /// </summary>
    [RegistrableAttribute(true)]
    public class NewAlphabet4TestingRegistration : IAlphabet
    {
        #region IAlphabet Members

        string IAlphabet.Name
        {
            get { return "New Alphabet4TestingRegistration"; }
        }

        bool IAlphabet.HasGaps
        {
            get { throw new System.NotImplementedException(); }
        }

        bool IAlphabet.HasAmbiguity
        {
            get { throw new System.NotImplementedException(); }
        }

        bool IAlphabet.HasTerminations
        {
            get { throw new System.NotImplementedException(); }
        }

        ISequenceItem IAlphabet.DefaultGap
        {
            get { throw new System.NotImplementedException(); }
        }

        ISequenceItem IAlphabet.LookupBySymbol(char symbol)
        {
            throw new System.NotImplementedException();
        }

        ISequenceItem IAlphabet.LookupBySymbol(string symbol)
        {
            throw new System.NotImplementedException();
        }

        ISequenceItem IAlphabet.GetConsensusSymbol(HashSet<ISequenceItem> symbols)
        {
            throw new System.NotImplementedException();
        }

        HashSet<ISequenceItem> IAlphabet.GetBasicSymbols(ISequenceItem symbol)
        {
            throw new System.NotImplementedException();
        }

        List<ISequenceItem> IAlphabet.LookupAll(bool includeBasics, bool includeGaps, bool includeAmbiguities, bool includeTerminations)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ICollection<ISequenceItem> Members

        void ICollection<ISequenceItem>.Add(ISequenceItem item)
        {
            throw new System.NotImplementedException();
        }

        void ICollection<ISequenceItem>.Clear()
        {
            throw new System.NotImplementedException();
        }

        bool ICollection<ISequenceItem>.Contains(ISequenceItem item)
        {
            throw new System.NotImplementedException();
        }

        void ICollection<ISequenceItem>.CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        int ICollection<ISequenceItem>.Count
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ICollection<ISequenceItem>.IsReadOnly
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ICollection<ISequenceItem>.Remove(ISequenceItem item)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IEnumerable<ISequenceItem> Members

        IEnumerator<ISequenceItem> IEnumerable<ISequenceItem>.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Creating new formatter class which is extended from ISequenceFormatter. 
    /// Also registered for auto-plugin by the registration attribute as true
    /// </summary>
    [RegistrableAttribute(true)]
    public class NewFormatter4TestingRegistration : ISequenceFormatter
    {
        #region ISequenceFormatter Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="writer"></param>
        public void Format(ISequence sequence, System.IO.TextWriter writer)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="filename"></param>
        public void Format(ISequence sequence, string filename)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="writer"></param>
        public void Format(ICollection<ISequence> sequences, System.IO.TextWriter writer)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="filename"></param>
        public void Format(ICollection<ISequence> sequences, string filename)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public string FormatString(ISequence sequence)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return "New Formatter4TestingRegistration"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FileTypes
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }

    /// <summary>
    /// Creating new parser class which is extended from ISequenceParser. 
    /// Also registered for auto-plugin by the registration attribute as true
    /// </summary>
    [RegistrableAttribute(true)]
    public class NewParser4TestingRegistration : ISequenceParser
    {
        #region ISequenceParser Members

        IAlphabet IParser.Alphabet
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        MBF.Encoding.IEncoding IParser.Encoding
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        IList<ISequence> ISequenceParser.Parse(System.IO.TextReader reader)
        {
            throw new System.NotImplementedException();
        }

        IList<ISequence> ISequenceParser.Parse(string filename)
        {
            throw new System.NotImplementedException();
        }

        ISequence ISequenceParser.ParseOne(System.IO.TextReader reader)
        {
            throw new System.NotImplementedException();
        }

        ISequence ISequenceParser.ParseOne(string filename)
        {
            throw new System.NotImplementedException();
        }

        string IParser.Name
        {
            get { return "New Parser4TestingRegistration"; }
        }

        string IParser.Description
        {
            get { throw new System.NotImplementedException(); }
        }

        string IParser.FileTypes
        {
            get { throw new System.NotImplementedException(); }
        }

        IList<ISequence> ISequenceParser.Parse(System.IO.TextReader reader, bool isReadOnly)
        {
            throw new NotImplementedException();
        }

        IList<ISequence> ISequenceParser.Parse(string filename, bool isReadOnly)
        {
            throw new NotImplementedException();
        }

        ISequence ISequenceParser.ParseOne(System.IO.TextReader reader, bool isReadOnly)
        {
            throw new NotImplementedException();
        }

        ISequence ISequenceParser.ParseOne(string filename, bool isReadOnly)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
