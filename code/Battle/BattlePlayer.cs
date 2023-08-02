using Sandbox;

namespace TerryTyper;

/// <summary>
/// A component representing an active Battle player.
/// </summary>
public partial class BattlePlayer : EntityComponent<Battle>
{

	/// <summary>
	/// The player's Steam64.
	/// </summary>
	[Net] public long SteamId { get; set; }
	
}
