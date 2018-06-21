using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetDownloader
{
	public class NugetManager
	{

		public event NugetQueuedHandler NugetQueued;
		public delegate void NugetQueuedHandler(NugetManager m, NugetQueuedArgs e);

		public event WriteConsoleHandler WroteConsole;
		public delegate void WriteConsoleHandler(NugetManager m, WriteConsoleArgs e);

		private const int THREAD_COUNT = 4;

		private List<NugetDownloaderWorker> mWorkers = new List<NugetDownloaderWorker>();

		private bool mIsStarted = false;
		private NugetManagerParams mP;
		private object mNugetLock = new object();

		private ConcurrentQueue<Nuget> mNugetsToProcess = new ConcurrentQueue<Nuget>();
		// hash nugets by their file name
		private HashSet<string> mNugetsFileNamesAlreadyAdded = new HashSet<string>();

		private List<NugetProgressItem> mNugetProgressItems = new List<NugetProgressItem>();

		public NugetManager(NugetManagerParams p)
		{
			mP = p;
		}

		public void WriteConsole(string message)
		{
			WriteConsoleArgs args = new WriteConsoleArgs();
			args.message = message;
			WroteConsole(this, args);
		}

		private bool CriticalIsNugetRepeatCheck(Nuget nuget)
		{
			string sNuget = nuget.GetFileName();
			lock (mNugetLock)
			{
				if (mNugetsFileNamesAlreadyAdded.Contains(sNuget))
				{
					return true;
				}
				mNugetsFileNamesAlreadyAdded.Add(sNuget);
				return false;
			}
		}

		public void AddNugetToQueue(Nuget nuget)
		{
			if (CriticalIsNugetRepeatCheck(nuget))
			{
				return;
			}

			NugetQueuedArgs args = new NugetQueuedArgs();
			args.nuget = nuget;
			NugetQueued(this, args);
			mNugetsToProcess.Enqueue(nuget);
		}

		public Nuget DequeueNuget()
		{
			if (mNugetsToProcess.TryDequeue(out Nuget nuget))
			{
				return nuget;
			}
			return null;
		}

		public void Execute()
		{
			if(mIsStarted)
			{
				return;
			}
			mIsStarted = true;

			foreach (Nuget nuget in mP.nugetsToDownload) {
				AddNugetToQueue(nuget);
			}

			//spawn worker threads
			for (int i = 0; i < THREAD_COUNT; i++)
			{
				NugetDownloaderWorker worker = new NugetDownloaderWorker(this);
				mWorkers.Add(worker);
			}
			//wait for them all to terminate.

		}



		// this is basically the function that workers use to coordinate if they
		// should keep looping to process the queue;
		// if everyone is done, except for 1 worker, the others still need to stay up
		// perhaps that last nuget package will end up having 10 more dependencies.
		public bool IsStillDoingWork()
		{
			if (mNugetsToProcess.Count() > 0)
			{
				return true;
			}
			foreach (NugetDownloaderWorker worker in mWorkers)
			{
				if (worker.IsProcessing)
				{
					return true;
				}
			}
			return false;
		}

	}

	public class NugetQueuedArgs: EventArgs
	{
		public Nuget nuget { get; set; }
	}
	public class WriteConsoleArgs : EventArgs
	{
		public string message { get; set; }
	}
}
