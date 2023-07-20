using Sandbox;

namespace Sandtype;

public class TerryGame : EntityComponent<Pawn>
{

	public int TerryMaxHealth = 10;
	public int TerryHealth = 10;
	
	public int PlayerMaxHealth = 10;
	public int PlayerHealth = 10;
	public float TerryKillTime;

	private int TerrySpawnCooldown = 100;
	
	protected override void OnActivate()
	{
		base.OnActivate();

		if ( Game.IsClient )
		{
			Entity.Hud.Game = this;
		}
	}

	public void Simulate()
	{
		if ( TerrySpawnCooldown <= 0 )
		{
			Entity.Hud.GameView.World.CreateTerry();
			TerrySpawnCooldown = 1000;
		}
		else
		{
			TerrySpawnCooldown -= 1;
		}

		Entity.Hud.Boss.TerryMax = TerryMaxHealth;
		Entity.Hud.Boss.TerryHealth = TerryHealth;
	}

	public void HandleTerryHit()
	{
		PlayerHealth -= 1;
	}
	
	public void HandleBulletHit()
	{
		TerryHealth -= 1;
		if ( TerryHealth <= 0 && TerryKillTime == 0 )
		{
			TerryKillTime = Time.Now;
		}
	}

}
