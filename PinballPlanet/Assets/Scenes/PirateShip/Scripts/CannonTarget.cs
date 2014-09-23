using UnityEngine;

/// <summary>
/// A target that floats by for the cannon to hit.
/// Gives a lot of points when the player hits the cannon when this object is floating by.
/// </summary>
public class CannonTarget : MonoBehaviour
{
    // Starting point.
    public Transform StartTransform;

    // End point.
    public Transform EndTransform;

    // Time to reach end point.
    public float TravelTime = 5f;
    private float _timeTravelling = 0;

    // Whether target can be hit.
    public bool InTargetArea = false;

    // Bonus points given when target is hit.
    public int BonusPoints = 1500;

    // Particle to spawn.
    public GameObject ParticlePrefab;

    // Update is called once per frame.
    void Update()
    {
        // Calculate current position for lerp.
        float lerpPos = _timeTravelling/TravelTime;

        // Set new position as a lerp between the end and start position.
        transform.position = Vector3.Lerp(StartTransform.position, EndTransform.position, lerpPos);

        // Update how long target has been travelling.
        _timeTravelling += Time.deltaTime;

        // Reset time travelling once end point is reached.
        if (lerpPos > 1f)
            _timeTravelling = 0;
    }

    // Calculates and returns the point where this object will be in a certain amount of time.
    public Vector3 CalculateFuturePos(float deltaTime)
    {
        // Calculate future position for lerp.
        float lerpPos = (_timeTravelling + deltaTime) / TravelTime;

        return Vector3.Lerp(StartTransform.position, EndTransform.position, lerpPos);
    }

    // Called when another object enters trgger.
    void OnTriggerEnter(Collider other)
    {
        // Set InTargetArea to be true so that the target can be hit by cannon.
        InTargetArea = true;

        //Debug.Log("--- In target area: " + InTargetArea + " ---");
    }

    // Called when another object enters trgger.
    void OnTriggerExit(Collider other)
    {
        // Set InTargetArea to be false so that the target can't be hit by cannon anymore.
        InTargetArea = false;

        //Debug.Log("--- In target area: " + InTargetArea + " ---");
    }

    // Called when this target is hit by a cannonball.
    public void Hit()
    {
        // Reset.
        _timeTravelling = 0;

        // Give bonus score for hitting the target.
        GameObject scorePopup = ScoreManager.use.ShowScore(BonusPoints, transform.position.zAdd(30), 1.5f, null, Color.white, gameObject);
        scorePopup.GetComponent<TextMesh>().characterSize = 4.5f;

        // Spawn particle.
        Instantiate(ParticlePrefab, transform.position.zAdd(5), Quaternion.identity);
    }
}
