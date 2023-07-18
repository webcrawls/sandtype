using Sandbox;
using Sandbox.UI;
using Sandtype.Engine;
using Sandtype.UI;

namespace Sandtype;

public partial class Pawn : AnimatedEntity
{
	
	public Hud Hud => Game.RootPanel as Hud;
	public TypingGame TypingGame;
	public ClientSettings Settings;
	
	public override void ClientSpawn()
	{
		// we only want these to happen on the client (for now)
		// there is no server involvement in the typing game
		InitializeSettings();
		InitializeGame();
		InitializeHud();
	}

	public override void Simulate( IClient cl )
	{
		if ( Client != cl )
		{
			return;
		}

		// focus text input if it's not already focused
		if ( Game.IsClient && Hud.Entry != null && !Hud.Entry.Label.HasFocus )
		{
			Hud.Entry.Label.Focus();
		}

		if ( Game.IsClient && (!TypingGame.Started || TypingGame.Completed) && Hud.Entry != null )
		{
			Hud.Entry.Label.Text = "";
		}

		base.Simulate( cl );
	}

	public void InitializeSettings()
	{
		Settings = new ClientSettings( 42 );
		Log.Info( "settings: "+Settings );
	}

	public void SaveSettings( ClientSettings settings )
	{
		Settings = settings;
		ClientSettings.Save( settings );
	}

	private void InitializeHud()
	{
		// only run this code on our client (why do we need to do this?)
		if ( Client != Game.LocalClient ) return;
		Game.RootPanel = new Hud(TypingGame);
		Hud.AddEventListener( "onchange", HandleInputChange );
	}
	
	private void InitializeGame()
	{
		// reset game on init
		TypingGame = new TypingGame( this );
		TypingGame.Reset();
	}
	
	public void HandleInputChange( PanelEvent e )
	{
		if ( e.Target is TextEntry entry )
		{
			TypingGame.HandleInput( entry.Text );
		}
	}

}
