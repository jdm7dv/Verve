using System.Windows;
using System.Windows.Controls;

namespace Lab06.Infrastructure
{
    /// <summary>
    /// This class provides the ability to DataBind to the TextBox.SelectedText property.
    /// Because the SelectedText property is not defined as a DependencyProperty, this feature is not possible
    /// directly but requires this shim to work properly.
    /// </summary>
    public static class TextBoxHelper
    {
        /// <summary>
        /// Dependency Property definition (attached)
        /// </summary>
        public static readonly DependencyProperty SelectedTextProperty = DependencyProperty.RegisterAttached("SelectedText", typeof(string), typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTextChanged));

        /// <summary>
        /// Retrieves the selected text attached property
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetSelectedText(DependencyObject obj)
        {
            return (string)obj.GetValue(SelectedTextProperty);
        }

        /// <summary>
        /// Sets the selected text attached property
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetSelectedText(DependencyObject obj, string value)
        {
            obj.SetValue(SelectedTextProperty, value);
        }

        /// <summary>
        /// This is called when the SelectedText attached property is changed.
        /// It wires up to the proper event handlers.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        static void OnSelectedTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TextBox tb = obj as TextBox;
            if (tb != null)
            {
                tb.SelectionChanged -= OnSelectionChanged;
                if (e.NewValue != null)
                {
                    tb.SelectionChanged += OnSelectionChanged;
                    string newValue = e.NewValue as string;
                    if (newValue != null && newValue != tb.SelectedText)
                        tb.SelectedText = newValue;
                }
            }
        }

        /// <summary>
        /// This is the TextBox.SelectionChanged event handler.
        /// It propogates the change in the TextBox back to the attached property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
                SetSelectedText(tb, tb.SelectedText);
        }
    }
}
