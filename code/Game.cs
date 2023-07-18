using Sandbox;

namespace Sandtype;

public partial class SandtypeGame : GameManager
{
	
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
