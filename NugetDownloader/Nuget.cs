﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NugetDownloader
{
	public class Nuget
	{

		public string name;
		// major.minor.patch[-suffix]
		public string version;

		public static Regex versionRegex = new Regex("\\d+\\.\\d+\\.\\d+(\\-[0-9A-Za-z-]+)?");

		//pass in nuget path, get
		public Nuget(string psPath)
		{
			string normalize = psPath.Replace("\\", "/");
			if (normalize.IndexOf("/") >= 0)
			{
				int index = normalize.LastIndexOf("/") + 1;
				normalize = normalize.Substring(index);
			}
			Match match = versionRegex.Match(normalize);
			if (match.Length > 0 && match.Index > 0)
			{
				name = normalize.Substring(0, match.Index-1);
				version = match.Value;
			}
		}

		public static bool TryParse(string psPath, out Nuget pNuget)
		{
			Nuget result = new Nuget(psPath);
			if (result.IsValid())
			{
				pNuget = result;
				return true;
			}
			pNuget = null;
			return false;
		}

		public bool IsValid()
		{
			return
			(
				(name != null) &&
				(name != "") &&
				(version != null) &&
				(version != "")
			);
		}

		public string GetFileName()
		{
			return string.Format(
				"{0}.{1}.nupkg",
				name.ToLower(),
				version.ToLower()
			);
		}
		public string GetNugetPath()
		{
			return string.Format(
				"{0}/{1}/{0}.{1}.nupkg",
				name.ToLower(),
				version.ToLower()
			);
		}
	}
}
