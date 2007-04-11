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
        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Error += new EventHandler(Application_Error);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this context is a re-bug.
        /// </summary>
        static bool _IsReBug;

        /// <summary>
        /// Gets or sets a value indicating whether this context is a re-bug.
        /// </summary>
        /// <value><c>true</c> if this context is a re-bug; otherwise, <c>false</c>.</value>
        public static bool IsReBug
        {
            get { return _IsReBug; }
            set { _IsReBug = value; }
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void Application_Error(object sender, EventArgs e)
        {
            if (IsReBug)
            {//Error handling is disabled.
                return;
            }
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

        /// <summary>
        /// Saves the session.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
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
                if (variableType.IsValueType || variableType == typeof(string))
                {
                    variable.Attributes.Append(root.OwnerDocument.CreateAttribute("value")).Value = Convert.ToString(value, CultureInfo.InvariantCulture);
                }
                else if (variableType.IsSerializable)
                {
                    variable.AppendChild(root.OwnerDocument.CreateCDataSection(BugSerializer.Serialize(value)));
                }
            }
        }

        /// <summary>
        /// Saves the URL.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveUrl(XmlNode root, HttpContext context)
        {
            root.AppendChild(root.OwnerDocument.CreateElement("url")).AppendChild(root.OwnerDocument.CreateCDataSection(context.Request.Url.ToString()));
            root.AppendChild(root.OwnerDocument.CreateElement("pathInfo")).AppendChild(root.OwnerDocument.CreateCDataSection(context.Request.PathInfo));
        }

        /// <summary>
        /// Saves the request.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
        static void SaveRequest(XmlNode root, HttpContext context)
        {
            HttpValueCollection.SaveCollectionToXmlNode(context.Request.QueryString,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("queryString")));
            HttpValueCollection.SaveCollectionToXmlNode(context.Request.Form,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("form")));
            HttpValueCollection.SaveCollectionToXmlNode(context.Request.Headers,
                                                        root.AppendChild(root.OwnerDocument.CreateElement("headers")));
        }

        /// <summary>
        /// Saves the exception.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="context">The context.</param>
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

        /// <summary>
        /// Builds the unique identifier.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Removes the debug information from stack trace.
        /// </summary>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns></returns>
        static string RemoveDebugInformationFromStackTrace(string stackTrace)
        {
            return Regex.Replace(stackTrace, @"\) in \w\:[/\\].+", ")");
        }
    }
}