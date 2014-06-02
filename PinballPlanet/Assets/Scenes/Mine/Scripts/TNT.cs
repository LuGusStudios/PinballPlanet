using UnityEngine;
using System.Collections;

public class TNT : Breakable
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Breaks the game object by opening the lock.
    public override void Break()
    {
        // Play animation on child lock.
        gameObject.transform.FindChild("DetonatorPlunger").GetComponent<Animation>().Play("TNTPush");

        // Turn off collider.
        collider.enabled = false;

        // Call inherited break function.
        base.Break();
    }

    // Reset.
    public override void Unbreak()
    {
        // Reverse lock open animation.
        transform.FindChild("DetonatorPlunger").GetComponent<Animation>().Play("TNTPush");
        AnimationState animState = transform.FindChild("DetonatorPlunger").animation["TNTPush"];
        animState.time = animState.length;
        const float animSpeed = -0.5f;
        animState.speed = animSpeed;

        // Turn on collider.
        collider.enabled = true;

        // Reset completed after animation is done.
        Invoke("UnbreakBase", animState.length / animSpeed);
    }

    // Resets inherited base.
    private void UnbreakBase()
    {
        ResetAnimation();

        // Call inherited unbreak function.
        base.Unbreak();
    }

    // Resets the animation.
    private void ResetAnimation()
    {
        // Reset Lock opening animation.
        AnimationState animState = transform.FindChild("DetonatorPlunger").animation["TNTPush"];
        animState.time = 0;
        animState.speed = 1;

        transform.FindChild("DetonatorPlunger").GetComponent<Animation>().Sample();
        transform.FindChild("DetonatorPlunger").GetComponent<Animation>().Stop();
    }

}
