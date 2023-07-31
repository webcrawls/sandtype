using Sandbox.UI;
using TerryTyper.Text;

namespace TerryTyper.UI.Hud.Menu.Race;

public class RaceTextTypeDropdown : DropDown
{
	

	public RaceTextTypeDropdown()
	{
		foreach ( var id in TextProvider.Providers.Keys )
		{
			var item = new Option( id, id );
			Options.Add( item );
			
			// update 'default' selection
			if ( Selected == null ) Selected = item;
		}
	}
	
}
