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
using System.Xml.XPath;
using System.Text.RegularExpressions;
using Bugx.Web.Collections;
using System.Xml.Xsl;
using System.Xml;
using System.Web;

namespace Bugx.Report
{
    /// <summary>
    /// Contains method to simplify XSL Transformation.
    /// </summary>
    public sealed class XsltExtension
    {
        /// <summary>
        /// Namespace of XSLT extension methods.
        /// </summary>
        public const string ExtensionNamespace = "http://www.wavenet.be/bugx/report/extensions";

        /// <summary>
        /// Registers the extension.
        /// </summary>
        /// <returns></returns>
        public static XsltArgumentList RegisterExtension()
        {
            return RegisterExtension(null);
        }

        /// <summary>
        /// Registers the extension.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static XsltArgumentList RegisterExtension(XsltArgumentList arguments)
        {
            if (arguments == null)
            {
                arguments = new XsltArgumentList();
            }
            arguments.AddExtensionObject(ExtensionNamespace, new XsltExtension());
            return arguments;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XsltExtension"/> class.
        /// </summary>
        XsltExtension(){}

        /// <summary>
        /// Creates the collection from cookie header.
        /// </summary>
        /// <param name="cookies">The cookie header.</param>
        /// <returns></returns>
        public IXPathNavigable CreateCollectionFromCookieHeader(string cookies)
        {
            return HttpValueCollection.CreateCollectionFromCookieHeader(cookies);
        }

        /// <summary>
        /// Avoids the template stretching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string AvoidTemplateStretching(string value)
        {
            return Regex.Replace(HttpUtility.HtmlEncode(value), @"((?:&[^;]{1, 10};|.|\s){10})", "$1<wbr />");
        }

        /// <summary>
        /// Decodes a name. This method does the reverse of the System.Xml.XmlConvert.EncodeName(System.String) and System.Xml.XmlConvert.EncodeLocalName(System.String) methods.
        /// </summary>
        /// <param name="name">The name to be transformed.</param>
        /// <returns>The decoded name.</returns>
        public string DecodeName(string name)
        {
            return XmlConvert.DecodeName(name);
        }
    }
}
