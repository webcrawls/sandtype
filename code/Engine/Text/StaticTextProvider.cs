using System;
using System.Collections.Generic;

namespace Sandtype.Engine.Text;

public class StaticTextProvider : TextProvider
{
	
	public String Text;

	public StaticTextProvider( String text )
	{
		Text = text;
	}

	public string GetName()
	{
		return "Static";
	}

	public string GetText( TextSize size )
	{
		return Text;
	}
}
