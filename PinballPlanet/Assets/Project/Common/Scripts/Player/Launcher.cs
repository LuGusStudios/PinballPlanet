using UnityEngine;

/// <summary>
/// Behaviour for when ball touches the launcher.
/// </summary>
public class Launcher : MonoBehaviour
{
    // Called when light is touched by ball.
    void OnCollisionEnter(Collision coll)
    {
        // Only break if the collider is a ball.
        if (coll.gameObject.tag != "Ball")
            return;

        // Reset the start light of the launch light chain.
        GameObject.Find("FloorLight_Launch_Link1").GetComponent<FloorLight_Link>().Unbreak();
    }
}
