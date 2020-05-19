using System;
using System.Windows.Forms;
using Bio.IO;

namespace Transcriber
{
    public partial class SelectParserForm : Form
    {
        public ISequenceParser SelectedParser { get; set; }

        public string FileName { get; set; }

        public SelectParserForm()
        {
            InitializeComponent();

            // Add each of the parsers to the ComboBox.
            foreach (var parser in SequenceParsers.All)
                cbParserList.Items.Add(parser);
            cbParserList.DisplayMember = "Name";

            // Select the first one in the list.
            cbParserList.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enable the OK button if a parser is selected.
            if (cbParserList.SelectedIndex >= 0)
                btnOK.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ISequenceParser foundParser = (ISequenceParser)cbParserList.SelectedItem;
            if (foundParser != null)
                SelectedParser = SequenceParsers.FindParserByName(FileName, foundParser.Name);
        }
    }
}
