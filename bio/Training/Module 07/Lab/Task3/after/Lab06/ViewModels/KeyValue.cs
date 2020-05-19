namespace Lab06.ViewModels
{
    /// <summary>
    /// Simple class used to expose a modifiable Tuple (key/value pair)
    /// </summary>
    public class KeyValue : BaseViewModel
    {
        private string _key;
        private string _value;

        public string Key
        {
            get { return _key; }
            set { _key = value; OnPropertyChanged("Key"); }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        /// <summary>
        /// Constructor to initialize Tuple
        /// </summary>
        public KeyValue(string key, string value = "")
        {
            Key = key;
            Value = value;
        }
    }
}
