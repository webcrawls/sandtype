using System.Collections.Generic;

namespace Sandtype.Text;

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
