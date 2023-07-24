using System;
using System.Collections.Generic;

namespace Sandtype.Text;

/// <summary>
/// Provides random words.
/// </summary>
public class DictionaryTextProvider : TextProvider
{
	
	public string[] Words;
	public int Size { get; private set; }

	public DictionaryTextProvider( string[] words )
	{
		Words = words;
	}

	public void SetSize( int size )
	{
		Size = size;
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
