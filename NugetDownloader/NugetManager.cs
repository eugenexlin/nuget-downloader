using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;

namespace NugetDownloader
{
	public class NugetManager
	{

		public event NugetProgressChangedHandler ProgressChanged;
		public delegate void NugetProgressChangedHandler(NugetManager m, NugetProgressArgs e);

		// removing this because cross thread txt access not allowed.
		// apparently event handlers are dont on the thread that called the event.
		//public event WriteConsoleHandler WroteConsole;
		//public delegate void WriteConsoleHandler(NugetManager m, WriteConsoleArgs e);

		private const int THREAD_COUNT = 4;

		private List<NugetDownloaderWorker> mWorkers = new List<NugetDownloaderWorker>();

		private bool HasError = false;
		private bool HasWarning = false;

		private bool mIsStarted = false;
		public NugetManagerParams mParams;
		private object mNugetLock = new object();

		private ConcurrentQueue<Nuget> mNugetsToProcess = new ConcurrentQueue<Nuget>();
		// hash nugets by their file name
		private HashSet<string> mNugetsFileNamesAlreadyAdded = new HashSet<string>();

		private List<NugetProgressItem> mNugetProgressItems = new List<NugetProgressItem>();
		
		public ConcurrentQueue<string> consoleOutput = new ConcurrentQueue<string>();

		public NugetManager(NugetManagerParams p)
		{
			mParams = p;
		}

		public void WriteConsole(string message)
		{
			//WriteConsoleArgs args = new WriteConsoleArgs();
			//args.message = message;

			string sTime = DateTime.Now.ToLongTimeString() + " - ";
			if (message.EndsWith(Environment.NewLine))
			{
				consoleOutput.Enqueue(sTime + message);
			}
			else
			{
				consoleOutput.Enqueue(sTime + message + Environment.NewLine);
			}
			//WroteConsole(this, args);
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


		// at this point should be on UI thread
		private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			NugetProgressItem nugetProgress = (NugetProgressItem)e.UserState;
			NugetProgressArgs args = new NugetProgressArgs();
			args.nugetProgress = nugetProgress;
			ProgressChanged(this, args);
		}

		// only use when there is an error and we might want to try it again.
		public void ForceAddNugetToQueue(Nuget nuget)
		{
			WriteConsole(String.Format("Queuing {0}", nuget.GetFileName()));
			mNugetsToProcess.Enqueue(nuget);
		}

		public void AddNugetToQueue(Nuget nuget)
		{
			if (CriticalIsNugetRepeatCheck(nuget))
			{
				return;
			}
			ForceAddNugetToQueue(nuget);
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

			foreach (Nuget nuget in mParams.nugetsToDownload) {
				AddNugetToQueue(nuget);
			}

			//spawn worker threads
			for (int i = 0; i < THREAD_COUNT; i++)
			{
				BackgroundWorker bgwrk = new BackgroundWorker();
				bgwrk.ProgressChanged += new ProgressChangedEventHandler(WorkerProgressChanged);
				NugetDownloaderWorker worker = new NugetDownloaderWorker(this, i, bgwrk);
				mWorkers.Add(worker);
			}
			// activate them after they are all in, because collection would change
			// in IsStillDoingWork
			foreach (NugetDownloaderWorker worker in mWorkers)
			{
				worker.Start();
			}

			//waiting thread so we can return thread to UI work.
			Thread managerThread = new Thread(() => {
				//wait for them all to terminate.
				bool workersRunning = true;
				while (workersRunning)
				{
					Thread.Sleep(200);
					workersRunning = false;
					foreach (NugetDownloaderWorker worker in mWorkers)
					{
						if (!worker.IsFinished)
						{
							workersRunning = true;
							break;
						}
					}
				}

				// ok everything should be done.
				if (HasError)
				{
					WriteConsole("Task has ended with ERROR!!");
				}
				else if(HasWarning)
				{
					WriteConsole("Task has ended with warning!");
				}
				else
				{
					WriteConsole("Task is a likely success!");
				}
			});
			managerThread.Start();
		}

		public void RaiseTheFlagOfError()
		{
			HasError = true;
		}
		public void RaiseTheFlagOfWarning()
		{
			HasWarning = true;
		}

		public void Dispose()
		{
			// activate them after they are all in, because collection would change
			// in IsStillDoingWork
			foreach (NugetDownloaderWorker worker in mWorkers)
			{
				worker.Dispose();
			}
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
	
	public class NugetProgressArgs : EventArgs
	{
		public NugetProgressItem nugetProgress { get; set; }
	}

	//public class WriteConsoleArgs : EventArgs
	//{
	//	public string message { get; set; }
	//}
}
