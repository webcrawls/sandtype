using System;
using Sandbox;

namespace Sandtype.UI.Game;

public class ProgressiveModeledEntity : ModeledEntity
{

	public Vector3 Initial;
	public Vector3 End;
	public float Progress;
	public Action ReachedEndAction;
	public float Speed;

	public ProgressiveModeledEntity( Vector3 initial, Vector3 end, float progressSpeed, SceneWorld world, Model model )
		: base( world, model )
	{
		Initial = initial;
		End = end;
		Speed = progressSpeed;
		Progress = 0.0f;
	}


	public override void Think()
	{
		if ( !IsActive() ) return;
		if ( SceneObject == null ) return;
		Progress += Speed;
		SceneObject.Position = Vector3.Lerp( Initial, End, Progress, false );
		SceneObject.Update( Time.Delta );
		if ( Progress >= 1 )
		{
			Despawn();
			ReachedEndAction?.Invoke();
		}
	}
}
