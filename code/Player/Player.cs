using Sandbox;

namespace TerryTyper;

/// <summary>
/// In the darkest hour,
/// Garry's words bring truth and light
/// He will save us all
///
/// https://github.com/orgs/sboxgame/discussions/975
/// </summary>
public partial class Player : AnimatedEntity
{
	
	public GlowController Glow => Components.Get<GlowController>();
	public UseController Use => Components.Get<UseController>();
	public MovementComponent Movement => Components.Get<MovementComponent>();
	public CameraComponent Camera => Components.Get<CameraComponent>();
	public AnimationComponent Animation => Components.Get<AnimationComponent>();
	public UnstuckComponent Unstuck => Components.Get<UnstuckComponent>();

	public void Respawn()
	{
		Spawn();
	}
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen/citizen.vmdl" );
		Velocity = Vector3.Zero;
		
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		
		CreateHull();
		Tags.Add( "player" );

		Components.Add( new WalkController() );
		Components.Add( new FirstPersonCamera() );
		Components.Add( new UnstuckComponent() );
		Components.Add( new CitizenAnimationComponent() );
		Components.Add( new UseController() );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		Components.Create<GlowController>();
		Components.Create<UseController>();
	}
	
	[GameEvent.Tick.Client]
	public void OnClientTick()
	{
	}

	private void SimulateComponents()
	{
		foreach ( var i in Components.GetAll<SimulatedComponent>() )
		{
			if ( i.Enabled ) i.BuildInput();
		}
	}

}
