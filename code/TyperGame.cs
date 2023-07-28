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
		[BindComponent] public ServerRaceManager RaceManager { get; } // serveronly
		public UIController UI { get; set; } // clientonly

		public TyperGame()
		{
			if ( Game.IsClient )
			{
				UI = Components.Create<UIController>();
			} else if ( Game.IsServer )
			{
				Components.Create<ServerRaceManager>();
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

		[ConCmd.Server("tw_join")]
		public static void JoinGameCmd(long raceId)
		{
//			Entity.RaceManager.AddPlayer( ConsoleSystem.Caller.Pawn as Pawn, raceId );
		}

		[ConCmd.Server("tw_input")]
		public static void InputGameCmd(string input)
		{
			Entity.RaceManager.HandleInput( ConsoleSystem.Caller.Pawn as Pawn, input );
		}

		[ConCmd.Server("tw_submit")]
		public static void SubmitGameCmd(string input)
		{
			Entity.RaceManager.HandleSubmit( ConsoleSystem.Caller.Pawn as Pawn );
		}

		[ConCmd.Server("tw_create")]
		public static void CreateGameCmd()
		{
			Entity.RaceManager.CreateRace( ConsoleSystem.Caller.Pawn as Pawn );
		}

		
		[ConCmd.Server("tw_delete")]
		public static void DeleteGameCmd(long raceId)
		{
			Entity.RaceManager.DeleteRace( ConsoleSystem.Caller.Pawn as Pawn, raceId );
		}

		[ConCmd.Server("tw_leave")]
		public static void LeaveGameCmd()
		{
			//var pawn = ConsoleSystem.Caller.Pawn as Pawn;
			//var race = Entity.RaceManager.GetJoinedRace( pawn.Client.SteamId );
			//if ( race == null ) return;
			//Entity.RaceManager.RemovePlayer( pawn, race.RaceId );
		}

	}
}
