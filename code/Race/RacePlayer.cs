using System.Collections.Generic;
using Sandbox;

namespace TerryTyper.Race;

public partial class RacePlayer : EntityComponent<RaceEntity>
{

	[Net] public string SteamName { get; set; }
	[Net] public long SteamId { get; set; }
	[Net] public bool Complete { get; set; }
	[Net] public IList<string> Input { get; set; }
	[Net] public string CurrentInput { get; set; }
	[Net] public string Theme { get; set; }
	[Net] public float CompleteTime { get; set; }

	public RacePlayer()
	{
		ShouldTransmit = true;
	}
	
	public void SubmitInput()
	{
		var input = new List<string>( Input );
		if ( CheckInputAccuracy() )
		{
			TyperGame.Entity.Pawns[SteamId].WordTyped();
		}
		input.Add( CurrentInput );
		Input = input;
		CurrentInput = "";
	}

	private bool CheckInputAccuracy()
	{
		if ( Input.Count >= Entity.Target.Count )
		{
			return false;
		}
		
		var target = Entity.Target[Input.Count];
		var input = CurrentInput;

		int accurateChars = 0;
		for ( int i = 0; i < target.Length; i++ )
		{
			if ( i >= input.Length )
			{
				continue;
			}
			if ( target[i] == input[i] )
			{
				accurateChars += 1;
			}
		}

		return (accurateChars / target.Length == 1);
	}

	public void SetInput(string input)
	{
		CurrentInput = input;
	}

}
