using UnityEngine;
using System.Collections;

public class Gravestone : Breakable
{

    // Breaks the stone by doing an animation.
    public override void Break()
    {
        // Play animation on child lock.
        transform.FindChild("GraveStone").GetComponent<Animation>().Play("GraveStone");

        // Turn off collider.
        collider.enabled = false;

        // Call inherited break function.
        base.Break();
    }

    // Reset.
    public override void Unbreak()
    {
        // Reverse animation.
        transform.FindChild("GraveStone").GetComponent<Animation>().Play("GraveStone");
        AnimationState animState = transform.FindChild("GraveStone").animation["GraveStone"];
        animState.time = animState.length;
        const float animSpeed = -0.5f;
        animState.speed = animSpeed;

        // Reset completed after animation is done.
        Invoke("UnbreakBase", animState.length / animSpeed);
    }

    // Resets inherited base.
    private void UnbreakBase()
    {
        ResetAnimation();

        // Turn on collider.
        collider.enabled = true;

        // Call inherited unbreak function.
        base.Unbreak();
    }

    // Resets the animation.
    private void ResetAnimation()
    {
        // Reset falling animation.
        AnimationState animState = transform.FindChild("GraveStone").animation["GraveStone"];
        animState.time = 0;
        animState.speed = 1;

        transform.FindChild("GraveStone").GetComponent<Animation>().Sample();
        transform.FindChild("GraveStone").GetComponent<Animation>().Stop();
    }
}
