using Sandbox;
using Sandbox.UI;

namespace TerryTyper;

public class DialogBase : Panel
{

	public bool AutoClose { get; set; } = true;
	public TimeUntil AutoCloseTimer = 5f;

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
