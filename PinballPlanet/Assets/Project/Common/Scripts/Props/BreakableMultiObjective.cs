﻿using System.Collections.Generic;
using UnityEngine;

public delegate void BreakableMultiTriggerEventHandler();

/// <summary>
/// This gameobject will require the player to break a number of 'Breakable' to activate the objective/function of this gameobject.
/// </summary>
public class BreakableMultiObjective : Breakable
{
	public event BreakableMultiTriggerEventHandler Trigger;

    // List of breakable objectives.
    public List<Breakable> Objectives;

    // Bonus points for completing objective.
    public int BonusPoints = 1500;

    // How long till the chest objective resets.
    public float ResetDelay = 3.0f;
	public float characterSize = 2.5f;

    private bool _recentlyActivated = false;

    // Returns true if all objectives are broken.
    public bool ObjectivesBroken
    {
        get
        {
            // If one objective is not broken return false.
            foreach (Breakable objective in Objectives)
            {
                if (!objective.IsBroken)
                    return false;
            }
            return true;
        }
    }

    // Use this for initialization.
    protected override void Start()
    {
        // Subscribe to the broken event of all breakable subobjectives.
        foreach (Breakable objective in Objectives)
        {
            objective.GetComponent<Breakable>().Broken += ObjectiveBroken;
        }

		// Let all 'ObjectHitCondition' know this object was created.
		List<Condition> hitConditions = ChallengeManager.use.GetConditionsOfType<BreakableMultiTriggerCondition>();
		foreach (Condition hitCond in hitConditions)
		{
			(hitCond as BreakableMultiTriggerCondition).BreakableMultiObjectCreated(this);
		}
    }

    // Called when a breakable objective is broken.
    private void ObjectiveBroken(GameObject sender)
    {
        // Activate if all breakables are broken.
        if (ObjectivesBroken && !_recentlyActivated)
        {
            Break();
            Activate();
			if (Trigger != null)
				Trigger();
        }
    }

    // Activates the objective once all breakable subobjectives are broken.
    public virtual void Activate()
    {
        _recentlyActivated = true;
        //Debug.Log("--- Objective " + gameObject.name + " achieved ---");

        // Hide the mesh.
        if (renderer != null)
            renderer.enabled = false;
        if (collider != null)
            collider.enabled = false;

        // Give bonus score for completing all objectives.
        GameObject scorePopup = ScoreManager.use.ShowScore(BonusPoints, transform.position.zAdd(20), 1.5f, null, Color.white, gameObject);
		scorePopup.GetComponent<TextMesh>().characterSize = characterSize;

        // Reset
        Invoke("Unbreak", ResetDelay);
    }

    // Restores the game object to its unbroken state.
    public override void Unbreak()
    {
        _recentlyActivated = false;

        // Reset all subobjectives.
        foreach (Breakable objective in Objectives)
        {
            objective.Unbreak();
        }

        // Call inherited unbreak function.
        base.Unbreak();
    }

}
