// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBF.Algorithms.Assembly.PaDeNA.Scaffold
{
    /// <summary>
    /// Class calculates distance between contigs using mate pairs mapped to contigs.
    /// Reference: The Greedy Path-Merging Algorithm for contig Scaffolding: HUSON et al.
    /// </summary>
    public class DistanceCalculator : IDistanceCalculator
    {
        #region IDistanceEstimation Members

        /// <summary>
        /// Calculates distances between contigs.
        /// </summary>
        /// <param name="contigPairedReads">Input Contigs and mate pairs mappping.</param>
        public void CalculateDistance(ContigMatePairs contigPairedReads)
        {
            Parallel.ForEach(contigPairedReads, (KeyValuePair<ISequence, Dictionary
                <ISequence, IList<ValidMatePair>>> contigPairedRead) =>
            {
                CalculateInterContigDistance(contigPairedRead, contigPairedRead.Key.Count);
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculate distance between single pair of contigs.
        /// </summary>
        /// <param name="contigsPairedRead">Contig mate pairs map.</param>
        /// <param name="lengthOfContig">Length of forward contig.</param>
        private static void CalculateInterContigDistance(
            KeyValuePair<ISequence, Dictionary<ISequence, IList<ValidMatePair>>> contigsPairedRead,
            int lengthOfContig)
        {
            Parallel.ForEach(contigsPairedRead.Value, (KeyValuePair<ISequence,
                IList<ValidMatePair>> contigPairedRead) =>
            {
                Parallel.ForEach(contigPairedRead.Value, (ValidMatePair validPairedRead) =>
                {
                    CalculateDistance(validPairedRead, lengthOfContig);
                });

                EdgeBundling(contigPairedRead.Value);
                if (contigPairedRead.Value.Count > 1)
                {
                    CalculateWeigthedEdge(contigPairedRead.Value);
                }
            });
        }

        /// <summary>
        /// Calculates distance between contigs for each pair of Full Overlap.
        /// </summary>
        /// <param name="validPairedRead">Valid mate pairs which have full Overlap 
        /// and support a perticular orientation of contigs</param>
        /// <param name="length">Length of forward contig.</param>
        private static void CalculateDistance(ValidMatePair validPairedRead, int length)
        {
            //For reverse read sequence in forward direction.
            validPairedRead.DistanceBetweenContigs.Add(
                validPairedRead.PairedRead.MeanLengthOfLibrary - (length -
                validPairedRead.ForwardReadStartPosition.First())
                - (validPairedRead.ReverseReadStartPosition.First() + 1));
            validPairedRead.StandardDeviation.Add(validPairedRead.PairedRead.StandardDeviationOfLibrary);
            validPairedRead.Weight = 1;

            //For reverse read sequence in reverse complementary direction.
            validPairedRead.DistanceBetweenContigs.Add(
                validPairedRead.PairedRead.MeanLengthOfLibrary - (length -
                validPairedRead.ForwardReadStartPosition[0])
                - (validPairedRead.ReverseReadReverseComplementStartPosition[0] + 1));
            validPairedRead.StandardDeviation.Add(validPairedRead.PairedRead.StandardDeviationOfLibrary);
        }

        /// <summary>
        /// Bundles all valid mate pairs in single edge but considering ±3σ limit.
        /// </summary>
        /// <param name="contigPairedRead">List of Valid Paired Reads</param>
        private static void EdgeBundling(IList<ValidMatePair> contigPairedRead)
        {
            int index = 0;
            List<ValidMatePair> estimatedDistances;
            while (index < contigPairedRead.Count())
            {
                estimatedDistances = contigPairedRead.Where(distance =>
                    (contigPairedRead[index].DistanceBetweenContigs.First() - (3.0 *
                    contigPairedRead[index].StandardDeviation.First())
                    <= distance.DistanceBetweenContigs.First()) &&
                    distance.DistanceBetweenContigs[0] <=
                    (contigPairedRead[index].DistanceBetweenContigs.First() + (3.0 *
                    contigPairedRead[index].StandardDeviation.First()))).ToList();
                if (estimatedDistances.Count > 1)
                {
                    float p = (float)estimatedDistances.Sum(dist => dist.DistanceBetweenContigs[0]
                        / Math.Pow(dist.StandardDeviation[0], 2));
                    float q = (float)estimatedDistances.Sum(sd => 1 / Math.Pow(sd.StandardDeviation[0], 2));
                    ValidMatePair distance = new ValidMatePair();
                    distance.DistanceBetweenContigs.Add(p / q);
                    distance.StandardDeviation.Add((float)(1 / Math.Sqrt(q)));

                    p = (float)estimatedDistances.Sum(dist => dist.DistanceBetweenContigs[1]
                        / Math.Pow(dist.StandardDeviation[1], 2));
                    q = (float)estimatedDistances.Sum(sd => 1 / Math.Pow(sd.StandardDeviation[1], 2));

                    distance.DistanceBetweenContigs.Add(p / q);
                    distance.StandardDeviation.Add((float)(1 / Math.Sqrt(q)));
                    foreach (ValidMatePair est in estimatedDistances)
                    {
                        contigPairedRead.Remove(est);
                    }

                    distance.Weight = estimatedDistances.Count;
                    contigPairedRead.Add(distance);
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
        }

        /// <summary>
        /// Further estimates distances using weighted mean 
        /// and standard deviation by merging valid mate pairs.
        /// </summary>
        /// <param name="distances">List of valid mate pairs.</param>
        private static void CalculateWeigthedEdge(IList<ValidMatePair> distances)
        {
            ValidMatePair finalDistance = new ValidMatePair();
            finalDistance.DistanceBetweenContigs.Add(
                distances.Sum(distance => distance.DistanceBetweenContigs[0] * distance.Weight));
            finalDistance.StandardDeviation.Add(
                distances.Sum(distance => distance.StandardDeviation[0] * distance.Weight));
            finalDistance.Weight = distances.Sum(distance => distance.Weight);
            finalDistance.DistanceBetweenContigs[0] /= finalDistance.Weight;
            finalDistance.StandardDeviation[0] /= finalDistance.Weight;
            finalDistance.DistanceBetweenContigs.Add(
                distances.Sum(distance => distance.DistanceBetweenContigs[1] * distance.Weight));
            finalDistance.StandardDeviation.Add(
                distances.Sum(distance => distance.StandardDeviation[0] * distance.Weight));
            finalDistance.DistanceBetweenContigs[1] /= finalDistance.Weight;
            finalDistance.StandardDeviation[1] /= finalDistance.Weight;
            distances.Clear();
            distances.Add(finalDistance);
        }

        #endregion
    }
}
