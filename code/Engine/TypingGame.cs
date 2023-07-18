﻿using System;
using System.Collections.Generic;
using Sandbox.UI;
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
		Provider = new DictionaryTextProvider( new List<string>() { "hello", "world" } );
	}

	public void Reset()
	{
		Started = true;
		Completed = false;
		StartTime = DateTime.Now;
		Target = Provider.GetText( TextSize.MEDIUM );
		Content = "";
		FinalWpm = 0;
	}

	public void HandleInput( string input )
	{
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
		Pawn.Hud.Entry.Disabled = true;
	}
	
}