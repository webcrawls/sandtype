using Sandtype.Entity.Interaction;
using Sandtype.Entity.Pawn.Hud;

namespace Sandtype.Entity.NPC;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class SettingsNPC : AnimatedEntity, IUse
{

	private WorldPanel _namePanel; // clientonly
	private bool _spokenTo = false;

	public override void Spawn()
	{
		base.Spawn();
		Model = Cloud.Model( "mml.cirno" );
		Scale = 2.0f;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		EnableSolidCollisions = false;
		var c = new ClothingContainer();
		var hat = new Clothing()
		{
			Model = Cloud.Model( "tfassets.hardhat001" ).ResourcePath,
			Category = Clothing.ClothingCategory.Hat
		};
		c.Toggle(hat);
		c.DressEntity( this );
		Tags.Add( "npc" );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		_namePanel = new NPCNametag("Cirno the Configurator");
	}

	[GameEvent.Tick.Client]
	public void ClientTick()
	{
		_namePanel.Position = Position.WithZ( Position.z + 50f );
	}

	public bool OnUse( Entity user )
	{
		if ( Game.IsClient )
		{
			user.Components.Create<SettingsHudComponent>();
		}
		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
