using System.Windows;
using System.Windows.Input;
using Bio.IO;
using SequenceLoader.ViewModels;

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
            DataContext = sequenceViewModel;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnOk, (s, e) => e.CanExecute = cbFormatters.SelectedItem != null));
            InitializeComponent();
        }

        /// <summary>
        /// Dismisses the dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOk(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
