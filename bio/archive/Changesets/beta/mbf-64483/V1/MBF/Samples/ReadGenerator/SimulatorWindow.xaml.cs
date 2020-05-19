// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Win32;

namespace ReadGenerator
{
    /// <summary>
    /// Interaction logic for SimulatorWindow.xaml
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private SaveFileDialog saveFileDialog = new SaveFileDialog();

        private SimulatorController controller = new SimulatorController();

        /// <summary>
        /// Default constructor for the window
        /// </summary>
        public SimulatorWindow()
        {
            //InitializeComponent();
            InitData();
        }

        /// <summary>
        /// Currently held data settings for simulation runs.
        /// </summary>
        public SimulatorSettings Settings
        {
            get { return (SimulatorSettings)FindResource("settings"); }
        }

        // Updates UI related to the input sequence. Should be called any time that
        // sequence is changed
        private void UpdateInputSequenceInfo()
        {
            Label idBox = (Label)FindName("InputSequenceStatus");
            Label sizeBox = (Label)FindName("InputSequenceSize");

            if (controller.Sequence == null)
            {
                idBox.Content = FindResource("NotLoaded");
                sizeBox.Content = FindResource("NoBasePairCount");
            }
            else
            {
                idBox.Content = controller.Sequence.DisplayID;
                sizeBox.Content = string.Format((string)FindResource("BasePairCount"), controller.Sequence.Count);
            }
        }

        // Updates UI related to information that is known just before performing the simulation
        internal void UpdateSimulationStats(int sequenceCount, int fileCount)
        {
            Label status1 = (Label)FindName("OutputSequenceStatus1");

            status1.Content = string.Format((string)FindResource("OutputStats"), sequenceCount, fileCount);
        }

        // Updates UI related the the results of simulation
        internal void NotifySimulationComplete()
        {
            Label status2 = (Label)FindName("OutputSequenceStatus2");

            status2.Content = FindResource("FinishedOutput");
        }

        private void InitData()
        {
            
        }

        // Loads the input sequence from the currently selected input file
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = ((TextBox)FindName("InputFileBox")).Text;
            try
            {
                controller.ParseSequence(fileName);
                UpdateInputSequenceInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error parsing the input file: " + ex.Message, "Parsing Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Initiates the work of simulation
        private void SimulationButton_Click(object sender, RoutedEventArgs e)
        {
            if (controller.Sequence == null)
            {
                MessageBox.Show(this, "Please load a sequence before attempting simulation", "Simulation Not Ready", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            TextBox outputBox = (TextBox)FindName("OutputDirectoryBox");
            string fileName = outputBox.Text;
            if (String.IsNullOrEmpty(fileName))
            {
                string inputName = ((TextBox)FindName("InputFileBox")).Text;
                fileName = inputName.Substring(0, inputName.LastIndexOf("."));
                outputBox.Text = fileName + "_#.fa";
            }

            try
            {
                controller.DoSimulation(this, fileName, (SimulatorSettings)FindResource("settings"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Opens the file chooser dialog to select an input sequence file
        private void InputBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == true)
            {
                TextBox inputBox = (TextBox)FindName("InputFileBox");
                inputBox.Text = openFileDialog.FileName;
            }
        }

        // Opens the file chooser dialog to select an output sequence file
        private void OutputBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == true)
            {
                TextBox outputBox = (TextBox)FindName("OutputDirectoryBox");
                outputBox.Text = saveFileDialog.FileName;
            }
        }

        // Sets the settings data to one of the known default settings
        private void DefaultsButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            if (button.Name == "SettingDefaultSanger")
                Settings.SetDefaults(DefaultSettings.SangerDideoxy);
            if (button.Name == "SettingDefaultPyro")
                Settings.SetDefaults(DefaultSettings.PyroSequencing);
            if (button.Name == "SettingDefaultShort")
                Settings.SetDefaults(DefaultSettings.ShortRead);
        }
    }
}