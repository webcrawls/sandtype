using Sandbox.UI;
using TerryTyper.UI.Hud.Page.Info;
using TerryTyper.UI.Hud.Page.Race;

namespace TerryTyper.UI.Hud.Page;

public class GamePageBase : NavHostPanel
{

	public string PageName
	{
		get
		{
			if ( CurrentPanel == null || !(CurrentPanel is IGamePage page) )
			{
				return "Unnamed Page :(";
			}

			return page.Name;
		}
	}

	public GamePageBase()
	{
		DefaultUrl = "/";
		AddDestination( "/", typeof(InfoPage) );
		AddDestination( "/stats", typeof(StatsPage) );
		AddDestination( "/race", typeof(RacePage) );
		AddDestination( "/shop", typeof(ShopPage) );
	}

	protected override void OnAfterTreeRender( bool firstTime )
	{
		base.OnAfterTreeRender( firstTime );
		if ( firstTime )
		{
			NavigatorCanvas.AddClass( "wrapper" );
			NavigatorCanvas.BindClass( "scrollable", () => CurrentPanel is IGamePage page && page.ShouldScroll);
		}
	}

	protected void HandleClose()
	{
		TyperGame.Entity.UI.ClosePage();
	}
}
