using System;
using System.Configuration;
using System.Xml;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Bugx.Web.Configuration
{
    public static class BugxConfiguration
    {
        /// <summary>
        /// XML Namespace URI.
        /// </summary>
        const string XmlNamespace = "http://www.wavenet.be/bugx/web/configuration.xsd";

        /// <summary>
        /// XML namespace manager.
        /// </summary>
        static readonly XmlNamespaceManager NamespaceManager = LoadNamespace();

        /// <summary>
        /// Loads the namespace.
        /// </summary>
        /// <returns></returns>
        static XmlNamespaceManager LoadNamespace()
        {
            XmlNamespaceManager xmlNamespace = new XmlNamespaceManager(new XmlDocument().NameTable);
            xmlNamespace.AddNamespace("bugx", XmlNamespace);
            return xmlNamespace;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        static XmlNode Configuration
        {
            get
            {
                XmlNode result = ConfigurationManager.GetSection("bugx") as XmlNode;
                if (result == null)
                {//Load default values.
                    XmlDocument document = new XmlDocument();
                    result = document.AppendChild(document.CreateElement("bugx", XmlNamespace));
                    result.AppendChild(document.CreateElement("dataToSave", XmlNamespace)).InnerText = "All";
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the data to save.
        /// </summary>
        /// <value>The data to save.</value>
        public static DataToSave DataToSave
        {
            get
            {
                XmlNode dataToSave = Configuration.SelectSingleNode("bugx:dataToSave", NamespaceManager);
                if (dataToSave != null)
                {
                    try
                    {
                        return (DataToSave) Enum.Parse(typeof (DataToSave), dataToSave.InnerText.Trim());
                    }
                    catch(ArgumentException){}
                }
                return DataToSave.All;
            }
        }

        /// <summary>
        /// Gets the max error reporting per minute.
        /// </summary>
        /// <value>The max error reporting per minute or null if unlimited.</value>
        public static int? MaxErrorReportingPerMinute
        {
            get
            {
                XmlNode maxErrorReportingPerMinute = Configuration.SelectSingleNode("bugx:maxErrorReportingPerMinute", NamespaceManager);
                if (maxErrorReportingPerMinute != null)
                {
                    int result;
                    if (Int32.TryParse(maxErrorReportingPerMinute.InnerText, out result))
                    {
                        return result;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the filters.
        /// </summary>
        /// <value>The filters.</value>
        public static StringCollection Filters
        {
            get
            {
                StringCollection result = new StringCollection();
                foreach (XmlNode filter in Configuration.SelectNodes("bugx:filters/bugx:filter", NamespaceManager))
                {
                    string filterText = filter.InnerText;
                    if (!string.IsNullOrEmpty(filterText))
                    {
                        result.Add(filterText);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Determines if specified error ID should be filtered.
        /// </summary>
        /// <param name="errorId">The error id.</param>
        /// <returns><c>true</c> if specified error ID should be filtered; Otherwise <c>false</c></returns>
        public static bool ShouldBeFiltered(string errorId)
        {
            foreach (XmlNode filter in Configuration.SelectNodes("bugx:filters/bugx:filter", NamespaceManager))
            {
                if (Regex.IsMatch(errorId, filter.InnerText, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
