﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NugetDownloader;

namespace NugetDownloader.Tests
{
	[TestClass]
	public class NugetTester
	{
		[TestMethod]
		public void TestParseOutNameAndVersion()
		{
			testNugetParse(
				"https://api.nuget.org/v3-flatcontainer/microsoft.aspnetcore.http/2.0.0/microsoft.aspnetcore.http.2.0.0.nupkg",
				"microsoft.aspnetcore.http",
				"2.0.0"
			);
			testNugetParse(
				"https://api.nuget.org/v3-flatcontainer/microsoft.aspnetcore.http/2.1.0-preview1-final/microsoft.aspnetcore.http.2.1.0-preview1-final.nupkg",
				"microsoft.aspnetcore.http",
				"2.1.0-preview1-final"
			);
			testNugetParse(
				"https://api.nuget.org/v3-flatcontainer/entityframework/4.1.10715/entityframework.4.1.10715.nupkg",
				"entityframework",
				"4.1.10715"
			);
			testNugetParse(
				"entityframework.4.1.10715.nupkg",
				"entityframework",
				"4.1.10715"
			);
			testNugetParse(
				"runtime.win7-x86.runtime.native.system.data.sqlclient.sni.4.0.1-rc2-24027.nupkg",
				"runtime.win7-x86.runtime.native.system.data.sqlclient.sni",
				"4.0.1-rc2-24027"
			);
		}

		public void testNugetParse(
			string input, 
			string expectedName, 
			string expectedVersion
			)
		{
			Nuget nuget = new Nuget(input);
			Assert.AreEqual(expectedName, nuget.name);
			Assert.AreEqual(expectedVersion, nuget.version);
		}
	}
}