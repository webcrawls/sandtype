namespace Sandtype.Engine.Text;

// todo configurabletextprovider
public interface TextProvider
{

	public string GetName();
	
	public string GetText( TextSize size );

}

public enum TextSize
{
	SHORT,
	MEDIUM,
	LONG
}
