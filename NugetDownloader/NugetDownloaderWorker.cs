using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Net;
using System.IO;

namespace NugetDownloader
{
	public class NugetDownloaderWorker
	{
		private static Random rand = new Random();

		private NugetManager mManager;
		private int mId;
		public bool IsProcessing { get; private set; } = true;

		private bool IsDownloading = false;
		private bool isAborting = false;
		private BackgroundWorker mWorker;
		private WebClient webClient;

		private NugetProgressItem currentNugetProgress;

		public delegate void PropsHandleProgressChanged(NugetDownloaderWorker m, ProgressChangedEventArgs e);

		public NugetDownloaderWorker(NugetManager pManager, int pId, BackgroundWorker worker)
		{
			mManager = pManager;
			mId = pId;

			webClient = new WebClient();
			webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
			webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(DownloadCompleted);

			mWorker = worker;
			mWorker.WorkerReportsProgress = true;
			mWorker.WorkerSupportsCancellation = true;
			mWorker.DoWork += new DoWorkEventHandler(ProcessUntilDone);

		}

		private void ReportProgress()
		{
			mWorker.ReportProgress(0, currentNugetProgress);
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
					currentNugetProgress = new NugetProgressItem(nuget);
					ReportProgress();

					if (isNugetAlreadyInLocal())
					{
						currentNugetProgress.downloadPercent = 100;
						ReportProgress();
					}
					else
					{
						string Url = mManager.mParams.remoteNugetPath + nuget.GetNugetPath();

						// HERE YOU CAN SPECIFY IF YOU WANT FOLDER STRUCTURE
						// OR JUST BUNCHA NUGETS AT ROOT
						string downloadPath = mManager.mParams.stagingNugetPath + nuget.GetFileName();
						currentNugetProgress.pathOnDisk = downloadPath;

						WriteConsole(String.Format("Downloading to '{0}'", downloadPath));

						IsDownloading = true;
						webClient.DownloadDataAsync(new Uri(Url), downloadPath);

						while (IsDownloading)
						{
							if (isAborting)
							{
								return;
							}
							Thread.Sleep(200);
						}
					}

					ProcessNugetDependencies();

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

		public void DownloadProgress(object sender, DownloadProgressChangedEventArgs args)
		{
			currentNugetProgress.downloadPercent = args.ProgressPercentage;
			ReportProgress();
		}

		public void DownloadCompleted(object sender, DownloadDataCompletedEventArgs args)
		{
			currentNugetProgress.downloadPercent = 100;
			IsDownloading = false;
			ReportProgress();
		}

		public bool isNugetAlreadyInLocal()
		{
			// search main nuget path
			string mainPath = mManager.mParams.localNugetPath + currentNugetProgress.nuget.GetFileName();
			if (File.Exists(mainPath))
			{
				currentNugetProgress.pathOnDisk = mainPath;
				return true;
			}
			// search staging nuget path
			string stagingPath = mManager.mParams.stagingNugetPath + currentNugetProgress.nuget.GetFileName();
			if (File.Exists(stagingPath))
			{
				currentNugetProgress.pathOnDisk = stagingPath;
				return true;
			}
			return false;
		}

		private void ProcessNugetDependencies()
		{
			//TODO
		}

		public void Dispose()
		{
			isAborting = true;
			mWorker.CancelAsync();
			mWorker.Dispose();
			webClient.CancelAsync();
			webClient.Dispose();
		}
	}
}
