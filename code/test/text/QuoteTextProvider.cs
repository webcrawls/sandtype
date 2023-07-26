using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandtype.Test.Text;

/// <summary>
/// Provides a static string of text.
/// </summary>
public class QuoteTextProvider : TextProvider
{

	public List<string> Text;

	public QuoteTextProvider( String text )
	{
		Text = text.Split( " " ).ToList();
	}

	public string GetName()
	{
		return "Static";
	}

	public void SetSize( int size )
	{
		// The size of the quote will be static, this does nothing.
	}

	public List<string> GetText()
	{
		return Text;
	}
}
