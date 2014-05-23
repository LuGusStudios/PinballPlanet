using UnityEngine;

/// <summary>
/// A sword that will break once it's hit by the ball. The mesh is replaced by a broken version.
/// </summary>
public class Sword : Breakable
{
    // Two different versions of the mesh.
    public Mesh PristineMesh;
    public Mesh BrokenMesh;

    // Particles to spawn when breaking.
    public GameObject BreakParticlesPrefab;

    // Breaks the game object and lowers the score.
    public override void Break()
    {
        // Replace mesh with broken version.
        GetComponent<MeshFilter>().mesh = BrokenMesh;

        // Play falling animation.
        transform.parent.GetComponent<Animation>().Play("SwordFalling");

        // Spawn particles.
        Instantiate(BreakParticlesPrefab, transform.position.z(10), Quaternion.identity);

        // Turn off collision.
        collider.enabled = false;

        // Call inherited base function.
        base.Break();
    }

    // Reset.
    public override void Unbreak()
    {
        // Replace mesh with broken version.
        GetComponent<MeshFilter>().mesh = PristineMesh;

        // Reverse sword falling animation.
        transform.parent.GetComponent<Animation>().Play("SwordFalling");
        AnimationState swordFall = transform.parent.animation["SwordFalling"];
        swordFall.time = swordFall.length;
        swordFall.speed = -0.5f;

        // Turn off collision.
        collider.enabled = true;

        // Reset completed after animation is done.
        Invoke("UnbreakBase", swordFall.length * 2);
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
        // Reset animation.
        AnimationState swordFall = transform.parent.animation["SwordFalling"];
        swordFall.time = 0;
        swordFall.speed = 1;

        transform.parent.GetComponent<Animation>().Sample();
        transform.parent.GetComponent<Animation>().Stop();
    }

}
