using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace TerryTyper;

/// <summary>
/// The core battle entity.
/// Displays itself as a Terry when it spawns, allows players to join and start a battle via interacting.
/// </summary>
public partial class Battle : AnimatedEntity
{
	
	/// <summary>
	/// Checks if a player with a given steam64 is in a battle.
	/// </summary>
	/// <param name="steamId">the player's steam64</param>
	/// <returns>true or false</returns>
	public static bool IsInBattle( long steamId )
	{
		return GetJoinedBattle( steamId ) != null;
	}

	/// <summary>
	/// Returns the battle a given player is in.
	/// </summary>
	/// <param name="steamId">The player's steam64.</param>
	/// <returns>The battle.</returns>
	public static Battle GetJoinedBattle( long steamId )
	{
		foreach ( var game in All.OfType<Battle>()) 
			if ( game.Players.Count( p => p.SteamId == steamId ) != 0 )
				return game;

		return null;
	}

	/// <summary>
	/// The default max boss health.
	/// </summary>
	public const int BOSS_HEALTH = 50;
	
	/// <summary>
	/// The default entity spawn time.
	/// </summary>
	public const int SPAWN_TIME = 60;

	/// <summary>
	/// The list of players in this battle.
	/// </summary>
	[Net] public IList<BattlePlayer> Players { get; set; } = new List<BattlePlayer>();
	
	/// <summary>
	/// The list of active enemies in this battle.
	/// </summary>
	[Net] public IList<BattleEnemy> Enemies { get; set; } = new List<BattleEnemy>();
	
	/// <summary>
	/// The current battle speed multiplier.
	/// </summary>
	[Net] public float SpeedMultiplier { get; set; } = 1.0f;
	
	/// <summary>
	/// The boss's max health.
	/// </summary>
	[Net] public int MaxBossHealth { get; set; } = BOSS_HEALTH;
	
	/// <summary>
	/// The boss's current health.
	/// </summary>
	[Net] public int BossHealth { get; set; } = BOSS_HEALTH;
	
	/// <summary>
	/// The enemy that's progressed the furthest.
	/// </summary>
	[Net] public BattleEnemy ClosestEnemy { get; set; } = null;
	
	/// <summary>
	/// The current list of target input strings.
	/// </summary>
	[Net] public IList<string> TargetList { get; set; } = new List<string>();

	private int _spawnCountdown = SPAWN_TIME;
	
	/// <summary>
	/// Returns the BattlePlayer based on a steam64.
	/// </summary>
	/// <param name="steamId">the steam64.</param>
	/// <returns>the BattlePlayer</returns>
	public BattlePlayer GetPlayer( long steamId )
	{
		return Players.FirstOrDefault( p => p.SteamId == steamId );
	}

	public override void Spawn()
	{
		base.Spawn();
		Transmit = TransmitType.Always;
		SetModel( "models/citizen/citizen.vmdl" );
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		Tags.Add( "npc" );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		Components.Create<GlowController>();
		Components.Add( new NametagController( new TerryBossNametag() ) );
	}
	
	/// <summary>
	/// Adds a player to the battle. This will add a BattlePlayer component to this Battle.
	/// </summary>
	/// <param name="player"></param>
	public void AddPlayer( Player player )
	{
		var players = new List<BattlePlayer>( Players );
		var bPlayer = new BattlePlayer();
		bPlayer.SteamId = player.Client.SteamId;
		Components.Add( bPlayer );
		players.Add( bPlayer );
		Players = players;
		player.ShowBattleHud( To.Single( player ), NetworkIdent );
	}

	/// <summary>
	/// Removes a player from this battle.
	/// </summary>
	/// <param name="player">the battle</param>
	public void RemovePlayer( Player player )
	{
		var existing = GetPlayer( player.Client.SteamId );
		var players = new List<BattlePlayer>( Players );
		player.HideBattleHud( To.Single( player ) );
		existing.Remove();
		players.Remove( existing );
		Players = players;
	}
	
	/// <summary>
	/// Submits a player's input. Does nothing if they're not in the game.
	/// </summary>
	/// <param name="steamId">the player's steam64</param>
	/// <param name="input">the player's input</param>
	public void Submit( long steamId, string input )
	{
		var firstWord = TargetList.Count == 0 ? null : TargetList[0];
		if ( firstWord == null ) return;
		var words = new List<string>( TargetList );
		words.RemoveAt( 0 );
		TargetList = words;
		KillEnemy( 0 );
	}
	
}
