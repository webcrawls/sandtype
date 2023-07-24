using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Sandtype.UI.Theme;

public class Theme
{

	public static IDictionary<string, Theme> Themes = new Dictionary<string, Theme>
	{
		{ "default", new Theme { ColorMain = "#99b898",
			ColorTyped = "#fecea8",
			ColorError = "#ff6961",
			ColorCaret = "#616161",
			ColorBackground = "#292929" } },
		{ "pastel", new Theme { ColorMain = "#d8bfd8",
			ColorTyped = "#74569b",
			ColorError = " #f7ffae ",
			ColorCaret = "  #96fbc7 ",
			ColorBackground = "#74569b" } },
		{ "red", new Theme {
			ColorMain = "#ee243d",
			ColorTyped = "#af2747",
			ColorError = "#6b2341",
			ColorCaret = "#281a2d",
			ColorBackground = "#0d101b",
		} }
	};
	
	public string ColorMain = "";
	public string ColorTyped = "";
	public string ColorError = "";
	public string ColorCaret = "";
	public string ColorBackground = "";
}
