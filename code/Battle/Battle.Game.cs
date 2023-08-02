using System;
using System.Collections.Generic;
using Sandbox;

namespace TerryTyper;

public partial class Battle
{
	
	[GameEvent.Tick.Server]
	public void OnServerTick()
	{
		TickEnemies();
	}

	
	private void TickEnemies()
	{
		if ( Enemies.Count == 0 )
		{
			// cleanup enemy stuff
			if ( ClosestEnemy != null ) ClosestEnemy = null;
		}
		else
		{
			var toRemove = new Queue<BattleEnemy>();
			var closest = ClosestEnemy;
		
			foreach ( var enemy in Enemies )
			{
				enemy.Progress += enemy.Speed * SpeedMultiplier;
				if ( enemy.Progress >= 1.0f )
				{
					toRemove.Enqueue( enemy );
				}
				else if (ClosestEnemy == null)
				{
					closest = enemy;
				} else if ( enemy.Progress > closest.Progress)
				{
					closest = enemy;
				}
			}

			ClosestEnemy = closest;

			while ( toRemove.TryDequeue( out var enemy ) )
			{
				enemy.Delete();
			}
		}

		if ( _spawnCountdown > 0 )
		{
			_spawnCountdown -= 1;
		}
		else
		{
			_spawnCountdown = SPAWN_TIME;
			SpawnEnemy();
		}
	}

	/// <summary>
	/// Spawns an enemy.
	/// </summary>
	private void SpawnEnemy()
	{
		var enemy = CreateEnemy();
		var enemies = new List<BattleEnemy>( Enemies ) { enemy };
		Enemies = enemies;
	}
	
	/// <summary>
	/// Creates an enemy.
	/// </summary>
	private BattleEnemy CreateEnemy()
	{
		int randomY = Random.Shared.Next( 500, 1000 ) * Random.Shared.Next(2) == 1 ? 1 : -1;
		int randomX = Random.Shared.Next( 500, 1000 ) * Random.Shared.Next(2) == 1 ? 1 : -1;

		var position = Position
			.WithX( Position.x + randomX )
			.WithY( Position.y + randomY );

		var enemy = new BattleEnemy { StartPosition = position, EndPosition = Position, Progress = 0f };
		
		var targets = new List<string>( TargetList );
		targets.Add( enemy.Target );
		TargetList = targets;

		return enemy;
	}
	
	/// <summary>
	/// Kills an enemy at the given index.
	/// </summary>
	/// <param name="index">Enemy</param>
	private void KillEnemy( int index )
	{
		var enemies = new List<BattleEnemy>(Enemies);
		if ( index >= enemies.Count )
		{
			return;
		}

		var enemy = enemies[index];
		enemy.Delete();
		enemies.Remove( enemy );
		Enemies = enemies;
	}


}
