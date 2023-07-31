using System.Collections.Generic;
using TerryTyper.Controller.Shop.Item;
using TerryTyper.UI.Text;
using TerryTyper.Util;

namespace TerryTyper.Controller.Shop.Category;

public class ThemeCategory : ShopCategory
{
	
	private static readonly IDictionary<string, float> _rarityWeights = new Dictionary<string, float>()
	{
		{ "common", 10 }, { "rare", 3.4f }, { "legendary", 1.1f }
	};

	public string Name { get; set; } = "Themes";
	public string WinText { get; } = "New theme unlocked!";
	public int RollPrice { get; set; } = 15;
	public IList<ShopItem> Items { get; } = new List<ShopItem>();

	public ThemeCategory()
	{
		foreach ( var theme in TextTheme.Themes.Values )
		{
			var item = new ShopItem
			{
				Id = theme.Id,
				Name = theme.Name,
				Description = theme.Description,
				Rarity = theme.Rarity,
				Weight = _rarityWeights[theme.Rarity]
			};
			Items.Add( item );
		}
	}

	public ShopOpenDescriptor Open()
	{
		var item = Items.RandomElementByWeight( e => e.Weight );
		var themes = new List<string>( TyperGame.Entity.GamePawn.Data.UnlockedThemes );
		themes.Add( item.Id );
		
		// update pawn data (this should be done a better way)
		TyperGame.Entity.GamePawn.Data.UnlockedThemes = themes;
		
		return new ShopOpenDescriptor
		{
			Category = this, ItemName = item.Name, ItemRarityName = item.Rarity, ItemRarityChance = item.Weight,
		};
	}
}
