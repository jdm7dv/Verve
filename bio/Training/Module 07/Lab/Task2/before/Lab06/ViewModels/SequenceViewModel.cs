using System.Linq;
using Bio;

namespace Lab06.ViewModels
{
    /// <summary>
    /// This ViewModel represents a Sequence in our application
    /// </summary>
    public class SequenceViewModel : BaseViewModel
    {
        private readonly ISequence _sequence;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sequence">Sequence to wrap</param>
        public SequenceViewModel(ISequence sequence)
        {
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
        /// Detail string used in tooltip of the view
        /// </summary>
        public string Details
        {
            get { return string.Format("{0}: [{1}] {2} elements", _sequence.ID, _sequence.Alphabet.Name, _sequence.Count); }
        }

        /// <summary>
        /// Returns the sequence data
        /// </summary>
        public string SequenceData
        {
            get { return new string(_sequence.Select(b => (char)b).Take(255).ToArray()); }
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
