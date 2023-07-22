using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandtype.Engine.Text;

namespace Sandtype;

public class TypingTest : EntityComponent<Pawn>
{
	
	public string[] TargetWords;
	public string[] InputWords;
	public int TypedWords;
	public List<float> AccuracyValues;
	public bool Initialized { get; set; }
	public float AverageAccuracy => AccuracyValues.Sum() / AccuracyValues.Count;
	public string CurrentInputText;
	public float FirstInputTime;
	public TextProvider Provider;

	public TypingTest()
	{
		Provider = new DictionaryFileTextProvider( "text/english_1k.json" );
		AccuracyValues = new List<float>();
	}

	public void SetTest(string provider)
	{
		Provider = new DictionaryFileTextProvider( provider );
		ResetTest();
	}

	public void ResetTest()
	{
		TargetWords = Provider.GetText();
		InputWords = new string[TargetWords.Length];
		TypedWords = 0;

		Initialized = true;

		string compiledText = "";
		foreach (var word in TargetWords)
		{
			compiledText += word;
			compiledText += " ";
		}
	}

	protected override void OnActivate()
	{
		base.OnActivate();
		ResetTest();
	}

	public void NextWord()
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
		InputWords[TypedWords] = word;
		CurrentInputText = "";
		TypedWords = CountWords();

		if ( Entity != null )
		{
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
	}

	public void SetInput(string content)
	{
		CurrentInputText = content;
	}

	public int CountWords()
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
