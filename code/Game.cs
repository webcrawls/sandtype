using Sandbox;
using Sandtype.UI;

namespace Sandtype;

public class SandtypeGame : GameManager
{
	public SandtypeGame()
	{
		if ( Game.IsServer ) return;
		Game.RootPanel = new Main();
	}
	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );
	}
	

}
