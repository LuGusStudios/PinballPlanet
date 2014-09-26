using System;
using UnityEngine;

/// <summary>
/// A condition that is met hitting an object.
/// </summary>
class ObjectHitCondition : Condition
{
    // Object to hit.
    private string _objectToHitName = "";

    // Whether condition is met when hit or when not hit.
    private bool _shouldHit = true;

    // Whether object was hit. Is only true one frame.
    private bool _hit = false;
    private bool _hitOnce = false;

    // Constructor.
    public ObjectHitCondition(string objectToHitName)
    {
        // Store object to hit.
        _objectToHitName = objectToHitName;
    }

    // Constructor.
    public ObjectHitCondition() { }

    // Internal function used to check condition met.
    protected override bool IsInternallyMet()
    {
        bool met = false;

        // If object should never be hit, keep returning false once hit.
        if (!_shouldHit && _hitOnce)
        {
            met = false;
        }
        else if (_hit == _shouldHit)
        {
            met = true;
        }

        // Reset hit every update.
        _hit = false;

        return met;
    }

    // Initialize from a string parameters dictionary. 
    public override void InitializeFromParameters(System.Collections.Generic.Dictionary<string, string> parameters)
    {
        // Name
        string key = "ObjectName";
        if (TryParseParameter(key, out _objectToHitName, "", ref parameters))
            parameters.Remove(key);

        // Should hit.
        key = "ShouldHit";
        if (TryParseParameter(key, out _shouldHit, true, ref parameters))
            parameters.Remove(key);

        // Base initialize.
        base.InitializeFromParameters(parameters);
    }

    // Called when object was hit.
    private void OnObjectHit()
    {
        _hit = true;
        _hitOnce = true;
    }

    // Subscribes to the score hit event.
    public void ScoreHitObjectCreated(ScoreHit scoreHit)
    {
        // Check if name corresponds (also check for cloned objects).
        if (scoreHit.name == _objectToHitName || scoreHit.name == _objectToHitName + "(Clone)")
        {
            Debug.Log("Subscribed to hit event of " + _objectToHitName);
            scoreHit.Hit += OnObjectHit;
        }
    }

    // Called when new level was loaded.
    public override void OnLevelWasLoaded()
    {
        // Reset hit.
        if (LevelLoadReset)
        {
            _hit = false;
            _hitOnce = false;
        }

        base.OnLevelWasLoaded();
    }
}
