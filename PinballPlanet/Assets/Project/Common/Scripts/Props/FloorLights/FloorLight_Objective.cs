using UnityEngine;

/// <summary>
/// Light that goes on and off together with another breakable.
/// </summary>
public class FloorLight_Objective : FloorLight_Toggle
{
    // Breakable this light is linked to.
    public Breakable LinkedBreakable;

    protected override void Start()
    {
        LinkedBreakable.Broken += OnLinkBreak;
        LinkedBreakable.UnBroken += OnLinkUnBreak;

        base.Start();
    }

    private void OnLinkBreak(GameObject sender)
    {
        Break();
    }

    private void OnLinkUnBreak(GameObject sender)
    {
        Unbreak();
    }
}
