using Sandbox.UI;

namespace Sandtype.UI.Text;

public class TextEntryDropDown : DropDown
{

	public TextEntryDropDown() : base()
	{
		Options.Add( new Option("English 1k", "text/english_1k.json") );
		Options.Add( new Option("English 10k", "text/english_10k.json") );
		Options.Add( new Option("English 25k", "text/english_10k.json") );
		Options.Add( new Option("English 450k", "text/english_450k.json") );
		Options.Add( new Option("French", "text/french.json") );
		Options.Add( new Option("C#", "text/code_csharp.json") );
		Options.Add( new Option("JavaScript 1k", "text/code_javascript_1k.json") );
		Options.Add( new Option("Python 1k", "text/code_python_1k.json") );
		Selected = Options[0];
	}
	
}
