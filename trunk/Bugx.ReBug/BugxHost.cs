using System;
using System.Web;
using System.Xml;
using Bugx.Web;

namespace Bugx.ReBug
{
    public class BugxHost : MarshalByRefObject
    {
        public void Process(string fileName)
        {
            ErrorModule.IsReBug = true;
            XmlDocument xml = new BugDocument();
            xml.Load(fileName);
            HttpWorkerRequest swr = new BugxWorkerRequest(new Uri(xml.SelectSingleNode("/bugx/url").InnerText),
                                                        xml.SelectSingleNode("/bugx/pathInfo").InnerText,
                                                        HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/queryString")),
                                                        HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/form")),
                                                        HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/headers")),
                                                        xml.SelectNodes("/bugx/sessionVariables/add"),
                                                        Console.Out);
            HttpRuntime.ProcessRequest(swr);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get{ return true; }
        }
    }
}
