namespace Sandtype.Test.Action;

public class TerrySpawnAction : TerryAction
{
	public TerrySpawnAction(
		TerryGame game, TypingTest test ) : base( game, test, "spawn", 0.0f, 1.0f )
	{
	}

	public override void Run()
	{
		base.Run();
		TerryGame.CreateTerry();
		
	}
}
