using UnityEngine;
using System.Collections;

public class PirateLauncher : MonoBehaviour
{
    // Trigger start/end transform.
    public Transform TriggerStartTransform;
    public Transform TriggerEndTransform;

    // Update is called once per frame
    void Update()
    {
        // Animate the launch trigger.
        transform.rotation = Quaternion.Lerp(TriggerStartTransform.rotation, TriggerEndTransform.rotation, Player.use.BallLaunchForce / Player.use.LaunchMaxForce);
    }
}
