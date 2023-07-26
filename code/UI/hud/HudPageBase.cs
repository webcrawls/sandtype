using Sandbox;
using Sandbox.UI;

namespace Sandtype.UI.Hud;

public class HudPageBase : Panel
{
	public HudPageBase()
	{
		AcceptsFocus = true;
	}

	public override void OnParentChanged()
	{
		base.OnParentChanged();
		Focus();
	}

	public override void OnButtonTyped( ButtonEvent e )
	{
		base.OnButtonTyped( e );
		if ( e.Pressed && e.Button == "escape" )
		{
			Parent.Delete();
		}
	}
}
