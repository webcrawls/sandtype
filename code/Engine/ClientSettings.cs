using System;
using System.Text.Json;
using Sandbox;

namespace Sandtype.Engine;

public class ClientSettings
{

	private static string _configFile = "sandtype_settings.json";
	
	public int TextSize { get; set; } = 42;
	public ClientSettings( int textSize )
	{
		TextSize = textSize;
	}

	
	public static void Save( ClientSettings data )
	{
		FileSystem.Data.WriteJson( _configFile, data );
	}

	public static ClientSettings Load()
	{
		try
		{
			return FileSystem.Data.ReadJson<ClientSettings>( _configFile );
		}
		catch ( Exception e )
		{
			Log.Info( e );
			Log.Info( "Retunring default" );
			return new ClientSettings(42);
		}
	}
}
