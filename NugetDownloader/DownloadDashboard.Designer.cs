namespace NugetDownloader
{
	partial class DownloadDashboard
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
			this.panDownloads = new System.Windows.Forms.Panel();
			this.tlpDownloads = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblOutput = new System.Windows.Forms.Label();
			this.lbConsole = new System.Windows.Forms.ListBox();
			this.panDownloads.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panDownloads
			// 
			this.panDownloads.AutoScroll = true;
			this.panDownloads.Controls.Add(this.tlpDownloads);
			this.panDownloads.Location = new System.Drawing.Point(0, 0);
			this.panDownloads.Name = "panDownloads";
			this.panDownloads.Size = new System.Drawing.Size(360, 537);
			this.panDownloads.TabIndex = 0;
			// 
			// tlpDownloads
			// 
			this.tlpDownloads.AutoSize = true;
			this.tlpDownloads.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpDownloads.ColumnCount = 1;
			this.tlpDownloads.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpDownloads.Location = new System.Drawing.Point(12, 12);
			this.tlpDownloads.Name = "tlpDownloads";
			this.tlpDownloads.RowCount = 1;
			this.tlpDownloads.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpDownloads.Size = new System.Drawing.Size(0, 0);
			this.tlpDownloads.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.panDownloads);
			this.panel1.Location = new System.Drawing.Point(12, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(360, 537);
			this.panel1.TabIndex = 1;
			// 
			// lblOutput
			// 
			this.lblOutput.AutoSize = true;
			this.lblOutput.Location = new System.Drawing.Point(378, 9);
			this.lblOutput.Name = "lblOutput";
			this.lblOutput.Size = new System.Drawing.Size(39, 13);
			this.lblOutput.TabIndex = 1;
			this.lblOutput.Text = "Output";
			// 
			// lbConsole
			// 
			this.lbConsole.FormattingEnabled = true;
			this.lbConsole.Location = new System.Drawing.Point(378, 25);
			this.lbConsole.Name = "lbConsole";
			this.lbConsole.Size = new System.Drawing.Size(394, 524);
			this.lbConsole.TabIndex = 2;
			// 
			// DownloadDashboard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.lbConsole);
			this.Controls.Add(this.lblOutput);
			this.Controls.Add(this.panel1);
			this.Name = "DownloadDashboard";
			this.Text = "DownloadDashboard";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadDashboard_FormClosing);
			this.Load += new System.EventHandler(this.DownloadDashboard_Load);
			this.panDownloads.ResumeLayout(false);
			this.panDownloads.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Panel panDownloads;
		private System.Windows.Forms.TableLayoutPanel tlpDownloads;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblOutput;
		private System.Windows.Forms.ListBox lbConsole;
	}
}