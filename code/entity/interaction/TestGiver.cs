using Sandtype.Test;

namespace Sandtype.Entity.Interaction;
using Sandbox;

public class TestGiver : InteractableComponent
{
	
	protected override void OnInteract( IClient cl )
	{
		cl.Pawn.Components.Add( new TypingTestComponent() );
	}
}
