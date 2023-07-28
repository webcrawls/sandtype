using System.Collections.Generic;
using Sandbox;
using TerryTyper.UI.Race;
using TerryTyper.UI.Text;

namespace TerryTyper.Race;

public partial class RacePlayer : EntityComponent<RaceEntity>
{

	[Net, Change(nameof(HandleNameChange))] public string SteamName { get; set; }
	[Net] public long SteamId { get; set; }
	[Net] public IList<string> Input { get; set; }
	[Net] public string CurrentInput { get; set; }
	[Net] public bool Winner { get; set; }
	public TextTheme Theme = new TextTheme();

	private Pawn _pawn => (Game.LocalPawn as Pawn);

	private void HandleNameChange()
	{
		Name = "Racer " + SteamName;
	}

	protected override void OnActivate()
	{
		base.OnActivate();
		if ( Game.IsClient && EnsurePlayer())
		{
			var page = new ActiveRacePage();
			page.Race = Entity;
			TyperGame.Entity.UI.PagePanel = page;
		}
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		if ( Game.IsClient && EnsurePlayer() )
		{
			TyperGame.Entity.UI.PagePanel = null;
		}
	}

	[GameEvent.Tick.Client]
	private void OnClientTick()
	{
	}

	[GameEvent.Tick.Server]
	private void OnServerTick()
	{
	}

	private bool EnsurePlayer()
	{
		if ( SteamId != Game.LocalClient.SteamId ) return false;
		return true;
	}

	
}
