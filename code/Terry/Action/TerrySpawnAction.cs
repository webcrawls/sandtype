using Sandbox;
using Sandtype.UI;
using Sandtype.UI.Game;

namespace Sandtype.Terry.Action;

public class TerrySpawnAction : TerryAction
{
	public TerrySpawnAction(
		Pawn pawn, TerryGame game, TypingTest test, Hud hud ) : base( pawn, game, test, hud, "spawn", 0.0f, 1.0f )
	{
	}

	public override void Run()
	{
		base.Run();
		TerryGame.CreateTerry();
	}
}
