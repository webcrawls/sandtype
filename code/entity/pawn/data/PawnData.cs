using System;
using System.Linq;
using Sandbox;

namespace Sandtype.Entity.Pawn.data;

public class PawnData
{

	public static PawnData LoadData( long id )
	{
		string fileName = "/"+id + ".json";
		try
		{
			Log.Info( "Reading "+fileName );
			var data = FileSystem.Data.ReadJson<PawnData>( fileName );
			Log.Info( data );
			if ( data == null )
			{
				data = new PawnData();
				SaveData( id, data );
				return data;
			}
			
			return data;
		}
		catch (Exception e)
		{
			Log.Info( "Returning empty data" );
			return new PawnData();
		}
	}

	public static void SaveData( long id, PawnData data )
	{
		string fileName = id + ".json";
		FileSystem.Data.WriteJson( fileName, data );
		Log.Info( "saved to "+fileName );
	}

	public float WPM { get; set; } = 0.0f;
	public int WordsTyped { get; set; } = 0;
	public string[] UnlockedThemes { get; set; } = {"default"};

	public PawnData WithWordsTyped( int typed )
	{
		var copy = (PawnData) MemberwiseClone();
		copy.WordsTyped = typed;
		return copy;
	}
	
	public PawnData WithUnlockedTheme( string theme )
	{
		var copy = (PawnData)this.MemberwiseClone();
		var themes = copy.UnlockedThemes.ToList();
		themes.Add( theme );
		copy.UnlockedThemes = themes.ToArray();
		return copy;
	}

}
