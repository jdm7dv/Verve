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
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Bio.Util;
using System.Linq;
using MBT.Escience.Parse;

namespace MBT.Escience
{
    [Serializable]
    public abstract class KeepTest<TRow>
    {
        public const char KEEP_TEST_DELIMITER = ';';

        private string _prefix, _args;
        protected KeepTest(string prefix, string args)
        {
            _prefix = prefix;
            _args = args;
        }

        /// <summary>
        /// General KeepTest factory. This method will parse keepTestNameAndArgs and return the "registered" KeepTest that exactly matches
        /// the string (case insensitive). To "register" a new keep test, it muse
        ///	 1) Inherit from KeepTest. It can permanently bind TRow, but then can only be created when GetInstance is called with TRow 
        ///		 bound to the same thing.
        ///	 2) Define {public | internal} static string Prefix = name, where name is what is used to parse keepTestNameAndArgs. That is,
        ///		 if keepTestNameAndArgs starts with Prefix, and instance of that class will be created. Please take care to make sure you 
        ///		 don't clober another keeptest. Note that if the scope is internal, it can be created by GetInstance, but will not be
        ///		 advertised by PrintKeepTests().
        ///	 3) [OPTIONAL] Define public static string ArgsDescriptor. If you keep test requires arguments, then you should define this
        ///		 static field, which describes what those arguments are. This is used only by PrintKeepTests() to advertise your KeepTest.
        ///	 4) Definine public static KeepTest {GetInstance() | GetInstance(string args)} as a factory method. If the keep test requires
        ///		 arguments, they must be parsible from a string, and you must implement GetInstance(string args). If no arguments are required,
        ///		 implement GetInstance(). For optional arguments, implement GetInstance(string args), and allow args to be the empty string.
        ///		 Note that if both are implemented, only GetInstance(string args) will be used.
        /// </summary>
        /// <param name="keepTestNameAndArgs"></param>
        /// <returns></returns>
        public static KeepTest<TRow> Parse(string keepTestNameAndArgs)
        {
            return GetInstance(keepTestNameAndArgs);
        }

        public static KeepTest<TRow> GetInstance(string keepTestNameAndArgs)
        {
            if (keepTestNameAndArgs.Equals("options", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new HelpException(PrintKeepTests().StringJoin("<br>"));
            }

            foreach (Type keepTestType in EnumerateKeepTestTypes())
            {
                MemberInfo[] infoArray = keepTestType.GetMember("Prefix", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);
                if (infoArray.Length == 1)
                {
                    FieldInfo prefixInfo = (FieldInfo)infoArray[0];
                    string prefix = prefixInfo.GetValue(null).ToString();

                    if (keepTestNameAndArgs.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
                    {
                        string argsString = keepTestNameAndArgs.Substring(prefix.Length);
                        string[] args;
                        MethodInfo getInstance;

                        getInstance = keepTestType.GetMethod("GetInstance", new Type[] { typeof(string) });
                        if (getInstance == null) // take string args
                        {
                            Helper.CheckCondition<ParseException>(argsString.Length == 0, string.Format("{0} does not have any GetInstance methods that take string parameters.", keepTestType.Name));
                            getInstance = keepTestType.GetMethod("GetInstance", new Type[0]);
                            args = new string[0];
                        }
                        else
                        {
                            args = new string[] { argsString };
                        }

                        try
                        {
                            KeepTest<TRow> result = (KeepTest<TRow>)getInstance.Invoke(null, args);
                            return result;
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine("Exception in GetInstance: " + e.InnerException.Message);
                            throw new ParseException(e.InnerException.Message);
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Could not parse {0} into a keep test of type {1}", keepTestNameAndArgs, typeof(TRow)));
        }

        public static List<KeepTest<TRow>> GetKeepTestList(string keepTestListAsString)
        {
            if (keepTestListAsString == null)
            {
                return new List<KeepTest<TRow>>();
            }
            if (keepTestListAsString.StartsWith("(") && keepTestListAsString.EndsWith(")"))
            {
                keepTestListAsString = keepTestListAsString.Substring(1, keepTestListAsString.Length - 2);
            }
            if (keepTestListAsString.Length == 0)
            {
                return new List<KeepTest<TRow>>();
            }

            List<KeepTest<TRow>> result = new List<KeepTest<TRow>>();

            List<string> keepTestNames = SpecialFunctions.ParenProtectedSplit(keepTestListAsString, KEEP_TEST_DELIMITER);
            foreach (string keepTestName in keepTestNames)
            {
                result.Add(GetInstance(keepTestName));
            }
            return result;
        }



        public static IEnumerable<Type> EnumerateKeepTestTypes()
        {
            Type keepType = typeof(KeepTest<TRow>);
            Assembly assembly = Assembly.GetAssembly(keepType);
            foreach (Type rawType in assembly.GetTypes())
            {
                Type emittedType = rawType;
                if (rawType.IsGenericTypeDefinition)
                {
                    Type[] typeParams = rawType.GetGenericArguments();
                    // Generic KeepTests have one generic type and it has no constraints. This check ensures we don't throw an exception
                    if (typeParams.Length == 1 && typeParams[0].GetGenericParameterConstraints().Length == 0)
                    {
                        emittedType = rawType.MakeGenericType(new Type[] { typeof(TRow) });
                    }
                }
                if (emittedType.IsSubclassOf(keepType))
                {
                    yield return emittedType;
                }
            }
        }

        public static IEnumerable<string> PrintKeepTests()
        {
            Console.WriteLine("Available KeepTests. <> denotes required parameters (omit <> when using), [] denotes optional parameters.");

            foreach (Type type in EnumerateKeepTestTypes())
            {
                MemberInfo[] infoArray = type.GetMember("Prefix", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
                if (infoArray.Length == 1)
                {
                    FieldInfo prefixInfo = (FieldInfo)infoArray[0];
                    string prefix = prefixInfo.GetValue(null).ToString();
                    string args = "";

                    infoArray = type.GetMember("ArgsDescriptor", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
                    if (infoArray.Length > 0)
                    {
                        FieldInfo argsInfo = (FieldInfo)infoArray[0];
                        args = string.Format("<{0}>", argsInfo.GetValue(null));
                    }
                    yield return string.Format("\t{0}{1}", prefix, args);
                }
            }
        }

        public abstract bool Test(TRow row);

        public override string ToString()
        {
            return _prefix + (_args == null ? "" : _args);
        }

        public virtual void Reset() { } // gives KeepTests a chance to reset after a run through the entire set.

        //public static KeepTest<TRow> GetGenericInstance(string inputDirectory, string keepTestName)
        //{
        //	if (keepTestName.Equals("AlwaysKeep", StringComparison.CurrentCultureIgnoreCase))
        //	{
        //		return AlwaysKeep<TRow>.GetInstance();
        //	}


        //	throw new Exception("Don't know KeepTest " + keepTestName);

        //	return null;
        //}

        public abstract bool IsCompatibleWithNewKeepTest(KeepTest<TRow> keepTestNew);
    }

    [Serializable]
    public class AlwaysKeep<TRow> : KeepTest<TRow>
    {
        public readonly static string Prefix = "AlwaysKeep";

        public AlwaysKeep()
            : base(Prefix, "")
        {
        }

        public static KeepTest<TRow> GetInstance()
        {
            return new AlwaysKeep<TRow>();
        }

        public override bool Test(TRow row)
        {
            return true;
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<TRow> keepTestNew)
        {
            return true;
        }

    }

    /// <summary>
    /// Does everything but the Test() method for Collection-based tests, such as And and Or.
    /// </summary>
    /// <typeparam name="TRow"></typeparam>
    public abstract class KeepCollection<TRow> : KeepTest<TRow>
    {
        public readonly static string ArgsDescriptor = "(KeepTest1;KeepTest2[;KeepTest3;...])";

        protected abstract string prefix
        {
            get;
        }

        public List<KeepTest<TRow>> KeepTestCollection;

        protected KeepCollection(string prefix, string keepTestNames)
            : base(prefix, keepTestNames)
        {
            //KeepTestCollection = new List<KeepTest<TRow>>();
            //foreach (string keepTestName in keepTestNames.Split(';'))
            //{
            //    if (keepTestName.Length > 0)
            //    {
            //        KeepTest<TRow> keepTest = KeepTest<TRow>.GetInstance(keepTestName);
            //        KeepTestCollection.Add(keepTest);
            //    }
            //}
            KeepTestCollection = GetKeepTestList(keepTestNames);
            if (KeepTestCollection.Count == 0)
            {
                KeepTestCollection.Add(AlwaysKeep<TRow>.GetInstance());
            }
        }

        protected KeepCollection(params KeepTest<TRow>[] keepTests)
            :
            this((IEnumerable<KeepTest<TRow>>)keepTests) { }

        protected KeepCollection(IEnumerable<KeepTest<TRow>> keepTestCollection)
            : base("", "")
        {
            KeepTestCollection = new List<KeepTest<TRow>>();
            foreach (KeepTest<TRow> keepTest in keepTestCollection)
            {
                if (!(keepTest is AlwaysKeep<TRow>))
                {
                    KeepTestCollection.Add(keepTest);
                }
            }

            if (KeepTestCollection.Count == 0)
            {
                KeepTestCollection.Add(AlwaysKeep<TRow>.GetInstance());
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", prefix, KeepTestCollection.StringJoin(";"));
        }




        public override bool IsCompatibleWithNewKeepTest(KeepTest<TRow> keepTestNew)
        {
            KeepCollection<TRow> newCollection = (KeepCollection<TRow>)keepTestNew;
            if (newCollection == null || this.KeepTestCollection.Count != newCollection.KeepTestCollection.Count)
            {
                return false;
            }
            //!!!could try every permutation, but for now require them to be in the same order

            for (int i = 0; i < KeepTestCollection.Count; ++i)
            {
                if (!KeepTestCollection[i].IsCompatibleWithNewKeepTest(newCollection.KeepTestCollection[i]))
                {
                    return false;
                }
            }
            return true;
        }


    }

    public class And<TRow> : KeepCollection<TRow>
    {
        public readonly static string Prefix = "And";

        protected override string prefix
        {
            get { return Prefix; }
        }

        private And(string keepTestNamesAndParams) : base(Prefix, keepTestNamesAndParams) { }

        private And(params KeepTest<TRow>[] keepTests)
            :
            this((IEnumerable<KeepTest<TRow>>)keepTests) { }

        private And(IEnumerable<KeepTest<TRow>> keepTests)
            :
            base(keepTests) { }


        new public static And<TRow> GetInstance(string keepTestNamesAndParams)
        {
            return new And<TRow>(keepTestNamesAndParams);
        }

        public static KeepTest<TRow> GetInstance(params KeepTest<TRow>[] keepTests)
        {
            return GetInstance((IEnumerable<KeepTest<TRow>>)keepTests);
        }

        public static KeepTest<TRow> GetInstance(IEnumerable<KeepTest<TRow>> keepTests)
        {
            And<TRow> andTest = new And<TRow>(keepTests);
            if (andTest.KeepTestCollection.Count == 1)
            {
                return andTest.KeepTestCollection[0];
            }
            else
            {
                return andTest;
            }
        }

        public override bool Test(TRow row)
        {
            foreach (KeepTest<TRow> keepTest in KeepTestCollection)
            {
                if (!keepTest.Test(row))
                {
                    return false;
                }
            }
            return true;
        }



    }

    public class Or<TRow> : KeepCollection<TRow>
    {
        public readonly static string Prefix = "Or";

        protected override string prefix
        {
            get { return Prefix; }
        }

        private Or(string keepTestNamesAndParams) : base(Prefix, keepTestNamesAndParams) { }

        protected Or(params KeepTest<TRow>[] keepTests)
            :
            this((IEnumerable<KeepTest<TRow>>)keepTests) { }

        protected Or(IEnumerable<KeepTest<TRow>> keepTests)
            :
            base(keepTests) { }

        new public static Or<TRow> GetInstance(string keepTestNamesAndParams)
        {
            return new Or<TRow>(keepTestNamesAndParams);
        }

        public static KeepTest<TRow> GetInstance(params KeepTest<TRow>[] keepTests)
        {
            return GetInstance((IEnumerable<KeepTest<TRow>>)keepTests);
        }

        public static KeepTest<TRow> GetInstance(IEnumerable<KeepTest<TRow>> keepTests)
        {
            Or<TRow> orTest = new Or<TRow>(keepTests);
            if (orTest.KeepTestCollection.Count == 1)
            {
                return orTest.KeepTestCollection[0];
            }
            else
            {
                return orTest;
            }
        }

        public override bool Test(TRow row)
        {
            foreach (KeepTest<TRow> keepTest in KeepTestCollection)
            {
                if (keepTest.Test(row))
                {
                    return true;
                }
            }
            return false;
        }

    }

    /// <summary>
    /// Returns true if exactly 1 of the collection of KeepTests returns true.
    /// </summary>
    /// <typeparam name="TRow"></typeparam>
    public class Xor<TRow> : KeepCollection<TRow>
    {
        public readonly static string Prefix = "Xor";

        protected override string prefix
        {
            get { return Prefix; }
        }

        private Xor(string keepTestNamesAndParams) : base(Prefix, keepTestNamesAndParams) { }

        private Xor(params KeepTest<TRow>[] keepTests)
            :
            this((IEnumerable<KeepTest<TRow>>)keepTests) { }

        private Xor(IEnumerable<KeepTest<TRow>> keepTests)
            :
            base(keepTests) { }

        new public static KeepTest<TRow> GetInstance(string keepTestNamesAndParams)
        {
            return new Xor<TRow>(keepTestNamesAndParams);
        }

        public static KeepTest<TRow> GetInstance(params KeepTest<TRow>[] keepTests)
        {
            return GetInstance((IEnumerable<KeepTest<TRow>>)keepTests);
        }

        public static KeepTest<TRow> GetInstance(IEnumerable<KeepTest<TRow>> keepTests)
        {
            Xor<TRow> xOrTest = new Xor<TRow>(keepTests);
            if (xOrTest.KeepTestCollection.Count == 1)
            {
                return xOrTest.KeepTestCollection[0];
            }
            else
            {
                return xOrTest;
            }
        }

        public override bool Test(TRow row)
        {
            bool returnVal = false;
            foreach (KeepTest<TRow> keepTest in KeepTestCollection)
            {
                if (keepTest.Test(row))
                {
                    if (returnVal)  // someone else has already returned true, so we're done.
                        return false;
                    else
                        returnVal = true;
                }
            }
            return returnVal;
        }

    }

    public class Not<TRow> : KeepTest<TRow>
    {
        public readonly static string Prefix = "Not";
        public readonly static string ArgsDescriptor = "NegatedKeepTest";

        private readonly KeepTest<TRow> _internalKeepTest;

        private Not(KeepTest<TRow> internalKeepTest)
            : base(Prefix, "")
        {
            _internalKeepTest = internalKeepTest;
        }


        new public static Not<TRow> GetInstance(string internalKeepTest)
        {
            return new Not<TRow>(KeepTest<TRow>.GetInstance(internalKeepTest));
        }
        public static Not<TRow> GetInstance(KeepTest<TRow> internalKeepTest)
        {
            return new Not<TRow>(internalKeepTest);
        }


        public override bool Test(TRow row)
        {
            return !_internalKeepTest.Test(row);
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<TRow> keepTestNew)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Prefix + _internalKeepTest.ToString();
        }
    }


    public class KeepRandom<TRow> : KeepTest<TRow>
    {
        public readonly static string Prefix = "KeepRandom";
        public readonly static string ArgsDescriptor = "P";
        private Random _rand;
        private double p;
        private string _seedString;
        private KeepRandom(double p, string seedString)
            : base(Prefix, p.ToString())
        {
            Helper.CheckCondition(p >= 0 && p <= 1, p + " is not a probability");
            _seedString = seedString;
            _rand = new MachineInvariantRandom(seedString);
            this.p = p;
        }

        new public static KeepRandom<TRow> GetInstance(string pString)
        {
            return new KeepRandom<TRow>(double.Parse(pString), "KeepRandom");
        }
        public static KeepRandom<TRow> GetInstance(double p)
        {
            return new KeepRandom<TRow>(p, "KeepRandom");
        }

        public override bool Test(TRow row)
        {
            return _rand.NextDouble() < p;
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<TRow> keepTestNew)
        {
            return false;   // random can't be compatible will anything
        }

        //public override string ToString()
        //{
        //	return Prefix + p;
        //}

        /// <summary>
        /// Reset random 
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            _rand = new MachineInvariantRandom(_seedString);
        }

    }



    //public class KeepPopularHlas : KeepTest<Dictionary<string, string>>
    //{
    //	private KeepPopularHlas()
    //	{
    //	}

    //	private int PidsPerHlaRequired;
    //	private CaseNamesAndHlas PidsAndHlas;
    //	private PatientsAndSequences PatientsAndSequences;

    //	internal static KeepTest<Dictionary<string, string>>GetInstance(string inputDirectory, string binarySeqFileName, string hlaFileName, int pidsPerHlaRequired, int merSize, Dictionary<int, string> pidToCaseName)
    //	{
    //		KeepPopularHlas aKeepPopularHlas = new KeepPopularHlas();
    //		aKeepPopularHlas.PidsPerHlaRequired = pidsPerHlaRequired;
    //		aKeepPopularHlas.PatientsAndSequences = PatientsAndSequences.GetInstance(inputDirectory, binarySeqFileName, merSize, pidToCaseName);
    //		aKeepPopularHlas.PidsAndHlas = CaseNamesAndHlas.GetInstance(inputDirectory, aKeepPopularHlas.PatientsAndSequences, pidToCaseName, hlaFileName);

    //		//aKeepPopularHlas.PidsAndHlas.Test();

    //		return aKeepPopularHlas;
    //	}

    //	internal static string Prefix = "KeepPopularHlas";

    //	public override string ToString()
    //	{
    //		return Prefix + PidsPerHlaRequired.ToString();
    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		string hla = row[Tabulate.HlaColumnName];
    //		if (!PidsAndHlas.HlaToCaseNameSet.ContainsKey(hla))
    //		{
    //			return false;
    //		}
    //		Set<string> pidSet = PidsAndHlas.HlaToCaseNameSet[hla];
    //		return pidSet.Count >= PidsPerHlaRequired;
    //	}

    //	//!!!would be nice if class didn't have to know all these classes it was compatible with
    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>>keepTestNew)
    //	{
    //		if (keepTestNew is KeepPopularHlas)
    //		{
    //			return PidsPerHlaRequired <= ((KeepPopularHlas)keepTestNew).PidsPerHlaRequired;
    //		}

    //		if (keepTestNew is And<Dictionary<string, string>>)
    //		{
    //			And<Dictionary<string, string>> aAnd = (And<Dictionary<string, string>>)keepTestNew;
    //			foreach (KeepTest<Dictionary<string, string>>conjunct in aAnd.KeepTestCollection)
    //			{
    //				if (!IsCompatibleWithNewKeepTest(conjunct))
    //				{
    //					return false;
    //				}
    //			}
    //			return true;
    //		}

    //		return false;
    //	}
    //}

    //public class KeepPopularMers : KeepTest<Dictionary<string, string>>
    //{
    //	private KeepPopularMers()
    //	{
    //	}

    //	private int CasesPerMerRequired;
    //	private PatientsAndSequences PatientsAndSequences;
    //	private int MerSize;

    //	internal static KeepTest<Dictionary<string, string>>GetInstance(string inputDirectory, string binarySeqFileName, int casesPerMerRequired, int merSize, Dictionary<int, string> pidToCaseName)
    //	{
    //		KeepPopularMers aKeepPopularMers = new KeepPopularMers();
    //		aKeepPopularMers.CasesPerMerRequired = casesPerMerRequired;
    //		Helper.CheckCondition(merSize > 1, "This test only makes sense for mers larger than 1");
    //		aKeepPopularMers.MerSize = merSize;
    //		aKeepPopularMers.PatientsAndSequences = PatientsAndSequences.GetInstance(inputDirectory, binarySeqFileName, merSize, pidToCaseName);

    //		//aKeepPopularMers.PidsAndMers.Test();

    //		return aKeepPopularMers;
    //	}

    //	internal static string Prefix = "KeepPopularMers";

    //	public override string ToString()
    //	{
    //		return Prefix + CasesPerMerRequired.ToString();
    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		Mer mer = Mer.GetInstance(MerSize, row[Tabulate.MerTargetColumnName]);
    //		Helper.CheckCondition(mer.ToString().Length != 1, "This test doesn't make sense to apply to 1mers");

    //		if (!PatientsAndSequences.MerToCaseNameSet.ContainsKey(mer))
    //		{
    //			return false;
    //		}
    //		Set<string> pidSet = PatientsAndSequences.MerToCaseNameSet[mer];
    //		//Debug.WriteLine(pidSet.Count);
    //		return pidSet.Count >= CasesPerMerRequired;
    //	}

    //	//!!!would be nice if class didn't have to know all these classes it was compatible with
    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>>keepTestNew)
    //	{
    //		if (keepTestNew is KeepPopularMers)
    //		{
    //			return CasesPerMerRequired <= ((KeepPopularMers)keepTestNew).CasesPerMerRequired;
    //		}

    //		if (keepTestNew is And<Dictionary<string, string>>)
    //		{
    //			And<Dictionary<string, string>> aAnd = (And<Dictionary<string, string>>)keepTestNew;
    //			foreach (KeepTest<Dictionary<string, string>>conjunct in aAnd.KeepTestCollection)
    //			{
    //				if (!IsCompatibleWithNewKeepTest(conjunct))
    //				{
    //					return false;
    //				}
    //			}
    //			return true;
    //		}

    //		return false;
    //	}
    //}

    //public class KeepMersAtPosition : KeepTest<Dictionary<string, string>>
    //{
    //	private KeepMersAtPosition()
    //	{
    //	}

    //	private int CasesPerMerRequired;
    //	private PatientsAndSequences PatientsAndSequences;
    //	private int MerSize;

    //	internal static KeepTest<Dictionary<string, string>>GetInstance(string inputDirectory, string binarySeqFileName, int casesRequired, int merSize, Dictionary<int, string> pidToCaseName)
    //	{
    //		KeepMersAtPosition aKeepMersAtPosition = new KeepMersAtPosition();
    //		aKeepMersAtPosition.CasesPerMerRequired = casesRequired;
    //		aKeepMersAtPosition.MerSize = merSize;
    //		aKeepMersAtPosition.PatientsAndSequences = PatientsAndSequences.GetInstance(inputDirectory, binarySeqFileName, merSize, pidToCaseName);

    //		//aKeepMersAtPosition.PidsAndMers.Test();

    //		return aKeepMersAtPosition;
    //	}

    //	internal static string Prefix = "KeepMersAtPosition";

    //	public override string ToString()
    //	{
    //		return Prefix + CasesPerMerRequired.ToString();
    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		Mer mer = Mer.GetInstance(MerSize, row[Tabulate.MerTargetColumnName]);
    //		int n1Pos = int.Parse(row[Tabulate.Nuc1TargetPositionColumnName]);

    //		Dictionary<string,bool?> caseNameToTargetValue = PatientsAndSequences.N1PosToMerToCaseNameToBool[n1Pos][mer];
    //		int trueCount = SpecialFunctions.CountIf(caseNameToTargetValue.Values, delegate(bool? value) { return value != null && (bool)value; });
    //		return trueCount >= CasesPerMerRequired;
    //	}

    //	//!!!would be nice if class didn't have to know all these classes it was compatible with
    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
    //	{
    //		if (keepTestNew is KeepMersAtPosition)
    //		{
    //			return CasesPerMerRequired <= ((KeepMersAtPosition)keepTestNew).CasesPerMerRequired;
    //		}

    //		//!!!this code is duplicate many times
    //		if (keepTestNew is And<Dictionary<string, string>>)
    //		{
    //			And<Dictionary<string, string>> aAnd = (And<Dictionary<string, string>>)keepTestNew;
    //			foreach (KeepTest<Dictionary<string, string>> conjunct in aAnd.KeepTestCollection)
    //			{
    //				if (!IsCompatibleWithNewKeepTest(conjunct))
    //				{
    //					return false;
    //				}
    //			}
    //			return true;
    //		}

    //		return false;
    //	}
    //}


    //public class K2 : KeepTest<Dictionary<string, string>>
    //{
    //	private K2()
    //	{
    //	}

    //	int k2;
    //	internal static KeepTest<Dictionary<string, string>> GetInstance(int k2)
    //	{
    //		if (k2 == int.MaxValue)
    //		{
    //			return AlwaysKeep<Dictionary<string, string>>.GetInstance();
    //		}
    //		K2 aK2 = new K2();
    //		aK2.k2 = k2;
    //		return aK2;
    //	}

    //	internal static string Prefix = "K2=";

    //	public override string ToString()
    //	{
    //		return string.Format("{0}{1}", Prefix, k2);
    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		int nullCount = int.Parse(row[Tabulate.PredictorNullNameCountColumnName]);
    //		return (nullCount <= k2);
    //	}

    //	//!!!would be nice if class didn't have to know all these classes it was compatible with
    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
    //	{
    //		if (keepTestNew is K2)
    //		{
    //			return k2 >= ((K2)keepTestNew).k2;
    //		}


    //		//!!!this code is duplicate many times
    //		if (keepTestNew is And<Dictionary<string, string>>)
    //		{
    //			And<Dictionary<string, string>> aAnd = (And<Dictionary<string, string>>)keepTestNew;
    //			foreach (KeepTest<Dictionary<string, string>> conjunct in aAnd.KeepTestCollection)
    //			{
    //				if (!IsCompatibleWithNewKeepTest(conjunct))
    //				{
    //					return false;
    //				}
    //			}
    //			return true;
    //		}

    //		return false;
    //	}
    //}

    public class Row : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "Row";
        public readonly static string ArgsDescriptor = "[colName]OPvalue where OP is [=|!=|<|>|<=|>=|StartsWith|EndsWith|Contains|Length|In|IsMerPos]";

        enum TestType
        {
            Double, String, Set
        };

        private delegate bool BinaryStringOperator(string val, string reference);
        private delegate bool BinaryDoubleOperator(double val, double reference);
        private delegate bool BinarySetOperator(string val, HashSet<string> reference);

        private readonly string _columnName;
        private readonly string _goalValueAsString;
        private readonly double _goalValueAsDouble;
        private readonly HashSet<string> _goalValueAsStringSet;

        private readonly BinaryDoubleOperator DoubleOp;
        private readonly BinaryStringOperator StringOp;
        private readonly BinarySetOperator SetOp;
        private readonly TestType _testType;

        private Row(string argString)
            : base(Prefix, argString)
        {

            Regex regex = new Regex(@"\[(?<col>[\w\s]+)\](?<op><=|>=|!=|=|<|>|StartsWith|EndsWith|Contains|Length|In|IsMerPos)(?<targVal>[\S]*)", RegexOptions.IgnoreCase);

            Match m = regex.Match(argString);
            Helper.CheckCondition(m.Success, "\nInvalid Row discriptor {0}. Specify a row using \"{1}\"", argString, ArgsDescriptor);



            _columnName = m.Groups["col"].Value;
            string op = m.Groups["op"].Value;
            _goalValueAsString = m.Groups["targVal"].Value.ToLower();

            switch (op.ToLower())
            {
                case "=":
                    StringOp = EqualsOp;
                    _testType = TestType.String;
                    break;
                case "!=":
                    StringOp = NotEqualsOp;
                    _testType = TestType.String;
                    break;
                case "<=":
                    DoubleOp = LessThanOrEqualToOp;
                    _testType = TestType.Double;
                    break;
                case "<":
                    DoubleOp = LessThanOp;
                    _testType = TestType.Double;
                    break;
                case ">":
                    DoubleOp = GreaterThanOp;
                    _testType = TestType.Double;
                    break;
                case ">=":
                    DoubleOp = GreaterThanOrEqualToOp;
                    _testType = TestType.Double;
                    break;
                case "startswith":
                    StringOp = StartsWithOp;
                    _testType = TestType.String;
                    break;
                case "endswith":
                    StringOp = EndsWithOp;
                    _testType = TestType.String;
                    break;
                case "contains":
                    StringOp = ContainsOp;
                    _testType = TestType.String;
                    break;
                case "ismerpos":
                    StringOp = IsMerPosOp;
                    _testType = TestType.String;
                    break;
                case "length":
                    try
                    {
                        _goalValueAsDouble = double.Parse(_goalValueAsString);
                    }
                    catch
                    {
                        throw new Exception("Could not parse " + _goalValueAsString + " into a double.");
                    }
                    StringOp = (str, dummy) => str.Length == _goalValueAsDouble;
                    //StringOp = delegate(string str, string dummy) { return str.Length == _goalValueAsDouble; };
                    _testType = TestType.String;
                    break;
                case "in":
                    _goalValueAsStringSet = _goalValueAsString.StartsWith("(") ? LoadStringSetFromParenthesizedSet(_goalValueAsString) : LoadStringSetFromFile(_goalValueAsString);
                    SetOp = (str, reference) => reference.Contains(str);
                    _testType = TestType.Set;
                    break;
                default:
                    throw new Exception("Can't get here or we wouldn't match the regex.");
            }

            if (_testType == TestType.Double)
            {
                try
                {
                    _goalValueAsDouble = double.Parse(_goalValueAsString);
                }
                catch
                {
                    throw new Exception("Could not parse " + _goalValueAsString + " into a double.");
                }
            }
        }

        private HashSet<string> LoadStringSetFromParenthesizedSet(string s)
        {
            Helper.CheckCondition(s[0] == '(' && s[s.Length - 1] == ')', "String set must be encapsulated by ()");
            var result = s.Substring(1, s.Length - 2).Split(',').ToHashSet(StringComparer.CurrentCultureIgnoreCase);
            return result;
        }

        private HashSet<string> LoadStringSetFromFile(string filename)
        {
            HashSet<string> set = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
            string line;
            using (TextReader reader = Bio.Util.FileUtils.OpenTextStripComments(filename))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    set.Add(line.Trim());
                }
            }
            return set;
        }

        new public static KeepTest<Dictionary<string, string>> GetInstance(string columnNameAndGoalValue)
        {
            return new Row(columnNameAndGoalValue);
            //int colNameStart = columnNameAndGoalValue.IndexOf('[') + 1;
            //int colNameLength = columnNameAndGoalValue.IndexOf(']') - colNameStart;
            //int valueStart = columnNameAndGoalValue.IndexOf('=') + 1;

            //Helper.CheckCondition(colNameStart >= 0 && colNameLength >= 0 && valueStart >= 0, string.Format("Invalid Row discriptor. Specify a row using \"{0}\"", ArgsDescriptor));

            //string columnName = columnNameAndGoalValue.Substring(colNameStart, colNameLength);
            //string goalValue = columnNameAndGoalValue.Substring(valueStart);

            //return new Row(columnName, goalValue);
        }



        public override bool Test(Dictionary<string, string> row)
        {
            try
            {
                string value = row[_columnName];
                switch (_testType)
                {
                    case TestType.Double:
                        double valueAsDouble = double.Parse(value);
                        return DoubleOp(valueAsDouble, _goalValueAsDouble);
                    case TestType.String:
                        return StringOp(value, _goalValueAsString);
                    case TestType.Set:
                        return SetOp(value, _goalValueAsStringSet);
                    default:
                        throw new Exception("Can't get here.");
                }
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(string.Format("Column name \"{0}\" is not in the table, which has keys\n{1}", _columnName, row.Keys.StringJoin(",")));
            }

            //bool test = row[_columnName].Equals(_goalValue, StringComparison.CurrentCultureIgnoreCase);
            //return test;
        }

        private static bool EqualsOp(string val1, string val2)
        {
            return val1.Equals(val2, StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool NotEqualsOp(string val1, string val2)
        {
            return !val1.Equals(val2, StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool StartsWithOp(string val1, string reference)
        {
            return val1.StartsWith(reference, StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool EndsWithOp(string val1, string reference)
        {
            return val1.EndsWith(reference, StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool ContainsOp(string val1, string reference)
        {
            return val1.ToLower().Contains(reference);
        }

        private static bool LessThanOp(double val, double reference)
        {
            return val < reference;
        }

        private static bool LessThanOrEqualToOp(double val, double reference)
        {
            return val <= reference;
        }

        private static bool GreaterThanOp(double val, double reference)
        {
            return val > reference;
        }

        private static bool GreaterThanOrEqualToOp(double val, double reference)
        {
            return val >= reference;
        }

        private static bool IsMerPosOp(string val, string reference)
        {
            KeyValuePair<string, double> merAndPos;
            return Tabulate.TryGetMerAndPos(val, out merAndPos);
        }


        //!!!would be nice if class didn't have to know all these classes it was compatible with
        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new NotImplementedException();
            //if (keepTestNew is Row)
            //{
            //    return _columnName == ((Row)keepTestNew)._columnName && _goalValue == ((Row)keepTestNew)._goalValue;
            //}

            //if (keepTestNew is And<Dictionary<string, string>>)
            //{
            //    And<Dictionary<string, string>> aAnd = (And<Dictionary<string, string>>)keepTestNew;
            //    foreach (KeepTest<Dictionary<string, string>> conjunct in aAnd.KeepTestCollection)
            //    {
            //        if (!IsCompatibleWithNewKeepTest(conjunct))
            //        {
            //            return false;
            //        }
            //    }
            //    return true;
            //}

            //return false;
        }

    }

    public class KeepOneDirection : KeepTest<Dictionary<string, string>>
    {
        bool keepPositiveCorrelation;

        public readonly static string Prefix = "KeepOneDirection";
        public readonly static string ArgsDescriptor = "Attraction|Escape";

        private KeepOneDirection() : base(Prefix, "") { }

        new public static KeepOneDirection GetInstance(string directionAttractionOrEscape)
        {
            KeepOneDirection keepTest = new KeepOneDirection();
            if (directionAttractionOrEscape.Equals("attraction", StringComparison.CurrentCultureIgnoreCase))
            {
                keepTest.keepPositiveCorrelation = true;
            }
            else if (directionAttractionOrEscape.Equals("escape", StringComparison.CurrentCultureIgnoreCase))
            {
                keepTest.keepPositiveCorrelation = false;
            }
            else
            {
                throw new ArgumentException(directionAttractionOrEscape + " must be either Attraction or Escape.");
            }
            return keepTest;
        }

        public override bool Test(Dictionary<string, string> row)
        {
            double pAB, pAb, paB, pab;
            if (row.ContainsKey("P_TT"))
            {
                pAB = double.Parse(row["P_TT"]);
                pAb = double.Parse(row["P_TF"]);
                paB = double.Parse(row["P_FT"]);
                pab = double.Parse(row["P_FF"]);
            }
            else if (row.ContainsKey(row["P_Ab"]))
            {
                pAB = double.Parse(row["P_AB"]);
                pAb = double.Parse(row["P_Ab"]);
                paB = double.Parse(row["P_aB"]);
                pab = double.Parse(row["P_ab"]);
            }
            else
            {
                throw new ArgumentException(Prefix + " works only when contingency tables are available. Please make sure you're running using Discrete and either Tabulating, or using the JointEvolution model or FishersExactTest models.");
            }

            double directionCoeff = pAB * pab - pAb * paB;
            if (keepPositiveCorrelation)
            {
                return directionCoeff >= 0;
            }
            else
            {
                return directionCoeff <= 0;
            }
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    //public class KeepFisherOneDirection : KeepTest<Dictionary<string, string>>
    //{
    //	bool keepPositiveCorrelation;

    //	public readonly static string Prefix = "KeepFisherOneDirection";

    //	private KeepFisherOneDirection() { }

    //	public static KeepFisherOneDirection GetInstance(string directionAttractionOrEscape)
    //	{
    //		KeepFisherOneDirection keepTest = new KeepFisherOneDirection();
    //		if (directionAttractionOrEscape == "attraction")
    //		{
    //			keepTest.keepPositiveCorrelation = true;
    //		}
    //		else if (directionAttractionOrEscape == "escape")
    //		{
    //			keepTest.keepPositiveCorrelation = false;
    //		}
    //		else
    //		{
    //			throw new ArgumentException(directionAttractionOrEscape + " must be either Attraction or Escape.");
    //		}
    //		return keepTest;
    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		double pAB = double.Parse(row["TT"]);
    //		double pAb = double.Parse(row["TF"]);
    //		double paB = double.Parse(row["FT"]);
    //		double pab = double.Parse(row["FF"]);

    //		double directionCoeff = pAB * pab - pAb * paB;
    //		if (keepPositiveCorrelation)
    //		{
    //			return directionCoeff >= 0;
    //		}
    //		else
    //		{
    //			return directionCoeff <= 0;
    //		}
    //	}

    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
    //	{
    //		throw new Exception("The method or operation is not implemented.");
    //	}
    //	public static string GetArgsDescription()
    //	{
    //		return "Attraction|Escape";
    //	}
    //}

    //public class KeepEndOfGag : KeepTest<Dictionary<string, string>>
    //{
    //	private KeepEndOfGag()
    //	{
    //	}

    //	bool KeepIt;
    //	internal static KeepTest<Dictionary<string, string>> GetInstance(bool keepIt)
    //	{
    //		if (keepIt)
    //		{
    //			return AlwaysKeep<Dictionary<string, string>>.GetInstance();
    //		}

    //		KeepEndOfGag aKeepEndOfGag = new KeepEndOfGag();
    //		aKeepEndOfGag.KeepIt = keepIt;
    //		return aKeepEndOfGag;
    //	}

    //	internal static string Prefix = "KeepEndOfGag";

    //	public override string ToString()
    //	{
    //		return string.Format("KeepEndOfGag{0}", KeepIt);
    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		Debug.Assert(!KeepIt); // real assert

    //		int nuc1Position = int.Parse(row[Tabulate.Nuc1TargetPositionColumnName]);
    //		Helper.CheckCondition((nuc1Position % 3) != 2, "nuc1Position is in neither the Gag nor the Pol frame");
    //		bool gagFrame = (nuc1Position % 3) == 1;
    //		if (gagFrame)
    //		{
    //			return nuc1Position < 2085;
    //		}
    //		else
    //		{
    //			return true;
    //		}

    //	}

    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
    //	{
    //		return false; //!!!could be made tighter
    //	}
    //}

    public class K1 : KeepTest<Dictionary<string, string>>
    {
        private K1()
            : base("", "")
        {
        }

        int k1;
        internal static KeepTest<Dictionary<string, string>> GetInstance(int k1)
        {
            if (k1 == 0)
            {
                return AlwaysKeep<Dictionary<string, string>>.GetInstance();
            }

            K1 aK1 = new K1();
            aK1.k1 = k1;
            return aK1;
        }

        internal static string Prefix = "K1=";

        public override string ToString()
        {
            return string.Format("K1={0}", k1);
        }

        public override bool Test(Dictionary<string, string> row)
        {
            int trueCount = int.Parse(row[Tabulate.PredictorTrueNameCountColumnName]);
            int falseCount = int.Parse(row[Tabulate.PredictorFalseNameCountColumnName]);
            return (trueCount >= k1 && falseCount >= k1);
        }

        //!!!would be nice if class didn't have to know all these classes it was compatible with
        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            if (keepTestNew is K1)
            {
                return k1 <= ((K1)keepTestNew).k1;
            }

            //!!!This code is duplicate many times
            if (keepTestNew is And<Dictionary<string, string>>)
            {
                And<Dictionary<string, string>> aAnd = (And<Dictionary<string, string>>)keepTestNew;
                foreach (KeepTest<Dictionary<string, string>> conjunct in aAnd.KeepTestCollection)
                {
                    if (!IsCompatibleWithNewKeepTest(conjunct))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }
    }

    public class KeepNonOverlappingAA : KeepTest<Dictionary<string, string>>
    {
        public static readonly String Prefix = "KeepNonOverlappingAA";

        private KeepNonOverlappingAA() : base(Prefix, "") { }

        public static KeepNonOverlappingAA GetInstance()
        {
            return new KeepNonOverlappingAA();
        }

        public override bool Test(Dictionary<string, string> row)
        {
            KeyValuePair<string, double> pred, targ;
            if (Tabulate.TryGetMerAndPos(row[Tabulate.PredictorVariableColumnName], out pred) &&
                Tabulate.TryGetMerAndPos(row[Tabulate.TargetVariableColumnName], out targ))
            {
                return Math.Abs(targ.Value - pred.Value) > 2;
            }
            return true;
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            // haven't done anything here.
            return false;
        }


    }

    /// <summary>
    /// Ensures that both TargetVariable and PredictorVariable (if AA) are within the ranges and are in frame.
    /// </summary>
    public class KeepGenes : KeepTest<Dictionary<string, string>>
    {
        public static readonly string Prefix = "KeepGenes";
        public static readonly string ArgsDescriptor = "DNAStart-DNAStop[,...]";

        private readonly List<Tuple<int, int>> _genes;
        private KeepGenes(string geneRanges)
            : base(Prefix, geneRanges)
        {
            _genes = new List<Tuple<int, int>>();
            try
            {
                foreach (string geneRange in geneRanges.Split(','))
                {
                    string[] fields = geneRange.Split('-');
                    int start = int.Parse(fields[0]);
                    int stop = int.Parse(fields[1]);
                    _genes.Add(new Tuple<int, int>(start, stop));
                }
            }
            catch
            {
                throw new FormatException(string.Format("Could not parse range {0}.", geneRanges));
            }
        }

        new public static KeepGenes GetInstance(string geneRanges)
        {
            KeepGenes keepGene = new KeepGenes(geneRanges);

            return keepGene;
        }

        public override bool Test(Dictionary<string, string> row)
        {
            bool keepTarget = true;
            bool keepPredictor = true;

            string targVar = row[Tabulate.TargetVariableColumnName];
            string predVar = row[Tabulate.PredictorVariableColumnName];

            if (predVar.Contains("@"))
            {
                int pos = (int)Tabulate.GetMerAndPos(predVar).Value;
                keepPredictor = PosInGene(pos);
            }
            if (keepPredictor && targVar.Contains("@"))
            {
                int pos = (int)Tabulate.GetMerAndPos(targVar).Value;
                keepTarget = PosInGene(pos);
            }


            return keepTarget && keepPredictor;

        }

        private bool PosInGene(int pos)
        {
            foreach (Tuple<int, int> geneRange in _genes)
            {
                if (pos >= geneRange.Item1 && pos <= geneRange.Item2 && (pos - geneRange.Item1) % 3 == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            // haven't done anything here.
            return false;
        }
    }

    public class KeepPeptide : KeepTest<Dictionary<string, string>>
    {
        public static readonly String Prefix = "KeepPeptide";
        public static readonly string ArgsDescriptor = "AAStart-AAStop";

        private readonly int _start, _stop;
        private KeepPeptide(string peptideRangeInAminoAcidSpace)
            : base(Prefix, peptideRangeInAminoAcidSpace)
        {
            string[] fields = peptideRangeInAminoAcidSpace.Split('-');
            try
            {
                _start = int.Parse(fields[0]);
                _stop = int.Parse(fields[1]);
            }
            catch
            {
                throw new FormatException(string.Format("Could not parse range {0}.", peptideRangeInAminoAcidSpace));
            }
        }

        new public static KeepPeptide GetInstance(string peptideRangeInAminoAcidSpace)
        {
            KeepPeptide keepGene = new KeepPeptide(peptideRangeInAminoAcidSpace);

            return keepGene;
        }

        public override bool Test(Dictionary<string, string> row)
        {
            bool keepTarget = true;
            bool keepPredictor = true;

            string targVar = row[Tabulate.TargetVariableColumnName];
            string predVar = row[Tabulate.PredictorVariableColumnName];

            if (targVar.Contains("@"))
            {
                int pos = (int)Tabulate.GetMerAndPos(targVar).Value;
                //int pos = int.Parse(targVar.Split('@')[1]);
                keepTarget = pos >= _start && pos <= _stop;
            }
            if (predVar.Contains("@"))
            {
                int pos = (int)Tabulate.GetMerAndPos(predVar).Value;
                //int pos = int.Parse(predVar.Split('@')[1]);
                keepPredictor = pos >= _start && pos <= _stop;
            }

            return keepTarget && keepPredictor;

        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            // haven't done anything here.
            return false;
        }

    }

    //public class KeepSpecificGenes : Or<Dictionary<string, string>>
    //{
    //	new public readonly static string Prefix = "KeepSpecificGenes";
    //	new public readonly static string ArgsDescriptor = "DNAStart1-DNAStop1[;DNAStart2-DNAStop2;...]";

    //	new public static KeepTest<Dictionary<string, string>> GetInstance(string geneRanges)
    //	{
    //		List<KeepTest<Dictionary<string, string>>> geneList = new List<KeepTest<Dictionary<string, string>>>();

    //		string[] genes = geneRanges.Split(',');
    //		foreach (string gene in genes)
    //		{
    //			geneList.Add(KeepGene.GetInstance(gene));
    //		}

    //		return Or<Dictionary<string, string>>.GetInstance(geneList);

    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		throw new Exception("The method or operation is not implemented.");
    //	}

    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
    //	{
    //		throw new Exception("The method or operation is not implemented.");
    //	}

    //	public override string ToString()
    //	{
    //		return Prefix;
    //	}
    //}

    //public class KeepSpecificPeptides : Or<Dictionary<string, string>>
    //{
    //	new public readonly static string Prefix = "KeepSpecificPeptides";
    //	new public readonly static string ArgsDescriptor = "AAStart1-AAStop1[;AAStart2-AAStop2;...]";

    //	new public static KeepTest<Dictionary<string, string>> GetInstance(string geneRanges)
    //	{
    //		List<KeepTest<Dictionary<string, string>>> geneList = new List<KeepTest<Dictionary<string, string>>>();

    //		string[] genes = geneRanges.Split(',');
    //		foreach (string gene in genes)
    //		{
    //			geneList.Add(KeepPeptide.GetInstance(gene));
    //		}

    //		return Or<Dictionary<string, string>>.GetInstance(geneList);

    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		throw new Exception("The method or operation is not implemented.");
    //	}

    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
    //	{
    //		throw new Exception("The method or operation is not implemented.");
    //	}

    //	public override string ToString()
    //	{
    //		return Prefix;
    //	}
    //}

    public class KeepOneOfAAPair : KeepTest<Dictionary<string, string>>
    {
        public static readonly string Prefix = "KeepOneOfAAPair";

        private KeepOneOfAAPair() : base(Prefix, "") { }

        public static KeepTest<Dictionary<string, string>> GetInstance()
        {
            return new KeepOneOfAAPair();
        }

        public override bool Test(Dictionary<string, string> row)
        {
            string predictor = row[Tabulate.PredictorVariableColumnName];
            string target = row[Tabulate.TargetVariableColumnName];
            //int pos1 = int.Parse(predictor.Split('@')[1]);
            //int pos2 = int.Parse(target.Split('@')[1]);

            double pos1 = Tabulate.GetMerAndPos(predictor).Value;
            double pos2 = Tabulate.GetMerAndPos(target).Value;

            //return pos1 >= pos2;
            return pos1 <= pos2;
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }

    public class KeepAllButSamePosition : KeepTest<Dictionary<string, string>>
    {
        public static readonly string Prefix = "KeepAllButSamePosition";

        private KeepAllButSamePosition() : base(Prefix, "") { }

        public static KeepTest<Dictionary<string, string>> GetInstance()
        {
            return new KeepAllButSamePosition();
        }

        public override bool Test(Dictionary<string, string> row)
        {
            string predictor = row[Tabulate.PredictorVariableColumnName];
            string target = row[Tabulate.TargetVariableColumnName];
            //int pos1 = int.Parse(predictor.Split('@')[1]);
            //int pos2 = int.Parse(target.Split('@')[1]);

            KeyValuePair<string, double> merAndPos1, merAndPos2;

            if (Tabulate.TryGetMerAndPos(predictor, out merAndPos1) &&
                Tabulate.TryGetMerAndPos(target, out merAndPos2))
            {
                bool samePosition = merAndPos1.Value == merAndPos2.Value;
                return !samePosition;
            }
            return true;
            //return pos1 >= pos2;
            //return pos1 != pos2;
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }

    public class KeepAllButSameDeletion : KeepTest<Dictionary<string, string>>
    {
        public static readonly string Prefix = "KeepAllButSameDeletion";

        private KeepAllButSameDeletion() : base(Prefix, "") { }

        public static KeepTest<Dictionary<string, string>> GetInstance()
        {
            return new KeepAllButSameDeletion();
        }

        public override bool Test(Dictionary<string, string> row)
        {
            string predictor = row[Tabulate.PredictorVariableColumnName];
            string target = row[Tabulate.TargetVariableColumnName];
            //string[] predParts = predictor.Split('@');
            //string[] targParts = target.Split('@');

            //string predAA = predParts[0];
            //string targAA = targParts[0];
            //int pos1 = int.Parse(predParts[1]);
            //int pos2 = int.Parse(targParts[1]);

            KeyValuePair<string, double> pred = Tabulate.GetMerAndPos(predictor);
            KeyValuePair<string, double> targ = Tabulate.GetMerAndPos(target);

            //return pos1 >= pos2;
            return pred.Key != targ.Key ||
                pred.Key != "-" ||
                targ.Key != "-" ||
                Math.Abs(pred.Value - targ.Value) > 5;	// reject anything that's part of the same deletion. 5 is totally arbitrary.
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }

    public class KeepSpecificRow : KeepTest<Dictionary<string, string>>
    {
        public static readonly String Prefix = "KeepSpecificRow";
        public readonly static string ArgsDescriptor = "Predictor,Target";

        //private Dictionary<string, string> _testRow;
        private string _predictor, _target;

        private KeepSpecificRow(string predictor, string target)
            : base(Prefix, predictor + "," + target)
        {
            _predictor = predictor;
            _target = target;
        }

        /// <summary>
        /// Useful for quickly hard coding the row you want to keep.
        /// </summary>
        /// <returns></returns>
        //public static KeepSpecificRow GetInstance()
        //{
        //	return GetInstance(895, "W", 886, "H");
        //}

        new public static KeepSpecificRow GetInstance(string commaDelimitedRowDefn)
        {
            string[] fields = commaDelimitedRowDefn.Split(',');
            Helper.CheckCondition(fields.Length == 2);
            return GetInstance(fields[0], fields[1]);
        }

        public static KeepSpecificRow GetInstance(string predictor, string target)
        {
            return new KeepSpecificRow(predictor, target);
        }

        public override bool Test(Dictionary<string, string> row)
        {
            return row[Tabulate.PredictorVariableColumnName].Equals(_predictor, StringComparison.CurrentCultureIgnoreCase) &&
                row[Tabulate.TargetVariableColumnName].Equals(_target, StringComparison.CurrentCultureIgnoreCase);
        }



        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            return false;
        }
    }


    public class KeepSpecificRows : KeepTest<Dictionary<string, string>>
    {
        internal static readonly String Prefix = "KeepSpecificRows";

        private List<KeepSpecificRow> _testRows;

        private KeepSpecificRows() : base(null, null) { }

        //public static KeepSpecificRows GetInstance()
        //{
        //	List<KeepSpecificRow> keepList = new List<KeepSpecificRow>();
        //	//// this is the list of associations that have q < 0.01 in Gag Within gene.
        //	//keepList.Add(KeepSpecificRow.GetInstance(2134, "P", 2146, "L"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2134, "L", 2146, "P"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2146, "P", 2134, "L"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2146, "L", 2134, "P"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1957, "I", 1990, "L"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1990, "L", 1957, "I"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2215, "K", 2227, "D"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1579, "K", 1306, "A"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1957, "V", 1990, "I"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1306, "A", 1579, "K"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1591, "M", 1579, "K"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1579, "K", 1591, "M"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1591, "M", 1306, "A"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2206, "P", 2182, "F"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1591, "L", 1579, "R"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2227, "D", 2215, "K"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1306, "A", 1591, "M"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1471, "I", 1531, "T"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1579, "R", 1591, "L"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2206, "P", 2176, "F"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2206, "P", 2215, "K"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2239, "E", 2224, "E"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2224, "E", 2239, "E"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2215, "G", 2206, "G"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2206, "G", 2215, "G"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(1531, "T", 1471, "I"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2176, "Q", 2170, "Q"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2170, "Q", 2176, "Q"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2212, "P", 2185, "E"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(2185, "E", 2212, "P"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(895, "W", 889, "I"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(904, "R", 889, "I"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(904, "R", 895, "W"));
        //	//keepList.Add(KeepSpecificRow.GetInstance(901, "C", 889, "I"));

        //	return GetInstance(keepList);
        //}

        new public static KeepSpecificRows GetInstance(string semiColonDelimitedRows)
        {
            string[] rowDefs = semiColonDelimitedRows.Split(';');
            Helper.CheckCondition(rowDefs.Length > 0);

            List<KeepSpecificRow> rows = new List<KeepSpecificRow>(rowDefs.Length);
            foreach (string row in rowDefs)
            {
                rows.Add(KeepSpecificRow.GetInstance(row));
            }
            return GetInstance(rows);
        }

        public static KeepSpecificRows GetInstance(List<KeepSpecificRow> rows)
        {
            KeepSpecificRows aKeepTest = new KeepSpecificRows();
            aKeepTest._testRows = rows;
            return aKeepTest;
        }


        public override bool Test(Dictionary<string, string> row)
        {
            foreach (KeepSpecificRow keepRowTest in _testRows)
            {
                if (keepRowTest.Test(row))
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeepSpecificRow row in _testRows)
            {
                string s = row.ToString().Substring(KeepSpecificRow.Prefix.Length);
                sb.Append(sb.Length == 0 ? Prefix : ";");
                sb.Append(s);
            }
            return sb.ToString();
        }
        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            return false;
        }
    }

    public class KeepNonTrivialRows : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepNonTrivial";

        private KeepNonTrivialRows() : base(Prefix, "") { }
        public static KeepNonTrivialRows GetInstance()
        {
            return new KeepNonTrivialRows();
        }

        public override bool Test(Dictionary<string, string> row)
        {
            if (int.Parse(row[Tabulate.PredictorFalseNameCountColumnName]) == 0 ||
                int.Parse(row[Tabulate.PredictorTrueNameCountColumnName]) == 0)
            {
                return false;
            }
            if (row.ContainsKey(Tabulate.TargetFalseNameCountColumnName) &&
                (int.Parse(row[Tabulate.TargetFalseNameCountColumnName]) == 0 ||
                int.Parse(row[Tabulate.TargetTrueNameCountColumnName]) == 0))
            {
                return false;
            }
            return true;
        }


        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class KeepMinMarginalCount : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepMinMarginalCount";
        public readonly static string ArgsDescriptor = "N";
        private readonly double _minCount;

        public KeepMinMarginalCount(double minCount)
            : base(Prefix, minCount.ToString())
        {
            _minCount = minCount;
        }

        new public static KeepMinMarginalCount GetInstance(string minCount)
        {
            return new KeepMinMarginalCount(double.Parse(minCount));
        }

        public override bool Test(Dictionary<string, string> row)
        {
            int tt = int.Parse(row["TT"]);
            int tf = int.Parse(row["TF"]);
            int ft = int.Parse(row["FT"]);
            int ff = int.Parse(row["FF"]);

            int predCount = tt + tf;
            int targCount = tt + ft;
            int n = tt + tf + ft + ff;

            return
                predCount >= _minCount &&
                targCount >= _minCount &&
                n - predCount >= _minCount &&
                n - targCount >= _minCount;
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new NotImplementedException();
        }
    }

    public class KeepContingencyTableMinCount : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepContingencyTableMinCount";
        public readonly static string ArgsDescriptor = "N";

        private readonly double _minCount;

        private KeepContingencyTableMinCount(double minCount)
            : base(Prefix, minCount.ToString())
        {
            _minCount = minCount;
        }

        new public static KeepContingencyTableMinCount GetInstance(string minCount)
        {
            return GetInstance(double.Parse(minCount));
        }

        public static KeepContingencyTableMinCount GetInstance(double minCount)
        {
            KeepContingencyTableMinCount keepTest = new KeepContingencyTableMinCount(minCount);
            return keepTest;
        }

        //public override bool Test(Dictionary<string, string> row)
        //{
        //	// !!! This code is taken from 
        //	int tt = int.Parse(row["TT"]);
        //	int tf = int.Parse(row["TF"]);
        //	int ft = int.Parse(row["FT"]);
        //	int ff = int.Parse(row["FF"]);

        //	int realMinCount = Math.Min(tt, Math.Min(tf, Math.Min(ft, ff)));

        //	double sum = tt + tf + ft + ff;
        //	double v1TrueP = (tt + tf) ;
        //	double v2TrueP = (tt + ft) ;
        //	double v1FalseP = (ff + ft);
        //	double v2FalseP = (ff + tf);

        //	double expectedMinCount =  Math.Min(v1TrueP * v2TrueP / sum,
        //							   Math.Min(v1TrueP * v2FalseP / sum,
        //							   Math.Min(v1FalseP * v2TrueP / sum,
        //										v1FalseP * v2FalseP / sum)));

        //	double minCount = Math.Max(realMinCount, expectedMinCount);

        //	return minCount >= _minCount;
        //}

        public override bool Test(Dictionary<string, string> row)
        {
            int tt = int.Parse(row["TT"]);
            int tf = int.Parse(row["TF"]);
            int ft = int.Parse(row["FT"]);
            int ff = int.Parse(row["FF"]);

            double minCount = SpecialFunctions.FishersMinimax(tt, tf, ft, ff);


            //double sum = tt + tf + ft + ff;
            //double v1TrueP = (tt + tf);
            //double v2TrueP = (tt + ft);
            //double v1FalseP = (ff + ft);
            //double v2FalseP = (ff + tf);

            //double ett = v1TrueP * v2TrueP / sum;
            //double etf = v1TrueP * v2FalseP / sum;
            //double eft = v1FalseP * v2TrueP / sum;
            //double eff = v1FalseP * v2FalseP / sum;

            //double minCount = Math.Min(Math.Max(tt, ett),
            //				  Math.Min(Math.Max(tf, etf),
            //				  Math.Min(Math.Max(ft, eft),
            //						   Math.Max(ff, eff))));
            //int realMinCount = Math.Min(tt, Math.Min(tf, Math.Min(ft, ff)));

            //double sum = tt + tf + ft + ff;
            //double v1TrueP = (tt + tf);
            //double v2TrueP = (tt + ft);
            //double v1FalseP = (ff + ft);
            //double v2FalseP = (ff + tf);

            //double expectedMinCount = Math.Min(v1TrueP * v2TrueP / sum,
            //						   Math.Min(v1TrueP * v2FalseP / sum,
            //						   Math.Min(v1FalseP * v2TrueP / sum,
            //									v1FalseP * v2FalseP / sum)));

            //double minCount = Math.Max(realMinCount, expectedMinCount);

            return minCount >= _minCount;
        }



        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    //public class KeepNonRare : KeepTest<Dictionary<string, string>>
    //{
    //	public const string Prefix = "KeepNonRare";
    //	public readonly static string ArgsDescriptor = "N";

    //	private int _minTrueCount;


    //	new public static KeepNonRare GetInstance(string minCount)
    //	{
    //		KeepNonRare keepTest = new KeepNonRare();
    //		keepTest._minTrueCount = int.Parse(minCount);
    //		return keepTest;
    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		if (row.ContainsKey("TT"))
    //		{
    //			int[] fisherCounts = new int[4];
    //			fisherCounts[0] = int.Parse(row["TT"]);
    //			fisherCounts[1] = int.Parse(row["TF"]);
    //			fisherCounts[2] = int.Parse(row["FT"]);
    //			fisherCounts[3] = int.Parse(row["FF"]);

    //			//test that each variable's false and true counts are at least _minTrueCount. Use
    //			// fisher count to account for missing data in the other variable.
    //			if (fisherCounts[0] + fisherCounts[1] < _minTrueCount) return false; // is first true enough?
    //			if (fisherCounts[0] + fisherCounts[2] < _minTrueCount) return false; // is second true enough?
    //			if (fisherCounts[1] + fisherCounts[3] < _minTrueCount) return false; // is second false enough?
    //			if (fisherCounts[2] + fisherCounts[3] < _minTrueCount) return false; // is first false enough?
    //			return true;
    //		}
    //		else
    //		{
    //			bool keepPred = true;
    //			bool keepTarg = true;
    //			bool keepGlobal = !row.ContainsKey(Tabulate.GlobalNonMissingCountColumnName) ? true : int.Parse(row[Tabulate.GlobalNonMissingCountColumnName]) >= _minTrueCount;
    //			if (row.ContainsKey(Tabulate.PredictorTrueNameCountColumnName))
    //			{
    //				keepPred =
    //					int.Parse(row[Tabulate.PredictorFalseNameCountColumnName]) >= _minTrueCount &&
    //					int.Parse(row[Tabulate.PredictorTrueNameCountColumnName]) >= _minTrueCount;
    //			}
    //			if (row.ContainsKey(Tabulate.TargetTrueNameCountColumnName))
    //			{
    //				keepTarg =
    //					int.Parse(row[Tabulate.TargetFalseNameCountColumnName]) >= _minTrueCount &&
    //					int.Parse(row[Tabulate.TargetTrueNameCountColumnName]) >= _minTrueCount;
    //			}

    //			return keepGlobal && keepPred && keepTarg;
    //		}
    //	}

    //	public override string ToString()
    //	{
    //		return Prefix + _minTrueCount;
    //	}

    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
    //	{
    //		throw new Exception("The method or operation is not implemented.");
    //	}
    //}

    //public class KeepAllBut : KeepTest<Dictionary<string, string>>
    //{
    //	public readonly static string Prefix = "KeepAllBut";
    //	public readonly static string ArgsDescriptor = "PredOrTargToExclude1[,PredOrTargToExclude2,...]";

    //	private KeepOnly _keepOnly;

    //	new public static KeepAllBut GetInstance(string commaDelimitedRejectList)
    //	{
    //		KeepAllBut keepTest = new KeepAllBut();
    //		keepTest._keepOnly = KeepOnly.GetInstance(commaDelimitedRejectList);
    //		return keepTest;
    //	}

    //	public override bool Test(Dictionary<string, string> row)
    //	{
    //		return !_keepOnly.Test(row);
    //	}

    //	public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
    //	{
    //		throw new Exception("The method or operation is not implemented.");
    //	}
    //}

    public class KeepThesePositions : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepThesePositions";
        public readonly static string ArgsDescriptor = "PositionFile";

        private readonly Set<int> _positionsToKeep;

        protected KeepThesePositions(string filename)
            : base(Prefix, filename)
        {
            _positionsToKeep = new Set<int>();
            string header;
            foreach (Dictionary<string, string> row in SpecialFunctions.TabReader(filename, out header))
            {
                int pos = int.Parse(row["position"]);
                _positionsToKeep.AddNew(pos);
            }
        }

        new public static KeepThesePositions GetInstance(string positionFile)
        {
            return new KeepThesePositions(positionFile);
        }

        public override bool Test(Dictionary<string, string> row)
        {
            int pos = (int)Tabulate.GetMerAndPos(row[Tabulate.TargetVariableColumnName]).Value;

            return _positionsToKeep.Contains(pos);
        }



        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class KeepOnly : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepOnly";
        public readonly static string ArgsDescriptor = "PredOrTargToKeep1[,PredOrTargToKeep2,...]";

        private List<string> _keepList = new List<string>();

        private KeepOnly(string commaDelimitedKeepList)
            : base(Prefix, commaDelimitedKeepList)
        {
            _keepList = new List<string>(commaDelimitedKeepList.ToLower().Split(','));

        }

        new public static KeepOnly GetInstance(string commaDelimitedKeepList)
        {
            KeepOnly keepTest = new KeepOnly(commaDelimitedKeepList);

            return keepTest;
        }

        public override bool Test(Dictionary<string, string> row)
        {
            string predictorVariable = row[Tabulate.PredictorVariableColumnName].ToLower();
            string targetVariable = row[Tabulate.TargetVariableColumnName].ToLower();

            return (_keepList.Contains(predictorVariable) || _keepList.Contains(targetVariable));
        }


        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class KeepLength : KeepTest<Dictionary<string, string>>
    {
        internal readonly static string Prefix = "KeepLength";
        internal readonly static string ArgsDescriptor = "{predictor|target}varLen";

        int _length;
        bool _testPredictor;	// true = pred, false = targ

        private KeepLength(bool testPred, int length, string args)
            : base(Prefix, args)
        {
            _testPredictor = testPred;
            _length = length;
        }

        new public static KeepLength GetInstance(string varAndLength)
        {
            varAndLength = varAndLength.ToLower();
            string var;
            int len;

            if (varAndLength.StartsWith("predictor", StringComparison.CurrentCultureIgnoreCase))
            {
                var = "predictor";
            }
            else if (varAndLength.StartsWith("target", StringComparison.CurrentCultureIgnoreCase))
            {
                var = "target";
            }
            else
            {
                throw new ArgumentException(Prefix + " must be given either \"predictor\" or \"target\"");
            }
            if (!int.TryParse(varAndLength.Substring(var.Length), out len))
            {
                throw new ArgumentException(Prefix + " must be given integer length as last argument");
            }

            KeepLength keepTest = new KeepLength(var == "predictor", len, varAndLength);
            return keepTest;
        }

        public override bool Test(Dictionary<string, string> row)
        {
            string variable = _testPredictor ? row[Tabulate.PredictorVariableColumnName] : row[Tabulate.TargetVariableColumnName];
            return variable.Length == _length;
        }



        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }


    public class KeepPredictorTargetPairs : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepPredictorTargetPairs";
        public readonly static string ArgsDescriptor = "[FileName] blank causes enumeration of matched pairs";

        public readonly List<KeyValuePair<string, string>> PredTargPairsToKeep;

        private KeepPredictorTargetPairs(string keepFile)
            : base(Prefix, keepFile)
        {
            if (!string.IsNullOrEmpty(keepFile))
            {
                string header = Tabulate.PredictorVariableColumnName + "\t" + Tabulate.TargetVariableColumnName;
                PredTargPairsToKeep = new List<KeyValuePair<string, string>>();
                foreach (Dictionary<string, string> row in SpecialFunctions.TabReader(keepFile, header))
                {
                    KeyValuePair<string, string> predAndTarg = new KeyValuePair<string, string>(
                        row[Tabulate.PredictorVariableColumnName],
                        row[Tabulate.TargetVariableColumnName]);
                    PredTargPairsToKeep.Add(predAndTarg);
                }
            }
        }

        public KeepPredictorTargetPairs(List<KeyValuePair<string, string>> predTargPairsToKeep)
            : base(Prefix, "list")
        {
            PredTargPairsToKeep = new List<KeyValuePair<string, string>>(predTargPairsToKeep);
        }

        new public static KeepPredictorTargetPairs GetInstance(string fileWithKeepPairsInIt)
        {
            KeepPredictorTargetPairs keepTest = new KeepPredictorTargetPairs(fileWithKeepPairsInIt);

            return keepTest;
        }
        public override bool Test(Dictionary<string, string> row)
        {
            // this class serves as a flag for the WorkList. It always returns true, it just signals to the WorkList that 
            // enumeration should be done differently than it otherwise would be.
            return true;
        }


        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class KeepPredVarPrefix : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepPredVarPrefix";
        public readonly static string ArgsDescriptor = "PredPrefix1[,PredPrefix2,...]";

        private readonly string[] _possiblePrefixes;

        protected KeepPredVarPrefix(string[] possiblePrefixes, string prefixesAsString)
            : base(Prefix, prefixesAsString)
        {
            _possiblePrefixes = possiblePrefixes;
        }

        new public static KeepPredVarPrefix GetInstance(string prefixes)
        {
            KeepPredVarPrefix keepTest = new KeepPredVarPrefix(prefixes.Split(','), prefixes);
            return keepTest;
        }

        public override bool Test(Dictionary<string, string> row)
        {
            foreach (string prefix in _possiblePrefixes)
            {
                if (row[Tabulate.PredictorVariableColumnName].StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }


        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }


    public class KeepMHCII : KeepPredVarPrefix
    {
        new public readonly static string Prefix = "KeepMHC-II";

        protected KeepMHCII() : base(new string[] { "D" }, "") { }
        public static KeepMHCII GetInstance()
        {
            return new KeepMHCII();
        }

        public override string ToString()
        {
            return Prefix;
        }
    }

    public class KeepMHCI : KeepPredVarPrefix
    {
        new public readonly static string Prefix = "KeepMHC-I";

        protected KeepMHCI() : base(new string[] { "A", "B", "C" }, "") { }
        public static KeepMHCI GetInstance()
        {
            return new KeepMHCI();
        }

        public override string ToString()
        {
            return Prefix;
        }
    }

    public class ExcludeThesePositions : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "ExcludeThesePositions";
        public readonly static string ArgsDescriptor = "filenameWithPosInHeader";

        private Set<double> _posToExclude;

        protected ExcludeThesePositions(string filename)
            : base(Prefix, filename)
        {
            _posToExclude = new Set<double>();
            foreach (Dictionary<string, string> row in SpecialFunctions.TabReader(filename))
            {
                double pos = double.Parse(row["pos"]);
                _posToExclude.AddNewOrOld(pos);
            }
        }

        new public static ExcludeThesePositions GetInstance(string filename)
        {
            ExcludeThesePositions keepTest = new ExcludeThesePositions(filename);
            return keepTest;
        }

        public override bool Test(Dictionary<string, string> row)
        {
            string pred = row[Tabulate.PredictorVariableColumnName];
            string targ = row[Tabulate.TargetVariableColumnName];

            KeyValuePair<string, double> predMerAndPos, targMerAndPos;

            bool keepPred = !(Tabulate.TryGetMerAndPos(pred, out predMerAndPos) && _posToExclude.Contains(predMerAndPos.Value));
            bool keepTarg = !(Tabulate.TryGetMerAndPos(targ, out targMerAndPos) && _posToExclude.Contains(targMerAndPos.Value));

            return keepPred && keepTarg;
        }



        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class ExcludeTheseContractPositions : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "ExcludeTheseContractPositions";
        public readonly static string ArgsDescriptor = "filenameWithPosInHeader";

        private Set<string> _posAndPossibleCharToExclude;

        protected ExcludeTheseContractPositions(string filename)
            : base(Prefix, filename)
        {
            _posAndPossibleCharToExclude = new Set<string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (Dictionary<string, string> row in SpecialFunctions.TabReader(filename))
            {
                _posAndPossibleCharToExclude.AddNewOrOld(row["pos"]);
            }
        }

        new public static ExcludeTheseContractPositions GetInstance(string filename)
        {
            ExcludeTheseContractPositions keepTest = new ExcludeTheseContractPositions(filename);
            return keepTest;
        }

        public override bool Test(Dictionary<string, string> row)
        {
            return KeepVar(row[Tabulate.TargetVariableColumnName]) && KeepVar(row[Tabulate.PredictorVariableColumnName]);
        }

        private bool KeepVar(string var)
        {
            string[] fields = var.Split('@');
            return fields.Length != 4 || !_posAndPossibleCharToExclude.Contains(fields[2]);

        }



        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class KeepAa2AaOnly : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepAa2AaOnly";

        public KeepAa2AaOnly() : base(Prefix, "") { }

        public static KeepAa2AaOnly GetInstance()
        {
            return new KeepAa2AaOnly();
        }

        public override bool Test(Dictionary<string, string> row)
        {
            return row[Tabulate.PredictorVariableColumnName].IndexOf('@') > 0 &&
                    row[Tabulate.TargetVariableColumnName].IndexOf('@') > 0;
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class KeepAminoAcid : KeepTest<string>
    {
        public readonly static string Prefix = "KeepAminoAcid";

        public KeepAminoAcid() : base(Prefix, "") { }

        public static KeepAminoAcid GetInstance()
        {
            return new KeepAminoAcid();
        }

        public override bool Test(string row)
        {
            KeyValuePair<string, double> merAndPos;
            return Tabulate.TryGetMerAndPos(row, out merAndPos);
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<string> keepTestNew)
        {
            throw new NotImplementedException();
        }
    }

    public class KeepSameProteinOnly : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepSameProteinOnly";

        private KeepSameProteinOnly() : base(Prefix, "") { }

        public static KeepSameProteinOnly GetInstance()
        {
            return new KeepSameProteinOnly();
        }

        public override bool Test(Dictionary<string, string> row)
        {
            string[] predFields = row[Tabulate.PredictorVariableColumnName].Split(':');
            string[] targFields = row[Tabulate.TargetVariableColumnName].Split(':');

            return (predFields.Length == 4 && targFields.Length == 4) &&
                predFields[1].Split('_')[0].Equals(targFields[1].Split('_')[0], StringComparison.CurrentCultureIgnoreCase);
        }


        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class KeepOrder : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepOrder";
        public readonly static string ArgsDescriptor = "filenameWithListOfPredsAndTargsInOrder";

        Dictionary<string, int> _nameToPos;

        private KeepOrder(string filename)
            : base(Prefix, filename)
        {
            _nameToPos = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
            string line;
            using (TextReader reader = File.OpenText(filename))
            {
                int idx = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    _nameToPos[line.Trim()] = idx++;
                }
            }
        }

        new public static KeepOrder GetInstance(string filename)
        {
            return new KeepOrder(filename);
        }

        public override bool Test(Dictionary<string, string> row)
        {
            string pred = row[Tabulate.PredictorVariableColumnName];
            string targ = row[Tabulate.TargetVariableColumnName];

            int predPos, targPos;
            if (!_nameToPos.TryGetValue(pred, out predPos) || !_nameToPos.TryGetValue(targ, out targPos))
                //return true;
                throw new ArgumentException(string.Format("{0} or {1} is missing from the ordering file.", pred, targ));

            return predPos < targPos;
        }


        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class KeepHlaInEpitopes : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepHlaInEpitopes";
        public readonly static string ArgsDescriptor = "filenameWithEpitopeDefinitions,protein";

        Dictionary<string, RangeCollection> _hlaToRanges;

        private KeepHlaInEpitopes(string filename, string protein)
            : base(Prefix, filename + "," + protein)
        {
            _hlaToRanges = new Dictionary<string, RangeCollection>();

            var hlaLines = MBT.Escience.FileUtils.ReadDelimitedFile(filename, new { HlaList = new List<string>(), Protein = "", Start = -1, Stop = -1 }, true, '\t')
                            .Where(line => line.Protein.Equals(protein, StringComparison.InvariantCultureIgnoreCase))
                            .SelectMany(line => line.HlaList.Select(hla => new { Hla = hla, Protein = line.Protein, Start = line.Start, Stop = line.Stop }));

            List<string> loci = new List<string> { "A", "B", "C" };

            hlaLines.ForEach(line =>
                Helper.CheckCondition<ParseException>(line.Hla.Length >= 3 && loci.Contains(char.ToUpper(line.Hla[0]).ToString()), "Invalid Hla: {0}", line.Hla));


            var resultAsList = from line in hlaLines.Distinct()
                               where line.Start > 0 && line.Stop > 0
                               let hla = line.Hla.Substring(0, 3)
                               group line by hla into g
                               select new { Hla = g.Key, Range = g.Aggregate(new RangeCollection(), (range, line) => { range.AddRange(line.Start, line.Stop); return range; }) };

            _hlaToRanges = resultAsList.ToDictionary(item => item.Hla, item => item.Range);

        }

        new public static KeepHlaInEpitopes GetInstance(string filenameCommaProtein)
        {
            if (filenameCommaProtein.StartsWith("("))
                filenameCommaProtein = filenameCommaProtein.Substring(1, filenameCommaProtein.Length - 2);

            var fields = filenameCommaProtein.Split(',');
            return new KeepHlaInEpitopes(fields[0], fields[1]);
        }

        public override bool Test(Dictionary<string, string> row)
        {
            string predictor = row[Tabulate.PredictorVariableColumnName];
            KeyValuePair<string, double> merAndPos = Tabulate.GetMerAndPos(row[Tabulate.TargetVariableColumnName]);

            return _hlaToRanges.ContainsKey(predictor) && _hlaToRanges[predictor].Contains((int)merAndPos.Value);
        }


        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class KeepMinFETPValue : KeepTest<Dictionary<string, string>>
    {
        public readonly static string Prefix = "KeepMinFETPValue";
        public readonly static string ArgsDescriptor = "maximumMinimumAchievableFETPValue";

        [Parse(ParseAction.Required)]
        public double MinPValue;

        public KeepMinFETPValue() : base(Prefix, "") { }

        new public static KeepMinFETPValue GetInstance(string args)
        {
            Console.WriteLine(args);
            return ConstructorArguments.Construct<KeepMinFETPValue>(args);
        }

        public override bool Test(Dictionary<string, string> row)
        {
            int tt = int.Parse(row["TT"]);
            int tf = int.Parse(row["TF"]);
            int ft = int.Parse(row["FT"]);
            int ff = int.Parse(row["FF"]);

            double minP = SpecialFunctions.MinFisherExactTestPValue(tt, tf, ft, ff);
            return minP < MinPValue;
        }

        public override bool IsCompatibleWithNewKeepTest(KeepTest<Dictionary<string, string>> keepTestNew)
        {
            throw new NotImplementedException();
        }
    }

}
