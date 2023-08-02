using System;
using System.Linq;
using Sandbox;

namespace TerryTyper
{
	public partial class TyperGame : GameManager
	{
		/// <summary>
		/// An easy reference to the current <code>TyperGame</code>.
		/// </summary>
		public static TyperGame Entity => Current as TyperGame;

		public TyperGame()
		{
			if ( Game.IsClient )
			{
				// Spawn our client-side entities
				_ = new DataController();
				_ = new AudioController();
				_ = new UIController();
				_ = new ShopController();
			}
		}

		public override void ClientJoined( IClient client )
		{
			base.ClientJoined( client );
			
			var pawn = new Player();
			pawn.DressFromClient( client );
			client.Pawn = pawn;
			pawn.Spawn();

			var spawnpoints = All.OfType<SpawnPoint>();
			var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

			if ( randomSpawnPoint != null )
			{
				var tx = randomSpawnPoint.Transform;
				tx.Position += Vector3.Up * 50.0f; // raise it up
				pawn.Transform = tx;
			}
		}
	}
}
