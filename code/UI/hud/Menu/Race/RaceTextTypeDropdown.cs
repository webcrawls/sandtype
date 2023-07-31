using Sandbox.UI;
using Sandtype.Engine.Text;

namespace TerryTyper.UI.Hud.Menu.Race;

public class RaceTextTypeDropdown : DropDown
{
	

	public RaceTextTypeDropdown()
	{
		foreach ( var (name, provider) in TextProvider.Providers )
		{
			var item = new Option( name, name );
			Options.Add( item );
			
			// update 'default' selection
			if ( Selected == null ) Selected = item;
		}
	}
	
}
