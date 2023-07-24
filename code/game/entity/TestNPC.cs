using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandtype.hud;

namespace Sandtype.game.entity;

public class TestNPC : AnimatedEntity, IUse
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
		Tags.Add( "npc" );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		_namePanel = new WorldPanel();
		_namePanel.Add.Label( "Cirno the Test Giver" );
		_namePanel.StyleSheet.Load( "/ui/WorldLabel.scss" );
	}

	[GameEvent.Tick.Client]
	public void ClientTick()
	{
		_namePanel.Position = Position.WithZ( Position.z + 50f );
	}

	public bool OnUse( Entity user )
	{
		if ( !_spokenTo )
		{
			var dialog = new TextDialogComponent();
			dialog.Text = "Lol, sup fuck :D";
			user.Components.Add( dialog );
			_spokenTo = true;
		}

		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
