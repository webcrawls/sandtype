using Sandbox;

namespace TerryTyper;

public class AudioController : Entity
{

	public static AudioController Current { get; private set; }
	private const string DefaultSoundAsset = "sounds/keypress.sound";

	public int Volume
	{
		get { return _volume; }
		set
		{
			DataController.Current.SavedVolume = value;
			_volume = value;
		}
	}

	private int _volume = 0;

	public AudioController()
	{
		Current = this;
	}

	public override void Spawn()
	{
		base.Spawn();
		Volume = DataController.Current.SavedVolume;
	}

	public override void OnKilled()
	{
		base.OnKilled();
		DataController.Current.SavedVolume = Volume;
	}

	public void PlayKey()
	{
		var sound = Sound.FromScreen( To.Single( Game.LocalClient ), GetSound() );
		sound.SetVolume( Volume / 100f );
	}

	private string GetSound()
	{
		var theme = TextTheme.Themes[DataController.Current.SelectedTheme];
		if ( string.IsNullOrEmpty(theme.Sound) )
		{
			return DefaultSoundAsset;
		}

		return theme.Sound;
	}
	

}
