/*
BUGx: An Asp.Net Bug Tracking tool.
Copyright (C) 2007 Olivier Bossaer

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

Wavenet, hereby disclaims all copyright interest in
the library `BUGx' (An Asp.Net Bug Tracking tool) written
by Olivier Bossaer. (olivier.bossaer@gmail.com)
*/
using System.Collections.Generic;
using System.Collections;
using System;
using System.Diagnostics;

namespace Bugx.Web.Collections
{
    /// <summary>
    /// Simple Set Implementation.
    /// </summary>
    /// <remarks>
    /// This class should be complete with set opperand like union, intersection, etc...
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    public class SetCollection<T>: ICollection<T>, ICollection
    {
        Dictionary<T, bool> _Implementation = new Dictionary<T, bool>();

        #region ICollection<T> Members

        public void Add(T item)
        {
            _Implementation[item] = true;
        }

        public void Clear()
        {
            _Implementation.Clear();
        }

        public bool Contains(T item)
        {
            return _Implementation.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _Implementation.Keys.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get{ return _Implementation.Count; }
        }

        public bool IsReadOnly
        {
            get{ return false; }
        }

        public bool Remove(T item)
        {
            return _Implementation.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) _Implementation.Keys).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_Implementation.Keys).GetEnumerator();
            
        }
        #endregion

        #region ICollection Members

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">array is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
        /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((T[])array, index);
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
        protected virtual bool IsSynchronized
        {
            get{ return false; }
        }
        
        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
        bool ICollection.IsSynchronized
        {
            get{ return IsSynchronized; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
        protected virtual object SyncRoot
        {
            get{ return _Implementation; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
        object ICollection.SyncRoot
        {
            get{ return SyncRoot; }
        }

        #endregion
    }
}
