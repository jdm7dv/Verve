// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF
{
    /// <summary>
    /// Generic Enumerator implementation which works on an IList.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")] // Nothing to dispose so suppressed
    public class GenericIListEnumerator<T> : IEnumerator<T>
    {
        private IList<T> _items;
        private int index;

        /// <summary>
        /// Constructs an enumerator for specified list of items.
        /// </summary>
        public GenericIListEnumerator(IList<T> items)
        {
            _items = items;
            Reset();
        }

        #region IEnumerator<T> Members
        /// <summary>
        /// The current item reference for the enumerator.
        /// </summary>
        public T Current
        {
            get
            {
                if (index < 0)
                    return default(T);

                return _items[index];
            }
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Takes care of any allocated memory
        /// </summary>
        // Nothing to dispose so suppressed the below warnings
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly"), 
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly")]
        public void Dispose()
        {
            // No op
        }

        #endregion

        #region IEnumerator Members
        /// <summary>
        /// The current item reference for the enumerator
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        /// <summary>
        /// Advances the enumerator to the next item
        /// </summary>
        /// <returns>True if the enumerator can advance. False if the end index is reached.</returns>
        public bool MoveNext()
        {
            if (index < (_items.Count - 1))
            {
                index++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the enumerator to the start position.
        /// </summary>
        public void Reset()
        {
            index = -1;
        }

        #endregion
    }
}
