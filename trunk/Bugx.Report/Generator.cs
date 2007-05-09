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

using System.Web;
using System.IO;
using System.Xml.Xsl;
using System.Xml;
using Bugx.Web;

namespace Bugx.Report
{
    public class Generator: IHttpHandler
    {
        const string Status404 = "404 Not Found";

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get{ return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            string fileName = context.Server.MapPath(context.Request.Url.AbsolutePath).Replace(".axd", string.Empty);
            if (!File.Exists(fileName))
            {
                context.Response.Status = Status404;
                context.Response.StatusCode = 404;
                context.Response.Write(Status404);
                context.Response.End();
            }
            BuildReport(fileName, context);
        }

        /// <summary>
        /// Builds the report.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="context">The context.</param>
        static void BuildReport(string fileName, HttpContext context)
        {
            context.Response.ContentType = "text/html";
            XslCompiledTransform xsl     = new XslCompiledTransform();

            using (Stream     xslFile = XslFile)
            using (XmlWriter  writer  = new XmlTextWriter(context.Response.Output))
            {
                BugDocument bug = new BugDocument();
                bug.Load(fileName);
                xsl.Load(new XmlTextReader(xslFile));
                xsl.Transform(bug, writer);
            }
        }

        /// <summary>
        /// Gets the XSL file.
        /// </summary>
        /// <value>The XSL file.</value>
        static Stream XslFile
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    string localFile = HttpContext.Current.Server.MapPath("~/bugx/Transformation.xslt");
                    if (File.Exists(localFile))
                    {
                        return new FileStream(localFile, FileMode.Open, FileAccess.Read);
                    }
                }
                return typeof(Generator).Assembly.GetManifestResourceStream("Bugx.Report.Xslt.Transformation.xslt");
            }
        }
    }
}
