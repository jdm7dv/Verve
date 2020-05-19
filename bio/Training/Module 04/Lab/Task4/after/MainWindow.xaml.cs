using System;
using System.Windows;
using System.Windows.Input;
using Bio.IO;
using Microsoft.Win32;
using SequenceLoader.ViewModels;

namespace SequenceLoader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainViewModel _vm = new MainViewModel();

        public MainWindow()
        {
            // Wire up command handlers for 3 menu commands.
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OnOpenFile));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, OnExportData, (s, e) => e.CanExecute = _vm.SelectedSequence != null));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (s,e) => Close()));

            DataContext = _vm;
            InitializeComponent();
        }

        /// <summary>
        /// This loads a sequence from disk using a parser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog(this) == true)
            {
                // Identify the file format if we can
                var parser = SequenceParsers.FindParserByFileName(ofd.FileName);
                if (parser != null)
                {
                    // Load the sequence(s) in.
                    var sequenceList = parser.Parse();
                    if (sequenceList != null)
                    {
                        // Add them to the UI
                        foreach (var sequence in sequenceList)
                        {
                            _vm.LoadedSequences.Add(new SequenceViewModel(sequence));
                        }
                    }
                }
            }
        }

        private void OnExportData(object sender, RoutedEventArgs e)
        {
            var selectFormatterView = new SelectFormatterView(_vm.SelectedSequence);
            if (selectFormatterView.ShowDialog() == true)
            {
                var sequenceFormatter = selectFormatterView.SelectedFormatter;
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = sequenceFormatter.Name + "|" +
                             sequenceFormatter.SupportedFileTypes.Replace(',', ';'),
                    AddExtension = false,
                    CreatePrompt = true
                };

                if (sfd.ShowDialog(this) == true)
                {
                    try
                    {
                        sequenceFormatter.Open(sfd.FileName);
                        sequenceFormatter.Write(_vm.SelectedSequence.RawSequence);
                        sequenceFormatter.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Failed to export data", MessageBoxButton.OK);
                    }
                }
            }
        }
    }
}
