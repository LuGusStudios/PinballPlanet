using UnityEngine;

public class CannonBall : Projectile
{
    // Whether the cannon ball will hit or miss the target.
    private bool _willHit = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        // Play shooting up/falling animation.
        transform.FindChild("CannonBall_Mesh").GetComponent<Animation>().Play("CannonBallHeight");
        // Scale animation to be as long as TravelTime.
        transform.FindChild("CannonBall_Mesh").animation["CannonBallHeight"].speed = transform.FindChild( "CannonBall_Mesh").animation["CannonBallHeight"].length / TravelTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Update base.
        base.Update();

        // Call target hit.
        if (TravelLerp > 1f)
            TargetHit();
    }

    // Called when the cannon target is hit.
    public void TargetHit()
    {
        // Destroy the cannon ball.
        Destroy(gameObject);

        if(_willHit)
            GameObject.Find("CannonTarget_Parent").GetComponent<CannonTarget>().Hit();
    }

    // Let's you set the target position to which the cannon ball shoots.
    public void SetTarget(Vector3 pos, bool willHit)
    {
        TargetPos = pos;
        _willHit = willHit;
    }
}
