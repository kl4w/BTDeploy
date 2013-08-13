using System;
using NUnit.Framework;
using BTDeploy.ServiceDaemon.TorrentClients;
using System.IO.Abstractions.TestingHelpers;
using System.Collections.Generic;
using Moq;
using System.IO;

namespace BTDeploy.Tests
{
	public abstract class TorrentClientBase
	{
		public ITorrentClient TorrentClient;

		[SetUp]
		public void SetUp()
		{
			var fileSystem = new MockFileSystem ();
			var mockedTorrentClient = new Mock<ITorrentClient> () { CallBase = true };
			var torrentDetails = new TorrentDetails {
				Id = "testId",
				Name = "testName",
				Files = new string[] {},
				OutputDirectory = "testOutputDirectory",
				Status = TorrentStatus.Seeding
			};
			mockedTorrentClient.Setup (c => c.List ()).Returns (new ITorrentDetails[] { torrentDetails });
			mockedTorrentClient.Setup (c => c.FileSystem).Returns (fileSystem);
			mockedTorrentClient.Setup (c => c.Add (It.IsAny<Stream> (), It.IsAny<string> ()))
				.Callback<Stream, string> ((t, o) => fileSystem.AddFile (o, new MockFileData("blah.")));
			TorrentClient = mockedTorrentClient.Object;
		}
	}
}

