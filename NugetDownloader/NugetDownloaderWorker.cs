using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace NugetDownloader
{
	public class NugetDownloaderWorker
	{
		private NugetManager mManager;
		private int mId;
		public bool IsProcessing { get; private set; } = true;
		public bool IsFinished { get; private set; } = false;

		//private int workerFailureCount = 0;
		private const int MAX_PACKAGE_FAILURE_COUNT = 5;

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
			webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);

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
			try
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
							WriteConsole(String.Format("Found Existing '{0}'", currentNugetProgress.pathOnDisk));
							currentNugetProgress.downloadPercent = 100;
							ReportProgress();
						}
						else
						{
							IsDownloading = true;
							mManager.AddNewNugetReportItem(nuget);

							string Url = mManager.mParams.remoteNugetPath + nuget.GetNugetPath();

							// have not figured out nuget folder structure yet.
							// will go will nugets in root.
							//string downloadPath = mManager.mParams.stagingNugetPath + nuget.GetNugetPath().Replace("/", "\\");
							string downloadPath = mManager.mParams.stagingNugetPath + nuget.GetFileName();
							string parentDir = Path.GetDirectoryName(downloadPath);
							if (!Directory.Exists(parentDir))
							{
								Directory.CreateDirectory(parentDir);
							}
							currentNugetProgress.pathOnDisk = downloadPath;

							WriteConsole(String.Format("Downloading from '{0}' to '{1}'", Url, downloadPath));

							webClient.DownloadFileAsync(new Uri(Url), downloadPath);

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
						WriteConsole("Error in worker thread: " + ex.ToString());
						if (nuget != null)
						{
							nuget.failCount += 1;
							if (nuget.failCount >= MAX_PACKAGE_FAILURE_COUNT)
							{
								WriteConsole(String.Format("Error: '{0}' failed too many times. Bye!", nuget.GetFileName()));
								mManager.RaiseTheFlagOfError();
							}
							else
							{
								mManager.ForceAddNugetToQueue(nuget);
							}
						}
						//workerFailureCount += 1;
						//if (workerFailureCount > MAX_FAILURE_COUNT)
						//{
						//	return;
						//}
						continue;
					}
				}
			}
			finally
			{
				IsFinished = true;
				WriteConsole("Thread Exited");
			}
		}

		public void DownloadProgress(object sender, DownloadProgressChangedEventArgs args)
		{
			currentNugetProgress.downloadPercent = args.ProgressPercentage;
			ReportProgress();
		}

		public void DownloadCompleted(object sender, AsyncCompletedEventArgs args)
		{
			currentNugetProgress.downloadPercent = 100;
			IsDownloading = false;
			ReportProgress();
		}

		public bool isNugetAlreadyInLocal()
		{
			// search main nuget path
			string mainFile = mManager.mParams.localNugetPath + currentNugetProgress.nuget.GetFileName();
			if (File.Exists(mainFile))
			{
				currentNugetProgress.pathOnDisk = mainFile;
				return true;
			}
			// search main nuget path
			string mainPath = mManager.mParams.localNugetPath + currentNugetProgress.nuget.GetNugetPath();
			if (File.Exists(mainPath))
			{
				currentNugetProgress.pathOnDisk = mainPath;
				return true;
			}
			// search staging nuget path
			string stagingFile = mManager.mParams.stagingNugetPath + currentNugetProgress.nuget.GetFileName();
			if (File.Exists(stagingFile))
			{
				currentNugetProgress.pathOnDisk = stagingFile;
				return true;
			}
			// search staging nuget path
			string stagingPath = mManager.mParams.stagingNugetPath + currentNugetProgress.nuget.GetNugetPath();
			if (File.Exists(stagingPath))
			{
				currentNugetProgress.pathOnDisk = stagingPath;
				return true;
			}
			return false;
		}

		private void ProcessNugetDependencies()
		{

			string sourcePath = currentNugetProgress.pathOnDisk;
			string tempFolder = Path.GetTempPath() + "NugetDownloader\\" + currentNugetProgress.nuget.GetFolderName();
			try
			{
				if (!File.Exists(sourcePath))
				{
					throw new Exception(String.Format("File '{0}' not found.", sourcePath));
				}
				if (new FileInfo(sourcePath).Length <= 0)
				{
					throw new Exception(String.Format("File '{0}' has zero size. Maybe this version does not exist?", sourcePath));
				}
				if (Directory.Exists(tempFolder))
				{
					Directory.Delete(tempFolder, true);
				}

				if (currentNugetProgress.nuget.id.Contains("ext"))
				{
					throw new Exception("failfish");
				}

				try
				{
					// separate try catch for extracting nuget, because if failure, then we should delete and redownload.
					using (ZipArchive za = ZipFile.Open(sourcePath, ZipArchiveMode.Read))
					{
						za.ExtractToDirectory(tempFolder);
						za.Dispose();
					}
				}
				catch
				{
					// problem specifically with extracting.
					// going to delete the source path on disk
					// because it is probably corrupt.
					File.Delete(currentNugetProgress.pathOnDisk);
					// and throw to try to redownload.
					throw;
				}

				string nuspecPath = tempFolder + currentNugetProgress.nuget.id + ".nuspec";
				XmlDocument nuspecDoc = new XmlDocument();
				using (XmlTextReader xtr = new XmlTextReader(nuspecPath))
				{
					xtr.Namespaces = false;
					nuspecDoc.Load(xtr);
				}

				mManager.UpdateNewNugetReportItem(currentNugetProgress.nuget, (XmlElement)nuspecDoc.SelectSingleNode("package/metadata"));

				XmlElement xDependencies = (XmlElement)nuspecDoc.SelectSingleNode("package/metadata/dependencies");

				if (xDependencies == null)
				{
					// cool, no dependencies?
					// we are done.
					return;
				}

				// BIG TODO
				// was planning for some framework version number string comparison,
				// but for now going by exact match only.
				XmlElement xMatchGroup = null;
				foreach (XmlElement xGroup in xDependencies.SelectNodes("group"))
				{
					string targetFramework = xGroup.GetAttribute("targetFramework");
					//if (targetFramework.StartsWith(mManager.mParams.framework))
					//{
					//	string frameworkVersion = targetFramework.Substring(mManager.mParams.framework.Length);
					//	if (mManager.mParams.frameworkVersion.CompareTo(frameworkVersion) > 0)
					//	{
					//		xMatchGroup = xGroup;
					//	}
					//}
					if (targetFramework == (mManager.mParams.framework + mManager.mParams.frameworkVersion))
					{
						xMatchGroup = xGroup;
					}
				}

				if (xMatchGroup == null)
				{
					//we have an inferior framework version, probably not supported.
					WriteConsole(String.Format(
						"Warning: no matching dependencies for '{0}'",
						currentNugetProgress.nuget.GetFileName()
					));
					mManager.RaiseTheFlagOfWarning();
					return;
				}

				foreach (XmlElement xDependency in xMatchGroup.SelectNodes("dependency"))
				{
					string name = xDependency.GetAttribute("id");
					string version = xDependency.GetAttribute("version");
					Nuget nuget = new Nuget(name, version);
					mManager.AddNugetToQueue(nuget);
				}
			}
			finally
			{
				if (Directory.Exists(tempFolder))
				{
					Directory.Delete(tempFolder, true);
				}
			}
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
