using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Script for a bumper that gives more score for each other bumper that is hit in quick sucession.
/// When the same bumper is hit before hitting another bumper the same score bonus is given
/// without increasing, but the timer still resets.
/// </summary>
public class BumperChainScore : MonoBehaviour
{
    // Hit ScoreHit.
    public int ScoreHit = 100;
    public AudioClip Sound;
	public event HitEventHandler Hit;

    // Bumper hit animation.
    public string AnimationName;

    // Use this for initialization
    void Start()
    {
        // Increase the 'actual' chain rest time for every instance of this script.
        ChainScore.use.ActualChainResetTime += ChainScore.use.ChainResetTime;

		// Let all 'ObjectHitCondition' know this object was created.
		List<Condition> hitConditions = ChallengeManager.use.GetConditionsOfType<ObjectHitCondition>();
		foreach (Condition hitCond in hitConditions)
		{
			(hitCond as ObjectHitCondition).BumperChainScoreObjectCreated(this);
		}
    }

    // Called every frame.
    void Update()
    {
        // Only update if barrels are hit at least once.
        if (ChainScore.use.ChainMultiplier > 1)
        {
            // Reset if the time since the last barrel hit exceeds the time to reset.
            if (ChainScore.use.TimeSinceHit < ChainScore.use.ActualChainResetTime)
            {
                ChainScore.use.TimeSinceHit += Time.deltaTime;
            }
            else
            {
                ChainScore.use.ChainMultiplier = 0;
                ChainScore.use.TimeSinceHit = 0;
            }
        }
    }

    // Called when another object collides.
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collider is the ball.
        if (collision.collider.gameObject.tag != "Ball")
            return;

		// Call hit event.
		if (Hit != null)
			Hit();

        // Play bump animation.
        GetComponent<Animation>().Play(AnimationName);
        
        // Don't increase score if hitting same bumper twice.
        if(ChainScore.use.LastHitBumper != gameObject)
        {
            // Add to the score chain.
            ++ChainScore.use.ChainMultiplier;         
        }        
        
        // Store last hit bumper.
        ChainScore.use.LastHitBumper = gameObject;

        // Reset time since last barrel hit.
        ChainScore.use.TimeSinceHit = 0;

        // Give score
        ScoreManager.use.ShowScore(ScoreHit + ChainScore.use.ChainBonusScore * (ChainScore.use.ChainMultiplier - 1), collision.contacts[0].point.zAdd(Random.Range(10, 20)), 2.0f, Sound, Color.white, gameObject);
    }
}
