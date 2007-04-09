namespace Bugx.ReBug
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox BugScenario;
            System.Windows.Forms.Label BugFileLabel;
            System.Windows.Forms.GroupBox ExceptionInfo;
            System.Windows.Forms.GroupBox EnvironmentInfo;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label UrlLabel;
            System.Windows.Forms.Panel CommandPannel;
            this.LoadBugFile = new System.Windows.Forms.Button();
            this.BugFile = new System.Windows.Forms.Label();
            this.ExceptionExplorer = new System.Windows.Forms.PropertyGrid();
            this.VariableTree = new System.Windows.Forms.TreeView();
            this.Images = new System.Windows.Forms.ImageList(this.components);
            this.Url = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.ReBug = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            BugScenario = new System.Windows.Forms.GroupBox();
            BugFileLabel = new System.Windows.Forms.Label();
            ExceptionInfo = new System.Windows.Forms.GroupBox();
            EnvironmentInfo = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            UrlLabel = new System.Windows.Forms.Label();
            CommandPannel = new System.Windows.Forms.Panel();
            BugScenario.SuspendLayout();
            ExceptionInfo.SuspendLayout();
            EnvironmentInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            CommandPannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BugScenario
            // 
            BugScenario.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            BugScenario.Controls.Add(this.LoadBugFile);
            BugScenario.Controls.Add(this.BugFile);
            BugScenario.Controls.Add(BugFileLabel);
            BugScenario.Location = new System.Drawing.Point(12, 108);
            BugScenario.Name = "BugScenario";
            BugScenario.Size = new System.Drawing.Size(748, 48);
            BugScenario.TabIndex = 2;
            BugScenario.TabStop = false;
            BugScenario.Text = "Bug Scenario :";
            // 
            // LoadBugFile
            // 
            this.LoadBugFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadBugFile.Location = new System.Drawing.Point(667, 15);
            this.LoadBugFile.Name = "LoadBugFile";
            this.LoadBugFile.Size = new System.Drawing.Size(75, 23);
            this.LoadBugFile.TabIndex = 2;
            this.LoadBugFile.Text = "Load";
            this.LoadBugFile.UseVisualStyleBackColor = true;
            this.LoadBugFile.Click += new System.EventHandler(this.LoadBugFile_Click);
            // 
            // BugFile
            // 
            this.BugFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BugFile.AutoEllipsis = true;
            this.BugFile.Location = new System.Drawing.Point(64, 20);
            this.BugFile.Name = "BugFile";
            this.BugFile.Size = new System.Drawing.Size(597, 23);
            this.BugFile.TabIndex = 1;
            // 
            // BugFileLabel
            // 
            BugFileLabel.AutoSize = true;
            BugFileLabel.Location = new System.Drawing.Point(7, 20);
            BugFileLabel.Name = "BugFileLabel";
            BugFileLabel.Size = new System.Drawing.Size(51, 13);
            BugFileLabel.TabIndex = 0;
            BugFileLabel.Text = "Bug File :";
            // 
            // ExceptionInfo
            // 
            ExceptionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            ExceptionInfo.Controls.Add(this.ExceptionExplorer);
            ExceptionInfo.Location = new System.Drawing.Point(12, 375);
            ExceptionInfo.Name = "ExceptionInfo";
            ExceptionInfo.Size = new System.Drawing.Size(748, 279);
            ExceptionInfo.TabIndex = 3;
            ExceptionInfo.TabStop = false;
            ExceptionInfo.Text = "Exception :";
            // 
            // ExceptionExplorer
            // 
            this.ExceptionExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ExceptionExplorer.Location = new System.Drawing.Point(12, 19);
            this.ExceptionExplorer.Name = "ExceptionExplorer";
            this.ExceptionExplorer.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ExceptionExplorer.Size = new System.Drawing.Size(730, 254);
            this.ExceptionExplorer.TabIndex = 1;
            // 
            // EnvironmentInfo
            // 
            EnvironmentInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            EnvironmentInfo.Controls.Add(this.VariableTree);
            EnvironmentInfo.Controls.Add(label1);
            EnvironmentInfo.Controls.Add(this.Url);
            EnvironmentInfo.Controls.Add(UrlLabel);
            EnvironmentInfo.Location = new System.Drawing.Point(12, 162);
            EnvironmentInfo.Name = "EnvironmentInfo";
            EnvironmentInfo.Size = new System.Drawing.Size(748, 207);
            EnvironmentInfo.TabIndex = 4;
            EnvironmentInfo.TabStop = false;
            EnvironmentInfo.Text = "Environment :";
            // 
            // VariableTree
            // 
            this.VariableTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.VariableTree.ImageIndex = 0;
            this.VariableTree.ImageList = this.Images;
            this.VariableTree.Location = new System.Drawing.Point(79, 42);
            this.VariableTree.Name = "VariableTree";
            this.VariableTree.SelectedImageIndex = 0;
            this.VariableTree.Size = new System.Drawing.Size(663, 159);
            this.VariableTree.TabIndex = 3;
            // 
            // Images
            // 
            this.Images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Images.ImageStream")));
            this.Images.TransparentColor = System.Drawing.Color.Transparent;
            this.Images.Images.SetKeyName(0, "ClosedVariableGroup");
            this.Images.Images.SetKeyName(1, "OpenVariableGroup");
            this.Images.Images.SetKeyName(2, "Variable");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 42);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(56, 13);
            label1.TabIndex = 2;
            label1.Text = "Variables :";
            // 
            // Url
            // 
            this.Url.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Url.AutoEllipsis = true;
            this.Url.Location = new System.Drawing.Point(76, 20);
            this.Url.Name = "Url";
            this.Url.Size = new System.Drawing.Size(666, 12);
            this.Url.TabIndex = 1;
            // 
            // UrlLabel
            // 
            UrlLabel.AutoSize = true;
            UrlLabel.Location = new System.Drawing.Point(7, 20);
            UrlLabel.Name = "UrlLabel";
            UrlLabel.Size = new System.Drawing.Size(35, 13);
            UrlLabel.TabIndex = 0;
            UrlLabel.Text = "URL :";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(378, 90);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // OpenFile
            // 
            this.OpenFile.Filter = "Bugx Files|*.bugx";
            // 
            // CommandPannel
            // 
            CommandPannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            CommandPannel.AutoSize = true;
            CommandPannel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            CommandPannel.Controls.Add(this.ReBug);
            CommandPannel.Controls.Add(this.CloseButton);
            CommandPannel.Location = new System.Drawing.Point(595, 660);
            CommandPannel.Name = "CommandPannel";
            CommandPannel.Size = new System.Drawing.Size(165, 31);
            CommandPannel.TabIndex = 5;
            // 
            // ReBug
            // 
            this.ReBug.Location = new System.Drawing.Point(6, 5);
            this.ReBug.Name = "ReBug";
            this.ReBug.Size = new System.Drawing.Size(75, 23);
            this.ReBug.TabIndex = 1;
            this.ReBug.Text = "ReBug !";
            this.ReBug.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(87, 5);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 700);
            this.Controls.Add(CommandPannel);
            this.Controls.Add(EnvironmentInfo);
            this.Controls.Add(ExceptionInfo);
            this.Controls.Add(BugScenario);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(780, 700);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReBug";
            BugScenario.ResumeLayout(false);
            BugScenario.PerformLayout();
            ExceptionInfo.ResumeLayout(false);
            EnvironmentInfo.ResumeLayout(false);
            EnvironmentInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            CommandPannel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog OpenFile;
        private System.Windows.Forms.Button LoadBugFile;
        private System.Windows.Forms.Label BugFile;
        private System.Windows.Forms.PropertyGrid ExceptionExplorer;
        private System.Windows.Forms.Label Url;
        private System.Windows.Forms.TreeView VariableTree;
        private System.Windows.Forms.ImageList Images;
        private System.Windows.Forms.Button ReBug;
        private System.Windows.Forms.Button CloseButton;
    }
}

