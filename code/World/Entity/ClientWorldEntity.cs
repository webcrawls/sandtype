using Sandbox.UI;
using Sandtype.Terry;

namespace Sandtype.UI.Game.Entity;

public class ClientWorldEntity
{

	public readonly int EntityId;
	public GameEntity EntityData { get; set; }
	private bool _active;

	public ClientWorldEntity( int entityId )
	{
		EntityId = entityId;
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
