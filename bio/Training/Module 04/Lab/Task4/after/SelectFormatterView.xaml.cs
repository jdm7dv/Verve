using System.Windows;
using System.Windows.Input;
using SequenceLoader.ViewModels;
using Bio.IO;

namespace SequenceLoader
{
    /// <summary>
    /// Interaction logic for SelectFormatterView.xaml
    /// </summary>
    public partial class SelectFormatterView
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sequenceViewModel"></param>
        public SelectFormatterView(SequenceViewModel sequenceViewModel)
        {
            Loaded += SelectFormatterView_Loaded;
            DataContext = sequenceViewModel;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnOk, (s, e) => e.CanExecute = cbFormatters.SelectedItem != null));
            InitializeComponent();
        }

        /// <summary>
        /// Called after the window is created and UI elements are 
        /// available and connected to the visual tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectFormatterView_Loaded(object sender, RoutedEventArgs e)
        {
            // Add all the formatters to the ComboBox.
            foreach (var formatter in SequenceFormatters.All)
                cbFormatters.Items.Add(formatter);
        }

        /// <summary>
        /// Selected formatter
        /// </summary>
        public ISequenceFormatter SelectedFormatter { get; private set; }

        /// <summary>
        /// Dismisses the dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOk(object sender, RoutedEventArgs e)
        {
            SelectedFormatter = (ISequenceFormatter) cbFormatters.SelectedItem;
            DialogResult = true;
        }
    }
}
