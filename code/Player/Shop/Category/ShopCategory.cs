using System.Collections.Generic;

namespace TerryTyper;

public interface ShopCategory
{

	public int RollPrice { get; }

	public ShopOpenDescriptor Open();

}
