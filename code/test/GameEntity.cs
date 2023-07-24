namespace Sandtype.Test;

/// <summary>
/// Data class for Terry game objects
/// </summary>
public class GameEntity
{

	/// <summary>
	/// The ID number of this entity.
	/// </summary>
	public int EntityId;
	
	/// <summary>
	/// A float between 0 and 1 representing the entity's progress to the end position.
	/// </summary>
	public float Progress { get; set; } = 0;

	/// <summary>
	/// The entity's current health.
	/// </summary>
	public float Health { get; set; } = 1;

	/// <summary>
	/// The entity's max health.
	/// </summary>
	public float MaxHealth { get; set; } = 1;

	/// <summary>
	/// The entity's movement speed (how fast Progress increases)
	/// </summary>
	public float Speed { get; set; } = 0.0025f;

	/// <summary>
	/// The size of the entity model.
	/// </summary>
	public float Scale { get; set; } = 1.0f;

	/// <summary>
	/// The entity's on-screen movement type.
	/// </summary>
	public MovementType MovementType { get; set; } = MovementType.TO_END;

	public ModelType ModelType { get; set; } = ModelType.BULLET;

	/// <summary>
	/// Executes this entity's logic, should be called once per tick.
	/// </summary>

	public GameEntity(int entityId)
	{
		EntityId = entityId;
	}
	
	public void Think()
	{
		Progress += Speed;
	}
}

public enum MovementType
{
	TO_START,
	TO_END
}

public enum ModelType
{
	TERRY,
	BULLET
}
