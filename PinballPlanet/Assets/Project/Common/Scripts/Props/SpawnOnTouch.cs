using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns a game object at a relative position to this object when touched.
/// </summary>
public class SpawnOnTouch : MonoBehaviour
{
    public GameObject ObjectToSpawn = null;
    public Vector3 RelativePos = Vector3.zero;

    // Called when another object enters trigger.
    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Ball")
            return;

        Instantiate(ObjectToSpawn, transform.position + RelativePos, Quaternion.identity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name != "Ball")
            return;

        Instantiate(ObjectToSpawn, transform.position + RelativePos, Quaternion.identity);
    }
} 
