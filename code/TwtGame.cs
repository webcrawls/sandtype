using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sandbox;
using Sandtype.Entity.NPC;
using Sandtype.Entity.Pawn;
using Sandtype.Test;
using Sandtype.Util;

namespace Sandtype
{
	public partial class TwtGame : GameManager
	{
		
		public static TwtGame Entity => Current as TwtGame;

		[Net] public IList<RaceData> Games { get; set; }
		
		private TestNPC _testNPC;

		private int gameCooldown = 50;
		private int gameTimer = -1;
		
		[GameEvent.Tick.Server]
		public void OnServerTick(){
		}

		public override void PostLevelLoaded()
		{
			SetupNPCs();

			if ( Game.IsServer )
			{
				Components.Create<ServerRaceManager>();
			}
		}

		public override void ClientJoined( IClient client )
		{
			base.ClientJoined( client );

			Pawn pawn = new Pawn();

			client.Pawn = pawn;
			client.Pawn.Position = FindSpawn();

			pawn.Respawn();
			pawn.DressFromClient( client );

			if ( Game.IsServer )
			{
				Components.Get<ServerRaceManager>().CreateRace( pawn );
			}
			
			Games.Add( new RaceData
			{
				OwnerName = client.Name,
				OwnerId = client.Client.Pawn.Id
			} );
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

			var testPos = rootPoint;
			var settingsPos = rootPoint.WithY( rootPoint.y - 150f );
			var racePos = rootPoint.WithY( rootPoint.y - 350f );
			var infoPos = rootPoint.WithY( rootPoint.y + 150f );
			var shopPos = rootPoint.WithY( rootPoint.y + 350f );

			var testNPC = new TestNPC();
			testNPC.Position = testPos;
			testNPC.Spawn();

			var settingsNPC = new SettingsNPC();
			settingsNPC.Position = settingsPos;
			settingsNPC.Spawn();

			var infoNPC = new InfoNPC();
			infoNPC.Position = infoPos;
			infoNPC.Spawn();

			var shopNPC = new ShopNPC();
			shopNPC.Position = shopPos;
			shopNPC.Spawn();

			var raceNPC = new RaceNPC();
			raceNPC.Position = racePos;
			raceNPC.Spawn();

		}

		/// <summary>
		/// Locates a spawn point for the supplied pawn.
		/// </summary>
		/// <param name="p">the pawn</param>
		/// <returns>A Vector3 containing a spawn point. Returns Vector3.Zero if none were found.</returns>
		private Vector3 FindSpawn() => All.OfType<SpawnPoint>()
			.Select( s => s.Position )
			.RandomElement( Vector3.Zero );
	}
}
