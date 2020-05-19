using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Bio;
using System.Collections.ObjectModel;

namespace SequenceLoader.ViewModels
{
    /// <summary>
    /// ViewModel for a single sequence.
    /// </summary>
    public class SequenceViewModel : INotifyPropertyChanged
    {
        private const int DisplayCount = 50;
        private readonly ISequence _sequence;
        private int _position;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sequence"></param>
        public SequenceViewModel(ISequence sequence)
        {
            _sequence = sequence;
            var md = new ObservableCollection<MetadataViewModel>(
                     _sequence.Metadata
                     .Select((kvp =>
                         new MetadataViewModel(_sequence.Metadata, kvp.Key))));
            Metadata = md;
            md.CollectionChanged += OnMetadataCollectionChanged;
        }

        /// <summary>
        /// This is called when the metadata changes in our collection
        /// which is being displayed in the UI.  We push it back into 
        /// the sequence we are holding onto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMetadataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (MetadataViewModel item in e.NewItems)
                {
                    string key = item.Key;
                    while (_sequence.Metadata.ContainsKey(key))
                        key += ".";
                    item.Key = key;
                    item.SetParentDictionary(_sequence.Metadata);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (MetadataViewModel item in e.OldItems)
                {
                    string key = item.Key;
                    if (_sequence.Metadata.ContainsKey(key))
                        _sequence.Metadata.Remove(key);
                }
            }
        }

        /// <summary>
        /// First 20 characters of sequence name
        /// </summary>
        public string ShortName
        {
            get { return _sequence.ID.Substring(0, Math.Min(20, _sequence.ID.Length)); }
        }

        /// <summary>
        /// Molecule type from alphabet
        /// </summary>
        public string MoleculeType
        {
            get { return _sequence.Alphabet.Name; }
        }

        /// <summary>
        /// Full name of the sequence
        /// </summary>
        public string FullName
        {
            get { return _sequence.ID; }
        }

        /// <summary>
        /// Metadata
        /// </summary>
        public IList<MetadataViewModel> Metadata { get; private set; }

        /// <summary>
        /// Current position within the sequence for the UI display.
        /// </summary>
        public int SegmentPosition
        {
            get { return _position; }
            set
            {
                _position = value;
                RaisePropertyChanged("SegmentPosition");
                RaisePropertyChanged("SegmentEnd");
                RaisePropertyChanged("Segment");
                RaisePropertyChanged("ReverseSegment");
                RaisePropertyChanged("ComplementSegment");
                RaisePropertyChanged("ReverseComplementSegment");
            }
        }

        /// <summary>
        /// Ending index for the sequence in the UI display.
        /// </summary>
        public int SegmentEnd
        {
            get { return _position + DisplayCount; }
        }

        /// <summary>
        /// Maximum range for the slider in the UI
        /// I.e. what it's value is at the far right end.
        /// </summary>
        public int SegmentMaxRange
        {
            get { return (int) _sequence.Count - DisplayCount; }
        }

        /// <summary>
        /// The slice of the sequence (Pos+Size)
        /// </summary>
        public string Segment
        {
            get { return GetString(_sequence.GetSubSequence(_position, DisplayCount)); }
        }

        /// <summary>
        /// The Reverse of the Segment.
        /// </summary>
        public string ReverseSegment
        {
            get { return GetString(_sequence.GetReversedSequence().GetSubSequence(_position, DisplayCount)); }
        }

        /// <summary>
        /// The Complement of the Segment.
        /// </summary>
        public string ComplementSegment
        {
            get
            {
                if (_sequence.Alphabet.IsComplementSupported)
                    return "N/A";
                return GetString(_sequence.GetSubSequence(_position, DisplayCount).GetComplementedSequence());
            }
        }

        /// <summary>
        /// The ReverseComplement of the Segment.
        /// </summary>
        public string ReverseComplementSegment
        {
            get
            {
                if (_sequence.Alphabet.IsComplementSupported)
                    return "N/A";
                return GetString(_sequence.GetReverseComplementedSequence().GetSubSequence(_position, DisplayCount));
            }
        }

        /// <summary>
        /// Generate a string from a sequence - limited to 255 characters.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        private static string GetString(ISequence sequence)
        {
            return new string(
                sequence.Select(b => (char) b).Take(255).ToArray());
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
