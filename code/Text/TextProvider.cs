using System.Collections.Generic;

namespace Sandtype.Engine.Text;

// todo configurabletextprovider
public interface TextProvider
{


	public string[] GetText( );

}

public enum TextSize
{
	SHORT,
	MEDIUM,
	LONG
}
