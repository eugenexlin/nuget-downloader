using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace NugetDownloader
{
	public class NugetDownloaderWorker
	{
		private static Random rand = new Random();

		private NugetManager mManager;
		private int mId;
		public bool IsProcessing { get; private set; } = true;

		private bool isAborting = false;
		private BackgroundWorker mWorker;

		public delegate void PropsHandleProgressChanged(NugetDownloaderWorker m, ProgressChangedEventArgs e);

		public NugetDownloaderWorker(NugetManager pManager, int pId, BackgroundWorker worker)
		{
			mManager = pManager;
			mId = pId;
			mWorker = worker;
			mWorker.WorkerReportsProgress = true;
			mWorker.WorkerSupportsCancellation = true;
			mWorker.DoWork += new DoWorkEventHandler(ProcessUntilDone);
		}

		private void ReportProgress(NugetProgressItem nugetProgress)
		{
			mWorker.ReportProgress(0, nugetProgress);
		}


		public void Start()
		{
			WriteConsole("Thread Started");
			mWorker.RunWorkerAsync();
		}

		private void WriteConsole(string message)
		{
			mManager.WriteConsole(String.Format("[{0}] - ", mId) + message);
		}

		private void ProcessUntilDone(object sender, DoWorkEventArgs args)
		{
			while (mManager.IsStillDoingWork())
			{
				if (isAborting)
				{
					return;
				}

				Nuget nuget = null;
				try
				{

					nuget = mManager.DequeueNuget();
					if (nuget == null)
					{
						IsProcessing = false;
						Thread.Sleep(100);
						continue;
					}
					IsProcessing = true;
					WriteConsole(String.Format("Processing {0}", nuget.GetFileName()));
					NugetProgressItem progress = new NugetProgressItem(nuget);
					ReportProgress(progress);

					Thread.Sleep(2000 + rand.Next(5000));
					mManager.ForceAddNugetToQueue(nuget);

				}
				catch (Exception ex)
				{
					WriteConsole("Error in worker thread: " + ex.Message);
					if (nuget != null)
					{
						mManager.ForceAddNugetToQueue(nuget);
					}
					continue;
				}
			}
			WriteConsole("Thread Exited");
		}

		public void Dispose()
		{
			isAborting = true;
			mWorker.CancelAsync();
			mWorker.Dispose();
		}
	}
}
