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
using System.Web.Caching;
using Bugx.Web;
using System.ComponentModel;
using System.Collections;
using System.Xml;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Security.Principal;
using System.Runtime.Serialization;

namespace Bugx.ReBug
{
    [Serializable]
    public class ReBugContext
    {
        #region Fields
        Uri _Url;
        HttpValueCollection _Form;
        HttpValueCollection _QueryString;
        HttpValueCollection _Cookies;
        HttpValueCollection _Headers;
        Dictionary<string, object> _Session;
        Dictionary<string, object> _Cache;
        Dictionary<string, object> _Application;
        Dictionary<object, object> _Context;
        string _PathInfo;
        Exception _Exception;
        IPrincipal _User;
        string _MachineName;
        int? _ScriptTimeout;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ReBugContext"/> class.
        /// </summary>
        /// <param name="bug">The bug.</param>
        protected ReBugContext(XmlDocument bug)
        {
            _QueryString = HttpValueCollection.CreateCollectionFromXmlNode(bug.SelectSingleNode("/bugx/queryString"));

            _Form = HttpValueCollection.CreateCollectionFromXmlNode(bug.SelectSingleNode("/bugx/form"));

            XmlNode cookie = bug.SelectSingleNode("/bugx/headers/Cookie");
            if (cookie != null)
            {
                _Cookies = HttpValueCollection.CreateCollectionFromCookieHeader(cookie.InnerText);
            }
            _Headers = HttpValueCollection.CreateCollectionFromXmlNode(bug.SelectSingleNode("/bugx/headers"));

            _Session = FillNameValue(bug.SelectNodes("/bugx/sessionVariables/add"));
            _Cache = FillNameValue(bug.SelectNodes("/bugx/cacheVariables/add"));
            _Application = FillNameValue(bug.SelectNodes("/bugx/applicationVariables/add"));
            _Context = FillHashtable(bug.SelectNodes("/bugx/contextVariables/add"));

            XmlNode exception = bug.SelectSingleNode("/bugx/exception");
            if (exception != null)
            {
                try
                {
                    _Exception = (Exception)BugSerializer.Deserialize(exception.InnerText);
                }catch(SerializationException){}
            }
            XmlNode url = bug.SelectSingleNode("/bugx/url");
            if (url != null)
            {
                _Url = new Uri(url.InnerText);
            }
            XmlNode pathInfo = bug.SelectSingleNode("/bugx/pathInfo");
            if (pathInfo != null)
            {
                _PathInfo = pathInfo.InnerText;
            }
            XmlNode machineName = bug.SelectSingleNode("/bugx/machineName");
            if (machineName != null)
            {
                _MachineName = machineName.InnerText;
            }
            XmlNode scriptTimeout = bug.SelectSingleNode("/bugx/scriptTimeout");
            if (scriptTimeout != null)
            {
                _ScriptTimeout = Convert.ToInt32(scriptTimeout.InnerText);
            }
            XmlNode user = bug.SelectSingleNode("/bugx/user");
            if (user != null)
            {
                try
                {
                    _User = (IPrincipal)BugSerializer.Deserialize(user.InnerText);
                }
                catch(SerializationException){}
                catch(TargetInvocationException){}
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ReBugContext"/> class.
        /// </summary>
        /// <param name="bug">The bug.</param>
        /// <returns></returns>
        public static ReBugContext Create(XmlDocument bug)
        {
            Type custom = Type.GetType(ConfigurationManager.AppSettings["CustomReBugContext"] ?? typeof(ReBugContext).AssemblyQualifiedName );
            if (custom != null && typeof(ReBugContext).IsAssignableFrom(custom))
            {
                ConstructorInfo constructor = custom.GetConstructor(BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.NonPublic, null, new Type[] {typeof (XmlDocument)}, null);
                if (constructor != null)
                {
                    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                        return (ReBugContext) constructor.Invoke(new object[] {bug});
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentCulture = currentCulture;
                    }
                }
            }
            return new ReBugContext(bug);
        } 
        #endregion

        #region Helpers
        /// <summary>
        /// Fills the name value.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns></returns>
        static Dictionary<string, object> FillNameValue(XmlNodeList nodes)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (XmlNode node in nodes)
            {
                object data;
                if (node.Attributes["value"] != null)
                {
                    data = Convert.ChangeType(node.Attributes["value"].Value, Type.GetType(node.Attributes["type"].Value));
                }
                else if (!string.IsNullOrEmpty(node.InnerText))
                {
                    try
                    {
                        data = BugSerializer.Deserialize(node.InnerText);
                    }
                    catch(SerializationException)
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
                result[node.Attributes["name"].Value] = data;
            }
            return result;
        }

        /// <summary>
        /// Fills the hashtable.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns></returns>
        static Dictionary<object, object> FillHashtable(XmlNodeList nodes)
        {
            Dictionary<object, object> result = new Dictionary<object, object>();
            foreach (XmlNode node in nodes)
            {
                object data;
                if (node.Attributes["value"] != null)
                {
                    data = Convert.ChangeType(node.Attributes["value"].Value, Type.GetType(node.Attributes["type"].Value));
                }
                else if (!string.IsNullOrEmpty(node.InnerText))
                {
                    try
                    {
                        data = BugSerializer.Deserialize(node.InnerText);    
                    }
                    catch(SerializationException)
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
                if (node.Attributes["nameType"] != null)
                {
                    result[Convert.ChangeType(node.Attributes["name"].Value, Type.GetType(node.Attributes["nameType"].Value))] = data;
                }
                else
                {
                    try
                    {
                        result[BugSerializer.Deserialize(node.SelectSingleNode("key").InnerText)] = data;
                    }catch(SerializationException){}
                }
            }
            return result;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets information about the URL of the current request.
        /// </summary>
        /// <value>The URL.</value>
        [Category("Request")]
        [Description("Gets information about the URL of the current request.")]
        public virtual Uri Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value;
            }
        }

        /// <summary>
        /// Gets a collection of form variables.
        /// </summary>
        /// <value>The form.</value>
        [Category("Request")]
        [Description("Gets a collection of form variables.")]
        public virtual HttpValueCollection Form
        {
            get
            {
                return _Form;
            }
        }

        /// <summary>
        /// Gets the collection of HTTP query string variables.
        /// </summary>
        [Category("Request")]
        [Description("Gets the collection of HTTP query string variables.")]
        public virtual HttpValueCollection QueryString
        {
            get
            {
                return _QueryString;
            }
        }

        /// <summary>
        /// Gets a collection of cookies sent by the client.
        /// </summary>
        [Category("Request")]
        [Description("Gets a collection of cookies sent by the client.")]
        public virtual HttpValueCollection Cookies
        {
            get
            {
                return _Cookies;
            }
        }

        /// <summary>
        /// Gets a collection of HTTP headers.
        /// </summary>
        [Category("Request")]
        [Description("Gets a collection of HTTP headers.")]
        public virtual HttpValueCollection Headers
        {
            get
            {
                return _Headers;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.SessionState.HttpSessionState"/> object for the current HTTP request.
        /// </summary>
        [Category("Environment")]
        [Description("Gets the System.Web.SessionState.HttpSessionState object for the current HTTP request.")]
        public virtual Dictionary<string, object> Session
        {
            get
            {
                return _Session;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.Caching.Cache"/> object for the current HTTP request.
        /// </summary>
        [Category("Environment")]
        [Description("Gets the System.Web.Caching.Cache object for the current HTTP request.")]
        public virtual Dictionary<string, object> Cache
        {
            get
            {
                return _Cache;
            }
        }

        /// <summary>
        /// Gets a key/value collection that can be used to organize and share data between an <see cref="System.Web.IHttpModule"/> interface and an <see cref="System.Web.IHttpHandler"/> interface during an HTTP request.
        /// </summary>
        [Category("Environment")]
        [Description("Gets a key/value collection that can be used to organize and share data between an System.Web.IHttpModule interface and an System.Web.IHttpHandler interface during an HTTP request.")]
        public virtual Dictionary<object, object> Context
        {
            get
            {
                return _Context;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.HttpApplicationState"/> object for the current HTTP request.
        /// </summary>
        [Category("Environment")]
        [Description("Gets the System.Web.HttpApplicationState object for the current HTTP request.")]
        public virtual Dictionary<string, object> Application
        {
            get
            {
                return _Application;
            }
        }

        /// <summary>
        /// Gets the first error accumulated during HTTP request processing.
        /// </summary>
        [Category("User - Exception")]
        [Description("Gets the first error accumulated during HTTP request processing.")]
        public virtual Exception Exception
        {
            get
            {
                return _Exception;
            }
            set
            {
                _Exception = value;
            }
        }

        /// <summary>
        /// Gets additional path information for a resource with a URL extension.
        /// </summary>
        [Category("Request")]
        [Description("Gets additional path information for a resource with a URL extension.")]
        public virtual string PathInfo
        {
            get{ return _PathInfo; }
            set{ _PathInfo = value; }
        } 

        /// <summary>
        /// Gets or sets security information for the current HTTP request.
        /// </summary>
        [Category("Environment")]
        [Description("Security information for the current HTTP request.")]
        public virtual IPrincipal User
        {
            get { return _User; }
            set { _User = value; }
        }

        /// <summary>
        /// Gets the server&apos;s computer name.
        /// </summary>
        [Category("Environment")]
        [Description("The name of the local computer.")]
        public virtual string MachineName
        {
            get { return _MachineName; }
        }

        /// <summary>
        /// Gets and sets the request time-out value in seconds.
        /// </summary>
        /// <value>The time-out value setting for requests.</value>
        [Category("Environment")]
        [Description("The time-out value setting for requests.")]
        public virtual int? ScriptTimeout
        {
            get { return _ScriptTimeout; }
            set{ _ScriptTimeout = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Restores the environment.
        /// </summary>
        public virtual void RestoreEnvironment()
        {
            HttpContext context = HttpContext.Current;
            HttpSessionState session = context.Session;
            if (session != null)
            {
                foreach (string key in Session.Keys)
                {
                    session[key] = Session[key];
                }
            }
            Cache cache = context.Cache;
            foreach (string key in Cache.Keys)
            {
                cache[key] = Cache[key];
            }
            IDictionary contextItems = context.Items;
            foreach (object key in Context.Keys)
            {
                contextItems[key] = Context[key];
            }
            HttpApplicationState application = context.Application;
            foreach (string key in Application.Keys)
            {
                application[key] = Application[key];
            }
            if (ScriptTimeout != null)
            {
                context.Server.ScriptTimeout = ScriptTimeout.Value;
            }
        } 
        #endregion

    }
}
