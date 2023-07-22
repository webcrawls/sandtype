using System.Runtime.InteropServices.ComTypes;
using Sandbox;
using Sandtype.UI;

namespace Sandtype;

public class Pawn : AnimatedEntity
{

	public Hud Hud;
	public TerryGame Game;
	public TypingTest Test;

	public Pawn()
	{
		if ( Sandbox.Game.IsClient )
		{
			Hud = new Hud();
			Sandbox.Game.RootPanel = Hud;
		}
	}
	
	public override void ClientSpawn()
	{
		// we only want these to happen on the client (for now)
		// there is no server involvement in the typing game

		Game = Components.Create<TerryGame>();
		Test = Components.Create<TypingTest>();
		Test.ResetTest();
	}

	public override void Simulate( IClient cl )
	{
		if ( Client != cl )
		{
			return;
		}

		SimulateGame();

		base.Simulate( cl );
	}

	private void SimulateGame()
	{
		// "Simulate"/"Think" our client-side components
		if ( Sandbox.Game.IsServer ) return;
		if ( Test == null ) return;
		if ( !Test.Initialized )
		{
			Test.ResetTest();
		}

		if ( Hud.Test == null )
		{
			Hud.Test = Test;
		}
		
		Game?.Simulate();
	}
	
	
}
