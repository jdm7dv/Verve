// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.Linq;
using MBF.Algorithms.Assembly.PaDeNA;
using MBF.Algorithms.Assembly.PaDeNA.Scaffold;
using MBF.Algorithms.Assembly.Graph;
using MBF.Util.Logging;
using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Removes containing paths and merge overlapping paths.
    /// </summary>
    [TestFixture]
    public class PathPurgerTest : ParallelDeNovoAssembler
    {
        static PathPurgerTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Containing paths.
        /// </summary>
        [Test]
        public void PathPurger1()
        {
            const int kmerLength = 7;
            List<ISequence> sequences = new List<ISequence>();
            sequences.Add(new Sequence(Alphabets.DNA, "GATTCAAGGGCTGGGGG"));
            this.KmerLength = kmerLength;
            this.AddSequenceReads(sequences);
            this.CreateGraph();
            List<DeBruijnNode> contigs = this.Graph.Nodes.ToList();
            IList<ScaffoldPath> paths =
                new List<ScaffoldPath>();
            ScaffoldPath path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs)
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(2, 5))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(3, 5))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(6, 5))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(0, 11))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(7, 4))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(11, 0))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(2, 9))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(1, 10))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            PathPurger assembler = new PathPurger();
            assembler.PurgePath(paths);
            Assert.AreEqual(paths.Count, 1);
            Assert.IsTrue(Compare(paths.First(), contigs));
        }

        /// <summary>
        /// Overlapping paths.
        /// </summary>
        [Test]
        public void PathPurger2()
        {
            const int kmerLength = 7;
            List<ISequence> sequences = new List<ISequence>();
            sequences.Add(new Sequence(Alphabets.DNA, "GATTCAAGGGCTGGGGG"));
            this.KmerLength = kmerLength;
            this.AddSequenceReads(sequences);
            this.CreateGraph();
            List<DeBruijnNode> contigs = this.Graph.Nodes.ToList();
            IList<ScaffoldPath> paths =
                new List<ScaffoldPath>();
            ScaffoldPath path = new ScaffoldPath();

            foreach (DeBruijnNode node in contigs.GetRange(0, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(3, 5))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(6, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(3, 3))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(7, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(8, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(2, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(1, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(8, 3))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            PathPurger assembler = new PathPurger();
            assembler.PurgePath(paths);
            Assert.AreEqual(paths.Count, 1);
            Assert.IsTrue(Compare(paths.First(), contigs));
        }

        /// <summary>
        /// Containing and Overlapping paths.
        /// </summary>
        [Test]
        public void PathPurger3()
        {
            const int kmerLength = 7;
            List<ISequence> sequences = new List<ISequence>();
            sequences.Add(new Sequence(Alphabets.DNA, "GATTCAAGGGCTGGGGG"));
            this.KmerLength = kmerLength;
            this.AddSequenceReads(sequences);
            this.CreateGraph();
            List<DeBruijnNode> contigs = this.Graph.Nodes.ToList();
            IList<ScaffoldPath> paths =
                new List<ScaffoldPath>();
            ScaffoldPath path = new ScaffoldPath();
            foreach (DeBruijnNode node in ((List<DeBruijnNode>)contigs).GetRange(0, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(1, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(8, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(7, 2))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(7, 4))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(11, 0))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(2, 9))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs.GetRange(1, 10))
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);
            path = new ScaffoldPath();
            foreach (DeBruijnNode node in contigs)
            {
                path.Add(new KeyValuePair<DeBruijnNode, DeBruijnEdge>(node, null));
            }

            paths.Add(path);

            PathPurger assembler = new PathPurger();
            assembler.PurgePath(paths);
            Assert.AreEqual(paths.Count, 1);
            Assert.IsTrue(Compare(paths.First(), contigs));
        }

        private bool Compare(ScaffoldPath path, IList<DeBruijnNode> contig)
        {
            if (path.Count == contig.Count)
            {
                for (int index = 0; index < contig.Count; index++)
                {
                    if (path[index].Key != contig[index])
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
