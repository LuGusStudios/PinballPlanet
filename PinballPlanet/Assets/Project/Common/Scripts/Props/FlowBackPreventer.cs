using UnityEngine;
using System.Collections;

public class FlowBackPreventer : Triggerable
{
    public float CloseDelay = 0.5f;

	// Use this for initialization
    protected override void Start () 
	{
	    base.Start();
	}
	
    // Used for when the trigger hit event is called.
    protected override void TriggerHit(GameObject trigger, GameObject other)
	{
        if (other.tag != "Ball")
			return;

		StartCoroutine( EnableColliderRoutine(trigger, other) );
    }
	
    // Calls the routine after a couple of seconds.
    public IEnumerator EnableColliderRoutine(GameObject trigger, GameObject other)
	{
		yield return new WaitForSeconds(CloseDelay);

        // Set trigger to block ball.
        if(trigger != null)
            trigger.collider.isTrigger = false;

        // Unhide object.
		renderer.enabled = true;
        if (collider != null) 
            collider.isTrigger = false;

        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SetActive(true);
        //}
	}

    // Reset the flow back preventer so that a new ball can be launched.
    public void Reset()
    {
        // Reset triggers.
        foreach (GameObject trigger in Triggers)
        {
            trigger.collider.isTrigger = true;
        }

        // Hide object.
        renderer.enabled = false;
        if (collider != null) 
            collider.isTrigger = true;

        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SetActive(false);
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ball")
            return;

        if (Triggers.Count > 0)
            return;

        TriggerHit(null, other.gameObject);
    }
}
