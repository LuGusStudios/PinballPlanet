using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PowerupKey
{
	NONE = 0, 

	// Temporary Powerup values below 100
	PTEMP_bonusBall = 1, 				// Bonus ball
	PTEMP_powerBall = 2, 				// Power ball
	PTEMP_multiplierBall = 3, 			// Multiplier
	PTEMP_startScoreBoost = 4,			// In the bank

	// Permanent Powerup values above 100
	PPERM_starCatcher = 101,			// Star catcher
	PPERM_starCheaper = 102,			// Stardom
	PPERM_ExtraStarOnChallenge = 103,	// Holy grail
	PPERM_expUp = 104					// Mr Fancy
}

public class PowerupManager : LugusSingletonRuntime<PowerupManager> {

	//public Powerup permanentPowerup = null;
	//public Powerup temporaryPowerup = null;

	public override void InitializeSingleton ()
	{
		base.InitializeSingleton ();
	}

	public void ActivatePowerups()
	{
		if (PlayerData.use.permanentPowerup != null)
			PlayerData.use.permanentPowerup.Activate();

		if (PlayerData.use.temporaryPowerup != null)
			PlayerData.use.temporaryPowerup.Activate();
	}

	public void ActivatePermanentPowerup()
	{
		if (PlayerData.use.permanentPowerup != null)
			PlayerData.use.permanentPowerup.Activate();
	}

	public void ActivateTemporaryPowerup()
	{
		if (PlayerData.use.temporaryPowerup != null)
			PlayerData.use.temporaryPowerup.Activate();
	}

	public void DeactivatePermanentPowerup()
	{
		if (PlayerData.use.permanentPowerup != null)
			PlayerData.use.permanentPowerup.Deactivate();
	}

	public void DeactivateTemporaryPowerup()
	{
		if (PlayerData.use.temporaryPowerup != null)
			PlayerData.use.temporaryPowerup.Deactivate();
	}

	public void ResetOnNewBall()
	{
		if (PlayerData.use.permanentPowerup != null && PlayerData.use.permanentPowerup.resetOnNewBall)
			PlayerData.use.permanentPowerup.Activate();
		
		if (PlayerData.use.temporaryPowerup != null && PlayerData.use.temporaryPowerup.resetOnNewBall)
			PlayerData.use.temporaryPowerup.Activate();
	}

	public void SetPermanentPowerup(PowerupKey key)
	{
		if (key == PowerupKey.NONE)
		{
			PlayerData.use.permanentPowerup = null;
		}

		if ((int)key > 100)
		{
			PlayerData.use.permanentPowerup = GetNewPowerupOfType(key);
		} 
		else 
		{
			PlayerData.use.permanentPowerup = null;
			Debug.LogWarning("Trying to add temporary Powerup to permanent slot: " + key.ToString());
		}
	}

	public void SetTemporaryPowerup(PowerupKey key)
	{
		if (key == PowerupKey.NONE)
		{
			PlayerData.use.temporaryPowerup = null;
		}

		if ((int)key < 100)
		{
			PlayerData.use.temporaryPowerup = GetNewPowerupOfType(key);
		} 
		else 
		{
			PlayerData.use.temporaryPowerup = null;
			Debug.LogWarning("Trying to add permanent Powerup to temporary slot: " + key.ToString());
		}
	}

	public Powerup GetPermanentPowerup()
	{
		return PlayerData.use.permanentPowerup;
	}

	public Powerup GetTemporaryPowerup()
	{
		return PlayerData.use.temporaryPowerup;
	}

	public Powerup GetNewPowerupOfType(PowerupKey key)
	{
		switch(key)
		{
			// Temporary Powerups
		case PowerupKey.PTEMP_bonusBall:
			return new PUBonusBall((int)key);
			break;
		case PowerupKey.PTEMP_powerBall:
			return new PUPowerBall((int)key);
			break;
		case PowerupKey.PTEMP_multiplierBall:
			return new PUMultiplierBall((int)key);
			break;
		case PowerupKey.PTEMP_startScoreBoost:
			return new PUStartScoreBoost((int)key);
			break;

			// Permanent Powerups
		case PowerupKey.PPERM_starCatcher:
			return new PUStarCatcher((int)key);
			break;
		case PowerupKey.PPERM_starCheaper:
			return new PUStarCheaper((int)key);
			break;
		case PowerupKey.PPERM_ExtraStarOnChallenge:
			return new PUExtraStarOnChallenge((int)key);
			break;
		case PowerupKey.PPERM_expUp:
			return new PUExpUp((int)key);
			break;

		default:
			return null;
			break;
		}
	}
}
