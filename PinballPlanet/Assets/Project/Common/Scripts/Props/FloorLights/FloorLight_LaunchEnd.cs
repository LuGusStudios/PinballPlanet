using UnityEngine;

public class FloorLight_LaunchEnd : Triggerable
{
    // The delay after which all launch lights are turned off after ball exits the launch area.
    public float TurnOffLaunchLightsDelay = 1.0f;

    // Called when a trigger is hit.
    protected override void TriggerHit(GameObject trigger, GameObject other)
    {
        // Turn on the end light.
        GetComponent<FloorLight_Link>().Break();

        // Turn off all launch lights after a small delay.
        Invoke("TurnOffLaunchLights", TurnOffLaunchLightsDelay);
    }

    // Turn off all launch lights by turning of the first one.
    private void TurnOffLaunchLights()
    {
        GameObject.Find("FloorLight_Launch_Link1").GetComponent<FloorLight_Link>().Unbreak();
    }
}
