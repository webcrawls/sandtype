using System;
using System.Collections.Generic;

namespace Sandtype.Engine.Text;

public class DictionaryTextProvider : TextProvider
{
	
	public string[] Words;
	public int Size = 10;

	public DictionaryTextProvider( string[] words )
	{
		Words = words;
	}

	public string[] GetText()
	{
		string[] words = new string[Size];
		for ( int i = 0; i < Size; i++ )
		{
			words[i] = Words[Random.Shared.Next( 0, Words.Length )];
		}

		return words;
	}
}
