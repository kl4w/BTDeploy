using System;
using ManyConsole;
using ServiceStack.Service;
using BTDeploy.ServiceDaemon;
using System.Collections.Generic;
using BTDeploy.ServiceDaemon.TorrentClients;
using BTDeploy.Helpers;
using System.Text.RegularExpressions;
using System.Linq;
using BTDeploy.Client.Commands;
using System.Threading;

namespace BTDeploy.Client.Commands
{
	public abstract class GeneralConsoleCommandBase : ConsoleCommand
	{
		protected readonly IEnvironmentDetails EnvironmentDetails;
		protected readonly IRestClient Client;

		protected bool Start = true;
		protected bool Kill = true;

		public GeneralConsoleCommandBase (IEnvironmentDetails environmentDetails, IRestClient client, string oneLineDescription)
		{
			// Set the base commands.
			IsCommand (this.GetType ().Name, oneLineDescription);
			HasOption ("noStart", "Prevents the long lasting process from being spawned automatically", o => Start = o == null);
			HasOption ("noStop", "Prevents the long lasting background daemon from being killed, this process will continue downloading and/or seeding after the application has exited.", o => Kill = o == null);
			SkipsCommandSummaryBeforeRunning ();

			// Set common items.
			EnvironmentDetails = environmentDetails;
			Client = client;
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			// If start, make sure started.
			if (Start && SocketHelpers.IsTCPPortAvailable(EnvironmentDetails.ServiceDaemonPort))
			{
				new Admin (EnvironmentDetails, Client) { Start = true }.Run (new string[] { });
				while(SocketHelpers.IsTCPPortAvailable(EnvironmentDetails.ServiceDaemonPort))
					Thread.Sleep(500);
			}

			// Before dying, send stop request.
			AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
			{
				if (Kill && Client.Get (new TorrentsListRequest ()).Count() == 0) 
					new Admin(EnvironmentDetails, Client) { Stop = true }.Run (new string[] {});
			};

			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		}

		protected IEnumerable<ITorrentDetails> FilterByIdOrPattern(IEnumerable<string> idOrPatterns, IEnumerable<ITorrentDetails> torrentDetailsCollection)
		{
			// Set ids and patterns.
			var ids = idOrPatterns;
			var patterns = idOrPatterns.Select (p => new Wildcard (p, RegexOptions.IgnoreCase | RegexOptions.Compiled)).ToList();

			// Do matching.
			var torrentDetailsIdMatches = torrentDetailsCollection.Where (torrentDetails => ids.Contains (torrentDetails.Id));
			var torrentDetailsPatternMatches = torrentDetailsCollection.Where (torrentDetails => patterns.Any(p => p.Match(torrentDetails.Name).Success)).ToList();

			// Return filtered list.
			return Enumerable.Union (torrentDetailsIdMatches, torrentDetailsPatternMatches).ToList ();
		}
	}
}