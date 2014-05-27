using UnityEngine;
using System.Collections;

public class CrystalShard : Projectile
{
    // Lifetime of the coin once it hits ground.
    public float Lifetime = 3.0f;
    private float _timeAlive = 0;

    // Animation names.
    private const string JumpAnimName = "CrystalShardJump";

    // Use this for initialization
    protected override void Start()
    {
        // Start base.
        base.Start();

        // Play shooting up/falling animation.
        transform.FindChild("CrystalShard").GetComponent<Animation>().Play(JumpAnimName);
        // Scale animation to be as long as TravelTime.
        transform.FindChild("CrystalShard").animation[JumpAnimName].speed = transform.FindChild("CrystalShard").animation[JumpAnimName].length / TravelTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Update base.
        base.Update();

        _timeAlive += Time.deltaTime;

        if(_timeAlive >= Lifetime)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // Return if not colliding with ball.
        if (other.gameObject.tag != "Ball")
            return;

        // Destroy the shard.
        Destroy(gameObject);
    }
}
