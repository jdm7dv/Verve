// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF
{
    public class SequenceRangeGroupingMetrics
    {
        public long cGroups;
        public long cRanges;
        public long cBases;
        public SequenceRangeGroupingMetrics()
        {
            cGroups = 0L;
            cRanges = 0L;
            cBases = 0L;
        }

        public SequenceRangeGroupingMetrics(SequenceRangeGrouping srg)
        {
            ComputeSequenceRangeGroupingMetrics(srg);
        }

        public void ComputeSequenceRangeGroupingMetrics(SequenceRangeGrouping srg)
        {
            cGroups = 0L;
            cRanges = 0L;
            cBases = 0L;

            foreach (string id in srg.GroupIDs)
            {
                ++cGroups;
                cRanges += srg.GetGroup(id).Count;
                foreach (SequenceRange sr in srg.GetGroup(id))
                {
                    cBases += sr.Length;
                }
            }
            return;
        }
    }
}
