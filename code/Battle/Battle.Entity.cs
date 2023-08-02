using Sandbox;

namespace TerryTyper;

public partial class Battle : IUse
{
	public bool OnUse( Entity user )
	{
		if ( user is not Player p )
			return false;

		if ( GetPlayer( p.Client.SteamId ) != null )
			return false;

		AddPlayer( p );
		
		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
