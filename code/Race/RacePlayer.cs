using System.Collections.Generic;
using Sandbox;
using TerryTyper.UI.Race;
using TerryTyper.UI.Text;

namespace TerryTyper.Race;

public partial class RacePlayer : EntityComponent<RaceEntity>
{

	[Net, Change(nameof(HandleNameChange))] public string SteamName { get; set; }
	[Net] public long SteamId { get; set; }
	[Net] public string Name { get; set; }
	[Net] public IList<string> Input { get; set; }
	[Net] public string CurrentInput { get; set; }
	[Net] public bool Winner { get; set; }
	[Net] public string Theme { get; set; }

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
		if ( Input.Count == Entity.Target.Count )
		{
			Winner = true;
		}
	}

	private bool CheckInputAccuracy()
	{
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

	private void HandleNameChange()
	{
		Name = "Racer " + SteamName;
	}

	private bool EnsurePlayer()
	{
		if ( SteamId != Game.LocalClient.SteamId ) return false;
		return true;
	}

	
}
