using Sandbox;

namespace TerryTyper;

public class DictionaryFileTextProvider : DictionaryTextProvider
{

	public override string Id { get; }

	public DictionaryFileTextProvider( string file ) : base(FileSystem.Mounted.ReadJson<TextDictionary>( file ).Words)
	{
		Id = file;
	}
}

class TextDictionary
{
	public string Name { get; set; }
	public bool NoLazyMode { get; set; }
	public bool OrderedByFrequency { get; set; }
	public string[] Words {
		get;
		set;
	}
}
