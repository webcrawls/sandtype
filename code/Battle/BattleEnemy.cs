using Sandbox;

namespace TerryTyper;

public partial class BattleEnemy : AnimatedEntity
{
	[Net] public Vector3 StartPosition {get; set;}
	[Net] public Vector3 EndPosition {get; set;}
	[Net] public float Progress {get; set;}
	[Net] public float Speed {get; set;} = 0.005f;
	[Net] public string Target { get; set; } = "Word";
	[Net] public string Input { get; set; } = "";

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen/citizen.vmdl" );
		Transmit = TransmitType.Always;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
		Tags.Add( "npc" );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		var glow = Components.Create<GlowController>();
		glow.DefaultColor = Color.Red;
		glow.DefaultWidth = 1;
		Components.Add( new NametagController( new TerryEnemyNametag( this ) ) );
	}

	[GameEvent.Tick.Server]
	public void OnTick()
	{
		Progress += 0.0025f;
		Rotation = Rotation.LookAt( EndPosition-StartPosition );
		Position = Vector3.Lerp( StartPosition, EndPosition, Progress );
	}
	
}
