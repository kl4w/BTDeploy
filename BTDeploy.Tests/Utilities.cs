using System;
using System.IO.Abstractions.TestingHelpers;
using System.Collections.Generic;

namespace BTDeploy.Tests
{
	public static class Utilities
	{
		public static MockFileSystem CreateMockFileSystem()
		{
			var fileSystem = new MockFileSystem (CreateExpectations ());
			return fileSystem;
		}

		public static Dictionary<string, MockFileData> CreateExpectations()
		{
			var files = new Dictionary<string, MockFileData> ();
			return files;
		}
	}
}

