using System.Collections.Generic;
using System.Net.Quic;
using Sandbox;
using TerryTyper.Controller.Shop.Category;
using TerryTyper.UI.Hud.Dialog;

namespace TerryTyper.Controller.Shop;

public class ShopController : EntityComponent<Pawn>
{

	public IList<ShopCategory> Categories = new List<ShopCategory>();
	private DataController Data => TyperGame.Entity.GamePawn.Data;
	private UIController UI => TyperGame.Entity.UI;

	public ShopController()
	{
		Categories.Add( new ThemeCategory() );
	}

	public void OpenCategory( ShopCategory c )
	{
		if ( Data.Currency < c.RollPrice ) return;
		Data.Currency -= c.RollPrice;
		var item = c.Open();
		UI.ShowDialog( new ShopDialog( item ) );
	}
	
}
