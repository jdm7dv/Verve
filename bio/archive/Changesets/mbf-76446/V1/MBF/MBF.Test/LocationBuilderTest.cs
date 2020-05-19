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
using System.Text;
using NUnit.Framework;
using MBF.Encoding;
using MBF.IO.GenBank;

namespace MBF.Test
{
    /// <summary>
    /// Class to test Location Builder.
    /// </summary>
    [TestFixture]
    public class LocationBuilderTest
    {
        /// <summary>
        /// Tests the location builder.
        /// </summary>
        [Test]
        public void TestLocationBuilder()
        {
            string location = "123";
            string result = string.Empty;
            ILocationBuilder locBuilder = new LocationBuilder();
            ILocation loc = locBuilder.GetLocation(location);
            Assert.AreEqual(123, loc.Start);
            Assert.AreEqual(123, loc.End);
            Assert.IsTrue(string.IsNullOrEmpty(loc.Separator));
            Assert.AreEqual(LocationOperator.None, loc.Operator);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "123.125";
            loc = locBuilder.GetLocation(location);
            Assert.AreEqual(123, loc.Start);
            Assert.AreEqual(125, loc.End);
            Assert.AreEqual(".", loc.Separator);
            Assert.AreEqual(LocationOperator.None, loc.Operator);
            Assert.AreEqual(0, loc.SubLocations.Count);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "123^125";
            loc = locBuilder.GetLocation(location);
            Assert.AreEqual(123, loc.Start);
            Assert.AreEqual(125, loc.End);
            Assert.AreEqual("^", loc.Separator);
            Assert.AreEqual(LocationOperator.None, loc.Operator);
            Assert.AreEqual(0, loc.SubLocations.Count);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "123..125";
            loc = locBuilder.GetLocation(location);
            Assert.AreEqual(123, loc.Start);
            Assert.AreEqual(125, loc.End);
            Assert.AreEqual("..", loc.Separator);
            Assert.AreEqual(LocationOperator.None, loc.Operator);
            Assert.AreEqual(0, loc.SubLocations.Count);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "complement(123..125)";
            loc = locBuilder.GetLocation(location);
            Assert.AreEqual(123, loc.Start);
            Assert.AreEqual(125, loc.End);
            Assert.IsTrue(string.IsNullOrEmpty(loc.Separator));
            Assert.AreEqual(LocationOperator.Complement, loc.Operator);
            Assert.AreEqual(1, loc.SubLocations.Count);
            Assert.AreEqual(123, loc.SubLocations[0].Start);
            Assert.AreEqual(125, loc.SubLocations[0].End);
            Assert.AreEqual("..", loc.SubLocations[0].Separator);
            Assert.AreEqual(LocationOperator.None, loc.SubLocations[0].Operator);
            Assert.AreEqual(0, loc.SubLocations[0].SubLocations.Count);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "join(complement(123..125),200..300)";
            loc = locBuilder.GetLocation(location);
            Assert.AreEqual(123, loc.Start);
            Assert.AreEqual(300, loc.End);
            Assert.IsTrue(string.IsNullOrEmpty(loc.Separator));
            Assert.AreEqual(LocationOperator.Join, loc.Operator);
            Assert.AreEqual(2, loc.SubLocations.Count);

            Assert.AreEqual(123, loc.SubLocations[0].Start);
            Assert.AreEqual(125, loc.SubLocations[0].End);
            Assert.IsTrue(string.IsNullOrEmpty(loc.SubLocations[0].Separator));
            Assert.AreEqual(LocationOperator.Complement, loc.SubLocations[0].Operator);
            Assert.AreEqual(1, loc.SubLocations[0].SubLocations.Count);

            Assert.AreEqual(200, loc.SubLocations[1].Start);
            Assert.AreEqual(300, loc.SubLocations[1].End);
            Assert.AreEqual("..", loc.SubLocations[1].Separator);
            Assert.AreEqual(LocationOperator.None, loc.SubLocations[1].Operator);
            Assert.AreEqual(0, loc.SubLocations[1].SubLocations.Count);

            List<ILocation> leafLocs = loc.GetLeafLocations();
            Assert.AreEqual(2, leafLocs.Count);
            Assert.AreEqual(123, leafLocs[0].Start);
            Assert.AreEqual(125, leafLocs[0].End);
            Assert.AreEqual(200, leafLocs[1].Start);
            Assert.AreEqual(300, leafLocs[1].End);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "join(complement(123..125),complement(200..300))";
            loc = locBuilder.GetLocation(location);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "join(complement(123),complement(200..300))";
            loc = locBuilder.GetLocation(location);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "complement(join(123..125,200..300))";
            loc = locBuilder.GetLocation(location);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "order(123..125,200..300)";
            loc = locBuilder.GetLocation(location);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);

            location = "complement(join(<123..125,200..>300))";
            loc = locBuilder.GetLocation(location);
            result = locBuilder.GetLocationString(loc);
            Assert.AreEqual(location, result);
        }
    }
}
