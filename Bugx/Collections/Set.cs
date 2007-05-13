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
    public class Set<T>: ICollection<T>
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
    }
}
