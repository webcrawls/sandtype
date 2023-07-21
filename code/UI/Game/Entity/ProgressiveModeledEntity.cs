using System;
using Sandbox;

namespace Sandtype.UI.Game.Entity;

public class ProgressiveModeledEntity : ModeledEntity
{

	public Vector3 Initial;
	public Vector3 End;
	public float Progress;

	public ProgressiveModeledEntity( int id, Vector3 initial, Vector3 end, SceneWorld world, Model model )
		: base( id, world, model )
	{
		Initial = initial;
		End = end;
		Progress = 0.0f;
	}


	public override void Think()
	{
		if ( SceneObject == null ) return;
		Progress = EntityData.Progress;
		SceneObject.Position = Vector3.Lerp( Initial, End, Progress, false );
		SceneObject.Update( Time.Delta );
	}
}
