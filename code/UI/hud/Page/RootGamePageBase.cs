using Sandbox.UI;

namespace TerryTyper;

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
		AddDestination( "/themes", typeof(ThemePage) );
		AddDestination( "/battle", typeof(BattlePage) );
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
		UIController.Current.ClosePage();
	}
}
