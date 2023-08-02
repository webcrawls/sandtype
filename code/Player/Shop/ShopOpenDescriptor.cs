namespace TerryTyper;

/// <summary>
/// Created by a ShopCategory when it is opened. Used by the UI to display the results to the user.
/// </summary>
public class ShopOpenDescriptor
{
	public ShopCategory Category { get; set; }
	public string ItemName { get; set; }
	public string ItemRarityName { get; set; }
	public double ItemRarityChance { get; set; }
	public string ItemIcon { get; set; }
}
