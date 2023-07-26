namespace Sandtype.Entity.Interaction;
using Pawn.Hud;
using Sandbox;

public class TextDialogGiver : InteractableComponent
{

	public string Text = "hello, player. welcome to hell.";
	
	public TextDialogGiver() {}

	public TextDialogGiver( string text )
	{
		Text = text;
	}

	protected override void OnInteract( IClient cl )
	{
		base.OnInteract( cl );
		var text = new TextDialogComponent();
		text.Text = Text;
		cl.Pawn.Components.Add( text );
	}
}
