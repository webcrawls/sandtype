using Sandbox;

namespace TerryTyper;

public class BattleCamera : CameraComponent
{
	private Battle _battle;
	private Entity _targetEntity;

	public BattleCamera( Battle battle )
	{
		_battle = battle;
	}
	
	public override void FrameSimulate( IClient cl )
	{
		var pl = Entity as Player;

		pl.EyeRotation = pl.ViewAngles.ToRotation();
		Camera.Position = pl.EyePosition;
		if ( _battle.ClosestEnemy != null )
		{
			Camera.Rotation = Rotation.LookAt( _battle.ClosestEnemy.Position-cl.Pawn.Position );

			if ( _targetEntity != _battle.ClosestEnemy )
			{
				if ( _targetEntity != null && _targetEntity.Components.TryGet( out GlowController oldGlow ) )
					oldGlow.Selected = false;
				_targetEntity = _battle.ClosestEnemy;
				if (_targetEntity != null && _targetEntity.Components.TryGet( out GlowController newGlow ) )
					newGlow.Selected = true;
			}
		}
		else
		{
			Camera.Rotation = Rotation.LookAt( _battle.Position-cl.Pawn.Position );;
		}
		Camera.Main.SetViewModelCamera( Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView ) );
		Camera.FirstPersonViewer = Entity;
		Camera.ZNear = 8 * pl.Scale;
	}
	public override void BuildInput()
	{
		if ( Game.LocalClient.Components.TryGet<DevCamera>( out var _ ) )
			return;

		var pl = Entity as Player;
		var viewAngles = (pl.ViewAngles + Input.AnalogLook).Normal;
		pl.ViewAngles = viewAngles.WithPitch( viewAngles.pitch.Clamp( -89f, 89f ) );
		return;
	}
}
