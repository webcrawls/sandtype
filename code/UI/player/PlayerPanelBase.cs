using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Timers;
using Sandbox;
using Sandbox.UI;
using Sandtype.Entity.Pawn;

namespace Sandtype.UI.Player;

public class PlayerPanelBase : Panel
{

	// todo mostly ok but still unfuck
	public Color StartColor = Color.White;
	public Color EndColor = Color.Red;
	public int TerryHealth = 10;
	public int TerryMax = 10;

	public bool DressFromClient = false;
	public Vector3 MovementFrom = Vector3.Zero.WithX( 30 );
	public Vector3 MovementTo = Vector3.Zero.WithX( 100 );

	private IList<KeyValuePair<Vector3, Vector3>> _list = new List<KeyValuePair<Vector3, Vector3>>()
	{
		new(Vector3.Zero.WithX( 30 ), Vector3.Zero.WithX( 100 )),
		new(Vector3.Zero.WithX( 30 ).WithY( -25 ), Vector3.Zero.WithX( 30 ).WithY( 25 )),
		new(Vector3.Zero.WithX( 30 ).WithY( 25 ), Vector3.Zero.WithY( 25 )),
	};

	private int _movementIndex = 0;
	private int _movementMax = 1000;
	private int _movementTick = 0;

	private SceneModel _boss;
	private ScenePanel _panel;
	
	public PlayerPanelBase()
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
				Position = MovementFrom,
				Rotation = Rotation.Identity.Forward.EulerAngles.ToRotation().RotateAroundAxis( Vector3.Up, -120f )
			}
		);

		if ( true )
		{
			var c = new ClothingContainer();
			c.Toggle(  ResourceLibrary.Get<Clothing>(1846461341) );
			var models = c.DressSceneObject( _boss );
			Log.Info( "models: " + models.Count );
		}
	}

	public override void Tick()
	{
		base.Tick();
		Simulate();
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

//		if ( MovementMode == MovementMode.FROM_TO )
//		{
//			if ( _movementTick >= _movementMax )
//			{
//				_movementTick = 0;
//				_movementIndex += 1;
//				if ( _movementIndex >= _list.Count )
//				{
//					_movementIndex = 0;
//				}
//			}
//			else
//			{
//				_movementTick += 1;
//			}
//			
//			var progress = (float) _movementTick / _movementMax;
//			_boss.Position = Vector3.Lerp( _list[_movementIndex].Key, _list[_movementIndex].Value, EaseIn(progress) );
//		}
//		
//		_boss.Update( Time.Delta / 2 );
	}

	private float EaseIn( float f )
	{
		return f * f;
	}
	
}
