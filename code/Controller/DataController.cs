using System.Collections.Generic;
using Sandbox;

namespace TerryTyper.Controller;

public class DataController : EntityComponent<Pawn>, ISingletonComponent
{

	public long Currency
	{
		get { return _data.Currency; }
		set
		{
			_data.Currency = value;
			SavePlayerData();
		}
	}

	public string SelectedTheme
	{
		get { return _data.SelectedTheme; }
		set
		{
			_data.SelectedTheme = value;
			SavePlayerData();
		}
	}

	public IList<string> UnlockedThemes
	{
		get { return _data.UnlockedThemes; }
		set
		{
			_data.UnlockedThemes = value;
			SavePlayerData();
		}
	}

	private long _steamId;
	private PlayerData _data;
	private string _filename => _steamId + ".json";

	public void UnlockTheme( string theme )
	{
		var themes = new List<string>( UnlockedThemes );
		themes.Add( theme );
		UnlockedThemes = themes;
	}

	public void Save()
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

	private void LoadPlayerData()
	{
		_steamId = Entity.Client.SteamId;
		_data = FileSystem.Data.ReadJson<PlayerData>( _filename );
		Log.Info( _data );
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
}
