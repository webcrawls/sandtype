using Sandbox;
using Sandtype.UI;
using Sandtype.UI.Game;

namespace Sandtype;

public partial class Pawn : AnimatedEntity
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
	}

	public override void Simulate( IClient cl )
	{
		if ( Client != cl )
		{
			return;
		}

		if ( Test == null )
		{
			Test = Components.Create<TypingTest>();
		}

		SimulateGame();

		base.Simulate( cl );
	}

	private void SimulateGame()
	{
		// "Simulate"/"Think" our client-side components
		if ( Sandbox.Game.IsServer ) return;
		Game?.Simulate();
		Test?.Simulate();
		Hud.Boss.Simulate();
	}
	
	
}
