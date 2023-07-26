﻿using Sandtype.Entity.Interaction;

namespace Sandtype.Entity.NPC;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

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
		Components.Add( new NametagComponent("the test giver") );
		Components.Add( new TestGiver() );
	}

	public bool OnUse( Entity user )
	{
		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
