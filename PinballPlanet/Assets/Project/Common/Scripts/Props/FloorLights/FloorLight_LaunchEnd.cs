using UnityEngine;

public class FloorLight_LaunchEnd : Triggerable
{
    // The delay after which all launch lights are turned off after ball exits the launch area.
    public float TurnOffLaunchLightsDelay = 1.0f;

    public int LaunchLightsScore = 150;

    // Called when a trigger is hit.
    protected override void TriggerHit(GameObject trigger, GameObject other)
    {
        if (!GetComponent<FloorLight_Link>().PreviousLight.IsBroken)
            return;

        // Turn on the end light.
        GetComponent<FloorLight_Link>().Break();

        ScoreManager.use.ShowScore(LaunchLightsScore, transform.position.zAdd(Random.Range(10, 20)), 2.0f, null, Color.white);
    }

    // Turn off all launch lights by turning of the first one.
    private void TurnOffLaunchLights()
    {
        GameObject.Find("FloorLight_Launch_Link1").GetComponent<FloorLight_Link>().Unbreak();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ball")
            return;

        if (!GetComponent<FloorLight_Link>().PreviousLight.IsBroken)
            return;

        ScoreManager.use.ShowScore(LaunchLightsScore, transform.position.zAdd(Random.Range(10, 20)), 2.0f, null, Color.white);
    }
}
