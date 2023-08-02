using System;
using System.Collections.Generic;
using Sandbox.UI;

namespace TerryTyper;

/// <summary>
/// A modified TextEntry with support for displaying a text ghost via <c>TargetTokens</c>.
/// </summary>
public class TextInputBase : TextEntry
{

	public Action<string> OnWordTyped;
	public TextView View;
	public TextTheme Theme;
	public IList<string> TargetTokens = new List<string>();
	public IList<string> InputTokens = new List<string>();
	private int _currentIndex => InputTokens.Count;
	private int _eraseTicks = 0;

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
		View.Target = TargetTokens;
		View.Input = InputTokens;

		if ( _eraseTicks > 0 )
		{
			View.CurrentInput = "";
			_eraseTicks -= 1;
		}
		StateHasChanged();
	}

	public override void OnButtonTyped( ButtonEvent e )
	{
		AudioController.Current.PlayKey();
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
			Text = "";
			View.CurrentInput = "";
			InputTokens.Add( input );
			OnWordTyped?.Invoke(input);
			CreateValueEvent( "space", input );
			_eraseTicks = 3;
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
