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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.Label BugFileLabel;
            System.Windows.Forms.Panel CommandPannel;
            System.Windows.Forms.GroupBox ContextInfo;
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
            resources.ApplyResources(BugScenario, "BugScenario");
            BugScenario.Controls.Add(this.LoadBugFile);
            BugScenario.Controls.Add(this.BugFile);
            BugScenario.Controls.Add(BugFileLabel);
            BugScenario.Name = "BugScenario";
            BugScenario.TabStop = false;
            // 
            // LoadBugFile
            // 
            resources.ApplyResources(this.LoadBugFile, "LoadBugFile");
            this.LoadBugFile.Name = "LoadBugFile";
            this.LoadBugFile.UseVisualStyleBackColor = true;
            this.LoadBugFile.Click += new System.EventHandler(this.LoadBugFile_Click);
            // 
            // BugFile
            // 
            resources.ApplyResources(this.BugFile, "BugFile");
            this.BugFile.AutoEllipsis = true;
            this.BugFile.Name = "BugFile";
            // 
            // BugFileLabel
            // 
            resources.ApplyResources(BugFileLabel, "BugFileLabel");
            BugFileLabel.Name = "BugFileLabel";
            // 
            // CommandPannel
            // 
            resources.ApplyResources(CommandPannel, "CommandPannel");
            CommandPannel.Controls.Add(this.ReBug);
            CommandPannel.Controls.Add(this.CloseButton);
            CommandPannel.Name = "CommandPannel";
            // 
            // ReBug
            // 
            resources.ApplyResources(this.ReBug, "ReBug");
            this.ReBug.Name = "ReBug";
            this.ReBug.UseVisualStyleBackColor = true;
            this.ReBug.Click += new System.EventHandler(this.ReBug_Click);
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ContextInfo
            // 
            resources.ApplyResources(ContextInfo, "ContextInfo");
            ContextInfo.Controls.Add(this.ContextExplorer);
            ContextInfo.Name = "ContextInfo";
            ContextInfo.TabStop = false;
            // 
            // ContextExplorer
            // 
            resources.ApplyResources(this.ContextExplorer, "ContextExplorer");
            this.ContextExplorer.Name = "ContextExplorer";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // OpenFile
            // 
            resources.ApplyResources(this.OpenFile, "OpenFile");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(CommandPannel);
            this.Controls.Add(ContextInfo);
            this.Controls.Add(BugScenario);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainForm";
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

