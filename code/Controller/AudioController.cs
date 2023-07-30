using Sandbox;

namespace TerryTyper.Controller;

public class AudioController
{

	public static float KeyVolume = 50f;
	
	public static void PlayKey(Pawn pawn)
	{
		var sound = Sound.FromScreen( To.Single(pawn), "sounds/keypress.sound" );
		sound.SetVolume( KeyVolume / 100f );
	}

}
