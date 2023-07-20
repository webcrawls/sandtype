using Sandbox;

namespace Sandtype.UI.Game;

public class TerryBotEntity : ProgressiveModeledEntity
{
	
	public TerryBotEntity(Vector3 initial, Vector3 end, SceneWorld world, Model model) : base(initial, end, 0.005f, world, model)
	{
	}

}
