using System;
using System.Collections.Generic;
using Sandbox;

namespace TerryTyper;

public class DataController : Entity
{

	public static DataController Current { get; private set; }
	
	public int SavedVolume { get { return _data.Volume; } set { SetVolume( value ); } }
	public long Currency { get { return _data.Currency; } set { SetCurrency( value ); } }
	public string SelectedTheme { get { return _data.SelectedTheme; } set { SetTheme( value ); } }
	public IList<string> UnlockedThemes { get { return _data.UnlockedThemes; } set { SetUnlockedThemes( value ); } }
	public IDictionary<string, TextTheme> ThemeData { get { return _data.Themes; } set { SetThemeData( value ); } }

	private long _steamId;
	private PlayerData _data;
	private int _saveTimer = -1;
	private Action _saveAction;
	private string _filename => _steamId + ".json";

	public DataController()
	{
		Current = this;
	}

	public override void Spawn()
	{
		base.Spawn();
		_saveAction = () => _saveTimer = 5;
		LoadPlayerData();
	}

	public override void OnKilled()
	{
		base.OnKilled();
		_saveAction = () => _saveTimer = 5;
		LoadPlayerData();
	}

	public void UnlockTheme( string theme )
	{
		var themes = new List<string>( UnlockedThemes );
		if ( themes.Contains( theme ) ) return;
		themes.Add( theme );
		UnlockedThemes = themes;
	}

	public void SelectTheme( string theme )
	{
		if ( !UnlockedThemes.Contains( theme ) ) return;
		SelectedTheme = theme;
		var race = Race.GetJoinedRace( Game.SteamId );
		if ( race != null ) Race.ThemeGameCmd( theme );
	}

	public void ForceSave()
	{
		SavePlayerData();
	}

	[GameEvent.Tick.Client]
	private void Tick()
	{
		// a simple sort of debounce logic
		if ( _saveTimer == -1 ) return;
		if ( _saveTimer == 0 ) SavePlayerData();
		if ( _saveTimer > 0 ) _saveTimer -= 1;
	}
	
	private void SetCurrency( long currency )
	{
		_data.Currency = currency;
		_saveAction.Invoke();
	}

	private void SetTheme( string theme )
	{
		_data.SelectedTheme = theme;
		_saveAction.Invoke();
	}

	private void SetVolume( int volume )
	{
		_data.Volume = volume;
		_saveAction.Invoke();
	}

	private void SetUnlockedThemes( IList<string> themes )
	{
		_data.UnlockedThemes = themes;
		_saveAction.Invoke();
	}

	private void SetThemeData( IDictionary<string, TextTheme> themes )
	{
		_data.Themes = themes;
		_saveAction.Invoke();
	}

	private void LoadPlayerData()
	{
		_steamId = Game.SteamId;
		_data = FileSystem.Data.ReadJson<PlayerData>( _filename ) ?? new PlayerData();
		_data.SelectedTheme ??= TextTheme.DefaultTheme.Id;
		if ( !TextTheme.Themes.ContainsKey( _data.SelectedTheme ) )
		{
			_data.SelectedTheme = TextTheme.DefaultTheme.Id;
		}
	}

	private void SavePlayerData()
	{
		FileSystem.Data.WriteJson( _filename, _data );
		_saveTimer = -1;
	}
}
