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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;

namespace Bugx.Web
{
    public static class BugSerializer
    {
        /// <summary>
        /// Serializes the specified graph.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public static string Serialize(object graph)
        {
            using (MemoryStream serializedData = new MemoryStream())
            {
                using (Stream writer = new GZipStream(serializedData, CompressionMode.Compress, true))
                {
                    new BinaryFormatter().Serialize(writer, graph);
                }
                return Convert.ToBase64String(serializedData.ToArray());
            }
        }

        /// <summary>
        /// Deserializes the specified serialized data.
        /// </summary>
        /// <param name="serializedData">The serialized data.</param>
        /// <returns>The deserialized object</returns>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The serializationStream supports seeking, but its length is 0.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.ArgumentNullException">The serializationStream is null.</exception>
        public static object Deserialize(string serializedData)
        {
            using (Stream buffer = new MemoryStream(Convert.FromBase64String(serializedData)))
            using (Stream reader = new GZipStream(buffer, CompressionMode.Decompress))
            {
                return new BinaryFormatter().Deserialize(reader);
            }
        }

    }
}
