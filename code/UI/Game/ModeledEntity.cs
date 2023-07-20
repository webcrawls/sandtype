using Sandbox;

namespace Sandtype.UI.Game;

public abstract class ModeledEntity : ClientWorldEntity
{

	public Color Color = Color.Transparent;
	protected SceneWorld World;
	protected Model Model;
	protected SceneModel SceneObject;
	private bool _dead;

	public ModeledEntity( SceneWorld world,
		Model model )
	{
		World = world;
		Model = model;
	}

	public bool IsActive()
	{
		return !_dead;
	}

	public void Spawn()
	{
		SceneObject = new SceneModel( World, Model, Transform.Zero );
		_dead = false;
	}

	public void Despawn()
	{
		SceneObject.Delete();
		_dead = true;
	}

	public void Delete()
	{
		if ( SceneObject == null ) return;
		
		SceneObject.Delete();
		SceneObject = null;
	}

	// Think(); is defined by WorldEntity, but
	// we can use the 'abstract' keyword to
	// force subclasses to implement it instead :D
	public virtual void Think()
	{
		SceneObject.ColorTint = Color;
	}
}
