using Sandbox;

namespace TerryTyper;

public partial class Battle
{
	
	[ConCmd.Server("tw_battlestart")]
	public static void CreateCmd()
	{
		var pawn = ConsoleSystem.Caller.Pawn as Player;
		var battle = new Battle();
		battle.Position = pawn.Position;
		battle.Spawn();
		battle.AddPlayer( pawn );
	}

	[ConCmd.Server("tw_battleleave")]
	public static void LeaveCmd()
	{
		var pawn = ConsoleSystem.Caller.Pawn as Player;
		var existing = GetJoinedBattle( pawn.Client.SteamId );
		if ( existing == null ) return;
		existing.RemovePlayer( pawn );
	}

	
	[ConCmd.Server("tw_battlesubmit")]
	public static void SubmitCmd(string input)
	{
		var pawn = ConsoleSystem.Caller.Pawn as Player;
		var existing = GetJoinedBattle( pawn.Client.SteamId );
		existing.Submit( pawn.Client.SteamId, input );
	}

}
