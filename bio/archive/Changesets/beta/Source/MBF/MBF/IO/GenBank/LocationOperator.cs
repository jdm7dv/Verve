// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.IO.GenBank
{
    /// <summary>
    /// Enum for location operators.
    /// </summary>
    public enum LocationOperator
    {
        /// <summary>
        /// No Operator.
        /// </summary>
        None,

        /// <summary>
        /// Complement Operator.
        /// </summary>
        Complement,

        /// <summary>
        /// Join Operator.
        /// </summary>
        Join,

        /// <summary>
        /// Order Operator.
        /// </summary>
        Order,

        /// <summary>
        /// Bond Operator.
        /// Found in protein files. 
        /// These generally are used to describe disulfide bonds.
        /// </summary>
        Bond
    }
}
