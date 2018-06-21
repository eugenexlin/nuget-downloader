namespace NugetDownloader
{
	partial class DownloadInputForm
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnDownload = new System.Windows.Forms.Button();
			this.txtNugets = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.btnDefault = new System.Windows.Forms.Button();
			this.txtRemoteNugetPath = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.txtOutputReportPath = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtStagingNugetPath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtLocalNugetPath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtFramework = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtFrameworkVersion = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.label8);
			this.splitContainer1.Panel1.Controls.Add(this.label7);
			this.splitContainer1.Panel1.Controls.Add(this.btnDownload);
			this.splitContainer1.Panel1.Controls.Add(this.txtNugets);
			this.splitContainer1.Panel1.Controls.Add(this.label5);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.label10);
			this.splitContainer1.Panel2.Controls.Add(this.txtFrameworkVersion);
			this.splitContainer1.Panel2.Controls.Add(this.label9);
			this.splitContainer1.Panel2.Controls.Add(this.txtFramework);
			this.splitContainer1.Panel2.Controls.Add(this.btnDefault);
			this.splitContainer1.Panel2.Controls.Add(this.txtRemoteNugetPath);
			this.splitContainer1.Panel2.Controls.Add(this.label6);
			this.splitContainer1.Panel2.Controls.Add(this.btnSave);
			this.splitContainer1.Panel2.Controls.Add(this.txtOutputReportPath);
			this.splitContainer1.Panel2.Controls.Add(this.label4);
			this.splitContainer1.Panel2.Controls.Add(this.txtStagingNugetPath);
			this.splitContainer1.Panel2.Controls.Add(this.label3);
			this.splitContainer1.Panel2.Controls.Add(this.txtLocalNugetPath);
			this.splitContainer1.Panel2.Controls.Add(this.label2);
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Size = new System.Drawing.Size(584, 321);
			this.splitContainer1.SplitterDistance = 360;
			this.splitContainer1.TabIndex = 0;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(9, 50);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(310, 38);
			this.label8.TabIndex = 13;
			this.label8.Text = "Info report html about all new nugets will be generated at the Report path.";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(12, 13);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(310, 38);
			this.label7.TabIndex = 12;
			this.label7.Text = "Downloads nuget and entire dependency tree to Staging path. Will not download nug" +
    "ets found in Local path.";
			// 
			// btnDownload
			// 
			this.btnDownload.Location = new System.Drawing.Point(12, 286);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(75, 23);
			this.btnDownload.TabIndex = 11;
			this.btnDownload.Text = "Download";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// txtNugets
			// 
			this.txtNugets.Location = new System.Drawing.Point(15, 120);
			this.txtNugets.Multiline = true;
			this.txtNugets.Name = "txtNugets";
			this.txtNugets.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtNugets.Size = new System.Drawing.Size(320, 160);
			this.txtNugets.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 104);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(289, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Desired Nuget Packages (line separated url or nupkg name)";
			// 
			// btnDefault
			// 
			this.btnDefault.Location = new System.Drawing.Point(88, 233);
			this.btnDefault.Name = "btnDefault";
			this.btnDefault.Size = new System.Drawing.Size(75, 23);
			this.btnDefault.TabIndex = 10;
			this.btnDefault.Text = "Default";
			this.btnDefault.UseVisualStyleBackColor = true;
			this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
			// 
			// txtRemoteNugetPath
			// 
			this.txtRemoteNugetPath.Location = new System.Drawing.Point(7, 50);
			this.txtRemoteNugetPath.Name = "txtRemoteNugetPath";
			this.txtRemoteNugetPath.Size = new System.Drawing.Size(200, 20);
			this.txtRemoteNugetPath.TabIndex = 9;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(4, 34);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(138, 13);
			this.label6.TabIndex = 8;
			this.label6.Text = "Remote Nuget Path or URL";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(7, 233);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// txtOutputReportPath
			// 
			this.txtOutputReportPath.Location = new System.Drawing.Point(7, 167);
			this.txtOutputReportPath.Name = "txtOutputReportPath";
			this.txtOutputReportPath.Size = new System.Drawing.Size(200, 20);
			this.txtOutputReportPath.TabIndex = 6;
			this.txtOutputReportPath.Leave += new System.EventHandler(this.txtPath_Leave);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(4, 151);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(99, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Output Report Path";
			// 
			// txtStagingNugetPath
			// 
			this.txtStagingNugetPath.Location = new System.Drawing.Point(7, 128);
			this.txtStagingNugetPath.Name = "txtStagingNugetPath";
			this.txtStagingNugetPath.Size = new System.Drawing.Size(200, 20);
			this.txtStagingNugetPath.TabIndex = 4;
			this.txtStagingNugetPath.Leave += new System.EventHandler(this.txtPath_Leave);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(4, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Staging Nuget Path";
			// 
			// txtLocalNugetPath
			// 
			this.txtLocalNugetPath.Location = new System.Drawing.Point(7, 89);
			this.txtLocalNugetPath.Name = "txtLocalNugetPath";
			this.txtLocalNugetPath.Size = new System.Drawing.Size(200, 20);
			this.txtLocalNugetPath.TabIndex = 2;
			this.txtLocalNugetPath.Leave += new System.EventHandler(this.txtPath_Leave);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 73);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Local Nuget Path";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Cursor = System.Windows.Forms.Cursors.Cross;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(4, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Settings";
			// 
			// txtFramework
			// 
			this.txtFramework.FormattingEnabled = true;
			this.txtFramework.Items.AddRange(new object[] {
            ".NETFramework",
            ".NETCore",
            ".NETStandard"});
			this.txtFramework.Location = new System.Drawing.Point(7, 206);
			this.txtFramework.Name = "txtFramework";
			this.txtFramework.Size = new System.Drawing.Size(120, 21);
			this.txtFramework.TabIndex = 11;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(4, 190);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(93, 13);
			this.label9.TabIndex = 12;
			this.label9.Text = "Target Framework";
			// 
			// txtFrameworkVersion
			// 
			this.txtFrameworkVersion.Location = new System.Drawing.Point(133, 206);
			this.txtFrameworkVersion.Name = "txtFrameworkVersion";
			this.txtFrameworkVersion.Size = new System.Drawing.Size(74, 20);
			this.txtFrameworkVersion.TabIndex = 13;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(130, 190);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(42, 13);
			this.label10.TabIndex = 14;
			this.label10.Text = "Version";
			// 
			// DownloadInputForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 321);
			this.Controls.Add(this.splitContainer1);
			this.Name = "DownloadInputForm";
			this.Text = "Nuget Downloader";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtLocalNugetPath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtStagingNugetPath;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtOutputReportPath;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtNugets;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtRemoteNugetPath;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnDefault;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox txtFramework;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtFrameworkVersion;
	}
}

