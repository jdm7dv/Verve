// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Windows.Forms;

namespace MBF.TemplateWizard
{
    /// <summary>
    /// This is a welcome screen of MBF Console Application wizard.
    /// </summary>
    public partial class WelcomeScreen : UserControl, IWizardScreen
    {
        /// <summary>
        /// Main header of the wizard, when this screen is shown
        /// </summary>
        public string MainHeader { get { return Properties.Resources.WelcomeScreenMainHeader; } }

        /// <summary>
        /// Sub header of the wizard, when this screen is shown
        /// </summary>
        public string SubHeader { get { return string.Empty; } }

        /// <summary>
        /// Initializes a new instance of the WelcomeScreen class.
        /// </summary>
        public WelcomeScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This screen has nothing to valiate, but implementing this from IWizard interface.
        /// </summary>
        /// <returns>Always true.</returns>
        public bool ValidateScreen()
        {
            return true;
        }
    }
}
