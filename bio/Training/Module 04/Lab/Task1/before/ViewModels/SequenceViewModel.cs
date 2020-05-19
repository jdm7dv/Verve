using System.ComponentModel;
using System.Diagnostics;

namespace SequenceLoader.ViewModels
{
    /// <summary>
    /// ViewModel for a single sequence.
    /// </summary>
    public class SequenceViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            // Verify property exists in debug builds.
            Debug.Assert(string.IsNullOrEmpty(propertyName) || GetType().GetProperty(propertyName) != null);

            // Raise PropertyChanged
            var inpc = PropertyChanged;
            if (inpc != null)
                inpc.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
