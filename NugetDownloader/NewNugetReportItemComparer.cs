using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetDownloader
{
	public class NewNugetReportItemComparer : IComparer<NewNugetReportItem>
	{
		public int Compare(NewNugetReportItem x, NewNugetReportItem y)
		{
			int result = x.id.CompareTo(y.id);
			if (result == 0)
			{
				result = x.version.CompareTo(y.version);
			}
			return result;
		}
	}
}
