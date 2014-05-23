using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A triggerable object that can be triggered by an object with the Trigger.cs script by subscribing to the Triggered event.
/// </summary>
public class Triggerable : MonoBehaviour
{
    // All the triggers this object will subscribe to.
    public List<GameObject> Triggers;

    // Use this for initialization
    protected virtual void Start()
    {
        foreach (GameObject trigger in Triggers)
        {
            trigger.GetComponent<Trigger>().Triggered += TriggerHit;
        }
    }

    // Called when a trigger is hit.
    protected virtual void TriggerHit(GameObject trigger, GameObject other)
    {
        Debug.Log("--- Object: " + other.name + " entered Trigger: " + trigger.name + " ---");
    }

    //This function is called when the MonoBehaviour will be destroyed.
    void OnDestroy()
    {
        foreach (GameObject trigger in Triggers )
        {
            if (trigger != null) 
                trigger.GetComponent<Trigger>().Triggered -= TriggerHit;
        }
    }
}
