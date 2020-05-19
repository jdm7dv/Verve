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
using System.IO;
using System.Linq;
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

using Microsoft.Win32;

using MBF;
using MBF.Algorithms.Translation;
using MBF.IO;
using MBF.IO.GenBank;
using MBF.IO.Fasta;
using MBF.Util;

using MBF.Controls;

namespace ReadGenerator
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private OpenFileDialog genBankFileDialog = new OpenFileDialog();
        private OpenFileDialog gffFileDialog = new OpenFileDialog();
        private OpenFileDialog fastaFileDialog = new OpenFileDialog();

        public TestWindow()
        {
            InitializeComponent();
            InitializeDataMembers();
        }

        // Reads a file location from a OpenFileDialog and places the resulting path into a specified TextBox
        private void BrowseFile(OpenFileDialog dialog, TextBox resultTextBox)
        {
            if (dialog.ShowDialog(this) == true)
            {
                resultTextBox.Text = dialog.FileName;
            }
        }

        // Parses a sequence and adds it to the displayed list

        private void ParseSequence(ISequenceParser parser, string filename)
        {
            IList<ISequence> parsed = parser.Parse(filename);
            ListBox sequenceList = (ListBox)FindName("SequencesListBox");
            foreach (ISequence seq in parsed)
            {
                sequenceList.Items.Add(seq);
            }
        }

        private void InitializeDataMembers()
        {
            /*
            genBankFileDialog.Filter = "*.gbk";
            gffFileDialog.Filter = "*.gff";
            fastaFileDialog.Filter = "*.fasta";
            */
        }

        #region Event Handlers

        private void GenBankBrowse_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)FindName("GenBankFile");
            BrowseFile(genBankFileDialog, tb);
        }

        private void GffBrowse_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)FindName("GffFile");
            BrowseFile(gffFileDialog, tb);
        }

        private void FastaBrowse_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)FindName("FastaFile");
            BrowseFile(fastaFileDialog, tb);
        }

        private void GenBankParse_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)FindName("GenBankFile");
            FileInfo fi = new FileInfo(tb.Text);
            ParseSequence(new GenBankParser(), fi.FullName);
        }

        private void GffParse_Click(object sender, RoutedEventArgs e)
        {
            /*
            TextBox tb = (TextBox)FindName("GffFile");
            FileInfo fi = new FileInfo(tb.Text);
            ParseSequence(new LegacyGffParser(fi.FullName));
             * */
        }

        private void FastaParse_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)FindName("FastaFile");
            FileInfo fi = new FileInfo(tb.Text);
            ParseSequence(new FastaParser(), fi.FullName);
        }
        #endregion

        private void SequencesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox source = (ListBox)sender;
            if (source.SelectedItem != null)
            {
                SequenceViewer viewer = (SequenceViewer)FindName("SequenceViewer");
                viewer.Sequence = (ISequence)source.SelectedItem;
            }
        }

        private void JaredTest_Click(object sender, RoutedEventArgs e)
        {
            Sequence rna = new Sequence(Alphabets.RNA, "AUGGCGCCGAUAAUGACGGUCCUUCCUUGA");
            ISequence protein = ProteinTranslation.Translate(rna);
            string rnaStr = rna.ToString();
            StringBuilder buff = new StringBuilder();
            foreach (AminoAcid aa in protein)
            {
                buff.Append(aa.ExtendedSymbol);
                buff.Append(' ');
            }
            string aaStr = buff.ToString();
        }
        
    }
}
