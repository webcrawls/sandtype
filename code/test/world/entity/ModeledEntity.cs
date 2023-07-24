using Sandbox;

namespace Sandtype.Test.World.Entity;

public abstract class ModeledEntity : ClientWorldEntity
{

	public Color Color = Color.Transparent;
	protected SceneWorld World;
	protected Model Model;
	protected SceneModel SceneObject;
	private bool _dead;

	public ModeledEntity( int id, SceneWorld world, Model model ) : base(id)
	{
		World = world;
		Model = model;
	}

	public override void Spawn()
	{
		SceneObject = new SceneModel( World, Model, Transform.Zero );
		SceneObject.Rotation = Rotation.Identity.Backward.EulerAngles.ToRotation();
		_dead = false;
	}

	public override void Delete()
	{
		if ( SceneObject == null ) return;
		
		SceneObject.Delete();
		SceneObject = null;
	}

	// Think(); is defined by WorldEntity, but
	// we can use the 'abstract' keyword to
	// force subclasses to implement it instead :D
	public override void Think()
	{
		SceneObject.ColorTint = Color;
	}
}
