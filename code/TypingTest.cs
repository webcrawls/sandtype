using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Sandbox;
using Sandtype.Engine.Text;
using Sandtype.UI.Game;
using Sandtype.UI.Text;

namespace Sandtype;

public class TypingTest : EntityComponent<Pawn>
{

	public List<string> TargetWords;
	public List<string> InputWords;
	public string CurrentInputText;
	public float FirstInputTime;
	private TextInput _input;
	private TypingView _typingView;
	private bool _initialized;

	public void Reset()
	{
		TargetWords = new DictionaryTextProvider( new List<string>(){"hello", "world"} )
			.GetText( TextSize.LONG )
			.Split( " " )
			.ToList();
		InputWords = new List<string>();
	}

	public void Simulate()
	{
		CurrentInputText = _input?.Text;
		if ( _typingView != null && _typingView.Test == null )
		{
			_typingView.Test = this;
		}

		if ( _input?.Text == " " )
		{
			_input.Text = "";
		}
	}

	protected override void OnActivate()
	{
		base.OnActivate();
		HookHud();
		Reset();
	}

	private void HookHud()
	{
		if ( Game.IsClient )
		{
			Entity.Hud.Test = this;
			_typingView = Entity.Hud.TypingView;
			_input = Entity.Hud.GameView.Input;
			_input.EnterAction = () => { };
			_input.TabAction = () => { };
			_input.ContentChangedAction = () =>
			{
				if ( FirstInputTime == null )
				{
					FirstInputTime = Time.Now;
				}
			};
			_input.WordCompleteAction = CompleteWord;
			
			Entity.Hud.GameView.World.TerryHitAction = Entity.Components.Get<TerryGame>().HandleTerryHit;
			Entity.Hud.GameView.World.BulletHitAction = Entity.Components.Get<TerryGame>().HandleBulletHit;
			Entity.Hud.GameView.Input.EnterAction = Entity.Hud.GameView.World.CreateTerry;
		}
	}

	public void CompleteWord()
	{
		InputWords.Add( CurrentInputText );
		_input.Text = "";
		CurrentInputText = "";
		Entity.Hud.GameView.World.CreateBullet();
	}
	
}
