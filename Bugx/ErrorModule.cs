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
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Bugx.Web
{
    public class ErrorModule : IHttpModule
    {
        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Error += new EventHandler(Application_Error);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this context is a re-bug.
        /// </summary>
        static bool _IsReBug;

        /// <summary>
        /// Gets or sets a value indicating whether this context is a re-bug.
        /// </summary>
        /// <value><c>true</c> if this context is a re-bug; otherwise, <c>false</c>.</value>
        public static bool IsReBug
        {
            get { return _IsReBug; }
            set { _IsReBug = value; }
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void Application_Error(object sender, EventArgs e)
        {
            if (IsReBug)
            {//Error handling is disabled.
                return;
            }
            HttpContext context = ((HttpApplication) sender).Context;
            BugDocument bug = new BugDocument();
            XmlNode root = bug.AppendChild(bug.CreateElement("bugx"));
            SaveUrl(root, context);
            SaveRequest(root, context);
            SaveSession(root, context);
            SaveException(root, context);
            DirectoryInfo destination =
                new DirectoryInfo(context.Request.MapPath("~/bugx/errors/" + context.Error.GetBaseException().GetType().FullName.Replace('.', '/') + "/" + BuildUniqueIdentifier(context.Error)));
            if (!destination.Exists)
            {
                destination.Create();
            }
            bug.Save(destination.FullName + DateTime.Now.ToUniversalTime().ToString("/yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture) + ".bugx");
        }

        /// <summary>
        /// Saves the session.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveSession(XmlNode root, HttpContext context)
        {
            XmlNode session = root.AppendChild(root.OwnerDocument.CreateElement("sessionVariables"));
            foreach (string key in HttpContext.Current.Session.Keys)
            {
                XmlNode variable = session.AppendChild(root.OwnerDocument.CreateElement("add"));
                variable.Attributes.Append(root.OwnerDocument.CreateAttribute("name")).Value = key;
                object value = context.Session[key];
                if (value == null)
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("value")).Value = "null";
                    continue;
                }
                Type variableType = value.GetType();
                variable.Attributes.Append(root.OwnerDocument.CreateAttribute("type")).Value = variableType.AssemblyQualifiedName;
                if (variableType.IsValueType || variableType == typeof(string))
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("value")).Value = Convert.ToString(value, CultureInfo.InvariantCulture);
                }
                else if (variableType.IsSerializable)
                {
                    variable.AppendChild(root.OwnerDocument.CreateCDataSection(BugSerializer.Serialize(value)));
                }
            }
        }

        /// <summary>
        /// Saves the URL.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveUrl(XmlNode root, HttpContext context)
        {
            root.AppendChild(root.OwnerDocument.CreateElement("url")).AppendChild(root.OwnerDocument.CreateCDataSection(context.Request.Url.ToString()));
            root.AppendChild(root.OwnerDocument.CreateElement("pathInfo")).AppendChild(root.OwnerDocument.CreateCDataSection(context.Request.PathInfo));
        }

        /// <summary>
        /// Saves the request.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveRequest(XmlNode root, HttpContext context)
        {
            HttpValueCollection.SaveCollectionToXmlNode(context.Request.QueryString,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("queryString")));
            HttpValueCollection.SaveCollectionToXmlNode(context.Request.Form,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("form")));
            HttpValueCollection.SaveCollectionToXmlNode(context.Request.Headers,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("headers")));
        }

        /// <summary>
        /// Saves the exception.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveException(XmlNode root, HttpContext context)
        {
            root.AppendChild(root.OwnerDocument.CreateElement("exception"))
                .AppendChild(root.OwnerDocument.CreateCDataSection(BugSerializer.Serialize(context.Error)));
        }

        /// <summary>
        /// Builds the unique identifier.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        static string BuildUniqueIdentifier(Exception exception)
        {
            StringBuilder messageHash = new StringBuilder();
            StringBuilder stackHash = new StringBuilder();
            while (exception != null)
            {
                messageHash.Append(exception.Message);
                stackHash.Append(RemoveDebugInformationFromStackTrace(exception.StackTrace));
                exception = exception.InnerException;
            }
            return string.Format(CultureInfo.InvariantCulture, "{0}-{1}", (uint) messageHash.ToString().GetHashCode(), (uint) stackHash.ToString().GetHashCode());
        }

        /// <summary>
        /// Removes the debug information from stack trace.
        /// </summary>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns></returns>
        static string RemoveDebugInformationFromStackTrace(string stackTrace)
        {
            return Regex.Replace(stackTrace, @"\) in \w\:[/\\].+", ")");
        }
    }
}