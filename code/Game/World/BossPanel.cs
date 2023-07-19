using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;

namespace Sandtype.Game.UI;

public class BossPanel : Panel
{
	public BossPanel()
	{
		Style.Width = Length.Percent( 100 );
		Style.Height = Length.Percent( 100 );

		var panel = new ScenePanel();
//		var model = Model.Load( "models/citizen/citizen.vmdl" );
		var model = Cloud.Model( "mml.cirno" );
		// init world
		var world = new SceneWorld()
		{
			ClearColor = Color.Black
		};
		
		// init scene panel
		panel = new ScenePanel();
		panel.World = world;
		panel.Camera.FieldOfView = 70;
		panel.Camera.AntiAliasing = true;
		panel.Camera.AmbientLightColor = Color.White;
		panel.Camera.ZFar = 100000f;
		panel.Camera.Position = Vector3.Zero.WithX( 0f );

		panel.Style.Width = Length.Percent( 100 );
		panel.Style.Height = Length.Percent( 100 );
		panel.Style.PointerEvents = PointerEvents.All;
		panel.Style.Cursor = "none";
		
		AddChild( panel );
		
		var _ = new SceneModel( world, model, Transform.Zero with
			{
				Position = Vector3.Zero.WithX( 65 )
					.WithZ( -57 )
					.WithY( -35 ),
				Rotation = Rotation.From( Angles.Zero ).RotateAroundAxis( Vector3.Up, 90f )
			}
		);
	}

	
}
