using Sandtype.UI;
using Sandtype.UI.Game;

namespace Sandtype.Terry.Action;

public class TerryAction
{

	public string Id;
	public float Chance = 1.0f;
	public float AngerLevel { get { return _angerLevel; } set { _angerLevel = value; } }

	protected Pawn Pawn;
	protected TerryGame TerryGame;
	protected TypingTest Test;
	protected Hud Hud;
	
	private float _angerLevel;

	public TerryAction( Pawn pawn, TerryGame game, TypingTest test, Hud hud, string id, float angerLevel, float chance )
	{
		Pawn = pawn;
		TerryGame = game;
		Test = test;
		Hud = hud;
		Id = id;
		_angerLevel = angerLevel;
		Chance = chance;
	}

	public virtual void Run()
	{
		
	}
	
}
