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
