using Sandbox;
using Sandbox.Services;
using TerryTyper.UI;

namespace TerryTyper;

/// <summary>
/// A class containing client RPC methods.
/// </summary>
public partial class Player
{
	
	[ClientRpc]
	public void ShowRaceHud()
	{
		UIController.Current.PageNavigate( "/race" );
	}

	[ClientRpc]
	public void HideRaceHud()
	{
		UIController.Current.ClosePage();
	}

	[ClientRpc]
	public void WordTyped()
	{
		Stats.Increment( "words_typed", 1 );
		DataController.Current.Currency += 1;
	}

	private BattleHud _battleHud;

	[ClientRpc]
	public void ShowBattleHud(int index)
	{
		var battle = (Battle)Entity.FindByIndex( index );
		Components.Add( new BattleCamera(battle) );
		_battleHud = new BattleHud( battle );
		UIController.Current.RootPanel.AddChild( _battleHud );
	}

	[ClientRpc]
	public void HideBattleHud()
	{
		Components.Add( new FirstPersonCamera() );
		_battleHud.Delete();
		_battleHud = null;
	}

}
