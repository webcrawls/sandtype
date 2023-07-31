using System;
using System.Collections.Generic;
using Sandbox;
using TerryTyper.Race;
using TerryTyper.UI.Text;
using TerryTyper.Util;

namespace TerryTyper.Controller;

public class DataController : EntityComponent<Pawn>, ISingletonComponent
{

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
	
	public void UnlockTheme( string theme )
	{
		var themes = new List<string>( UnlockedThemes );
		themes.Add( theme );
		UnlockedThemes = themes;
	}

	public void SelectTheme( string theme )
	{
		if ( !UnlockedThemes.Contains( theme ) ) return;
		SelectedTheme = theme;
		var race = RaceEntity.GetJoinedRace( Entity.Client.SteamId );
		if ( race != null ) RaceEntity.ThemeGameCmd( theme );
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
	
	protected override void OnActivate()
	{
		base.OnActivate();
		_saveAction = () => _saveTimer = 5;
		LoadPlayerData();
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		_saveTimer = -1;
		SavePlayerData();
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
		_steamId = Entity.Client.SteamId;
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

public class PlayerData
{
	public IList<string> UnlockedThemes {get; set;} = new List<string>{"default"};
	public string SelectedTheme {get; set;}
	public long Currency { get; set; } = 0;
	public IDictionary<string, TextTheme> Themes { get; set; } = new Dictionary<string, TextTheme>();
	public int Volume { get; set; } = 50;
}
