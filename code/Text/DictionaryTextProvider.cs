using System;
using System.Collections.Generic;

namespace TerryTyper;

public class DictionaryTextProvider : TextProvider
{

	public virtual string Id { get; } = "dictionary";
	public string[] Words;

	public DictionaryTextProvider( string[] words )
	{
		Words = words;
	}

	public string[] GetText(int size = 20)
	{
		string[] words = new string[size];
		for ( int i = 0; i < size; i++ )
		{
			words[i] = Words[Random.Shared.Next( 0, Words.Length )];
		}

		return words;
	}
}
