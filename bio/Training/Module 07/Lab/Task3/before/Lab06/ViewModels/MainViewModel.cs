using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lab05.Infrastructure;
using Bio;
using Bio.IO;
using Bio.Web;
using Bio.Web.Blast;
using Microsoft.Win32;

namespace Lab06.ViewModels
{
    /// <summary>
    /// Primary ViewModel that manages the application state and logic.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private IBlastServiceHandler _selectedBlastAlgorithm;
        private SequenceViewModel _selectedSequence;
        private string _selectedSequenceFragment;
        private IList<Hit> _resultHits;
        private string _isRunning;

        /// <summary>
        /// Loaded sequences
        /// </summary>
        public IList<SequenceViewModel> LoadedSequences { get; private set; }

        /// <summary>
        /// Returns the selected sequence
        /// </summary>
        public SequenceViewModel SelectedSequence
        {
            get { return _selectedSequence; }
            set
            {
                _selectedSequence = value;
                OnPropertyChanged("SelectedSequence");
            }
        }

        /// <summary>
        /// The fragment selected from the sequence
        /// </summary>
        public string SelectedSequenceFragment
        {
            get { return _selectedSequenceFragment; }
            set { _selectedSequenceFragment = value; OnPropertyChanged("SelectedSequenceFragment"); }
        }

        /// <summary>
        /// Command used to import
        /// </summary>
        public ICommand ImportFile { get; private set; }

        /// <summary>
        /// Run BLAST
        /// </summary>
        public ICommand RunBlast { get; private set; }

        /// <summary>
        /// Used to cancel a pending request
        /// </summary>
        public ICommand CancelBlast { get; private set; }

        /// <summary>
        /// True if you can change the algorithm style
        /// </summary>
        public bool CanChangeAlgorithm
        {
            get { return IsRunning == null; }
        }

        /// <summary>
        /// True when we are executing a query
        /// </summary>
        public string IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged("IsRunning");
                OnPropertyChanged("CanChangeAlgorithm");
                // Force WPF to reevaluate commands
                CommandManager.InvalidateRequerySuggested();
            }
        }

        /// <summary>
        /// List of available BLAST algorithms
        /// </summary>
        public IEnumerable<IBlastServiceHandler> AvailableBlastAlgorithms
        {
            get { return WebServices.All.OfType<IBlastServiceHandler>(); }
        }

        /// <summary>
        /// BLAST algorithm selected in ComboBox
        /// </summary>
        public IBlastServiceHandler SelectedBlastAlgorithm
        {
            get { return _selectedBlastAlgorithm; }
            set
            {
                _selectedBlastAlgorithm = value;
                OnPropertyChanged("SelectedBlastAlgorithm");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            LoadedSequences = new ObservableCollection<SequenceViewModel>();
            ImportFile = new DelegatingCommand(OnLoadFile);
            RunBlast = new DelegatingCommand(OnBlastSequence, () => SelectedSequence != null 
                                                && SelectedBlastAlgorithm != null && IsRunning == null);
            CancelBlast = new DelegatingCommand(OnCancelBlast, () => IsRunning != null);
        }

        /// <summary>
        /// Used to display a message on the screen when an exception occurs.
        /// </summary>
        /// <param name="title">Title of the message</param>
        /// <param name="message">Message to display</param>
        /// <param name="exception">Exception that has occurred</param>
        private static void ShowError(string title, string message, Exception exception)
        {
            string errorMessage = (exception != null) ? exception.Message : string.Empty;
            MessageBox.Show(string.Format("{0}{1}{2}", message, string.IsNullOrEmpty(errorMessage) ? "" : ": ", errorMessage), title);
        }

        /// <summary>
        /// This method loads new sequences from a file.
        /// </summary>
        private void OnLoadFile()
        {
            string filterString = "All Supported Formats|" + string.Join(";", SequenceParsers.All.Select(parser => parser.SupportedFileTypes.Replace(',', ';').Replace(".", "*."))) + "|" +
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
                    if (parser != null)
                    {
                        parser.Open(openFileDialog.FileName);
                    }
                }

                // Cannot parse this file.
                if (parser == null)
                {
                    MessageBox.Show(string.Format("Cannot locate a sequence parser for {0}", openFileDialog.FileName), "Cannot Parse File");
                    return;
                }

                // Parse the file - open it read-only as we will not be writing the sequences back out.
                try
                {
                    foreach (var sequence in parser.Parse())
                        LoadedSequences.Add(new SequenceViewModel(sequence));

                }
                catch (Exception ex)
                {
                    ShowError("Cannot Parse File", "Failed to open " + openFileDialog.FileName, ex);
                }
            }
        }

        /// <summary>
        /// Read-only results
        /// </summary>
        public IList<Hit> ResultHits
        {
            get { return _resultHits; }
            private set { _resultHits = value; OnPropertyChanged("ResultHits"); }
        }

        /// <summary>
        /// This method is called when the user wants to run BLAST on a sequence
        /// </summary>
        private void OnBlastSequence()
        {
            // Get the full sequence, or the selected fragment
            ISequence sequence = !string.IsNullOrEmpty(SelectedSequenceFragment)
                ? new Sequence(SelectedSequence.RawSequence.Alphabet, SelectedSequenceFragment)
                : SelectedSequence.RawSequence;

            IBlastServiceHandler blastServiceHandler = SelectedBlastAlgorithm;

            BlastParameters bp = new BlastParameters();
            bp.Add("Program", "blastn");
            bp.Add("Database", "nr");
            bp.Add("Expect", "10.0");

            blastServiceHandler.RequestCompleted += new EventHandler<BlastRequestCompletedEventArgs>(BlastServiceHandlerRequestCompleted);

            try
            {
                IsRunning = blastServiceHandler.SubmitRequest(sequence, bp);
                if (string.IsNullOrEmpty(IsRunning))
                    ShowError("Failed to execute Blast", "No results returned", null);
            }
            catch (Exception ex)
            {
                ShowError("Failed to execute Blast", "Error occurred", ex);
                ResultHits = null;
            }
        }

        /// <summary>
        /// Called when the request completed asynchronously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BlastServiceHandlerRequestCompleted(object sender, BlastRequestCompletedEventArgs e)
        {
            IsRunning = null;

            if (e.IsCanceled)
                return;

            if (e.Error != null || !string.IsNullOrEmpty(e.ErrorMessage))
            {
                ShowError("An error occurred", e.ErrorMessage, e.Error);
                return;
            }

            IList<BlastResult> results = e.SearchResult;
            if (results == null || results.Count == 0)
            {
                MessageBox.Show("No Results returned.");
                ResultHits = null;
            }
            else
                ResultHits = results[0].Records[0].Hits;
        }

        /// <summary>
        /// Used to cancel a pending request
        /// </summary>
        private void OnCancelBlast()
        {
            if (!SelectedBlastAlgorithm.CancelRequest(IsRunning))
                MessageBox.Show("Operation cannot be canceled.");
            else
            {
                IsRunning = null;
                MessageBox.Show("Pending request has been canceled!");
                ResultHits = null;
            }
        }
    }
}
