using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NugetDownloader
{
	public partial class DownloadDashboard : Form
	{
		public NugetManager mNugetManager;

		private const int CONSOLE_MAX_ITEM_COUNT = 5000;
		private const string PROGRESS_BAR_NAME = "PROGRESS_BAR_NAME";

		// clash with Threading timer.
		private System.Windows.Forms.Timer consoleTimer = new System.Windows.Forms.Timer();

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
			consoleTimer.Interval = 200;
			consoleTimer.Tick += consoleTick;
			consoleTimer.Enabled = true;
			lbConsole.DrawMode = DrawMode.OwnerDrawVariable;
			lbConsole.MeasureItem += lst_MeasureItem;
			lbConsole.DrawItem += lst_DrawItem;

			// needs to be called with actual UI thread so we can use BackgroundWorker to callback to UI changes.
			mNugetManager.Execute();
		}
		// referenced https://stackoverflow.com/questions/17613613/winforms-dotnet-listbox-items-to-word-wrap-if-content-string-width-is-bigger-tha
		private void lst_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			e.ItemHeight = (int)e.Graphics.MeasureString(lbConsole.Items[e.Index].ToString(), lbConsole.Font, lbConsole.Width).Height;
		}
		private void lst_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			e.DrawFocusRectangle();
			e.Graphics.DrawString(lbConsole.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
		}

		private void consoleTick(object sender, EventArgs args)
		{
			int maxUpdates = CONSOLE_MAX_ITEM_COUNT;
			while (mNugetManager.consoleOutput.Count > 0)
			{
				if (mNugetManager.consoleOutput.TryDequeue(out string message)){
					lbConsole.Items.Add(message);
					lbConsole.TopIndex = lbConsole.Items.Count - 1;
					while (lbConsole.Items.Count > CONSOLE_MAX_ITEM_COUNT)
					{
						lbConsole.Items.RemoveAt(0);
					}
				}
				maxUpdates -= 1;
				if (maxUpdates <= 0)
				{
					while (mNugetManager.consoleOutput.Count > CONSOLE_MAX_ITEM_COUNT)
					{
						mNugetManager.consoleOutput.TryDequeue(out String discard);
					}
					break;
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
				panDownloads.ScrollControlIntoView(infoBox);
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
