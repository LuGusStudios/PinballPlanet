using System;
using UnityEngine;

/// <summary>
/// A condition that is met by comparing the score.
/// </summary>
class ScoreCondition : Condition
{
    // Comparison value.
    private int _compareValue;

    // Compare function.
    private Func<int, int, bool> _comparer;

    // Constructor.
    public ScoreCondition(Func<int, int, bool> comparer, int compareValue)
    {
        // Store values.
        _compareValue = compareValue;
        _comparer = comparer;
    }

    // Constructor.
    public ScoreCondition() 
    {
        _comparer = Functor.Greater<int>();
    }

    // Internal function used to check condition met.
    protected override bool IsInternallyMet()
    {
        if (!ScoreManager.Exists())
            return false;

        return _comparer(ScoreManager.use.TotalScore, _compareValue);
    }

    // Initialize from a string parameters dictionary. 
    public override void InitializeFromParameters(System.Collections.Generic.Dictionary<string, string> parameters)
    {
        // Value
        string key = "Value";
        if (TryParseParameter(key, out _compareValue, _compareValue, ref parameters))
            parameters.Remove(key);

        // Comparer
        key = "Comparer";
        if (TryParseComparerParameter<int>(key, out _comparer, _comparer, ref parameters))
            parameters.Remove(key);

        // Base initialize.
        base.InitializeFromParameters(parameters);
    }
}
