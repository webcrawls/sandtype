namespace Sandtype.Terry;

public class TerryBot : GameEntity
{
	public TerryBot(int entityId) : base(entityId)
	{
		ModelType = ModelType.TERRY;
		Speed = 0.005f;
		MovementType = MovementType.TO_END;

	}
}
