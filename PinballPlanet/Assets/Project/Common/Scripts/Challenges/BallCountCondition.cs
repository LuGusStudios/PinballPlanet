using System;
using UnityEngine;

/// <summary>
/// A condition that is met by comparing the ball count.
/// </summary>
class BallCountCondition : Condition
{
    // Comparison value.
    private int _amount = 0;

    // Compare function.
    private Func<int, int, bool> _comparer;

    // Constructor.
    public BallCountCondition(Func<int, int, bool> comparer, int amount)
    {
        // Store values.
        _amount = amount;
        _comparer = comparer;
    }

    // Constructor.
    public BallCountCondition() 
    {
        // Default values.
        _comparer = Functor.Equal<int>();
    }

    // Internal function used to check condition met.
    protected override bool IsInternallyMet()
    {
        if (!ScoreManager.Exists())
            return false;

        return _comparer(ScoreManager.use.BallCount, _amount);
    }

    // Initialize from a string parameters dictionary. 
    public override void InitializeFromParameters(System.Collections.Generic.Dictionary<string, string> parameters)
    {
        // Value
        string key = "Value";
        if (TryParseParameter(key, out _amount, _amount, ref parameters))
            parameters.Remove(key);

        // Comparer
        key = "Comparer";
        if (TryParseComparerParameter<int>(key, out _comparer, _comparer, ref parameters))
            parameters.Remove(key);

        // Base initialize.
        base.InitializeFromParameters(parameters);
    }
}
