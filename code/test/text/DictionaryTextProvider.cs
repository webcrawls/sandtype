using System;
using System.Collections.Generic;

namespace Sandtype.Test.Text;

/// <summary>
/// Provides random words.
/// </summary>
public class DictionaryTextProvider : TextProvider
{
	
	public string[] Words;
	public int Size { get; private set; } = 50;

	public DictionaryTextProvider( string[] words )
	{
		Words = words;
	}

	public void SetSize( int size )
	{
		Size = size;
	}

	public List<string> GetText()
	{
		var words = new List<string>();
		for ( int i = 0; i < Size; i++ )
		{
			words.Add(Words[Random.Shared.Next( 0, Words.Length )]);
		}

		return words;
	}
}
