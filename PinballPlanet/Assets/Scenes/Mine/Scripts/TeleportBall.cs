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
        Vector3 newPos = Exit.transform.position;
        newPos.z = 5.637837f;
        _ball.transform.position = newPos;
        // Apply force.
        _ball.rigidbody.velocity = Exit.up.normalized * LaunchForce;
    }
}
