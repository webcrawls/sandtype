﻿using Sandbox;

namespace TerryTyper;

public partial class UseController : SimulatedComponent
{
	/// <summary>
	/// Entity the player is currently using via their interaction key.
	/// </summary>
	public Entity Using { get; protected set; }

	private Entity _hoveredEntity;
	private GlowController _usedGlow;
	
	public override void Simulate( IClient cl )
	{

		// Turn prediction off
		using ( Prediction.Off() )
		{
			Using = FindUsable();

			if ( _usedGlow == null && Using != null )
			{
				_hoveredEntity = Using;
				if ( _hoveredEntity.Components.TryGet( out _usedGlow ) )
				{
					_usedGlow.Selected = true;
				}
			} else if ( Using == null && _usedGlow != null )
			{
				_usedGlow.Selected = false;
				_usedGlow = null;
			}
			
			// This is serverside only
			if ( !Game.IsServer ) return;

			if ( Input.Pressed( "Use" ) )
			{
				if ( Using == null )
				{
					UseFail();
					return;
				}
			}

			if ( !Input.Down( "Use" ) )
			{
				StopUsing();
				return;
			}

			if ( !Using.IsValid() )
				return;

			// If we move too far away or something we should probably ClearUse()?

			//
			// If use returns true then we can keep using it
			//
			if ( Using is IUse use && use.OnUse( Entity ) )
				return;

			StopUsing();
		}
	}

	/// <summary>
	/// Player tried to use something but there was nothing there.
	/// Tradition is to give a disappointed boop.
	/// </summary>
	protected virtual void UseFail()
	{
		Entity.PlaySound( "player_use_fail" );
	}

	/// <summary>
	/// If we're using an entity, stop using it
	/// </summary>
	protected virtual void StopUsing()
	{
		Using = null;
	}

	/// <summary>
	/// Returns if the entity is a valid usable entity
	/// </summary>
	protected bool IsValidUseEntity( Entity e )
	{
		if ( e == null ) return false;
		if ( e is not IUse use ) return false;
		if ( !use.IsUsable( Entity ) ) return false;

		return true;
	}

	/// <summary>
	/// Find a usable entity for this player to use
	/// </summary>
	protected virtual Entity FindUsable()
	{
		int distance = 650;
		
		// First try a direct 0 width line
		var tr = Trace.Ray( Entity.EyePosition, Entity.EyePosition + Entity.EyeRotation.Forward * distance )
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
			tr = Trace.Ray( Entity.EyePosition, Entity.EyePosition + Entity.EyeRotation.Forward * distance )
			.Radius( 5 )
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
}
