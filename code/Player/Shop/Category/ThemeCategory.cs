using System;
using System.Collections.Generic;

namespace TerryTyper;

public class ThemeCategory : ShopCategory
{
	
	private static readonly IDictionary<string, float> _rarityWeights = new Dictionary<string, float>()
	{
		{ "common", 10 }, { "rare", 3.4f }, { "legendary", 1.1f }
	};

	public int RollPrice { get; set; } = 15;

	public ShopOpenDescriptor Open()
	{
		var (item, chance) = TextTheme.Themes.Values.RandomElementByWeight( e => _rarityWeights[e.Rarity] );
		var themes = new List<string>( DataController.Current.UnlockedThemes );
		themes.Add( item.Id );
		
		// update pawn data (this should be done a better way)
		DataController.Current.UnlockTheme( item.Id );
		
		return new ShopOpenDescriptor
		{
			Category = this,
			ItemName = item.Name,
			ItemRarityName = item.Rarity,
			ItemIcon = item.Icon,
			ItemRarityChance = Math.Round(chance, 2),
		};
	}
}
