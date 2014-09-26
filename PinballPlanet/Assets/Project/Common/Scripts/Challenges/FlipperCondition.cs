using System;
using UnityEngine;

/// <summary>
/// A condition that is met by checking if a flipper is used.
/// </summary>
class FlipperCondition : Condition
{
    // Flipper to check.
    private string _flipperName = "";
    private FlipperNew _flipper = null;

    // Compare function.
    private Func<bool, bool, bool> _comparer;

    // If flipper was used once.
    private bool _useOnce = false;
    private bool _usedOnce = false;

    // Constructor.
    public FlipperCondition() 
    {
        _comparer = Functor.Equal<bool>();
    }

    // Internal function used to check condition met.
    protected override bool IsInternallyMet()
    {
        // Check if flipper found.
        if (_flipper == null)
            return false;

        // Set used once.
        if (_flipper.Pressed)
            _usedOnce = true;


        // Compare with used once if use once is set.
        if (_useOnce)
            return _comparer(_usedOnce, true);

        return _comparer(_flipper.Pressed, true);
    }

    // Initialize from a string parameters dictionary. 
    public override void InitializeFromParameters(System.Collections.Generic.Dictionary<string, string> parameters)
    {
        // Flipper
        string key = "Flipper";
        if (TryParseParameter(key, out _flipperName, "", ref parameters))
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
        // Find flipper.
        GameObject flipper = GameObject.Find(_flipperName);
        if (flipper != null)
            _flipper = flipper.GetComponent<FlipperNew>();
        else
            _flipper = null;

        // Reset hit.
        if (LevelLoadReset)
        {
            _usedOnce = false;
        }

        base.OnLevelWasLoaded();
    }

}
