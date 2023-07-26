using Sandtype.Entity.Interaction;

namespace Sandtype.Entity.Pawn.Select;
using Sandbox;
using Sandbox.Component;


public class PawnSelectComponent : EntityComponent<Pawn>, ITickable
{

	private Entity _useTarget;
	private Glow _useGlow;

	private void TickPlayerUse()
	{
		// Turn prediction off
		using ( Prediction.Off() )
		{
			if ( Input.Pressed( InputButton.Use ) )
			{
				_useTarget = FindUsable();
				Log.Info( $"Using: {_useTarget}" );

				if ( _useTarget == null )
				{
					UseFail();
					return;
				}
			}

			if ( !Input.Down( InputButton.Use ) )
			{
				StopUsing();
				return;
			}

			if ( !_useTarget.IsValid() )
				return;

			// If we move too far away or something we should probably ClearUse()?

			//
			// If use returns true then we can keep using it
			//

			if ( !(_useTarget is IUse usable) )
				return;

			var interactables = _useTarget.Components.GetAll<InteractableComponent>();
			if ( usable.OnUse( Entity ) )
			{
				foreach ( var interactable in interactables )
				{
					interactable.HandleInteract( Entity.Client );
				}
			}

			StopUsing();
		}
	}

	private void TickUsableGlow(TraceResult trace)
	{
		if ( Game.IsServer ) return;
		if ( _useTarget == null && trace.Entity != null )
		{
			_useTarget = trace.Entity;
			_useGlow = trace.Entity.Components.Create<Glow>();
			_useGlow.Width = 2;
			_useGlow.Color = Color.White;
		} else if ( _useTarget != null && trace.Entity == null )
		{
			if ( _useGlow != null ) _useTarget.Components.Remove( _useGlow );
			_useGlow = null;
			_useTarget = null;
		}
	}

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

	private void UseFail()
	{
		Entity.PlaySound( "player_use_fail" );
	}

	private void StopUsing()
	{
		_useTarget = null;
	}

	public void Tick()
	{
		TickPlayerUse();
	}
}
