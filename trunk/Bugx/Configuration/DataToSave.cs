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
namespace Bugx.Web.Configuration
{
    /// <summary>
    /// Kind of data bugx can save by default.
    /// </summary>
    [Flags]
    public enum DataToSave
    {
        /// <summary>
        /// Nothing except Request Headers, Url and Form.
        /// </summary>
        None        = 0x00,

        /// <summary>
        /// Session variables.
        /// </summary>
        Session     = 0x01,

        /// <summary>
        /// Cached variables.
        /// </summary>
        Cache       = 0x02,

        /// <summary>
        /// Context variable.
        /// </summary>
        Context     = 0x04,

        /// <summary>
        /// Current Exception.
        /// </summary>
        Exception   = 0x08,

        /// <summary>
        /// Application variables
        /// </summary>
        Application = 0x10,

        /// <summary>
        /// Security information.
        /// </summary>
        User        = 0x20,

        /// <summary>
        /// Save all information.
        /// </summary>
        All         = Session | Cache | Context | Exception | Application | User
    }
}
