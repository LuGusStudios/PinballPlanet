using UnityEngine;

public static class ChainScore
{
    // Chaining barrel hits in quick succession gives bonus score for each one hit.
    public static int ChainBonusScore = 50;
    public static int ChainMultiplier = 0;
    // Time chain stays active since last barrel hit.
    public static float ChainResetTime = 1.5f;
    // Time till actual reset: single chain reset time is added for every object that has a BumperChainScore script attached.
    public static float ActualChainResetTime = 0;
    public static float TimeSinceHit = 0;
}

public class BumperChainScore : MonoBehaviour
{
    // Hit Score.
    public int Score = 100;
    public AudioClip Sound;

    public string AnimationName;

    // Use this for initialization
    void Start()
    {
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
        
        // Add to the score chain.
        ++ChainScore.ChainMultiplier;
        // Reset time since last barrel hit.
        ChainScore.TimeSinceHit = 0;

        // Give score
        ScoreManager.use.ShowScore(Score + ChainScore.ChainBonusScore * (ChainScore.ChainMultiplier - 1), collision.contacts[0].point.zAdd(Random.Range(10, 20)), 2.0f, Sound, Color.white);
    }
}
