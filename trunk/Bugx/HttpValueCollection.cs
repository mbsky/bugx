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
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Bugx.Web
{
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    public class HttpValueCollection : NameValueCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpValueCollection"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object that contains the information required to serialize the new <see cref="T:System.Collections.Specialized.NameValueCollection"></see> instance.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> object that contains the source and destination of the serialized stream associated with the new <see cref="T:System.Collections.Specialized.NameValueCollection"></see> instance.</param>
        protected HttpValueCollection(
                   SerializationInfo info, StreamingContext context):base(info, context)
        {}

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (string key in AllKeys)
            {
                result.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(this[key]));
            }
            if (result.Length == 0)
            {
                return string.Empty;
            }
            return result.ToString(0, result.Length - 1);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            HttpValueCollection collection = obj as HttpValueCollection;
            if (collection == null)
            {
                return false;
            }
            return collection.ToString() == ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpValueCollection"/> class.
        /// </summary>
        public HttpValueCollection() : base(StringComparer.CurrentCultureIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpValueCollection"/> class.
        /// </summary>
        /// <param name="innerCollection">The inner collection.</param>
        public HttpValueCollection(NameValueCollection innerCollection) : base(innerCollection)
        {
        }

        /// <summary>
        /// Loads from node.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void LoadFromNode(XmlNode node)
        {
            if (node == null)
            {
                return;
            }
            LoadCollectionFromXmlNode(this, node);
        }

        public static void LoadCollectionFromXmlNode(NameValueCollection collection, XmlNode node)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            foreach (XmlNode nameValue in node.SelectNodes("*"))
            {
                collection.Add(XmlConvert.DecodeName(nameValue.Name), nameValue.InnerText);
            }
        }

        /// <summary>
        /// Creates the collection from XML node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static HttpValueCollection CreateCollectionFromXmlNode(XmlNode node)
        {
            HttpValueCollection result = new HttpValueCollection();
            if (node != null)
            {
                LoadCollectionFromXmlNode(result, node);
            }
            return result;
        }

        /// <summary>
        /// Creates the collection from URL encoded.
        /// </summary>
        /// <param name="urlEncoded">The URL encoded.</param>
        /// <returns></returns>
        public static HttpValueCollection CreateCollectionFromUrlEncoded(string urlEncoded)
        {
            if (string.IsNullOrEmpty(urlEncoded))
            {
                return new HttpValueCollection();
            }
            HttpValueCollection result = new HttpValueCollection();
            foreach (string entry in urlEncoded.Split('&'))
            {
                if (entry.Trim().Length == 0)
                {
                    continue;
                }
                string[] nameValue = entry.Split('=');
                if (nameValue.Length == 1)
                {
                    result[HttpUtility.UrlDecode(nameValue[0])] = string.Empty;
                }
                else
                {
                    result[HttpUtility.UrlDecode(nameValue[0])] = HttpUtility.UrlDecode(nameValue[1]);
                }
            }
            return result;
        }

        /// <summary>
        /// Creates the collection from cookie header.
        /// </summary>
        /// <param name="cookieHeader">The cookie header.</param>
        /// <returns></returns>
        public static HttpValueCollection CreateCollectionFromCookieHeader(string cookieHeader)
        {
            if (string.IsNullOrEmpty(cookieHeader))
            {
                return new HttpValueCollection();
            }
            HttpValueCollection result = new HttpValueCollection();
            foreach (string entry in cookieHeader.Split(';'))
            {
                if (entry.Trim().Length == 0)
                {
                    continue;
                }
                string[] nameValue = entry.Split('=');
                if (nameValue.Length == 1)
                {
                    result[nameValue[0].Trim()] = string.Empty;
                }
                else
                {
                    result[nameValue[0].Trim()] = string.Join("=", nameValue, 1, nameValue.Length - 1);
                }
            }
            return result;
        }

        /// <summary>
        /// Saves to node.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void SaveToNode(XmlNode node)
        {
            SaveCollectionToXmlNode(this, node);
        }

        /// <summary>
        /// Saves the collection to XML node.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="node">The node.</param>
        public static void SaveCollectionToXmlNode(NameValueCollection collection, XmlNode node)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            foreach (string key in collection.AllKeys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    node.AppendChild(node.OwnerDocument.CreateElement(XmlConvert.EncodeName(key))).InnerText = collection[key];
                }
            }
        }
    }
}