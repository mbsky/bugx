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

using System.IO;
using System.Xml;
using System.IO.Compression;
using System;
using System.Globalization;

namespace Bugx.Web
{
    /// <summary>
    /// BugDcoument is an <see cref="XmlDocument"/> with some quick access and with compression to avoid large XML files.
    /// </summary>
    public class BugDocument : XmlDocument
    {
        #region Properties
        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        public DateTime? CreationDate
        {
            get
            {
                if (DocumentElement == null || DocumentElement.Attributes["date"] == null)
                {
                    return null;
                }
                DateTime result;
                if (DateTime.TryParseExact(DocumentElement.Attributes["date"].Value,
                                           "yyyy-MM-ddTHH:mm:ss UTC",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.AssumeUniversal,
                                           out result))
                {
                    return result;
                }
                return null;
            }
            set
            {
                if (DocumentElement == null)
                {
                    throw new InvalidOperationException();
                }
                if (value == null)
                {
                    if (DocumentElement.Attributes["date"] != null)
                    {
                        DocumentElement.Attributes.Remove(DocumentElement.Attributes["date"]);
                    }
                }
                else
                {
                    DateTime date = value.Value;
                    if (date.Kind == DateTimeKind.Local)
                    {
                        date = date.ToUniversalTime();
                    }
                    XmlAttribute dateAttribute = DocumentElement.Attributes["date"] ?? DocumentElement.Attributes.Append(CreateAttribute("date"));
                    dateAttribute.Value = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-ddTHH:mm:ss} UTC", date);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the error is throw by a bot.
        /// </summary>
        /// <value><c>true</c> if the error is throw by a bot; otherwise, <c>false</c>.</value>
        public bool ErrorFromBot
        {
            get
            {
                if (DocumentElement == null || DocumentElement.Attributes["bot"] == null)
                {
                    return false;
                }
                return DocumentElement.Attributes["bot"].Value == "1";
            }
            set
            {
                if (DocumentElement == null)
                {
                    throw new InvalidOperationException();
                }
                XmlAttribute bot = DocumentElement.Attributes["bot"] ?? DocumentElement.Attributes.Append(CreateAttribute("bot"));
                bot.InnerText = value ? "1" : "0";
            }
        } 
        #endregion

        #region Load & Save
        /// <summary>
        /// Saves the XML document to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to which you want to save.</param>
        /// <exception cref="T:System.Xml.XmlException">The operation would not result in a well formed XML document (for example, no document element or duplicate XML declarations). </exception>
        public override void Save(Stream outStream)
        {
            using (Stream stream = new GZipStream(outStream, CompressionMode.Compress, true))
            {
                base.Save(stream);
            }
        }

        /// <summary>
        /// Saves the XML document to the specified file.
        /// </summary>
        /// <param name="filename">The location of the file where you want to save the document.</param>
        /// <exception cref="T:System.Xml.XmlException">The operation would not result in a well formed XML document (for example, no document element or duplicate XML declarations). </exception>
        public override void Save(string filename)
        {
            using (Stream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                Save(file);
            }
        }

        /// <summary>
        /// Loads the XML document from the specified URL.
        /// </summary>
        /// <param name="filename">URL for the file containing the XML document to load.</param>
        /// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
        public override void Load(string filename)
        {
            using (Stream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                Load(file);
            }
        }

        /// <summary>
        /// Loads the XML document from the specified stream.
        /// </summary>
        /// <param name="inStream">The stream containing the XML document to load.</param>
        /// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
        public override void Load(Stream inStream)
        {
            using (Stream stream = new GZipStream(inStream, CompressionMode.Decompress, true))
            {
                base.Load(stream);
            }
        } 
        #endregion
    }
}