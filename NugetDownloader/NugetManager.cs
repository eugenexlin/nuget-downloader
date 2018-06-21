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
		private string mNugetServerPath;
		private string nLocalNugetPath;
		private string mStagingNugetPath;

		//path to output report
		private string mReportPath;

		private bool mIsStarted = false;

		private ConcurrentQueue<Nuget> mNugetsToProcess;

		public NugetManager
			(
				string pNugetServerPath,
				string pLocalNugetPath,
				string pStagingNugetPath,
				string pReportPath
			)
		{
			mNugetServerPath = pNugetServerPath;
			nLocalNugetPath = pLocalNugetPath;
			mStagingNugetPath = pStagingNugetPath;
			mReportPath = pReportPath;
		}

		public void addNugetToQueue(Nuget nuget)
		{
			mNugetsToProcess.Enqueue(nuget);
		}

		public void execute()
		{
			if(mIsStarted)
			{
				return;
			}
			mIsStarted = true;

		}

	}
}
