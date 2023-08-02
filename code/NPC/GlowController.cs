using Sandbox;
using Sandbox.Component;

namespace TerryTyper;

/// <summary>
/// Provides entity glow functionality, allowing you to 
/// </summary>
public class GlowController : EntityComponent<AnimatedEntity>
{
	
	/// <summary>
	/// The default glow colour to show.
	/// </summary>
	public Color DefaultColor { get; set; } = Color.White;
	
	/// <summary>
	/// The colour to show when this entity is selected.
	/// </summary>
	public Color SelectColor { get; set; } = Color.Blue;

	/// <summary>
	/// The default glow's width.
	/// </summary>
	public int DefaultWidth { get; set; } = 1;
	
	/// <summary>
	/// The select glow's width.
	/// </summary>
	public int SelectWidth { get; set; } = 2;

	/// <summary>
	/// Set this to a value to change the displayed glow.
	/// </summary>
	public bool Selected
	{
		get { return _selected; }
		set
		{
			_selected = value;
			Update();
		}
	}

	private bool _selected = false;
	private Glow _glowRef;

	protected override void OnActivate()
	{
		base.OnActivate();
		_glowRef = Entity.Components.GetOrCreate<Glow>();
		Update();
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		_glowRef.Remove();
	}

	private void Update()
	{
		if ( _selected )
		{
			_glowRef.Color = SelectColor;
			_glowRef.Width = SelectWidth;
			Log.Info( "Updating" );
		}
		else
		{
			_glowRef.Color = DefaultColor;
			_glowRef.Width = DefaultWidth;
		}
	}


}
