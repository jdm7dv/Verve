// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using Bio.Algorithms.Alignment;

namespace Bio.Algorithms.Assembly.Comparative
{
    /// <summary>
    /// Reads ambiguously placed due to genomic reads.
    /// This step requires mate pair information to resolve the ambiguity about placements of repeated sequences.
    /// </summary>
    public static class RepeatResolver
    {
        /// <summary>
        /// Reads ambiguously placed due to genomic reads.
        /// This step requires mate pair information to resolve the ambiguity about placements of repeated sequences.
        /// </summary>
        /// <param name="alignmentBetweenReferenceAndReads">Alignment between reference genome and reads.</param>
        /// <returns>List of DeltaAlignments after resolving repeating reads.</returns>
        public static List<DeltaAlignment> ResolveAmbiguity(IList<IEnumerable<DeltaAlignment>> alignmentBetweenReferenceAndReads)
        {
            if (alignmentBetweenReferenceAndReads == null)
            {
                throw new ArgumentNullException("alignmentBetweenReferenceAndReads");
            }

            List<DeltaAlignment> result = new List<DeltaAlignment>();
            List<IEnumerable<DeltaAlignment>> readDeltas = alignmentBetweenReferenceAndReads.ToList();

            // Process reads and add to result list.
            // Loop till all reads are processed
            while (readDeltas.Count > 0)
            {
                IEnumerable<DeltaAlignment> curReadDeltas = readDeltas[0];
                readDeltas.RemoveAt(0); // remove currently processing item from the list

                // If curReadDeltas has only one delta, then there are no repeats so add it to result
                // Or if any delta is a partial alignment, dont try to resolve, add all deltas to result
                if (curReadDeltas.Count() == 1 || curReadDeltas.Any(a =>
                {
                    return a.SecondSequenceEnd != a.QuerySequence.Count - 1;
                }))
                {
                    foreach (DeltaAlignment curDelta in curReadDeltas)
                    {
                        result.Add(curDelta);
                    }
                }
                else
                {
                    // Resolve repeats
                    DeltaAlignment firstDelta = curReadDeltas.ElementAt(0);
                    string[] readMetadata = firstDelta.QuerySequence.ID.Split('.', ':');

                    // If read is not having proper ID, ignore the read
                    if (readMetadata.Length != 3 || (readMetadata[1] != "F" && readMetadata[1] != "R"))
                    {
                        foreach (DeltaAlignment curDelta in curReadDeltas)
                        {
                            result.Add(curDelta);
                        }

                        continue;
                    }

                    // Find mate pair
                    IEnumerable<DeltaAlignment> mateDeltas = alignmentBetweenReferenceAndReads.FirstOrDefault(a =>
                    {
                        string[] matepairMetadata = a.ElementAt(0).QuerySequence.ID.Split('.', ':');
                        if (matepairMetadata.Length == 3 &&
                            matepairMetadata[0] == readMetadata[0] && matepairMetadata[2] == readMetadata[2] &&
                            matepairMetadata[1] == (readMetadata[1] == "F" ? "R" : "F"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });

                    // If mate pair not found, ignore current read
                    if (mateDeltas == null)
                    {
                        foreach (DeltaAlignment curDelta in curReadDeltas)
                        {
                            result.Add(curDelta);
                        }

                        continue;
                    }

                    // Resolve using distance method
                    List<DeltaAlignment> resolvedDeltas = ResolveRepeatUsingMatePair(curReadDeltas, mateDeltas);
                    if (resolvedDeltas != null)
                    {
                        readDeltas.Remove(mateDeltas);
                        result.AddRange(resolvedDeltas);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Resolve repeats between two sets of deltas coming from paired reads
        /// </summary>
        /// <param name="curReadDeltas">Deltas from a read</param>
        /// <param name="mateDeltas">Deltas from mate pair</param>
        /// <returns>Selected delta out of all given deltas</returns>
        private static List<DeltaAlignment> ResolveRepeatUsingMatePair(IEnumerable<DeltaAlignment> curReadDeltas, IEnumerable<DeltaAlignment> mateDeltas)
        {
            // Check if all mate pairs are completly aligned, else return null (cannot resolve)
            if (mateDeltas.Any(a =>
                {
                    return a.SecondSequenceEnd != a.QuerySequence.Count - 1;
                }))
            {
                return null;
            }

            // Get clone library information
            string libraryName = curReadDeltas.ElementAt(0).QuerySequence.ID.Split(':')[1];
            CloneLibraryInformation libraryInfo = CloneLibrary.Instance.GetLibraryInformation(libraryName);
            float mean = libraryInfo.MeanLengthOfInsert;
            float stdDeviation = libraryInfo.StandardDeviationOfInsert;

            // Find delta with a matching distance.
            foreach (DeltaAlignment pair1 in curReadDeltas)
            {
                foreach (DeltaAlignment pair2 in mateDeltas)
                {
                    long distance = Math.Abs(pair1.FirstSequenceStart - pair2.FirstSequenceEnd);

                    // Find delta with matching distance.
                    if (distance - mean <= stdDeviation)
                    {
                        List<DeltaAlignment> resolvedDeltas = new List<DeltaAlignment>(2);

                        resolvedDeltas.Add(pair1);
                        resolvedDeltas.Add(pair2);

                        return resolvedDeltas;
                    }
                }
            }

            return null;
        }
    }
}
