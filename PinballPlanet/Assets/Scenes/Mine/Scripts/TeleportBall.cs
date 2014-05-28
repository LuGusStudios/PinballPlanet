using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TeleportBall : MonoBehaviour
{
    // List of exits a new ball can come from.
    public Transform Exit = null; 

    // How hard the new balls launch.
    public float LaunchForce = 500;

    // How long till the ball launches from the exit.
    public float LaunchDelay = 2;

    // Stores the ball when teleporting.
    private GameObject _ball = null;

    void OnTriggerEnter(Collider other)
    {
        // Return if not colliding with ball.
        if (other.tag != "Ball")
            return;

        // Check if there are exits.
        if (Exit == null)
        {
            Debug.LogError("--- Teleport exit not found. ---");
            return;
        }

        // Set ball at exit.
        _ball = other.gameObject;
        _ball.transform.position = Exit.transform.position;
        _ball.rigidbody.isKinematic = true;

        Invoke("LaunchBall", LaunchDelay);
    }

    private void LaunchBall()
    {
        // Apply force.
        _ball.rigidbody.isKinematic = false;
        _ball.rigidbody.velocity = Exit.up * LaunchForce;
    }
}
