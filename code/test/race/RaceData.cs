using System.Collections.Generic;
using Sandbox;
using Sandbox.Menu;

namespace Sandtype.Test;

public partial class RaceData : BaseNetworkable
{
	[Net] public string OwnerName { get; set; }
	[Net] public long OwnerId { get; set; }
	[Net] public string ProviderType { get; set; } = "english_1k";
	[Net] public long RaceId { get; set; }
	[Net] public IDictionary<long, string> PlayerInputs { get; set; }
	[Net] public IList<long> JoinedPlayers { get; set; }
	[Net] public IList<long> QuitPlayers { get; set; }
}
