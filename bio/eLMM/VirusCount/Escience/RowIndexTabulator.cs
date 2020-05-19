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

using System.Collections.Generic;
using Bio.Util;

namespace MBT.Escience
{
    abstract public class RowIndexTabulator
    {
        internal RowIndexTabulator()
        {
        }

        static public RowIndexTabulator GetInstance(bool audit)
        {
            if (audit)
            {
                return new Auditor();
            }
            else
            {
                return new NoAudit();
            }
        }

        /// <returns>True if item was added. False if it already existed in the range.</returns>
        abstract public bool TryAdd(Dictionary<string, string> row, string fileName);
        abstract public bool IsComplete();
        abstract public string GetSkipRangeCollection();

        public void CheckIsComplete(string inputFilePattern)
        {
            Helper.CheckCondition(IsComplete(),
                string.Format("Not all needed rows were found. Here are the indexes of the found rows:\n{0}\n{1}", GetSkipRangeCollection(), inputFilePattern));
        }
    }

    public class Auditor : RowIndexTabulator
    {
        internal Auditor()
        {
        }

        RangeCollection RowIndexRangeCollection = new RangeCollection();
        long RowCountSoFar = long.MinValue;

        /// <returns>True if item was added. False if it already existed in the range.</returns>
        override public bool TryAdd(Dictionary<string, string> row, string fileName)
        {
            Helper.CheckCondition(row.ContainsKey("rowIndex"), string.Format(@"When auditing tabulation a ""rowIndex"" column is required. (File ""{0}"")", fileName));
            Helper.CheckCondition(row.ContainsKey("rowCount"), string.Format(@"When auditing tabulation a ""rowCount"" column is required. (File ""{0}"")", fileName));

            RangeCollection rowIndexRange = RangeCollection.Parse(row["rowIndex"]);
            long rowCount = long.Parse(row["rowCount"]);

            Helper.CheckCondition(rowIndexRange.IsBetween(0, rowCount - 1), string.Format(@"rowIndex must be at least zero and less than rowCount (File ""{0}"")", fileName));
            if (RowCountSoFar == long.MinValue)
            {
                RowCountSoFar = rowCount;
            }
            else
            {
                Helper.CheckCondition(RowCountSoFar == rowCount, string.Format("A different row count was at rowIndex {0} in file {1}", rowIndexRange, fileName));
            }

            bool tryAdd = RowIndexRangeCollection.TryAddRangeCollection(rowIndexRange);
            return tryAdd;
        }

        public override bool IsComplete()
        {
            return RowIndexRangeCollection.IsComplete(RowCountSoFar);
        }

        public override string GetSkipRangeCollection()
        {
            return RowIndexRangeCollection.ToString();
        }
    }

    public class NoAudit : RowIndexTabulator
    {
        internal NoAudit()
        {
        }

        override public bool TryAdd(Dictionary<string, string> row, string fileName)
        {
            return true;
        }

        public override bool IsComplete()
        {
            return true;
        }

        public override string GetSkipRangeCollection()
        {
            return "";
        }
    }

}
