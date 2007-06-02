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
using Bugx.Web.Collections;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Xml.XPath;
using System.Xml;

namespace Bugx.Web
{
    /// <summary>
    /// Helper about client navigators, user-agents, ...
    /// </summary>
    public static class BrowserHelper
    {
        const string KnownBotsCacheKey = "Bugx.Web.BrowserHelper.KnownBots";

        /// <summary>
        /// Gets the bots data base file.
        /// </summary>
        /// <value>The bots data base file.</value>
        static string BotsDataBaseFile
        {
            get{ return HttpContext.Current.Server.MapPath("~/bugx/bots.xml"); }
        }

        /// <summary>
        /// Gets the known bots collection.
        /// </summary>
        /// <value>The known bots collection.</value>
        static SetCollection<int> KnownBots
        {
            get
            {
                Cache cache = HttpRuntime.Cache;
                if (cache == null)
                {
                    throw new NotImplementedException("No implementation outside web application.");
                }
                SetCollection<int> knownBots = cache[KnownBotsCacheKey] as SetCollection<int>;
                if (knownBots == null)
                {
                    knownBots = BuildBotSet();
                    cache.Add(KnownBotsCacheKey, knownBots, new CacheDependency(BotsDataBaseFile), Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), CacheItemPriority.Normal, null);
                }
                return knownBots;
            }    
        }

        /// <summary>
        /// Builds the bot set.
        /// </summary>
        /// <returns></returns>
        static SetCollection<int> BuildBotSet()
        {
            SetCollection<int> result = new SetCollection<int>();
            string fileName = BotsDataBaseFile;
            if (File.Exists(fileName))
            {
                using (Stream xmlFile = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    XPathNavigator xml = new XPathDocument(xmlFile).CreateNavigator();
                    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xml.NameTable);
                    namespaceManager.AddNamespace("bugx", "http://www.wavenet.be/bugx/web/bots.xsd");
                    foreach (XPathNavigator node in xml.Select("/bugx:bots/bugx:bot", namespaceManager))
                    {
                        result.Add(node.Value.GetHashCode());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether the specified user agent is a known bot.
        /// </summary>
        /// <param name="userAgent">The user agent.</param>
        /// <returns>
        /// 	<c>true</c> if the specified user agent is a known bot; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKnownBot(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return false;
            }
            return KnownBots.Contains(userAgent.Trim().GetHashCode());
        }
    }
}
