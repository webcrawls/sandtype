using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Sandtype.Entity.NPC;

public class NPCNametag : WorldPanel
{

	public NPCNametag(string name)
	{
		StyleSheet.Load( "/ui/styles/WorldLabel.scss" );
		Add.Label( name );
		var width = 3500;
		PanelBounds = new Rect( ((float) -width / 3), -700, width, 500 );
	}
	
}
