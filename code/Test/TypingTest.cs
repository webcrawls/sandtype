using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandtype.Text;

namespace Sandtype;

public class TypingTest
{
	
	public string[] TargetWords;
	public string[] InputWords;
	public int TypedWords;
	public int RealTypedWords { get { return _realTypedWords; } }
	public List<float> AccuracyValues;
	public bool RunsForever = true;
	public bool Initialized { get; set; }
	public float AverageAccuracy => AccuracyValues.Sum() / AccuracyValues.Count;
	public string CurrentInputText;
	public float FirstInputTime;
	public TextProvider Provider;
	public float StartTime;
	private int _realTypedWords = 0;

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
		StartTime = Time.Now;

		Initialized = true;

		string compiledText = "";
		foreach (var word in TargetWords)
		{
			compiledText += word;
			compiledText += " ";
		}
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
		_realTypedWords += 1;
		Log.Info( "typed; "+TypedWords );
		if ( (TypedWords ) == (TargetWords.Length) )
		{
			Log.Info( "Refreshing text..." );
			TargetWords = Provider.GetText();
			TypedWords = 0;
			ClearInputWords();
		}
	}

	public void SetInput(string content)
	{
		CurrentInputText = content;
	}

	public int CountWords()
	{
		int words = 0;
		for ( int i = 0; i < InputWords.Length; i++ )
		{
			if ( InputWords[i] == null )
			{
				continue;
			}

			words += 1;
		}

		return words;
	}
	
	private List<KeyValuePair<char, bool>> GetCharsTyped()
	{
		List<KeyValuePair<char, bool>> list = new List<KeyValuePair<char, bool>>();
		
		// https://stackoverflow.com/questions/24950412/how-wpm-calculate-in-typing-speed-apps
		int typed = 0;
		for ( int i = 0; i < InputWords.Length; i++ )
		{
			var word = InputWords[i];
			var target = TargetWords[i];
			if ( word == null || target == null ) break;

			for ( int c = 0; c < word.Length; c++ )
			{
				list.Add( new KeyValuePair<char, bool>( ' ', target.Length > c && target[c] == word[c] ) );
			}
		}

		return list;
	}

	public float GetWpm()
	{
		// This is actually so fucking ridiculously fucked
		// How do i do this
		// Please someone help this is beyond my capacity
		var keystrokes = GetCharsTyped();
		var timeMinutes = Time.Now - StartTime;
		var accurateCharacters = keystrokes.Where( p => p.Value );
		return keystrokes.Count / timeMinutes * (accurateCharacters.Count());
	}

	public float GetWpmPercent()
	{
		// This is so arbitrary it hurts
		return GetWpm() / 1000f;
	}

	private void ClearInputWords()
	{
		for ( int i = 0; i < InputWords.Length; i++ )
		{
			InputWords[i] = null;
		}
	}
}
