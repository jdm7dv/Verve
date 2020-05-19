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
namespace SequenceAssembler
{
    #region -- Using Directives --

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    #endregion -- Using Directives --

    /// <summary>
    /// AlphabetLabel inherits from Label class and will be used to display
    /// only Alphabets. 
    /// </summary>
    public class AlphabetLabel : Label
    {
        #region -- Constructor --

        /// <summary>
        /// Initializes a new instance of the AlphabetLabel class.
        /// </summary>
        public AlphabetLabel()
        {            
            this.FontFamily = new FontFamily("Courier New");
            this.FontSize = 11;
            this.Padding = new Thickness(5, 0, 5, 0);         
            this.Margin = new Thickness(0, 7, 0, 0);
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.FontWeight = FontWeights.Bold;
        }

        #endregion -- Constructor --
    }
}
