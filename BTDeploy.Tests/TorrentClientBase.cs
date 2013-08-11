using System;
using NUnit.Framework;
using BTDeploy.ServiceDaemon.TorrentClients;
using System.IO.Abstractions.TestingHelpers;
using System.Collections.Generic;

namespace BTDeploy.Tests
{
	public abstract class TorrentClientBase
	{
		public MonoTorrentClient TorrentClient;

		[SetUp]
		public void SetUp()
		{
			var fileSystem = new MockFileSystem (new Dictionary<string, MockFileData> {
				{ @"C:\foo\bar\test.txt", new MockFileData("hello world.") }
			});
			TorrentClient = new MonoTorrentClient ("~/appdata", fileSystem);
		}
	}
}

