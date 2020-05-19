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
using System.Linq;
using System.Text;
using Bio.Matrix;
using Bio.Util;
using System.Threading.Tasks;
using MBT.Escience; using MBT.Escience.Parse;

namespace EScienceLearn.RowFilters
{
    [Serializable]
    abstract public class RowFilter
    {
        [Parse(ParseAction.Optional)]
        public bool Verbose = false;

        virtual public Matrix<string, string, T> Filter<T>(Matrix<string, string, T> predictorIn, Matrix<string, string, T> target)
        {
            var counterWithMessages = new CounterWithMessages("rowFilter " + ToString(), null, predictorIn.RowCount, quiet:!Verbose);
            var goodRowKeySet =
                (
                from rowKey in predictorIn.RowKeys
                .AsParallel().WithParallelOptionsScope()
                where AlwaysTrue(counterWithMessages) && IsGood(predictorIn.SelectRowsView(rowKey), target)
                select rowKey
            ).ToHashSet();
            counterWithMessages.Finished();

            var predictorOut = predictorIn.SelectRowsView(predictorIn.RowKeys.Intersect(goodRowKeySet));
            return predictorOut;
        }

        public abstract bool IsGood<T>(Matrix<string, string, T> rowMatrix, Matrix<string, string, T> target);

        public bool AlwaysTrue(CounterWithMessages counterWithMessages)
        {
            counterWithMessages.Increment();
            return true;
        }

        public override string ToString()
        {
            string name = ConstructorArguments.FromParsable(this).ToString();
            return name;
        }

        [Parse(ParseAction.ArgumentString)]
        public string ArgumentString; //Can't be private because if it was the derived types could not see its value.

        public static Matrix<string, string, T> ApplyList<T>(List<RowFilter> RowFilterList, Matrix<string, string, T> input,  Matrix<string, string, T> target)
        {
            var output= input;
            foreach (var rowFilter in RowFilterList)
            {
                output = rowFilter.Filter(output, target);
            }
            return output;
        }
    }
}
