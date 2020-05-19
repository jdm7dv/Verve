using System;
using System.Linq;
using Bio;

namespace Lab05.ViewModels
{
    /// <summary>
    /// This ViewModel represents a Sequence in our application
    /// </summary>
    public class SequenceViewModel : BaseViewModel
    {
        private readonly ISequence _sequence;
        private readonly MainViewModel _parent;
        private bool _isSelected;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent view model</param>
        /// <param name="sequence">Sequence to wrap</param>
        public SequenceViewModel(MainViewModel parent, ISequence sequence)
        {
            _parent = parent;
            _sequence = sequence;
        }

        /// <summary>
        /// Name used to represent sequence in list
        /// </summary>
        public string DisplayID
        {
            get { return _sequence.ID; }
        }

        /// <summary>
        /// Count of elements in sequence
        /// </summary>
        public long Count { get { return _sequence.Count; } }

        /// <summary>
        /// Alphabet for sequence
        /// </summary>
        public IAlphabet Alphabet { get { return _sequence.Alphabet; } }

        /// <summary>
        /// Detail string used in tooltip of the view
        /// </summary>
        public string Details
        {
            get { return string.Format("{0}: [{1}] {2} elements", _sequence.ID, _sequence.Alphabet.Name, _sequence.Count); }
        }

        /// <summary>
        /// Returns the sequence data as a string - we limit it to 
        /// 255 characters for display purposes.
        /// </summary>
        public string SequenceData
        {
            get 
            {
                string data = new string(_sequence.Select(b => (char)b).Take(255).ToArray());
                if (Count > 255)
                    data += "...";
                return data;
            }
        }

        /// <summary>
        /// True if this sequence is selected in the list.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set 
            { 
                _isSelected = value; 
                OnPropertyChanged("IsSelected");
                _parent.OnSequenceSelectionChanged(this); 
            }
        }

        /// <summary>
        /// Data accessor - retrieves underlying sequence interface
        /// </summary>
        internal ISequence RawSequence
        {
            get { return _sequence; }
        }
    }
}
