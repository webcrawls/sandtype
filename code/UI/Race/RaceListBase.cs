using System;
using System.Numerics;
using Sandbox;
using Sandbox.Menu;
using Sandbox.UI;

namespace TerryTyper.UI.Race;

public class RaceListBase : Panel
{

	private bool InLobby = false;
	private ILobby Lobby;
	private TimeUntil TimeUntilRefresh = 15f;
	private TimeSince TimeSinceLastInteract = 0f;

	protected void JoinRace(long raceId)
	{
		if ( TimeSinceLastInteract < 0.5f ) return;
		TimeSinceLastInteract = 0f;
		TyperGame.JoinGameCmd( raceId );
	}
	
	protected void CreateRace()
	{
		if ( TimeSinceLastInteract < 0.5f ) return;
		TimeSinceLastInteract = 0f;
		TyperGame.CreateGameCmd(  );
	}

	protected void DeleteRace( long raceId )
	{
		if ( TimeSinceLastInteract < 0.5f ) return;
		TimeSinceLastInteract = 0f;
		TyperGame.DeleteGameCmd( raceId );
	}
	
	protected void HandleLeaveRace()
	{
		if ( TimeSinceLastInteract < 0.5f ) return;
		TimeSinceLastInteract = 0f;
		TyperGame.LeaveGameCmd();
	}

}
