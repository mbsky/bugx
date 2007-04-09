using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;
using Bugx.Web;
using ICSharpCode.SharpZipLib.GZip;

namespace Bugx.ReBug
{
    public partial class MainForm : Form
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            VariableTree.BeforeExpand += new TreeViewCancelEventHandler(VariableTree_BeforeExpand);
            VariableTree.BeforeCollapse += new TreeViewCancelEventHandler(VariableTree_BeforeCollapse);
            ReBug.Click += new EventHandler(ReBug_Click);
        } 
        #endregion

        #region Loading
        /// <summary>
        /// Loads the bug.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        void LoadBug(string fileName)
        {
            XmlDocument xml = new BugDocument();
            xml.Load(fileName);
            byte[] buffer = Convert.FromBase64String(xml.SelectSingleNode("/bugx/exception").InnerText);
            Exception throwException;
            using (Stream compressedStream = new MemoryStream(buffer))
            using (Stream stream = new GZipInputStream(compressedStream))
            {
                throwException = (Exception)new BinaryFormatter().Deserialize(stream);
            }
            ExceptionExplorer.SelectedObject = new ExceptionDescriptor(throwException);
            Url.Text = xml.SelectSingleNode("/bugx/url").InnerText;
            LoadVariables(xml);
        }

        /// <summary>
        /// Loads the variables.
        /// </summary>
        /// <param name="xml">The XML.</param>
        void LoadVariables(XmlDocument xml)
        {
            VariableTree.Nodes.Clear();
            FillValues(HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/queryString")),
                       VariableTree.Nodes.Add("QueryString", "QueryString", "ClosedVariableGroup", "OpenVariableGroup"));

            FillValues(HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/form")),
                       VariableTree.Nodes.Add("Form", "Form", "ClosedVariableGroup", "OpenVariableGroup"));

            FillValues(HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/headers")),
                       VariableTree.Nodes.Add("Headers", "Headers", "ClosedVariableGroup", "OpenVariableGroup"));

            VariableTree.ExpandAll();
            VariableTree.Nodes[0].EnsureVisible();
        }

        /// <summary>
        /// Fills the values.
        /// </summary>
        /// <param name="nameValue">The name value.</param>
        /// <param name="treeNode">The tree node.</param>
        static void FillValues(HttpValueCollection nameValue, TreeNode treeNode)
        {
            foreach (string key in nameValue.Keys)
            {
                treeNode.Nodes.Add(key, key + ": " + nameValue[key], "Variable", "Variable");
            }
        } 
        #endregion

        #region Events
        void LoadBugFile_Click(object sender, EventArgs e)
        {
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                BugFile.Text = OpenFile.FileName;
                LoadBug(OpenFile.FileName);
            }
        }

        /// <summary>
        /// Handles the BeforeCollapse event of the VariableTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewCancelEventArgs"/> instance containing the event data.</param>
        static void VariableTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageKey = "ClosedVariableGroup";
            e.Node.SelectedImageKey = "ClosedVariableGroup";
        }

        /// <summary>
        /// Handles the BeforeExpand event of the VariableTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewCancelEventArgs"/> instance containing the event data.</param>
        static void VariableTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageKey = "OpenVariableGroup";
            e.Node.SelectedImageKey = "OpenVariableGroup";
        }

        /// <summary>
        /// Handles the Click event of the ReBug control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void ReBug_Click(object sender, EventArgs e)
        {

        } 
        #endregion
    }
}