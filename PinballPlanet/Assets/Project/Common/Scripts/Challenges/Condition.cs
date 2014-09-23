using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Condition
{
    // Last frame met value.
    private bool _met = false;
    public bool Met
    {
        get { return _met; }
        set
        {
            _met = value;

            if (value)
                _metOnce = true;
        }
    }

    // Whether Condition stays true once met.
    public bool MeetOnce = false;
    private bool _metOnce = false;

    //// How long condition should be met.
    //public float TimeToMeet = 0;
    //private float _timeMet = 0;

    //// If time counter should restart when condition is not met.
    //public bool ResetTime = false;

    // How many times condition should be met.
    public int CountToMeet = 1;
    public int _metCounter;

    // Whether to only increase times met counter when changed.
    public bool CountChangedOnly = false;
    private bool _beforeChangeMet = false;

    // Whether to reset on level change.
    public bool LevelLoadReset = false;

    // Returns whether the condition has been met.
    public bool IsMet()
    {
        // Always return true if condition should only meet once and it's already met.
        if (MeetOnce && _metOnce)
            return true;

        // Get internal met.
        bool internalMet = IsInternallyMet();

        // Actually met after all extra checks.
        bool actuallyMet = internalMet;

        // Update counter.
        if (_metCounter < CountToMeet)
        {
            if (CountChangedOnly)
            {
                // Check if met changed.
                if (_beforeChangeMet != internalMet)
                {
                    // Add count only when changed.
                    if (internalMet)
                        ++_metCounter;

                    // Update before change met.
                    _beforeChangeMet = internalMet;
                }
            }
            else
            {
                // Add count when met.
                if (internalMet)
                    ++_metCounter;
            }

            // False if still not met enough times.
            if (_metCounter < CountToMeet)
                actuallyMet = false;
        }

        // Overwrite old met.
        _met = actuallyMet;

        return _met;
    }

    // The internal condition met calculation. This disregards any extra checks like counter and timer.
    protected virtual bool IsInternallyMet()
    {
        // Override with inheriting class.
        Debug.Log("IsInternallyMet not implemented");
        return false;
    }

    // Called when new level was loaded.
    public virtual void OnLevelWasLoaded()
    {
        // Reset condition.
        if (LevelLoadReset)
        {
            _metCounter = 0;
        }
    }

    // Initialize from a string parameters dictionary. 
    public virtual void InitializeFromParameters(Dictionary<string, string> parameters)
    {
        //Debug.Log("Initializing condition from parameters. ");

        // Meet once.
        string key = "MeetOnce";
        if (TryParseParameter(key, out MeetOnce, MeetOnce, ref parameters))
            parameters.Remove(key);

        // Meet Count.
        key = "CountToMeet";
        if (TryParseParameter(key, out CountToMeet, CountToMeet, ref parameters))
            parameters.Remove(key);

        // Count changed only.
        key = "CountChangedOnly";
        if (TryParseParameter(key, out CountChangedOnly, CountChangedOnly, ref parameters))
            parameters.Remove(key);

        // Reset on level load.
        key = "LevelLoadReset";
        if (TryParseParameter(key, out LevelLoadReset, LevelLoadReset, ref parameters))
            parameters.Remove(key);

        // Check if all parameters given have been used.
        if (parameters.Count > 0)
            Debug.LogError("Not all parameters initialized. Make sure you used correct parameter names.");
    }

    //------------------
    // Parse parameters.
    //------------------
    /// <summary>
    /// Tries to parse a condition value from a parameter key.
    /// </summary>
    /// <param name="key">Key in parameter dictionary.</param>
    /// <param name="param">Value to be filled in.</param>
    /// <param name="defaultValue">Default value used when no valid comparer can be parsed.</param>
    /// <param name="parameters">Dictionary of parameters in which to search for the key.</param>
    /// <returns>True when parameter is found and valid, false when it isn't.</returns>
    protected bool TryParseParameter(string key, out int param, int defaultValue, ref Dictionary<string, string> parameters)
    {
        string readValue;
        if (parameters.TryGetValue(key, out readValue))
        {
            int result;
            if (int.TryParse(readValue, out result))
            {
                Debug.Log(key + ": " + result);
                param = result;

                return true;
            }
            else
            {
                Debug.LogError(key + " parameter value not valid!");
            }
        }

        param = defaultValue;
        return false;
    }
    /// <summary>
    /// Tries to parse a condition value from a parameter key.
    /// </summary>
    /// <param name="key">Key in parameter dictionary.</param>
    /// <param name="param">Value to be filled in.</param>
    /// <param name="defaultValue">Default value used when no valid comparer can be parsed.</param>
    /// <param name="parameters">Dictionary of parameters in which to search for the key.</param>
    /// <returns>True when parameter is found and valid, false when it isn't.</returns>
    protected bool TryParseParameter(string key, out bool param, bool defaultValue, ref Dictionary<string, string> parameters)
    {
        string readValue;
        if (parameters.TryGetValue(key, out readValue))
        {
            bool result;
            if (bool.TryParse(readValue, out result))
            {
                Debug.Log(key + ": " + result);
                param = result;

                return true;
            }
            else
            {
                Debug.LogError(key + " parameter value not valid!");
            }
        }

        param = defaultValue;
        return false;
    }
    /// <summary>
    /// Tries to parse a condition value from a parameter key.
    /// </summary>
    /// <param name="key">Key in parameter dictionary.</param>
    /// <param name="param">Value to be filled in.</param>
    /// <param name="defaultValue">Default value used when no valid comparer can be parsed.</param>
    /// <param name="parameters">Dictionary of parameters in which to search for the key.</param>
    /// <returns>True when parameter is found and valid, false when it isn't.</returns>
    protected bool TryParseParameter(string key, out float param, float defaultValue, ref Dictionary<string, string> parameters)
    {
        string readValue;
        if (parameters.TryGetValue(key, out readValue))
        {
            float result;
            if (float.TryParse(readValue, out result))
            {
                Debug.Log(key + ": " + result);
                param = result;

                return true;
            }
            else
            {
                Debug.LogError(key + " parameter value not valid!");
            }
        }

        param = defaultValue;
        return false;
    }
    /// <summary>
    /// Tries to parse a condition value from a parameter key.
    /// </summary>
    /// <param name="key">Key in parameter dictionary.</param>
    /// <param name="param">Value to be filled in.</param>
    /// <param name="defaultValue">Default value used when no valid comparer can be parsed.</param>
    /// <param name="parameters">Dictionary of parameters in which to search for the key.</param>
    /// <returns>True when parameter is found and valid, false when it isn't.</returns>
    protected bool TryParseParameter(string key, out string param, string defaultValue, ref Dictionary<string, string> parameters)
    {
        string readValue;
        if (parameters.TryGetValue(key, out readValue))
        {
            param = parameters[key];
            Debug.Log(key + ": " + param);

            return true;
        }

        param = defaultValue;
        return false;
    }

    /// <summary>
    /// Tries to parse a comparer function from a parameter key.
    /// </summary>
    /// <typeparam name="T">The type used in the comparer.</typeparam>
    /// <param name="key">Key in parameter dictionary.</param>
    /// <param name="param">Comparer function to be filled in.</param>
    /// <param name="defaultValue">Default comparer used when no valid comparer can be parsed.</param>
    /// <param name="parameters">Dictionary of parameters in which to search for the comparer key.</param>
    /// <returns>True when comparer is found and valid, false when it isn't.</returns>
    protected bool TryParseComparerParameter<T>(string key, out Func<T, T, bool> param, Func<T, T, bool> defaultValue, ref Dictionary<string, string> parameters)
       where T : IComparable<T>
    {
        string readValue;
        if (parameters.TryGetValue(key, out readValue))
        {
            if (readValue == "Greater")
            {
                param = Functor.Greater<T>();
                Debug.Log("Comparer Greater added.");
                return true;
            }
            else if (readValue == "Less")
            {
                param = Functor.Less<T>();
                Debug.Log("Comparer Less added.");
                return true;
            }
            else if (readValue == "Equal")
            {
                param = Functor.Equal<T>();
                Debug.Log("Comparer Equal added.");
                return true;
            }
            else
            {
                Debug.LogError(key + " parameter value not valid!");
            }
        }

        param = defaultValue;
        return false;
    }
}