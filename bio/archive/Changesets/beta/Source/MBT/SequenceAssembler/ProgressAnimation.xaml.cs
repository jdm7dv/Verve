// -------------------------------------------------------------------------------------
// <copyright file="ProgressAnimation.xaml.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// This would contain the interaction logic for the animation 
// shown while importing files.
// </summary>
// -------------------------------------------------------------------------------------
namespace SequenceAssembler
{
    #region -- Using Directives --
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    #endregion

    /// <summary>
    /// Interaction logic for Animation.xaml
    /// </summary>
    public partial class ProgressAnimation
    {
        #region -- Constructor --
        /// <summary>
        /// Initializes the Import Animation
        /// </summary>
        public ProgressAnimation()
        {
            this.InitializeComponent();
        }
        #endregion
    }
}