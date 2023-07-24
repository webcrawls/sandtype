using Sandtype.Entity.Pawn.Test;

namespace Sandtype.Entity.Interaction;
using Sandbox;

public class TestGiver : InteractableComponent
{
	
	protected override void OnInteract( IClient cl )
	{
		Log.Info( "Test: "+cl );
		cl.Pawn.Components.Add( new TypingTestComponent() );
	}
}
