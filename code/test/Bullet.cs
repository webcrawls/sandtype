namespace Sandtype.Test;

public class Bullet : GameEntity
{
	public Bullet(int entityId) : base(entityId)
	{
		ModelType = ModelType.BULLET;
		Speed = 0.005f;
		MovementType = MovementType.TO_START;
	}
}
