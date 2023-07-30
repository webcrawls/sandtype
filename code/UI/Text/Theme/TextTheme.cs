using System.Collections.Generic;
using Editor;

namespace TerryTyper.UI.Text;

public class TextTheme
{

	public static IDictionary<string, TextTheme> DefaultThemes { get; private set; } =
		new Dictionary<string, TextTheme>
		{
			{
				"typeracer",
				new TextTheme()
				{
					Id = "typeracer",
					Cost = 5,
					Name = "TypeRacer",
					Stylesheet = "/UI/Text/Styles/TypeRacer.Theme.scss"
				}
			},
			{
				"typeracerdark",
				new TextTheme()
				{
					Id = "typeracerdark",
					Cost = 5,
					Name = "TypeRacer Dark",
					Stylesheet = "/UI/Text/Styles/TypeRacerDark.Theme.scss"
				}
			},
			{
				"pastel",
				new TextTheme()
				{
					Id = "pastel",
					Cost = 5,
					Name = "Pastel",
					Stylesheet = "/UI/Text/Styles/Pastel.Theme.scss"
				}
			},
			{
				"obamium",
				new TextTheme()
				{
					Id = "obamium",
					Cost = 5,
					Name = "Obamium",
					Stylesheet = "/UI/Text/Styles/Obamium.Theme.scss"
				}
			},
			{
				"myahoo",
				new TextTheme()
				{
					Id = "myahoo",
					Cost = 100,
					Name = "Myahoo",
					Stylesheet = "/UI/Text/Styles/Myahoo.Theme.scss",
					Sound = "sounds/myahoo/myahoo.sound"
				}
			}

		};

	public static TextTheme DefaultTheme => DefaultThemes["typeracer"];

	public int Cost = 5;
	public string Id = "typeracer";
	public string Name;
	public string Stylesheet;
	public string Sound = "sounds/keypress.sound";
}
