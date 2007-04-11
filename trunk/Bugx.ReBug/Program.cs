using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace Bugx.ReBug
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string fileName = null;
            if (args.Length > 0)
            {
                Uri url;
                if (Uri.TryCreate(args[0], UriKind.Absolute, out url) && (url.Scheme.ToLowerInvariant() == "bugx"))
                {
                    fileName = Download(url);
                }
                else if (File.Exists(args[0]))
                {
                    fileName = args[0];
                }
            }
            RegisterProtocol();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(fileName));
        }

        /// <summary>
        /// Downloads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private static string Download(Uri fileName)
        {
            fileName = new Uri(Regex.Replace(fileName.ToString(), @"bugx://(\w+)/(.+)", "$1://$2", RegexOptions.IgnoreCase));
            string destinationFile = DetermineDestinationFile(fileName);
            if (!File.Exists(destinationFile))
            {
                Download(fileName, destinationFile);
            }
            return destinationFile;
        }

        /// <summary>
        /// Downloads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="destinationFile">The destination file.</param>
        static void Download(Uri fileName, string destinationFile)
        {
            const int BufferSize = 10240;
            WebRequest request = WebRequest.Create(fileName);
            new DirectoryInfo(Path.GetDirectoryName(destinationFile)).Create();
            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (Stream destinationStream = new FileStream(destinationFile, FileMode.CreateNew))
            {
                byte[] buffer = new byte[BufferSize];
                for (int size = responseStream.Read(buffer, 0, BufferSize); size != 0; size = responseStream.Read(buffer, 0, BufferSize))
                {
                    destinationStream.Write(buffer, 0, size);
                }
            }
        }

        /// <summary>
        /// Determines the destination file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        static string DetermineDestinationFile(Uri file)
        {
            Uri applicationPath = new Uri(Assembly.GetCallingAssembly().CodeBase);
            Match path = Regex.Match(file.ToString(), "/bugx/errors/.+", RegexOptions.IgnoreCase);
            if (!path.Success)
            {
                throw new NotSupportedException("Unknown uri format: " + file);
            }
            return new Uri(applicationPath, ".." + path.Value).LocalPath;
        }

        /// <summary>
        /// Registers the protocol.
        /// </summary>
        static void RegisterProtocol()
        {
            Registry.SetValue(@"HKEY_CLASSES_ROOT\bugx", string.Empty, "URL: bugx Protocol");
            Registry.SetValue(@"HKEY_CLASSES_ROOT\bugx", "URL Protocol", string.Empty);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\bugx\shell\open\command", string.Empty, string.Format("\"{0}\" \"%1\"", new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath));
        }
    }
}