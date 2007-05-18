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
using System.Collections;
using BugEventHandler = System.EventHandler<Bugx.Web.BugEventArgs>;
using ApplicationUnloadEventHandler = System.EventHandler<Bugx.Web.ApplicationUnloadEventArgs>;
using Bugx.Web.Configuration;
using System.Collections.Generic;


namespace Bugx.Web
{
    public class ErrorModule : IHttpModule
    {
        #region ExceptionTracking
        class ExceptionTracking
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ExceptionTracking"/> class.
            /// </summary>
            ExceptionTracking()
            {
            }

            #region Fields
            /// <summary>
            /// Gets the time stamp.
            /// </summary>
            int _TimeStamp = Now;

            /// <summary>
            /// Gets the count.
            /// </summary>
            int _Count = 1;

            /// <summary>
            /// Contains all error to track.
            /// </summary>
            static Dictionary<string, ExceptionTracking> _Items = new Dictionary<string, ExceptionTracking>();
            #endregion

            #region Properties
            /// <summary>
            /// Gets the time stamp.
            /// </summary>
            /// <value>The time stamp.</value>
            public int TimeStamp
            {
                get
                {
                    return _TimeStamp;
                }
            }

            /// <summary>
            /// Gets the count.
            /// </summary>
            /// <value>The count.</value>
            public int Count
            {
                get
                {
                    return _Count;
                }
            }
            #endregion

            #region Implementation

            /// <summary>
            /// Gets the now.
            /// </summary>
            /// <value>The now.</value>
            static int Now
            {
                get
                {
                    return (int)new TimeSpan(DateTime.Now.Ticks).TotalMinutes;
                }
            }

            /// <summary>
            /// Cleans the tracking.
            /// </summary>
            static void CleanTracking()
            {
                int now = Now - 1;
                List<string> keyToRemove = new List<string>();
                foreach (KeyValuePair<string, ExceptionTracking> trackingInfo in _Items)
                {
                    if (trackingInfo.Value.TimeStamp < now)
                    {
                        keyToRemove.Add(trackingInfo.Key);
                    }
                }
                foreach (string key in keyToRemove)
                {
                    _Items.Remove(key);
                }
            }

            /// <summary>
            /// Gets the specified <see cref="ExceptionTracking"/>.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The specified ExceptionTracking or a new one for this exception.</returns>
            public static ExceptionTracking Get(string key)
            {
                CleanTracking();
                ExceptionTracking tracking;
                if (_Items.TryGetValue(key, out tracking))
                {
                    tracking._TimeStamp = Now;
                    tracking._Count++;
                    return tracking;
                }
                return new ExceptionTracking();
            }
            #endregion
        } 
        #endregion

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
            context.Error += new EventHandler(SafeApplication_Error);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
        }

        /// <summary>
        /// Handles the DomainUnload event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            OnApplicationUnload(new ApplicationUnloadEventArgs());
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
        /// Handles the Error event of the Application control (Safe).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void SafeApplication_Error(object sender, EventArgs e)
        {
            SafeRunner.Invoke(delegate{Application_Error(sender);});
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        static void Application_Error(object sender)
        {
            if (IsReBug)
            {//Error handling is disabled.
                return;
            }
            HttpContext context = ((HttpApplication) sender).Context;
            if (context == null || context.Error == null)
            {
                return;
            }
            BugDocument bug = new BugDocument();
            XmlNode root = bug.AppendChild(bug.CreateElement("bugx"));
            bug.CreationDate = DateTime.Now;
            bug.ErrorFromBot = BrowserHelper.IsKnownBot(context.Request.UserAgent);
            BugEventArgs bugEventArgs = new BugEventArgs();
            bugEventArgs.Bug = bug;
            DataToSave dataToSave = BugxConfiguration.DataToSave;
            string errorId = BuildUniqueIdentifier(context.Error);
            string bugVirtualPath = "~/bugx/errors/" + context.Error.GetBaseException().GetType().FullName.Replace('.', '/') + "/" + errorId;
            DirectoryInfo destination = new DirectoryInfo(context.Request.MapPath(bugVirtualPath));
            if (!destination.Exists)
            {
                destination.Create();
            }
            string fileName = DateTime.Now.ToUniversalTime().ToString("/yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture) + ".bugx";
            if (File.Exists(fileName))
            {
                return;
            }
            //Minimum data to save.
            SaveUrl(root, context);
            SaveRequest(root, context);

            if ((dataToSave & DataToSave.Session) != 0)
            {
                SaveSession(root, context);
            }
            if ((dataToSave & DataToSave.Cache) != 0)
            {
                SaveCache(root, context);
            }
            if ((dataToSave & DataToSave.Context) != 0)
            {
                SaveContext(root, context);
            }
            if ((dataToSave & DataToSave.Application) != 0)
            {
                SaveApplication(root, context);
            }
            if ((dataToSave & DataToSave.User) != 0)
            {
                SaveUser(root, context);
            }
            if ((dataToSave & DataToSave.Exception) != 0)
            {
                SaveException(root, context);
            }
            OnError(bugEventArgs);
            try
            {
                bug.Save(destination.FullName + fileName);
                bugEventArgs.BugUri = ExceptionHelper.BuildBugUri(bugVirtualPath + "/" + fileName);
                bugEventArgs.BugReport = ExceptionHelper.BuildBugReportUri(bugVirtualPath + "/" + fileName);
                if (MaySendErrorComplete(errorId) && !BugxConfiguration.ShouldBeFiltered(errorId))
                {
                    OnErrorComplete(bugEventArgs);
                }
            }catch(IOException){}//Nothing to do if the same bug arrives twice.
        }

        /// <summary>
        /// Mays send <c>ErrorComplete</c> event.
        /// </summary>
        /// <param name="errorId">The error id.</param>
        /// <returns></returns>
        static bool MaySendErrorComplete(string errorId)
        {
            int? maxErrorReportingPerMinute = BugxConfiguration.MaxErrorReportingPerMinute;
            if (maxErrorReportingPerMinute == null)
            {//No rules about notification.
                return true;
            }
            return ExceptionTracking.Get(errorId).Count <= maxErrorReportingPerMinute.Value;
        }

        /// <summary>
        /// Saves the context.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveContext(XmlNode root, HttpContext context)
        {
            XmlNode contextNode = root.AppendChild(root.OwnerDocument.CreateElement("contextVariables"));
            foreach (DictionaryEntry entry in context.Items)
            {
                Type keyType = entry.Key.GetType();
                XmlNode variable = contextNode.AppendChild(root.OwnerDocument.CreateElement("add"));
                if (keyType.IsValueType || keyType == typeof(string))
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("nameType")).Value = keyType.AssemblyQualifiedName;
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("name")).Value = Convert.ToString(entry.Key, CultureInfo.InvariantCulture);
                }
                else if (keyType.IsSerializable)
                {
                    variable.AppendChild(root.OwnerDocument.CreateElement("key")).AppendChild(root.OwnerDocument.CreateCDataSection(BugSerializer.Serialize(entry.Key)));
                }
                else
                {
                    variable.ParentNode.RemoveChild(variable);
                    continue;
                }
                if (entry.Value == null)
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("value")).Value = "null";
                    continue;
                }
                Type variableType = entry.Value.GetType();
                variable.Attributes.Append(root.OwnerDocument.CreateAttribute("type")).Value = variableType.AssemblyQualifiedName;
                if (variableType.IsValueType || variableType == typeof(string))
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("value")).Value = Convert.ToString(entry.Value, CultureInfo.InvariantCulture);
                }
                else if (variableType.IsSerializable)
                {
                    variable.AppendChild(root.OwnerDocument.CreateCDataSection(BugSerializer.Serialize(entry.Value)));
                }
            }
        }

        /// <summary>
        /// Saves the application.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveApplication(XmlNode root, HttpContext context)
        {
            XmlNode session = root.AppendChild(root.OwnerDocument.CreateElement("applicationVariables"));
            foreach (string key in context.Application.Keys)
            {
                XmlNode variable = session.AppendChild(root.OwnerDocument.CreateElement("add"));
                variable.Attributes.Append(root.OwnerDocument.CreateAttribute("name")).Value = key;
                object value = context.Application[key];
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
        /// Saves the cache.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveCache(XmlNode root, HttpContext context)
        {
            XmlNode session = root.AppendChild(root.OwnerDocument.CreateElement("cacheVariables"));
            foreach (DictionaryEntry entry in context.Cache)
            {
                string key = entry.Key.ToString();
                XmlNode variable = session.AppendChild(root.OwnerDocument.CreateElement("add"));
                variable.Attributes.Append(root.OwnerDocument.CreateAttribute("name")).Value = key;
                if (entry.Value == null)
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("value")).Value = "null";
                    continue;
                }
                Type variableType = entry.Value.GetType();
                variable.Attributes.Append(root.OwnerDocument.CreateAttribute("type")).Value = variableType.AssemblyQualifiedName;
                if (variableType.IsValueType || variableType == typeof(string))
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("value")).Value = Convert.ToString(entry.Value, CultureInfo.InvariantCulture);
                }
                else if (variableType.IsSerializable)
                {
                    variable.AppendChild(root.OwnerDocument.CreateCDataSection(BugSerializer.Serialize(entry.Value)));
                }
            }
        }

        /// <summary>
        /// Saves the session.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveSession(XmlNode root, HttpContext context)
        {
            if (context.Session == null)
            {
                return;
            }
            XmlNode session = root.AppendChild(root.OwnerDocument.CreateElement("sessionVariables"));
            session.Attributes.Append(root.OwnerDocument.CreateAttribute("mode")).Value = context.Session.Mode.ToString();
            session.Attributes.Append(root.OwnerDocument.CreateAttribute("codePage")).Value = context.Session.CodePage.ToString(CultureInfo.InvariantCulture);
            foreach (string key in context.Session.Keys)
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
            if (context.Request == null)
            {
                return;
            }
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
            if (context.Request != null)
            {
                HttpValueCollection.SaveCollectionToXmlNode(context.Request.QueryString,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("queryString")));
                HttpValueCollection.SaveCollectionToXmlNode(context.Request.Form,
                                                            root.AppendChild(root.OwnerDocument.CreateElement("form")));
                HttpValueCollection.SaveCollectionToXmlNode(context.Request.Headers,
                                                            root.AppendChild(root.OwnerDocument.CreateElement("headers")));
            }
            if (context.Server != null)
            {
                root.AppendChild(root.OwnerDocument.CreateElement("machineName")).InnerText = context.Server.MachineName;
                root.AppendChild(root.OwnerDocument.CreateElement("scriptTimeout")).InnerText = context.Server.ScriptTimeout.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveUser(XmlNode root, HttpContext context)
        {
            if (context.User != null && context.User.GetType().IsSerializable)
            {
                XmlNode user = root.AppendChild(root.OwnerDocument.CreateElement("user"));
                user.Attributes.Append(root.OwnerDocument.CreateAttribute("name")).Value = context.User.Identity.Name;
                user.Attributes.Append(root.OwnerDocument.CreateAttribute("authenticationType")).Value = context.User.Identity.AuthenticationType;
                user.AppendChild(root.OwnerDocument.CreateCDataSection(BugSerializer.Serialize(context.User)));
            }
        }

        /// <summary>
        /// Saves the exception.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveException(XmlNode root, HttpContext context)
        {
            if (context.Error == null)
            {
                return;
            }
            XmlNode exception = root.AppendChild(root.OwnerDocument.CreateElement("exception"));
            Exception baseException = context.Error.GetBaseException();
            exception.Attributes.Append(root.OwnerDocument.CreateAttribute("type")).Value = baseException.GetType().FullName;
            exception.Attributes.Append(root.OwnerDocument.CreateAttribute("message")).Value = baseException.Message;
            exception.Attributes.Append(root.OwnerDocument.CreateAttribute("source")).Value = ExceptionHelper.GetRelevantSource(context.Error)??baseException.Source;
            exception.AppendChild(root.OwnerDocument.CreateCDataSection(BugSerializer.Serialize(context.Error)));
            ExceptionHelper.XmlSerializeStackTrace(baseException, root.AppendChild(root.OwnerDocument.CreateElement("stackTrace")));
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
            if(string.IsNullOrEmpty(stackTrace))
            {
                return string.Empty;
            }
            return Regex.Replace(stackTrace, @"\) in \w\:[/\\].+", ")");
        }

        /// <summary>
        /// Raises the error event.
        /// </summary>
        public static event BugEventHandler Error;

        /// <summary>
        /// Raises the error event.
        /// </summary>
        /// <param name="e">The <see cref="Bugx.Web.BugEventArgs"/> instance containing the event data.</param>
        static void OnError(BugEventArgs e)
        {
            if (Error != null)
            {
                SafeRunner.Invoke(delegate {Error(HttpContext.Current.ApplicationInstance, e);});
            }
        }

        /// <summary>
        /// Raises the error complete event.
        /// </summary>
        public static event BugEventHandler ErrorComplete;

        /// <summary>
        /// Raises the error complete event.
        /// </summary>
        /// <param name="e">The <see cref="Bugx.Web.BugEventArgs"/> instance containing the event data.</param>
        static void OnErrorComplete(BugEventArgs e)
        {
            if (ErrorComplete != null)
            {
                SafeRunner.Invoke(delegate {ErrorComplete(HttpContext.Current.ApplicationInstance, e);});
            }
        }

        public static event ApplicationUnloadEventHandler ApplicationUnload;

        /// <summary>
        /// Test purpose.
        /// </summary>
        static int UnloadCounter;

        /// <summary>
        /// Raises the application unload event.
        /// </summary>
        /// <param name="e">The <see cref="Bugx.Web.ApplicationUnloadEventArgs"/> instance containing the event data.</param>
        static void OnApplicationUnload(ApplicationUnloadEventArgs e)
        {
            if (ApplicationUnload != null && ++UnloadCounter <= 2)
            {
                ApplicationUnload(null, e);
            }
        }
    }
}