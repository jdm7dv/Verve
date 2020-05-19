using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace SequenceLoader.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private SequenceViewModel _selectedSequence;

        /// <summary>
        /// Collection of loaded sequences
        /// </summary>
        public IList<SequenceViewModel> LoadedSequences { get; private set; }

        /// <summary>
        /// The selected sequence
        /// </summary>
        public SequenceViewModel SelectedSequence
        {
            get { return _selectedSequence; }
            set { _selectedSequence = value; RaisePropertyChanged("SelectedSequence"); }
        }

        public MainViewModel()
        {
            LoadedSequences = new ObservableCollection<SequenceViewModel>();
        }

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
