namespace Sandtype.Entity.Pawn.Hud;
using Sandbox;
using Sandtype.UI.Text;


public class TextDialogComponent : EntityComponent<Pawn>, ITickable
{

	public string Text;
	public int delay = 100;
	private TextView _view;
	
	protected override void OnActivate()
	{
		base.OnActivate();
		_view = new TextView();
		_view.Text = Text;
		Game.RootPanel.AddChild( _view );
	}
	
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		if ( _view == null ) return;
		_view.Delete();
	}

	public void Tick()
	{
		if ( delay > 0 )
		{
			delay -= 1;
			return;
		}

		Entity.Components.Remove( this );
	}
}
