using System.ComponentModel;
using Sandbox;

namespace TerryTyper;

public partial class Player
{
	
	// An example BuildInput method within a player's Pawn class.
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

	public BBox Hull
	{
		get => new
		(
			new Vector3( -16, -16, 0 ),
			new Vector3( 16, 16, 72 )
		);
	}

	public override Ray AimRay => new Ray( EyePosition, EyeRotation.Forward );
	
	/// <summary>
	/// Create a physics hull for this player. The hull stops physics objects and players passing through
	/// the player. It's basically a big solid box. It also what hits triggers and stuff.
	/// The player doesn't use this hull for its movement size.
	/// </summary>
	public virtual void CreateHull()
	{
		SetupPhysicsFromAABB( PhysicsMotionType.Keyframed, Hull.Mins, Hull.Maxs );

		//var capsule = new Capsule( new Vector3( 0, 0, 16 ), new Vector3( 0, 0, 72 - 16 ), 32 );
		//var phys = SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, capsule );


		//	phys.GetBody(0).RemoveShadowController();

		// TODO - investigate this? if we don't set movetype then the lerp is too much. Can we control lerp amount?
		// if so we should expose that instead, that would be awesome.
		EnableHitboxes = true;
	}
	
	/// <summary>
	/// Pawns get a chance to mess with the input. This is called on the client.
	/// </summary>
	public override void BuildInput()
	{
		base.BuildInput();
		// these are to be done in order and before the simulated components
		Camera?.BuildInput();
		Movement?.BuildInput();
		Animation?.BuildInput();

		foreach ( var i in Components.GetAll<SimulatedComponent>() )
		{
			if ( i.Enabled ) i.BuildInput();
		}
	}
	
		/// <summary>
	/// Called every tick, clientside and serverside.
	/// </summary>
	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );
		// toggleable third person
		if ( Input.Pressed( "View" ) && Game.IsServer )
		{
			if ( Camera is FirstPersonCamera )
			{
				Components.Add( new ThirdPersonCamera() );
			}
			else if ( Camera is ThirdPersonCamera )
			{
				Components.Add( new FirstPersonCamera() );
			}
		}

		if ( Input.Pressed( "Menu" ) && Game.IsClient )
		{
			UIController.Current.ToggleUI();
		}
		//if ( Game.IsClient )
		//{
		//	if ( Input.MouseWheel > 0.1 )
		//	{
		//		Inventory?.SwitchActiveSlot( 1, true );
		//	}
		//	if ( Input.MouseWheel < -0.1 )
		//	{
		//		Inventory?.SwitchActiveSlot( -1, true );
		//	}
		//}
		// these are to be done in order and before the simulated components
		Unstuck?.Simulate( cl );
		Movement?.Simulate( cl );
		Camera?.Simulate( cl );
		Animation?.Simulate( cl );
		SimulateComponents();
		foreach ( var i in Components.GetAll<SimulatedComponent>() )
		{
			if ( i.Enabled ) i.Simulate( cl );
		}
	}

	/// <summary>
	/// Called every frame on the client
	/// </summary>
	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );
		// these are to be done in order and before the simulated components
		Unstuck?.FrameSimulate( cl );
		Movement?.FrameSimulate( cl );
		Camera?.FrameSimulate( cl );
		Animation?.FrameSimulate( cl );
		foreach ( var i in Components.GetAll<SimulatedComponent>() )
		{
			if ( i.Enabled ) i.FrameSimulate( cl );
		}
	}
	TimeSince timeSinceLastFootstep = 0;
	public override void OnAnimEventFootstep( Vector3 position, int foot, float volume )
	{
		if ( LifeState != LifeState.Alive )
			return;

		if ( Game.IsServer )
			return;

		if ( timeSinceLastFootstep < 0.2f )
			return;
		volume *= FootstepVolume();
		var tr = Trace.Ray( position, position + Vector3.Down * 20 ).Radius( 1 ).Ignore( this ).Run();
		if ( !tr.Hit ) return;
		timeSinceLastFootstep = 0;
		tr.Surface.DoFootstep( this, tr, foot, volume * 10 );
	}

	public virtual float FootstepVolume()
	{
		if ( Movement is WalkController wlk )
		{
			if ( wlk.IsDucking ) return 0.3f;
		}
		return 1;
	}
}
