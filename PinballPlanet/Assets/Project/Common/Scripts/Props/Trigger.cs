using UnityEngine;

public delegate void TriggeredEventHandler(GameObject trigger, GameObject other);

/// <summary>
/// Trigger script that can will trigger all Triggerable.cs objects subscribed to its Triggered event.
/// This script should be used for triggers that can't use the objects own collider, because it needs to be a different position/size/shape.
/// </summary>
public class Trigger : MonoBehaviour
{
    // Triggered event.
    public event TriggeredEventHandler Triggered;

    // Called when another object enters trigger.
    void OnTriggerEnter(Collider other)
    {
        if(Triggered != null)
            Triggered(gameObject, other.gameObject);
    }
}
