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
    public class BugxWorkerRequest : SimpleWorkerRequest
    {
        static Dictionary<string, int> _Headers = new Dictionary<string, int>();
        static string[] _HeaderList = new string[RequestHeaderMaximum];
        static BugxWorkerRequest()
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
            return _Context.Url.Host;
        }
        public override int GetLocalPort()
        {
            return _Context.Url.Port;
        }
        public override string GetProtocol()
        {
            return _Context.Url.Scheme;
        }
        public override bool IsSecure()
        {
            return string.Compare(_Context.Url.Scheme, "https", true, CultureInfo.InvariantCulture) == 0;
        }
        public override string GetPathInfo()
        {
            return _Context.PathInfo;
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

        bool _IsInitialized;

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
