using UnityEngine;

/// <summary>
/// Basic light that toggles on/off when ball hits it.
/// Inherits from breakable so that it can be used as an objective.
/// </summary>
public class FloorLight_Toggle : Breakable
{
    // Materials for light when turned on/off.
    public Material OnMaterial;
    private Material _offMaterial;

    // Initialization.
    protected override void Start()
    {
        base.Start();

        // Save original material.
        _offMaterial = renderer.material;
    }

    // Turn on light.
    public override void Break()
    {
        renderer.material = OnMaterial;
        IsBroken = true;

        // Play sound.
        Player.use.PlayLightOnSound();

        // Call broken event.
        OnBreak();
    }

    // Turn off light.
    public override void Unbreak()
    {
        renderer.material = _offMaterial;
        IsBroken = false;

        // Play sound.
        Player.use.PlayLightOffSound();

        // Call unbroken event.
        OnUnBreak();
    }

    // Called when light is touched by ball.
    void OnTriggerEnter(Collider other)
    {
        // Only break if the collider is a ball.
        if (other.tag != "Ball")
            return;

        if (!IsBroken)
            Break();
        else
            Unbreak();
    }
}
