using System.Collections.Generic;
using Sandbox;

namespace Sandtype.Game.Component;

public class TerryGameComponent : EntityComponent<Pawn>
{

	public int TerryMaxHealth = 10;
	public int TerryHealth = 10;
	public TerryBotData[] TerryBots => _botData.ToArray();
	private List<TerryBotData> _botData;
	
	protected override void OnActivate()
	{
		base.OnActivate();
		_botData = new List<TerryBotData>() {};
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		_botData.Clear();
		_botData = null; // something i'd do in java, do we need this in c#?
		// probably not since calling OnActivate() again would just update _botData with a new list
		// so....
	}
	
	public void Simulate()
	{
		for ( int i = 0; i < _botData.Count; i++ )
		{
			var data = _botData[i];
			if ( data.Progress >= 1.0 )
			{
				_botData.RemoveAt( i );
				break;
			}
		}
	}
}
