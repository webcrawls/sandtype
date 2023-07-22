using System.Collections.Generic;
using Sandtype.Engine.Text;
using Sandtype.UI;
using Sandtype.UI.Game;

namespace Sandtype.Terry.Action;

public class LanguageChangeAction : TerryAction
{

	private List<string> languages = new()
	{
		"text/english_1k.json",
		"text/english_25k.json",
		"text/english_450k.json",
		"text/code_javascript_1k.json",
		"text/code_csharp.json",
		"text/code_python_1k.json",
	}; 
	
	public LanguageChangeAction(
		Pawn pawn, TerryGame game, TypingTest test, Hud hud ) : base( pawn, game, test, hud, "lang", 0.5f, 0.2f )
	{
		
	}

	public override void Run()
	{
		base.Run();
		Test.Provider = new DictionaryFileTextProvider( languages.RandomElementByWeight( s => 1 ) );
	}
}
