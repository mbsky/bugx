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

namespace Bugx.ReBug
{
    public class ObjectInspector
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        object _Object;

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public object Object
        {
            get { return _Object; }
            set { _Object = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        string _Description;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInspector"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="data">The data.</param>
        public ObjectInspector(string description, object data)
        {
            _Description = description;
            _Object = data;
        }
    }
}