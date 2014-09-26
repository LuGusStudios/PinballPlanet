using System;
using UnityEngine;

/// <summary>
/// A condition that is met by checking how many balls are in game.
/// </summary>
class BallsInPlayCondition : Condition
{
    // Comparison value.
    private int _amount = 0;

    // Compare function.
    private Func<int, int, bool> _comparer;

    // Constructor.
    public BallsInPlayCondition(Func<int, int, bool> comparer, int amount)
    {
        // Store values.
        _amount = amount;
        _comparer = comparer;
    }

    // Constructor.
    public BallsInPlayCondition() 
    {
        // Default values.
        _comparer = Functor.Equal<int>();
    }

    // Internal function used to check condition met.
    protected override bool IsInternallyMet()
    {
        if (!Player.Exists())
            return false;

        return _comparer(Player.use.BallsInPlay.Count, _amount);
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
