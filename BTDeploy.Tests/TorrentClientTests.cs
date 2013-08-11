using System;
using NUnit.Framework;
using System.IO;

namespace BTDeploy.Tests
{
	[TestFixture]
	public class TorrentClientTests : TorrentClientBase
	{
		[Test]
		public void AddTest ()
		{
			var torrentFile = new MemoryStream ();
			var outputPath = "test";
			TorrentClient.Add (torrentFile, outputPath);
			Assert.IsTrue (TorrentClient.FileSystem.File.Exists(outputPath));
		}

		[Test]
		public void List()
		{
			var list = TorrentClient.List ();
			Assert.IsTrue (list.Length != 0);
		}
	}
}

