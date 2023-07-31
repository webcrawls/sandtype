using System.Collections.Generic;
using Sandbox;
using Sandtype.Text;

namespace Sandtype.Engine.Text;

public class DictionaryFileTextProvider : DictionaryTextProvider
{

	public override string Id { get; }

	public DictionaryFileTextProvider( string file ) : base(FileSystem.Mounted.ReadJson<TextDictionary>( file ).Words)
	{
		Id = file;
	}
}
