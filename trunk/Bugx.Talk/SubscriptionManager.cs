/*
BUGx Talk: An Asp.Net Bug Tracking tool.
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
using System.Collections.ObjectModel;
using System.Web;
using System.IO;
using System.Xml.XPath;
using System.Web.Caching;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;

namespace Bugx.Talk
{
    public class SubscriptionManager: Collection<string>
    {
        const string XmlNameSpace = "http://www.wavenet.be/bugx/talk/Settings.xsd";

        string _User;
        string _Password;
        string _NetworkHost;
        string _Server;

        /// <summary>
        /// Gets the name of the setting file.
        /// </summary>
        static string _SettingFileName;

        bool _IsInitialization;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionManager"/> class.
        /// </summary>
        protected SubscriptionManager()
        {
            string fileName = _SettingFileName;
            if (File.Exists(fileName))
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    if (file.Length == 0)
                    {
                        return;
                    }
                    _IsInitialization = true;
                    XPathDocument document = new XPathDocument(file);
                    XPathNavigator xpath = document.CreateNavigator();
                    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xpath.NameTable);
                    namespaceManager.AddNamespace("bugx", XmlNameSpace);
                    _User        = xpath.SelectSingleNode("/bugx:bugx.talk/bugx:network/bugx:user", namespaceManager).Value;
                    _Password    = xpath.SelectSingleNode("/bugx:bugx.talk/bugx:network/bugx:password", namespaceManager).Value;
                    _Server      = xpath.SelectSingleNode("/bugx:bugx.talk/bugx:network/bugx:server", namespaceManager).Value;
                    _NetworkHost = xpath.SelectSingleNode("/bugx:bugx.talk/bugx:network/bugx:networkHost", namespaceManager).Value;
                    
                    foreach (XPathNavigator node in xpath.Select("/bugx:bugx.talk/bugx:subscriptions/bugx:subscription", namespaceManager))
                    {
                        Add(node.Value);
                    }
                    _IsInitialization = false;
                }
            }
        }

        static Cache _Cache;

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public static void LoadSettings()
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            {//Only availabe on request time.
                return;
            }
            _SettingFileName = context.Request.MapPath("~/bugx.talk.config");
            _Cache = context.Cache;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SubscriptionManager Instance
        {
            get
            {
                SubscriptionManager result = _Cache[typeof(SubscriptionManager).FullName] as SubscriptionManager;
                if (result == null)
                {
                    result = new SubscriptionManager();
                    SaveInCache(result);
                }
                return result;
            }
        }

        /// <summary>
        /// Saves the <c>subscriptionManager</c> in cache.
        /// </summary>
        /// <param name="subscriptionManager">The subscription manager.</param>
        static void SaveInCache(SubscriptionManager subscriptionManager)
        {
            _Cache.Add(typeof(SubscriptionManager).FullName, subscriptionManager, new CacheDependency(_SettingFileName), Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), CacheItemPriority.Low, null); 
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"></see> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.</exception>
        protected override void InsertItem(int index, string item)
        {
            if (Contains(item))
            {
                return;
            }
            base.InsertItem(index, item);
            if (_IsInitialization)
            {
                return;
            }
            using (FileStream file = new FileStream(_SettingFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                XmlDocument document = new XmlDocument();
                if (file.Length > 0)
                {
                    document.Load(file);
                }
                if (document.DocumentElement == null)
                {
                    document.AppendChild(document.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
                    document.AppendChild(document.CreateElement("subscriptions"));
                }
                document.DocumentElement.AppendChild(document.CreateElement("subscription")).InnerText = item;
                file.SetLength(0);
                document.Save(file);
            }
            SaveInCache(this);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.</exception>
        protected override void SetItem(int index, string item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1"></see>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is equal to or greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.</exception>
        protected override void RemoveItem(int index)
        {
            string name = this[index];
            base.RemoveItem(index);
            using (FileStream file = new FileStream(_SettingFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                XmlDocument document = new XmlDocument();
                document.Load(file);
                XPathNavigator xpath = document.CreateNavigator();
                foreach (XPathNavigator node in xpath.Select(string.Format(CultureInfo.InvariantCulture, "/subscriptions/subscription[.='{0}']", name)))
                {
                    node.DeleteSelf();
                }
                file.SetLength(0);
                document.Save(file);
            }
            SaveInCache(this);
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>The user.</value>
        public string User
        {
            get { return _User; }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return _Password; }
        }

        /// <summary>
        /// Gets the network host.
        /// </summary>
        /// <value>The network host.</value>
        public string NetworkHost
        {
            get { return _NetworkHost; }
        }

        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <value>The server.</value>
        public string Server
        {
            get { return _Server; }
        }

        
    }
}
