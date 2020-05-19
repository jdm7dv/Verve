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
using MBT.Escience;
using Bio.Util;

namespace MBT.Escience.LogisticRegression
{
    /// <summary>
    /// A class for representing multinomial distributions
    /// </summary>
    [Serializable]
    public struct Multinomial
    {
        readonly ushort _correct;
        readonly ushort _numOutcomes;
        private double[] _probs;

        /// <summary>
        /// Returns whether the multinomial has all mass on a single outcome
        /// </summary>
        public readonly bool IsDefinite;

        public static bool TryParse(string s, out Multinomial m)
        {
            m = new Multinomial();
            double p;
            int state;
            if (s[0] == '[' && s[s.Length - 1] == ']')
            {
                string[] fields = s.Substring(1, s.Length - 2).Split(',');
                if (fields.Length < 2) return false;
                double[] probs = new double[fields.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    if (!double.TryParse(fields[i], out p)) return false;
                    probs[i] = p;
                }

                if (Math.Abs(1 - probs.Sum()) > Epsilon) return false;

                m = new Multinomial(probs);
                return true;
            }
            else if (int.TryParse(s, out state))
            {
                m = new Multinomial(state + 1, state);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a multinomial that gives all probability mass to a single outcome
        /// </summary>
        /// <param name="correct">The outcome to give all mass to</param>
        /// <param name="numOutcomes">The total number of outcomes</param>
        public Multinomial(int numOutcomes, int correct)
        {
            if (numOutcomes <= 0 || numOutcomes > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(string.Format("numOutcomes must be non-negative and less than {0}", ushort.MaxValue));
            }
            if (correct < 0 || correct > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(string.Format("correct must be positive and less than {0}", ushort.MaxValue));
            }
            _correct = (ushort)correct;
            _numOutcomes = (ushort)numOutcomes;
            _probs = null;
            IsDefinite = true;
        }

        /// <summary>
        /// Creates a multinomial with an arbitrary distribution over outcomes
        /// </summary>
        /// <param name="probs">Probabilities of each outcome</param>
        public Multinomial(IList<double> probs)
        {
            try
            {
                _numOutcomes = (ushort)probs.Count;
            }
            catch (InvalidCastException)
            {
                throw new ArgumentOutOfRangeException(string.Format("numOutcomes must be non-negative and less than {0}", ushort.MaxValue));
            }
            _correct = 0;
            _probs = new double[_numOutcomes];
            double totalMass = _probs[0] = probs[0];
            IsDefinite = totalMass == 1;
            for (ushort i = 1; i < _numOutcomes; i++)
            {
                double prob = probs[i];
                if (prob < 0 || prob > 1) throw new ArgumentException("probs must be between 0 and 1");
                if (prob == 1) IsDefinite = true;
                _probs[i] = prob;
                totalMass += prob;
                if (prob > _probs[_correct]) _correct = i;
            }
            if (Math.Abs(totalMass - 1.0) > Epsilon)
            {
                throw new ArgumentException("probs must sum to one.", "probs");
            }
        }

        static private double Epsilon = 1e-12;

        /// <summary>
        /// Creates a multinomial with an arbitrary distriubtion over outcomes
        /// </summary>
        /// <param name="probs">Probabilities of each outcome</param>
        public Multinomial(params double[] probs) : this((IList<double>)probs) { }

        ///// <summary>
        ///// Returns whether the multinomial has all mass on a single outcome
        ///// </summary>
        //public bool IsDefinite { get; private set; }

        /// <summary>
        /// Gets the amount of probability mass assigned to the ith outcome
        /// </summary>
        /// <param name="i">which outcome</param>
        /// <returns>probability of outcome</returns>
        public double this[int outcome]
        {
            get
            {
                return !IsDefinite ? _probs[outcome] : (outcome == _correct ? 1.0 : 0.0);
            }
        }
        public double[] ProbabilityArray
        {
            get
            {
                if (_probs == null)
                {
                    _probs = new double[_numOutcomes];
                    _probs[_correct] = 1.0;
                }
                return _probs;
            }
        }

        ///// <summary>
        ///// Get's the probability array.
        ///// </summary>
        ///// <returns></returns>
        //public double[] ToProbArray()
        //{
        //    return Probs.ToArray();
        //    ////!!!When Probs is around, this requires both an allocation and a copy (two linear operations). Why not just pass a read-only version of Probs?
        //    //double[] result = new double[NumOutcomes];
        //    //if (Probs == null)
        //    //    result[_correct] = 1;
        //    //else
        //    //    Array.Copy(Probs, result, Probs.Length);
        //    //return result;
        //}

        /// <summary>
        /// Gets the number of discrete outcomes
        /// </summary>
        public int NumOutcomes
        {
            get { return _numOutcomes; }
        }

        /// <summary>
        /// Gets the index of the outcome with the highest probability
        /// </summary>
        public int BestOutcome
        {
            get { return _correct; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Multinomial)) return false;
            else return this.Equals((Multinomial)obj);
        }

        public bool Equals(Multinomial other)
        {
            if (_correct != other._correct) return false;
            if (NumOutcomes != other.NumOutcomes) return false;
            if (_probs == null)
            {
                if (other._probs != null) return false;
            }
            else
            {
                if (!Enumerable.SequenceEqual(_probs, other._probs)) return false;
            }
            return true;
        }

        public static bool operator ==(Multinomial a, Multinomial b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Multinomial a, Multinomial b)
        {
            return !a.Equals(b);
        }
        public override int GetHashCode()
        {
            int hashCode = _correct.GetHashCode() ^ NumOutcomes.GetHashCode();
            if (_probs != null)
                for (int i = 0; i < _numOutcomes; i++)
                    hashCode ^= _probs[i].GetHashCode();
            return hashCode;
        }

        public double GenerateOne(ref Random random)
        {
            ///This could be made faster by keeping a cummulative distribution around and then using binary search
            if (null == _probs)
            {
                double value = random.NextDouble(); //We draw a number randomly, even if it is not used so that the state of the random number generator doesn't depend on the nature of the multinomial.
                return _correct;
            }
            else
            {
                return SpecialFunctions.PickRandomBin(_probs, ref random);
            }

        }


        public override string ToString()
        {
            return "[" + _probs.StringJoin(",") + "]";
        }
    }
}
