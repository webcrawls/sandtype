using Sandbox.UI;

namespace Sandtype.UI.GameMenu;

public class TextEntryDropDown : DropDown
{

	public TextEntryDropDown() : base()
	{
		Options.Add( new Option("English 1k", "text/english_1k.json") );
		Options.Add( new Option("English 10k", "text/english_10k.json") );
		Options.Add( new Option("English 25k", "text/english_10k.json") );
		Options.Add( new Option("English 450k", "text/english_450k.json") );
		Options.Add( new Option("C#", "text/code_csharp.json") );
		Selected = Options[0];
	}
	
}
