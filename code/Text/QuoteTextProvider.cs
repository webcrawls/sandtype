using System;
using System.Collections.Generic;

namespace TerryTyper;

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

	public string[] GetText(int size = 0)
	{
		return Text;
	}

	public string Id { get; } = "Quote";
}
