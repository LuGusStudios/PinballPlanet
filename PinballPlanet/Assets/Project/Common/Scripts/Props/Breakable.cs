using UnityEngine;
using System.Collections.Generic;

public delegate void OnBrokenEventHandler(GameObject sender);
public delegate void OnUnBrokenEventHandler(GameObject sender);

/// <summary>
/// A script that will break the object once it's hit by a ball.
/// The 'Broken' event will send a message to any object subscribed to it.
/// A 'Breakable' can be used as a subobjective for a 'BreakableMultiObjective'.
/// </summary>
public class Breakable : MonoBehaviour
{
    // Event notifying that this object was broken.
    public event OnBrokenEventHandler Broken;
    public event OnUnBrokenEventHandler UnBroken;

    // Holds whether the object is broken.
    public bool IsBroken = false;

    // The new score given when hitting this object once it's broken.
    public int BrokenScore = 100;
    private int _originalScore;

    // Initialization.
    protected virtual void Start()
    {
        // Store original score.
        if (GetComponent<ScoreHit>() != null)
            _originalScore = GetComponent<ScoreHit>().score;

		List<Condition> hitConditions = ChallengeManager.use.GetConditionsOfType<ObjectHitCondition>();
		foreach (Condition hitCond in hitConditions)
		{
			(hitCond as ObjectHitCondition).BreakableObjectCreated(this);
		}
    }

    // Called when another object collides.
    void OnCollisionEnter(Collision collision)
    {
        // Only break if the collider is a ball.
        if (collision.collider.gameObject.tag != "Ball")
            return;

        if (!IsBroken)
            Break();
    }

    // Breaks the game object.
    public virtual void Break()
    {
        // Set to broken.
        IsBroken = true;

        // Lower the score after the object is broken..
        // Invoked after a short time so that the score isn't lowered before the first score is given.
        Invoke("LowerScore", 0.1f);

        // Call break event.
        OnBreak();
    }

    // Restores the game object to its unbroken state.
    public virtual void Unbreak()
    {
        IsBroken = false;

        if (GetComponent<ScoreHit>() != null)
            GetComponent<ScoreHit>().score = _originalScore;

        // Call unbroken event.
        OnUnBreak();
    }

    // Lower score of the game object.
    protected virtual void LowerScore()
    {
        if (GetComponent<ScoreHit>() != null)
            GetComponent<ScoreHit>().score = BrokenScore;

        if (GetComponent<BumperChainScore>() != null)
            GetComponent<BumperChainScore>().ScoreHit = BrokenScore;
    }

    // Call break event.
    protected void OnBreak()
    {
        if (Broken != null)
            Broken(gameObject);
    }

    // Call unbreak event.
    protected void OnUnBreak()
    {
        if (UnBroken != null)
            UnBroken(gameObject);
    }
}
