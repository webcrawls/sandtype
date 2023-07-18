using System;
using System.Linq;
using Sandtype.Engine.Text;

namespace Sandtype.Engine;

public class TypingGame
{
	public Pawn Pawn;
	public TextProvider Provider;
	public String Target = "";
	public String Content = "";
	public DateTime StartTime;
	public DateTime EndTime;
	public double FinalWpm = 0;
	public TimeSpan Time => EndTime - StartTime;
	public bool Started;
	public bool Completed;

	public TypingGame(Pawn pawn)
	{
		Pawn = pawn;
		Provider = new DictionaryTextProvider("The quick brown fox jumps over the lazy dog".Split( " " ).ToList());
	}

	public void Reset()
	{
		Started = false;
		Completed = false;
		StartTime = DateTime.Now;
		Target = Provider.GetText( TextSize.MEDIUM );
		Content = "";
		FinalWpm = 0;
	}

	public void HandleInput( string input )
	{
		if ( Completed )
		{
			return;
		}
		
		if ( input.Length == Target.Length )
		{
			Complete();
		}
		if ( !Started )
		{
			// handle start state
			Started = true;
			StartTime = DateTime.Now;
		}
		Content = input;
	}

	private void Complete()
	{
		EndTime = DateTime.Now;
		Completed = true;
	}
	
}
