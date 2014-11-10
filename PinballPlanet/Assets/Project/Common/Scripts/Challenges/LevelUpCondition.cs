using UnityEngine;
using System;

/// <summary>
/// A condition that is met by comparing the ball count.
/// </summary>
public class LevelUpCondition : Condition
{
	// Comparison value.
	private int _amount = 0;
	
	// Compare function.
	private Func<int, int, bool> _comparer;
	
	// Constructor.
	public LevelUpCondition(Func<int, int, bool> comparer, int amount)
	{
		// Store values.
		_amount = amount;
		_comparer = comparer;
	}
	
	// Constructor.
	public LevelUpCondition() 
	{
		// Default values.
		_comparer = Functor.Equal<int>();
	}
	
	// Internal function used to check condition met.
	protected override bool IsInternallyMet()
	{
		return _comparer(PlayerData.use.GetLevel(), _amount);
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
