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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;
using Bugx.Web;
using System.Web.Hosting;
using System.Diagnostics;
using System.Runtime.Remoting;

namespace Bugx.ReBug
{
    public partial class MainForm : Form
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm(string fileName)
        {
            InitializeComponent();
            VariableTree.BeforeExpand += new TreeViewCancelEventHandler(VariableTree_BeforeExpand);
            VariableTree.BeforeCollapse += new TreeViewCancelEventHandler(VariableTree_BeforeCollapse);
            if (!string.IsNullOrEmpty(fileName))
            {
                LoadBug(fileName);
            }
        } 
        #endregion

        #region Properties
        BugxHost _Host;
        public BugxHost Host
        {
            get
            {
                if (_Host == null)
                {
                    DirectoryInfo webPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent;
                    _Host = (BugxHost)ApplicationHost.CreateApplicationHost(
                                                                   typeof(BugxHost),
                                                                   "/",
                                                                   webPath.FullName);
                }
                try
                {
                    bool isConnected = _Host.IsConnected;
                }
                catch (RemotingException)
                {//Remoting context is broken.
                    DirectoryInfo webPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent;
                    _Host = (BugxHost)ApplicationHost.CreateApplicationHost(
                                                                   typeof(BugxHost),
                                                                   "/",
                                                                   webPath.FullName);
                }
                return _Host;
            }
        }
        #endregion 

        #region Loading
        /// <summary>
        /// Loads the bug.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        void LoadBug(string fileName)
        {
            BugFile.Text = fileName;
            XmlDocument xml = new BugDocument();
            xml.Load(fileName);
            ExceptionExplorer.SelectedObject = new ExceptionDescriptor((Exception)BugSerializer.Deserialize(xml.SelectSingleNode("/bugx/exception").InnerText));
            Url.Text = xml.SelectSingleNode("/bugx/url").InnerText;
            LoadVariables(xml);
            ReBug.Enabled = true;
        }

        /// <summary>
        /// Loads the variables.
        /// </summary>
        /// <param name="xml">The XML.</param>
        void LoadVariables(XmlDocument xml)
        {
            VariableTree.Nodes.Clear();
            FillValues(HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/queryString")),
                       VariableTree.Nodes.Add("QueryString", "QueryString", "ClosedVariableGroup", "ClosedVariableGroup"));

            FillValues(HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/form")),
                       VariableTree.Nodes.Add("Form", "Form", "ClosedVariableGroup", "ClosedVariableGroup"));

            XmlNode cookie = xml.SelectSingleNode("/bugx/headers/Cookie");
            if (cookie != null)
            {
                FillValues(HttpValueCollection.CreateCollectionFromUrlEncoded(cookie.InnerText),
                           VariableTree.Nodes.Add("Cookies", "Cookies", "ClosedVariableGroup", "ClosedVariableGroup"));
                
            }
            FillValues(HttpValueCollection.CreateCollectionFromXmlNode(xml.SelectSingleNode("/bugx/headers")),
                       VariableTree.Nodes.Add("Headers", "Headers", "ClosedVariableGroup", "ClosedVariableGroup"));

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
            if (nameValue.Count == 0)
            {
                treeNode.Remove();
                return;
            }
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
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the ReBug control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ReBug_Click(object sender, EventArgs e)
        {
            if (!Debugger.IsAttached && MessageBox.Show(this, Texts.DebuggerIsNotAttached, Texts.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Yes)
            {
                Debugger.Launch();    
            }
            if (Debugger.IsAttached)
            {
                Host.Process(BugFile.Text);
                MessageBox.Show(this, Texts.BugComplete, Texts.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion
    }
}