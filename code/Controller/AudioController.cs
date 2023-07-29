using Sandbox;

namespace TerryTyper.Controller;

public class AudioController 
{

	public static void PlayKey(Pawn pawn)
	{
			var sound = Sound.FromScreen( To.Single(pawn), "sounds/keypress.sound" );
	}

}
