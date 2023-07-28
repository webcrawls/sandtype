using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sandbox;

namespace TerryTyper.Race;

public partial class RaceEntity : Entity
{
	
	public Pawn Owner;

	public List<RacePlayer> Players => Components.GetAll<RacePlayer>().ToList();

	public int RaceId => NetworkIdent;
	
	[Net] public long OwnerId { get; set; }
	[Net] public IList<long> QuitPlayers { get; set; }
	[Net] public IList<long> Winners { get; set; }
	[Net] public IList<string> Target { get; set; }
	[Net] public string OwnerName { get; set; }
	[Net] public bool Started { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		OwnerName = Owner?.Client.Name ?? "Unknown";
		Name = OwnerName + "'s Race";
		QuitPlayers = new List<long>();
		Winners = new List<long>();
		Target = new List<string>();
	}

	public void AddPlayer(Pawn pawn)
	{
		var cmp = Components.Create<RacePlayer>();
		cmp.SteamId = pawn.Client.SteamId;
		cmp.SteamName = pawn.Client.Name;
		cmp.Input = new List<string>();
		cmp.CurrentInput = "";
	}
	
	public void RemovePlayer( Pawn pawn )
	{
		
	}
	
}
