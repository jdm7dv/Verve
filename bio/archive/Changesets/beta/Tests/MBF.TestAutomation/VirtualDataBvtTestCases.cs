// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * VirtualDataBVTTestCases.cs
 * 
 * This file contains the VirtualData BVT test case validation.
 * 
******************************************************************************/

using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Virtual Data and BVT level validations.
    /// </summary>
    [TestClass]
    public class VirtualDataBvtTestCases
    {

        #region Enums

        /// <summary>
        /// VirtualData additional parameters based on which 
        /// the test cases are validated.
        /// </summary>
        enum AdditionalParameters
        {
            Add,
            AddData,
            AddDelay,
            AddDataDelay,
            Clear,
            ClearStaleData,
            GetAllData,
            GetData,
            GetEnumerator,
            Insert,
            Remove,
            RemoveAt,
            Contains,
            CopyTo
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static VirtualDataBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Virtual Data BVT Test Cases

        /// <summary>
        /// Validate Virtual Data with Add() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataAdd()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.Add);
        }

        /// <summary>
        /// Validate Virtual Data with AddData() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataAddData()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.AddData);
        }

        /// <summary>
        /// Validate Virtual Data with Add() method with Delay.
        /// Input Data : Valid Cache Box with Delay
        /// Output Data : Validation of virtual data.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataAddDelay()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.AddDelay);
        }

        /// <summary>
        /// Validate Virtual Data with AddData() method with Delay.
        /// Input Data : Valid Cache Box with Delay
        /// Output Data : Validation of virtual data.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataAddDataDelay()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.AddDataDelay);
        }

        /// <summary>
        /// Validate Virtual Data with Clear() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of cleared virtual data.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataClear()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.Clear);
        }

        /// <summary>
        /// Validate Virtual Data with ClearStaleData() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of cleared stale virtual data.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataClearStaleData()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.ClearStaleData);
        }

        /// <summary>
        /// Validate Virtual Data with GetAllData() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data read.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataGetAllData()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.GetAllData);
        }

        /// <summary>
        /// Validate Virtual Data with GetData() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data read.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataGetData()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.GetData);
        }

        /// <summary>
        /// Validate Virtual Data with Insert() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data read.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataInsert()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.Insert);
        }

        /// <summary>
        /// Validate Virtual Data with Remove() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data removed.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataRemove()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.Remove);
        }

        /// <summary>
        /// Validate Virtual Data with RemoveAt() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data removed.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataRemoveAt()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.RemoveAt);
        }

        /// <summary>
        /// Validate Virtual Data with Contains() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataContains()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.Contains);
        }

        /// <summary>
        /// Validate Virtual Data with CopyTo() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataCopyTo()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.CopyTo);
        }

        /// <summary>
        /// Validate Virtual Data with GetEnumerator() method.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of virtual data removed.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataGetEnumerator()
        {
            ValidateGeneralVirtualDataTestCases(AdditionalParameters.GetEnumerator);
        }

        /// <summary>
        /// Validate all properties in Virtual Data.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of all properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVirtualDataAllProperties()
        {
            VirtualData<Sequence> vdObj = new VirtualData<Sequence>();
            Assert.AreEqual(0, vdObj.BlockSize);
            Assert.AreEqual(0, vdObj.Count);
            Assert.AreEqual(0, vdObj.BlockSize);
            Assert.IsFalse(vdObj.IsReadOnly);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(
                "Virtual Data BVT : Successfully validated all the properties.");
            ApplicationLog.WriteLine(
                "Virtual Data BVT : Successfully validated all the properties.");
        }

        /// <summary>
        /// Validate all properties in Cache Box.
        /// Input Data : Valid Cache Box
        /// Output Data : Validation of all properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateCacheBoxAllProperties()
        {
            CacheBox<Sequence> ch = new CacheBox<Sequence>(10);
            Sequence seqObj = new Sequence(Alphabets.DNA, "AGCT");
            ch.Data = seqObj;
            ch.EndRange = 6;
            DateTime nowObj = DateTime.Now;
            ch.LastAccessTime = nowObj;
            ch.StartRange = 0;

            Assert.AreEqual(10, ch.BlockSize);
            Assert.AreEqual(seqObj, ch.Data);
            Assert.AreEqual(6, ch.EndRange);
            Assert.AreEqual(nowObj, ch.LastAccessTime);
            Assert.AreEqual(0, ch.StartRange);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(
                "Cache Box BVT : Successfully validated all the properties.");
            ApplicationLog.WriteLine(
                "Cache Box BVT : Successfully validated all the properties.");
        }

        #endregion Virtual Data BVT Test Cases

        #region Supporting method

        /// <summary>
        /// General method to validate virtual data class.
        /// <param name="addParam">Additional parameter.</param>
        /// </summary>
        static void ValidateGeneralVirtualDataTestCases(AdditionalParameters addParam)
        {
            VirtualData<Sequence> vdObj = new VirtualData<Sequence>();

            // Sets the Initial Properties and required Cache box
            vdObj.MaxNumberOfBlocks = 5;
            vdObj.BlockSize = 5;
            CacheBox<Sequence> cb1 = new CacheBox<Sequence>(11);
            cb1.StartRange = 0;
            cb1.EndRange = 10;
            cb1.Data = new Sequence(Alphabets.DNA, "GGGGGGGGGGGGG");

            CacheBox<Sequence> cb2 = new CacheBox<Sequence>(11);
            cb2.StartRange = 0;
            cb2.EndRange = 10;
            cb2.Data = new Sequence(Alphabets.DNA, "TTTTTTTTTTTTT");

            CacheBox<Sequence> cb3 = new CacheBox<Sequence>(11);
            cb3.StartRange = 0;
            cb3.EndRange = 10;
            cb3.Data = new Sequence(Alphabets.DNA, "CCCCCCCCCCCCC");

            CacheBox<Sequence> cb4 = new CacheBox<Sequence>(11);
            cb4.StartRange = 0;
            cb4.EndRange = 10;
            cb4.Data = new Sequence(Alphabets.DNA, "AAAAAAAAAAAAAA");

            List<CacheBox<Sequence>> cbList = new List<CacheBox<Sequence>>();
            cbList.Add(cb1);
            cbList.Add(cb2);
            cbList.Add(cb3);
            cbList.Add(cb4);

            switch (addParam)
            {
                case AdditionalParameters.Add:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);

                    for (int i = 0; i < vdObj.Count; i++)
                    {
                        Assert.AreEqual(cbList[i].Data, vdObj[i].Data);
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the Add() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the Add() method.");
                    break;
                case AdditionalParameters.AddData:
                    vdObj.AddData(new Sequence(Alphabets.DNA, "GGGGGGGGGGGGG"), 0, 10);
                    vdObj.AddData(new Sequence(Alphabets.DNA, "TTTTTTTTTTTTT"), 0, 10);
                    vdObj.AddData(new Sequence(Alphabets.DNA, "CCCCCCCCCCCCC"), 0, 10);
                    vdObj.AddData(new Sequence(Alphabets.DNA, "AAAAAAAAAAAAAA"), 0, 10);

                    for (int i = 0; i < vdObj.Count; i++)
                    {
                        Assert.AreEqual(cbList[i].Data.ToString(), vdObj[i].Data.ToString());
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the AddData() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the AddData() method.");
                    break;
                case AdditionalParameters.AddDelay:
                    vdObj.MaxNumberOfBlocks = 2;
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    // Sleep for 20 seconds so that the cache box would be cleared
                    // and the count of items in the VirtualData would be reduced
                    Thread.Sleep(20000);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);

                    for (int i = 0; i < vdObj.Count; i++)
                    {
                        Assert.AreEqual(cbList[i + 2].Data, vdObj[i].Data);
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the Add() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the Add() method.");
                    break;
                case AdditionalParameters.AddDataDelay:
                    vdObj.MaxNumberOfBlocks = 2;
                    vdObj.AddData(new Sequence(Alphabets.DNA, "GGGGGGGGGGGGG"), 0, 10);
                    vdObj.AddData(new Sequence(Alphabets.DNA, "TTTTTTTTTTTTT"), 0, 10);
                    // Sleep for 20 seconds so that the cache box would be cleared
                    // and the count of items in the VirtualData would be reduced
                    Thread.Sleep(20000);
                    vdObj.AddData(new Sequence(Alphabets.DNA, "CCCCCCCCCCCCC"), 0, 10);
                    vdObj.AddData(new Sequence(Alphabets.DNA, "AAAAAAAAAAAAAA"), 0, 10);

                    for (int i = 0; i < vdObj.Count; i++)
                    {
                        Assert.AreEqual(cbList[i + 2].Data.ToString(), vdObj[i].Data.ToString());
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the AddData() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the AddData() method.");
                    break;
                case AdditionalParameters.Clear:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);
                    vdObj.Clear();
                    Assert.AreEqual(0, vdObj.Count);
                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the Clear() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the Clear() method.");
                    break;
                case AdditionalParameters.ClearStaleData:
                    vdObj.MaxNumberOfBlocks = 2;
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    // Sleep for 20 seconds so that the cache box would be cleared
                    // and the count of items in the VirtualData would be reduced
                    Thread.Sleep(20000);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);
                    Assert.IsTrue(3 > vdObj.Count);
                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the ClearStaleData() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the ClearStaleData() method.");
                    break;
                case AdditionalParameters.GetAllData:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);
                    // Gets all the data in Virtual data object
                    IList<Sequence> allSeqData = vdObj.GetAllData();
                    for (int i = 0; i < allSeqData.Count; i++)
                    {
                        Assert.AreEqual(cbList[i].Data.ToString(), allSeqData[i].ToString());
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the GetAllData() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the GetAllData() method.");
                    break;
                case AdditionalParameters.GetData:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);
                    // Gets the data in Virtual data object
                    for (int i = 0; i < vdObj.Count; i++)
                    {
                        Assert.AreEqual(cbList[i].Data.ToString(), vdObj.GetData(i).ToString());
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the GetData() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the GetData() method.");
                    break;
                case AdditionalParameters.GetEnumerator:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    IEnumerator<CacheBox<Sequence>> enumObj = vdObj.GetEnumerator();
                    Assert.IsTrue(null != enumObj);
                    Assert.AreEqual(cb1, vdObj.FirstOrDefault(C => 0 >= C.StartRange && 0 <= C.EndRange));
                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the GetEnumerator() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the GetEnumerator() method.");
                    break;
                case AdditionalParameters.Insert:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    vdObj.Add(cb3);
                    // Inserts the cache box in Virtual data object
                    vdObj.Insert(3, cb4);
                    for (int i = 0; i < vdObj.Count; i++)
                    {
                        Assert.AreEqual(cbList[i].Data, vdObj[i].Data);
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the Insert() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the Insert() method.");
                    break;
                case AdditionalParameters.Remove:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);

                    // Add a Cache Box and remove the same
                    CacheBox<Sequence> cb5 = new CacheBox<Sequence>(11);
                    cb5.StartRange = 0;
                    cb5.EndRange = 10;
                    cb5.Data = new Sequence(Alphabets.DNA, "AAAAAATTTTTTT");
                    vdObj.Add(cb5);
                    vdObj.Remove(cb5);
                    // Removes the cache box from the virtual data list
                    for (int i = 0; i < cbList.Count; i++)
                    {
                        Assert.AreEqual(cbList[i].Data, vdObj[i].Data);
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the Remove() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the Remove() method.");
                    break;
                case AdditionalParameters.RemoveAt:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);

                    // Add a Cache Box and remove the same
                    CacheBox<Sequence> cbRemove = new CacheBox<Sequence>(11);
                    cbRemove.StartRange = 0;
                    cbRemove.EndRange = 10;
                    cbRemove.Data = new Sequence(Alphabets.DNA, "AAAAAATTTTTTT");
                    vdObj.Add(cbRemove);
                    vdObj.RemoveAt(4);

                    for (int i = 0; i < cbList.Count; i++)
                    {
                        Assert.AreEqual(cbList[i].Data, vdObj[i].Data);
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the RemoveAt() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the RemoveAt() method.");
                    break;
                case AdditionalParameters.Contains:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);
                    // Validates the contains block
                    Assert.IsTrue(vdObj.Contains(cb4));

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the Contains() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the Contains() method.");
                    break;
                case AdditionalParameters.CopyTo:
                    vdObj.Add(cb1);
                    vdObj.Add(cb2);
                    vdObj.Add(cb3);
                    vdObj.Add(cb4);
                    CacheBox<Sequence>[] cbArray = new CacheBox<Sequence>[vdObj.Count];
                    vdObj.CopyTo(cbArray, 0);

                    for (int i = 0; i < vdObj.Count; i++)
                    {
                        Assert.AreEqual(cbArray[i].Data, vdObj[i].Data);
                    }

                    // Logs to the NUnit GUI (Console.Out) window
                    Console.WriteLine(
                        "Virtual Data BVT : Successfully validated the CopyTo() method.");
                    ApplicationLog.WriteLine(
                        "Virtual Data BVT : Successfully validated the CopyTo() method.");
                    break;

                default:
                    break;
            }
        }

        #endregion Supporting method
    }
}