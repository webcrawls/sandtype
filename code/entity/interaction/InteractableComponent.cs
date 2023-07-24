namespace Sandtype.Entity.Interaction;
using Sandbox;

public class InteractableComponent : EntityComponent<Entity>
{

	private float _lastInteractTime = 0;

	public void HandleInteract( IClient cl )
	{
		var now = Time.Now;
		var delta = now - _lastInteractTime;
		Log.Info( delta );
		if ( delta <= 0.5 ) return;
		_lastInteractTime = Time.Now;
		OnInteract( cl );
	}

	protected virtual void OnInteract( IClient cl )
	{
		
	}

}
