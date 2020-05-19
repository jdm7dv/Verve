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

namespace MBT.Escience.Matrix
{
    /// <summary>
    /// This class wraps a matrix of multistate values in a view that makes it look binary.
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
    ///             new KeyValuePair&lt;string, IEnumerable&lt;string&gt;&gt;(merAndPos.Value + "@" + merAndPos.Key,
    ///             merAndPos.Key.Split('#').Select(aa => (int)merAndPos.Value + "@" + aa)));
    /// </code>
    /// Note this view is completely generic. This makes for some difficulty with the TVal, as binary and multistate suggest
    /// 0/1 and integer encoding, respectfully. To allow TVal to be fully generic, we therefore require the caller to provide
    /// delegates mapping from "state" (ie, 0,1,2...) to TVal, and vice versa, which are specified using a ValueConverter from TVal
    /// to int.
    /// Note also that any row for which there is a one-to-one mapping between binary and multistate values (e.g. "B57") will have
    /// identical values for both the binary view and the multistate matrix. That is, the underlying value is simply passed on.
    /// </remarks>
    public class MultistateToBinaryView<TRowKey, TColKey, TVal> : Matrix<TRowKey, TColKey, TVal>
    {
        private Matrix<TRowKey, TColKey, TVal> _multistateMatrix;
        private Dictionary<TRowKey, KeyValuePair<TRowKey, int>> _binaryKeyToMultKeyAndState;
        private ValueConverter<TVal, int> _cnvrtTValToState;
        //private Converter<int, TVal> _fnStateToTVal;
        //private Converter<TVal, int> _fnTValToState;


        /// <summary>
        /// Creates a this wrapper.
        /// </summary>
        /// <param name="multistateMatrixToWrap"></param>
        /// <param name="multistateKeyToOrderedBinaryKeys">First key is multistate name (e.g. 1@A#B#C), the value is an enumeration of binary keys that map to the multistate keys.
        /// The order specifies the state value in multistate space. (e.g. 1@A is state 0, 1@B is state 2, 1@C is state 2)</param>
        /// <param name="convertTValToInt">Specifies how to convert from TVal to 0,1,2,3... and back. 0/1 are used for binary, 0..N are assumed to be used by the multistate matrix.</param>
        public MultistateToBinaryView(
            Matrix<TRowKey, TColKey, TVal> multistateMatrixToWrap,
            IEnumerable<KeyValuePair<TRowKey, IEnumerable<TRowKey>>> multistateKeyToOrderedBinaryKeys,
            ValueConverter<TVal, int> convertTValToInt)
        //Converter<int, TVal> fnStateToTVal,
        //Converter<TVal, int> fnTValToBinaryState)
        {
            _multistateMatrix = multistateMatrixToWrap;
            _binaryKeyToMultKeyAndState = multistateKeyToOrderedBinaryKeys.SelectMany(mAndB =>
                mAndB.Value.Select((b, idx) =>
                    new KeyValuePair<TRowKey, KeyValuePair<TRowKey, int>>(b, new KeyValuePair<TRowKey, int>(mAndB.Key, idx)))).ToDictionary();
            _cnvrtTValToState = convertTValToInt;
            //_fnStateToTVal = fnStateToTVal;
            //_fnTValToState = fnTValToBinaryState;

            _rowKeys = new ReadOnlyCollection<TRowKey>(multistateKeyToOrderedBinaryKeys.SelectMany(pair => pair.Value).ToList());
            _indexOfRowKey = new RestrictedAccessDictionary<TRowKey, int>(_rowKeys.Select((key, idx) => new KeyValuePair<TRowKey, int>(key, idx)).ToDictionary());
        }

        private ReadOnlyCollection<TRowKey> _rowKeys;
        public override ReadOnlyCollection<TRowKey> RowKeys
        {
            get { return _rowKeys; }
        }

        public override ReadOnlyCollection<TColKey> ColKeys
        {
            get { return _multistateMatrix.ColKeys; }
        }

        private RestrictedAccessDictionary<TRowKey, int> _indexOfRowKey;
        public override IDictionary<TRowKey, int> IndexOfRowKey
        {
            get { return _indexOfRowKey; }
        }

        public override IDictionary<TColKey, int> IndexOfColKey
        {
            get { return _multistateMatrix.IndexOfColKey; }
        }

        public override TVal MissingValue
        {
            get { return _multistateMatrix.MissingValue; }
        }

        public override bool TryGetValue(TRowKey rowKey, TColKey colKey, out TVal value)
        {

            KeyValuePair<TRowKey, int> multKeyAndState;
            TVal actualMultState;
            if (_binaryKeyToMultKeyAndState.TryGetValue(rowKey, out multKeyAndState) &&
                _multistateMatrix.TryGetValue(multKeyAndState.Key, colKey, out actualMultState))
            {
                if (rowKey.Equals(multKeyAndState.Key))
                    value = actualMultState;
                else
                {
                    int state = _cnvrtTValToState.ConvertForward(actualMultState);

                    bool thisIsTheState = state == multKeyAndState.Value;

                    value = thisIsTheState ? _cnvrtTValToState.ConvertBackward(1) : _cnvrtTValToState.ConvertBackward(0);
                }
                return true;
            }

            value = MissingValue;

            return false;   //must be missing if we get to here.
        }

        public override bool TryGetValue(int rowIndex, int colIndex, out TVal value)
        {
            return TryGetValue(RowKeys[rowIndex], ColKeys[colIndex], out value);
        }

        /// <summary>
        /// Removing one binary key removes the entire multistate key, and hence all the sister binary keys.
        /// </summary>
        public override bool Remove(TRowKey rowKey, TColKey colKey)
        {
            KeyValuePair<TRowKey, int> multKeyAndState;
            if (_binaryKeyToMultKeyAndState.TryGetValue(rowKey, out multKeyAndState))
                return _multistateMatrix.Remove(multKeyAndState.Key, colKey);
            else
                return false;
        }

        public override bool Remove(int rowIndex, int colIndex)
        {
            return Remove(RowKeys[rowIndex], ColKeys[colIndex]);
        }

        /// <summary>
        /// Gets or sets the value of the binary row key. Note that when setting a value to 1 means setting the multistate state to point to that
        /// rowKey's state. Setting a state to 0 is problematic, as you can lose the one-hot encoding. If the value is already 0, then there 
        /// is nothing to do and the method returns silently. If, however, the binary node was already 1, then this will throw an exception, 
        /// as the result is an undefined multistate.  
        /// </summary>
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
                    throw new ArgumentException("Can't assign missing value using this[row,col]. Use SetValueOrMissing(row, col, value) or Remove(row,vol)");

                KeyValuePair<TRowKey, int> multKeyAndState;
                if (!_binaryKeyToMultKeyAndState.TryGetValue(rowKey, out multKeyAndState))
                    throw new ArgumentException("don't know row key " + rowKey);

                if (rowKey.Equals(multKeyAndState.Key))
                    _multistateMatrix[rowKey, colKey] = value;
                else
                {

                    int state = _cnvrtTValToState.ConvertForward(value);
                    TVal multStateValForThisKey = _cnvrtTValToState.ConvertBackward(multKeyAndState.Value);

                    if (state == 1) // set it so the multistate value is this state
                    {
                        _multistateMatrix[multKeyAndState.Key, colKey] = multStateValForThisKey;
                    }
                    else if (_multistateMatrix[multKeyAndState.Key, colKey].Equals(multStateValForThisKey))
                    {
                        throw new ArgumentException(string.Format("Setting binary value {0} to 0 will result in all binary nodes of multistate node {1} being zero. This is an undefined state.",
                            rowKey, multKeyAndState.Key));
                    }
                    else
                    {
                        // do nothing. The state is already 0.
                    }
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

        public override void SetValueOrMissing(TRowKey rowKey, TColKey colKey, TVal value)
        {
            throw new NotImplementedException();
        }

        public override void SetValueOrMissing(int rowIndex, int colIndex, TVal value)
        {
            throw new NotImplementedException();
        }
    }
}
