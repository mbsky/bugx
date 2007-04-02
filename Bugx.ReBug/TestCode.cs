using System;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Configuration;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using Bugx.Web;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Globalization;
namespace Bugx.ReBug
{
    public class ReBugWorkerRequest : SimpleWorkerRequest
    {
        static Dictionary<string, int> _Headers = new Dictionary<string, int>();
        static string[] _HeaderList = new string[RequestHeaderMaximum];
        static ReBugWorkerRequest()
        {
            DefineHeader(0, "Cache-Control", "HTTP_CACHE_CONTROL");
            DefineHeader(1, "Connection", "HTTP_CONNECTION");
            DefineHeader(2, "Date", "HTTP_DATE");
            DefineHeader(3, "Keep-Alive", "HTTP_KEEP_ALIVE");
            DefineHeader(4, "Pragma", "HTTP_PRAGMA");
            DefineHeader(5, "Trailer", "HTTP_TRAILER");
            DefineHeader(6, "Transfer-Encoding", "HTTP_TRANSFER_ENCODING");
            DefineHeader(7, "Upgrade", "HTTP_UPGRADE");
            DefineHeader(8, "Via", "HTTP_VIA");
            DefineHeader(9, "Warning", "HTTP_WARNING");
            DefineHeader(10, "Allow", "HTTP_ALLOW");
            DefineHeader(11, "Content-Length", "HTTP_CONTENT_LENGTH");
            DefineHeader(12, "Content-Type", "HTTP_CONTENT_TYPE");
            DefineHeader(13, "Content-Encoding", "HTTP_CONTENT_ENCODING");
            DefineHeader(14, "Content-Language", "HTTP_CONTENT_LANGUAGE");
            DefineHeader(15, "Content-Location", "HTTP_CONTENT_LOCATION");
            DefineHeader(0x10, "Content-MD5", "HTTP_CONTENT_MD5");
            DefineHeader(0x11, "Content-Range", "HTTP_CONTENT_RANGE");
            DefineHeader(0x12, "Expires", "HTTP_EXPIRES");
            DefineHeader(0x13, "Last-Modified", "HTTP_LAST_MODIFIED");
            DefineHeader(20, "Accept", "HTTP_ACCEPT");
            DefineHeader(0x15, "Accept-Charset", "HTTP_ACCEPT_CHARSET");
            DefineHeader(0x16, "Accept-Encoding", "HTTP_ACCEPT_ENCODING");
            DefineHeader(0x17, "Accept-Language", "HTTP_ACCEPT_LANGUAGE");
            DefineHeader(0x18, "Authorization", "HTTP_AUTHORIZATION");
            DefineHeader(0x19, "Cookie", "HTTP_COOKIE");
            DefineHeader(0x1a, "Expect", "HTTP_EXPECT");
            DefineHeader(0x1b, "From", "HTTP_FROM");
            DefineHeader(0x1c, "Host", "HTTP_HOST");
            DefineHeader(0x1d, "If-Match", "HTTP_IF_MATCH");
            DefineHeader(30, "If-Modified-Since", "HTTP_IF_MODIFIED_SINCE");
            DefineHeader(0x1f, "If-None-Match", "HTTP_IF_NONE_MATCH");
            DefineHeader(0x20, "If-Range", "HTTP_IF_RANGE");
            DefineHeader(0x21, "If-Unmodified-Since", "HTTP_IF_UNMODIFIED_SINCE");
            DefineHeader(0x22, "Max-Forwards", "HTTP_MAX_FORWARDS");
            DefineHeader(0x23, "Proxy-Authorization", "HTTP_PROXY_AUTHORIZATION");
            DefineHeader(0x24, "Referer", "HTTP_REFERER");
            DefineHeader(0x25, "Range", "HTTP_RANGE");
            DefineHeader(0x26, "TE", "HTTP_TE");
            DefineHeader(0x27, "User-Agent", "HTTP_USER_AGENT");
        }

        private static void DefineHeader(int index, string headerName, string serverVariable)
        {
            _HeaderList[index] = headerName;
            _Headers[headerName] = index;
        }

        

        byte[] PostedData;
        public override string GetHttpVerbName()
        {
            if (PostedData == null)
            {
                return base.GetHttpVerbName();
            }
            return "POST";
        }
        public override string GetLocalAddress()
        {
            return _RequestUri.Host;
        }
        public override int GetLocalPort()
        {
            return _RequestUri.Port;
        }
        public override string GetProtocol()
        {
            return _RequestUri.Scheme;
        }
        public override bool IsSecure()
        {
            return string.Compare(_RequestUri.Scheme, "https", true, CultureInfo.InvariantCulture) == 0;
        }
        public override string GetPathInfo()
        {
            return _PathInfo;
        }
        public override bool IsEntireEntityBodyIsPreloaded()
        {
            return true;
        }

        public override byte[] GetPreloadedEntityBody()
        {
            if (PostedData == null)
            {
                return base.GetPreloadedEntityBody();
            }
            return PostedData;
        }

        public override string GetKnownRequestHeader(int index)
        {
            if (_SessionVariables != null)
            {
                HttpSessionState session = HttpContext.Current.Session;
                BinaryFormatter deserializer = new BinaryFormatter();
                foreach (XmlNode sessionVariable in _SessionVariables)
                {
                    try
                    {
                        Type type = Type.GetType(sessionVariable.Attributes["type"].Value);
                        if (type != null && (type.IsValueType || type == typeof(string)))
                        {
                            session[sessionVariable.Attributes["name"].Value] = Convert.ChangeType(sessionVariable.Attributes["value"].Value, type, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            using (Stream reader = new MemoryStream(Convert.FromBase64String(sessionVariable.InnerText)))
                            {
                                session[sessionVariable.Attributes["name"].Value] = deserializer.Deserialize(reader);
                            }
                        }
                    }
                    catch
                    {
                     /*
                      * No error when we deserialize session variable, this only support for some specific exception scenarios
                      */
                    }
                }
                _SessionVariables = null;
            }
            return _HeaderCollection[_HeaderList[index]];
        }

        Uri _RequestUri;
        string _PathInfo;
        HttpValueCollection _HeaderCollection;
        XmlNodeList _SessionVariables;
        public ReBugWorkerRequest(Uri requestUri, string pathInfo, HttpValueCollection query, HttpValueCollection post, HttpValueCollection headers, XmlNodeList sessionVariables, TextWriter output)
            : base(requestUri.AbsolutePath.Substring(1), query.ToString(), output)
        {
            _PathInfo = pathInfo;
            _RequestUri = requestUri;
            _HeaderCollection = headers;
            _SessionVariables = sessionVariables;
            if (post.Count > 0)
            {
                PostedData = Encoding.UTF8.GetBytes(post.ToString());
            }
        }
    }
    public class ReBugHost : MarshalByRefObject
    {
        public void Process()
        {
            XmlDocument xml = new BugDocument();
            xml.Load(@"C:\Documents and Settings\Olivier Bossaer\My Documents\Visual Studio 2005\Projects\Bugx\Bugx.TestSite\bugx\errors\System\DivideByZeroException\3077697825-1797930606\20070402T191529Z.bugx");
            HttpWorkerRequest swr = new ReBugWorkerRequest(new Uri(xml.SelectSingleNode("/bugx/url").InnerText),
                                                        xml.SelectSingleNode("/bugx/pathInfo").InnerText,
                                                        HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/queryString")),
                                                        HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/form")),
                                                        HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/headers")),
                                                        xml.SelectNodes("/bugx/sessionVariables/add"),
                                                        Console.Out);
            //System.Diagnostics.Debugger.Break();
            HttpRuntime.ProcessRequest(swr);
        }
    }
    class TestCode
    {
        [STAThread]
        static void Main()
        {
            Console.ReadLine();
            DirectoryInfo webPath = new DirectoryInfo(Environment.CurrentDirectory).Parent;
            ReBugHost host = (ReBugHost)ApplicationHost.CreateApplicationHost(
                                                           typeof(ReBugHost),
                                                           "/",
                                                           webPath.FullName);
            host.Process();
        }
    }
}
