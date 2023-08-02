using System.Collections.Generic;

namespace TerryTyper;

// todo configurabletextprovider
public interface TextProvider
{

	public static IDictionary<string, TextProvider> Providers = new Dictionary<string, TextProvider>()
	{
		{"English", new DictionaryFileTextProvider( "text/english_1k.json" )},
		{"English 5k", new DictionaryFileTextProvider( "text/english_5k.json" )},
		{"English 10k", new DictionaryFileTextProvider( "text/english_10k.json" )},
		{"English 25k", new DictionaryFileTextProvider( "text/english_25k.json" )},
		{"English 450k", new DictionaryFileTextProvider( "text/english_450k.json" )},
		{"French", new DictionaryFileTextProvider( "text/french.json" )},
		{"Python 1k", new DictionaryFileTextProvider( "text/code_python_1k.json" )},
		{"JavaScript 1k", new DictionaryFileTextProvider( "text/code_javascript_1k.json" )},
		{"C#", new DictionaryFileTextProvider( "text/code_csharp.json" )},
	};


	public string[] GetText( int length = 20 );
	
	public string Id { get; }

}

public enum TextSize
{
	SHORT,
	MEDIUM,
	LONG
}
