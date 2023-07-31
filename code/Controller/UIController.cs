using System.Collections.Generic;
using Sandbox;
using TerryTyper.Race;
using TerryTyper.UI.Hud;
using TerryTyper.UI.Hud.Dialog;
using TerryTyper.UI.Hud.Menu;
using TerryTyper.UI.Hud.Page;

namespace TerryTyper.Controller;

public class UIController : EntityComponent<GameManager>
{

	public RootHud Hud;
	public GameMenu MenuPanel { get { return _menuPanel; } set {SetMenu( value );} }
	public RootGamePage PagePanel { get { return _pagePanel; } set {SetPage( value );} }
	
	private GameMenu _menuPanel;
	private RootGamePage _pagePanel;
	
	public void PageNavigate(string route)
	{
		OpenPage();
		_pagePanel.Navigate( route );
	}

	public void ShowDialog( Dialog dialog )
	{
		Hud.DialogWrapper.AddChild( dialog );
	}
	
	public void ToggleUI()
	{
		bool hasOpen = MenuPanel != null || PagePanel != null;
		if ( hasOpen )
		{
			CloseMenu();
			ClosePage();
		}
		else
		{
			OpenMenu();
			if ( RaceEntity.GetJoinedRace( Game.LocalClient.SteamId ) != null )
			{
				PageNavigate( "/race" );
			}
		}
	}

	public void OpenMenu()
	{
		if ( MenuPanel == null )
		{
			MenuPanel = new GameMenu();
		}
	}

	public void OpenPage()
	{
		if ( PagePanel== null )
		{
			PagePanel = new RootGamePage();
		}
	}

	
	public void CloseMenu()
	{
		if ( MenuPanel != null )
		{
			MenuPanel = null;
		}
	}

	public void ClosePage()
	{
		if ( PagePanel != null )
		{
			PagePanel = null;
		}
	}

	public void ToggleMenu()
	{
		if ( MenuPanel == null )
		{
			MenuPanel = new GameMenu();
		}
		else
		{
			MenuPanel.Delete();
			MenuPanel = null;
		}
	}

	public void TogglePage()
	{
		if ( PagePanel == null )
		{
			PagePanel = new RootGamePage();
		}
		else
		{
			PagePanel.Delete();
			PagePanel = null;
		}
	}

	protected override void OnActivate()
	{
		base.OnActivate();
		Hud = new RootHud();
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		Hud.Delete();
		Hud = null;
	}
	
	private void SetMenu( GameMenu panel )
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

	private void SetPage( RootGamePage panel )
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
