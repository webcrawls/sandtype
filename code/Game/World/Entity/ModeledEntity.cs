using Sandbox;

namespace Sandtype.Game.UI.Entity;

public abstract class ModeledEntity : WorldEntity
{

	protected SceneWorld World;
	protected Model Model;
	protected SceneObject SceneObject;

	public ModeledEntity( SceneWorld world,
		Model model )
	{
		World = world;
		Model = model;
	}

	public virtual void Spawn()
	{
		SceneObject = new SceneModel( World, Model, Transform.Zero );
	}

	public virtual void Delete()
	{
		if ( SceneObject == null ) return;
		
		SceneObject.Delete();
		SceneObject = null;
	}

	// Think(); is defined by WorldEntity, but
	// we can use the 'abstract' keyword to
	// force subclasses to implement it instead :D
	public abstract void Think();
}
