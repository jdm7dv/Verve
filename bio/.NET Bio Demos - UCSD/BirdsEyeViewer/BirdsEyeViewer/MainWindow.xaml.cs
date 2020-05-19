using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bio.IO;
using Bio;

namespace BirdsEyeViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            InitializeComponent();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var sequences = SequenceParsers
                .FindParserByFileName(@"c:\users\mark\desktop\data\5s.a.fasta")
                .Parse()
                .ToList();

            int width = (int) sequences.Max(s => s.Count);
            int height = sequences.Count;

            WriteableBitmap wb = new WriteableBitmap(
                width, height, 96, 96, 
                PixelFormats.Bgra32, null);
            theImage.Source = wb;

            byte[] backBuffer = new byte[width*height*4];
            for (int row = 0; row < height; row++)
            {
                ISequence sequence = sequences[row];
                for (int col = 0; col < width; col++)
                {
                    int pos = (row*wb.BackBufferStride) +
                              (col*4);
                    Color symbolColor = (sequence.Count > col)
                                            ? GetColorForSymbol(sequence[col])
                                            : Colors.White;
                    backBuffer[pos] = symbolColor.B;
                    backBuffer[pos+1] = symbolColor.G;
                    backBuffer[pos+2] = symbolColor.R;
                    backBuffer[pos+3] = 0xff;
                }
            }

            wb.WritePixels(new Int32Rect(0,0,width,height), backBuffer, wb.BackBufferStride, 0);
        }

        public Color GetColorForSymbol(byte symbol)
        {
            Color returnColor = Colors.White;
            switch (Char.ToUpper((char) symbol))
            {
                case 'A':
                    returnColor = Colors.BlanchedAlmond;
                    break;
                case 'G':
                    returnColor = Colors.ForestGreen;
                    break;
                case 'U':
                    returnColor = Colors.Orchid;
                    break;
                case 'T':
                    returnColor = Colors.Khaki;
                    break;
                case 'C':
                    returnColor = Colors.MediumSlateBlue;
                    break;
            }

            return returnColor;
        }
    }
}
