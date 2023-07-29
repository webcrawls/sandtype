using Sandbox;
using Sandbox.Services;
using TerryTyper.Controller;

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

	public DataController Data { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen/citizen.vmdl" );
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		Data = Components.Create<DataController>();
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
		TyperGame.Entity.UI.PageNavigate( "/race" );
	}

	[ClientRpc]
	public void HideRaceHud()
	{
		TyperGame.Entity.UI.ClosePage();
	}

	[ClientRpc]
	public void WordTyped()
	{
		Stats.Increment( "words_typed", 1 );
		Data.Currency += 1;
	}

	public void Respawn()
	{
		ResetInput();
	}

}
