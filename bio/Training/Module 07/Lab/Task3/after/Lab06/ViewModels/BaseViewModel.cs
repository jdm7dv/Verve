using System.ComponentModel;
using System.Diagnostics;

namespace Lab06.ViewModels
{
    /// <summary>
    /// Base class for all ViewModel objects
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            Debug.Assert(string.IsNullOrEmpty(propName) || GetType().GetProperty(propName) != null);
            var inpc = PropertyChanged;
            if (inpc != null)
                inpc.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }
}
