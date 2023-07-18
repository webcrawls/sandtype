using System;
using System.Collections.Generic;
using Sandbox;
using Sandtype.Engine.Text;
using Sandtype.UI.Typing;

namespace Sandtype;

public partial class SandtypeGame : GameManager
{
	
	public static SandtypeGame Manager => SandtypeGame.Current as SandtypeGame;
	private TextProvider _provider = new DictionaryTextProvider(new List<string>() {"hello", "world"});

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		var steamId = client.SteamId;

		Pawn pawn = new Pawn();
		client.Pawn = pawn;
	}

	public override void Simulate( IClient cl )
	{
		if ( cl.Pawn is Pawn p )
			p.Simulate( cl );
	}
}
