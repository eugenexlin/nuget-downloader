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
		// if for some reason comparing greater or less via string is how microsoft does it
		// HA LLE LU JAH [EXH]
		public string frameworkVersion;

		public List<Nuget> nugetsToDownload = new List<Nuget>();
		
	}
}
