using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandtype.Engine.Text;

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

	public string[] GetText()
	{
		return Text;
	}
}
