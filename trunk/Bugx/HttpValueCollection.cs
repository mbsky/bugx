using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;

namespace Bugx.Web
{
    [Serializable]
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
                collection.Add(nameValue.Name, nameValue.InnerText);
            }
        }

        public static HttpValueCollection CreateCollectionFromXmlNode(XmlNode node)
        {
            HttpValueCollection result = new HttpValueCollection();
            LoadCollectionFromXmlNode(result, node);
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
                node.AppendChild(node.OwnerDocument.CreateElement(key)).InnerText = collection[key];
            }
        }
    }
}