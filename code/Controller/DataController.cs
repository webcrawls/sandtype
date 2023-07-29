using System.Collections.Generic;
using Sandbox;
using TerryTyper.UI.Text;

namespace TerryTyper.Controller;

public class DataController : EntityComponent<Pawn>, ISingletonComponent
{

	public long Currency { get { return _data.Currency; } set { SetCurrency( value ); } }
	public string SelectedTheme { get { return _data.SelectedTheme; } set { SetTheme( value ); } }
	public IList<string> UnlockedThemes { get { return _data.UnlockedThemes; } set { SetUnlockedThemes( value ); } }
	public IDictionary<string, TextTheme> ThemeData { get { return _data.Themes; } set { SetThemeData( value ); } }

	private long _steamId;
	
	private PlayerData _data;
	private string _filename => _steamId + ".json";

	public void UnlockTheme( string theme )
	{
		var themes = new List<string>( UnlockedThemes );
		themes.Add( theme );
		UnlockedThemes = themes;
	}

	public void ForceSave()
	{
		SavePlayerData();
	}
	
	protected override void OnActivate()
	{
		base.OnActivate();
		LoadPlayerData();
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		SavePlayerData();
	}

	private void SetCurrency( long currency )
	{
		_data.Currency = currency;
		SavePlayerData();
	}

	private void SetTheme( string theme )
	{
		_data.SelectedTheme = theme;
	}

	private void SetUnlockedThemes( IList<string> themes )
	{
		_data.UnlockedThemes = themes;
	}

	private void SetThemeData( IDictionary<string, TextTheme> themes )
	{
		_data.Themes = themes;
	}

	private void LoadPlayerData()
	{
		_steamId = Entity.Client.SteamId;
		_data = FileSystem.Data.ReadJson<PlayerData>( _filename ) ?? new PlayerData();
		if ( !TextTheme.DefaultThemes.ContainsKey( _data.SelectedTheme ) )
		{
			_data.SelectedTheme = TextTheme.DefaultTheme.Id;
		}
	}

	private void SavePlayerData()
	{
		FileSystem.Data.WriteJson( _filename, _data );
	}
}

public class PlayerData
{
	public IList<string> UnlockedThemes {get; set;} = new List<string>{"default"};
	public string SelectedTheme {get; set;} = "default";
	public long Currency { get; set; } = 0;
	public IDictionary<string, TextTheme> Themes { get; set; } = new Dictionary<string, TextTheme>();
}
