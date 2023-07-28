using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using TerryTyper.Controller;
using TerryTyper.Race;

namespace TerryTyper
{
	public partial class TyperGame : GameManager
	{
		public static TyperGame Entity => Current as TyperGame;
		public UIController UI { get; private set; } // clientonly
		public Pawn GamePawn => Game.LocalPawn as Pawn; // clientonly

		public TyperGame()
		{
			if ( Game.IsClient )
			{
				UI = Components.Create<UIController>();
			}
		}
		
		public IDictionary<long, Pawn> Pawns = new Dictionary<long, Pawn>();

		public override void ClientJoined( IClient client )
		{
			base.ClientJoined( client );
			
			var pawn = new Pawn();
			pawn.DressFromClient( client );
			pawn.Respawn();
			client.Pawn = pawn;
			Pawns[client.SteamId] = pawn;

			var spawnpoints = All.OfType<SpawnPoint>();
			var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

			if ( randomSpawnPoint != null )
			{
				var tx = randomSpawnPoint.Transform;
				tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
				pawn.Transform = tx;
			}
		}
	}
}
