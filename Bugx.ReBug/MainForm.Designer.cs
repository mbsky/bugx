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
            System.Windows.Forms.GroupBox BugScenario;
            System.Windows.Forms.Label BugFileLabel;
            System.Windows.Forms.Panel CommandPannel;
            System.Windows.Forms.GroupBox ContextInfo;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.LoadBugFile = new System.Windows.Forms.Button();
            this.BugFile = new System.Windows.Forms.Label();
            this.ReBug = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ContextExplorer = new System.Windows.Forms.PropertyGrid();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.OpenFile = new System.Windows.Forms.OpenFileDialog();
            BugScenario = new System.Windows.Forms.GroupBox();
            BugFileLabel = new System.Windows.Forms.Label();
            CommandPannel = new System.Windows.Forms.Panel();
            ContextInfo = new System.Windows.Forms.GroupBox();
            BugScenario.SuspendLayout();
            CommandPannel.SuspendLayout();
            ContextInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.ReBug.Enabled = false;
            this.ReBug.Location = new System.Drawing.Point(6, 5);
            this.ReBug.Name = "ReBug";
            this.ReBug.Size = new System.Drawing.Size(75, 23);
            this.ReBug.TabIndex = 1;
            this.ReBug.Text = "ReBug !";
            this.ReBug.UseVisualStyleBackColor = true;
            this.ReBug.Click += new System.EventHandler(this.ReBug_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(87, 5);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ContextInfo
            // 
            ContextInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            ContextInfo.Controls.Add(this.ContextExplorer);
            ContextInfo.Location = new System.Drawing.Point(12, 162);
            ContextInfo.Name = "ContextInfo";
            ContextInfo.Size = new System.Drawing.Size(748, 492);
            ContextInfo.TabIndex = 3;
            ContextInfo.TabStop = false;
            ContextInfo.Text = "Context :";
            // 
            // ContextExplorer
            // 
            this.ContextExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ContextExplorer.Location = new System.Drawing.Point(12, 19);
            this.ContextExplorer.Name = "ContextExplorer";
            this.ContextExplorer.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.ContextExplorer.Size = new System.Drawing.Size(730, 467);
            this.ContextExplorer.TabIndex = 1;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 700);
            this.Controls.Add(CommandPannel);
            this.Controls.Add(ContextInfo);
            this.Controls.Add(BugScenario);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(780, 734);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReBug";
            BugScenario.ResumeLayout(false);
            BugScenario.PerformLayout();
            CommandPannel.ResumeLayout(false);
            ContextInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog OpenFile;
        private System.Windows.Forms.Button LoadBugFile;
        private System.Windows.Forms.Label BugFile;
        private System.Windows.Forms.PropertyGrid ContextExplorer;
        private System.Windows.Forms.Button ReBug;
        private System.Windows.Forms.Button CloseButton;
    }
}

