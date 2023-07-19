using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;
using Sandtype.Game.UI.Entity;
using WorldEntity = Sandtype.Game.UI.Entity.WorldEntity;

namespace Sandtype.Game.UI;

public class GameWorld : Panel
{

	private List<WorldEntity> _entities;
	private SceneWorld World;
	private ScenePanel _scenePanel;
	private Vector3 headPos;
	private Vector3 aimPos;
	private Model citizenModel;
	private Model bulletModel;

	public GameWorld()
	{
		_entities = new List<WorldEntity>();
		Style.Width = Length.Percent( 100 );
		Style.Height = Length.Percent( 100 );
		
		citizenModel = Model.Load( "models/citizen/citizen.vmdl" );
		bulletModel = Model.Load( "models/citizen_props/beachball.vmdl" );

		// init world
		World = new SceneWorld()
		{
			ClearColor = Color.Black
		};
		
		// init scene panel
		_scenePanel = new ScenePanel();
		_scenePanel.World = World;
		_scenePanel.Camera.FieldOfView = 70;
		_scenePanel.Camera.AntiAliasing = true;
		_scenePanel.Camera.AmbientLightColor = Color.White;
		_scenePanel.Camera.ZFar = 100000f;
		_scenePanel.Camera.Position = Vector3.Zero.WithX( 0f );

		_scenePanel.Style.Width = Length.Percent( 100 );
		_scenePanel.Style.Height = Length.Percent( 100 );
		_scenePanel.Style.PointerEvents = PointerEvents.All;
		
		AddChild( _scenePanel );

		new SceneSkyBox(World, Cloud.Material("semxy.bluecloudskybox" ));
	}

	public override void Tick()
	{
		base.Tick();

		foreach ( var entity in _entities )
		{
			entity.Think();
		}

		bool terryReached = false;
		
		// foreach ( var terry in TerryList )
		// {
		// 	terry.Position = terry.Position with { y = terry.Position.y + 0.2f };
		// 	terry.Update( RealTime.Delta );
// 
		// 	if ( terry.Position.y > 100 )
		// 	{
		// 		terryReached = true;
		// 		break;
		// 	}
		// }
		// 
		// foreach ( var bullet in BulletList )
		// {
		// 	if (! bullet.IsValid() )
		// 	{
		// 		continue;
		// 	}
		// 	
		// 	bullet.Position = bullet.Position with { y = bullet.Position.y - 0.2f };
		// 	bullet.Update( RealTime.Delta );
		// 	if ( bullet.Position.y < 0 )
		// 	{
		// 		bullet.Delete();
		// 	}
		// }
// 
		// 
		// if (terryReached)
		// 	Game.TerryHit();
	}
	

	public void CreateTerry()
	{
		var terry = new TerryBotEntity( 1, Color.Transparent, World, citizenModel );
		_entities.Add( terry );
	}

}
