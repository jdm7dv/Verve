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
using Bio;

namespace Tools.VennDiagram
{
    public class SequenceRangeGroupingMetrics
    {
        public long groups;
        public long ranges;
        public long bases;
        public SequenceRangeGroupingMetrics()
        {
            groups = 0L;
            ranges = 0L;
            bases = 0L;
        }

        public SequenceRangeGroupingMetrics(SequenceRangeGrouping srg)
        {
            ComputeSequenceRangeGroupingMetrics(srg);
        }

        public void ComputeSequenceRangeGroupingMetrics(SequenceRangeGrouping srg)
        {
            groups = 0L;
            ranges = 0L;
            bases = 0L;

            foreach (string id in srg.GroupIDs)
            {
                ++groups;
                ranges += srg.GetGroup(id).Count;
                foreach (SequenceRange sr in srg.GetGroup(id))
                {
                    bases += sr.Length;
                }
            }
            return;
        }
    }
}
