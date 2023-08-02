using System.Collections.Generic;

namespace TerryTyper;

public class PlayerData
{
	public IList<string> UnlockedThemes { get; set; } = new List<string> { "default/racer.json" };
	public string SelectedTheme { get; set; }
	public long Currency { get; set; } = 0;
	public IDictionary<string, TextTheme> Themes { get; set; } = new Dictionary<string, TextTheme>();
	public int Volume { get; set; } = 50;
}
