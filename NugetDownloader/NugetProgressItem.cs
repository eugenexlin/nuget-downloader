using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetDownloader
{
	public class NugetProgressItem
	{
		public NugetProgressItem(Nuget pNuget)
		{
			nuget = pNuget;
		}
		public Nuget nuget { get; private set; }
		public int sizeBytes = 0;
		public int downloadedBytes = 100;
		public int downloadPercent = 0;
		public string pathOnDisk = "";
	}
}
