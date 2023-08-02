using Sandbox;

namespace TerryTyper;

/// <summary>
/// A class containing the player's clothing logic.
/// </summary>
public partial class Player
{
	
	public ClothingContainer Clothing { get; protected set; } = new();
	
	public void DressFromClient( IClient cl )
	{
		Clothing ??= new();
		Clothing.LoadFromClient( cl );
		
	}

}
