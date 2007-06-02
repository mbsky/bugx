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

using System.Collections.Generic;
using System.Text;
using System.Web.Hosting;
using System.IO;
using System.Globalization;

namespace Bugx.ReBug
{
    /// <summary>
    /// BugxWorkerRequest simulate crash requests.
    /// </summary>
    public class BugxWorkerRequest : SimpleWorkerRequest
    {
        static Dictionary<string, int> _Headers;
        static string[] _HeaderList;
        static BugxWorkerRequest()
        {
            _Headers = new Dictionary<string, int>();
            _HeaderList = new string[RequestHeaderMaximum];
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
        
        /// <summary>
        /// Returns the HTTP request verb.
        /// </summary>
        /// <returns>The HTTP verb for this request.</returns>
        public override string GetHttpVerbName()
        {
            if (PostedData == null)
            {
                return base.GetHttpVerbName();
            }
            return "POST";
        }

        /// <summary>
        /// Returns the server IP address of the interface on which the request was received.
        /// </summary>
        /// <returns>
        /// The server IP address of the interface on which the request was received.
        /// </returns>
        public override string GetLocalAddress()
        {
            return _Context.Url.Host;
        }

        /// <summary>
        /// Returns the port number on which the request was received.
        /// </summary>
        /// <returns>
        /// The server port number on which the request was received.
        /// </returns>
        public override int GetLocalPort()
        {
            return _Context.Url.Port;
        }

        /// <summary>
        /// Returns the HTTP protocol (HTTP or HTTPS).
        /// </summary>
        /// <returns>
        /// HTTPS if the <see cref="M:System.Web.HttpWorkerRequest.IsSecure"></see> method is true, otherwise HTTP.
        /// </returns>
        public override string GetProtocol()
        {
            return _Context.Url.Scheme;
        }

        /// <summary>
        /// Returns a value indicating whether the connection uses SSL.
        /// </summary>
        /// <returns>
        /// true if the connection is an SSL connection; otherwise, false. The default is false.
        /// </returns>
        public override bool IsSecure()
        {
            return string.Compare(_Context.Url.Scheme, "https", true, CultureInfo.InvariantCulture) == 0;
        }

        /// <summary>
        /// Returns additional path information for a resource with a URL extension. That is, for the path /virdir/page.html/tail, the return value is /tail.
        /// </summary>
        /// <returns>
        /// Additional path information for a resource.
        /// </returns>
        public override string GetPathInfo()
        {
            return _Context.PathInfo;
        }

        /// <summary>
        /// Returns a value indicating whether all request data is available and no further reads from the client are required.
        /// </summary>
        /// <returns>
        /// true if all request data is available; otherwise, false.
        /// </returns>
        public override bool IsEntireEntityBodyIsPreloaded()
        {
            return true;
        }

        /// <summary>
        /// Returns the portion of the HTTP request body that has already been read.
        /// </summary>
        /// <returns>
        /// The portion of the HTTP request body that has been read.
        /// </returns>
        public override byte[] GetPreloadedEntityBody()
        {
            if (PostedData == null)
            {
                return base.GetPreloadedEntityBody();
            }
            return PostedData;
        }

        bool _IsInitialized;

        /// <summary>
        /// Returns the standard HTTP request header that corresponds to the specified index.
        /// </summary>
        /// <param name="index">The index of the header. For example, the <see cref="F:System.Web.HttpWorkerRequest.HeaderAllow"></see> field.</param>
        /// <returns>The HTTP request header.</returns>
        public override string GetKnownRequestHeader(int index)
        {
            if (!_IsInitialized)
            {
                _IsInitialized = true;
                _Context.RestoreEnvironment();
            }
            return _Context.Headers[_HeaderList[index]];
        }

        ReBugContext _Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BugxWorkerRequest"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="output">The output.</param>
        public BugxWorkerRequest(ReBugContext context, TextWriter output)
            : base(context.Url.AbsolutePath.Substring(1), context.QueryString.ToString(), output)
        {
            _Context = context;
            if (context.Form.Count > 0)
            {
                PostedData = Encoding.UTF8.GetBytes(context.Form.ToString());
            }
        }
    }
}
