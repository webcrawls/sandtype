using System.Collections.Generic;
using TerryTyper.Controller.Shop.Item;

namespace TerryTyper.Controller.Shop.Category;

public interface ShopCategory
{

	public string Name { get; }
	public string WinText { get; }
	public IList<ShopItem> Items { get; }
	public int RollPrice { get; }

	public ShopOpenDescriptor Open();

}
