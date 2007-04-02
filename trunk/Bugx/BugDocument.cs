using System.IO;
using System.Xml;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Bugx.Web
{
    public class BugDocument : XmlDocument
    {
        /// <summary>
        /// Saves the XML document to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to which you want to save.</param>
        /// <exception cref="T:System.Xml.XmlException">The operation would not result in a well formed XML document (for example, no document element or duplicate XML declarations). </exception>
        public override void Save(Stream outStream)
        {
            DeflaterOutputStream stream = new GZipOutputStream(outStream);
            base.Save(stream);
            stream.Finish();
        }

        /// <summary>
        /// Saves the XML document to the specified file.
        /// </summary>
        /// <param name="filename">The location of the file where you want to save the document.</param>
        /// <exception cref="T:System.Xml.XmlException">The operation would not result in a well formed XML document (for example, no document element or duplicate XML declarations). </exception>
        public override void Save(string filename)
        {
            using (Stream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using (Stream stream = new GZipOutputStream(file))
                {
                    base.Save(stream);
                }
            }
        }

        /// <summary>
        /// Loads the XML document from the specified URL.
        /// </summary>
        /// <param name="filename">URL for the file containing the XML document to load.</param>
        /// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
        public override void Load(string filename)
        {
            using (Stream file = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                using (Stream stream = new GZipInputStream(file))
                {
                    base.Load(stream);
                }
            }
        }

        /// <summary>
        /// Loads the XML document from the specified stream.
        /// </summary>
        /// <param name="inStream">The stream containing the XML document to load.</param>
        /// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
        public override void Load(Stream inStream)
        {
            Stream stream = new GZipInputStream(inStream);
            base.Load(stream);
        }
    }
}