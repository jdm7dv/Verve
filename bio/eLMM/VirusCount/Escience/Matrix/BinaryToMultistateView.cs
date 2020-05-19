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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Bio.Matrix;
using Bio.Util;
using MBT.Escience.LogisticRegression;

namespace MBT.Escience.Matrix
{
    /// <summary>
    /// This class wraps a matrix of binary (0/1) values in a view that makes it look multistate.
    /// </summary>
    /// <remarks>
    /// The primary input of this class is the mapping from multistate values to binary values. This is an enumeration
    /// of key value pairs, where the key is the multistate key and the value is and enumeration of binary values that
    /// map to that multistate key. Here is an example used in the PhyloD context. Here, binary keys are of the form
    /// 1@A, 1@B, 1@C, which means position 1 can take on values of A,B or C. The multistate key of position 1 is
    /// 1@A#B#C.  
    /// <code>
    /// var multToBinary = multistateView.RowKeys.Select(key =>
    ///         Tabulate.GetMerAndPos(key)).Select(merAndPos =>
    ///             new KeyValuePair&lt; string, IEnumerable&lt; string&gt; &gt;(merAndPos.Value + "@" + merAndPos.Key,
    ///             merAndPos.Key.Split('#').Select(aa => (int)merAndPos.Value + "@" + aa)));
    /// </code>
    /// Note this view is completely generic. This makes for some difficulty with the TVal, as binary and multistate suggest
    /// 0/1 and integer encoding, respectfully. To allow TVal to be fully generic, we therefore require the caller to provide
    /// delegates mapping from "state" (ie, 0,1,2...) to TVal, and vice versa, which are specified using a ValueConverter from TVal
    /// to int.
    /// </remarks>
    public class BinaryToMultistateView<TRowKey, TColKey, TVal> : Matrix<TRowKey, TColKey, TVal>
    {
        private Matrix<TRowKey, TColKey, TVal> _binaryMatrix;
        private Dictionary<TRowKey, List<TRowKey>> _multKeyToBinaryKeys;
        private ValueConverter<TVal, Multinomial> _cnvrtTValToState;
        //private Converter<int, TVal> _fnStateToTVal;
        //private Converter<TVal, int> _fnTValToState;

        /// <summary>
        /// Creates a this wrapper.
        /// </summary>
        /// <param name="binaryMatrixToWrap">Must have values that map to 0/1</param>
        /// <param name="multistateKeyToOrderedBinaryKeys">First key is multistate name (e.g. 1@A#B#C), the value is an enumeration of binary keys that map to the multistate keys.
        /// The order specifies the state value in multistate space. (e.g. 1@A is state 0, 1@B is state 2, 1@C is state 2)</param>
        /// <param name="convertTValToMultinomial">Specifies how to convert from TVal to 0,1,2,3... and back. 0/1 are used for binary, 0..N are assumed to be used by the multistate matrix.</param>
        public BinaryToMultistateView(
            Matrix<TRowKey, TColKey, TVal> binaryMatrixToWrap,
            IEnumerable<KeyValuePair<TRowKey, IEnumerable<TRowKey>>> multistateKeyToOrderedBinaryKeys,
            ValueConverter<TVal, Multinomial> convertTValToMultinomial)
        {
            _binaryMatrix = binaryMatrixToWrap;
            _multKeyToBinaryKeys = multistateKeyToOrderedBinaryKeys.Select(pair =>
                new KeyValuePair<TRowKey, List<TRowKey>>(pair.Key, pair.Value.ToList())).ToDictionary();
            //_fnStateToTVal = fnStateToTVal;
            //_fnTValToState = fnTValToBinaryState;
            _cnvrtTValToState = convertTValToMultinomial;

            _rowKeys = new ReadOnlyCollection<TRowKey>(multistateKeyToOrderedBinaryKeys.Select(pair => pair.Key).ToList());
            _indexOfRowKey = new RestrictedAccessDictionary<TRowKey, int>(_rowKeys.Select((key, idx) => new KeyValuePair<TRowKey, int>(key, idx)).ToDictionary());
        }

        /// <summary>
        /// Creates a this wrapper.
        /// </summary>
        /// <param name="binaryMatrixToWrap">Must have values that map to 0/1</param>
        /// <param name="multistateKeyToOrderedBinaryKeys">First key is multistate name (e.g. 1@A#B#C), the value is an enumeration of binary keys that map to the multistate keys.
        /// The order specifies the state value in multistate space. (e.g. 1@A is state 0, 1@B is state 2, 1@C is state 2)</param>
        /// <param name="convertTValToInt">Specifies how to convert from TVal to 0,1,2,3... and back. 0/1 are used for binary, 0..N are assumed to be used by the multistate matrix.</param>
        public BinaryToMultistateView(
            Matrix<TRowKey, TColKey, TVal> binaryMatrixToWrap,
            IEnumerable<KeyValuePair<TRowKey, IEnumerable<TRowKey>>> multistateKeyToOrderedBinaryKeys,
            ValueConverter<TVal, int> convertTValToInt)
            : this(binaryMatrixToWrap, multistateKeyToOrderedBinaryKeys,
                new ValueConverter<TVal, Multinomial>(v => IntToMultinomial(convertTValToInt.ConvertForward(v)), m => convertTValToInt.ConvertBackward(MultinomialToInt(m))))
        { }

        private static Multinomial IntToMultinomial(int i)
        {
            return new Multinomial(i + 1, i);
        }

        private static int MultinomialToInt(Multinomial m)
        {
            return m.BestOutcome;
        }

        private ReadOnlyCollection<TRowKey> _rowKeys;
        public override ReadOnlyCollection<TRowKey> RowKeys
        {
            get { return _rowKeys; }
        }

        public override ReadOnlyCollection<TColKey> ColKeys
        {
            get { return _binaryMatrix.ColKeys; }
        }

        private RestrictedAccessDictionary<TRowKey, int> _indexOfRowKey;
        public override IDictionary<TRowKey, int> IndexOfRowKey
        {
            get { return _indexOfRowKey; }
        }

        public override IDictionary<TColKey, int> IndexOfColKey
        {
            get { return _binaryMatrix.IndexOfColKey; }
        }

        public override TVal MissingValue
        {
            get { return _binaryMatrix.MissingValue; }
        }

        //public override bool TryGet(TRowKey rowKey, TColKey colKey, out TVal value)
        //{
        //    value = MissingValue;
        //    if (!_multKeyToBinaryKeys.ContainsKey(rowKey))
        //        return false;

        //    var binaryKeys = _multKeyToBinaryKeys[rowKey];
        //    if (binaryKeys.Count == 1)
        //        return _binaryMatrix.TryGet(rowKey, colKey, out value);


        //    int state = 0;
        //    TVal tempVal;
        //    foreach (var key in binaryKeys)
        //    {
        //        if (_binaryMatrix.TryGet(key, colKey, out tempVal) && _cnvrtTValToState.ConvertForward(tempVal) == 1)//_fnTValToState(tempVal) == 1)
        //        {
        //            //value = _fnStateToTVal(state);
        //            value = _cnvrtTValToState.ConvertBackward(state);
        //            return true;
        //        }
        //        state++;
        //    }
        //    return false;   //must be missing if we get to here.
        //}

        //public override bool TryGet(int rowIndex, int colIndex, out TVal value)
        //{
        //    return TryGet(RowKeys[rowIndex], ColKeys[colIndex], out value);
        //}

        public override bool Remove(TRowKey rowKey, TColKey colKey)
        {
            bool removed = false;
            foreach (var key in _multKeyToBinaryKeys[rowKey])
            {
                removed |= _binaryMatrix.Remove(key, colKey);
            }
            return removed;
        }

        public override bool Remove(int rowIndex, int colIndex)
        {
            return Remove(RowKeys[rowIndex], ColKeys[colIndex]);
        }

        public override TVal this[TRowKey rowKey, TColKey colKey]
        {
            get
            {
                TVal result;
                if (TryGetValue(rowKey, colKey, out result))
                    return result;
                else
                    throw new Exception(string.Format("[{0},{1}] is missing.", rowKey, colKey));
            }
            set
            {
                if (MissingValue.Equals(value))
                    throw new Exception("Can't assign missing value using this[row,col]. Use SetValueOrMissing(row, col, value) or Remove(row,vol)");

                var binaryKeys = _multKeyToBinaryKeys[rowKey];
                Helper.CheckCondition<InvalidOperationException>(binaryKeys.Count > 0, "Every multistate key must have at least one binary key. {0} does not.", rowKey);
                if (binaryKeys.Count == 1 && binaryKeys[0].Equals(rowKey))
                    _binaryMatrix[binaryKeys[0], colKey] = value;

                else
                {
                    //int state = _cnvrtTValToState.ConvertForward(value);
                    Multinomial m = _cnvrtTValToState.ConvertForward(value);
                    Helper.CheckCondition(m.NumOutcomes == binaryKeys.Count || m.IsDefinite && m.NumOutcomes <= binaryKeys.Count,
                        "{0}, which maps to state {1} is outside the value range of values for this state ([{2},{3}])",
                        value, m, 0, binaryKeys.Count - 1);

                    int state = 0;
                    foreach (var key in binaryKeys)
                    {
                        double prob = m[state];
                        Multinomial binomial = new Multinomial(1 - prob, prob);
                        _binaryMatrix[key, colKey] = _cnvrtTValToState.ConvertBackward(binomial);
                        state++;
                    }

                    //int hotState = 0;
                    //TVal trueState = _cnvrtTValToState.ConvertBackward(1);
                    //TVal falseState = _cnvrtTValToState.ConvertBackward(0);
                    //foreach (var key in binaryKeys)
                    //{
                    //    _binaryMatrix[key, colKey] = hotState == state ? trueState : falseState;
                    //    hotState++;
                    //}
                }
            }
        }

        public override TVal this[int rowIndex, int colIndex]
        {
            get
            {
                return this[RowKeys[rowIndex], ColKeys[colIndex]];
            }
            set
            {
                this[RowKeys[rowIndex], ColKeys[colIndex]] = value;
            }
        }

        public override bool TryGetValue(TRowKey rowKey, TColKey colKey, out TVal value)
        {
            value = MissingValue;



            List<TRowKey> binaryKeys;
            if (!_multKeyToBinaryKeys.TryGetValue(rowKey, out binaryKeys))
                return false;

            // if they're the same key, just return the binary result. If you don't, you'll have problems because the semantics are different. 
            // (below, will cycle through states until one is found with value=1)
            if (binaryKeys.Count == 1 && binaryKeys[0].Equals(rowKey))
                return _binaryMatrix.TryGetValue(binaryKeys[0], colKey, out value);
            Helper.CheckCondition<InvalidOperationException>(binaryKeys.Count > 0, "Every multistate key must have at least one binary key. {0} does not.", rowKey);


            int state = 0;
            TVal tempVal;
            double[] probs = new double[binaryKeys.Count];
            double sum = 0;

            foreach (var key in binaryKeys)
            {
                if (_binaryMatrix.TryGetValue(key, colKey, out tempVal))//_fnTValToState(tempVal) == 1)
                {
                    Multinomial m = _cnvrtTValToState.ConvertForward(tempVal);
                    probs[state] = m[1];
                    sum += probs[state];

                    if (sum == 1)
                    {
                        value = _cnvrtTValToState.ConvertBackward(new Multinomial(probs));
                        return true;
                    }
                    //value = _fnStateToTVal(state);
                    //value = _cnvrtTValToState.ConvertBackward(state);
                    //return true;
                }
                state++;
            }
            return false;   //must be missing if we get to here.
        }

        public override bool TryGetValue(int rowIndex, int colIndex, out TVal value)
        {
            return TryGetValue(RowKeys[rowIndex], ColKeys[colIndex], out value);
        }

        public override void SetValueOrMissing(TRowKey rowKey, TColKey colKey, TVal value)
        {
            throw new NotSupportedException("Setting missing value doesn't make sense for this view. Use the [] notation.");
        }

        public override void SetValueOrMissing(int rowIndex, int colIndex, TVal value)
        {
            throw new NotSupportedException("Setting missing value doesn't make sense for this view. Use the [] notation.");
        }
    }
}
