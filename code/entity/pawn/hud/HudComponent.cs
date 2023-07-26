using Sandtype.UI.Hud;

namespace Sandtype.Entity.Pawn.Hud;
using Sandbox;

public class HudComponent : EntityComponent<Pawn>
{
	
	public GameHud Hud;
	
	protected override void OnActivate()
	{
		base.OnActivate();
		Hud = new();
	}
	
	protected override void OnDeactivate()
	{
		Hud?.Delete();
	}
}
