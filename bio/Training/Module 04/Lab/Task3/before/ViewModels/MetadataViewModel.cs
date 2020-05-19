using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Collections;

namespace SequenceLoader.ViewModels
{
    /// <summary>
    /// This ViewModel wraps a single metadata element and provides editing support
    /// </summary>
    public class MetadataViewModel : INotifyPropertyChanged
    {
        private IDictionary<string, object> _parent;
        private string _key;
        private object _value;

        /// <summary>
        /// Constructor used to add items in the DataGrid
        /// </summary>
        public MetadataViewModel()
        {
            _key = "NewData";
            _value = string.Empty;
        }

        /// <summary>
        /// Constructor used to initialize from Metadata Dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        public MetadataViewModel(IDictionary<string,object> dictionary, string key)
        {
            _parent = dictionary;
            _key = key;
            _value = dictionary[_key];
        }

        /// <summary>
        /// This is used to associate the ViewModel with the proper Metadata Dictionary
        /// on the sequence parent.
        /// </summary>
        /// <param name="parent"></param>
        internal void SetParentDictionary(IDictionary<string,object> parent)
        {
            _parent = parent;
            _parent[_key] = _value;
        }

        /// <summary>
        /// The key in the metadata dictionary
        /// </summary>
        public string Key
        {
            get { return _key; }
            set
            {
                // Remove the existing key, add the same 
                // data to the new key
                if (string.Compare(_key, value) != 0)
                {
                    _parent.Remove(_key);
                    _key = value;
                    _parent.Add(_key, _value);
                    RaisePropertyChanged("Key");
                }
            }
        }

        /// <summary>
        /// Returns whether the object data is a text string.
        /// </summary>
        public bool IsNonTextData
        {
            get
            {
                if (_value != null)
                {
                    if (_value.ToString() == _value.GetType().ToString())
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// The raw object data
        /// </summary>
        public object Value
        {
            get
            {
                return IsNonTextData ? string.Empty : _value;
            }

            set
            {
                _value = value;
                if (_parent != null)
                    _parent[_key] = value;

                RaisePropertyChanged("Value");
                RaisePropertyChanged("IsNonTextData");
            }
        }

        /// <summary>
        /// The bindable properties for the object data.
        /// These are bound to a data grid in the UI.
        /// </summary>
        public IList<Tuple<string,object>> ValueDetails
        {
            get
            {
                if (_value == null || _value.GetType() == typeof(string))
                    return new List<Tuple<string, object>>();

                return _value.GetType().GetProperties()
                    .Select(pi => new Tuple<string, object>(pi.Name, GetPropertyValue(pi,_value)))
                    .ToList();
            }
        }

        /// <summary>
        /// This returns a text representation for a single property on an object.
        /// If it is a text string or intrinsic, that is returned directly.
        /// If it's a complex type, it is expanded out to property name+value.
        /// If it's a collection it is ommited.
        /// </summary>
        /// <param name="pi">Reflection PropertyInfo</param>
        /// <param name="container">Object where property is located</param>
        /// <returns></returns>
        private static string GetPropertyValue(PropertyInfo pi, object container)
        {
            object propertyValue = pi.GetValue(container, null);
            if (propertyValue == null)
                return string.Empty;

            try
            {
                string toStringValue = propertyValue.ToString();
                if (toStringValue != pi.PropertyType.ToString() && !toStringValue.StartsWith("System."))
                    return toStringValue;

                // Array/Collection of some sort.
                if (propertyValue is IEnumerable)
                    return string.Empty;

                StringBuilder sb = new StringBuilder();
                foreach (var childProp in pi.PropertyType.GetProperties())
                {
                    string textValue = GetPropertyValue(childProp, propertyValue);
                    if (!string.IsNullOrEmpty(textValue))
                        sb.AppendFormat("{0} = {1}", childProp.Name, textValue);
                    else
                        sb.Append(childProp.Name);
                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
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
