using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using Sandbox;
using Sandtype.Engine.Text;

namespace TerryTyper.Race;

public partial class RaceEntity : Entity
{

	[ConCmd.Server("tw_create")]
	public static void CreateGameCmd()
	{
		var caller = ConsoleSystem.Caller.Pawn as Pawn;
		var existingGame = GetJoinedRace( caller.Client.SteamId );
		if ( existingGame != null )
		{
			return;
		}
		
		var raceEntity = new RaceEntity();
		raceEntity.GameOwner = caller;
		raceEntity.Spawn();
	}

	[ConCmd.Server("tw_delete")]
	public static void DeleteGameCmd(int raceId)
	{
		var caller = ConsoleSystem.Caller.Pawn as Pawn;
		var race = GetRace( raceId );
		if ( race == null || race.OwnerId != caller.Client.SteamId )
		{
			return;
		}
		race.Delete();
	}

	[ConCmd.Server("tw_leave")]
	public static void LeaveGameCmd()
	{
		var caller = ConsoleSystem.Caller.Pawn as Pawn;
		var game = GetJoinedRace( caller.Client.SteamId );
		if ( game == null ) return;
		game.RemovePlayer( caller );
	}

	[ConCmd.Server("tw_join")]
	public static void JoinGameCmd(int raceId, string theme = "default")
	{
		var caller = ConsoleSystem.Caller.Pawn as Pawn;
		var existingGame = GetJoinedRace( caller.Client.SteamId );
		if ( existingGame != null )
		{
			existingGame.RemovePlayer( caller );
		}
		
		var race = GetRace( raceId );
		if ( race == null ) return;
		race.AddPlayer( caller );
		var player = race.GetPlayer( caller.Client.SteamId );
		player.Theme = theme;
	}

	[ConCmd.Server("tw_input")]
	public static void InputGameCmd(string input)
	{
		var caller = ConsoleSystem.Caller;
		var race = GetJoinedRace( caller.SteamId );
		if ( race == null ) return;
		var player = race.GetPlayer( caller.SteamId );
		player.SetInput(input);
	}
	
	[ConCmd.Server("tw_submit")]
	public static void SubmitGameCmd()
	{
		var caller = ConsoleSystem.Caller;
		var race = GetJoinedRace( caller.SteamId );
		if ( race == null ) return;
		var player = race.GetPlayer( caller.SteamId );
		player.SubmitInput();
	}

	[ConCmd.Server("tw_forcestart")]
	public static void ForceStartGameCmd()
	{
		var caller = ConsoleSystem.Caller;
		var race = GetJoinedRace( caller.SteamId );
		if ( race == null ) return;
		if ( race.OwnerId != caller.SteamId ) return;
		race.StartGame();
	}

	public static RaceEntity GetRace( int raceId )
	{
		return FindByIndex( raceId ) as RaceEntity;
	}

	public static RaceEntity GetJoinedRace( long steamId )
	{
		foreach ( var game in All.OfType<RaceEntity>()) 
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


	public Pawn GameOwner; 
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

	public void AddPlayer(Pawn pawn)
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
	
	public void RemovePlayer( Pawn pawn )
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
		Target = new DictionaryFileTextProvider( "text/english_1k.json" ).GetText();
	}
	
}

public enum RaceState
{
	NOT_YET_STARTED,
	COUNTING_DOWN,
	RUNNING,
	ENDED
}
