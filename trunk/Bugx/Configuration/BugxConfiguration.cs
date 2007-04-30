using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Bugx.Web.Configuration
{
    public static class BugxConfiguration
    {
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
                    result = document.AppendChild(document.CreateElement("bugx"));
                    result.AppendChild(document.CreateElement("dataToSave")).InnerText = "All";
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
                XmlNode dataToSave = Configuration.SelectSingleNode("dataToSave");
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
                XmlNode maxErrorReportingPerMinute = Configuration.SelectSingleNode("maxErrorReportingPerMinute");
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
    }
}
