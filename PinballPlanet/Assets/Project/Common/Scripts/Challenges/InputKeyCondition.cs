using System;
using UnityEngine;

/// <summary>
/// A condition that is met by checking if an input key is used.
/// </summary>
class InputKeyCondition : Condition
{
    // Key press to check.
    private string _key = "";

    // Compare function.
    private Func<bool, bool, bool> _comparer;

    // If key was used once.
    private bool _useOnce = false;
    private bool _usedOnce = false;

    // Constructor.
    public InputKeyCondition() 
    {
        _comparer = Functor.Equal<bool>();
    }

    // Internal function used to check condition met.
    protected override bool IsInternallyMet()
    {
        // Set to used once if used.
        if(Input.GetKey(_key))
            _usedOnce = true;

        // Compare with used once if use once is set.
        if (_useOnce)
            return _comparer(_usedOnce, true);

        // Return if used.
        return _comparer(Input.GetKey(_key), true);
    }

    // Initialize from a string parameters dictionary. 
    public override void InitializeFromParameters(System.Collections.Generic.Dictionary<string, string> parameters)
    {
        // Input key
        string key = "Key";
        if (TryParseParameter(key, out _key, "", ref parameters))
            parameters.Remove(key);

        // Comparer
        key = "Comparer";
        if (TryParseComparerParameter<bool>(key, out _comparer, _comparer, ref parameters))
            parameters.Remove(key);

        // Use once
        key = "UseOnce";
        if (TryParseParameter(key, out _useOnce, _useOnce, ref parameters))
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
            _usedOnce = false;
        }

        base.OnLevelWasLoaded();
    }

}
