using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Challenge
{
    // Unique ID
    public string ID = "";

    // Priority in which challenge is given.
    public int Priority = int.MaxValue;

    // Description
    public string Description = "";

    // Stars awarded.
    public int StarsReward = 1;

    // Completed
    public bool Completed = false;

    // Has been viewed by player.
    public bool Viewed = false;

    // Required level key.
    public LevelKey LevelKey = LevelKey.None;

    // Conditions
    public List<Condition> Conditions = new List<Condition>();

    // Constructor.
    public Challenge(string description, params Condition[] conditionParams)
    {
        // Fill in description.
        Description = description;

        // Fill in conditions.
        Conditions.AddRange(conditionParams);
    }

    // Constructor.
    public Challenge() {}

    // Returns if challenge is completed.
    public bool IsCompleted()
    {
        bool met = true;

        // Check if in correct level.
        if (LevelKey != LevelKey.None)
        {
            if (Application.loadedLevelName != "Pinball_" + LevelKey.ToString())
                met = false;
        }

        // Return if false.
        if (!met)
            return false;

        // False when a single condition is not met.
        foreach (Condition condition in Conditions)
        {
            met &= condition.IsMet();

            // Return if false.
            if (!met)
                return false;
        }

        // Set to completed.
        if (met)
        {
            Debug.Log("Challenge completed: " + ID);
            Completed = true;
        }

        return met;
    }
}
