using System;
using UnityEngine;

/// <summary>
/// A condition that is met by what level is played.
/// </summary>
class LevelCondition : Condition
{
    // Comparison name.
    private string _name;

    // Compare function.
    private Func<string, string, bool> _comparer;

    // Constructor.
    public LevelCondition(Func<string, string, bool> comparer, string lvlName)
    {
        // Store values.
        _name = lvlName;
        _comparer = comparer;
    }
       
    // Constructor.
    public LevelCondition() 
    {
        // Default values.
        _name = PlayerData.MainLvlName;
        _comparer = Functor.Equal<string>();
    }

    // Internal function used to check condition met.
    protected override bool IsInternallyMet()
    {
        return _comparer(Application.loadedLevelName, _name);
    }

    // Initialize from a string parameters dictionary. 
    public override void InitializeFromParameters(System.Collections.Generic.Dictionary<string, string> parameters)
    {
        // Name
        string key = "Name";
        if (TryParseParameter(key, out _name, _name, ref parameters))
            parameters.Remove(key);

        // Comparer
        key = "Comparer";
        if (TryParseComparerParameter<string>(key, out _comparer, _comparer, ref parameters))
            parameters.Remove(key);

        // Base initialize.
        base.InitializeFromParameters(parameters);
    }
}
