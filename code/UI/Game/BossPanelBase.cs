using System;
using System.Reflection.Metadata.Ecma335;
using Sandbox;
using Sandbox.UI;

namespace Sandtype.UI.Game;

public class BossPanelBase : Panel
{

	// todo mostly ok but still unfuck
	public Color StartColor = Color.White;
	public Color EndColor = Color.Red;
	public int TerryHealth = 10;
	public int TerryMax = 10;
	private SceneModel _boss;
	private ScenePanel _panel;
	
	public BossPanelBase()
	{
		Style.BackgroundColor = Color.Transparent;

		_panel = new ScenePanel();
		var model = Model.Load( "models/citizen/citizen.vmdl" );
		// init world
		var world = new SceneWorld()
		{
			ClearColor = Color.Transparent
		};
		
		// init scene panel
		_panel = new ScenePanel();
		_panel.World = world;
		_panel.Camera.FieldOfView = 70;
		_panel.Camera.AntiAliasing = true;
		_panel.Camera.AmbientLightColor = Color.White;
		_panel.Camera.ZFar = 100000f;
		_panel.Camera.Position = Vector3.Zero;

		_panel.Style.Width = Length.Percent( 100 );
		_panel.Style.Height = Length.Percent( 100 );
		_panel.Style.PointerEvents = PointerEvents.All;
		
		AddChild( _panel );
		
		_boss = new SceneModel( world, model, Transform.Zero with
			{
				Position = Vector3.Zero.WithX( 30 ),
				Rotation = Rotation.Identity.Backward.EulerAngles.ToRotation().RotateAroundAxis( Vector3.Up, -50f )
			}
		); ;
	}

	public void Simulate()
	{
		if ( TerryHealth == 0 )
		{
			// todo
		}
		else
		{
			var color = Color.Lerp( EndColor, StartColor, (float) TerryHealth / TerryMax );
			_boss.ColorTint = color;
		}

		_panel.Camera.Position = _panel.Camera.Position
			.WithZ( 60f )
			.WithX( 0f );

		_boss.Update( Time.Delta / 2 );
	}

	
}
