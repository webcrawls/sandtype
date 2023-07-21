using Sandtype.UI.Game.Entity;
using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;
using Sandtype.Terry;

namespace Sandtype.UI.Game;

public class BetterGameWorld : Panel
{

	private static Model _citizenModel = Model.Load( "models/citizen/citizen.vmdl" );

	/// <summary>
	/// The TerryGame instance. Must be set by external game logic.
	/// </summary>
	public TerryGame Game;
	
	private List<ClientWorldEntity> _entities;
	private ScenePanel _scenePanel;
	private SceneWorld _sceneWorld;

	private Vector3 _start = Vector3.Zero
		.WithX( -80 )
		.WithY( 150f )
		.WithZ( 40 );

	private Vector3 _end = Vector3.Zero
		.WithX( 220 )
		.WithY( 150f )
		.WithZ( 40 );

	public BetterGameWorld()
	{
		_entities = new List<ClientWorldEntity>();
		InitializeWorld();
	}

	public override void Tick()
	{
		if ( Game == null )
		{
			return;
		}

		TickEntities();

		base.Tick();
	}

	private void InitializeWorld()
	{
		_sceneWorld = new SceneWorld() { ClearColor = Color.Black };

		_scenePanel = new ScenePanel();
		_scenePanel.World = _sceneWorld;

		_scenePanel.Camera.FieldOfView = 70;
		_scenePanel.Camera.AntiAliasing = true;
		_scenePanel.Camera.AmbientLightColor = Color.White;
		_scenePanel.Camera.Ortho = true; 
		_scenePanel.Camera.OrthoWidth = 350f;
		_scenePanel.Camera.OrthoHeight = 100f;
		_scenePanel.Camera.ZFar = 100000f;
		_scenePanel.Camera.Rotation = Vector3.Left.EulerAngles.ToRotation();
		_scenePanel.Camera.Position = Vector3.Zero
			.WithX( 60 )
			.WithZ( 80 );

		_scenePanel.Style.Width = Length.Percent( 100 );
		_scenePanel.Style.Height = Length.Percent( 100 );
		_scenePanel.Style.PointerEvents = PointerEvents.All;

		AddChild( _scenePanel );
		new SceneLight( _sceneWorld );
	}

	private ClientWorldEntity? GetEntity( int id )
	{
		foreach ( var entity in _entities )
		{
			if ( entity.EntityId == id )
			{
				return entity;
			}
		}

		return null;
	}

	private void TickEntities()
	{
		// Create any WorldEntities that are missing from the world.
		for ( int i = 0; i < Game.Entities.Count; i++ )
		{
			var gameEntity = i < Game.Entities.Count ? Game.Entities[i] : null;
			var worldEntity = gameEntity != null ? GetEntity( gameEntity.EntityId ) : null;

			if ( gameEntity != null && worldEntity == null )
			{
				var start = gameEntity.MovementType == MovementType.TO_START ? _start : _end;
				var end = gameEntity.MovementType == MovementType.TO_START ? _end : _start;
				var entity = new ProgressiveModeledEntity( gameEntity.EntityId, start, end, _sceneWorld, _citizenModel );
				
				entity.Spawn();
				entity.EntityData = gameEntity;
				
				_entities.Add( entity );
			}
		}

		for ( int i = 0; i < _entities.Count; i++ )
		{
			var worldEntity = _entities[i];
			if ( worldEntity == null ) continue;

			var gameEntity = Game.GetEntity( worldEntity.EntityId );
			if ( gameEntity == null )
			{
				worldEntity.Delete();
				_entities.Remove( worldEntity );
			}
		}

		foreach ( var entity in _entities )
		{
			entity.Think();
		}
	}
}
