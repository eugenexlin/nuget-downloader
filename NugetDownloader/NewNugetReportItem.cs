using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetDownloader
{
	public class NewNugetReportItem
	{
		public string id { get; set; }
		public string version { get; set; }
		public string authors { get; set; }
		public string owners { get; set; }
		public string projectUrl { get; set; }
		
		public NewNugetReportItem(Nuget nuget)
		{
			id = nuget.id;
			version = nuget.version;
		}

	}
}
