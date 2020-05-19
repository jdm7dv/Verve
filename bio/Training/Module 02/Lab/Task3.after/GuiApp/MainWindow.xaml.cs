using System.Windows;
using ConsoleApp;
using StringProcessor;

namespace GuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnProcessText(object sender, RoutedEventArgs e)
        {
            IStringProcessor processor = availableProcessors.SelectedItem as IStringProcessor;
            if (processor != null)
            {
                string input = originalText.Text;
                string result = processor.Process(input);
                resultText.Text = result;
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            availableProcessors.Items.Add(new ReverseProcessor());
            availableProcessors.Items.Add(new UppercaseString());
            availableProcessors.Items.Add(new RandomizeString());
            availableProcessors.Items.Add(new ReverseWords());
            availableProcessors.SelectedIndex = 0;
        }
    }
}
