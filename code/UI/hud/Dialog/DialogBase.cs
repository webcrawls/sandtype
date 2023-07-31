using Sandbox;
using Sandbox.UI;

namespace TerryTyper.UI.Hud.Dialog;

public class DialogBase : Panel
{

	public bool AutoClose { get; set; }
	public TimeUntil AutoCloseTimer = 15f;

	public DialogBase()
	{
		StyleSheet.Load( "/UI/Hud/Dialog/Dialog.razor.scss" );
	}

	public override void Tick()
	{
		base.Tick();

		if ( !AutoClose ) return;

		if ( AutoCloseTimer <= 0f )
		{
			Delete();
		}
	}
}
