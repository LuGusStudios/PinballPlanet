using UnityEngine;

/// <summary>
/// Cannon that shoots a cannon ball when it's hit by the ball.
/// If this cannonball hits a target the player gets bonus points otherwise it simply flies away.
/// </summary>
public class Cannon : MonoBehaviour
{
    // Starting point.
    public Transform CannonBallStart;

    // End position when ball misses target.
    public Transform CannonBallMiss;

    // Cannon target object.
    public CannonTarget Target;

    // Cannon ball prefab to spawn.
    public GameObject CannonBall;

    // Particles to spawn when firing.
    public GameObject FireParticlesPrefab;
    public GameObject MissParticlesPrefab;

    // Called when another object collides.
    void OnCollisionEnter(Collision collision)
    {
        // Only shoot the cannon if the collider is a ball.
        if (collision.collider.gameObject.tag != "Ball")
            return;

        // Spawn a cannon ball.
        CannonBall cannonBall = (Instantiate(CannonBall, CannonBallStart.position, Quaternion.identity) as GameObject).GetComponent<CannonBall>();

        // Spawn particles.
        Instantiate(FireParticlesPrefab);

        // If the target is in the correct area shoot the ball to the target, otherwise miss the target.
        if (Target.GetComponent<CannonTarget>().InTargetArea)
        {
            cannonBall.SetTarget(Target.CalculateFuturePos(cannonBall.TravelTime), true);
        }
        else
        {
            cannonBall.SetTarget(CannonBallMiss.position, false);
            Invoke("SpawnMissParticles", cannonBall.TravelTime);
        }
    }

    // Spawns particles for when the cannonball misses.
    private void SpawnMissParticles()
    {
        Instantiate(MissParticlesPrefab, CannonBallMiss.position, Quaternion.identity);
    }
}
