using UnityEngine;

public class Coin : Projectile
{
    // Lifetime of the coin once it hits ground.
    public float Lifetime = 3.0f;

    // How long before the coin is destroyed when its animation is over.
    public float DestroyDelay = 0.5f;

    // Names for animations.
    private const string SpinAnimName = "CoinSpin";
    private const string JumpAnimName = "CoinJump";

    // True when the spinning animation has been started.
    private bool _spinAnimPlayed = false;

    // Use this for initialization
    protected override void Start()
    {
        // Initialize base.
        base.Start();

        // Play shooting up/falling animation.
        transform.FindChild("Pivot").GetComponent<Animation>().Play(JumpAnimName);
        // Scale animation to be as long as TravelTime.
        transform.FindChild("Pivot").animation[JumpAnimName].speed = transform.FindChild("Pivot").animation[JumpAnimName].length / TravelTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Update base.
        base.Update();

        // Play the coin spin animation once it lands.
        if (!transform.FindChild("Pivot").animation.IsPlaying(JumpAnimName) && !_spinAnimPlayed)
        {
            // Play Rotating animation.
            transform.FindChild("Pivot").GetComponent<Animation>().Play(SpinAnimName);
            // Slow down animation.
            transform.FindChild("Pivot").animation[SpinAnimName].speed = transform.FindChild("Pivot").animation[SpinAnimName].length / Lifetime;

            _spinAnimPlayed = true;
        }
        // Destroy coin once both animations are over.
        if (!transform.FindChild("Pivot").animation.IsPlaying(JumpAnimName) && !transform.FindChild("Pivot").animation.IsPlaying(SpinAnimName))
        {
            Destroy(gameObject, DestroyDelay);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Return if not colliding with ball.
        if (other.gameObject.tag != "Ball")
            return;

        // Destroy the coin by destroying the parent object.
        Destroy(gameObject);
    }
}
