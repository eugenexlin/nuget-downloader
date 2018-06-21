using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace NugetDownloader
{
	public class NugetDownloaderWorker
	{
		private NugetManager mManager;
		public bool IsProcessing { get; private set; } = false;

		public NugetDownloaderWorker(NugetManager pManager)
		{
			mManager = pManager;
			Thread work = new Thread(ProcessUntilDone);
		}

		private void ProcessUntilDone()
		{
			while (mManager.IsStillDoingWork())
			{
				try
				{

					Nuget nuget = mManager.DequeueNuget();
					if (nuget == null)
					{
						Thread.Sleep(100);
						continue;
					}
					IsProcessing = true;

				}
				catch (Exception ex)
				{
					mManager.WriteConsole("Error in worker thread: " + ex.Message);
					continue;
				}
				finally
				{
					IsProcessing = false;
				}
			}
		}
	}
}
