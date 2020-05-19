// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Algorithms.Alignment
{
    /// <summary>
    /// This class extends PairwiseAlignmentAttributes and adds MUMmer specific attributes
    /// required to run the MUMmer algorithm.
    /// </summary>
    public class MUMmerAttributes : PairwiseAlignmentAttributes
    {
        /// <summary>
        /// Describes the Minimal length Maximal Unique Match parameter
        /// </summary>
        public const string LengthOfMUM = "LENGTHOFMUM";

        /// <summary>
        /// Initializes a new instance of the MUMmerAttributes class.
        /// </summary>
        public MUMmerAttributes()
        {
            AlignmentInfo alignmentAttribute = new AlignmentInfo(
                    Properties.Resource.LENGTH_OF_MUM_NAME,
                    Properties.Resource.LENGTH_OF_MUM_DESCRIPTION,
                    true,
                    "20",
                    AlignmentInfo.IntType,
                    null);
            Attributes.Add(LengthOfMUM, alignmentAttribute);
        }
    }
}