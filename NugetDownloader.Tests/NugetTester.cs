using System;
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

		[TestMethod]
		public void TestNugetNameGeneration()
		{
			Nuget nuget1 = new Nuget("https://api.nuget.org/v3-flatcontainer/entityframework/4.1.10715/entityframework.4.1.10715.nupkg");
			Assert.AreEqual("entityframework/4.1.10715/entityframework.4.1.10715.nupkg", nuget1.getNugetPath());
			Nuget nuget2 = new Nuget(nuget1.getNugetPath());
			Assert.AreEqual("entityframework/4.1.10715/entityframework.4.1.10715.nupkg", nuget2.getNugetPath());
			Nuget nuget3 = new Nuget(nuget1.getFileName());
			Assert.AreEqual("entityframework.4.1.10715.nupkg", nuget3.getFileName());
			Assert.AreEqual(nuget1.name, nuget3.name);
			Assert.AreEqual(nuget1.version, nuget3.version);

		}

		public void testNugetParse
		(
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
