using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Sandbox;
using Sandbox.Menu;
using Sandbox.UI;
using TerryTyper.Race;

namespace TerryTyper.UI.Race;

public class RaceListBase : Panel
{

	private bool InLobby = false;
	private ILobby Lobby;
	private TimeUntil TimeUntilRefresh = 15f;
	private TimeSince TimeSinceLastInteract = 0f;

	protected void JoinRace(int raceId)
	{
		if ( TimeSinceLastInteract < 0.5f ) return;
		TimeSinceLastInteract = 0f;
		RaceEntity.JoinGameCmd( raceId, TyperGame.Entity.GamePawn.Data.SelectedTheme );
	}
	
	protected void DeleteRace( int raceId )
	{
		if ( TimeSinceLastInteract < 0.5f ) return;
		TimeSinceLastInteract = 0f;
		RaceEntity.DeleteGameCmd( raceId );
	}
	
	protected void HandleLeaveRace()
	{
		if ( TimeSinceLastInteract < 0.5f ) return;
		TimeSinceLastInteract = 0f;
		RaceEntity.LeaveGameCmd( );
	}
	
	// todo what is this stuff doing here
	protected int SelectedIndex = 0;
	protected IList<RaceEntity> Races => Entity.All.OfType<RaceEntity>().ToList();
	
	protected override int BuildHash()
	{
		var code = new HashCode();
		foreach ( var race in Races )
		{
			code.Add( race.RaceId );
		}
		var res = code.ToHashCode();
		return res;
	}

	protected void HandleCreateRace()
	{
		if ( TimeSinceLastInteract < 0.5f ) return;
		TimeSinceLastInteract = 0f;
		RaceEntity.CreateGameCmd(  );
	}


}
