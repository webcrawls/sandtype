using Sandbox;
using Sandtype.Entity.Pawn;
using Sandtype.Test.World;
using Sandtype.UI.Game;
using Sandtype.UI.Hud;
using Sandtype.UI.Text;

namespace Sandtype.Test;

public class TypingTestComponent : EntityComponent<Pawn>, ISingletonComponent, ITickable
{

	private TypingTest _test;
	private TypingTestHud _hud;
	private TypingView _view;
	private TerryGame _game;
	private GameView _gameView;
	private GameWorld _gameWorld;

	protected override void OnActivate()
	{
		
		base.OnActivate();
		
		_test = new TypingTest();
		_test.ResetTest();
		_test.WordTypedAction = HandleWordTyped;

		_hud = new TypingTestHud();
		
		_view = new TypingView();
		_view.Theme = Entity.Theme;

		_game = Entity.Components.Create<TerryGame>();

		_gameView = new GameView();
		_gameView.Game = _game;
		_gameView.Test = _test;

		_gameWorld = new GameWorld( _game );
		_gameView.AddChild( _gameWorld );

		_hud.AddChild( _gameView );
		_hud.AddChild( _view );
		
		_view.AddEventListener( "onescape", HandleEscape );
		_view.Test = _test;
		
		Game.RootPanel.AddChild( _hud );
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		_hud.Delete();
	}

	private void HandleEscape()
	{
		Entity.Components.Remove( this );
	}

	public void Tick()
	{
		_gameView.BossHealthBar.ProgressValue = ((float) _game.TerryHealth / _game.TerryMaxHealth) * 100; // Remember this value sucks
	}

	private void HandleWordTyped( float accuracy )
	{
		if ( !accuracy.AlmostEqual(1f) ) return;
		_game.CreateBullet();
		Entity.PawnData = Entity.PawnData.WithWordsTyped( Entity.PawnData.WordsTyped + 1 );
	}
}

