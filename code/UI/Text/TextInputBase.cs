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
	public TextView View;
	public TextTheme Theme;
	public IList<string> TargetTokens = new List<string>();
	public IList<string> InputTokens = new List<string>();
	private int _currentIndex => InputTokens.Count;
	private string _currentTarget => _currentIndex < TargetTokens.Count ? TargetTokens[_currentIndex] : "";

	public TextInputBase()
	{
		TargetTokens = "Hello World My Name Jeff".Split( " " );
		CaretColor = Color.Transparent;
	}

	public override void Tick()
	{
		base.Tick();
		View.Theme = Theme;
		View.CurrentInput = Text;
		View.TargetTokens = TargetTokens;
		View.InputTokens = InputTokens;
		StateHasChanged();
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
			string input = Text.Replace( " ", "" );
			InputTokens.Add( input.Trim() );
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

	protected override int BuildHash()
	{
		return HashCode.Combine( Text );
	}

}
