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
using System.Text;
using Bio.Util;
using System.Linq;
using MBT.Escience.LogisticRegression;


namespace MBT.Escience
{
    [Serializable]
    public abstract class SufficientStatistics //: IComparable<SufficientStatistics>
    {
        public abstract bool IsMissing();

        public abstract StatisticsList AsStatisticsList();
        public abstract GaussianStatistics AsGaussianStatistics();
        public abstract ContinuousStatistics AsContinuousStatistics();
        public abstract DiscreteStatistics AsDiscreteStatistics();
        public abstract BooleanStatistics AsBooleanStatistics();
        public abstract MultinomialStatistics AsMultinomialStatistics();
        public abstract StringMapStatistics AsStringMapStatistics();

        public abstract bool Equals(SufficientStatistics stats);

        public static bool TryParse(string val, out SufficientStatistics result)
        {
            return
                MissingStatistics.TryParse(val, out result) ||
                BooleanStatistics.TryParse(val, out result) ||
                DiscreteStatistics.TryParse(val, out result) ||
                ContinuousStatistics.TryParse(val, out result) ||
                MultinomialStatistics.TryParse(val, out result) ||
                //StringMapStatistics.TryParse(val, out result) ||
                GaussianStatistics.TryParse(val, out result) ||
                StatisticsList.TryParse(val, out result);
        }

        public static SufficientStatistics Parse(string val)
        {
            SufficientStatistics result;
            if (TryParse(val, out result))
            {
                return result;
            }
            throw new ArgumentException(string.Format("Unable to parse \"{0}\" into an instance of ISufficientStatistics", val));
        }


        public override int GetHashCode()
        {
            throw new NotSupportedException("Derived class must override GetHashCode()");
        }
        public override bool Equals(object obj)
        {
            var stats = obj as SufficientStatistics;
            return stats != null && this.Equals(stats);
        }
    }

    [Serializable]
    public class MissingStatistics : SufficientStatistics
    {
        public const char MissingChar = '?';
        private MissingStatistics()
        {
        }

        static public MissingStatistics GetInstance()
        {
            return _singleton.Value;
        }
        static Lazy<MissingStatistics> _singleton = new Lazy<MissingStatistics>(() => new MissingStatistics());



        new public static bool TryParse(string val, out SufficientStatistics result)
        {
            result = null;
            if (
                string.IsNullOrEmpty(val) ||
                val.Equals("?") ||
                val.Equals("null", StringComparison.CurrentCultureIgnoreCase) ||
                val.Equals("missing", StringComparison.CurrentCultureIgnoreCase))
            {
                result = GetInstance();
            }
            return result != null;
        }

        public override bool IsMissing()
        {
            return true;
        }




        public override bool Equals(SufficientStatistics stats)
        {
            return stats.IsMissing();
        }

        public override bool Equals(object obj)
        {
            if (obj is SufficientStatistics)
            {
                return ((SufficientStatistics)obj).IsMissing();
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return "Missing".GetHashCode();
        }
        public override string ToString()
        {
            return MissingChar.ToString();
        }

        #region ISufficientStatistics Members

        public override StatisticsList AsStatisticsList()
        {
            return StatisticsList.GetMissingInstance();
        }

        public override GaussianStatistics AsGaussianStatistics()
        {
            return GaussianStatistics.GetMissingInstance();
        }

        public override ContinuousStatistics AsContinuousStatistics()
        {
            return ContinuousStatistics.GetMissingInstance();
        }

        public override DiscreteStatistics AsDiscreteStatistics()
        {
            return DiscreteStatistics.GetMissingInstance();
        }

        public override BooleanStatistics AsBooleanStatistics()
        {
            return BooleanStatistics.GetMissingInstance();
        }

        public override MultinomialStatistics AsMultinomialStatistics()
        {
            return MultinomialStatistics.GetMissingInstance();
        }

        public override StringMapStatistics AsStringMapStatistics()
        {
            return StringMapStatistics.GetMissingInstance();
        }

        #endregion

    }

    [Serializable]
    public class GaussianStatistics : SufficientStatistics
    {
        private readonly double _mean;
        private readonly double _variance;
        private readonly int _sampleSize;
        private readonly bool _isMissing;    // this way, the default constructor will set _isMissing to true.

        private GaussianStatistics()
        {
            _isMissing = true;
        }

        private GaussianStatistics(double mean, double var, int sampleSize)
        {
            _mean = mean;
            _variance = var;
            _sampleSize = sampleSize;
            _isMissing = false;
        }


        static public GaussianStatistics GetMissingInstance()
        {
            return new GaussianStatistics();
        }

        static public GaussianStatistics GetInstance(double mean, double variance, int sampleSize)
        {
            return new GaussianStatistics(mean, variance, sampleSize);
        }

        /// <summary>
        /// Get's the sufficient statistics of the population using <b>population</b> variance (as opposed to the unbiased sample variance).
        /// </summary>
        /// <param name="observations"></param>
        /// <returns></returns>
        public static GaussianStatistics GetInstance(IEnumerable<double> observations)
        {
            int n = 0;
            double sum = 0;
            foreach (double d in observations)
            {
                sum += d;
                n++;
            }
            double mean = sum / n;
            double variance = 0;
            foreach (double d in observations)
            {
                variance += (d - mean) * (d - mean);
            }
            variance /= n;
            return GetInstance(mean, variance, n);
        }

        public double Mean
        {
            get
            {
                Helper.CheckCondition(!IsMissing());
                return _mean;
            }
        }
        public double Variance
        {
            get
            {
                Helper.CheckCondition(!IsMissing());
                return _variance;
            }
        }
        public int SampleSize
        {
            get
            {
                Helper.CheckCondition(!IsMissing());
                return _sampleSize;
            }
        }
        public double SumOfSquares
        {
            get
            {
                Helper.CheckCondition(!IsMissing());
                return (_variance + _mean * _mean) * _sampleSize;
            }
        }

        new public static bool TryParse(string val, out SufficientStatistics result)
        {
            result = null;
            if (val.Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                result = GetMissingInstance();
                return false;
            }
            else
            {
                string[] fields = val.Split(',');
                if (!(fields.Length == 3))
                {
                    return false;
                }
                double mean, variance;
                int sampleSize;

                if (double.TryParse(fields[0], out mean) &&
                    double.TryParse(fields[1], out variance) &&
                    int.TryParse(fields[2], out sampleSize))
                {
                    result = GaussianStatistics.GetInstance(mean, variance, sampleSize);
                    return true;
                }

                return false;
            }
        }

        public override string ToString()
        {
            return IsMissing() ? "Missing" : string.Format("{0},{1},{2}", Mean, Variance, SampleSize);
            //return IsMissing() ? "Missing" : string.Format("m={0},v={1},ss={2}", Mean, Variance, SampleSize);
        }

        public override bool IsMissing()
        {
            return _isMissing;
        }

        //public static bool operator !=(ISufficientStatistics stats1, GaussianStatistics stats2)
        //{
        //    return stats2 != stats1;
        //}
        ////public static bool operator !=(GaussianStatistics stats1, ISufficientStatistics stats2)
        ////{
        ////    return !(stats1 == stats2);
        ////}
        ////////public static bool operator ==(ISufficientStatistics stats1, GaussianStatistics stats2)
        ////////{
        ////////    return stats2 == stats1;
        //////}
        ////public static bool operator ==(GaussianStatistics stats1, ISufficientStatistics stats2)
        ////{
        ////    throw new NotSupportedException();
        ////    object o1 = stats1 as Object;
        ////    object o2 = stats2 as Object;
        ////    if (o1 != null && o2 != null)
        ////    {
        ////        if (stats1.IsMissing() && stats2.IsMissing())
        ////            return true;
        ////        return stats1.Equals(stats2);
        ////    }
        ////    else
        ////    {
        ////        return o1 == o2;    // at this point, only equal if both are null.
        ////    }
        //}

        public override bool Equals(object obj)
        {
            if (obj is SufficientStatistics)
            {
                return Equals((SufficientStatistics)obj);
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(SufficientStatistics obj)
        {
            SufficientStatistics stats = (SufficientStatistics)obj;
            if (IsMissing() && stats.IsMissing())
            {
                return true;
            }
            GaussianStatistics gaussStats = stats.AsGaussianStatistics();

            return _mean == gaussStats._mean && _variance == gaussStats._variance && _sampleSize == gaussStats._sampleSize;
        }

        public override int GetHashCode()
        {
            return IsMissing() ? MissingStatistics.GetInstance().GetHashCode() : ToString().GetHashCode();
        }

        public static implicit operator GaussianStatistics(MissingStatistics missing)
        {
            return GaussianStatistics.GetMissingInstance();
        }


        #region ISufficientStatistics Members


        public override StatisticsList AsStatisticsList()
        {
            return IsMissing() ? StatisticsList.GetMissingInstance() : StatisticsList.GetInstance(this);
        }

        public override GaussianStatistics AsGaussianStatistics()
        {
            return this;
        }

        public override ContinuousStatistics AsContinuousStatistics()
        {
            return IsMissing() ? ContinuousStatistics.GetMissingInstance() : ContinuousStatistics.GetInstance(Mean);
        }

        public override DiscreteStatistics AsDiscreteStatistics()
        {
            return IsMissing() ? DiscreteStatistics.GetMissingInstance() : DiscreteStatistics.GetInstance((int)Mean);
        }

        public override BooleanStatistics AsBooleanStatistics()
        {
            int meanAsInt = (int)Mean;
            if (!IsMissing() && (meanAsInt < -1 || meanAsInt > 1))
                throw new InvalidCastException(string.Format("Cannot cast {0} to Boolean.", Mean));

            return IsMissing() || meanAsInt == -1 ? BooleanStatistics.GetMissingInstance() : BooleanStatistics.GetInstance(meanAsInt == 1);
        }

        public override MultinomialStatistics AsMultinomialStatistics()
        {
            throw new NotSupportedException();
        }

        public override StringMapStatistics AsStringMapStatistics()
        {
            return IsMissing() ? StringMapStatistics.GetMissingInstance() : StringMapStatistics.GetInstance(ToString(), (int)Mean);
        }
        #endregion

        public static GaussianStatistics Add(GaussianStatistics x, GaussianStatistics y)
        {
            int rN = x.SampleSize + y.SampleSize;
            double rMean = (x.SampleSize * x.Mean + y.SampleSize * y.Mean) / rN;
            double rVar = (x.SumOfSquares + y.SumOfSquares) / rN - rMean * rMean;
            if (rVar < 0)
            {
                Helper.CheckCondition(rVar > -1e-10, "Computed negative variance! " + rVar);
                rVar = 0;
            }
            GaussianStatistics result = GaussianStatistics.GetInstance(rMean, rVar, rN);
            return result;
        }

    }

    [Serializable]
    public class DiscreteStatistics : SufficientStatistics
    {
        private readonly int _value;
        private readonly bool _isMissing;
        //private readonly bool _isNotMissing;    // makes it so default instance is missing.

        public int Value
        {
            get
            {
                return _value;
                //return _class < 0 ? -1 : _class;
            }
        }

        private DiscreteStatistics()
        {
            _value = -1;
            _isMissing = true;
        }

        private DiscreteStatistics(int discreteClassification)
        {
            _value = discreteClassification;
            _isMissing = false;
            //_isNotMissing = discreteClassification >= 0;
        }


        public static DiscreteStatistics GetMissingInstance()
        {
            //return new DiscreteStatistics(-1);
            return new DiscreteStatistics();
        }

        public static DiscreteStatistics GetInstance(int discreteteClassification)
        {
            return new DiscreteStatistics(discreteteClassification);
        }

        new public static bool TryParse(string val, out SufficientStatistics result)
        {
            int valAsInt;
            if (int.TryParse(val, out valAsInt))
            {
                result = DiscreteStatistics.GetInstance(valAsInt);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool IsMissing()
        {
            //return Class < 0;
            //return !_isNotMissing;
            //return _value == null;
            return _isMissing;
        }

        public static implicit operator DiscreteStatistics(int classification)
        {
            return new DiscreteStatistics(classification);
        }

        public static implicit operator int(DiscreteStatistics stats)
        {
            return stats.Value;
        }

        public static implicit operator DiscreteStatistics(MissingStatistics missing)
        {
            return DiscreteStatistics.GetMissingInstance();
        }

        #region old == code. Replaced with SufficientStatistics' op overload.

        //public static bool operator !=(DiscreteStatistics stats1, DiscreteStatistics stats2)
        //{
        //    return !(stats1 == stats2);
        //}
        //public static bool operator ==(DiscreteStatistics discreteStatistics1, DiscreteStatistics discreteStatistics2)
        //{
        //    object o1 = discreteStatistics1 as Object;
        //    object o2 = discreteStatistics2 as Object;
        //    if (o1 != null && o2 != null)
        //        return discreteStatistics1.Class == discreteStatistics2.Class;
        //    else
        //        return o1 == o2;    // at this point, only equal if both are null.
        //}

        //public static bool operator !=(ISufficientStatistics stats1, DiscreteStatistics stats2)
        //{
        //    return stats2 != stats1;
        //}
        //public static bool operator !=(DiscreteStatistics stats1, ISufficientStatistics stats2)
        //{
        //    return !(stats1 == stats2);
        //}
        //////public static bool operator ==(ISufficientStatistics stats1, DiscreteStatistics stats2)
        //////{
        //////    return stats2 == stats1;
        //////}
        //public static bool operator ==(DiscreteStatistics stats1, ISufficientStatistics stats2)
        //{
        //    throw new NotSupportedException();
        //    object o1 = stats1 as Object;
        //    object o2 = stats2 as Object;
        //    if (o1 != null && o2 != null)
        //    {
        //        if (stats1.IsMissing() && stats2.IsMissing())
        //            return true;
        //        return stats1.Equals(stats2);
        //    }
        //    else
        //    {
        //        return o1 == o2;    // at this point, only equal if both are null.
        //    }
        //}
        #endregion

        public override bool Equals(object obj)
        {
            if (obj is SufficientStatistics)
            {
                return Equals((SufficientStatistics)obj);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(SufficientStatistics stats)
        {
            if (IsMissing() && stats.IsMissing())
            {
                return true;
            }
            else if (stats is DiscreteStatistics)
            {
                return this.Value == stats.AsDiscreteStatistics().Value;
            }
            else if (stats is BooleanStatistics)
            {
                return this.Value == stats.AsDiscreteStatistics().Value;
            }
            else
            {
                return stats.Equals(this);
            }
        }

        public override int GetHashCode()
        {
            return IsMissing() ? MissingStatistics.GetInstance().GetHashCode() : Value.GetHashCode();
        }
        public override string ToString()
        {
            return IsMissing() ? "Missing" : Value.ToString();
        }



        #region ISufficientStatistics Members

        public override StatisticsList AsStatisticsList()
        {
            return IsMissing() ? StatisticsList.GetMissingInstance() : StatisticsList.GetInstance(this);
        }

        public override GaussianStatistics AsGaussianStatistics()
        {
            return IsMissing() ? GaussianStatistics.GetMissingInstance() : GaussianStatistics.GetInstance(Value, 0, 1);
        }

        public override ContinuousStatistics AsContinuousStatistics()
        {
            return IsMissing() ? ContinuousStatistics.GetMissingInstance() : ContinuousStatistics.GetInstance(Value);
        }

        public override DiscreteStatistics AsDiscreteStatistics()
        {
            return this;
        }

        public override BooleanStatistics AsBooleanStatistics()
        {
            if (!IsMissing() && (Value < -1 || Value > 1))
            {
                throw new InvalidCastException(string.Format("Cannot convert {0} to Boolean", Value));
            }
            return IsMissing() || Value == -1 ? BooleanStatistics.GetMissingInstance() : BooleanStatistics.GetInstance(Value == 1);
        }
        public override MultinomialStatistics AsMultinomialStatistics()
        {
            return IsMissing() ? MultinomialStatistics.GetMissingInstance() : MultinomialStatistics.GetInstance(new Multinomial(Value + 1, Value));
        }

        public override StringMapStatistics AsStringMapStatistics()
        {
            return IsMissing() ? StringMapStatistics.GetMissingInstance() : StringMapStatistics.GetInstance("" + Value, Value);
        }

        #endregion
    }

    [Serializable]
    public class BooleanStatistics : SufficientStatistics
    {
        private static BooleanStatistics _missingSingleton = new BooleanStatistics(null);
        private static BooleanStatistics _falseSingleton = new BooleanStatistics(false);
        private static BooleanStatistics _trueSingleton = new BooleanStatistics(true);

        // Use integer mapping for consistent array indexing conventions.
        public const int Missing = -1;
        public const int False = 0;
        public const int True = 1;

        private readonly bool? _value;
        //public enum Classification
        //{
        //    Missing = BooleanStatistics.Missing,
        //    False = BooleanStatistics.False,
        //    True = BooleanStatistics.True
        //};

        //private readonly Classification _value;
        //private readonly bool _isMissing;
        //private bool _isNotMissing;

        //protected BooleanStatistics() : base(-1){}  // get missing instance
        //private BooleanStatistics() : this(Classification.Missing) { }// get missing instance

        private BooleanStatistics(bool? classification) //: this(classification ? Classification.True : Classification.False) { }
        {
            _value = classification;
        }
        //private BooleanStatistics(Classification classification)
        //{
        //    _value = classification;
        //    _isMissing = classification == Classification.Missing;
        //    //_isNotMissing = classification != Classification.Missing;
        //}

        public static BooleanStatistics GetMissingInstance()
        {
            return _missingSingleton;
        }

        public static BooleanStatistics GetInstance(bool classification)
        {
            return classification ? _trueSingleton : _falseSingleton;
        }

        public static BooleanStatistics GetInstance(bool? classification)
        {
            return classification == null ? _missingSingleton :
                classification.Value ? _trueSingleton : _falseSingleton;
        }

        public static Dictionary<string, BooleanStatistics> ConvertToBooleanStatistics(Dictionary<string, SufficientStatistics> dictionary)
        {
            Dictionary<string, BooleanStatistics> result = new Dictionary<string, BooleanStatistics>();

            foreach (KeyValuePair<string, SufficientStatistics> stringAndSuff in dictionary)
            {
                result.Add(stringAndSuff.Key, (BooleanStatistics)stringAndSuff.Value);
            }
            return result;
        }

        public override string ToString()
        {
            //return ((int)_value).ToString();
            return ValueAsInt().ToString();
            //return IsMissing() ? "Missing" : (Class > 0).ToString();
        }

        private int ValueAsInt()
        {
            return !_value.HasValue ? Missing : (bool)_value ? True : False;
        }


        public static implicit operator bool(BooleanStatistics stats)
        {
            return !stats.IsMissing() && stats._value.Value;
            //return !stats.IsMissing() && stats._value == Classification.True;
        }


        public static implicit operator BooleanStatistics(bool classification)
        {
            //return new BooleanStatistics(classification);
            return BooleanStatistics.GetInstance(classification);
        }

        public static explicit operator int(BooleanStatistics stats)
        {
            if (stats.IsMissing())
            {
                throw new InvalidCastException("Cannot cast missing statistics to a bool.");
            }
            //return stats.Class > 0;
            //return (int)stats._value;
            return stats.ValueAsInt();
        }

        public static explicit operator BooleanStatistics(DiscreteStatistics stats)
        {
            if (stats.IsMissing() || stats.Value < 0)
            {
                return GetMissingInstance();
            }
            //Classification value = (Classification)(int)stats;
            int i = (int)stats;
            Helper.CheckCondition<InvalidCastException>(i == 0 || i == 1, "Cannot cast {0} to BooleanStatistics.", stats);
            return BooleanStatistics.GetInstance(i == 1);
        }

        public static implicit operator DiscreteStatistics(BooleanStatistics stats)
        {
            return DiscreteStatistics.GetInstance(stats.ValueAsInt());
        }

        public static implicit operator BooleanStatistics(MissingStatistics missing)
        {
            return BooleanStatistics.GetMissingInstance();
        }

        public override bool Equals(object obj)
        {
            if (obj is SufficientStatistics)
            {
                return Equals((SufficientStatistics)obj);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(SufficientStatistics stats)
        {
            if (stats.IsMissing())
            {
                return IsMissing();
            }
            else if (stats is BooleanStatistics)
            {
                return _value == stats.AsBooleanStatistics()._value;
            }
            else
            {
                return stats.Equals(this);
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        public override bool IsMissing()
        {
            //return !_isNotMissing;
            //return _value == null;
            return _value == null;
        }

        new public static bool TryParse(string val, out SufficientStatistics result)
        {
            result = null;
            // optimize for the numerical cases, which are more common.
            if (val == "1")
            {
                result = _trueSingleton;
            }
            else if (val == "0")
            {
                result = _falseSingleton;
            }
            else if (val == "-1")
            {
                result = _missingSingleton;
            }
            else if (val.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                result = _trueSingleton;
            }
            else if (val.Equals("false", StringComparison.CurrentCultureIgnoreCase))
            {
                result = _falseSingleton;
            }
            else if (val.Equals("null", StringComparison.CurrentCultureIgnoreCase))
            {
                result = _missingSingleton;
            }
            return result != null;
        }



        #region ISufficientStatistics Members

        public override StatisticsList AsStatisticsList()
        {
            return IsMissing() ? StatisticsList.GetMissingInstance() : StatisticsList.GetInstance(this);
        }

        public override GaussianStatistics AsGaussianStatistics()
        {
            return IsMissing() ? GaussianStatistics.GetMissingInstance() : GaussianStatistics.GetInstance(ValueAsInt(), 0, 1);
        }

        public override ContinuousStatistics AsContinuousStatistics()
        {
            return IsMissing() ? ContinuousStatistics.GetMissingInstance() : ContinuousStatistics.GetInstance(ValueAsInt());
        }

        public override DiscreteStatistics AsDiscreteStatistics()
        {
            return IsMissing() ? DiscreteStatistics.GetMissingInstance() : DiscreteStatistics.GetInstance(ValueAsInt());
        }

        public override BooleanStatistics AsBooleanStatistics()
        {
            return this;
        }

        public override MultinomialStatistics AsMultinomialStatistics()
        {
            return IsMissing() ? MultinomialStatistics.GetMissingInstance() : MultinomialStatistics.GetInstance(new Multinomial(2, ValueAsInt()));
        }

        public override StringMapStatistics AsStringMapStatistics()
        {
            return IsMissing() ? StringMapStatistics.GetMissingInstance() : StringMapStatistics.GetInstance(_value.ToString(), ValueAsInt());
        }

        #endregion
    }

    [Serializable]
    public class ContinuousStatistics : SufficientStatistics
    {
        private readonly double _value;
        private readonly bool _isMissing;
        //private readonly bool _isNotMissing;

        public double Value
        {
            get
            {
                if (IsMissing()) throw new ArgumentException("Attempting to retrieve value from missing statistics");
                return _value;
            }
        }

        private ContinuousStatistics()
        {
            //_isNotMissing = true;
            //_value = null;
            _isMissing = true;
        }


        private ContinuousStatistics(double value)
        {
            _value = value;
            //_isNotMissing = true;
            _isMissing = false;
        }
        //private ContinuousStatistics() { }
        public static ContinuousStatistics GetMissingInstance()
        {
            return new ContinuousStatistics();
        }

        public static ContinuousStatistics GetInstance(double value)
        {
            return new ContinuousStatistics(value);
        }

        new public static bool TryParse(string val, out SufficientStatistics result)
        {
            double valAsDouble;
            if (double.TryParse(val, out valAsDouble))
            {
                result = ContinuousStatistics.GetInstance(valAsDouble);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool IsMissing()
        {
            //return !_isNotMissing;
            //return _value == null;
            return _isMissing;
        }

        public static implicit operator ContinuousStatistics(double value)
        {
            return new ContinuousStatistics(value);
        }

        public static implicit operator double(ContinuousStatistics stats)
        {
            return stats.Value;
        }

        public static implicit operator ContinuousStatistics(MissingStatistics missing)
        {
            return ContinuousStatistics.GetMissingInstance();
        }

        public static implicit operator ContinuousStatistics(DiscreteStatistics discreteStats)
        {
            return discreteStats.IsMissing() ? ContinuousStatistics.GetMissingInstance() : ContinuousStatistics.GetInstance(discreteStats.Value);
        }

        public static explicit operator DiscreteStatistics(ContinuousStatistics continuousStats)
        {
            return continuousStats.IsMissing() ? DiscreteStatistics.GetMissingInstance() : DiscreteStatistics.GetInstance((int)continuousStats.Value);
        }

        #region old == code. Replaced with SufficientStatistics' op overload.
        //public static bool operator !=(ISufficientStatistics stats1, ContinuousStatistics stats2)
        //{
        //    return stats2 != stats1;
        //}
        //public static bool operator !=(ContinuousStatistics stats1, ISufficientStatistics stats2)
        //{
        //    return !(stats1 == stats2);
        //}
        //////public static bool operator ==(ISufficientStatistics stats1, ContinuousStatistics stats2)
        //////{
        //////    return stats2 == stats1;
        //////}
        //public static bool operator ==(ContinuousStatistics stats1, ISufficientStatistics stats2)
        //{
        //    throw new NotSupportedException();
        //    object o1 = stats1 as Object;
        //    object o2 = stats2 as Object;
        //    if (o1 != null && o2 != null)
        //    {
        //        if (stats1.IsMissing() && stats2.IsMissing())
        //            return true;
        //        return stats1.Equals(stats2);
        //    }
        //    else
        //    {
        //        return o1 == o2;    // at this point, only equal if both are null.
        //    }
        //}
        #endregion

        public override bool Equals(object obj)
        {
            if (obj is SufficientStatistics)
            {
                return Equals((SufficientStatistics)obj);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(SufficientStatistics stats)
        {
            if (IsMissing() && stats.IsMissing())
            {
                return true;
            }
            return this.AsGaussianStatistics().Equals(stats);  // let the most general class decide
        }

        public override int GetHashCode()
        {
            return IsMissing() ? MissingStatistics.GetInstance().GetHashCode() : Value.GetHashCode();
        }
        public override string ToString()
        {
            return IsMissing() ? "Missing" : Value.ToString();
        }

        #region ISufficientStatistics Members

        public override StatisticsList AsStatisticsList()
        {
            return IsMissing() ? StatisticsList.GetMissingInstance() : StatisticsList.GetInstance(this);
        }

        public override GaussianStatistics AsGaussianStatistics()
        {
            return IsMissing() ? GaussianStatistics.GetMissingInstance() : GaussianStatistics.GetInstance(Value, 0, 1);
        }

        public override ContinuousStatistics AsContinuousStatistics()
        {
            return this;
        }

        public override DiscreteStatistics AsDiscreteStatistics()
        {
            return IsMissing() ? DiscreteStatistics.GetMissingInstance() : DiscreteStatistics.GetInstance((int)Value);
        }

        public override BooleanStatistics AsBooleanStatistics()
        {
            int valAsInt = (int)Value;
            if (!IsMissing() && !(valAsInt < -1 || valAsInt > 1))
            {
                throw new InvalidCastException(string.Format("Cannot convert {0} to Boolean.", Value));
            }
            return IsMissing() || valAsInt == -1 ? BooleanStatistics.GetMissingInstance() : BooleanStatistics.GetInstance(valAsInt == 1);
        }

        public override MultinomialStatistics AsMultinomialStatistics()
        {
            throw new NotSupportedException();
        }

        public override StringMapStatistics AsStringMapStatistics()
        {
            return IsMissing() ? StringMapStatistics.GetMissingInstance() : StringMapStatistics.GetInstance("" + Value, (int)Value);
        }

        #endregion
    }

    [Serializable]
    public class StringMapStatistics : SufficientStatistics
    {
        private readonly int _intVal;
        private readonly string _stringVal;
        private readonly bool _isMissing;

        public int AsInteger
        {
            get { return _intVal; }
        }
        public String AsString
        {
            get { return _stringVal; }
        }

        public StringMapStatistics()
        {
            _intVal = -1;
            _stringVal = null;
            _isMissing = true;
        }

        public StringMapStatistics(string name, int value)
        {
            _intVal = value;
            _stringVal = name;
            _isMissing = false;
        }

        public static StringMapStatistics GetMissingInstance()
        {
            return new StringMapStatistics();
        }

        public static StringMapStatistics GetInstance(string str, int val)
        {
            return new StringMapStatistics(str, val);
        }

        new public static bool TryParse(string val, out SufficientStatistics result)
        {
            result = null;
            if (val.Equals("null", StringComparison.InvariantCultureIgnoreCase) ||
                val.Equals("Missing", StringComparison.InvariantCultureIgnoreCase))
            {
                result = GetMissingInstance();
                return false;
            }
            else
            {
                string[] fields = val.Split(',');
                if (fields.Length != 2) return false;
                int intVal;

                if (int.TryParse(fields[1], out intVal))
                {
                    result = StringMapStatistics.GetInstance(fields[0], intVal);
                    return true;
                }
                return false;
            }
        }

        public override bool IsMissing()
        {
            return _isMissing;
        }

        //public static implicit operator DiscreteStatistics(int classification)
        //{
        //    return new DiscreteStatistics(classification);
        //}

        public static implicit operator int(StringMapStatistics stats)
        {
            return stats.AsInteger;
        }

        public static implicit operator StringMapStatistics(MissingStatistics missing)
        {
            return StringMapStatistics.GetMissingInstance();
        }

        public override bool Equals(object obj)
        {
            if (obj is SufficientStatistics)
            {
                return Equals((SufficientStatistics)obj);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(SufficientStatistics stats)
        {
            if (IsMissing() && stats.IsMissing())
            {
                return true;
            }
            else if (stats is DiscreteStatistics)
            {
                return this.AsInteger == stats.AsDiscreteStatistics().Value;
            }
            else if (stats is BooleanStatistics)
            {
                return this.AsInteger == stats.AsDiscreteStatistics().Value;
            }
            else if (stats is StringMapStatistics)
            {
                return this.AsInteger == stats.AsStringMapStatistics().AsInteger && this.AsString.Equals(stats.AsStringMapStatistics().AsString);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return IsMissing() ? MissingStatistics.GetInstance().GetHashCode() : AsString.GetHashCode();
        }
        public override string ToString()
        {
            return IsMissing() ? "Missing" : AsString + "," + AsInteger;
        }

        #region ISufficientStatistics Members

        public override StatisticsList AsStatisticsList()
        {
            return IsMissing() ? StatisticsList.GetMissingInstance() : StatisticsList.GetInstance(this);
        }

        public override GaussianStatistics AsGaussianStatistics()
        {
            return GaussianStatistics.GetMissingInstance();
            //return IsMissing() ? GaussianStatistics.GetMissingInstance() : GaussianStatistics.GetInstance(Value, 0, 1);
        }

        public override ContinuousStatistics AsContinuousStatistics()
        {
            return IsMissing() ? ContinuousStatistics.GetMissingInstance() : ContinuousStatistics.GetInstance(AsInteger);
        }

        public override DiscreteStatistics AsDiscreteStatistics()
        {
            return IsMissing() ? DiscreteStatistics.GetMissingInstance() : DiscreteStatistics.GetInstance(AsInteger); ;
        }

        public override BooleanStatistics AsBooleanStatistics()
        {
            if (!IsMissing() && (AsInteger < -1 || AsInteger > 1))
            {
                throw new InvalidCastException(string.Format("Cannot convert {0} to Boolean", AsInteger));
            }
            return IsMissing() || AsInteger == -1 ? BooleanStatistics.GetMissingInstance() : BooleanStatistics.GetInstance(AsInteger == 1);
        }

        public override MultinomialStatistics AsMultinomialStatistics()
        {
            throw new NotSupportedException();
        }

        public override StringMapStatistics AsStringMapStatistics()
        {
            return this;
        }

        #endregion

    }


    [Serializable]
    public class StatisticsList : SufficientStatistics, IEnumerable<SufficientStatistics>, ICloneable
    {
        public const char SEPARATOR = ';';
        private readonly List<SufficientStatistics> _stats;
        private bool _isMissing;
        private int? _hashCode;

        public SufficientStatistics this[int idx]
        {
            get
            {
                return _stats[idx];
            }
            set
            {
                _stats[idx] = value;
                _isMissing |= value.IsMissing();
                _hashCode = null;
            }
        }

        public int Count
        {
            get { return _stats.Count; }
        }

        private SufficientStatistics Last
        {
            get { return _stats[_stats.Count - 1]; }
        }

        public StatisticsList()
        {
            _stats = new List<SufficientStatistics>();
            _isMissing = true;
        }

        private StatisticsList(IEnumerable<SufficientStatistics> stats)
            : this()
        {
            foreach (SufficientStatistics stat in stats)
            {
                Add(stat);
            }
        }

        public static StatisticsList GetMissingInstance()
        {
            return StatisticsList.GetInstance(MissingStatistics.GetInstance());
        }

        public static StatisticsList GetInstance(params SufficientStatistics[] stats)
        {
            return new StatisticsList(stats);
        }

        public static StatisticsList GetInstance(IEnumerable<SufficientStatistics> stats)
        {
            return new StatisticsList(stats);
        }

        new public static bool TryParse(string val, out SufficientStatistics result)
        {
            result = null;
            string[] fields = val.Split(SEPARATOR);
            if (fields.Length < 2)
                return false;

            List<SufficientStatistics> stats = new List<SufficientStatistics>();
            foreach (string stat in fields)
            {
                stats.Add(SufficientStatistics.Parse(stat));
            }
            result = GetInstance(stats);
            return true;
        }

        public void Add(SufficientStatistics statsToAdd)
        {
            _isMissing = (Count == 0 ? statsToAdd.IsMissing() : _isMissing || statsToAdd.IsMissing());
            _hashCode = null;

            StatisticsList asList = statsToAdd as StatisticsList;
            if (asList != null)
            {
                _stats.AddRange(asList._stats);
            }
            else
            {
                _stats.Add(statsToAdd);
            }
        }

        public static StatisticsList Add(SufficientStatistics stats1, SufficientStatistics stats2)
        {
            StatisticsList newList = StatisticsList.GetInstance(stats1);
            newList.Add(stats2);
            return newList;
        }


        public override bool IsMissing()
        {
            return _isMissing;
        }

        public static implicit operator StatisticsList(MissingStatistics missing)
        {
            return StatisticsList.GetMissingInstance();
        }

        public override bool Equals(object obj)
        {
            if (obj is SufficientStatistics)
            {
                return Equals((SufficientStatistics)obj);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(SufficientStatistics stats)
        {
            if (IsMissing() && stats.IsMissing())
            {
                return true;
            }

            StatisticsList other = stats.AsStatisticsList();
            if (other._stats.Count != _stats.Count || GetHashCode() != other.GetHashCode())
            {
                return false;
            }

            for (int i = 0; i < _stats.Count; i++)
            {
                if (!_stats[i].Equals(other._stats[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                if (IsMissing())
                {
                    _hashCode = MissingStatistics.GetInstance().GetHashCode();
                }
                _hashCode = 0;
                foreach (SufficientStatistics stat in _stats)
                {
                    _hashCode ^= stat.GetHashCode();
                }
            }
            return (int)_hashCode;
        }

        public override string ToString()
        {
            if (Count == 0)
            {
                return "Missing";
            }

            // may still be missing, but enumerate all anyways. one will be missing, but we'll be able to recover the full set.
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;
            foreach (SufficientStatistics stat in _stats)
            {
                if (!isFirst)
                {
                    sb.Append(SEPARATOR);
                }
                sb.Append(stat.ToString());
                isFirst = false;
            }
            return sb.ToString();
        }

        #region ISufficientStatistics Members

        public override StatisticsList AsStatisticsList()
        {
            return this;
        }

        public override GaussianStatistics AsGaussianStatistics()
        {
            return IsMissing() ? GaussianStatistics.GetMissingInstance() : Last.AsGaussianStatistics();
        }

        public override ContinuousStatistics AsContinuousStatistics()
        {
            return IsMissing() ? ContinuousStatistics.GetMissingInstance() : Last.AsContinuousStatistics();
        }

        public override DiscreteStatistics AsDiscreteStatistics()
        {
            return IsMissing() ? DiscreteStatistics.GetMissingInstance() : Last.AsDiscreteStatistics();
        }

        public override BooleanStatistics AsBooleanStatistics()
        {
            return IsMissing() ? BooleanStatistics.GetMissingInstance() : Last.AsBooleanStatistics();
        }

        public override MultinomialStatistics AsMultinomialStatistics()
        {
            return IsMissing() ? MultinomialStatistics.GetMissingInstance() : Last.AsMultinomialStatistics();
        }

        public override StringMapStatistics AsStringMapStatistics()
        {
            return IsMissing() ? StringMapStatistics.GetMissingInstance() : Last.AsStringMapStatistics();
        }

        #endregion

        #region IEnumerable<SufficientStatistics> Members

        public IEnumerator<SufficientStatistics> GetEnumerator()
        {
            return _stats.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _stats.GetEnumerator();
        }

        #endregion


        public object Clone()
        {
            StatisticsList result = new StatisticsList(_stats);
            return result;
        }

        public SufficientStatistics Remove(int i)
        {
            SufficientStatistics stat = _stats[i];
            _stats.RemoveAt(i);
            if (stat.IsMissing())  // check to see if this was the reason we're missing
            {
                ResetIsMissing();
            }
            _hashCode = null;
            return stat;
        }

        private void ResetIsMissing()
        {
            _isMissing = false;
            foreach (SufficientStatistics ss in _stats)
            {
                _isMissing |= ss.IsMissing();
            }
        }

        public void RemoveRange(int startPos, int count)
        {
            _stats.RemoveRange(startPos, count);
            _hashCode = null;
            ResetIsMissing();
        }

        public SufficientStatistics SubSequence(int start, int count)
        {
            return count == 1 ? _stats[start] : new StatisticsList(_stats.SubSequence(start, count));
        }
    }

    public class MultinomialStatistics : SufficientStatistics
    {
        public Multinomial Multinomial { get; private set; }

        public double this[int pos]
        {
            get
            {
                return Multinomial[pos];
            }
        }

        private MultinomialStatistics(Multinomial m)
        {
            Multinomial = m;
        }

        private MultinomialStatistics(IEnumerable<double> distribution)
        {
            Multinomial = new Multinomial(distribution.ToList());
        }

        public static MultinomialStatistics GetInstance(Multinomial m)
        {
            return new MultinomialStatistics(m);
        }

        public static MultinomialStatistics GetInstance(IEnumerable<double> distribution)
        {
            return new MultinomialStatistics(distribution);
        }

        public static MultinomialStatistics GetMissingInstance()
        {
            return new MultinomialStatistics(new Multinomial());
        }

        new public static bool TryParse(string s, out SufficientStatistics stats)
        {
            stats = null;
            Multinomial m;
            if (!Multinomial.TryParse(s, out m)) return false;

            stats = new MultinomialStatistics(m);
            return true;
            //stats = null;

            //if (!s.StartsWith("[") && s.EndsWith("]"))
            //    return false;

            //string[] valsAsStrings = s.Substring(1, s.Length - 2).Split(',');

            //double[] values = new double[valsAsStrings.Length];

            //double sum = 0;
            //for (int i = 0; i < valsAsStrings.Length; i++)
            //{
            //    double d;
            //    if (double.TryParse(valsAsStrings[i], out d))
            //        values[i] = d;
            //    else
            //        return false;
            //    sum += d;
            //}

            //double tol = Math.Abs(1 - sum);
            //if (tol > 0.001) return false;
            //else if (tol > 0) values.Normalize();

            //stats = new MultinomialStatistics(values);
            //return true;
        }

        public override bool IsMissing()
        {
            return Multinomial.NumOutcomes == 0;
        }

        public override string ToString()
        {
            return Multinomial.ToString();
            //return "[" + Distribution.StringJoin(",") + "]";
        }

        public override bool Equals(SufficientStatistics stats)
        {
            var statsAsMulti = stats as MultinomialStatistics;
            return statsAsMulti != null && statsAsMulti.Multinomial.Equals(Multinomial);
        }

        public override int GetHashCode()
        {
            return Multinomial.GetHashCode();
        }

        public override StatisticsList AsStatisticsList()
        {
            throw new NotImplementedException();
        }

        public override GaussianStatistics AsGaussianStatistics()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If the multinomial has 2 states, returns the probability mass associated with state 1. else, throws a NotSupportedException.
        /// </summary>
        /// <returns></returns>
        public override ContinuousStatistics AsContinuousStatistics()
        {
            if (Multinomial.NumOutcomes == 2)
                return ContinuousStatistics.GetInstance(Multinomial[1]);
            else if (IsMissing())
                return ContinuousStatistics.GetMissingInstance();
            else
                throw new NotSupportedException("Can only cast multinomial to a continuous value if the multinomial has two states.");
        }

        public override DiscreteStatistics AsDiscreteStatistics()
        {
            if (Multinomial.IsDefinite) return DiscreteStatistics.GetInstance(Multinomial.BestOutcome);
            else throw new NotSupportedException("Multinomial is not definite.");
        }

        public override BooleanStatistics AsBooleanStatistics()
        {
            if (Multinomial.IsDefinite && Multinomial.BestOutcome <= 1) return BooleanStatistics.GetInstance(Multinomial.BestOutcome == 1);
            else throw new NotSupportedException("Multinomial is not definite 0/1.");
        }

        public override MultinomialStatistics AsMultinomialStatistics()
        {
            return this;
        }

        public override StringMapStatistics AsStringMapStatistics()
        {
            throw new NotImplementedException();
        }


    }

}
