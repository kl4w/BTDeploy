using System;

namespace BTDeploy.ServiceDaemon.TorrentClients
{
	public interface ITorrentClient
	{
		ITorrentDetails[] List();
		string Add(string torrentPath, string outputDirectoryPath);
		void Remove(string Id, bool deleteFiles = false);
	}

	public interface ITorrentDetails
	{
		string Id { get; }
		string Name { get; }
		string[] Files { get; }
		string OutputDirectory { get; }
		TorrentStatus Status { get; }
		long Size { get; }
		double Progress { get; }
		double DownloadBytesPerSecond { get; }
		double UploadBytesPerSecond { get; }
	}

	public enum TorrentStatus
	{
		Hashing,
		Downloading,
		Seeding,
		Stopped,
		Error
	}
}