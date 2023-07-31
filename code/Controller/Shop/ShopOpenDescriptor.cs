using TerryTyper.Controller.Shop.Category;

namespace TerryTyper.Controller.Shop;

/// <summary>
/// Created by a ShopCategory when it is opened. Used by the UI to display the results to the user.
/// </summary>
public class ShopOpenDescriptor
{
	public ShopCategory Category { get; set; }
	public string ItemName { get; set; }
	public string ItemRarityName { get; set; }
	public float ItemRarityChance { get; set; }
}
