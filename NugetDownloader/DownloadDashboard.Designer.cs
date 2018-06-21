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
			this.panel2 = new System.Windows.Forms.Panel();
			this.txtConsole = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panDownloads.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panDownloads
			// 
			this.panDownloads.AutoScroll = true;
			this.panDownloads.Controls.Add(this.tlpDownloads);
			this.panDownloads.Dock = System.Windows.Forms.DockStyle.Fill;
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
			// panel2
			// 
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.txtConsole);
			this.panel2.Location = new System.Drawing.Point(378, 12);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(394, 537);
			this.panel2.TabIndex = 2;
			// 
			// txtConsole
			// 
			this.txtConsole.Location = new System.Drawing.Point(3, 28);
			this.txtConsole.MaxLength = 100000;
			this.txtConsole.Multiline = true;
			this.txtConsole.Name = "txtConsole";
			this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtConsole.Size = new System.Drawing.Size(388, 506);
			this.txtConsole.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Output";
			// 
			// DownloadDashboard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "DownloadDashboard";
			this.Text = "DownloadDashboard";
			this.Load += new System.EventHandler(this.DownloadDashboard_Load);
			this.panDownloads.ResumeLayout(false);
			this.panDownloads.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel panDownloads;
		private System.Windows.Forms.TableLayoutPanel tlpDownloads;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox txtConsole;
		private System.Windows.Forms.Label label1;
	}
}