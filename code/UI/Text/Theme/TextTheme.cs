using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Editor;
using Microsoft.VisualBasic;
using FileSystem = Sandbox.FileSystem;

namespace TerryTyper.UI.Text;

public class TextTheme
{

	public const string FEATURE_CURSOR_IMAGE = "FEATURE_CURSOR_IMG";
	public static readonly IDictionary<string, TextTheme> Themes = LoadThemeDictionary();
	public static TextTheme DefaultTheme => Themes["Default/racer"];
	private static IDictionary<string, TextTheme> LoadThemeDictionary()
	{
		var folder = "/UI/Text/Theme/Themes";
		var files = FileSystem.Mounted.FindFile(folder, "*.json", true );
		IDictionary<string, TextTheme> result = new Dictionary<string, TextTheme>();
		foreach (var filename in files)
		{
			var id = filename.Replace( ".json", "" );
			var theme = FileSystem.Mounted.ReadJson<TextTheme>( folder + "/" + filename );
			theme.Id = id;
			theme.Features ??= new List<string>();
			result[id] = theme;
		}
		
		Log.Info( String.Join( ", ", result.Keys) );

		return result.ToImmutableSortedDictionary( StringComparer.CurrentCulture );
	}

	public string Id {get; set;}
	public string Name {get; set;}
	public string Stylesheet {get; set;}
	public string Sound {get; set;}
	public string Description {get; set;}
	public string Rarity {get; set;}
	public string Icon {get; set;}
	public List<string> Features {get; set;}
}
