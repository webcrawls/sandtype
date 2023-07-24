using System.Linq;
using Sandbox;
using Sandtype.Entity.NPC;
using Sandtype.Entity.Pawn;
using Sandtype.UI;
using Sandtype.Util;
namespace Sandtype;
public partial class SandtypeGame : GameManager
{

	private TestNPC _testNPC;

	public SandtypeGame()
	{
		InitializeHud();
	}

	public override void PostLevelLoaded()
	{
		SetupNPCs();
	}

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		Pawn pawn = new Pawn();
		
		client.Pawn = pawn;
		client.Pawn.Position = FindSpawn(  );
		
		pawn.Respawn();
		pawn.DressFromClient( client );
	}

	public override void Simulate( IClient cl )
	{
		if ( cl.Pawn is Pawn p )
		{
			_testNPC?.Simulate( cl );
			p.Simulate( cl );
		}
	}

	private void SetupNPCs()
	{
		var rootPoint = FindSpawn();
		rootPoint = rootPoint.WithZ( rootPoint.z - 35f );

		var testPoint = rootPoint;
		var settingsPoint = rootPoint.WithY( rootPoint.y - 150f );
		var informerPoint = rootPoint.WithY( rootPoint.y - 350f );

		var testNPC = new TestNPC();
		testNPC.Position = testPoint;
		testNPC.Spawn();

		var settingsNPC = new SettingsNPC();
		settingsNPC.Position = settingsPoint;
		settingsNPC.Spawn();

		var informerNPC = new InformerNPC();
		informerNPC.Position = informerPoint;
		informerNPC.Spawn();
	}
	
	private void InitializeHud( )
	{
		if ( !Game.IsClient ) return;
		Game.RootPanel = new Hud();
	}

	/// <summary>
	/// Locates a spawn point for the supplied pawn.
	/// </summary>
	/// <param name="p">the pawn</param>
	/// <returns>A Vector3 containing a spawn point. Returns Vector3.Zero if none were found.</returns>
	private Vector3 FindSpawn( ) => All.OfType<SpawnPoint>()
		.Select( s => s.Position )
		.RandomElement(Vector3.Zero);
	
}
