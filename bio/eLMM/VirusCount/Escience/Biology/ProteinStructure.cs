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
using MBT.Escience;
using Bio.Util;

namespace MBT.Escience.Biology
{
    public class ProteinStructure
    {
        List<Dictionary<int, Residue>> _models;

        private ProteinStructure(List<Dictionary<int, Residue>> models)
        {
            _models = models;
        }

        public static ProteinStructure GetInstance(string pdbFilename)
        {
            List<Dictionary<int, Residue>> residues = new List<Dictionary<int, Residue>>();

            string line;
            int currentModel = -1;
            bool onlyOneModel = false;
            using (TextReader reader = Bio.Util.FileUtils.OpenTextStripComments(pdbFilename))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("MODEL"))
                    {
                        Helper.CheckCondition(!onlyOneModel, "Thought we wouldn't see any of these for this particular file.");
                        currentModel = int.Parse(line.Substring(10, 4).Trim());
                        residues.Add(new Dictionary<int, Residue>());
                    }
                    if (line.StartsWith("ATOM"))
                    {
                        if (currentModel < 0)
                        {
                            currentModel = 1;
                            residues.Add(new Dictionary<int, Residue>());
                            onlyOneModel = true;
                        }
                        Atom atom = Atom.GetInstance(line);

                        if (!residues[currentModel - 1].ContainsKey(atom.Position))
                        {
                            residues[currentModel - 1].Add(atom.Position, Residue.GetInstance(atom.Position, atom.ResidueName));
                        }
                        residues[currentModel - 1][atom.Position].AddAtom(atom);
                    }
                }
            }

            return new ProteinStructure(residues);
        }

        public double DistanceBetweenResidues(int pos1, int pos2)
        {
            double dist;
            if (TryGetDistanceBetweenResidues(pos1, pos2, out dist))
            {
                return dist;
            }
            else
            {
                throw new ArgumentException(string.Format("Either {0} or {1} is out of range in the models.", pos1, pos2));
            }
        }

        public bool TryGetDistanceBetweenResidues(int pos1, int pos2, out double distance)
        {
            distance = double.MaxValue;
            foreach (Dictionary<int, Residue> model in _models)
            {
                if (model.ContainsKey(pos1) && model.ContainsKey(pos2))
                {
                    distance = Math.Min(distance, Residue.Distance(model[pos1], model[pos2]));
                }
            }
            return distance != double.MaxValue;
        }

        private class Residue
        {
            public readonly int Position;
            public readonly string Name;
            private readonly List<Atom> _atoms = new List<Atom>();

            private Residue(int pos, string res)
            {
                Position = pos;
                Name = res;
            }

            public static Residue GetInstance(int pos, string res)
            {
                return new Residue(pos, res);
            }

            public void AddAtom(Atom atom)
            {
                _atoms.Add(atom);
            }

            public static double Distance(Residue r1, Residue r2)
            {
                double minDist = double.MaxValue;
                foreach (Atom a1 in r1._atoms)
                {
                    foreach (Atom a2 in r2._atoms)
                    {
                        double dist = Atom.Distance(a1, a2);
                        minDist = Math.Min(dist, minDist);
                    }
                }
                return minDist;
            }


        }

        private class Atom
        {
            public readonly double X, Y, Z;
            public readonly string ResidueName;
            public readonly int Position;

            private Atom(int pos, string res, double x, double y, double z)
            {
                Position = pos;
                ResidueName = res;
                X = x;
                Y = y;
                Z = z;
            }

            public static Atom GetInstance(string pdbAtomLine)
            {
                string residue = pdbAtomLine.Substring(17, 3).Trim();
                int position = int.Parse(pdbAtomLine.Substring(22, 4).Trim());
                double x = double.Parse(pdbAtomLine.Substring(30, 8).Trim());
                double y = double.Parse(pdbAtomLine.Substring(38, 8).Trim());
                double z = double.Parse(pdbAtomLine.Substring(46, 8).Trim());
                return new Atom(position, residue, x, y, z);
            }

            public static double Distance(Atom a1, Atom a2)
            {
                return Math.Sqrt(Math.Pow(a1.X - a2.X, 2) + Math.Pow(a1.Y - a2.Y, 2) + Math.Pow(a1.Z - a2.Z, 2));
            }
        }
    }


}
