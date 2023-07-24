using Sandtype.UI;
using Sandtype.UI.Game;

namespace Sandtype.Terry.Action;

public class LayoutFlipTerryAction : TerryAction
{
	public LayoutFlipTerryAction(
		Pawn pawn, TerryGame game, TypingTest test, Hud hud ) : base( pawn, game, test, hud, "flip", 0.0f, 0.1f )
	{
		
	}

	public override void Run()
	{
		base.Run();
	}
}
