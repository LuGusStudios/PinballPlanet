using UnityEngine;

public class MastSupport : Breakable
{
    // Breaks the game object by turning off the renderer and the collision.
    public override void Break()
    {
        //Debug.Log("--- Hiding " + name + ". ---");

        // Hide the mesh.
        renderer.enabled = false;
        collider.enabled = false;

        // Hide all child meshes.
        foreach (Transform child in transform)
        {
            //Debug.Log("--- Hiding " + child.name + ". ---");
            child.renderer.enabled = false;
        }

        // Call inherited break function.
        base.Break();
    }

    // Restores the game object to its unbroken state.
    public override void Unbreak()
    {
        // Unhide the mesh.
        renderer.enabled = true;
        collider.enabled = true;

        // Unhide all child meshes.
        foreach (Transform child in transform)
        {
            child.renderer.enabled = true;
        }

        // Call inherited unbreak function.
        base.Unbreak();
    }
}
