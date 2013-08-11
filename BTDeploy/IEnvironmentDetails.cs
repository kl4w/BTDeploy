using System.IO.Abstractions;

namespace BTDeploy
{
	public interface IEnvironmentDetails
	{
		string ServiceDaemonCommand { get; }
		string ApplicationDataDirectoryPath { get; }
		int ServiceDaemonPort { get; }
		string ServiceDaemonEndpoint { get; }
		IFileSystem FileSystem { get; }
	}
}