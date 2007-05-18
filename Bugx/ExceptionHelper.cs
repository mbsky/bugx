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
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Globalization;
using System.Web;
using System.Xml.XPath;

namespace Bugx.Web
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Gets the relevent source.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static string GetRelevantSource(Exception exception)
        {
            if (exception == null || string.IsNullOrEmpty(exception.StackTrace))
            {
                return null;
            }
            string result = GetRelevantSource(exception.InnerException);
            if (string.IsNullOrEmpty(result))
            {
                if (!string.IsNullOrEmpty(exception.Source) &&
                    !exception.Source.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase) &&
                    !exception.Source.StartsWith("System", StringComparison.InvariantCultureIgnoreCase) &&
                    string.Compare(exception.Source, ".Net SqlClient Data Provider", StringComparison.InvariantCultureIgnoreCase) != 0)
                {//If exception source is relevant then simply return it.
                    return exception.Source;
                }
                //Search relevant information.
                Match firstReleventLine = Regex.Match(exception.StackTrace, @"\sat (?!System)(?<Type>.+)\.[^(\s]+\(");
                if (!firstReleventLine.Success)
                {
                    return null;
                }
                Type type = FindType(firstReleventLine.Groups["Type"].Value);
                if (type == null)
                {
                    return null;
                }
                return type.Assembly.FullName.Split(',')[0];
            }
            return result;
        }

        /// <summary>
        /// Finds the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        static Type FindType(string type)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type result = assembly.GetType(type);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Serializes stack trace in XML.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="root">The root.</param>
        public static void XmlSerializeStackTrace(Exception exception, IXPathNavigable root)
        {
            if (exception == null || string.IsNullOrEmpty(exception.StackTrace))
            {
                return;
            }
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            root.CreateNavigator().InnerXml =
                Regex.Replace(exception.StackTrace.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;"),
                              @"(\s+at\s+|(?<MethodName>[^\s\.]+)(?<MethodArguments>\([^)]*\))(?<Location>.*)|(?<ClassPath>[^\s\.]+\.)+)",
                              delegate(Match match)
                                  {
                                      if (match.Groups["MethodName"].Success)
                                      {//method
                                          if (match.Groups["Location"].Value.Trim().Length > 0)
                                          {
                                              return string.Format(CultureInfo.InvariantCulture,
                                                                   "<method>{0}</method><arguments>{1}</arguments><location>{2}</location></trace>",
                                                                   match.Groups["MethodName"].Value,
                                                                   XmlSerializeParameters(match.Groups["MethodArguments"].Value),
                                                                   match.Groups["Location"].Value);
                                          }
                                          else
                                          {
                                              return string.Format(CultureInfo.InvariantCulture,
                                                                   "<method>{0}</method><arguments>{1}</arguments></trace>",
                                                                   match.Groups["MethodName"].Value,
                                                                   XmlSerializeParameters(match.Groups["MethodArguments"].Value));
                                          }
                                      }
                                      else if (match.Groups["ClassPath"].Success)
                                      {//class
                                          StringBuilder path = new StringBuilder();
                                          CaptureCollection captures = match.Groups["ClassPath"].Captures;
                                          path.AppendFormat("<trace relevant=\"{0}\">", captures[0].Value != "System." ? 1 : 0);
                                          for (int i = 0; i < captures.Count - 1; i++)
                                          {
                                              path.AppendFormat("<namespace>{0}</namespace>.",
                                                                captures[i].Value.Substring(0, captures[i].Value.Length - 1));
                                          }
                                          path.AppendFormat("<class>{0}</class>.",
                                                            captures[captures.Count - 1].Value.Substring(0, captures[captures.Count - 1].Value.Length - 1));
                                          return path.ToString();
                                      }
                                      return string.Empty;
                                  });
        }

        /// <summary>
        /// Builds the bug URI.
        /// </summary>
        /// <param name="bugVirtualPath">The bug virtual path.</param>
        /// <returns></returns>
        public static Uri BuildBugUri(string bugVirtualPath)
        {
            if (bugVirtualPath == null)
            {
                throw new ArgumentNullException("bugVirtualPath");
            }
            HttpContext context = HttpContext.Current;
            string applicationPath = context.Request.ApplicationPath;
            if (!applicationPath.EndsWith("/"))
            {
                applicationPath += "/";
            }
            bugVirtualPath = new Uri(context.Request.Url, bugVirtualPath.Replace("~/", applicationPath)).ToString();
            return new Uri(Regex.Replace(bugVirtualPath, "^(https?)://", "bugx://$1/", RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Builds the bug URI.
        /// </summary>
        /// <param name="bugVirtualPath">The bug virtual path.</param>
        /// <returns></returns>
        public static Uri BuildBugReportUri(string bugVirtualPath)
        {
            if (bugVirtualPath == null)
            {
                throw new ArgumentNullException("bugVirtualPath");
            }
            HttpContext context = HttpContext.Current;
            string applicationPath = context.Request.ApplicationPath;
            if (!applicationPath.EndsWith("/"))
            {
                applicationPath += "/";
            }
            return new Uri(context.Request.Url, bugVirtualPath.Replace("~/", applicationPath) + ".axd");
        }

        /// <summary>
        /// Serializes parameters in XML.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private static string XmlSerializeParameters(string parameters)
        {
            return Regex.Replace(parameters, @"(\w+)((?:&amp;|\[\]|`\d+|.)\s*\w+)(\s*,\s*)?", "<class>$1</class>$2$3");
        }
    }
}
