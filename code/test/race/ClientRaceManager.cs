using System.Collections.Generic;
using Sandbox;
using Sandtype.Entity.Pawn;

namespace Sandtype.Test;

public partial class ClientRaceManager : EntityComponent<Pawn>
{
	[Net, Change] public IList<RaceData> Races { get; set; }

	public void OnRacesChanged(IList<RaceData> oldValue, IList<RaceData> newValue)
	{
		Log.Info( newValue[0] );
		Log.Info( $"Races changed (Count: {newValue.Count})" );
	}
	
}
