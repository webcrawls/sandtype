using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.UI;

namespace TerryTyper.UI.Text;

/// <summary>
/// A modified TextEntry with support for displaying a text ghost via <c>TargetTokens</c>.
/// </summary>
public class TextInputBase : TextEntry
{
	public Action EnterPressed;
	public Action TabPressed;
	public Action SpacePressed;
	public Action EscapePressed;
	
	public TextTheme Theme = new();
	public IList<string> TargetTokens = new List<string>();
	public IList<string> InputTokens = new List<string>(); 

	public TextInputBase()
	{
		TargetTokens = "Hello World My Name Jeff".Split( " " );
		CaretColor = Color.Transparent;
	}
	
	public override void OnButtonTyped( ButtonEvent e )
	{
		if ( e.Pressed && e.Button == "enter" )
		{
			CreateEvent( "onenter" );
			return;
		}

		if ( e.Pressed && e.Button == "tab" )
		{
			CreateEvent( "ontab" );
			return;
		}

		if ( e.Pressed && e.Button == "space" )
		{
			string input = Text;
			InputTokens.Add( input );
			CreateValueEvent( "space", input );
			Text = "";
			return;
		}

		if ( e.Pressed && e.Button == "escape" )
		{
			CreateEvent( "onescape" );
			return;
		}

		CreateEvent( "onchanged");
		
		base.OnButtonTyped( e );
	}

}

public class TextTheme
{
	public string Id = "default";
	public string Font = "Poppins";
	public int FontSize = 30;
	
	public string ColorMain = "#fbf5ef";
	public string ColorTyped = "#f2d3ab";
	public string ColorError = "#c69fa5";
	public string ColorCaret = "#8b6d9c";
	public string ColorBackgroundAlt = "#494d7e";
	public string ColorBackground = " #272744 ";

}
