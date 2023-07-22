using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandtype.Terry;
using Sandtype.Terry.Action;
using Sandtype.UI.Game;

namespace Sandtype;

public class TerryGame : EntityComponent<Pawn>
{

	public int TerryMaxHealth = 10;
	public int TerryHealth = 10;
	public int PlayerMaxHealth = 10;
	public int PlayerHealth = 10;
	public float TerryAnger { get { return _terryAnger;  } }
	public int ActionCooldown { get { return _actionTimer; } }
	public bool Paused { get { return _hasEnded; } }
	
	public List<GameEntity> Entities;

	public float TerryKillTime;

	private GameWorld _gameWorld;

	private GameEntity _closestBullet = null;
	private GameEntity _closestEnemy = null;
	private Queue<GameEntity> _terryDeleteQueue; // not sure if this is the right place to use a queue but it feels good so
	private bool _hasEnded = false;

	private float _terryAnger = 0.0f;
	private IDictionary<string, TerryAction> _actions;
	private int _actionCooldown = 10;
	private int _actionTimer;
	private TypingTest _test;

	protected override void OnActivate()
	{
		base.OnActivate();

		if ( Game.IsClient )
		{
			Entity.Hud.Game = this;
		}

		_test = Entity.Components.Get<TypingTest>();
		_actions = new Dictionary<string, TerryAction>();
		_terryDeleteQueue = new Queue<GameEntity>();
		Entities = new List<GameEntity>(); // 50 max entities?
		
		RegisterAction( new LayoutFlipTerryAction( Entity, this, _test, Entity.Hud ) );
		RegisterAction( new TerrySpawnAction( Entity, this, _test, Entity.Hud ) );
		RegisterAction( new LanguageChangeAction( Entity, this, _test, Entity.Hud ) );
	}

	public void Simulate()
	{
		_gameWorld = Entity?.Hud?.GameView?.World;
		if ( Entity != null && Entity.Hud != null && Entity.Hud.Game == null )
		{
			Entity.Hud.Game = this;
		}

		
		// would love to stop being stupid about this ;-)
		if ( Entity != null && Entity.Hud != null && Entity.Hud.GameView != null && Entity.Hud.GameView.Boss != null )
		{
			Entity.Hud.GameView.Boss.TerryHealth = TerryHealth;
			Entity.Hud.GameView.Boss.TerryMax = TerryMaxHealth;
			Entity.Hud.GameView.BossHealthBar.ProgressValue = ((float) TerryHealth / TerryMaxHealth) * 100; // Remember this value sucks
		}

		LoadWorld();
		_gameWorld?.Tick();

		if ( _hasEnded )
		{
			return;
		}

		_terryAnger = 1.0f - (float) TerryHealth / TerryMaxHealth;
		Log.Info( "_terryAnger: "+_terryAnger );
		
		TickEntities();
		TickActions();
	}

	private int FindUnusedId()
	{
		int currentId = 0;
		while ( GetEntity( currentId ) != null )
		{
			currentId += 1;
		}

		return currentId;
	}

	public void DeleteEntity( int id )
	{
		var entity = GetEntity( id );
		if ( entity != null )
		{
			_terryDeleteQueue.Enqueue( entity );
		}
	}

	public GameEntity? GetEntity( int id )
	{
		foreach ( var entity in Entities )
		{
			if ( entity.EntityId == id )
			{
				return entity;
			}
		}

		return null;
	}

	public void CreateTerry()
	{
		Entities.Add( new TerryBot(FindUnusedId()) );
	}

	public void CreateBullet()
	{
		Entities.Add( new Bullet( FindUnusedId() ) );
	}


	private void TickBullet( Bullet bullet )
	{
		if ( _closestBullet == null )
		{
			_closestBullet = bullet;
		}
		else if ( bullet.Progress > _closestBullet.Progress )
		{
			_closestBullet = bullet;
		}
				
		var progress = 1.0 - bullet.Progress; // todo (1.0 - ...) will fuck me in the future :D
		var closestProgress = _closestEnemy?.Progress ?? 0.0;
		var delta = progress - closestProgress;
		var range = 0.1;

		if ( delta <= range && delta >= -range )
		{
			_terryDeleteQueue.Enqueue( bullet );
			if ( _closestEnemy == null )
			{
				HandleBossHit( bullet );
			}
			else
			{
				HandleEnemyHit( bullet, _closestEnemy );
			}
		}

	}

	private void TickTerry( TerryBot terry )
	{
		if ( _closestEnemy == null )
		{
			_closestEnemy = terry;
		}
		else if ( terry.Progress > _closestEnemy.Progress )
		{
			_closestEnemy = terry;
		}

		if ( terry.Progress > 1 )
		{
			HandleEnemyReached( terry );
		}
	
	}
	
	private void TickEntities()
	{
		for ( int i = 0; i < Entities.Count; i++ )
		{
			var entity = Entities[i];
			entity.Think();

			if ( entity is Bullet bullet)
			{
				TickBullet( bullet );
			}
			else if (entity is TerryBot terry)
			{
				TickTerry( terry );
			}
			
			if ( entity.Progress > 1 )
			{
				_terryDeleteQueue.Enqueue( entity );
			}
		}

		while ( _terryDeleteQueue.TryDequeue( out GameEntity entity ) )
		{
			if ( entity == _closestEnemy ) _closestEnemy = null;
			if ( entity == _closestBullet ) _closestBullet = null;
			
			Entities.Remove(entity);
		}
		
		// safety reset
		if ( EnemyCount() == 0 )
		{
			_closestEnemy = null;
		}
		
	}

	private void LoadWorld()
	{
		if ( _gameWorld == null && Entity?.Hud != null && Entity?.Hud?.GameView != null )
		{
			_gameWorld = Entity.Hud.GameView.World;
		} 
	}
	
	private void HandleEnd(GameEntity gameEntity)
	{
		Entities.Remove( gameEntity );
	}

	private void HandleEnemyHit( GameEntity bullet, GameEntity enemy )
	{
		_terryDeleteQueue.Enqueue( bullet );
		_terryDeleteQueue.Enqueue( enemy );
	}

	private void HandleBossHit( GameEntity bullet )
	{
		TerryHealth -= 1;

		if ( TerryHealth <= 0 )
		{
			_hasEnded = true;
		}
		
		_terryDeleteQueue.Enqueue( bullet );
	}

	private void HandleEnemyReached( GameEntity enemy )
	{
		_terryDeleteQueue.Enqueue( enemy );
		PlayerHealth -= 1;

		if ( PlayerHealth <= 0 )
		{
			_hasEnded = true;
		}
	}

	private int EnemyCount()
	{
		int i = 0;
		foreach ( var e in Entities )
		{
			if ( e is TerryBot ) i += 1;
		}

		return i;
	}

	private void TickActions()
	{
		if ( _actionTimer > 0 )
		{
			_actionTimer -= 1;
			return;
		}

		_actionTimer = (int) Math.Round(((1 - _terryAnger) * 100) + _actionCooldown);
		Log.Info( 1 - _terryAnger );
		if ( _actions.Count == 0 )
		{
			return;
		}

		var action = _actions.RandomElementByWeight( p => p.Value.Chance );
		action.Value.Run();
	}

	private void RegisterAction( TerryAction terryAction )
	{
		_actions[terryAction.Id] = terryAction;
	}
}
	
	// ideas:
	// health terrys can be green
	// some terrys are bigger and have more health
	// some terrys are small and move fast
