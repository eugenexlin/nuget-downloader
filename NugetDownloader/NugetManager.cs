using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.IO;

namespace NugetDownloader
{
	public class NugetManager
	{

		public event NugetProgressChangedHandler ProgressChanged;
		public delegate void NugetProgressChangedHandler(NugetManager m, NugetProgressArgs e);

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

		//These are here to generate the report at the end.
		private List<NewNugetReportItem> newNugets = new List<NewNugetReportItem>();
		private Dictionary<string, NewNugetReportItem> newNugetHash = new Dictionary<string, NewNugetReportItem>();

		public NugetManager(NugetManagerParams p)
		{
			mParams = p;
		}

		public void WriteConsole(string message)
		{
			string sTime = DateTime.Now.ToLongTimeString() + " - ";
			if (message.EndsWith(Environment.NewLine))
			{
				consoleOutput.Enqueue(sTime + message);
			}
			else
			{
				consoleOutput.Enqueue(sTime + message + Environment.NewLine);
			}
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

		// only add if it is newly downloaded
		public void AddNewNugetReportItem(Nuget nuget)
		{
			string key = nuget.GetFileName();
			if (newNugetHash.ContainsKey(key))
			{
				return;
			}
			NewNugetReportItem item = new NewNugetReportItem(nuget);
			newNugetHash[key] = item;
			newNugets.Add(item);
		}
		// only update if was added to dict with above function
		public void UpdateNewNugetReportItem(Nuget nuget, XmlElement xMetadata)
		{
			string key = nuget.GetFileName();
			if (!newNugetHash.ContainsKey(key))
			{
				return;
			}
			NewNugetReportItem item = newNugetHash[key];
			item.authors = GetElementValue(xMetadata, "authors");
			item.owners = GetElementValue(xMetadata, "owners");
			item.projectUrl = GetElementValue(xMetadata, "projectUrl");
		}

		private string GetElementValue(XmlElement xMetadata, string tagName)
		{
			XmlNode node = xMetadata.SelectSingleNode(tagName);
			if (node == null)
			{
				return "";
			}
			return node.InnerText;
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

				GenerateReport();
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

		private void GenerateReport()
		{
			NewNugetReportItemComparer comparer = new NewNugetReportItemComparer();
			newNugets.Sort(comparer);

			StringBuilder reportHtml = new StringBuilder();
			DateTime timestamp = DateTime.Now;

			reportHtml.Append("<html>");
			reportHtml.Append("<head>");
			reportHtml.Append(
				"<style>" +
				" html { background: #eee; } " +
				" * { font-family: 'Segoe UI', Sans-Serif; } " +
				".nuget-item{" +
				" margin-bottom: 8px; padding: 8px; background: #fff; border-radius:3px; " +
				" box-shadow: 0 1px 3px rgba(0,0,0,0.15), 1px 1px 2px rgba(0,0,0,0.2); " +
				" transition: all 0.3s cubic-bezier(.25, .8, .25, 1); " +
				"} " +
				".nuget-item:hover{" +
				" box-shadow: 0 4px 8px rgba(0,0,0,0.2), 1px 1px 3px rgba(0,0,0,0.3); " +
				"} " +
				".id-ver-block{" +
				" display:inline-block; vertical-align: top; margin-right:24px; " +
				"} " +
				".extra-info-block{" +
				" display:inline-block; vertical-align: top; " +
				"} " +
				" h2 {" +
				" margin-bottom:0; " +
				"} " +
				".id {" +
				" font-weight:bold; " +
				"} " +
				".version{" +
				" margin-left:20px; " +
				"} " +
				".stage-path{" +
				" margin-left:10px; font-style:italic; " +
				"} " +
				".timestamp{" +
				" margin-left:10px; margin-bottom:20px; font-style:italic; " +
				"} " +
				" td {" +
				" vertical-align:top; " +
				"} " +
				" .t-label {" +
				" color: #888; " +
				"} " +
				" .t-value {" +
				" color: #000; " +
				"} " +
				"</style>");
			reportHtml.Append("</head>");
			reportHtml.Append("<body>");
			reportHtml.Append("<h2>Downloaded Nugets</h2>");
			reportHtml.Append("<div class='stage-path'>" + mParams.stagingNugetPath + "</div>");
			reportHtml.Append("<div class='timestamp'>" + timestamp.ToString() + "</div>");
			if (newNugets.Count() <= 0)
			{
				reportHtml.Append("<div>No newly downloaded nugets!</div>");
			}
			else
			{
				string previousId = "";
				bool openNugetItemDiv = false;
				NewNugetReportItem prevItem = null;
				foreach (NewNugetReportItem item in newNugets)
				{
					if (previousId != item.id)
					{
						if (openNugetItemDiv)
						{
							openNugetItemDiv = false;
							reportHtml.Append("</div>");
							reportHtml.Append(getNugetExtraInfoHtml(prevItem));
							reportHtml.Append("</div>");
						}
						reportHtml.Append("<div class='nuget-item'>");
						reportHtml.Append("<div class='id-ver-block'>");
						reportHtml.Append("<div class='id'>" + item.id + "</div>");
						openNugetItemDiv = true;
						previousId = item.id;
					}
					reportHtml.Append("<div class='version'>" + item.version + "</div>");
					prevItem = item;
				}
				if (openNugetItemDiv)
				{
					openNugetItemDiv = false;
					reportHtml.Append("</div>");
					reportHtml.Append(getNugetExtraInfoHtml(prevItem));
					reportHtml.Append("</div>");
				}
			}
			reportHtml.Append("</body></html>");

			string reportName = "nugetdownloader-" + DateTime.Now.ToString("yyyy-M-dd--HHmmssff") + ".html";
			string reportPath = mParams.outputReportPath + reportName;
			File.WriteAllText(reportPath, reportHtml.ToString());
			WriteConsole("Report generated at " + reportPath);
			System.Diagnostics.Process.Start(@reportPath);
		}

		private string getNugetExtraInfoHtml(NewNugetReportItem item)
		{
			if (item == null) { return ""; }
			StringBuilder result = new StringBuilder();
			result.Append("<div class='extra-info-block'><table>");
			result.Append("<tr>");
			result.Append("<td class='t-label'>author: </td>");
			result.Append("<td class='t-value'>" + item.authors + "</td>");
			result.Append("</tr>");
			result.Append("<tr>");
			result.Append("<td class='t-label'>owner: </td>");
			result.Append("<td class='t-value'>" + item.owners + "</td>");
			result.Append("</tr>");
			if (item.projectUrl != null && item.projectUrl.Length > 0)
			{
				result.Append("<tr>");
				result.Append("<td class='t-label'>website: </td>");
				result.Append("<td class='t-value'>" + String.Format("<a href='{0}'>{0}</a>", item.projectUrl) + "</td>");
				result.Append("</tr>");
			}
			string nugetWebUrl = String.Format("https://www.nuget.org/packages/{0}", item.id);
			result.Append("<tr>");
			result.Append("<td class='t-label'>inspect: </td>");
			result.Append("<td class='t-value'>" + String.Format("<a href='{0}'>{0}</a>", nugetWebUrl) + "</td>");
			result.Append("</tr>");
			result.Append("</table></div>");
			return result.ToString();
		}
	}
	
	public class NugetProgressArgs : EventArgs
	{
		public NugetProgressItem nugetProgress { get; set; }
	}

}
