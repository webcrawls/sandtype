using Sandbox;

namespace TerryTyper;

public class FirstPersonCamera : CameraComponent
{
	protected override void OnActivate()
	{
		base.OnActivate();
		// Set field of view to whatever the user chose in options
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );
	}
	public override void FrameSimulate( IClient cl )
	{

		var pl = Entity as Player;

		pl.EyeRotation = pl.ViewAngles.ToRotation();

		Camera.Position = pl.EyePosition;
		Camera.Rotation = pl.ViewAngles.ToRotation();

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
