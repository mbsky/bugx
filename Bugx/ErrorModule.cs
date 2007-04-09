using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using ICSharpCode.SharpZipLib.GZip;

namespace Bugx.Web
{
    public class ErrorModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Error += new EventHandler(Application_Error);
        }

        static void Application_Error(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication) sender).Context;
            BugDocument bug = new BugDocument();
            XmlNode root = bug.AppendChild(bug.CreateElement("bugx"));
            SaveUrl(root, context);
            SaveRequest(root, context);
            SaveSession(root, context);
            SaveException(root, context);
            DirectoryInfo destination =
                new DirectoryInfo(context.Request.MapPath("~/bugx/errors/" + context.Error.GetBaseException().GetType().FullName.Replace('.', '/') + "/" + BuildUniqueIdentifier(context.Error)));
            if (!destination.Exists)
            {
                destination.Create();
            }
            bug.Save(destination.FullName + DateTime.Now.ToUniversalTime().ToString("/yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture) + ".bugx");
        }

        static void SaveSession(XmlNode root, HttpContext context)
        {
            XmlNode session = root.AppendChild(root.OwnerDocument.CreateElement("sessionVariables"));
            BinaryFormatter serializer = new BinaryFormatter();
            foreach (string key in HttpContext.Current.Session.Keys)
            {
                XmlNode variable = session.AppendChild(root.OwnerDocument.CreateElement("add"));
                variable.Attributes.Append(root.OwnerDocument.CreateAttribute("name")).Value = key;
                object value = context.Session[key];
                if (value == null)
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("value")).Value = "null";
                    continue;
                }
                Type variableType = value.GetType();
                variable.Attributes.Append(root.OwnerDocument.CreateAttribute("type")).Value = variableType.AssemblyQualifiedName;
                if (variableType.IsValueType || variableType.FullName == "System.String")
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("value")).Value = Convert.ToString(value, CultureInfo.InvariantCulture);
                }
                else if (variableType.IsSerializable)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, value);
                        stream.Seek(0, SeekOrigin.Begin);
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        variable.AppendChild(root.OwnerDocument.CreateCDataSection(Convert.ToBase64String(buffer)));
                    }
                }
            }
        }

        static void SaveUrl(XmlNode root, HttpContext context)
        {
            root.AppendChild(root.OwnerDocument.CreateElement("url")).AppendChild(root.OwnerDocument.CreateCDataSection(context.Request.Url.ToString()));
            root.AppendChild(root.OwnerDocument.CreateElement("pathInfo")).AppendChild(root.OwnerDocument.CreateCDataSection(context.Request.PathInfo));
        }

        static void SaveRequest(XmlNode root, HttpContext context)
        {
            HttpValueCollection.SaveCollectionToXmlNode(context.Request.QueryString,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("queryString")));
            HttpValueCollection.SaveCollectionToXmlNode(context.Request.Form,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("form")));
            HttpValueCollection.SaveCollectionToXmlNode(context.Request.Headers,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("headers")));
        }

        static void SaveException(XmlNode root, HttpContext context)
        {
            XmlNode exception = root.AppendChild(root.OwnerDocument.CreateElement("exception"));
            using (MemoryStream data = new MemoryStream())
            {
                using (GZipOutputStream writer = new GZipOutputStream(data))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(writer, context.Error);
                    writer.Finish();
                    data.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[data.Length];
                    data.Read(buffer, 0, buffer.Length);
                    exception.AppendChild(root.OwnerDocument.CreateCDataSection(Convert.ToBase64String(buffer, Base64FormattingOptions.None)));
                }
            }
        }

        static string BuildUniqueIdentifier(Exception exception)
        {
            StringBuilder messageHash = new StringBuilder();
            StringBuilder stackHash = new StringBuilder();
            while (exception != null)
            {
                messageHash.Append(exception.Message);
                stackHash.Append(RemoveDebugInformationFromStackTrace(exception.StackTrace));
                exception = exception.InnerException;
            }
            return string.Format(CultureInfo.InvariantCulture, "{0}-{1}", (uint) messageHash.ToString().GetHashCode(), (uint) stackHash.ToString().GetHashCode());
        }

        static string RemoveDebugInformationFromStackTrace(string stackTrace)
        {
            return Regex.Replace(stackTrace, @"\) in \w\:[/\\].+", ")");
        }
    }
}