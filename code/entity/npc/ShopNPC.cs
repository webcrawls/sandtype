using Sandtype.Entity.Pawn.Hud;
using Sandtype.UI.Hud.Pages;

namespace Sandtype.Entity.NPC;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class ShopNPC : AnimatedEntity, IUse
{

	private WorldPanel _namePanel; // clientonly
	private bool _spokenTo = false;
	private ShopHud _hud;

	public override void Spawn()
	{
		base.Spawn();
		Model = Cloud.Model( "jammie.hula_toy" );
		Scale = 10f;
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
		Components.Add( new NametagComponent("Da Shop") );
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
			_hud = new ShopHud();
			user.Components.Get<HudComponent>().Hud.MiddleSection.AddChild( _hud );
		}
		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
