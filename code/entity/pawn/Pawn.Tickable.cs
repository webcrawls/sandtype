using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace Sandtype.Entity.Pawn;

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
		var tickables = _tickables.Select( item => item ).ToList();
		foreach ( var tickable in tickables )
		{
			tickable.Tick();
		}
	}

}
