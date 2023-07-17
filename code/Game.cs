using Sandbox;
using System;
using Sandbox.UI;
using Sandtype.UI;

namespace Sandtype;

public class SandtypeGame : GameManager
{
	public String TypeTarget = "The quick brown fox jumps over the lazy dog.";

	private String _content = "";

	public bool IsComplete;
	public String TypeContent
	{
		get => _content;
		set
		{
			if ( _content == TypeTarget )
			{
				Complete();
			}

			_content = value;
		}
	}

	public SandtypeGame()
	{
		if ( Game.IsServer ) return;
		var panel = new Main();
		panel.Game = this;
		Game.RootPanel = panel;
	}
	
	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );
	}

	public void ProcessInput()
	{
		
	}

	public void Reset()
	{
		TypeContent = "";
	}

	public void Complete()
	{
		IsComplete = true;
		Log.Info( "You completed!" );
	}

}
