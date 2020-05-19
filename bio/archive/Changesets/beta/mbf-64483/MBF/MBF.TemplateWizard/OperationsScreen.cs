// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MBF.TemplateWizard
{
    /// <summary>
    /// This wizard screen will show a list of available operations to choose from.
    /// </summary>
    public partial class OperationsScreen : UserControl, IWizardScreen
    {
        /// <summary>
        /// Registry key location of MBI versions
        /// </summary>
        private const string MBIRegistryPath = @"SOFTWARE\Microsoft\MBI\";

        /// <summary>
        /// Registry key location of MBF Installation path
        /// </summary>
        private const string MBFRegistryPath = @"\MBF\";

        /// <summary>
        /// Registry key name of MBF Installation path
        /// </summary>
        private const string MBFInstalltionPathKey = "InstallationPath";

        /// <summary>
        /// Main header of the wizard, when this screen is shown
        /// </summary>
        public string MainHeader { get { return Properties.Resources.OperationsMainHeader; } }

        /// <summary>
        /// Sub header of the wizard, when this screen is shown
        /// </summary>
        public string SubHeader { get { return Properties.Resources.OperationsSubHeader; } }

        /// <summary>
        /// List of selected operations
        /// </summary>
        private List<string> selectedTags = new List<string>();

        /// <summary>
        /// List of selected operations exposed as a readonly collection.
        /// </summary>
        public ReadOnlyCollection<string> SnippetTags { get; private set; }

        public string MBFAssemblyPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the OperationsScreen class.
        /// </summary>
        public OperationsScreen()
        {
            InitializeComponent();
            SnippetTags = selectedTags.AsReadOnly();

            // Check for MBF framework
            RegistryKey mbfPathKey = Registry.LocalMachine.OpenSubKey(MBIRegistryPath);
            
            if (mbfPathKey == null || mbfPathKey.SubKeyCount == 0)
            {
                // Inform user, and proceed.
                MessageBox.Show(Properties.Resources.MBFMissing,
                Properties.Resources.Caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

                MBFAssemblyPath = string.Empty;

                versionSelector.Enabled = false;
                versionSelectorLabel.Enabled = false;
            }
            else
            {
                // Load available version numbers to the version selector combo
                foreach (string version in mbfPathKey.GetSubKeyNames())
                {
                    versionSelector.Items.Add(version);
                }
                versionSelector.SelectedIndex = 0;

                // initialize the path so that it points to the first version in the list
                mbfPathKey = Registry.LocalMachine.OpenSubKey(MBIRegistryPath + versionSelector.Text + MBFRegistryPath);
                if (mbfPathKey != null)
                {
                    MBFAssemblyPath = mbfPathKey.GetValue(MBFInstalltionPathKey).ToString();
                } 
            }
        }

        /// <summary>
        /// Checks for selected choices and updates the SelectedTags list.
        /// </summary>
        /// <returns>True if validation completes successfully</returns>
        public bool ValidateScreen()
        {
            selectedTags.Clear();
            foreach(Control currentControl in operationsPanel.Controls)
            {
                if ((currentControl as CheckBox).Checked)
                {
                    selectedTags.Add(currentControl.Tag as string);
                }
            }

            SnippetTags = selectedTags.AsReadOnly();

            // Set the MBF assembly path
            RegistryKey mbfPathKey = Registry.LocalMachine.OpenSubKey(MBIRegistryPath + versionSelector.Text + MBFRegistryPath);
            if (mbfPathKey != null)
            {
                MBFAssemblyPath = mbfPathKey.GetValue(MBFInstalltionPathKey).ToString();
            }
                        
            return true;
        }
    }
}
