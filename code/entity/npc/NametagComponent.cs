using Sandbox.Services;
using Sandbox.UI;

namespace Sandtype.Entity.NPC;
using Sandbox;

public class NametagComponent : EntityComponent<Entity>, ISingletonComponent
{

	public string Name { get; set; } = "unnamed";
	private WorldPanel _panel;

	public NametagComponent() { }

	public NametagComponent( string name )
	{
		Name = name;
	}
 
	protected override void OnActivate()
	{
		base.OnActivate();
		_panel = new NametagPanel( Name );
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		_panel = new NametagPanel( Name );
	}

	[GameEvent.Tick.Client]
	public void Tick()
	{
		var z = Entity.Position.z + Entity.WorldSpaceBounds.Size.z;
		var y = Entity.Position.y - (Entity.WorldSpaceBounds.Size.y / 2);
		_panel.Position = Entity.Position.WithZ( z ).WithY( y );
		_panel.Rotation = Rotation.Identity;
	}
}
