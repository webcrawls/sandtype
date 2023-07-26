using Sandtype.Entity.Pawn.Hud;
using Sandtype.UI.Hud.Pages;

namespace Sandtype.Entity.NPC;
using Sandbox;
using Sandbox.UI;

public class InfoNPC : AnimatedEntity, IUse
{

	private WorldPanel _namePanel; // clientonly
	private bool _spokenTo = false;
	private InfoHud _hud;

	public override void Spawn()
	{
		base.Spawn();
		Model = Cloud.Model( "goodolepink.question_block" );
		Scale = 2.0f;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		EnableSolidCollisions = false;
		Tags.Add( "npc" );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		Components.Add( new NametagComponent("info!") );
	}

	[GameEvent.Tick.Server]
	private void OnTick()
	{
		Rotation = Rotation.RotateAroundAxis( Vector3.Up, 1f );
	}

	public bool OnUse( Entity user )
	{
		if ( Game.IsClient && (_hud == null || !_hud.IsValid) )
		{
			_hud = new InfoHud();
			user.Components.Get<HudComponent>().Hud.MiddleSection.AddChild( _hud );
		}
		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
