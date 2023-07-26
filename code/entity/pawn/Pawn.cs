using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sandbox;
using Sandbox.Services;
using Sandtype.Entity.Pawn.data;
using Sandtype.Entity.Pawn.Hud;
using Sandtype.Entity.Pawn.Select;
using Sandtype.Test;
using Sandtype.UI.Theme;

namespace Sandtype.Entity.Pawn;

public partial class Pawn : AnimatedEntity
{

	[ClientInput] public Vector3 InputDirection { get; set; }

	[ClientInput] public Angles ViewAngles { get; set; }

	/// <summary>
	/// Position a player should be looking from in world space.
	/// </summary>
	[Browsable( false )]
	public Vector3 EyePosition
	{
		get => Transform.PointToWorld( EyeLocalPosition );
		set => EyeLocalPosition = Transform.PointToLocal( value );
	}

	/// <summary>
	/// Position a player should be looking from in local to the entity coordinates.
	/// </summary>
	[Net, Predicted, Browsable( false )]
	public Vector3 EyeLocalPosition { get; set; }

	/// <summary>
	/// Rotation of the entity's "eyes", i.e. rotation for the camera when this entity is used as the view entity.
	/// </summary>
	[Browsable( false )]
	public Rotation EyeRotation
	{
		get => Transform.RotationToWorld( EyeLocalRotation );
		set => EyeLocalRotation = Transform.RotationToLocal( value );
	}

	/// <summary>
	/// Rotation of the entity's "eyes", i.e. rotation for the camera when this entity is used as the view entity. In local to the entity coordinates.
	/// </summary>
	[Net, Predicted, Browsable( false )]
	public Rotation EyeLocalRotation { get; set; }

	public Theme Theme = Theme.Themes["default"];

	public BBox Hull
	{
		get => new
		(
			new Vector3( -16, -16, 0 ),
			new Vector3( 16, 16, 64 )
		);
	}

	[BindComponent] public PawnController Controller { get; }
	[BindComponent] public PawnAnimator Animator { get; }
	[BindComponent] public ClientRaceManager RaceServer { get; }
	
	public override Ray AimRay => new Ray( EyePosition, EyeRotation.Forward );

	public PawnData PawnData
	{
		get { return _pawnData; }
		set
		{
			_pawnData = value;
			PawnData.SaveData( Client?.SteamId ?? 0l, _pawnData );
		}
	}

	private PawnData _pawnData;
	
	/// <summary>
	/// Called when the entity is first created 
	/// </summary>
	public override void Spawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		_tickables = new List<ITickable>();

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		_pawnData = new PawnData();

		Components.Create<PawnSelectComponent>();
		
		Tags.Add( "npc" );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		_pawnData = PawnData.LoadData( Client.SteamId );
		Components.Create<PawnSelectGlowComponent>();
		Components.Create<PawnSelectComponent>();
		Components.Create<HudComponent>();
	}
	
	public void Respawn()
	{
		Components.Create<PawnController>();
		Components.Create<PawnAnimator>();
		Components.Create<ClientRaceManager>();
	}

	public void DressFromClient( IClient cl )
	{
		var c = new ClothingContainer();
		c.LoadFromClient( cl );
		c.DressEntity( this );
	}

	public override void Simulate( IClient cl )
	{
		SimulateRotation();
		Controller?.Simulate( cl );
		Animator?.Simulate();
		TickTickables();
		EyeLocalPosition = Vector3.Up * (64f * Scale);
	}

	public override void BuildInput()
	{
		InputDirection = Input.AnalogMove;

		if ( Input.StopProcessing )
			return;

		var look = Input.AnalogLook;

		if ( ViewAngles.pitch > 90f || ViewAngles.pitch < -90f )
		{
			look = look.WithYaw( look.yaw * -1f );
		}

		var viewAngles = ViewAngles;
		viewAngles += look;
		viewAngles.pitch = viewAngles.pitch.Clamp( -89f, 89f );
		viewAngles.roll = 0f;
		ViewAngles = viewAngles.Normal;
	}

	bool IsThirdPerson { get; set; } = false;

	public override void FrameSimulate( IClient cl )
	{
		SimulateRotation();

		Camera.Rotation = ViewAngles.ToRotation();
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );

		if ( Input.Pressed( "view" ) )
		{
			IsThirdPerson = !IsThirdPerson;
		}

		if ( IsThirdPerson )
		{
			Vector3 targetPos;
			var pos = Position + Vector3.Up * 64;
			var rot = Camera.Rotation * Rotation.FromAxis( Vector3.Up, -16 );

			float distance = 80.0f * Scale;
			targetPos = pos + rot.Right * ((CollisionBounds.Mins.x + 50) * Scale);
			targetPos += rot.Forward * -distance;

			var tr = Trace.Ray( pos, targetPos )
				.WithAnyTags( "solid" )
				.Ignore( this )
				.Radius( 8 )
				.Run();
			
			Camera.FirstPersonViewer = null;
			Camera.Position = tr.EndPosition;
		}
		else
		{
			Camera.FirstPersonViewer = this;
			Camera.Position = EyePosition;
		}
	}

	public TraceResult TraceBBox( Vector3 start, Vector3 end, float liftFeet = 0.0f )
	{
		return TraceBBox( start, end, Hull.Mins, Hull.Maxs, liftFeet );
	}

	public TraceResult TraceBBox( Vector3 start, Vector3 end, Vector3 mins, Vector3 maxs, float liftFeet = 0.0f )
	{
		if ( liftFeet > 0 )
		{
			start += Vector3.Up * liftFeet;
			maxs = maxs.WithZ( maxs.z - liftFeet );
		}

		var tr = Trace.Ray( start, end )
					.Size( mins, maxs )
					.WithAnyTags( "solid", "playerclip", "passbullets" )
					.Ignore( this )
					.Run();

		return tr;
	}

	protected void SimulateRotation()
	{
		EyeRotation = ViewAngles.ToRotation();
		Rotation = ViewAngles.WithPitch( 0f ).ToRotation();
	}
	
}
