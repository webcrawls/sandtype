namespace Sandtype.Entity.Pawn.Select;
using Sandbox;
using Sandbox.Component;

/// <summary>
/// Applies a client-side Glow component to any entities being hovered.
/// </summary>
public class PawnSelectGlowComponent : EntityComponent<Pawn>, ITickable
{

	public Color SelectColor = Color.White;
	public Color UsingColor = Color.Blue;
	public int SelectWidth = 1;
	public int UsingWidth = 1;

	private Entity _useTarget;
	private Glow _useGlow;

	private Entity FindUsable()
	{
		// First try a direct 0 width line
		var eyePosition = Entity.EyePosition;
		var forward = Entity.EyeRotation.Forward;
		var tr = Trace.Ray( eyePosition, eyePosition + forward * 85 )
			.Ignore( Entity )
			.Run();

		// See if any of the parent entities are usable if we ain't.
		var ent = tr.Entity;
		while ( ent.IsValid() && !IsValidUseEntity( ent ) )
		{
			ent = ent.Parent;
		}

		// Nothing found, try a wider search
		if ( !IsValidUseEntity( ent ) )
		{
			tr = Trace.Ray( eyePosition, eyePosition + forward * 85 )
				.Radius( 2 )
				.Ignore( Entity )
				.Run();

			// See if any of the parent entities are usable if we ain't.
			ent = tr.Entity;
			while ( ent.IsValid() && !IsValidUseEntity( ent ) )
			{
				ent = ent.Parent;
			}
		}

		// Still no good? Bail.
		if ( !IsValidUseEntity( ent ) ) return null;

		return ent;
	}
	
	private bool IsValidUseEntity( Entity e )
	{
		if ( e == null ) return false;
		if ( e is not IUse use ) return false;
		if ( !use.IsUsable( Entity ) ) return false;

		return true;
	}

	public void Tick()
	{
		var foundTarget = FindUsable();

		if ( foundTarget != null && foundTarget == _useTarget )
		{
			// we already found current target, don't update
		}
		else if ( foundTarget != null && foundTarget != _useTarget )
		{
			if ( _useGlow != null ) _useTarget.Components.Remove( _useGlow );
			_useTarget = foundTarget;
			_useGlow = _useTarget.Components.Create<Glow>();
			_useGlow.Color = SelectColor;
			_useGlow.Width = SelectWidth;
		}
		else if ( foundTarget == null && _useTarget == null )
		{
			// also return we dont gotta do anything
		}
		else if ( foundTarget == null && _useTarget != null )
		{
			if ( _useGlow != null ) _useTarget.Components.Remove( _useGlow );
			_useTarget = null;
			_useGlow = null;
		} 
	}
}
