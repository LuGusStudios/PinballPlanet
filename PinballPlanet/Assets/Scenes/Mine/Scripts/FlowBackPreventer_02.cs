using UnityEngine;
using System.Collections;

public class FlowBackPreventer_02 : Triggerable
{
    public float CloseDelay = 0.5f;

	// Use this for initialization
    protected override void Start () 
	{
	    base.Start();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
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
        trigger.collider.isTrigger = false;
        // Unhide object.
		renderer.enabled = true;
		
		Invoke("Disable", 2.0f);
		
	}

    // Reset the flow back preventer so that a new ball can be launched.
    public void Reset()
    {
        // Reset triggers.
        foreach (GameObject trigger in Triggers)
        {
            trigger.collider.enabled = true;
			trigger.collider.isTrigger = true;
        }
        // Hide object.
        renderer.enabled = false;
    }
	
	public void Disable()
    {
        // Reset triggers.
        foreach (GameObject trigger in Triggers)
        {
			trigger.collider.enabled = false;
        }
        // Hide object.
        renderer.enabled = false;
    }

}
