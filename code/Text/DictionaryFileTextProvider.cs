using Sandbox;

namespace Sandtype.Text;

/// <summary>
/// Provides random words from a .json file, structured like TextDictionary.
/// </summary>
public class DictionaryFileTextProvider : DictionaryTextProvider
{
	
	public string File { get; set; }

	public DictionaryFileTextProvider( string file ) : base(FileSystem.Mounted.ReadJson<TextDictionary>( file ).Words)
	{
		File = file;
	}
}

public class TextDictionary
{
	public string Name { get; set; }
	public bool NoLazyMode { get; set; }
	public bool OrderedByFrequency { get; set; }
	public string[] Words {
		get;
		set;
	}
}
