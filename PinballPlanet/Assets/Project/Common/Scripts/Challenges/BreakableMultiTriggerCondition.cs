using UnityEngine;
using System;

public class BreakableMultiTriggerCondition : Condition {

	// Object to hit.
	private string _objectToTriggerName = "";
	
	// Whether condition is met when hit or when not hit.
	private bool _shouldTrigger = true;
	
	// Whether object was hit. Is only true one frame.
	private bool _triggered = false;
	private bool _triggeredOnce = false;
	
	// Constructor.
	public BreakableMultiTriggerCondition(string objectToHitName)
	{
		// Store object to hit.
		_objectToTriggerName = objectToHitName;
	}
	
	// Constructor.
	public BreakableMultiTriggerCondition() { }
	
	// Internal function used to check condition met.
	protected override bool IsInternallyMet()
	{
		bool met = false;
		
		// If object should never be hit, keep returning false once hit.
		if (!_shouldTrigger && _triggeredOnce)
		{
			met = false;
		}
		else if (_triggered == _shouldTrigger)
		{
			met = true;
		}
		
		// Reset hit every update.
		_triggered = false;
		
		return met;
	}
	
	// Initialize from a string parameters dictionary. 
	public override void InitializeFromParameters(System.Collections.Generic.Dictionary<string, string> parameters)
	{
		// Name
		string key = "ObjectName";
		if (TryParseParameter(key, out _objectToTriggerName, "", ref parameters))
			parameters.Remove(key);
		
		// Should hit.
		key = "ShouldTrigger";
		if (TryParseParameter(key, out _shouldTrigger, true, ref parameters))
			parameters.Remove(key);
		
		// Base initialize.
		base.InitializeFromParameters(parameters);
	}
	
	// Called when object was hit.
	private void OnObjectTriggered()
	{
		_triggered = true;
		_triggeredOnce = true;
	}
	
	private void OnObjectTriggered(GameObject go)
	{
		OnObjectTriggered();
	}
	
	// Subscribes to the score hit event.
	public void BreakableMultiObjectCreated(BreakableMultiObjective bmo)
	{
		// Check if name corresponds (also check for cloned objects).
		if (bmo.name == _objectToTriggerName || bmo.name == _objectToTriggerName + "(Clone)")
		{
			Debug.Log("Subscribed to hit event of " + _objectToTriggerName);
			bmo.Trigger += OnObjectTriggered;
		}
	}
	
	// Called when new level was loaded.
	public override void OnLevelWasLoaded()
	{
		// Reset hit.
		if (LevelLoadReset)
		{
			_triggered = false;
			_triggeredOnce = false;
		}
		
		base.OnLevelWasLoaded();
	}
}
