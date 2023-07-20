using System;
using Sandbox.UI;

namespace Sandtype.UI.Text;

public class TextInput : TextEntry
{

	public bool IsActive = true;
	public Action EnterAction;
	public Action TabAction;
	public Action ContentChangedAction;
	public Action WordCompleteAction;

	public TextInput()
	{
		Style.Width = Length.Percent( 100 );
		Style.Height = Length.Percent( 100 );
		Style.Position = PositionMode.Absolute;
		Style.ZIndex = 2;
		Style.Cursor = "default";
		CaretColor = Color.Transparent;
	}

	public override void Tick()
	{
		if ( IsActive && !HasFocus )
		{
			Focus();
		}
		
		base.Tick();
	}

	// OnButtonEvent can be used to detect single keypresses, instead of grabbing the whole input.
	public override void OnButtonEvent( ButtonEvent e )
	{
		if ( e.Pressed && e.Button == "enter" )
		{
			Log.Info( e );
			EnterAction?.Invoke();
			return;
		}

		if ( e.Pressed && e.Button == "tab" )
		{
			Log.Info( e );
			TabAction?.Invoke();
			return;
		}

		if ( e.Pressed && e.Button == "space" )
		{
			WordCompleteAction?.Invoke();
		}
		
		ContentChangedAction?.Invoke();
	}
}
