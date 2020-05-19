using System.Windows;
using Lab05.ViewModels;

namespace Lab05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }

        /// <summary>
        /// Invoked by the MenuItem "Exit".  Closes the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
