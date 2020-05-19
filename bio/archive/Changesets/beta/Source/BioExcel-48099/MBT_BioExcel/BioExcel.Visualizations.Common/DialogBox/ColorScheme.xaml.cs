// ------------------------------------------------------------------------------
// <copyright file="ColorScheme.xaml.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// ColorScheme class is a user control class which stores the
// molecule type, the color corresponding to molecule type
// and a button to change the color scheme.
// </summary>
// ------------------------------------------------------------------------------

namespace BioExcel.Visualizations.Common
{
    #region -- Using Directives --

    using System.Windows.Controls;
    using System.Windows.Media;

    #endregion -- Using Directives --

    /// <summary>
    /// ColorScheme class is a user control class which stores the
    /// molecule type, the color corresponding to molecule type
    /// and a button to change the color scheme.
    /// </summary>
    public partial class ColorScheme : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ColorScheme class.
        /// </summary>
        public ColorScheme()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Gets or sets the Molecule symbol in a label control.
        /// </summary>
        public string MoleculeLabel
        {
            get { return this.colorDropDown.Text; }
            set { this.colorDropDown.Text = value; }
        }

        /// <summary>
        /// Gets or sets the color chosen by the user for Molecule label.
        /// </summary>
        public Color ChosenColor
        {
            get { return this.colorDropDown.SelectedColor; }
            set { this.colorDropDown.SelectedColor = value; }
        }
    }
}
