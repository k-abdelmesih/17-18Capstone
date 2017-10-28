﻿using System;

namespace Capstone
{
	public class HealthComponent
	{
		public float playerHealth;
		public float enemyHealth;
		public bool isPlayerDead = false;

		public void PlayerTakesDamage(float damageAmount) 
		{
			playerHealth -= damageAmount;
			playerHasDied ();
		}

		public void EnemyTakesDamage(float damageAmount) 
		{
			enemyHealth -= damageAmount;
		}

		public void playerHasDied() 
		{
			if (playerHealth == 0f)
				isPlayerDead = true;
				showGameOverScreen ();
		}

		public void showGameOverScreen()
		{
			//will be implemented later
		}
	}
}

