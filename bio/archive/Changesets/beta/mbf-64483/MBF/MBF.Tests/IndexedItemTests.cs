// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class to test IndexedItem.
    /// </summary>
    [TestClass]
    public class IndexedItemTests
    {
        /// <summary>
        /// Test IndexedItem class with ISequenceItem.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestIndexedItemWithISequenceItem()
        {
            IndexedItem<ISequenceItem> item1 = new IndexedItem<ISequenceItem>(2, Alphabets.DNA.A);
            IndexedItem<ISequenceItem> item2 = new IndexedItem<ISequenceItem>(2, Alphabets.DNA.A);
            IndexedItem<ISequenceItem> item3 = new IndexedItem<ISequenceItem>(0, Alphabets.DNA.G);
            IndexedItem<ISequenceItem> item4 = new IndexedItem<ISequenceItem>(1, Alphabets.DNA.A);
            IndexedItem<ISequenceItem> item5 = new IndexedItem<ISequenceItem>(2, Alphabets.DNA.T);

            IndexedItem<ISequenceItem> nullitem1 = null;
            IndexedItem<ISequenceItem> nullitem2 = null;

            Assert.AreEqual(item1.Index, 2);
            Assert.AreSame(item1.Item, Alphabets.DNA.A);

            Assert.AreEqual(item1, item2);
            Assert.IsTrue(item3 <= item1);
            Assert.IsTrue(item2 <= item1);
            Assert.IsTrue(item3 < item1);
            Assert.IsTrue(item1 > item3);
            Assert.IsTrue(item1 >= item3);
            Assert.IsTrue(item1 >= item2);
            Assert.AreNotEqual(item1, item3);

            #region Test - CompareTo
            Assert.IsTrue(item1.CompareTo(item2) == 0);
            Assert.IsTrue(item3.CompareTo(item1) < 0);
            Assert.IsTrue(item1.CompareTo(item3) > 0);
            Assert.IsTrue(item1.CompareTo(null) > 0);

            try
            {
                Assert.IsFalse(item1.CompareTo("ABCD") == 0);
                Assert.Fail();
            }
            catch
            {
            }

            List<IndexedItem<ISequenceItem>> seqItemList = new List<IndexedItem<ISequenceItem>>();

            seqItemList.Add(item1);
            seqItemList.Add(item3);
            seqItemList.Add(item4);
            seqItemList.Sort();
            Assert.AreEqual(seqItemList[0].Index, 0);
            Assert.AreEqual(seqItemList[1].Index, 1);
            Assert.AreEqual(seqItemList[2].Index, 2);
            Assert.AreSame(seqItemList[0], item3);
            Assert.AreSame(seqItemList[1], item4);
            Assert.AreSame(seqItemList[2], item1);
            #endregion Test - CompareTo

            #region Test - Equals
            Assert.IsTrue(item1.Equals(item2));
            Assert.IsFalse(item1.Equals(item3));
            Assert.IsFalse(item1.Equals(null));
            Assert.IsFalse(item1.Equals(nullitem1));
            Assert.IsFalse(item5.Equals(item1));

            Assert.IsFalse(item1.Equals("ABCD"));

            Assert.IsFalse(object.ReferenceEquals(item1, item2));
            IndexedItem<ISequenceItem> refItem = item1;
            Assert.IsTrue(object.ReferenceEquals(item1, refItem));
            #endregion Test - Equals

            #region Test - "==" operator
            Assert.IsTrue(nullitem1 == nullitem2);
            Assert.IsFalse(item1 == item2);
            Assert.IsFalse(item1 == item3);
            Assert.IsFalse(item1 == nullitem1);
            Assert.IsFalse(nullitem1 == item1);
            #endregion Test - "==" operator

            #region Test - "!=" operator
            Assert.IsFalse(nullitem1 != nullitem2);
            Assert.IsTrue(item1 != item3);
            Assert.IsTrue(item1 != item2);
            Assert.IsTrue(item1 != nullitem1);
            Assert.IsTrue(nullitem1 != item1);
            #endregion Test - "!=" operator

            #region Test - "<" operator
            Assert.IsFalse(nullitem1 < nullitem2);
            Assert.IsTrue(item4 < item1);
            Assert.IsFalse(item1 < item4);
            Assert.IsTrue(nullitem1 < item1);
            Assert.IsFalse(item1 < nullitem1);
            #endregion Test - "<" operator

            #region Test - "<=" operator
            Assert.IsTrue(nullitem1 <= nullitem2);
            Assert.IsTrue(item4 <= item1);
            Assert.IsFalse(item1 <= item4);
            Assert.IsTrue(nullitem1 <= item1);
            Assert.IsFalse(item1 <= nullitem1);
            Assert.IsTrue(item1 <= item2);
            #endregion Test - "<=" operator

            #region Test - ">" operator
            Assert.IsFalse(nullitem1 > nullitem2);
            Assert.IsFalse(item4 > item1);
            Assert.IsTrue(item1 > item4);
            Assert.IsFalse(nullitem1 > item1);
            Assert.IsTrue(item1 > nullitem1);
            #endregion Test - ">" operator

            #region Test - ">=" operator
            Assert.IsTrue(nullitem1 >= nullitem2);
            Assert.IsFalse(item4 >= item1);
            Assert.IsTrue(item1 >= item4);
            Assert.IsFalse(nullitem1 >= item1);
            Assert.IsTrue(item1 >= nullitem1);
            Assert.IsTrue(item1 >= item2);
            #endregion Test - ">=" operator

            #region Test - GetHashCode
            Assert.AreEqual(item1.GetHashCode(), item2.GetHashCode());
            Assert.AreNotEqual(item1.GetHashCode(), item5.GetHashCode());
            #endregion Test - GetHashCode
        }

        /// <summary>
        /// Test IndexedItem class with UpdatedSequenceItem.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestIndexedItemWithUpdatedSequenceItem()
        {
            IndexedItem<UpdatedSequenceItem> item1 = new IndexedItem<UpdatedSequenceItem>(2, new UpdatedSequenceItem(Alphabets.DNA.A, UpdateType.Inserted));
            IndexedItem<UpdatedSequenceItem> item2 = new IndexedItem<UpdatedSequenceItem>(2, item1.Item);
            IndexedItem<UpdatedSequenceItem> item3 = new IndexedItem<UpdatedSequenceItem>(0, new UpdatedSequenceItem(Alphabets.DNA.G, UpdateType.Removed));
            IndexedItem<UpdatedSequenceItem> item4 = new IndexedItem<UpdatedSequenceItem>(1, new UpdatedSequenceItem(Alphabets.DNA.A, UpdateType.Replaced));
            IndexedItem<UpdatedSequenceItem> item5 = new IndexedItem<UpdatedSequenceItem>(2, new UpdatedSequenceItem(Alphabets.DNA.T, UpdateType.Inserted));

            IndexedItem<UpdatedSequenceItem> nullitem1 = null;
            IndexedItem<UpdatedSequenceItem> nullitem2 = null;

            Assert.AreEqual(item1.Index, 2);
            Assert.AreSame(item1.Item.SequenceItem, Alphabets.DNA.A);

            Assert.AreEqual(item1, item2);
            Assert.IsTrue(item3 <= item1);
            Assert.IsTrue(item2 <= item1);
            Assert.IsTrue(item3 < item1);
            Assert.IsTrue(item1 > item3);
            Assert.IsTrue(item1 >= item3);
            Assert.IsTrue(item1 >= item2);
            Assert.AreNotEqual(item1, item3);

            #region Test - CompareTo
            Assert.IsTrue(item1.CompareTo(item2) == 0);
            Assert.IsTrue(item3.CompareTo(item1) < 0);
            Assert.IsTrue(item1.CompareTo(item3) > 0);
            Assert.IsTrue(item1.CompareTo(null) > 0);

            try
            {
                Assert.IsFalse(item1.CompareTo("ABCD") == 0);
                Assert.Fail();
            }
            catch
            {
            }

            List<IndexedItem<UpdatedSequenceItem>> seqItemList = new List<IndexedItem<UpdatedSequenceItem>>();

            seqItemList.Add(item1);
            seqItemList.Add(item3);
            seqItemList.Add(item4);
            seqItemList.Sort();
            Assert.AreEqual(seqItemList[0].Index, 0);
            Assert.AreEqual(seqItemList[1].Index, 1);
            Assert.AreEqual(seqItemList[2].Index, 2);
            Assert.AreSame(seqItemList[0], item3);
            Assert.AreSame(seqItemList[1], item4);
            Assert.AreSame(seqItemList[2], item1);
            #endregion Test - CompareTo

            #region Test - Equals
            Assert.IsTrue(item1.Equals(item2));
            Assert.IsFalse(item1.Equals(item3));
            Assert.IsFalse(item1.Equals(null));
            Assert.IsFalse(item1.Equals(nullitem1));
            Assert.IsFalse(item5.Equals(item1));

            Assert.IsFalse(item1.Equals("ABCD"));

            Assert.IsFalse(object.ReferenceEquals(item1, item2));
            IndexedItem<UpdatedSequenceItem> refItem = item1;
            Assert.IsTrue(object.ReferenceEquals(item1, refItem));
            #endregion Test - Equals

            #region Test - "==" operator
            Assert.IsTrue(nullitem1 == nullitem2);
            Assert.IsFalse(item1 == item2);
            Assert.IsFalse(item1 == item3);
            Assert.IsFalse(item1 == nullitem1);
            Assert.IsFalse(nullitem1 == item1);
            #endregion Test - "==" operator

            #region Test - "!=" operator
            Assert.IsFalse(nullitem1 != nullitem2);
            Assert.IsTrue(item1 != item3);
            Assert.IsTrue(item1 != item2);
            Assert.IsTrue(item1 != nullitem1);
            Assert.IsTrue(nullitem1 != item1);
            #endregion Test - "!=" operator

            #region Test - "<" operator
            Assert.IsFalse(nullitem1 < nullitem2);
            Assert.IsTrue(item4 < item1);
            Assert.IsFalse(item1 < item4);
            Assert.IsTrue(nullitem1 < item1);
            Assert.IsFalse(item1 < nullitem1);
            #endregion Test - "<" operator

            #region Test - "<=" operator
            Assert.IsTrue(nullitem1 <= nullitem2);
            Assert.IsTrue(item4 <= item1);
            Assert.IsFalse(item1 <= item4);
            Assert.IsTrue(nullitem1 <= item1);
            Assert.IsFalse(item1 <= nullitem1);
            Assert.IsTrue(item1 <= item2);
            #endregion Test - "<=" operator

            #region Test - ">" operator
            Assert.IsFalse(nullitem1 > nullitem2);
            Assert.IsFalse(item4 > item1);
            Assert.IsTrue(item1 > item4);
            Assert.IsFalse(nullitem1 > item1);
            Assert.IsTrue(item1 > nullitem1);
            #endregion Test - ">" operator

            #region Test - ">=" operator
            Assert.IsTrue(nullitem1 >= nullitem2);
            Assert.IsFalse(item4 >= item1);
            Assert.IsTrue(item1 >= item4);
            Assert.IsFalse(nullitem1 >= item1);
            Assert.IsTrue(item1 >= nullitem1);
            Assert.IsTrue(item1 >= item2);
            #endregion Test - ">=" operator

            #region Test - GetHashCode
            Assert.AreEqual(item1.GetHashCode(), item2.GetHashCode());
            Assert.AreNotEqual(item1.GetHashCode(), item5.GetHashCode());
            #endregion Test - GetHashCode
        }
    }
}
