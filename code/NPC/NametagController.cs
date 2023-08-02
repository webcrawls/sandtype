using Sandbox;
using Sandbox.UI;

namespace TerryTyper;

public class NametagController : EntityComponent<AnimatedEntity>
{

	private Panel ChildPanel;
	private WorldPanel _world;
	private Vector3 _position => Entity.Position.WithZ( Entity.Position.z + Entity.WorldSpaceBounds.Size.z );

	public NametagController(Panel child)
	{
		ChildPanel = child;
	}
	
	protected override void OnActivate()
	{
		base.OnActivate();
		_world = new WorldPanel();
		_world.Position = _position;
		_world.AddChild( ChildPanel );
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		_world.Delete();
		_world = null;
	}
	
	[GameEvent.Tick.Client]
	private void OnTick()
	{
		_world.Position = _position;
	}
}
