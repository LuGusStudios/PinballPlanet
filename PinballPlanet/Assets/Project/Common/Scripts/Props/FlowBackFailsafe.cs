using UnityEngine;
using System.Collections;

public class FlowBackFailsafe : Triggerable 
{
	public FlowBackPreventer fbp = null;
	
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
	}
	
	// Used for when the trigger hit event is called.
	// reset flowback preventer if the ball drops back down
	protected override void TriggerHit(GameObject trigger, GameObject other)
	{
		if (other.tag != "Ball")
			return;

		fbp.StopAllCoroutines();
		fbp.Reset();
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

