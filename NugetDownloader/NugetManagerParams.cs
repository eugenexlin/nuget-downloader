using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetDownloader
{
	public class NugetManagerParams
	{
		public string remoteNugetPath;
		public string localNugetPath;
		public string stagingNugetPath;
		public string outputReportPath;

		public string framework;
		public string frameworkVersion;

		public List<Nuget> nugetsToDownload = new List<Nuget>();
		
	}
}
