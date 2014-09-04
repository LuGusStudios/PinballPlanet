using UnityEngine;

/// <summary>
/// Notifies the halloween launcher that the ball is touching.
/// </summary>
public class BallTouchNotifier : MonoBehaviour
{
    private HalloweenLauncher _launcher;

    void Start()
    {
        _launcher = GameObject.Find("BallLaunch").GetComponent<HalloweenLauncher>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Ball")
            return;

        _launcher.BallTouched(collision.collider.GetComponent<Ball>());
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag != "Ball")
            return;

        _launcher.BallExit();
    }
}
