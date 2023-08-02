using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace TerryTyper;

public partial class Race : Entity
{

	public static bool IsInRace( long steamId )
	{
		return GetJoinedRace( steamId ) != null;
	}
	
	public static Race GetRace( int raceId )
	{
		return FindByIndex( raceId ) as Race;
	}

	public static Race GetJoinedRace( long steamId )
	{
		foreach ( var game in All.OfType<Race>()) 
		{
			if ( game.Players.Where( p => p.SteamId == steamId ).Count() != 0 )
			{
				return game;
			}
		}

		return null;
	}

	[Net] public long OwnerId { get; set; }
	[Net] public IList<long> QuitPlayers { get; set; }
	[Net] public IList<long> Winners { get; set; }
	[Net] public IList<string> Target { get; set; }
	[Net] public string OwnerName { get; set; }
	[Net] public RaceState State { get; set; }
	[Net] public float TimeUntilStart { get; set; }
	[Net] public float StartTime { get; set; }


	public Player GameOwner;
	public string Language;
	public int Size;
	public int RaceId => NetworkIdent;
	public List<RacePlayer> Players => Components.GetAll<RacePlayer>().ToList();

	private bool _countdownStarted = false;
	private TimeUntil _timeUntilStart = 0f;
	
	public override void Spawn()
	{
		base.Spawn();
		State = RaceState.NOT_YET_STARTED;
		Transmit = TransmitType.Always;
		OwnerName = GameOwner?.Client.Name ?? "Unknown";
		OwnerId = GameOwner?.Client.SteamId ?? 0;
		Name = OwnerName + "'s Race";
		QuitPlayers = new List<long>();
		Winners = new List<long>();
		Target = new List<string>();
		Tags.Add( "race" );
	}

	public RacePlayer GetPlayer( long steamId )
	{
		foreach ( var player in Players )
		{
			if ( player.SteamId == steamId ) return player;
		}

		return null;
	}

	public void AddPlayer(Player pawn)
	{
		var cmp = Components.Create<RacePlayer>();
		cmp.SteamId = pawn.Client.SteamId;
		cmp.SteamName = pawn.Client.Name;
		cmp.Input = new List<string>();
		cmp.CurrentInput = "";
		cmp.Theme = "default";
		cmp.Name = "Racer " + pawn.Client.Name;
		pawn.ShowRaceHud(To.Single( pawn ));
		StartCountdown();
	}
	
	public void RemovePlayer( Player pawn )
	{
		var player = GetPlayer( pawn.Client.SteamId );
		if ( player == null ) return;
		player.Remove();
		pawn.HideRaceHud(To.Single( pawn ));
	}

	public void StartCountdown()
	{
		if ( _countdownStarted ) return;
		_countdownStarted = true;
		_timeUntilStart = 7.5f;
		State = RaceState.COUNTING_DOWN;
	}
	
	[GameEvent.Tick.Server]
	private void TickServer()
	{
		if ( State == RaceState.COUNTING_DOWN )
		{
			TickCountdown();
			return;
		}

		// if the race is running
		if ( State == RaceState.RUNNING
		     // or, the race has ended but not all players have finished
		     || (State == RaceState.ENDED && Winners.Count != Players.Count))
		{
			foreach ( var player in Players )
			{
				CheckPlayerInput(player);
			}
		}
		
	}

	private void CheckPlayerInput( RacePlayer player )
	{
		if ( Winners.Contains( player.SteamId ) ) return;
		if ( player.Input.Count >= Target.Count )
			AddWinner( player );
	}
	
	private void AddWinner( RacePlayer player )
	{
		player.CompleteTime = Time.Now;
		player.Complete = true;
		var winners = new List<long>( Winners );
		winners.Add( player.SteamId );
		Winners = winners;

		if ( State != RaceState.ENDED )
			State = RaceState.ENDED;
	}

	private void TickCountdown()
	{
		var timeUntil = _timeUntilStart;
		if ( timeUntil <= 0 )
		{
			StartGame();
		}
		else
		{
			TimeUntilStart = timeUntil;
		}
	}

	private void StartGame()
	{
		State = RaceState.RUNNING;
		StartTime = Time.Now;
		Target = TextProvider.Providers[Language].GetText( Size );
	}
	
}

public enum RaceState
{
	NOT_YET_STARTED,
	COUNTING_DOWN,
	RUNNING,
	ENDED
}
