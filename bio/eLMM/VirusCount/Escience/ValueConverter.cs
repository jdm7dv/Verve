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
using MBT.Escience.Parse;
using Bio.Util;
using MBT.Escience.LogisticRegression;

namespace MBT.Escience
{
    public static class ValueConverter
    {
        public static readonly Bio.Util.ValueConverter<string, double> StringToDouble = new StringToDoubleConverter();
        public static readonly Bio.Util.ValueConverter<double, double> DoubleToDouble = new DoubleToDoubleConverter();
        public static readonly Bio.Util.ValueConverter<double, string> DoubleToString = StringToDouble.Inverted;
        public static readonly Bio.Util.ValueConverter<int, SufficientStatistics> IntToSufficientStatistics = new IntToSufficientStatisticsConverter();
        public static readonly Bio.Util.ValueConverter<SufficientStatistics, int> SufficientStatisticsToInt = IntToSufficientStatistics.Inverted;
        public static readonly Bio.Util.ValueConverter<char, SufficientStatistics> CharToSufficientStatistics = new CharToSufficientStatisticsConverter();
        public static readonly Bio.Util.ValueConverter<SufficientStatistics, char> SufficientStatisticsToChar = CharToSufficientStatistics.Inverted;
        public static readonly Bio.Util.ValueConverter<Multinomial, SufficientStatistics> MultinomialToSufficientStatistics = new MultinomialToSufficientStatisticsConverter();
        public static readonly Bio.Util.ValueConverter<SufficientStatistics, Multinomial> SufficientStatisticsToMultinomial = MultinomialToSufficientStatistics.Inverted;
        public static readonly Bio.Util.ValueConverter<string, UOPair<char>> StringToUOPair = new StringToUOPairConverter();
        public static readonly Bio.Util.ValueConverter<UOPair<char>, string> UOPairToString = StringToUOPair.Inverted;
    }
    internal static class ValueOrMissingConverter
    {
        public static readonly Bio.Util.ValueConverter<double, double> NaNAsZero = new NaNAsZeroConverter();
    }

    //!!!would be nice to get simplify identitys away
    internal class DoubleToDoubleConverter : Bio.Util.ValueConverter<double, double>
    {
        public DoubleToDoubleConverter() : base(value => value, value => value) { }
    }

    internal class NaNAsZeroConverter : Bio.Util.ValueConverter<double, double>
    {
        public NaNAsZeroConverter() : base(valueOrNaN => double.IsNaN(valueOrNaN) ? 0 : valueOrNaN, value => { throw new Exception("Can't convert back"); }) { }
    }


    internal class MultinomialToSufficientStatisticsConverter : Bio.Util.ValueConverter<Multinomial, SufficientStatistics>
    {

        public MultinomialToSufficientStatisticsConverter()
            : base(
                m => m.IsDefinite ? (SufficientStatistics)DiscreteStatistics.GetInstance(m.BestOutcome) : (SufficientStatistics)MultinomialStatistics.GetInstance(m),
                s =>
                {
                    var mult = s as MultinomialStatistics;
                    if (mult != null)
                        return mult.Multinomial;
                    int i = (int)s.AsDiscreteStatistics();
                    return new Multinomial(i + 1, i);
                }
                )
        { }
    }

    internal class IntToSufficientStatisticsConverter : Bio.Util.ValueConverter<int, SufficientStatistics>
    {
        public IntToSufficientStatisticsConverter() : base(i => DiscreteStatistics.GetInstance(i), stats => (int)stats.AsDiscreteStatistics()) { }
    }

    internal class CharToSufficientStatisticsConverter : Bio.Util.ValueConverter<char, SufficientStatistics>
    {
        public CharToSufficientStatisticsConverter() : base(c => SufficientStatistics.Parse(c.ToString()), ss => SpecialFunctions.FirstAndOnly(ss.ToString())) { }
    }


    internal class StringToDoubleConverter : Bio.Util.ValueConverter<string, double>
    {
        public StringToDoubleConverter() : base(c => double.Parse(c.Trim()), c => c.ToString()) { }
    }

    internal class StringToUOPairConverter : ValueConverter<string, UOPair<char>>
    {
        public StringToUOPairConverter() : base(s => UOPair.Create(s[0], s[1]), pair => string.Format("{0}{1}", pair.First, pair.Second)) { }
    }
}

