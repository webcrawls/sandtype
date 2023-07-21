using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sandbox;
using Sandbox.Internal;
using Sandtype.Engine.Text;
using Sandtype.UI.Text;

namespace Sandtype;

public class TypingTest : EntityComponent<Pawn>
{
	
	public string[] TargetWords;
	public string[] InputWords;
	public int TypedWords { get { return _typedWords; } }
	public List<float> AccuracyValues;
	public bool Initialized { get; set; }
	public float AverageAccuracy => AccuracyValues.Sum() / AccuracyValues.Count;
	public string CurrentInputText;
	public float FirstInputTime;
	public TextProvider Provider;
	private int _typedWords = 0;

	public TypingTest()
	{
		Provider = new DictionaryFileTextProvider( "text/english_1k.json" );
		AccuracyValues = new List<float>();
	}

	public void Reset()
	{
		if ( Entity.Hud.TypingView == null || Entity.Hud.TypingView.Input == null)
		{
			return;
		}
		
		TargetWords = Provider.GetText();
		InputWords = new string[TargetWords.Length];

		Entity.Hud.Test = this;
		Entity.Hud.TypingView.Input.Text = "";
		Entity.Hud.TypingView.Input.AddEventListener( "onspace" , CheckWord);
		Entity.Hud.TypingView.Input.AddEventListener( "ontab", Reset );
		Entity.Hud.TypingView.Input.AddEventListener( "onchanged", ReadInput );
		Initialized = true;

		string compiledText = "";
		foreach (var word in TargetWords)
		{
			compiledText += word;
			compiledText += " ";
		}
	}

	public void Simulate()
	{
		UpdateHud();
	}

	
	
	protected override void OnActivate()
	{
		base.OnActivate();
		Reset();
	}

	private void UpdateHud()
	{
		if ( !Game.IsClient )
		{
			return;
		}

		if ( Entity.Hud.Test == null )
		{
			Entity.Hud.Test = this;
		}
		
		Entity.Hud.Accuracy = AverageAccuracy;
	}
	
	public void CheckWord()
	{
		var word = CurrentInputText;
		
		// csharp might cringe
		var targetWord = TargetWords[TypedWords];

		int accurateCharacters = 0;

		for ( int i = 0; i < targetWord.Length; i++ )
		{
			if ( word.Length <= i )
			{
				break;
			}

			if ( targetWord[i] == word[i] )
			{
				accurateCharacters += 1;
			}
		}

		var extraLength = word.Length - targetWord.Length;
		var extraInaccuracy = extraLength == 0 ? 0 : (extraLength / targetWord.Length);
		var accuracy = (accurateCharacters / targetWord.Length);

		AccuracyValues.Add( accuracy );
		InputWords[_typedWords] = word;
		CurrentInputText = "";
		Entity.Hud.TypingView.Input.Text = "";
		_typedWords = CountWords();

		var game = Entity.Components.Get<TerryGame>();
		if ( game != null )
		{
			if ( accuracy == 1 )
			{
				Log.Info( "Accuracy: "+accuracy+$", word: {word}, targetWord: {targetWord}, {extraInaccuracy}" );
				game.CreateBullet();
			}
		}
	}

	private void ReadInput()
	{
		CurrentInputText = Entity.Hud.TypingView.Input.Text;
	}

	private int CountWords()
	{
		for ( int i = 0; i < InputWords.Length; i++ )
		{
			if ( InputWords[i] == null )
			{
				return i;
			}	
		}

		return 0;
	}
}
