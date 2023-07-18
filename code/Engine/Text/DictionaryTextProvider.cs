using System;
using System.Collections.Generic;

namespace Sandtype.Engine.Text;

public class DictionaryTextProvider : TextProvider
{
	
	public List<string> Words;

	public DictionaryTextProvider( List<string> words )
	{
		Words = words;
	}

	public string GetName()
	{
		return "Words";
	}

	public string GetText( TextSize size )
	{
		int length = GetSize( size );
		string output = "";
		
		for ( int i = 0; i < length; i++ )
		{
			if ( i != 0 ) output += " ";
			output += Words[Random.Shared.Next(0, Words.Count)];
		}

		return output;
	}

	private int GetSize( TextSize size )
	{
		switch ( size )
		{
			case TextSize.LONG:
				return 50;
			case TextSize.MEDIUM:
				return 25;
			case TextSize.SHORT:
				return 10;
		}

		return 0;
	}
}
