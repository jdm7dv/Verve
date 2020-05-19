using System;
using System.Windows;
using System.Windows.Input;
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

        private void OnOpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog(this) == true)
            {
                // TODO: add parser logic here
            }
        }

        private void OnExportData(object sender, RoutedEventArgs e)
        {
            // TODO: add formatter selection here
        }
    }
}
