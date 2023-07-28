using Sandbox;
using Sandbox.UI;
using TerryTyper.UI.Hud;

namespace TerryTyper.Controller;

public class UIController : EntityComponent<GameManager>
{

	public RootHud Hud;
	public Panel MenuPanel { get { return _menuPanel; } set {SetMenu( value );} }
	public Panel PagePanel { get { return _pagePanel; } set {SetPage( value );} }

	private Panel _menuPanel;
	private Panel _pagePanel;
	

	protected override void OnActivate()
	{
		base.OnActivate();
		Hud = new RootHud();
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		Hud.Delete();
	}

	private void SetMenu( Panel panel )
	{
		if ( MenuPanel != null && MenuPanel.IsValid )
		{
			MenuPanel.Delete();
		}

		if ( panel != null )
		{
			Hud.MenuWrapper.AddChild( panel );
		}

		_menuPanel = panel;
	}

	private void SetPage( Panel panel )
	{
		if ( PagePanel != null && PagePanel.IsValid )
		{
			PagePanel.Delete();
		}

		if ( panel != null )
		{
			Hud.PageWrapper.AddChild( panel );
		}
		
		_pagePanel = panel;
	}
}
