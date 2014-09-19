using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Challenge
{
    // Unique ID
    public string ID = "";

    // Description
    public string Description = "";

    // Stars awarded.
    public int StarsReward = 1;

    // Completed
    public bool Completed = false;
    public bool Done = false;

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
        foreach (Condition condition in Conditions)
        {
            // Return false if one condition isn't met.
            if (!condition.IsMet())
                return false;
        }

        Completed = true;
        return true;
    }
}
