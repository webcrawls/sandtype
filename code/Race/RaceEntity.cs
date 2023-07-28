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
		raceEntity.Owner = caller;
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

	public Pawn Owner;

	public List<RacePlayer> Players => Components.GetAll<RacePlayer>().ToList();

	public int RaceId => NetworkIdent;
	
	[Net] public long OwnerId { get; set; }
	[Net] public IList<long> QuitPlayers { get; set; }
	[Net] public IList<long> Winners { get; set; }
	[Net] public IList<string> Target { get; set; }
	[Net] public string OwnerName { get; set; }
	[Net] public bool Started { get; set; }
	
	public override void Spawn()
	{
		base.Spawn();
		Transmit = TransmitType.Always;
		OwnerName = Owner?.Client.Name ?? "Unknown";
		OwnerId = Owner?.Client.SteamId ?? 0;
		Name = OwnerName + "'s Race";
		QuitPlayers = new List<long>();
		Winners = new List<long>();
		Target = new DictionaryFileTextProvider( "text/english_1k.json" ).GetText();
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
		cmp.Name = pawn.Client.Name;
		pawn.ShowRaceHud(To.Single( pawn ));
	}
	
	public void RemovePlayer( Pawn pawn )
	{
		var player = GetPlayer( pawn.Client.SteamId );
		if ( player == null ) return;
		Components.Remove( player );
		pawn.HideRaceHud(To.Single( pawn ));
	}
}
