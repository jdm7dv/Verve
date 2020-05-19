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
using System.Diagnostics;
using Bio.Util;
using MBT.Escience;
using System;

namespace MBT.Escience.Biology
{
    public enum AAMatch
    {
        FALSE = 0,
        TRUE = 1,
        NA = 2
    }

    abstract public class AminoAcidSet
    {
        public AminoAcidSet()
        {
        }

        public abstract IList<string> Positives
        {
            get;
        }

        static public AminoAcidSet GetInstance(bool simple)
        {
            if (simple)
            {
                return new SimpleAminoAcidSet();
            }
            else
            {
                return new ComplexAminoAcidSet();
            }
        }

        abstract public AAMatch Match(string aminoAcid);

        //public AAMatch MatchWithIgnoreList(string aminoAcid, SimpleAminoAcidSet rgIgnorePatientsWithTheseAminoAcidsOrNull)
        //{
        //    if (AnyAminoAcidInCommon(rgIgnorePatientsWithTheseAminoAcidsOrNull))
        //    {
        //        return AAMatch.NA;
        //    }

        //    return Match(aminoAcid);
        //}

        public static AAMatch Match(string sAminoAcid, AminoAcidSet rgAminoAcid)
        {
            AAMatch aamatch;
            if (rgAminoAcid == null)
            {
                aamatch = AAMatch.NA;
            }
            else
            {
                aamatch = rgAminoAcid.Match(sAminoAcid);
            }
            return aamatch;
        }


        public bool AnyAminoAcidInCommon(SimpleAminoAcidSet aaCollection2)
        {
            foreach (string aminoAcid1 in Positives)
            {
                if (aaCollection2.Contains(aminoAcid1))
                {
                    return true;
                }
            }
            return false;
        }


    }

    public class ComplexAminoAcidSet : AminoAcidSet
    {
        public ComplexAminoAcidSet()
        {
            Table = new SortedList<string, bool>();
        }

        private SortedList<string, bool> Table;

        public void AddOrCheck(string aa, bool p)
        {
            Helper.CheckCondition(aa.Length != 1);
            if (Table.ContainsKey(aa))
            {
                Helper.CheckCondition(Table[aa] == p);
            }
            else
            {
                Table.Add(aa, p);
            }
        }

        //!!!should not have calculation on a property
        public override IList<string> Positives
        {
            get
            {
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, bool> aaAndVal in Table)
                {
                    if (aaAndVal.Value)
                    {
                        list.Add(aaAndVal.Key);
                    }
                }
                return list;
            }
        }

        public override AAMatch Match(string aminoAcid)
        {
            if (!Table.ContainsKey(aminoAcid))
            {
                return AAMatch.NA;
            }
            else
            {
                return Table[aminoAcid] ? AAMatch.TRUE : AAMatch.FALSE;
            }
        }


    }

    // Used when the class doesn't matter
    public struct Ignore
    {
        public static Ignore GetInstance()
        {
            return _singleton.Value;
        }
        static Lazy<Ignore> _singleton = new Lazy<Ignore>(() => new Ignore());

    }

    public class SimpleAminoAcidSet : AminoAcidSet
    {
        private SortedList<string, Ignore> Set;
        public SimpleAminoAcidSet()
        {
            Set = new SortedList<string, Ignore>();
        }

        static public SimpleAminoAcidSet GetInstance()
        {
            return new SimpleAminoAcidSet();
        }


        override public AAMatch Match(string aminoAcid)
        {
            Helper.CheckCondition(aminoAcid != "<none>"); //!!!const //!!! raise error
            Helper.CheckCondition(!aminoAcid.StartsWith("Not an amino acid:"));//!!!const //!!!raise error
            //Debug.Assert(sAminoAcid.Length == 3 || sAminoAcid == "STOP"); //!!!const


            if (this == null) //!!!const
            {
                return AAMatch.NA;
            }

            //If can't match then return false
            if (PositiveCount > 1)
            {
                return Contains(aminoAcid) ? AAMatch.NA : AAMatch.FALSE;
            }
            else
            {
                Debug.Assert(PositiveCount == 1); // real assert
                return (Positives[0] == aminoAcid) ? AAMatch.TRUE : AAMatch.FALSE;
            }
        }


        public string GetOneAminoAcidOrNull()
        {
            if (this == null || PositiveCount != 1)
            {
                return null;
            }

            return Positives[0];
        }


        static public AAMatch Match(string sAminoAcid, string sAminoAcidPatientHiv)
        {
            Helper.CheckCondition(sAminoAcid != "<none>"); //!!!const //!!! raise error
            Helper.CheckCondition(!sAminoAcid.StartsWith("Not an amino acid:"));//!!!const //!!!raise error
            //Debug.Assert(sAminoAcid.Length == 3 || sAminoAcid == "STOP"); //!!!const


            if (sAminoAcidPatientHiv == "<none>") //!!!const
            {
                return AAMatch.NA;
            }
            if (sAminoAcidPatientHiv == sAminoAcid)
            {
                // Being equal is a match only if they are not ambiguous
                if (sAminoAcid == "Ambiguous Amino Acid") //!!!const
                {
                    return AAMatch.FALSE;
                }
                else
                {
                    return AAMatch.TRUE;
                }
            }
            else
            {
                Debug.Assert(sAminoAcidPatientHiv != null); //real assert
                return AAMatch.FALSE;
            }
        }

        public override string ToString()
        {
            if (PositiveCount > 1)
            {
                return Set.Keys.StringJoin(",");
            }
            else
            {
                Helper.CheckCondition(PositiveCount == 1); //!!!raise error
                return Positives[0];
            }

        }



        override public IList<string> Positives
        {
            get
            {
                return Set.Keys;
            }
        }

        public void AddOrCheck(string sAminoAcid)
        {
            Helper.CheckCondition(sAminoAcid.Length != 1);
            Set[sAminoAcid] = Ignore.GetInstance();
        }

        public bool Contains(string sAminoAcid)
        {
            return Set.ContainsKey(sAminoAcid);
        }

        public int PositiveCount
        {
            get
            {
                return Set.Count;
            }
        }

        public bool AminoAcidSetEqual(SimpleAminoAcidSet aminoAcidCollection2)
        {
            if (PositiveCount != aminoAcidCollection2.PositiveCount)
            {
                return false;
            }

            foreach (string sAminoAcid1 in Positives)
            {
                if (!aminoAcidCollection2.Contains(sAminoAcid1))
                {
                    return false;
                }
            }
            return true;
        }


    }
}
