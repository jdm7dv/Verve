using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.Algorithms.Translation;
using Lab05.Infrastructure;
using Bio.IO;
using Microsoft.Win32;
using Bio.Algorithms.Assembly;

namespace Lab05.ViewModels
{
    /// <summary>
    /// Primary ViewModel that manages the application state and logic.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private ISequenceAligner _selectedAligner;
        private ISequenceAlignment _alignment;
        private IDeNovoAssembler _selectedAssembler;
        private IDeNovoAssembly _assembledSequence;

        /// <summary>
        /// Loaded sequences
        /// </summary>
        public IList<SequenceViewModel> LoadedSequences { get; private set; }

        /// <summary>
        /// Returns the selected sequences
        /// </summary>
        public IList<ISequence> SelectedSequences
        {
            get
            {
                return LoadedSequences
                    .Where(seq => seq.IsSelected)
                    .Select(seq => seq.RawSequence)
                    .ToList();
            }
        }

        /// <summary>
        /// Sequences with a translation
        /// </summary>
        public IList<ISequence> TranslatedSequences { get; private set; }

        /// <summary>
        /// The selected alignment algorithm
        /// </summary>
        public ISequenceAligner SelectedAligner
        {
            get { return _selectedAligner; }
            set { _selectedAligner = value; OnPropertyChanged("SelectedAligner"); }
        }

        /// <summary>
        /// The alignment results
        /// </summary>
        public ISequenceAlignment Alignment
        {
            get { return _alignment; }
            set { _alignment = value; OnPropertyChanged("Alignment"); }
        }

        /// <summary>
        /// The selected assembly algorithm
        /// </summary>
        public IDeNovoAssembler SelectedAssembler
        {
            get { return _selectedAssembler; }
            set { _selectedAssembler = value; OnPropertyChanged("SelectedAssembler"); }
        }

        /// <summary>
        /// Results from assembly
        /// </summary>
        public IDeNovoAssembly AssembledSequence
        {
            get { return _assembledSequence; }
            set { _assembledSequence = value; OnPropertyChanged("AssembledSequence"); }
        }

        /// <summary>
        /// Return list of known assemblers
        /// </summary>
        public IEnumerable<IDeNovoAssembler> AvailableAssemblers
        {
            get
            {
                yield return new OverlapDeNovoAssembler();
            }
        }

        /// <summary>
        /// Command used to import
        /// </summary>
        public ICommand ImportFile { get; private set; }

        /// <summary>
        /// Align 2+ sequences
        /// </summary>
        public ICommand AlignSequences { get; private set; }

        /// <summary>
        /// Assembles 2+ sequences
        /// </summary>
        public ICommand AssembleSequences { get; private set; }

        /// <summary>
        /// Transforms selected sequences
        /// </summary>
        public ICommand TransformSequence { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            LoadedSequences = new ObservableCollection<SequenceViewModel>();
            SelectedAligner = SequenceAligners.SmithWaterman;
            SelectedAssembler = AvailableAssemblers.FirstOrDefault(); 
            TranslatedSequences = new ObservableCollection<ISequence>();
            
            ImportFile = new DelegatingCommand(OnLoadFile);
            AlignSequences = new DelegatingCommand(OnAlignSequences, () => SelectedSequences.Count >= 2);
            AssembleSequences = new DelegatingCommand(OnAssembleSequences, () => SelectedSequences.Count >= 2 && SelectedAssembler != null);
            TransformSequence = new DelegatingCommand(OnTransformSequences, () => SelectedSequences.Count > 0 && SelectedSequences.Any(si => si.Alphabet.IsComplementSupported));
        }

        /// <summary>
        /// This is called when a sequence is selected/deselected
        /// </summary>
        /// <param name="sequenceViewModel"></param>
        internal void OnSequenceSelectionChanged(SequenceViewModel sequenceViewModel)
        {
            OnPropertyChanged("SelectedSequences");
            AssembledSequence = null;
        }

        /// <summary>
        /// Used to display a message on the screen when an exception occurs.
        /// This method takes the Parallel Task Library into account by handling
        /// the AggregatedException and dumping the internal exceptions.
        /// </summary>
        /// <param name="title">Title of the message</param>
        /// <param name="message">Message to display</param>
        /// <param name="exception">Exception that has occurred</param>
        private static void ShowError(string title, string message, Exception exception)
        {
            // Break out any aggregate execptions; or use the exception message directly.
            string errorMessage = exception is AggregateException
                    ? String.Join(Environment.NewLine,
                        ((AggregateException)exception).InnerExceptions.Select(e => e.Message))
                    : exception.Message;

            // Display an error using a MessageBox.
            MessageBox.Show(string.Format("{0}: {1}", message, errorMessage), title);
        }

        /// <summary>
        /// This method loads new sequences from a file.
        /// </summary>
        private void OnLoadFile()
        {
            string filterString = "All Supported Formats|" + string.Join(";", SequenceParsers.All.Select(parser=>parser.SupportedFileTypes.Replace(',',';').Replace(".","*."))) + "|" +
                string.Join("|", SequenceParsers.All.Select(parser => string.Format("{0}|{1}", parser.Name, parser.SupportedFileTypes.Replace(',', ';').Replace(".", "*."))));

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = filterString
            };

            // Prompt the user for the filename
            if (openFileDialog.ShowDialog() == true)
            {
                // See if we can auto-locate the parser
                ISequenceParser parser = SequenceParsers.FindParserByFileName(openFileDialog.FileName);
                if (parser == null)
                {
                    // Use the extension
                    string fileExtension = Path.GetExtension(openFileDialog.FileName);
                    parser = SequenceParsers.All.FirstOrDefault(sp => sp.SupportedFileTypes.Contains(fileExtension));
                }

                // Cannot parse this file.
                if (parser == null)
                {
                    MessageBox.Show(string.Format("Cannot locate a sequence parser for {0}", openFileDialog.FileName),"Cannot Parse File");
                    return;
                }

                // Parse the file - open it read-only as we will not be writing the sequences back out.
                try
                {
                    foreach (var sequence in parser.Parse())
                        LoadedSequences.Add(new SequenceViewModel(this, sequence));
                }
                catch (Exception ex)
                {
                    ShowError("Cannot Parse File", "Failed to open " + openFileDialog.FileName, ex);
                }
                finally
                {
                    parser.Close();
                }
            }
        }


        /// <summary>
        /// This is called to align two sequences.
        /// </summary>
        private void OnAlignSequences()
        {
            var selectedSequences = SelectedSequences;
            Debug.Assert(selectedSequences.Count >= 2);
            Debug.Assert(SelectedAligner != null);

            try
            {
                var result = SelectedAligner.Align(selectedSequences.Take(2));
                Alignment = result.Count > 0 ? result[0] : null;
            }
            catch (Exception ex)
            {
                ShowError("Failed to align sequences", "Could not align sequences", ex);
            }
        }

        /// <summary>
        /// This is called to assemble the selected sequences
        /// </summary>
        private void OnAssembleSequences()
        {
            var selectedSequences = SelectedSequences;
            Debug.Assert(selectedSequences.Count >= 2);
            Debug.Assert(SelectedAssembler != null);
            AssembledSequence = null;

            try
            {
                AssembledSequence = SelectedAssembler.Assemble(selectedSequences);
            }
            catch (Exception ex)
            {
                ShowError("Failed to assemble sequences", "Could not assemble sequences", ex);
            }
        }

        /// <summary>
        /// This is called to transform sequences
        /// </summary>
        private void OnTransformSequences()
        {
            TranslatedSequences.Clear();

            foreach (var sequence in SelectedSequences)
            {
                ISequence result = null;

                try
                {
                    if (sequence.Alphabet == Alphabets.DNA)
                    {
                        result = Transcription.Transcribe(sequence);
                        result.Metadata["Documentation"] = "Transcribed RNA from DNA sequence " + sequence.ID;
                    }
                    else if (sequence.Alphabet == Alphabets.RNA)
                    {
                        var alphabet = sequence.Alphabet;
                        var noGapSequence = new Sequence(alphabet, sequence.Where(b => !alphabet.CheckIsGap(b)).ToArray());

                        result = ProteinTranslation.Translate(noGapSequence);
                        result.Metadata["Documentation"] = "Translated Protein from RNA sequence " + sequence.ID;
                    }
                }
                catch (Exception ex)
                {
                    ShowError("Translation Failed",
                              "Unable to transcribe/translate sequence", ex);
                }

                if (result != null)
                    TranslatedSequences.Add(result);
            }
        }
    }
}
