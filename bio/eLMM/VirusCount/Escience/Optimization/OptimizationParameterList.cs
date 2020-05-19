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
using Bio.Util;
using MBT.Escience;
using System.Xml.Serialization;

namespace MBT.Escience.Optimization
{
    [Serializable]
    public class OptimizationParameterList : IEnumerable<OptimizationParameter>, IXmlSerializable
    {
        private Dictionary<string, OptimizationParameter> _asDictionary = new Dictionary<string, OptimizationParameter>(StringComparer.CurrentCultureIgnoreCase);
        private List<OptimizationParameter> _asParameterList = new List<OptimizationParameter>();
        private List<int> _enumerationOrder;
        private Predicate<OptimizationParameterList> CheckConditions = null;

        public OptimizationParameterList()
        {
        }

        public OptimizationParameter this[string name]
        {
            get
            {
                return _asDictionary[name];
            }
        }

        public OptimizationParameter this[int index]
        {
            get
            {
                return _asParameterList[index];
            }
        }


        public IEnumerable<string> Names
        {
            get
            {
                foreach (OptimizationParameter param in this)
                {
                    yield return param.Name;
                }
            }
        }

        public IEnumerable<double> Values
        {
            get
            {
                foreach (OptimizationParameter param in this)
                {
                    yield return param.Value;
                }
            }
        }

        public int Count
        {
            get
            {
                return _asParameterList.Count;
            }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OptimizationParameter[]));
            reader.ReadStartElement("Parameters");
            reader.ReadStartElement("AsParameterList");
            OptimizationParameter[] asParameterList = (OptimizationParameter[])serializer.Deserialize(reader);
            OptimizationParameterList list = OptimizationParameterList.GetInstance(asParameterList);
            reader.ReadEndElement();
            reader.ReadStartElement("EnumerationOrder");
            serializer = new XmlSerializer(typeof(int[]));
            int[] enumerationOrder = (int[])serializer.Deserialize(reader);
            reader.ReadEndElement();
            reader.ReadEndElement();
            list.SetEnumerationOrder(enumerationOrder);
            this._asDictionary = list._asDictionary;
            this._asParameterList = list._asParameterList;
            this._asDictionary = list._asDictionary;
            this.SetEnumerationOrder(enumerationOrder);
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OptimizationParameter[]));
            writer.WriteStartElement("AsParameterList");
            serializer.Serialize(writer, _asParameterList.ToArray());
            writer.WriteEndElement();
            writer.WriteStartElement("EnumerationOrder");
            serializer = new XmlSerializer(typeof(int[]));
            serializer.Serialize(writer, _enumerationOrder.ToArray());
            writer.WriteEndElement();
        } 


        public static OptimizationParameterList GetInstance(params OptimizationParameter[] parameterCollection)
        {
            return GetInstance2(parameterCollection);
        }


        public static OptimizationParameterList GetInstance2(IEnumerable<OptimizationParameter> parameterCollection)
        {
            OptimizationParameterList aParamsList = new OptimizationParameterList();
            parameterCollection.ForEach(p => aParamsList.Add(p, false));
            //foreach (OptimizationParameter parameter in parameterCollection)
            //{
            //    aParamsList._asDictionary.Add(parameter.Name, parameter);
            //    aParamsList._asParameterList.Add(parameter);
            //}

            // no longer requires sort.
            ////Must do this after because the parameters may get re-ordered.
            //aQmrrParams._asParameterArray = new OptimizationParameter[aQmrrParams._asSortedDictionary.Count];
            //int iParam = -1;
            //foreach (OptimizationParameter parameter in aQmrrParams._asSortedDictionary.Values)
            //{
            //    ++iParam;
            //    aQmrrParams._asParameterArray[iParam] = parameter;
            //}

            return aParamsList;
        }

        public void SetCheckConditions(Predicate<OptimizationParameterList> checkConditionsFunction)
        {
            CheckConditions = checkConditionsFunction;
        }

        /// <summary>
        /// Returns true iff the current settings of this parameter list satisfies the conditions given by CheckConditions.
        /// </summary>
        public bool SatisfiesConditions()
        {
            return CheckConditions == null || CheckConditions(this);
        }

        public bool DoSearch(int parameterIndex)
        {
            return _asParameterList[parameterIndex].DoSearch;
        }

        public override string ToString()
        {
            return this.StringJoin("\t");
            //StringBuilder sb = new StringBuilder();
            //foreach (OptimizationParameter param in this)
            //{
            //    if (sb.Length > 0)
            //        sb.Append("\t");
            //    sb.Append(param.Value);
            //    //if (param.Low >= 0 && param.High > 1)
            //    //    sb.Append("\t" + Math.Log(param.Value));
            //}
            //return sb.ToString();
        }

        public string ToValueString()
        {
            return this.Select(p => p.Value).StringJoin("\t");
        }

        public int CountFreeParameters()
        {
            int i = 0;
            foreach (OptimizationParameter param in _asParameterList)
            {
                if (param.DoSearch)
                {
                    i++;
                }
            }
            return i;
        }

        public string ToStringHeader()
        {
            StringBuilder sb = new StringBuilder();
            foreach (OptimizationParameter param in this)
            {
                if (sb.Length > 0)
                    sb.Append("\t");
                sb.Append(param.Name);
                //if (param.Low >= 0 && param.High > 1)
                //    sb.Append("\tLog" + param.Name);
            }
            return sb.ToString();
        }

        public bool IsClose(OptimizationParameterList other, double eps)
        {
            if (other == null)
            {
                return false;
            }

            Helper.CheckCondition(other.Count == Count);

            for (int iParam = 0; iParam < Count; ++iParam)
            {
                if (!IsClose(_asParameterList[iParam], other._asParameterList[iParam], eps))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsClose(OptimizationParameter myValue, OptimizationParameter otherValue, double eps)
        {
            return Math.Abs(myValue.Value - otherValue.Value) < eps;
        }


        public OptimizationParameterList Clone()
        {
            List<OptimizationParameter> parameterCollection = new List<OptimizationParameter>();
            foreach (OptimizationParameter parameter in _asParameterList)
            {
                parameterCollection.Add(parameter.Clone());
            }
            OptimizationParameterList result = GetInstance2(parameterCollection);
            if (_enumerationOrder != null)
            {
                result.SetEnumerationOrder(_enumerationOrder);
            }
            return result;
        }

        public List<double> ExtractParameterValueListForSearch()
        {
            List<double> parameterValueListForSearch = new List<double>(Count);
            foreach (OptimizationParameter param in this)
            {
                parameterValueListForSearch.Add(param.ValueForSearch);
            }
            return parameterValueListForSearch;
        }
        public List<double> ExtractParameterLowListForSearch()
        {
            List<double> parameterLowListForSearch = new List<double>(Count);
            foreach (OptimizationParameter param in this)
            {
                parameterLowListForSearch.Add(param.LowForSearch);
            }
            return parameterLowListForSearch;
        }
        public List<double> ExtractParameterHighListForSearch()
        {
            List<double> parameterHighListForSearch = new List<double>(Count);
            foreach (OptimizationParameter param in this)
            {
                parameterHighListForSearch.Add(param.HighForSearch);
            }
            return parameterHighListForSearch;
        }

        public OptimizationParameterList CloneWithNewValuesForSearch(List<double> point)
        {
            List<OptimizationParameter> parameterCollection = new List<OptimizationParameter>();
            for (int iParam = 0; iParam < Count; ++iParam)
            {
                OptimizationParameter clone = _asParameterList[iParam].Clone();
                clone.ValueForSearch = point[iParam];
                parameterCollection.Add(clone);
            }
            return GetInstance2(parameterCollection);
        }

        #region IEnumerable<OptimizationParameter> Members

        public IEnumerator<OptimizationParameter> GetEnumerator()
        {
            if (_enumerationOrder == null)
            {
                foreach (OptimizationParameter param in _asParameterList)
                {
                    yield return param;
                }
            }
            else
            {
                foreach (int index in _enumerationOrder)
                {
                    yield return _asParameterList[index];
                }
            }
        }

        #endregion

        public IEnumerable<OptimizationParameter> InNativeOrder()
        {
            foreach (OptimizationParameter p in _asParameterList)
                yield return p;
        }

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            if (_enumerationOrder == null)
            {
                foreach (OptimizationParameter param in _asParameterList)
                {
                    yield return param;
                }
            }
            else
            {
                foreach (int index in _enumerationOrder)
                {
                    yield return _asParameterList[index];
                }
            }
        }

        #endregion

        /// <summary>
        /// Inserts optimizedParameter at the specified index.
        /// </summary>
        public void Add(int idx, OptimizationParameter optimizationParameter)
        {
            while (_asParameterList.Count <= idx)
                _asParameterList.Add(null);
            _asParameterList[idx] = optimizationParameter;
            _asDictionary.Add(optimizationParameter.Name, optimizationParameter);
        }

        public void Add(OptimizationParameter optimizationParameter)
        {
            Add(optimizationParameter, false);
        }

        public void Add(OptimizationParameter optimizationParameter, bool enumerateFirstNotLast)
        {
            //Helper.CheckCondition(_enumerationOrder != null || !enumerateFirstNotLast, "Can't enumerate first if there's no specified enumeration order.");
            Add(Count, optimizationParameter);

            if (_enumerationOrder != null)
            {
                if (enumerateFirstNotLast)
                    _enumerationOrder.Insert(0, Count - 1);
                else
                    _enumerationOrder.Add(Count - 1);
            }
        }

        public void Insert(int idx, OptimizationParameter parameter)
        {
            _asParameterList.Insert(idx, parameter);
            _asDictionary.Add(parameter.Name, parameter);

            if (_enumerationOrder != null)
            {
                for (int i = 0; i < _enumerationOrder.Count; i++)
                {
                    if (_enumerationOrder[i] >= idx)
                        _enumerationOrder[i]++;
                }
                _enumerationOrder.Insert(idx, idx);
            }
        }

        public void SetEnumerationOrder(params int[] searchOrder)
        {
            SetEnumerationOrder((IEnumerable<int>)searchOrder);
        }

        public void SetEnumerationOrder(IEnumerable<int> searchOrder)
        {
            _enumerationOrder = new List<int>(_asParameterList.Count);
            foreach (int i in searchOrder)
            {
                Helper.CheckCondition(i >= 0 && i < _asParameterList.Count);
                _enumerationOrder.Add(i);
            }
            //Helper.CheckCondition(_enumerationOrder.Count == this.Count);
        }

        public void ReverseEnumerationOrder()
        {
            if (_enumerationOrder != null)
                _enumerationOrder.Reverse();
            else
            {
                _enumerationOrder = new List<int>(_asParameterList.Count);
                for (int i = _asParameterList.Count - 1; i >= 0; i--)
                    _enumerationOrder.Add(i);
            }
        }

        public bool ContainsKey(string parameterName)
        {
            return _asDictionary.ContainsKey(parameterName);
        }

        /// <summary>
        /// Sets the values of the this ParameterList's parameters to the given values.
        /// The values must be in the same order as returned by the indexer.
        /// </summary>
        /// <param name="parameters"></param>
        public void SetParameterValues(double[] parameters)
        {
            Helper.CheckCondition(parameters.Length == _asParameterList.Count, "Wrong number of parameters.");

            for (int i = 0; i < parameters.Length; i++)
            {
                this[i].Value = parameters[i];
            }
        }

        //public void ChangeParameterName(string oldName, string newName)
        //{
        //    OptimizationParameter param = _asDictionary[oldName];
        //    param.Name = newName;
        //    _asDictionary.Remove(oldName);
        //    _asDictionary.Add(newName, param);
        //}



        public void Remove(int i)
        {
            OptimizationParameter paramToRemove = _asParameterList[i];
            _asParameterList.RemoveAt(i);
            _asDictionary.Remove(paramToRemove.Name);
            //_enumerationOrder.Remove(i);
            _enumerationOrder = _enumerationOrder.Where(idx => idx != i).Select(idx => idx < i ? idx : idx - 1).ToList();
        }

        /// <summary>
        /// Removes the given parameter.
        /// </summary>
        /// <param name="paramName">The name of the parameter to remove. Case Insensitive.</param>
        public void Remove(string paramName)
        {
            int idx = -1;
            for (int i = 0; i < _asParameterList.Count; i++)
            {
                if (_asParameterList[i].Name.Equals(paramName, StringComparison.CurrentCultureIgnoreCase))
                {
                    idx = i;
                    break;
                }
            }

            if (idx >= 0)
                Remove(idx);
        }

        /// <summary>
        /// Renames the given parameter.
        /// </summary>
        /// <param name="oldName">Old name is case insensitive.</param>
        public void RenameParameter(string oldName, string newName)
        {
            OptimizationParameter param;
            if (_asDictionary.TryGetValue(oldName, out param))
            {
                param.Name = newName;
                _asDictionary.Remove(oldName);
                _asDictionary.Add(newName, param);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is OptimizationParameterList)
                return Equals((OptimizationParameterList)obj);
            else
                return false;
        }

        /// <summary>
        /// Checks that the enumeration order is the same and that each parameter is equal according to OptimizationParameter.Equals(OptimizationParameter).
        /// </summary>
        public bool Equals(OptimizationParameterList list, bool ignoreEnumerationOrder = false)
        {
            if (list == null)
                return false;
            if (Count != list.Count)
                return false;
            if (!ignoreEnumerationOrder)
            {
                if ((this._enumerationOrder == null) != (list._enumerationOrder == null))
                    return false;
                if (!_enumerationOrder.SequenceEqual(list._enumerationOrder))
                    return false;
            }
            for (int i = 0; i < _asParameterList.Count; i++)
                if (!_asParameterList[i].Equals(list._asParameterList[i]))
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = ToString().GetHashCode();
            return hashCode;
        }

        public void EnumerateInReverse()
        {
            SetEnumerationOrder(Enumerable.Range(0, Count).Reverse());
        }


    }
}
