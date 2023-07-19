using Sandbox;
using Sandtype.Game.Component;

namespace Sandtype;

public partial class Pawn : AnimatedEntity
{
	
	public override void ClientSpawn()
	{
		// we only want these to happen on the client (for now)
		// there is no server involvement in the typing game

		Components.Create<TerryGameComponent>();
		Components.Create<TypingTestComponent>();
	}

	public override void Simulate( IClient cl )
	{
		if ( Client != cl )
		{
			return;
		}
		
		base.Simulate( cl );
	}
	
	
}
