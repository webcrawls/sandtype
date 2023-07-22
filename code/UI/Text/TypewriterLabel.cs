using Sandbox.UI;

namespace Sandtype.UI.Text;

public class TypewriterLabel : Label
{

	private string _targetText;
	private int _typewriteSpeed;
	private int _typewriteCooldown;
	private string _currentContent;
	private int _currentChar;

	public override void Tick()
	{
		TickTypewrite();
		base.Tick();
	}

	public void Typewrite( string text )
	{
		_targetText = text;
		Text = "";
	}

	private void TickTypewrite()
	{
		if ( _targetText == "" )
		{
			return;
		}
		
		if ( _typewriteCooldown > 0 )
		{
			return;
		}
		
		_typewriteCooldown = _typewriteSpeed;

		if ( _currentChar >= _currentContent.Length )
		{
			return;
		}

		Text += _targetText[_currentChar];
		_currentChar += 1;
	}
	
}
