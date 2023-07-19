using Sandbox;

namespace Sandtype.Game.UI.Entity;

public class TerryBotEntity : ModeledEntity
{

	public float Scale;
	public Color Color;

	public TerryBotEntity(float scale, Color color, SceneWorld world, Model model) : base(world, model)
	{
		Scale = scale;
		Color = color;
	}

	public override void Think()
	{
		SceneObject.Position = SceneObject.Position with { y = SceneObject.Position.y + 0.2f };
		// todo 'SceneModel#Update(delta)`?
	}
}
