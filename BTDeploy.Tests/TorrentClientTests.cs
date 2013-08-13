using System;
using NUnit.Framework;
using System.IO;
using FakeItEasy;
using System.IO.Abstractions.TestingHelpers;
using NLipsum.Core;

namespace BTDeploy.Tests
{
	[TestFixture]
	public class TorrentClientTests : TorrentClientBase
	{
		[Test]
		public void AddTest ()
		{
			TorrentClient.Add (fakeStream, fakePath);
			Assert.IsTrue (TorrentClient.FileSystem.File.Exists(fakePath));
		}

		[Test]
		public void List()
		{
			var list = TorrentClient.List ();
			Assert.IsTrue (list.Length == 8);
		}

		[Test]
		public void Remove()
		{
			TorrentClient.FileSystem.File.WriteAllText (fakePath, LipsumGenerator.Generate (1));

			// make sure file exists
			Assert.IsTrue (TorrentClient.FileSystem.File.Exists (fakePath));

			// remove file
			TorrentClient.Remove (fakeId);
			Assert.IsFalse (TorrentClient.FileSystem.File.Exists (fakePath));
		}
	}
}

