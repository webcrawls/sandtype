using Sandtype.Entity.Pawn.Test;

namespace Sandtype.Test.Action;
using Entity.Pawn;
using Test;
using UI;

public class TerryAction
{

	public string Id;
	public float Chance = 1.0f;
	public float AngerLevel { get { return _angerLevel; } set { _angerLevel = value; } }

	protected TerryGame TerryGame;
	protected TypingTest Test;
	
	private float _angerLevel;

	public TerryAction( TerryGame game, TypingTest test, string id, float angerLevel, float chance )
	{
		TerryGame = game;
		Test = test;
		Id = id;
		_angerLevel = angerLevel;
		Chance = chance;
	}

	public virtual void Run()
	{
		
	}
	
}
