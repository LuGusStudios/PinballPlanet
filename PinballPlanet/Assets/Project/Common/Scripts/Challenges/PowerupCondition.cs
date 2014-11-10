using UnityEngine;
using System.Collections;
using System;

public class PowerupCondition : Condition
{

	PowerupKey _puKey = PowerupKey.NONE;
	bool _any = false;
	bool _anyPermanent = false;
	bool _anyTemporary = false;

	bool _isMet = false;
	bool _mustMeet = true;
	
	// Constructor.
	public PowerupCondition(string puID)
	{
		if 		(puID == "any") _any = true;
		else if (puID == "anyPermanent") _anyPermanent = true;
		else if (puID == "anyTemporary") _anyTemporary = true;
		else 	_puKey = (PowerupKey) Enum.Parse(typeof(PowerupKey), puID);
	}
	
	// Constructor.
	public PowerupCondition() 
	{
		// Default values.
		_any = true;
	}
	
	// Internal function used to check condition met.
	protected override bool IsInternallyMet()
	{
		bool valueToReturn = false;

		if (_isMet)
			valueToReturn = true;
		else if (_any) 
		{
			if (PlayerData.use.permanentPowerup != null || PlayerData.use.temporaryPowerup != null)
			{
				_isMet = true;
				valueToReturn = true;
			}
		}
		else if (_anyPermanent)
		{
			if (PlayerData.use.permanentPowerup != null)
			{
				_isMet = true;
				valueToReturn = true;
			}
		}
		else if (_anyTemporary)
		{
			if (PlayerData.use.temporaryPowerup != null)
			{
				_isMet = true;
				valueToReturn = true;
			}
		}
		else 
		{
			if ((PowerupKey)PlayerData.use.permanentPowerup.id == _puKey || 
			    (PowerupKey)PlayerData.use.temporaryPowerup.id == _puKey)
			{
				_isMet = true;
				valueToReturn = true;
			}
		}

		if (_mustMeet)
			return valueToReturn;
		else
			return !valueToReturn;

	}
	
	// Initialize from a string parameters dictionary. 
	public override void InitializeFromParameters(System.Collections.Generic.Dictionary<string, string> parameters)
	{
		// Value
		string key = "Type";
		string puID = "";
		if (TryParseParameter(key, out puID, puID, ref parameters))
			parameters.Remove(key);

		if 		(puID == "any") _any = true;
		else if (puID == "anyPermanent") _anyPermanent = true;
		else if (puID == "anyTemporary") _anyTemporary = true;
		else 	_puKey = (PowerupKey) Enum.Parse(typeof(PowerupKey), puID);

		key = "ShouldUse";
		if (TryParseParameter(key, out _mustMeet, true, ref parameters))
			parameters.Remove(key);

		// Base initialize.
		base.InitializeFromParameters(parameters);
	}	

	// Called when new level was loaded.
	public override void OnLevelWasLoaded()
	{
		// Reset hit.
		if (LevelLoadReset)
		{
			_isMet = false;
		}
		
		base.OnLevelWasLoaded();
	}
}
