using System.Collections.Generic;
using Sandbox;
using Sandtype.Text;

namespace Sandtype.Engine.Text;

public class DictionaryFileTextProvider : DictionaryTextProvider
{
	
	public string File { get; set; }

	public DictionaryFileTextProvider( string file ) : base(FileSystem.Mounted.ReadJson<TextDictionary>( file ).Words)
	{
		File = file;
	}
}
