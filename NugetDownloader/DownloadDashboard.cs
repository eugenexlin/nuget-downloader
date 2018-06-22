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

		private const int CONSOLE_MAX_ITEM_COUNT = 5000;
		private const string PROGRESS_BAR_NAME = "PROGRESS_BAR_NAME";

		private Timer consoleTimer = new Timer();

		private Dictionary<string, GroupBox> progressDict = new Dictionary<string, GroupBox>(StringComparer.OrdinalIgnoreCase);

		public DownloadDashboard(NugetManagerParams p)
		{
			mNugetManager = new NugetManager(p);
			mNugetManager.ProgressChanged += new NugetManager.NugetProgressChangedHandler(HandleProgressChanged);
			//mNugetManager.WroteConsole += new NugetManager.WriteConsoleHandler(HandleWroteConsole);
			InitializeComponent();
		}

		private void DownloadDashboard_Load(object sender, EventArgs e)
		{
			ActiveControl = lblOutput;
			mNugetManager.Execute();
			consoleTimer.Interval = 200;
			consoleTimer.Tick += consoleTick;
			consoleTimer.Enabled = true;
		}
		
		private void consoleTick(object sender, EventArgs args)
		{
			while (mNugetManager.consoleOutput.Count > 0)
			{
				if (mNugetManager.consoleOutput.TryDequeue(out string message)){
					lbConsole.Items.Add(message);
					while (lbConsole.Items.Count > CONSOLE_MAX_ITEM_COUNT)
					{
						lbConsole.Items.RemoveAt(0);
					}
				}
			}

			consoleTimer.Enabled = true;
		}

		private void HandleProgressChanged(object sender, NugetProgressArgs e)
		{
			NugetProgressItem progress = e.nugetProgress;
			Nuget nuget = progress.nuget;
			string key = nuget.GetFileName();
			GroupBox infoBox;
			if (!progressDict.ContainsKey(key))
			{
				infoBox = CreateNewProgressBox(key);
				progressDict.Add(key, infoBox);
				tlpDownloads.Controls.Add(infoBox);
			}
			else
			{
				infoBox = progressDict[key];
			}
			ProgressBar bar = (ProgressBar)infoBox.Controls.Find(PROGRESS_BAR_NAME, false)[0];
			bar.Value = progress.downloadPercent;
		}

		private GroupBox CreateNewProgressBox(string name)
		{
			GroupBox infoBox = new GroupBox();
			infoBox.Text = name;
			infoBox.Width = 300;
			infoBox.Height = 60;

			ProgressBar bar = new ProgressBar();
			bar.Name = PROGRESS_BAR_NAME;
			bar.Top = 20;
			bar.Left = 10;
			bar.Height = 10;
			bar.Width = 280;
			infoBox.Controls.Add(bar);

			return infoBox;
		}

		//private void HandleWroteConsole(NugetManager manager, WriteConsoleArgs args)
		//{
		//	if (args.message.EndsWith(Environment.NewLine))
		//	{
		//		txtConsole.Text += DateTime.Now.ToLongTimeString() + " - " + args.message;
		//	}
		//	else
		//	{
		//		txtConsole.Text += DateTime.Now.ToLongTimeString() + " - " + args.message + Environment.NewLine;
		//	}
		//}

		private void DownloadDashboard_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (mNugetManager != null) {
				mNugetManager.Dispose();
			}
		}
	}
}
