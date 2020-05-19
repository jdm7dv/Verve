using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Bio;
using Bio.IO;
using Bio.Algorithms.Translation;

namespace Transcriber
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// List of loaded sequences
        /// </summary>
        IList<ISequence> _sequences;

        /// <summary>
        /// Transcribed sequence (can be null).
        /// </summary>
        ISequence _transcribedSequence;

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
                sourceSequenceText.Text = GetString(seq);
            }
            else
            {
                sequenceTypeText.Text = "N/A";
                sourceSequenceText.Text = string.Empty;
            }
        }

        /// <summary>
        /// This method is called when the Transcribe button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // If we have an active sequence
            if (cbSequences.SelectedIndex >= 0)
            {
                ISequence seq = _sequences[cbSequences.SelectedIndex];
                
                // Determine the sequence type (DNA vs. RNA)
                ISequence result = null, proteinResult = null;
                if (seq.Alphabet == Alphabets.DNA)
                {
                    result = Transcription.Transcribe(seq);
                    proteinResult = ProteinTranslation.Translate(result);
                }
                else if (seq.Alphabet == Alphabets.RNA)
                    result = Transcription.ReverseTranscribe(seq);

                // Set the result.
                targetSequenceText.Text = (result != null)
                    ? GetString(result)
                    : string.Empty;

                // Translate the protein result
                proteinTextBox.Text = (proteinResult != null)
                    ? GetString(proteinResult)
                    : string.Empty;

                // Save off the transcribed sequence.
                _transcribedSequence = result;
            }
            else
            {
                // Reset the fields.
                targetSequenceText.Text = string.Empty;
            }
        }

        /// <summary>
        /// This is invoked when the user clicks the Save menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_transcribedSequence != null)
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    Title = "Select Filename for Transcribed Sequence",
                    Filter = string.Join("|", SequenceFormatters.All
                                                 .Select(sf => string.Format("{0}|{1}", 
                                                           sf.Name, sf.SupportedFileTypes.Replace(',', ';')))),
                    OverwritePrompt = true,
                    RestoreDirectory = true
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var formatter = SequenceFormatters.FindFormatterByFileName(sfd.FileName);
                    if (formatter != null)
                    {
                        formatter.Write(_transcribedSequence);
                        MessageBox.Show("Sequence has been saved to " + sfd.FileName, "Sequence Saved");
                        formatter.Close();
                    }
                }
            }
        }

        private static string GetString(ISequence sequence)
        {
            return new string(sequence.Select(b => (char)b)
                                    .Take(255)
                                    .ToArray());
        }
    }
}
