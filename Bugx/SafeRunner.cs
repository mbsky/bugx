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
using System.Text;
using System.IO;
using System.Web;
using System.Diagnostics;

namespace Bugx.Web
{
    /// <summary>
    /// This class run safely a method to avoid any handling crash.
    /// </summary>
    [DebuggerStepThrough]
    public static class SafeRunner
    {
        /// <summary>
        /// Empty delgate.
        /// </summary>
        public delegate void Method();

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        public static void Invoke(Method method)
        {
            if (method == null)
            {
                return;
            }
            try
            {
                method();
            }
            catch (Exception error)
            {
                LogException(error);
            }
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="error">The error.</param>
        static void LogException(Exception error)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(HttpContext.Current.Server.MapPath("~/bugx/crash.log"), true, Encoding.Default))
                {
                    file.WriteLine("/** {0:yyyy/MM/dd HH:mm:ss} *******************", DateTime.Now);
                    file.WriteLine(error);
                    file.WriteLine("/***************************************");
                    file.WriteLine();
                }
            }
            catch{}
        }
    }
}
