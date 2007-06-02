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
using System.Xml.XPath;

namespace Bugx.Web.Collections
{

    /// <summary>
    /// Represents a collection similar to the <see cref="NameValueCollection"/> but with capacities to decode <c>QueryString</c>, <c>CookieHeader</c> and can be send to XSL Transformation for reporting purposes.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    public class HttpValueCollection : NameValueCollection, IXPathNavigable
    {
        #region HttpValueXPathNavigator
        class HttpValueXPathNavigator : XPathNavigator
        {
            HttpValueCollection _Collection;
            int _Index = -1;
            XmlNameTable _NameTable = new NameTable();
            bool _Value;

            /// <summary>
            /// Initializes a new instance of the <see cref="HttpValueXPathNavigator"/> class.
            /// </summary>
            /// <param name="collection">The collection.</param>
            public HttpValueXPathNavigator(HttpValueCollection collection)
            {
                _Collection = collection;
            }

            /// <summary>
            /// Gets the base URI for the current node.
            /// Returns <c>String.Empty</c> in this context.
            /// </summary>
            /// <value></value>
            /// <returns>The location from which the node was loaded, or <see cref="F:System.String.Empty"></see> if there is no value.</returns>
            public override string BaseURI
            {
                get
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// Creates a new <see cref="T:System.Xml.XPath.XPathNavigator"></see> positioned at the same node as this <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
            /// </summary>
            /// <returns>
            /// A new <see cref="T:System.Xml.XPath.XPathNavigator"></see> positioned at the same node as this <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
            /// </returns>
            public override XPathNavigator Clone()
            {
                HttpValueXPathNavigator clone = new HttpValueXPathNavigator(_Collection);
                clone._Index = _Index;
                clone._NameTable = _NameTable;
                clone._Value = _Value;
                return clone;
            }

            /// <summary>
            /// Gets a value indicating whether the current node is an empty element without an end element tag.
            /// </summary>
            /// <value></value>
            /// <returns>Returns true if the current node is an empty element; otherwise, false.</returns>
            public override bool IsEmptyElement
            {
                get
                {
                    return !HasChildren;
                }
            }

            /// <summary>
            /// Determines whether the current <see cref="T:System.Xml.XPath.XPathNavigator"></see> is at the same position as the specified <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
            /// </summary>
            /// <param name="other">The <see cref="T:System.Xml.XPath.XPathNavigator"></see> to compare to this <see cref="T:System.Xml.XPath.XPathNavigator"></see>.</param>
            /// <returns>
            /// Returns true if the two <see cref="T:System.Xml.XPath.XPathNavigator"></see> objects have the same position; otherwise, false.
            /// </returns>
            public override bool IsSamePosition(XPathNavigator other)
            {
                HttpValueXPathNavigator xpath = other as HttpValueXPathNavigator;
                return xpath != null && xpath._Collection == _Collection && xpath._Index == _Index;
            }

            /// <summary>
            /// Gets the <see cref="P:System.Xml.XPath.XPathNavigator.Name"></see> of the current node without any namespace prefix.
            /// </summary>
            /// <value></value>
            /// <returns>A <see cref="T:System.String"></see> that contains the local name of the current node, or <see cref="F:System.String.Empty"></see> if the current node does not have a name (for example, text or comment nodes).</returns>
            public override string LocalName
            {
                get
                {
                    return Name;
                }
            }

            /// <summary>
            /// Moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the same position as the specified <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
            /// </summary>
            /// <param name="other">The <see cref="T:System.Xml.XPath.XPathNavigator"></see> positioned on the node that you want to move to.</param>
            /// <returns>
            /// Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the same position as the specified <see cref="T:System.Xml.XPath.XPathNavigator"></see>; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
            /// </returns>
            public override bool MoveTo(XPathNavigator other)
            {
                HttpValueXPathNavigator xpath = other as HttpValueXPathNavigator;
                if (xpath == null || xpath._Collection != _Collection)
                {
                    return false;
                }
                _Index = xpath._Index;
                _NameTable = xpath._NameTable;
                _Value = xpath._Value;
                return true;
            }

            /// <summary>
            /// Moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the first attribute of the current node.
            /// In this context, returns <c>False</c>.
            /// </summary>
            /// <returns>
            /// Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the first attribute of the current node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
            /// </returns>
            public override bool MoveToFirstAttribute()
            {
                return false;
            }

            /// <summary>
            /// Moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the first child node of the current node.
            /// </summary>
            /// <returns>
            /// Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the first child node of the current node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
            /// </returns>
            public override bool MoveToFirstChild()
            {
                if (_Value || _Collection.Count == 0)
                {
                    return false;
                }
                if (_Index == -1)
                {
                    _Index = 0;
                }
                else
                {
                    _Value = true;
                }
                return true;
            }

            /// <summary>
            /// Moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the first namespace node that matches the <see cref="T:System.Xml.XPath.XPathNamespaceScope"></see> specified.
            /// </summary>
            /// <param name="namespaceScope">An <see cref="T:System.Xml.XPath.XPathNamespaceScope"></see> value describing the namespace scope.</param>
            /// <returns>
            /// Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the first namespace node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
            /// </returns>
            public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
            {
                return false;
            }

            /// <summary>
            /// Moves to the node that has an attribute of type ID whose value matches the specified <see cref="T:System.String"></see>.
            /// </summary>
            /// <param name="id">A <see cref="T:System.String"></see> representing the ID value of the node to which you want to move.</param>
            /// <returns>
            /// true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving; otherwise, false. If false, the position of the navigator is unchanged.
            /// </returns>
            public override bool MoveToId(string id)
            {
                for (int i = 0; i < _Collection.Count; i++)
                {
                    if (id == _Collection.Keys[i])
                    {
                        _Index = i;
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// Moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the next sibling node of the current node.
            /// </summary>
            /// <returns>
            /// true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the next sibling node; otherwise, false if there are no more siblings or if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is currently positioned on an attribute node. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
            /// </returns>
            public override bool MoveToNext()
            {
                if (_Index == -1 ||
                    _Value ||
                    _Index + 1 >= _Collection.Count)
                {
                    return false;
                }
                _Index++;
                return true;
            }

            /// <summary>
            /// Moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the next attribute.
            /// In this context, returns <c>False</c>.
            /// </summary>
            /// <returns>
            /// Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the next attribute; false if there are no more attributes. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
            /// </returns>
            public override bool MoveToNextAttribute()
            {
                return false;
            }

            /// <summary>
            /// Moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the next namespace node matching the <see cref="T:System.Xml.XPath.XPathNamespaceScope"></see> specified.
            /// In this context, returns <c>False</c>.
            /// </summary>
            /// <param name="namespaceScope">An <see cref="T:System.Xml.XPath.XPathNamespaceScope"></see> value describing the namespace scope.</param>
            /// <returns>
            /// Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the next namespace node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
            /// </returns>
            public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
            {
                return false;
            }

            /// <summary>
            /// Moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the parent node of the current node.
            /// </summary>
            /// <returns>
            /// Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the parent node of the current node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
            /// </returns>
            public override bool MoveToParent()
            {
                if (_Value)
                {
                    _Value = false;
                    return true;
                }
                if (_Index == -1)
                {
                    return false;
                }
                _Index = -1;
                return true;
            }

            /// <summary>
            /// Moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the previous sibling node of the current node.
            /// </summary>
            /// <returns>
            /// Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the previous sibling node; otherwise, false if there is no previous sibling node or if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is currently positioned on an attribute node. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
            /// </returns>
            public override bool MoveToPrevious()
            {
                if (_Value || _Index - 1 < 0)
                {
                    return false;
                }
                _Index--;
                return true;
            }

            /// <summary>
            /// Gets the qualified name of the current node.
            /// </summary>
            /// <value></value>
            /// <returns>A <see cref="T:System.String"></see> that contains the qualified <see cref="P:System.Xml.XPath.XPathNavigator.Name"></see> of the current node, or <see cref="F:System.String.Empty"></see> if the current node does not have a name (for example, text or comment nodes).</returns>
            public override string Name
            {
                get
                {
                    if (_Index != -1 && !_Value)
                    {
                        return XmlConvert.EncodeName(_Collection.Keys[_Index]);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            /// <summary>
            /// Gets the <see cref="T:System.Xml.XmlNameTable"></see> of the <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
            /// </summary>
            /// <value></value>
            /// <returns>An <see cref="T:System.Xml.XmlNameTable"></see> object enabling you to get the atomized version of a <see cref="T:System.String"></see> within the XML document.</returns>
            public override XmlNameTable NameTable
            {
                get
                {
                    return _NameTable;
                }
            }

            /// <summary>
            /// Gets the namespace URI of the current node.
            /// In this context, returns <c>String.Empty</c>.
            /// </summary>
            /// <value></value>
            /// <returns>A <see cref="T:System.String"></see> that contains the namespace URI of the current node, or <see cref="F:System.String.Empty"></see> if the current node has no namespace URI.</returns>
            public override string NamespaceURI
            {
                get
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// Gets the <see cref="T:System.Xml.XPath.XPathNodeType"></see> of the current node.
            /// </summary>
            /// <value></value>
            /// <returns>One of the <see cref="T:System.Xml.XPath.XPathNodeType"></see> values representing the current node.</returns>
            public override XPathNodeType NodeType
            {
                get
                {
                    if (_Index == -1)
                    {
                        return XPathNodeType.Root;
                    }
                    else if (!_Value)
                    {
                        return XPathNodeType.Element;
                    }
                    else
                    {
                        return XPathNodeType.Text;
                    }
                }
            }

            /// <summary>
            /// Gets the namespace prefix associated with the current node.
            /// In this context, returns <c>String.Empty</c>.
            /// </summary>
            /// <value></value>
            /// <returns>A <see cref="T:System.String"></see> that contains the namespace prefix associated with the current node.</returns>
            public override string Prefix
            {
                get
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// Gets the string value of the item.
            /// </summary>
            /// <value></value>
            /// <returns>The string value of the item.</returns>
            public override string Value
            {
                get
                {
                    if (_Index == -1)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return _Collection[_Index];
                    }
                }
            }
        } 
        #endregion

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
        public virtual void LoadFromNode(IXPathNavigable node)
        {
            if (node == null)
            {
                return;
            }
            LoadCollectionFromXmlNode(this, node);
        }

        /// <summary>
        /// Loads the collection from XML node.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="node">The node.</param>
        public static void LoadCollectionFromXmlNode(NameValueCollection collection, IXPathNavigable node)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            foreach (XPathNavigator nameValue in node.CreateNavigator().Select("*"))
            {
                collection.Add(XmlConvert.DecodeName(nameValue.Name), nameValue.Value);
            }
        }

        /// <summary>
        /// Creates the collection from XML node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static HttpValueCollection CreateCollectionFromXmlNode(IXPathNavigable node)
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
        public virtual void SaveToNode(IXPathNavigable node)
        {
            SaveCollectionToXmlNode(this, node);
        }

        /// <summary>
        /// Saves the collection to XML node.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="node">The node.</param>
        public static void SaveCollectionToXmlNode(NameValueCollection collection, IXPathNavigable node)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (collection.AllKeys.Length > 0)
            {
                XPathNavigator navigator = node.CreateNavigator();
                foreach (string key in collection.AllKeys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        navigator.AppendChildElement(string.Empty, XmlConvert.EncodeName(key), string.Empty, collection[key]);
                    }
                }
            }
        }

        #region IXPathNavigable Members

        /// <summary>
        /// Returns a new <see cref="T:System.Xml.XPath.XPathNavigator"></see> object.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.XPath.XPathNavigator"></see> object.
        /// </returns>
        public virtual XPathNavigator CreateNavigator()
        {
            return new HttpValueXPathNavigator(this);
        }

        #endregion
    }
}