using System;
using Sandbox;
using TerryTyper.Controller;
using TerryTyper.Race;
using TerryTyper.UI.Hud;
using TerryTyper.UI.Race;

namespace TerryTyper;

/// <summary>
/// In the darkest hour,
/// Garry's words bring truth and light
/// He will save us all
///
/// https://github.com/orgs/sboxgame/discussions/975
/// </summary>
public partial class Pawn : AnimatedEntity
{
	
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen/citizen.vmdl" );
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );
		SimulateInput( cl );
	}
	
	public void DressFromClient( IClient cl )
	{
		var c = new ClothingContainer();
		c.LoadFromClient( cl );
		c.DressEntity( this );
	}

	[ClientRpc]
	public void ShowRaceHud()
	{
		var page = new ActiveRacePage();
		TyperGame.Entity.UI.PagePanel = page;
	}

	[ClientRpc]
	public void HideRaceHud()
	{
		TyperGame.Entity.UI.PagePanel = null;
	}

	public void Respawn()
	{
		ResetInput();
	}

}
