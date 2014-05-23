using UnityEngine;
using System.Collections;

/// <summary>
/// Simple breakable that hides its mesh when hit.
/// </summary>
public class BreakableHide : Breakable
{
    public override void Break()
    {
        renderer.enabled = false;
        collider.enabled = false;

        base.Break();
    }

    public override void Unbreak()
    {
        renderer.enabled = true;
        collider.enabled = true;
        
        base.Unbreak();
    }
}
