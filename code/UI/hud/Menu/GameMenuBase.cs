using Sandbox.UI;

namespace TerryTyper;

public class GameMenuBase : NavHostPanel
{
	
	public GameMenuBase()
	{
		DefaultUrl = "/";
		AddDestination( "/", typeof(MainMenuPage) );
		AddDestination( "/races", typeof(RaceMenuPage) );
		AddDestination( "/collection", typeof(CollectionMenuPage) );
	}

}
