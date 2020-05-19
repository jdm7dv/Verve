// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * VirtualSequenceP2TestCases.cs
 * 
 * This file contains the Virtual Sequence P2 test case validation.
 * 
******************************************************************************/

using System;

using MBF.IO;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Virtual Sequences and P2 level validations.
    /// </summary>
    [TestClass]
    public class VirtualSequenceP2TestCases
    {
        #region Enum

        /// <summary>
        /// VirtualSequence parameters which are used for different test cases
        /// based on which the test cases are executed.
        /// </summary>
        enum VirtualSequenceMethod
        {
            Gap,
            GapPosition,
            LastGap,
            LastGapPosition,
            Remove,
            Replace,
            GetEnum,
            Insert,
            GetObjData
        }

        #endregion Enum

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static VirtualSequenceP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region VirtualSequence P2 TestCases

        /// <summary>
        /// Validate an exception when passing null value for alphabet.
        /// Input Data : Null.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceNull()
        {
            try
            {
                // Create virtual seqeunce with null value
                VirtualSequence virtualSeq = new VirtualSequence(null);
                Assert.IsTrue(null != virtualSeq);
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "Virtual Sequence P2: Sequence exception validated successfully.");
                Console.WriteLine(
                    "Virtual Sequence P2: Sequence exception validated successfully.");
            }
        }

        /// <summary>
        /// Validate an exception when try to get the ToString((IFormatProvider)null) of virtual sequence
        /// Input Data : Sequence item with .ToString((IFormatProvider)null)
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceToString()
        {
            try
            {
                // Create virtual seqeunce with null value
                VirtualSequence virtualSeq = new VirtualSequence(Alphabets.Protein);
                virtualSeq.ToString();
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(
                    "Virtual Sequence P2: Sequence exception validated successfully.");
                Console.WriteLine(
                    "Virtual Sequence P2: Sequence exception validated successfully.");
            }
        }

        /// <summary>
        /// Invalidate IndexOfNonGap() method of VirtualSequence
        /// Input Data : Invalid Value
        /// Output Data : Validate Expected Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceIndexOfNonGap()
        {
            ValidateVirtualSequence(VirtualSequenceMethod.Gap);
        }

        /// <summary>
        /// Invalidate IndexOfNonGap(position) method of VirtualSequence
        /// Input Data : Invalid Value
        /// Output Data : Validate Expected Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceIndexOfNonGapPosition()
        {
            ValidateVirtualSequence(
                VirtualSequenceMethod.GapPosition);
        }

        /// <summary>
        /// Invalidate LastIndexOfNonGap() method of VirtualSequence
        /// Input Data : Invalid Value
        /// Output Data : Validate Expected Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceLastGap()
        {
            ValidateVirtualSequence(VirtualSequenceMethod.LastGap);
        }

        /// <summary>
        /// Invalidate LastIndexOfNonGap(position) method of VirtualSequence
        /// Input Data : Invalid Value
        /// Output Data : Validate Expected Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceLastIndexOfNonGapPosition()
        {
            ValidateVirtualSequence(
                VirtualSequenceMethod.LastGapPosition);
        }

        /// <summary>
        /// Invalidate Remove() method of VirtualSequence
        /// Input Data : Invalid Value
        /// Output Data : Validate Expected Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceRemove()
        {
            ValidateVirtualSequence(VirtualSequenceMethod.Remove);
        }

        /// <summary>
        /// Invalidate Replace() method of VirtualSequence
        /// Input Data : Invalid Value
        /// Output Data : Validate Expected Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceReplace()
        {
            ValidateVirtualSequence(VirtualSequenceMethod.Replace);
        }

        /// <summary>
        /// Invalidate GetEnumerator() method of VirtualSequence
        /// Input Data : Invalid Value
        /// Output Data : Validate Expected Result
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceGetEnum()
        {
            ValidateVirtualSequence(VirtualSequenceMethod.GetEnum);
        }

        /// <summary>
        /// Invalidate Insert() method of VirtualSequence
        /// Input Data : Invalid Value
        /// Output Data : Validate Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceInsert()
        {
            ValidateVirtualSequence(VirtualSequenceMethod.Insert);
        }

        /// <summary>
        /// Invalidate GetObjectData() method of VirtualSequence
        /// Input Data : null SerializationInfo
        /// Output Data : Validate Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateVirtualSequenceGetObjData()
        {
            ValidateVirtualSequence(VirtualSequenceMethod.GetObjData);
        }

        #endregion VirtualSequence P2 TestCases

        #region Supported Methods

        /// <summary>
        /// General method to validate VirtualSequence
        /// <param name="method"> Method name to be validate </param>
        /// </summary>
        static void ValidateVirtualSequence(VirtualSequenceMethod method)
        {
            try
            {
                ISequence virSeq = new VirtualSequence(Alphabets.DNA);

                switch (method)
                {
                    case VirtualSequenceMethod.Gap:
                        virSeq.IndexOfNonGap();
                        break;
                    case VirtualSequenceMethod.GapPosition:
                        virSeq.IndexOfNonGap(0);
                        break;
                    case VirtualSequenceMethod.LastGap:
                        virSeq.LastIndexOfNonGap();
                        break;
                    case VirtualSequenceMethod.LastGapPosition:
                        virSeq.LastIndexOfNonGap(0);
                        break;
                    case VirtualSequenceMethod.Remove:
                        virSeq.Remove(null);
                        break;
                    case VirtualSequenceMethod.Replace:
                        virSeq.Replace(0, null);
                        break;
                    case VirtualSequenceMethod.GetEnum:
                        virSeq.GetEnumerator();
                        break;
                    case VirtualSequenceMethod.Insert:
                        virSeq.Insert(0, null);
                        break;
                    case VirtualSequenceMethod.GetObjData:
                        virSeq.GetObjectData(null as SerializationInfo,
                            new StreamingContext());
                        break;
                }

                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(
                    "Virtual Sequence P2: Method exception validated successfully.");
                Console.WriteLine(
                    "Virtual Sequence P2: Method exception validated successfully.");
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "Virtual Sequence P2: Method exception validated successfully.");
                Console.WriteLine(
                    "Virtual Sequence P2: Method exception validated successfully.");
            }
        }

        #endregion Supported Methods
    }
}