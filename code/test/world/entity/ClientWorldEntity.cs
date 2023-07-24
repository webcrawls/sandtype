using Sandbox.Component;

namespace Sandtype.Test.World.Entity;

public class ClientWorldEntity
{

	public readonly int EntityId;
	public GameEntity EntityData { get; set; }
	private bool _active;

	public ClientWorldEntity( int entityId )
	{
		EntityId = entityId;
		new Glow();
	}

	public bool IsActive()
	{
		return !_active;
	}

	public virtual void Spawn()
	{
		_active = true;
	}

	public virtual void Delete()
	{
		
	}

	public virtual void Think()
	{
	}

}
