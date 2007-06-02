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
using System.Web;
using System.Xml;
using Bugx.Web;

namespace Bugx.ReBug
{
    /// <summary>
    /// Bugx is an ASP.NET application domain wich can processes <see cref="ReBugContext"/> requests.
    /// </summary>
    public class BugxHost : MarshalByRefObject
    {
        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Process(ReBugContext context)
        {
            ErrorModule.IsReBug = true;
            HttpWorkerRequest swr = new BugxWorkerRequest(context, Console.Out);
            HttpRuntime.ProcessRequest(swr);
        }

        /// <summary>
        /// Checks the app domain connection.
        /// </summary>
        public void CheckAppDomainConnection()
        {
            
        }
    }
}
