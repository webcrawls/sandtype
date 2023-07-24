using System;

namespace Sandtype.Text;

/// <summary>
/// Provides a static string of text.
/// </summary>
public class QuoteTextProvider : TextProvider
{

	public string[] Text;

	public QuoteTextProvider( String text )
	{
		Text = text.Split( " " );
	}

	public string GetName()
	{
		return "Static";
	}

	public void SetSize( int size )
	{
		// The size of the quote will be static, this does nothing.
	}

	public string[] GetText()
	{
		return Text;
	}
}
