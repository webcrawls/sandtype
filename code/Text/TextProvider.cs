namespace Sandtype.Text;

/// <summary>
/// Provides an array of text from a source.
/// </summary>
public interface TextProvider
{
	
	/// <summary>
	/// Sets the size of the text provider. May be ignored.
	/// </summary>
	/// <param name="size">The desired size of the array returned by GetText();</param>
	public void SetSize( int size );

	/// <summary>
	/// Generates an array of text and returns it. Depending on the provider, this may be random every time.
	/// </summary>
	/// <returns>An array of text.</returns>
	public string[] GetText( );

}


