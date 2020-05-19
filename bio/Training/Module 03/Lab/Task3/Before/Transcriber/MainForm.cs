using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bio;
using Bio.IO;

namespace Transcriber
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// List of loaded sequences
        /// </summary>
        IList<ISequence> _sequences;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This method is called when you select the File | Exit option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This method is invoked when the user clicks the "File | Open" menu option.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display Open File Dialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Sequence File";
            ofd.Filter = "All Files|*.*";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // See if we can auto-detect the parser type.
                var filename = ofd.FileName;
                var parser = SequenceParsers.FindParserByFileName(filename);
                
                // Failed - prompt the user
                if (parser == null)
                {
                    SelectParserForm form = new SelectParserForm()
                    {
                        FileName = filename
                    };
                    if (form.ShowDialog() == DialogResult.Cancel)
                        return;
                    parser = form.SelectedParser;
                }

                // Load the sequences
                try
                {
                    _sequences = parser.Parse().ToList();
                    parser.Close();
                }
                catch (Exception ex)
                {
                    _sequences = null;
                    MessageBox.Show(ex.Message, "Failed to parse " +
                                    Path.GetFileName(filename));
                }
            }

            // Setup the UI with the sequences.
            LoadSequences();
        }

        /// <summary>
        /// This method is used to load the sequence data into the UI.
        /// </summary>
        private void LoadSequences()
        {
            cbSequences.Items.Clear();
            cbSequences.SelectedIndex = -1;

            if (_sequences != null)
            {
                foreach (var seq in _sequences)
                    cbSequences.Items.Add(seq.ID);
                cbSequences.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// This method is invoked when the user selects a new choice in the Sequence ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSequences_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Selected sequence has changed.  Reset the UI.
            if (cbSequences.SelectedIndex >= 0)
            {
                ISequence seq = _sequences[cbSequences.SelectedIndex];
                sequenceTypeText.Text = seq.Alphabet.Name;
                sourceSequenceText.Text = new string(
                    seq.Select(b => (char) b)
                       .Take(255)
                       .ToArray());
            }
            else
            {
                sequenceTypeText.Text = "N/A";
                sourceSequenceText.Text = string.Empty;
            }
        }
    }
}
