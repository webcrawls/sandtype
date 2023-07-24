using Sandbox;
using Sandtype.UI.Setting;

namespace Sandtype.Entity.Pawn.Hud;

public class SettingsHudComponent : EntityComponent<Pawn>, ISingletonComponent
{
	private SettingHud _view;
	
	protected override void OnActivate()
	{
		base.OnActivate();
		_view = new SettingHud();
		_view.AddEventListener( "onclose", () => Entity.Components.Remove( this ) );
		Game.RootPanel.AddChild( _view );
	}
	
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		if ( _view == null ) return;
		_view.Delete();
	}

}
