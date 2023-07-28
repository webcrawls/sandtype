using System;
using System.Collections;
using System.Collections.Generic;
using Sandbox;

namespace TerryTyper.Race;

public partial class RaceData : BaseNetworkable
{
	
	[Net] public long OwnerId { get; set; }
	[Net] public long RaceId { get; set; }
	[Net] public IList<long> Players { get; set; }
	[Net] public IList<long> QuitPlayers { get; set; }
	[Net] public IList<long> Winners { get; set; }
	[Net] public IDictionary<long, string> Themes { get; set; }
	[Net] public IList<string> TargetTokens { get; set; }
	[Net] public IDictionary<long, string> Inputs { get; set; }
	[Net] public IDictionary<long, string> CurrentInputs { get; set; }
	[Net] public string OwnerName { get; set; }
	[Net] public bool Started { get; set; }

	public RaceData CopyOf()
	{
		return new RaceData()
		{
			OwnerId = OwnerId,
			RaceId = RaceId,
			Players = new List<long>(Players),
			Themes = new Dictionary<long, string>(Themes),
			TargetTokens = new List<string>(TargetTokens),
			Winners = new List<long>(Winners),
			QuitPlayers = new List<long>(QuitPlayers),
			Inputs = new Dictionary<long, string>(Inputs),
			CurrentInputs = new Dictionary<long, string>(CurrentInputs),
			OwnerName = OwnerName,
			Started = Started
		};
	}
}
