using UnityEngine;
using System.Collections;

/// <summary>
/// Class that holds all the data shared across each bumper chain score.
/// Used to calculate the bonus score given for hitting multiple bumpers quickly.
/// </summary>
public class ChainScore : LugusSingletonExisting<ChainScore>
{
    // Chaining barrel hits in quick succession gives bonus score for each one hit.
    public int ChainBonusScore = 50;
    public int ChainMultiplier = 0;
    // Time chain stays active since last barrel hit.
    public float ChainResetTime = 2.0f;
    public float TimeSinceHit = 0;
    // Time till actual reset: single chain reset time is added for every object that has a BumperChainScore script attached.
    public float ActualChainResetTime = 0;
    // Last hit object used to check if last hit object was the same.
    public GameObject LastHitBumper;
}
