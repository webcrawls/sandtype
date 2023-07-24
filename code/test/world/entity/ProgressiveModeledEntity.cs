namespace Sandtype.Test.World.Entity;
using System;
using System.Collections.Generic;
using Sandbox;


public class ProgressiveModeledEntity : ModeledEntity
{

	private static List<string> Animations = new()
	{
		"IdlePose_07_R",
		"IdlePose_07_L",
		"Dab",
		"CrouchWalk_Default_1D",
		"Land",
		"LocomotionLean_UpperBody_N",
		"LocomotionLean_UpperBody_E",
		"LocomotionLean_UpperBody_S",
		"LocomotionLean_UpperBody_W",
	};

	public Vector3 Initial;
	public Vector3 End;
	public float Progress;
	private string Animation = Animations[Random.Shared.Next(0, Animations.Count)];

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
		if ( SceneObject.DirectPlayback.Name != Animation )
		{
			SceneObject.DirectPlayback.Play( Animation );
		}
		Progress = EntityData.Progress;
		SceneObject.Transform = SceneObject.Transform.WithScale(EntityData.Scale);
		SceneObject.Position = Vector3.Lerp( Initial, End, Progress, false );
		SceneObject.Position = SceneObject.Position.WithZ((float) (SceneObject.Position.z +
		                                                  ((float)Math.Sin( Time.Tick / 3 ) * 1.5)));
		SceneObject.Update( Time.Delta );
	}
}
