using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NugetDownloader
{
	public partial class DownloadDashboard : Form
	{
		public NugetManager mNugetManager;

		public DownloadDashboard(NugetManagerParams p)
		{
			mNugetManager = new NugetManager(p);
			mNugetManager.NugetQueued += new NugetManager.NugetQueuedHandler(HandleNugetQueued);
			InitializeComponent();
		}
		

		private void DownloadDashboard_Load(object sender, EventArgs e)
		{
			mNugetManager.Execute();
		}

		private void HandleWroteConsole(NugetManager manager, WriteConsoleArgs args)
		{
			txtConsole.Text += DateTime.Now.ToString() + " - " + args.message + "\n";
			if (txtConsole.Text.Length > 50000)
			{
				txtConsole.Text = txtConsole.Text.Substring(txtConsole.Text.Length - 50000);
			}
		}

		private void HandleNugetQueued(NugetManager manager, NugetQueuedArgs args) 
		{
			GroupBox info = new GroupBox();
			info.Text = args.nuget.GetFileName();
			info.Width = 300;
			info.Height = 60;
			tlpDownloads.Controls.Add(info);
		}

	}
}
