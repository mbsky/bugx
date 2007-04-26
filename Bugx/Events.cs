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
using System;
using System.Collections.Generic;
using System.Text;

namespace Bugx.Web
{
    public class BugEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the bug URI.
        /// </summary>
        Uri _BugUri;

        /// <summary>
        /// Gets or sets the bug URI.
        /// </summary>
        /// <value>The bug URI.</value>
        public Uri BugUri
        {
            get { return this._BugUri; }
            set { this._BugUri = value; }
        }

        /// <summary>
        /// Gets or sets the bug.
        /// </summary>
        BugDocument _Bug;

        /// <summary>
        /// Gets or sets the bug.
        /// </summary>
        /// <value>The bug.</value>
        public BugDocument Bug
        {
            get { return this._Bug; }
            set { this._Bug = value; }
        }

    }
}