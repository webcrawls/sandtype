using System;
using Sandbox;
using Sandbox.UI;
using Sandtype.Engine;
using Sandtype.UI;

namespace Sandtype;

public partial class Pawn : AnimatedEntity
{
	
	public Hud Hud => Game.RootPanel as Hud;
	public TypingGame TypingGame;
	
	public override void ClientSpawn()
	{
		// we only want these to happen on the client (for now)
		// there is no server involvement in the typing game
		InitializeGame();
		InitializeHud();
	}
	
	private void InitializeHud()
	{
		// only run this code on our client (why do we need to do this?)
		if ( Client != Game.LocalClient ) return;
		Game.RootPanel = new Hud(TypingGame);
		
		// handle input from onchange, emitted by the TextEntry in Hud
		// can we use a method reference here like in Java?
		Hud.AddEventListener("onchange", (v) => {
			Log.Info( v.Target );
			if ( v.Target is TextEntry entry )
			{
				TypingGame.HandleInput( entry.Text );
			}
		});
	}

	private void InitializeGame()
	{
		// reset game on init
		TypingGame = new TypingGame( this );
		TypingGame.Reset();
	}
	
}
