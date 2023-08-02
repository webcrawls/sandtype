using System.Collections.Generic;
using Sandbox;

namespace TerryTyper;

public class ShopController : Entity
{

	public static ShopController Current { get; private set; }

	public IList<ShopCategory> Categories = new List<ShopCategory>();

	public ShopController()
	{
		Categories.Add( new ThemeCategory() );
		Current = this;
	}

	public void OpenCategory( ShopCategory c )
	{
		if ( DataController.Current.Currency < c.RollPrice ) return;
		DataController.Current.Currency -= c.RollPrice;
		var item = c.Open();
		UIController.Current.ShowDialog( new ShopDialog( item ) );
	}
	
}
