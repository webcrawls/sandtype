using Sandtype.Entity.Pawn.Hud;
using Sandtype.UI.Hud.Pages;

namespace Sandtype.Entity.NPC;
using Sandbox;
using Sandbox.UI;

public class SettingsNPC : AnimatedEntity, IUse
{

	private WorldPanel _namePanel; // clientonly
	private bool _spokenTo = false;
	private SettingsHud _hud;

	public override void Spawn()
	{
		base.Spawn();
		Model = Cloud.Model( "facepunch.tv" );
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
		Components.Add( new NametagComponent("the configuration box (Opens two i dont know why)") );
	}
	
	public bool OnUse( Entity user )
	{
		if ( Game.IsClient )
		{
			_hud = new SettingsHud();
			user.Components.Get<HudComponent>().Hud.MiddleSection.AddChild( _hud );
			Log.Info( "Opening hud" );
		}
		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
