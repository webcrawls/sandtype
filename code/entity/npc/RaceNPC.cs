using Sandtype.Entity.Pawn.Hud;
using Sandtype.UI.Hud.Pages;

namespace Sandtype.Entity.NPC;
using Sandbox;
using Sandbox.UI;

public class RaceNPC : AnimatedEntity, IUse
{

	private WorldPanel _namePanel; // clientonly
	private bool _spokenTo = false;
	private RaceHud _hud;

	public override void Spawn()
	{
		base.Spawn();
		Model = Cloud.Model( "mdlresrc.devcom_bubble" );
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
		Components.Add( new NametagComponent("Type race") );
	}
	
	public bool OnUse( Entity user )
	{
		if ( Game.IsClient )
		{
			_hud = new RaceHud();
			user.Components.Get<HudComponent>().Hud.MiddleSection.AddChild( _hud );
		}
		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
