using System.Collections.Generic;
using Sandbox;

namespace Sandtype;

public partial class Pawn
{
	
	private List<ITickable> _tickables = new();

	protected override void OnComponentAdded( EntityComponent component )
	{
		if ( component is ITickable t )
		{
			_tickables.Add( t );
		}
	}

	protected override void OnComponentRemoved( EntityComponent component )
	{
		if ( component is ITickable t )
		{
			_tickables.Remove( t );
		}
	}

	private void TickTickables()
	{
		foreach ( var tickable in _tickables )
		{
			tickable.Tick();
		}
	}

}
