using UnityEngine;

/// <summary>
/// A floorlight linked to another one.
/// This light can only turn on if previous one is turned on.
/// This light turns off is previous light is off.
/// </summary>
public class FloorLight_Link : FloorLight_Toggle
{
    // Previous light in the link.
    public FloorLight_Link PreviousLight;

    // Initialization.
    protected override void Start()
    {
        // Subscribe to unbroken event (light turned off event).
        if (PreviousLight != null)
            PreviousLight.UnBroken += PrevLightBroken;

        base.Start();
    }

    // Called when the previous light in the chain is turned off.
    private void PrevLightBroken(GameObject sender)
    {
        // Turn off light.
        Unbreak();
    }

    // Turn on light.
    public override void Break()
    {
        // If there is no previous light, this is the start light of the chain and should just turn on.
        if (PreviousLight == null)
        {
            base.Break(); 
            return;
        }

        // Only turn on if the previous light is on.
        if (PreviousLight.IsBroken || PreviousLight == null)
        {
            base.Break();
        }
    }
}
