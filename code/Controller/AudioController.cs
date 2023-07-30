using System;
using System.Collections.Generic;
using Sandbox;
using TerryTyper.UI.Text;

namespace TerryTyper.Controller;

public class AudioController : EntityComponent<Pawn>
{
	private const string DefaultSoundAsset = "sounds/keypress.sound";

	public int Volume
	{
		get { return _volume; }
		set
		{
			_data.SavedVolume = value;
			_volume = value;
		}
	}

	private DataController _data;
	private int _volume = 0;

	protected override void OnActivate()
	{
		base.OnActivate();
		_data = TyperGame.Entity.GamePawn.Data;
		if ( _data == null )
		{
			throw new Exception( "Audio should be initialized after Data!" );
		}
		Volume = _data.SavedVolume;
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		_data.SavedVolume = Volume;
	}

	public void PlayKey( Pawn pawn )
	{
		var sound = Sound.FromScreen( To.Single( pawn ), GetSound() );
		sound.SetVolume( Volume / 100f );
	}

	private string GetSound()
	{
		var theme = TextTheme.DefaultThemes[_data.SelectedTheme];
		if ( string.IsNullOrEmpty(theme.Sound) )
		{
			return DefaultSoundAsset;
		}

		return theme.Sound;
	}
	

}
