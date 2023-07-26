using System.Collections.Generic;
using Sandbox;
using Sandtype.Entity.Pawn;

namespace Sandtype.Test;

public class ServerRaceManager : EntityComponent<GameManager>
{
	public IList<RaceData> Races { get; set; }

	protected override void OnActivate()
	{
		base.OnActivate();
		Log.Info( "Creating races" );
		Races = new List<RaceData>();
		Log.Info( Races );
	}

	public void CreateRace( Pawn author )
	{
		var race =new RaceData()
		{
			OwnerId = author.Client.SteamId,
			OwnerName = author.Client.Name,
			RaceId = author.Client.SteamId
		};
		Races.Add( race );
		
		Log.Info( "Race created for "+author.Name );

		author.Components.Get<ClientRaceManager>().Races.Add( race );
	}
	
}
