using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Starting point.
    protected Vector3 StartPos;
    // Target point.
    protected Vector3 TargetPos;

    // Time to reach end point.
    public float TravelTime = 5.0f;
    private float _timeTravelling = 0;

    // How far along the lerp the projectile is.
    protected float TravelLerp;

    // Use this for initialization
    protected virtual void Start()
    {
        // Store start position.
        StartPos = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Calculate current position for lerp.
        TravelLerp = _timeTravelling / TravelTime;

        // Set new position as a lerp between the end and start position.
        transform.position = Vector3.Lerp(StartPos, TargetPos, TravelLerp);

        // Update how long target has been travelling.
        _timeTravelling += Time.deltaTime;
    }

    // Let's you set the target position to which the cannon ball shoots.
    public void SetTarget(Vector3 pos)
    {
        TargetPos = pos;
    }
}
