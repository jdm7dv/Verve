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
using MBT.Escience;
using Bio.Util;
using System.Xml.Serialization;

namespace MBT.Escience.Optimization
{
    [Serializable]
    public class OptimizationParameter
    {
        static public OptimizationParameter GetProbabilityInstance(string name, double value, bool doSearch)
        {
            OptimizationParameter parameter = new OptimizationParameter();

            //parameter.TransformForSearch = SpecialFunctions.LogOdds;
            //parameter.TransformFromSearch = SpecialFunctions.Probability;
            //parameter.Low = .001;
            //parameter.High = .999;
            //parameter.LowForSearch = parameter.TransformForSearch(parameter.Low);
            //parameter.HighForSearch = parameter.TransformForSearch(parameter.High); 

            parameter.Name = name;
            parameter.Value = value;
            parameter.DoSearch = doSearch;

            parameter.ConvertToProbabilityInstance();

            return parameter;

        }

        static public OptimizationParameter GetPositiveFactorInstance(string name, double value, bool doSearch)
        {
            OptimizationParameter parameter = new OptimizationParameter();

            //parameter.TransformForSearch = delegate(double r) { return Math.Log(r); };
            //parameter.TransformFromSearch = delegate(double r) { return Math.Exp(r); };
            //parameter.Low = 1.0 / 1000.0;
            //parameter.High = 1000;
            //parameter.LowForSearch = parameter.TransformForSearch(parameter.Low);
            //parameter.HighForSearch = parameter.TransformForSearch(parameter.High);

            parameter.Name = name;
            parameter.Value = value;
            parameter.DoSearch = doSearch;
            parameter.ConvertToPositiveFactorInstance();

            return parameter;

        }

        /// <summary>
        /// Return an positive factor instance with given low and high bounds. Author: Henry Lin.
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Initial value</param>
        /// <param name="doSearch">Whether search on this parameter or not</param>
        /// <returns>An instance whose search is on the log space</returns>
        static public OptimizationParameter GetPositiveFactorInstance(string name, double value, bool doSearch, double low, double high)
        {
            OptimizationParameter parameter = GetPositiveFactorInstance(name, value, doSearch);
            parameter.Low = low; parameter.High = high;
            return parameter;
        }

        static public OptimizationParameter GetUntransformedInstance(string name, double value, bool doSearch, double low, double high)
        {
            OptimizationParameter parameter = new OptimizationParameter();

            parameter.Name = name;
            parameter.Value = value;
            parameter.DoSearch = doSearch;
            parameter.ConvertToUntransformedInstance(low, high);

            return parameter;

        }


        static public OptimizationParameter GetTanInstance(string name, double value, bool doSearch, double low, double high)
        {
            OptimizationParameter parameter = new OptimizationParameter();

            parameter.Name = name;
            parameter.Value = value;
            parameter.DoSearch = doSearch;
            //parameter.LowForSearch = parameter.TransformForSearch(parameter.Low);
            //parameter.HighForSearch = parameter.TransformForSearch(parameter.High);
            parameter.ConvertToTanInstance(low, high);

            return parameter;

        }


        public OptimizationParameter()
        {
            TransformForSearch = Identity;
            TransformFromSearch = Identity;
        }

        public string Name;
        public bool DoSearch;
        private Converter<double, double> TransformForSearch;
        [XmlIgnore]
        public Converter<double, double> TransformFromSearch;
        private double _value;
        //private double _valueForSearch;
        private double _low;
        private double _high;
        //private double _lowForSearch;
        //private double _highForSearch;      

        private T Identity<T>(T example)
        {
            return example;
        }

        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                //_valueForSearch = TransformForSearch(value);
            }
        }

        internal double ValueForSearch
        {
            get
            {
                //return _valueForSearch;
                return TransformForSearch(Value);
            }
            set
            {
                //_valueForSearch = value;
                _value = TransformFromSearch(value);
            }
        }

        [XmlIgnore]
        public double Low
        {
            get
            {
                return _low;
            }
            private set
            {
                _low = value;
                //_lowForSearch = TransformForSearch(value);
            }
        }

        internal double LowForSearch
        {
            get
            {
                //return _lowForSearch;
                return TransformForSearch(Low);
            }
            private set
            {
                //_lowForSearch = value;
                _low = TransformFromSearch(value);
            }
        }

        [XmlIgnore]
        public double High
        {
            get
            {
                return _high;
            }
            private set
            {
                _high = value;
                //_highForSearch = TransformForSearch(value);
            }
        }

        internal double HighForSearch
        {
            get
            {
                //return _highForSearch;
                return TransformForSearch(High);
            }
            private set
            {
                //_highForSearch = value;
                _high = TransformFromSearch(value);
            }
        }

        public void ConvertToProbabilityInstance()
        {
            TransformForSearch = SpecialFunctions.LogOdds;
            TransformFromSearch = SpecialFunctions.InverseLogOdds;
            Low = .00001;
            High = .99999;
            //Value = Math.Max(Low, Math.Min(High, Value));
        }

        public void ConvertToPositiveFactorInstance()
        {
            TransformForSearch = delegate(double r) { return Math.Log(r); };
            TransformFromSearch = delegate(double r) { return Math.Exp(r); };
            Low = 1.0 / 1000.0;
            High = 10000;
            //Value = Math.Max(Low, Math.Min(High, Value));
        }

        public void ConvertToTanInstance(double low, double high)
        {
            TransformForSearch = delegate(double r) { return Math.Atan(r) * 2 / Math.PI; };
            TransformFromSearch = delegate(double r) { return Math.Tan(r * Math.PI / 2); };
            Low = low;
            High = high;
        }

        public void ConvertToUntransformedInstance(double low, double high)
        {
            TransformForSearch = (x => x); // the 'do nothing' transformation
            TransformFromSearch = (x => x); // the 'do nothing' transformation
            Low = low;
            High = high;
        }


        public OptimizationParameter Clone()
        {
            OptimizationParameter parameter = new OptimizationParameter();

            parameter.TransformForSearch = TransformForSearch;
            parameter.TransformFromSearch = TransformFromSearch;
            parameter.Name = Name;
            parameter.Value = Value;
            parameter.DoSearch = DoSearch;
            parameter.Low = Low;
            parameter.High = High;

            return parameter;
        }

        public override string ToString()
        {
            return Name + "=" + Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is OptimizationParameter)
                return Equals((OptimizationParameter)obj);
            else
                return false;
        }

        /// <summary>
        /// Checks equals based off having identical fields for Name, DoSearch, Value, High, Low and the corresponding values under transformation.
        /// Name check is case sensitive.
        /// </summary>
        public bool Equals(OptimizationParameter param)
        {
            return this.Name.Equals(param.Name) && this.Value == param.Value && this.High == param.High && this.Low == param.Low && this.DoSearch == param.DoSearch &&
                this.HighForSearch == param.HighForSearch && this.LowForSearch == param.LowForSearch && this.ValueForSearch == param.ValueForSearch;    // not guaranteed to check that transformation is the same, but awefully close.
        }


        public override int GetHashCode()
        {
            int hashCode = ToString().GetHashCode();
            return hashCode;
        }
    }

}
