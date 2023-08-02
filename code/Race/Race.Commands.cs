using Sandbox;

namespace TerryTyper;

public partial class Race
{
		[ConCmd.Server("tw_create")]
	public static void CreateGameCmd(string language = "English", int size = 20)
	{
		var caller = ConsoleSystem.Caller.Pawn as Player;
		var existingGame = GetJoinedRace( caller.Client.SteamId );
		if ( existingGame != null )
		{
			return;
		}
		
		var raceEntity = new Race();
		raceEntity.GameOwner = caller;
		raceEntity.Language = language;
		raceEntity.Size = size;
		raceEntity.Spawn();
	}

	[ConCmd.Server("tw_delete")]
	public static void DeleteGameCmd(int raceId)
	{
		var caller = ConsoleSystem.Caller.Pawn as Player;
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
		var caller = ConsoleSystem.Caller.Pawn as Player;
		var game = GetJoinedRace( caller.Client.SteamId );
		if ( game == null ) return;
		game.RemovePlayer( caller );
	}

	[ConCmd.Server("tw_join")]
	public static void JoinGameCmd(int raceId, string theme = "default")
	{
		var caller = ConsoleSystem.Caller.Pawn as Player;
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

	[ConCmd.Client("tw_freecoins")]
	public static void FreeCoinsGameCmd()
	{
		DataController.Current.Currency = 1000;
	}

	[ConCmd.Server("tw_theme")]
	public static void ThemeGameCmd(string theme)
	{
		var caller = ConsoleSystem.Caller;
		var race = GetJoinedRace( caller.SteamId );
		if ( race == null ) return;
		var player = race.GetPlayer( caller.SteamId );
		player.Theme = theme;
	}
}
