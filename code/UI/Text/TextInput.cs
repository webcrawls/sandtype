using System;
using Sandbox.Component;
using Sandbox.UI;

namespace Sandtype.UI.Text;

/// <summary>
/// A TextEntry Panel with additional Action triggers. Useful for grabbing input and displaying it via other means.
/// </summary>
public class TextInput : TextEntry
{
	
	public TextInput()
	{
		Style.Width = Length.Percent( 100 );
		Style.Height = Length.Percent( 100 );
		Style.Cursor = "default";
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
			CreateEvent( "onspace" );
		}
		
		CreateEvent( "onchanged");
		
		base.OnButtonTyped( e );
	}
}
