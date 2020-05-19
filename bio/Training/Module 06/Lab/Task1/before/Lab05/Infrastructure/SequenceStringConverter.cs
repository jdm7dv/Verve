using System;
using System.Linq;
using System.Windows.Data;
using Bio;

namespace Lab05.Infrastructure
{
    /// <summary>
    /// Value Converter to return the text portion of a sequence
    /// </summary>
    [ValueConversion(typeof(ISequence), typeof(string))]
    public class SequenceStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ISequence sequence = value as ISequence;
            return sequence != null
                       ? new string(sequence.Take(255).Select(b => (char) b).ToArray())
                       : "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
