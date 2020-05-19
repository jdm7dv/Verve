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
namespace BioExcel.Visualizations.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Interface that defines a contract for contract implementing selection
    /// dialog box.
    /// </summary>
    public interface ISelectionDialog
    {
        /// <summary>
        /// Gets or sets currently selected sequence in the list. 
        /// Used by selection helper and other classes to set the sequence address from outside.
        /// </summary>
        InputSequenceItem SelectedItem { get; set; }

        /// <summary>
        /// Displays the dialog (modal).
        /// </summary>
        /// <returns>Success flag</returns>
        bool? ShowDialog();
    }
}
