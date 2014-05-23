using UnityEngine;

public static class BarrelChainScore
{
    // Chaining barrel hits in quick succession gives bonus score for each one hit.
    public static int ChainBonusScore = 50;
    public static int ChainMultiplier = 0;
    // Time chain stays active since last barrel hit.
    public static float ChainResetTime = 5.0f;
    public static float TimeSinceHit = 0;
}

public class Barrel : MonoBehaviour
{
    // Hit Score.
    public int score = 100;
    public AudioClip sound;

    // Called at fixed frames.
    void Update()
    {
        // Only update if barrels are hit at least once.
        if (BarrelChainScore.ChainMultiplier > 1)
        {
            // Reset if the time since the last barrel hit exceeds the time to reset.
            if (BarrelChainScore.TimeSinceHit < BarrelChainScore.ChainResetTime)
            {
                BarrelChainScore.TimeSinceHit += Time.deltaTime; 
            }
            else
            {
                BarrelChainScore.ChainMultiplier = 0;
                BarrelChainScore.TimeSinceHit = 0;
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
        GetComponent<Animation>().Play("BarrelBump");

        // Add to the score chain.
        ++BarrelChainScore.ChainMultiplier;
        // Reset time since last barrel hit.
        BarrelChainScore.TimeSinceHit = 0;

        // Give score
        ScoreManager.use.ShowScore(score + BarrelChainScore.ChainBonusScore * (BarrelChainScore.ChainMultiplier - 1), collision.contacts[0].point.zAdd(Random.Range(10, 20)), 2.0f, sound, Color.white);

    }
}
