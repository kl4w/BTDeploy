using System;
using NUnit.Framework;
using BTDeploy.ServiceDaemon.TorrentClients;
using System.IO.Abstractions.TestingHelpers;
using System.Collections.Generic;
using FakeItEasy;
using System.IO;
using NLipsum.Core;

namespace BTDeploy.Tests
{
	public abstract class TorrentClientBase
	{
		public ITorrentClient TorrentClient;
		
		protected const string fakePath = "/my/fake/path";
		protected const string fakeId = "myFakeId";
		protected Stream fakeStream = A.Fake<Stream> ();
		protected MockFileData fakeMockFileData = A.Fake<MockFileData> ();

		[SetUp]
		public void SetUp()
		{
			var fakefileSystem = A.Fake<MockFileSystem> ();
			var fakeTorrentClient = A.Fake<ITorrentClient> ();

			// Fake list call
			A.CallTo (() => fakeTorrentClient.List ()).Returns (CreateTorrents (8)); // Because 8 is a lucky number for some...

			// Fake add call
			A.CallTo (() => fakeTorrentClient.Add (fakeStream, fakePath)).Invokes (() => fakefileSystem.AddFile(fakePath, fakeMockFileData));

			// Fake remove call
			A.CallTo (() => fakeTorrentClient.Remove (fakeId, false)).Invokes (() => fakefileSystem.RemoveFile (fakePath));

			TorrentClient = fakeTorrentClient;
			TorrentClient.FileSystem = fakefileSystem;
		}
		
		private ITorrentDetails[] CreateTorrents(int randomNumber)
		{
			var details = new List<ITorrentDetails> ();
			for (int i = 0; i < randomNumber; ++i) 
			{
				details.Add(new TorrentDetails {
					Id = string.Format("testId{0}", randomNumber),
					Name = string.Format("testName{0}", randomNumber),
					Files = CreateNewFiles(),
					OutputDirectory = string.Format("testOutputDirectory{0}", randomNumber),
					Status = TorrentStatus.Seeding
				});
			}
			return details.ToArray();
		}

		private string[] CreateNewFiles()
		{
			var files = new List<string> ();
			var randomNumber = LipsumUtilities.RandomInt (1, 9);
			for (int i = 0; i < randomNumber; ++i)
			{
				files.Add (LipsumGenerator.Generate(1));
			}
			return files.ToArray ();
		}
	}
}

