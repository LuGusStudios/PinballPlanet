using UnityEngine;

/// <summary>
/// Static class that holds all the data shared across each bumper chain score.
/// Used to calculate the bonus score given for hitting multiple bumpers quickly.
/// </summary>
public static class ChainScore
{
    // Chaining barrel hits in quick succession gives bonus score for each one hit.
    public static int ChainBonusScore = 50;
    public static int ChainMultiplier = 0;
    // Time chain stays active since last barrel hit.
    public static float ChainResetTime = 2.0f;
    public static float TimeSinceHit = 0;
    // Time till actual reset: single chain reset time is added for every object that has a BumperChainScore script attached.
    public static float ActualChainResetTime = 0;
    // Last hit object used to check if last hit object was the same.
    public static GameObject LastHitBumper;
}

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

    // Bumper hit animation.
    public string AnimationName;

    // Use this for initialization
    void Start()
    {
        // Increase the 'actual' chain rest time for every instance of this script.
        ChainScore.ActualChainResetTime += ChainScore.ChainResetTime;
    }

    // Called every frame.
    void Update()
    {
        // Only update if barrels are hit at least once.
        if (ChainScore.ChainMultiplier > 1)
        {
            // Reset if the time since the last barrel hit exceeds the time to reset.
            if (ChainScore.TimeSinceHit < ChainScore.ActualChainResetTime)
            {
                ChainScore.TimeSinceHit += Time.deltaTime;
            }
            else
            {
                ChainScore.ChainMultiplier = 0;
                ChainScore.TimeSinceHit = 0;
            }
        }
    }

    // Called when another object collides.
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collider is the ball.
        if (collision.collider.gameObject.tag != "Ball")
            return;

        // Play bump animation.
        GetComponent<Animation>().Play(AnimationName);
        
        // Don't increase score if hitting same bumper twice.
        if(ChainScore.LastHitBumper != gameObject)
        {
            // Add to the score chain.
            ++ChainScore.ChainMultiplier;         
        }        
        
        // Store last hit bumper.
        ChainScore.LastHitBumper = gameObject;

        // Reset time since last barrel hit.
        ChainScore.TimeSinceHit = 0;

        // Give score
        ScoreManager.use.ShowScore(ScoreHit + ChainScore.ChainBonusScore * (ChainScore.ChainMultiplier - 1), collision.contacts[0].point.zAdd(Random.Range(10, 20)), 2.0f, Sound, Color.white);
    }
}
