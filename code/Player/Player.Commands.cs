using Sandbox;

namespace TerryTyper;

public partial class Player
{
	[ConCmd.Admin( "noclip" )]
	static void DoPlayerNoclip()
	{
		if ( ConsoleSystem.Caller.Pawn is Player player )
		{
			if ( player.Movement is NoclipController )
			{
				player.Components.Add( new WalkController() );
			}
			else
			{
				player.Components.Add( new NoclipController() );
			}
		}
	}

	[ConCmd.Admin( "kill" )]
	static void DoPlayerSuicide()
	{
		if ( ConsoleSystem.Caller.Pawn is Player basePlayer )
		{
			basePlayer.TakeDamage( new DamageInfo { Damage = basePlayer.Health * 99 } );
		}
	}
	[ConCmd.Admin( "respawn" )]
	static void DoPlayerRespawn()
	{
		if ( ConsoleSystem.Caller.Pawn is Player basePlayer )
		{
			basePlayer.Respawn();
		}
	}
}
